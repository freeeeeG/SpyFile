using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000307 RID: 775
public class RailGunPayloadConfig : IEntityConfig
{
	// Token: 0x06000FB0 RID: 4016 RVA: 0x000547D8 File Offset: 0x000529D8
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000FB1 RID: 4017 RVA: 0x000547E0 File Offset: 0x000529E0
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("RailGunPayload", ITEMS.RAILGUNPAYLOAD.NAME, ITEMS.RAILGUNPAYLOAD.DESC, 200f, true, Assets.GetAnim("railgun_capsule_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 0.75f, 1f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IgnoreMaterialCategory,
			GameTags.Experimental
		});
		gameObject.AddOrGetDef<RailGunPayload.Def>().attractToBeacons = true;
		gameObject.AddComponent<LoopingSounds>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(gameObject, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.allowItemRemoval = false;
		storage.capacityKg = 200f;
		DropAllWorkable dropAllWorkable = gameObject.AddOrGet<DropAllWorkable>();
		dropAllWorkable.dropWorkTime = 30f;
		dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
		dropAllWorkable.ConfigureMultitoolContext("build", EffectConfigs.BuildSplashId);
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		BallisticClusterGridEntity ballisticClusterGridEntity = gameObject.AddOrGet<BallisticClusterGridEntity>();
		ballisticClusterGridEntity.clusterAnimName = "payload01_kanim";
		ballisticClusterGridEntity.isWorldEntity = true;
		ballisticClusterGridEntity.nameKey = new StringKey("STRINGS.ITEMS.RAILGUNPAYLOAD.NAME");
		gameObject.AddOrGet<ClusterTraveler>();
		return gameObject;
	}

	// Token: 0x06000FB2 RID: 4018 RVA: 0x00054924 File Offset: 0x00052B24
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x06000FB3 RID: 4019 RVA: 0x00054926 File Offset: 0x00052B26
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x040008A0 RID: 2208
	public const string ID = "RailGunPayload";

	// Token: 0x040008A1 RID: 2209
	public const float MASS = 200f;

	// Token: 0x040008A2 RID: 2210
	public const int LANDING_EDGE_PADDING = 3;
}
