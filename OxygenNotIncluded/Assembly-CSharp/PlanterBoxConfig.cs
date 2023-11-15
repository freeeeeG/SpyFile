using System;
using TUNING;
using UnityEngine;

// Token: 0x020002D1 RID: 721
public class PlanterBoxConfig : IBuildingConfig
{
	// Token: 0x06000EA1 RID: 3745 RVA: 0x00050A08 File Offset: 0x0004EC08
	public override BuildingDef CreateBuildingDef()
	{
		string id = "PlanterBox";
		int width = 1;
		int height = 1;
		string anim = "planterbox_kanim";
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] farmable = MATERIALS.FARMABLE;
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, farmable, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.ForegroundLayer = Grid.SceneLayer.BuildingBack;
		buildingDef.Overheatable = false;
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Glass";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x06000EA2 RID: 3746 RVA: 0x00050A7C File Offset: 0x0004EC7C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		Storage storage = go.AddOrGet<Storage>();
		PlantablePlot plantablePlot = go.AddOrGet<PlantablePlot>();
		plantablePlot.AddDepositTag(GameTags.CropSeed);
		plantablePlot.SetFertilizationFlags(true, false);
		go.AddOrGet<CopyBuildingSettings>().copyGroupTag = GameTags.Farm;
		BuildingTemplates.CreateDefaultStorage(go, false);
		storage.SetDefaultStoredItemModifiers(Storage.StandardSealedStorage);
		go.AddOrGet<DropAllWorkable>();
		go.AddOrGet<PlanterBox>();
		go.AddOrGet<AnimTileable>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x06000EA3 RID: 3747 RVA: 0x00050AE4 File Offset: 0x0004ECE4
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000866 RID: 2150
	public const string ID = "PlanterBox";
}
