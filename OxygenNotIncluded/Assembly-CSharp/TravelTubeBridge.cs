using System;
using UnityEngine;

// Token: 0x020006AD RID: 1709
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/TravelTubeBridge")]
public class TravelTubeBridge : KMonoBehaviour, ITravelTubePiece
{
	// Token: 0x1700032A RID: 810
	// (get) Token: 0x06002E49 RID: 11849 RVA: 0x000F48AA File Offset: 0x000F2AAA
	public Vector3 Position
	{
		get
		{
			return base.transform.GetPosition();
		}
	}

	// Token: 0x06002E4A RID: 11850 RVA: 0x000F48B8 File Offset: 0x000F2AB8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Grid.HasTube[Grid.PosToCell(this)] = true;
		Components.ITravelTubePieces.Add(this);
		base.Subscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate);
		base.Subscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate);
	}

	// Token: 0x06002E4B RID: 11851 RVA: 0x000F490C File Offset: 0x000F2B0C
	protected override void OnCleanUp()
	{
		base.Unsubscribe<TravelTubeBridge>(774203113, TravelTubeBridge.OnBuildingBrokenDelegate, false);
		base.Unsubscribe<TravelTubeBridge>(-1735440190, TravelTubeBridge.OnBuildingFullyRepairedDelegate, false);
		Grid.HasTube[Grid.PosToCell(this)] = false;
		Components.ITravelTubePieces.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002E4C RID: 11852 RVA: 0x000F495D File Offset: 0x000F2B5D
	private void OnBuildingBroken(object data)
	{
	}

	// Token: 0x06002E4D RID: 11853 RVA: 0x000F495F File Offset: 0x000F2B5F
	private void OnBuildingFullyRepaired(object data)
	{
	}

	// Token: 0x04001B46 RID: 6982
	private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingBrokenDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>(delegate(TravelTubeBridge component, object data)
	{
		component.OnBuildingBroken(data);
	});

	// Token: 0x04001B47 RID: 6983
	private static readonly EventSystem.IntraObjectHandler<TravelTubeBridge> OnBuildingFullyRepairedDelegate = new EventSystem.IntraObjectHandler<TravelTubeBridge>(delegate(TravelTubeBridge component, object data)
	{
		component.OnBuildingFullyRepaired(data);
	});
}
