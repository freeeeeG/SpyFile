using System;
using TUNING;
using UnityEngine;

// Token: 0x02000164 RID: 356
public class FlowerVaseHangingConfig : IBuildingConfig
{
	// Token: 0x060006F5 RID: 1781 RVA: 0x0002D0C8 File Offset: 0x0002B2C8
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlowerVaseHanging";
		int width = 1;
		int height = 2;
		string anim = "flowervase_hanging_basic_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.GenerateOffsets(1, 1);
		return buildingDef;
	}

	// Token: 0x060006F6 RID: 1782 RVA: 0x0002D148 File Offset: 0x0002B348
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.AddDepositTag(GameTags.DecorSeed);
		plantablePlot.occupyingObjectVisualOffset = new Vector3(0f, -0.25f, 0f);
		go.AddOrGet<FlowerVase>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060006F7 RID: 1783 RVA: 0x0002D1A4 File Offset: 0x0002B3A4
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040004DD RID: 1245
	public const string ID = "FlowerVaseHanging";
}
