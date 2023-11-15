using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x0200005D RID: 93
public class DebrisPayloadConfig : IEntityConfig
{
	// Token: 0x060001AA RID: 426 RVA: 0x0000BF52 File Offset: 0x0000A152
	public string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060001AB RID: 427 RVA: 0x0000BF5C File Offset: 0x0000A15C
	public GameObject CreatePrefab()
	{
		GameObject gameObject = EntityTemplates.CreateLooseEntity("DebrisPayload", ITEMS.DEBRISPAYLOAD.NAME, ITEMS.DEBRISPAYLOAD.DESC, 100f, true, Assets.GetAnim("rocket_debris_combined_kanim"), "object", Grid.SceneLayer.Front, EntityTemplates.CollisionShape.RECTANGLE, 1f, 1f, true, 0, SimHashes.Creature, new List<Tag>
		{
			GameTags.IgnoreMaterialCategory,
			GameTags.Experimental
		});
		RailGunPayload.Def def = gameObject.AddOrGetDef<RailGunPayload.Def>();
		def.attractToBeacons = false;
		def.clusterAnimSymbolSwapTarget = "debris1";
		def.randomClusterSymbolSwaps = new List<string>
		{
			"debris1",
			"debris2",
			"debris3"
		};
		def.worldAnimSymbolSwapTarget = "debris";
		def.randomWorldSymbolSwaps = new List<string>
		{
			"debris",
			"2_debris",
			"3_debris"
		};
		SymbolOverrideControllerUtil.AddToPrefab(gameObject);
		gameObject.AddOrGet<LoopingSounds>();
		Storage storage = BuildingTemplates.CreateDefaultStorage(gameObject, false);
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		storage.allowSettingOnlyFetchMarkedItems = false;
		storage.allowItemRemoval = false;
		storage.capacityKg = 5000f;
		DropAllWorkable dropAllWorkable = gameObject.AddOrGet<DropAllWorkable>();
		dropAllWorkable.dropWorkTime = 30f;
		dropAllWorkable.choreTypeID = Db.Get().ChoreTypes.Fetch.Id;
		dropAllWorkable.ConfigureMultitoolContext("build", EffectConfigs.BuildSplashId);
		ClusterDestinationSelector clusterDestinationSelector = gameObject.AddOrGet<ClusterDestinationSelector>();
		clusterDestinationSelector.assignable = false;
		clusterDestinationSelector.shouldPointTowardsPath = true;
		clusterDestinationSelector.requireAsteroidDestination = true;
		clusterDestinationSelector.canNavigateFogOfWar = true;
		BallisticClusterGridEntity ballisticClusterGridEntity = gameObject.AddOrGet<BallisticClusterGridEntity>();
		ballisticClusterGridEntity.clusterAnimName = "rocket_debris_kanim";
		ballisticClusterGridEntity.isWorldEntity = true;
		ballisticClusterGridEntity.nameKey = new StringKey("STRINGS.ITEMS.DEBRISPAYLOAD.NAME");
		gameObject.AddOrGet<ClusterTraveler>();
		return gameObject;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x0000C11C File Offset: 0x0000A31C
	public void OnPrefabInit(GameObject inst)
	{
	}

	// Token: 0x060001AD RID: 429 RVA: 0x0000C11E File Offset: 0x0000A31E
	public void OnSpawn(GameObject inst)
	{
	}

	// Token: 0x04000103 RID: 259
	public const string ID = "DebrisPayload";

	// Token: 0x04000104 RID: 260
	public const float MASS = 100f;

	// Token: 0x04000105 RID: 261
	public const float MAX_STORAGE_KG_MASS = 5000f;

	// Token: 0x04000106 RID: 262
	public const float STARMAP_SPEED = 10f;
}
