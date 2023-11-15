using System;
using UnityEngine;

// Token: 0x02000264 RID: 612
public class ClustercraftConfig : IEntityConfig
{
	// Token: 0x06000C58 RID: 3160 RVA: 0x00045D05 File Offset: 0x00043F05
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000C59 RID: 3161 RVA: 0x00045D0C File Offset: 0x00043F0C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateEntity("Clustercraft", "Clustercraft", true);
		SaveLoadRoot saveLoadRoot = gameObject.AddOrGet<SaveLoadRoot>();
		saveLoadRoot.DeclareOptionalComponent<WorldInventory>();
		saveLoadRoot.DeclareOptionalComponent<WorldContainer>();
		saveLoadRoot.DeclareOptionalComponent<OrbitalMechanics>();
		gameObject.AddOrGet<Clustercraft>();
		gameObject.AddOrGet<CraftModuleInterface>();
		gameObject.AddOrGet<UserNameable>();
		RocketClusterDestinationSelector rocketClusterDestinationSelector = gameObject.AddOrGet<RocketClusterDestinationSelector>();
		rocketClusterDestinationSelector.requireLaunchPadOnAsteroidDestination = true;
		rocketClusterDestinationSelector.assignable = true;
		rocketClusterDestinationSelector.shouldPointTowardsPath = true;
		gameObject.AddOrGet<ClusterTraveler>().stopAndNotifyWhenPathChanges = true;
		gameObject.AddOrGetDef<AlertStateManager.Def>();
		gameObject.AddOrGet<Notifier>();
		gameObject.AddOrGetDef<RocketSelfDestructMonitor.Def>();
		return gameObject;
	}

	// Token: 0x06000C5A RID: 3162 RVA: 0x00045D90 File Offset: 0x00043F90
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000C5B RID: 3163 RVA: 0x00045D92 File Offset: 0x00043F92
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000751 RID: 1873
	public const string ID = "Clustercraft";
}
