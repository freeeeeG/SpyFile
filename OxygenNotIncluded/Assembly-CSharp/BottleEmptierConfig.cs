using System;
using TUNING;
using UnityEngine;

// Token: 0x02000029 RID: 41
public class BottleEmptierConfig : IBuildingConfig
{
	// Token: 0x060000B2 RID: 178 RVA: 0x00006360 File Offset: 0x00004560
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BottleEmptier";
		int width = 1;
		int height = 3;
		string anim = "liquidator_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x000063C6 File Offset: 0x000045C6
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.storageFilters = STORAGEFILTERS.LIQUIDS;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.capacityKg = 200f;
		go.AddOrGet<TreeFilterable>();
		go.AddOrGet<BottleEmptier>();
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x00006405 File Offset: 0x00004605
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000077 RID: 119
	public const string ID = "BottleEmptier";
}
