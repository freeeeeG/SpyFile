using System;
using TUNING;
using UnityEngine;

// Token: 0x020001F0 RID: 496
public class LadderConfig : IBuildingConfig
{
	// Token: 0x060009EA RID: 2538 RVA: 0x000397C4 File Offset: 0x000379C4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Ladder";
		int width = 1;
		int height = 1;
		string anim = "ladder_kanim";
		int hitpoints = 10;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER2;
		string[] all_MINERALS = MATERIALS.ALL_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		BuildingTemplates.CreateLadderDef(buildingDef);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.DragBuild = true;
		return buildingDef;
	}

	// Token: 0x060009EB RID: 2539 RVA: 0x0003984D File Offset: 0x00037A4D
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		Ladder ladder = go.AddOrGet<Ladder>();
		ladder.upwardsMovementSpeedMultiplier = 1f;
		ladder.downwardsMovementSpeedMultiplier = 1f;
		go.AddOrGet<AnimTileable>();
	}

	// Token: 0x060009EC RID: 2540 RVA: 0x00039877 File Offset: 0x00037A77
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400062F RID: 1583
	public const string ID = "Ladder";
}
