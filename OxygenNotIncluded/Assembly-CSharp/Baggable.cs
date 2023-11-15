using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200059D RID: 1437
[AddComponentMenu("KMonoBehaviour/scripts/Baggable")]
public class Baggable : KMonoBehaviour
{
	// Token: 0x06002308 RID: 8968 RVA: 0x000BFE30 File Offset: 0x000BE030
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.minionAnimOverride = Assets.GetAnim("anim_restrain_creature_kanim");
		Pickupable pickupable = base.gameObject.AddOrGet<Pickupable>();
		pickupable.workAnims = new HashedString[]
		{
			new HashedString("capture"),
			new HashedString("pickup")
		};
		pickupable.workAnimPlayMode = KAnim.PlayMode.Once;
		pickupable.workingPstComplete = null;
		pickupable.workingPstFailed = null;
		pickupable.overrideAnims = new KAnimFile[]
		{
			this.minionAnimOverride
		};
		pickupable.trackOnPickup = false;
		pickupable.useGunforPickup = this.useGunForPickup;
		pickupable.synchronizeAnims = false;
		pickupable.SetWorkTime(3f);
		if (this.mustStandOntopOfTrapForPickup)
		{
			pickupable.SetOffsets(new CellOffset[]
			{
				default(CellOffset),
				new CellOffset(0, -1)
			});
		}
		base.Subscribe<Baggable>(856640610, Baggable.OnStoreDelegate);
		if (base.transform.parent != null)
		{
			if (base.transform.parent.GetComponent<Trap>() != null || base.transform.parent.GetSMI<ReusableTrap.Instance>() != null)
			{
				base.GetComponent<KBatchedAnimController>().enabled = true;
			}
			if (base.transform.parent.GetComponent<EggIncubator>() != null)
			{
				this.wrangled = true;
			}
		}
		if (this.wrangled)
		{
			this.SetWrangled();
		}
	}

	// Token: 0x06002309 RID: 8969 RVA: 0x000BFF8C File Offset: 0x000BE18C
	private void OnStore(object data)
	{
		Storage storage = data as Storage;
		if (storage != null || (data != null && (bool)data))
		{
			base.gameObject.AddTag(GameTags.Creatures.Bagged);
			if (storage && storage.IsPrefabID(GameTags.Minion))
			{
				this.SetVisible(false);
				return;
			}
		}
		else
		{
			if (!this.keepWrangledNextTimeRemovedFromStorage)
			{
				this.Free();
			}
			this.keepWrangledNextTimeRemovedFromStorage = false;
		}
	}

	// Token: 0x0600230A RID: 8970 RVA: 0x000BFFFC File Offset: 0x000BE1FC
	private void SetVisible(bool visible)
	{
		KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
		if (component != null && component.enabled != visible)
		{
			component.enabled = visible;
		}
		KSelectable component2 = base.gameObject.GetComponent<KSelectable>();
		if (component2 != null && component2.enabled != visible)
		{
			component2.enabled = visible;
		}
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x000C0054 File Offset: 0x000BE254
	public static string GetBaggedAnimName(GameObject baggableObject)
	{
		string result = "trussed";
		Pickupable pickupable = baggableObject.AddOrGet<Pickupable>();
		if (pickupable != null && pickupable.storage != null)
		{
			IBaggedStateAnimationInstructions component = pickupable.storage.GetComponent<IBaggedStateAnimationInstructions>();
			if (component != null)
			{
				string baggedAnimationName = component.GetBaggedAnimationName();
				if (baggedAnimationName != null)
				{
					result = baggedAnimationName;
				}
			}
		}
		return result;
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x000C00A4 File Offset: 0x000BE2A4
	public void SetWrangled()
	{
		this.wrangled = true;
		Navigator component = base.GetComponent<Navigator>();
		if (component && component.IsValidNavType(NavType.Floor))
		{
			component.SetCurrentNavType(NavType.Floor);
		}
		base.gameObject.AddTag(GameTags.Creatures.Bagged);
		base.GetComponent<KAnimControllerBase>().Play(Baggable.GetBaggedAnimName(base.gameObject), KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x000C010D File Offset: 0x000BE30D
	public void Free()
	{
		base.gameObject.RemoveTag(GameTags.Creatures.Bagged);
		this.wrangled = false;
		this.SetVisible(true);
	}

	// Token: 0x040013FF RID: 5119
	[SerializeField]
	private KAnimFile minionAnimOverride;

	// Token: 0x04001400 RID: 5120
	public bool mustStandOntopOfTrapForPickup;

	// Token: 0x04001401 RID: 5121
	[Serialize]
	public bool wrangled;

	// Token: 0x04001402 RID: 5122
	[Serialize]
	public bool keepWrangledNextTimeRemovedFromStorage;

	// Token: 0x04001403 RID: 5123
	public bool useGunForPickup;

	// Token: 0x04001404 RID: 5124
	private static readonly EventSystem.IntraObjectHandler<Baggable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Baggable>(delegate(Baggable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x04001405 RID: 5125
	public const string DEFAULT_BAGGED_ANIM_NAME = "trussed";
}
