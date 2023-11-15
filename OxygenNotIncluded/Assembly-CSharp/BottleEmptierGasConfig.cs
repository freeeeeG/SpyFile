using System;
using TUNING;
using UnityEngine;

// Token: 0x0200002A RID: 42
public class BottleEmptierGasConfig : IBuildingConfig
{
	// Token: 0x060000B6 RID: 182 RVA: 0x00006410 File Offset: 0x00004610
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BottleEmptierGas";
		int width = 1;
		int height = 3;
		string anim = "gas_emptying_station_kanim";
		int hitpoints = 30;
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00006478 File Offset: 0x00004678
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Prioritizable.AddRef(go);
		Storage storage = go.AddOrGet<Storage>();
		storage.storageFilters = STORAGEFILTERS.GASES;
		storage.showInUI = true;
		storage.showDescriptor = true;
		storage.capacityKg = 200f;
		go.AddOrGet<TreeFilterable>();
		BottleEmptier bottleEmptier = go.AddOrGet<BottleEmptier>();
		bottleEmptier.isGasEmptier = true;
		bottleEmptier.emptyRate = 0.25f;
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x000064D2 File Offset: 0x000046D2
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000078 RID: 120
	public const string ID = "BottleEmptierGas";
}
