using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000A0A RID: 2570
[AddComponentMenu("KMonoBehaviour/scripts/Trappable")]
public class Trappable : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06004CE2 RID: 19682 RVA: 0x001AF348 File Offset: 0x001AD548
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
		this.OnCellChange();
	}

	// Token: 0x06004CE3 RID: 19683 RVA: 0x001AF35C File Offset: 0x001AD55C
	protected override void OnCleanUp()
	{
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x06004CE4 RID: 19684 RVA: 0x001AF36C File Offset: 0x001AD56C
	private void OnCellChange()
	{
		int cell = Grid.PosToCell(this);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.trapsLayer, this);
	}

	// Token: 0x06004CE5 RID: 19685 RVA: 0x001AF396 File Offset: 0x001AD596
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.Register();
	}

	// Token: 0x06004CE6 RID: 19686 RVA: 0x001AF3A4 File Offset: 0x001AD5A4
	protected override void OnCmpDisable()
	{
		this.Unregister();
		base.OnCmpDisable();
	}

	// Token: 0x06004CE7 RID: 19687 RVA: 0x001AF3B4 File Offset: 0x001AD5B4
	private void Register()
	{
		if (this.registered)
		{
			return;
		}
		base.Subscribe<Trappable>(856640610, Trappable.OnStoreDelegate);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "Trappable.Register");
		this.registered = true;
	}

	// Token: 0x06004CE8 RID: 19688 RVA: 0x001AF404 File Offset: 0x001AD604
	private void Unregister()
	{
		if (!this.registered)
		{
			return;
		}
		base.Unsubscribe<Trappable>(856640610, Trappable.OnStoreDelegate, false);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		this.registered = false;
	}

	// Token: 0x06004CE9 RID: 19689 RVA: 0x001AF443 File Offset: 0x001AD643
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_TRAP, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_TRAP, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x06004CEA RID: 19690 RVA: 0x001AF46C File Offset: 0x001AD66C
	public void OnStore(object data)
	{
		Storage storage = data as Storage;
		if (storage && (storage.GetComponent<Trap>() != null || storage.GetSMI<ReusableTrap.Instance>() != null))
		{
			base.gameObject.AddTag(GameTags.Trapped);
			return;
		}
		base.gameObject.RemoveTag(GameTags.Trapped);
	}

	// Token: 0x0400322E RID: 12846
	private bool registered;

	// Token: 0x0400322F RID: 12847
	private static readonly EventSystem.IntraObjectHandler<Trappable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Trappable>(delegate(Trappable component, object data)
	{
		component.OnStore(data);
	});
}
