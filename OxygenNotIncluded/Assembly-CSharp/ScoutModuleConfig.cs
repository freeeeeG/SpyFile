using System;
using System.Collections.Generic;
using TUNING;
using UnityEngine;

// Token: 0x02000328 RID: 808
public class ScoutModuleConfig : IBuildingConfig
{
	// Token: 0x0600106F RID: 4207 RVA: 0x00059139 File Offset: 0x00057339
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x00059140 File Offset: 0x00057340
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ScoutModule";
		int width = 3;
		int height = 3;
		string anim = "rocket_scout_cargo_module_kanim";
		int hitpoints = 1000;
		float construction_time = 30f;
		float[] hollow_TIER = BUILDINGS.ROCKETRY_MASS_KG.HOLLOW_TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, hollow_TIER, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.DefaultAnimState = "deployed";
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		return buildingDef;
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x000591DC File Offset: 0x000573DC
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		Storage storage = go.AddComponent<Storage>();
		storage.showInUI = true;
		storage.SetDefaultStoredItemModifiers(Storage.StandardInsulatedStorage);
		BuildingInternalConstructor.Def def = go.AddOrGetDef<BuildingInternalConstructor.Def>();
		def.constructionMass = 500f;
		def.outputIDs = new List<string>
		{
			"ScoutLander",
			"ScoutRover"
		};
		def.spawnIntoStorage = true;
		def.storage = storage;
		def.constructionSymbol = "under_construction";
		go.AddOrGet<BuildingInternalConstructorWorkable>().SetWorkTime(30f);
		JettisonableCargoModule.Def def2 = go.AddOrGetDef<JettisonableCargoModule.Def>();
		def2.landerPrefabID = "ScoutLander".ToTag();
		def2.landerContainer = storage;
		def2.clusterMapFXPrefabID = "DeployingScoutLanderFXConfig";
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 3), GameTags.Rocket, null)
		};
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x000592E4 File Offset: 0x000574E4
	public override void DoPostConfigureComplete(GameObject go)
	{
		Prioritizable.AddRef(go);
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MODERATE, 0f, 0f);
	}

	// Token: 0x04000910 RID: 2320
	public const string ID = "ScoutModule";
}
