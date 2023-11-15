﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x0200001C RID: 28
public class AstronautTrainingCenterConfig : IBuildingConfig
{
	// Token: 0x06000077 RID: 119 RVA: 0x000050B4 File Offset: 0x000032B4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "AstronautTrainingCenter";
		int width = 5;
		int height = 5;
		string anim = "centrifuge_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.NONE, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 0.5f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.PowerInputOffset = new CellOffset(-2, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "large";
		buildingDef.Deprecated = true;
		return buildingDef;
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00005158 File Offset: 0x00003358
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<BuildingComplete>().isManuallyOperated = true;
		Prioritizable.AddRef(go);
		AstronautTrainingCenter astronautTrainingCenter = go.AddOrGet<AstronautTrainingCenter>();
		astronautTrainingCenter.workTime = float.PositiveInfinity;
		astronautTrainingCenter.requiredSkillPerk = Db.Get().SkillPerks.CanTrainToBeAstronaut.Id;
		astronautTrainingCenter.daysToMasterRole = 10f;
		astronautTrainingCenter.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_centrifuge_kanim")
		};
		astronautTrainingCenter.workLayer = Grid.SceneLayer.BuildingFront;
	}

	// Token: 0x06000079 RID: 121 RVA: 0x000051D4 File Offset: 0x000033D4
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x0400005B RID: 91
	public const string ID = "AstronautTrainingCenter";
}
