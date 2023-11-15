using System;
using TUNING;
using UnityEngine;

// Token: 0x02000166 RID: 358
public class FlowerVaseWallConfig : IBuildingConfig
{
	// Token: 0x060006FD RID: 1789 RVA: 0x0002D2D4 File Offset: 0x0002B4D4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "FlowerVaseWall";
		int width = 1;
		int height = 1;
		string anim = "flowervase_wall_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] raw_METALS = MATERIALS.RAW_METALS;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnWall;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Decor.ID;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		return buildingDef;
	}

	// Token: 0x060006FE RID: 1790 RVA: 0x0002D350 File Offset: 0x0002B550
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<Storage>();
		Prioritizable.AddRef(go);
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.AddDepositTag(GameTags.DecorSeed);
		plantablePlot.occupyingObjectVisualOffset = new Vector3(0f, -0.25f, 0f);
		go.GetComponent<KPrefabID>().AddTag(GameTags.Decoration, false);
	}

	// Token: 0x060006FF RID: 1791 RVA: 0x0002D3A5 File Offset: 0x0002B5A5
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040004DF RID: 1247
	public const string ID = "FlowerVaseWall";
}
