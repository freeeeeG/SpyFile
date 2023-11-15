using System;
using TUNING;
using UnityEngine;

// Token: 0x02000326 RID: 806
public class ScannerModuleConfig : IBuildingConfig
{
	// Token: 0x06001065 RID: 4197 RVA: 0x00058E60 File Offset: 0x00057060
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06001066 RID: 4198 RVA: 0x00058E68 File Offset: 0x00057068
	public override BuildingDef CreateBuildingDef()
	{
		string id = "ScannerModule";
		int width = 5;
		int height = 5;
		string anim = "rocket_scanner_module_kanim";
		int hitpoints = 1000;
		float construction_time = 120f;
		float[] construction_mass = new float[]
		{
			350f,
			1000f
		};
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString(),
			SimHashes.Polypropylene.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		buildingDef.Cancellable = false;
		return buildingDef;
	}

	// Token: 0x06001067 RID: 4199 RVA: 0x00058F44 File Offset: 0x00057144
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGetDef<ScannerModule.Def>().scanRadius = 0;
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
	}

	// Token: 0x06001068 RID: 4200 RVA: 0x00058FB4 File Offset: 0x000571B4
	public override void DoPostConfigureComplete(GameObject go)
	{
		BuildingTemplates.ExtendBuildingToRocketModuleCluster(go, null, ROCKETRY.BURDEN.MINOR_PLUS, 0f, 0f);
	}

	// Token: 0x0400090C RID: 2316
	public const string ID = "ScannerModule";
}
