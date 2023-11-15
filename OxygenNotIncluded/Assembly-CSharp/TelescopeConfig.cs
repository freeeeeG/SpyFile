using System;
using TUNING;
using UnityEngine;

// Token: 0x02000364 RID: 868
public class TelescopeConfig : IBuildingConfig
{
	// Token: 0x060011BC RID: 4540 RVA: 0x00060247 File Offset: 0x0005E447
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_VANILLA_ONLY;
	}

	// Token: 0x060011BD RID: 4541 RVA: 0x00060250 File Offset: 0x0005E450
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Telescope";
		int width = 4;
		int height = 6;
		string anim = "telescope_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 0.125f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.InputConduitType = ConduitType.Gas;
		buildingDef.UtilityInputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x060011BE RID: 4542 RVA: 0x000602F4 File Offset: 0x0005E4F4
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.ScienceBuilding, false);
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		Telescope telescope = go.AddOrGet<Telescope>();
		telescope.clearScanCellRadius = 4;
		telescope.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telescope_kanim")
		};
		telescope.requiredSkillPerk = Db.Get().SkillPerks.CanStudyWorldObjects.Id;
		telescope.workLayer = Grid.SceneLayer.BuildingFront;
		Storage storage = go.AddOrGet<Storage>();
		storage.capacityKg = 1000f;
		storage.showInUI = true;
		ConduitConsumer conduitConsumer = go.AddOrGet<ConduitConsumer>();
		conduitConsumer.conduitType = ConduitType.Gas;
		conduitConsumer.consumptionRate = 1f;
		conduitConsumer.capacityTag = ElementLoader.FindElementByHash(SimHashes.Oxygen).tag;
		conduitConsumer.wrongElementResult = ConduitConsumer.WrongElementResult.Dump;
		conduitConsumer.capacityKG = 10f;
		conduitConsumer.forceAlwaysSatisfied = true;
		go.AddOrGetDef<PoweredController.Def>();
	}

	// Token: 0x060011BF RID: 4543 RVA: 0x000603D5 File Offset: 0x0005E5D5
	public override void DoPostConfigureComplete(GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x060011C0 RID: 4544 RVA: 0x000603DD File Offset: 0x0005E5DD
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x060011C1 RID: 4545 RVA: 0x000603E5 File Offset: 0x0005E5E5
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		TelescopeConfig.AddVisualizer(go);
	}

	// Token: 0x060011C2 RID: 4546 RVA: 0x000603ED File Offset: 0x0005E5ED
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.OriginOffset.y = 3;
		skyVisibilityVisualizer.TwoWideOrgin = true;
		skyVisibilityVisualizer.RangeMin = -4;
		skyVisibilityVisualizer.RangeMax = 5;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x040009A6 RID: 2470
	public const string ID = "Telescope";

	// Token: 0x040009A7 RID: 2471
	public const float POINTS_PER_DAY = 2f;

	// Token: 0x040009A8 RID: 2472
	public const float MASS_PER_POINT = 2f;

	// Token: 0x040009A9 RID: 2473
	public const float CAPACITY = 30f;

	// Token: 0x040009AA RID: 2474
	public const int SCAN_RADIUS = 4;

	// Token: 0x040009AB RID: 2475
	public const int VERTICAL_SCAN_OFFSET = 3;

	// Token: 0x040009AC RID: 2476
	public static readonly SkyVisibilityInfo SKY_VISIBILITY_INFO = new SkyVisibilityInfo(new CellOffset(0, 3), 4, new CellOffset(1, 3), 4, 0);

	// Token: 0x040009AD RID: 2477
	public static readonly Tag INPUT_MATERIAL = GameTags.Glass;
}
