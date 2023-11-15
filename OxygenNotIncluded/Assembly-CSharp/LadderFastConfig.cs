using System;
using TUNING;
using UnityEngine;

// Token: 0x020001F1 RID: 497
public class LadderFastConfig : IBuildingConfig
{
	// Token: 0x060009EE RID: 2542 RVA: 0x00039884 File Offset: 0x00037A84
	public override BuildingDef CreateBuildingDef()
	{
		string id = "LadderFast";
		int width = 1;
		int height = 1;
		string anim = "ladder_plastic_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER1;
		string[] plastics = MATERIALS.PLASTICS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, plastics, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		BuildingTemplates.CreateLadderDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Plastic";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x060009EF RID: 2543 RVA: 0x0003990D File Offset: 0x00037B0D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		Ladder ladder = go.AddOrGet<Ladder>();
		ladder.upwardsMovementSpeedMultiplier = 1.2f;
		ladder.downwardsMovementSpeedMultiplier = 1.2f;
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x060009F0 RID: 2544 RVA: 0x00039937 File Offset: 0x00037B37
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000630 RID: 1584
	public const string ID = "LadderFast";
}
