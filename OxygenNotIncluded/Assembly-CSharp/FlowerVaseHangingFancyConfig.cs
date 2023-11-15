using System;
using TUNING;
using UnityEngine;

// Token: 0x02000165 RID: 357
public class FlowerVaseHangingFancyConfig : IBuildingConfig
{
	// Token: 0x060006F9 RID: 1785 RVA: 0x0002D1B0 File Offset: 0x0002B3B0
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlowerVaseHangingFancy";
		int width = 1;
		int height = 2;
		string anim = "flowervase_hanging_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] transparents = MATERIALS.TRANSPARENTS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnCeiling;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, transparents, melting_point, build_location_rule, new EffectorValues
		{
			amount = BUILDINGS.DECOR.BONUS.TIER1.amount,
			radius = BUILDINGS.DECOR.BONUS.TIER3.radius
		}, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.SceneLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingUse;
		buildingDef.GenerateOffsets(1, 1);
		return buildingDef;
	}

	// Token: 0x060006FA RID: 1786 RVA: 0x0002D264 File Offset: 0x0002B464
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.AddDepositTag(GameTags.DecorSeed);
		plantablePlot.plantLayer = Grid.SceneLayer.BuildingUse;
		plantablePlot.occupyingObjectVisualOffset = new Vector3(0f, -0.45f, 0f);
		go.AddOrGet<FlowerVase>();
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060006FB RID: 1787 RVA: 0x0002D2C8 File Offset: 0x0002B4C8
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040004DE RID: 1246
	public const string ID = "FlowerVaseHangingFancy";
}
