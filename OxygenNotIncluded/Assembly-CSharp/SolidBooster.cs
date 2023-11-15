using System;
using UnityEngine;

// Token: 0x020009B9 RID: 2489
public class SolidBooster : RocketEngine
{
	// Token: 0x06004A21 RID: 18977 RVA: 0x001A199F File Offset: 0x0019FB9F
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.Subscribe<SolidBooster>(-887025858, SolidBooster.OnRocketLandedDelegate);
	}

	// Token: 0x06004A22 RID: 18978 RVA: 0x001A19B8 File Offset: 0x0019FBB8
	[ContextMenu("Fill Tank")]
	public void FillTank()
	{
		Element element = ElementLoader.GetElement(this.fuelTag);
		GameObject go = element.substance.SpawnResource(base.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element.defaultValues.temperature, byte.MaxValue, 0, false, false, false);
		this.fuelStorage.Store(go, false, false, true, false);
		element = ElementLoader.GetElement(GameTags.OxyRock);
		go = element.substance.SpawnResource(base.gameObject.transform.GetPosition(), this.fuelStorage.capacityKg / 2f, element.defaultValues.temperature, byte.MaxValue, 0, false, false, false);
		this.fuelStorage.Store(go, false, false, true, false);
	}

	// Token: 0x06004A23 RID: 18979 RVA: 0x001A1A80 File Offset: 0x0019FC80
	private void OnRocketLanded(object data)
	{
		if (this.fuelStorage != null && this.fuelStorage.items != null)
		{
			for (int i = this.fuelStorage.items.Count - 1; i >= 0; i--)
			{
				Util.KDestroyGameObject(this.fuelStorage.items[i]);
			}
			this.fuelStorage.items.Clear();
		}
	}

	// Token: 0x040030CA RID: 12490
	public Storage fuelStorage;

	// Token: 0x040030CB RID: 12491
	private static readonly EventSystem.IntraObjectHandler<SolidBooster> OnRocketLandedDelegate = new EventSystem.IntraObjectHandler<SolidBooster>(delegate(SolidBooster component, object data)
	{
		component.OnRocketLanded(data);
	});
}
