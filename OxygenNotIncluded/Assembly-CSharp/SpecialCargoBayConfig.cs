using System;
using TUNING;
using UnityEngine;

// Token: 0x0200034B RID: 843
public class SpecialCargoBayConfig : IBuildingConfig
{
	// Token: 0x06001134 RID: 4404 RVA: 0x0005CBE0 File Offset: 0x0005ADE0
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_VANILLA_ONLY;
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x0005CBE8 File Offset: 0x0005ADE8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpecialCargoBay";
		int width = 5;
		int height = 5;
		string anim = "rocket_storage_live_kanim";
		int hitpoints = 1000;
		float construction_time = 480f;
		float[] cargo_MASS = BUILDINGS.ROCKETRY_MASS_KG.CARGO_MASS;
		string[] construction_materials = new string[]
		{
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, cargo_MASS, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier, 0.2f);
		BuildingTemplates.CreateRocketBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingFront;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = GameTags.Rocket;
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.RequiresPowerInput = false;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.CanMove = true;
		return buildingDef;
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x0005CC98 File Offset: 0x0005AE98
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), GameTags.Rocket, null)
		};
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x0005CCFC File Offset: 0x0005AEFC
	public override void DoPostConfigureComplete(GameObject go)
	{
		CargoBay cargoBay = go.AddOrGet<CargoBay>();
		cargoBay.storage = go.AddOrGet<Storage>();
		cargoBay.storageType = CargoBay.CargoType.Entities;
		cargoBay.storage.capacityKg = 100f;
		BuildingTemplates.ExtendBuildingToRocketModule(go, "rocket_storage_live_bg_kanim", false);
	}

	// Token: 0x0400095D RID: 2397
	public const string ID = "SpecialCargoBay";
}
