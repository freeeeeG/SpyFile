using System;
using TUNING;
using UnityEngine;

// Token: 0x02000066 RID: 102
public class DevRadiationGeneratorConfig : IBuildingConfig
{
	// Token: 0x060001D2 RID: 466 RVA: 0x0000CBEC File Offset: 0x0000ADEC
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevRadiationGenerator";
		int width = 1;
		int height = 1;
		string anim = "dev_generator_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x060001D3 RID: 467 RVA: 0x0000CC68 File Offset: 0x0000AE68
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		RadiationEmitter radiationEmitter = go.AddOrGet<RadiationEmitter>();
		radiationEmitter.emitType = RadiationEmitter.RadiationEmitterType.Constant;
		radiationEmitter.radiusProportionalToRads = false;
		radiationEmitter.emitRadiusX = 12;
		radiationEmitter.emitRadiusY = 12;
		radiationEmitter.emitRads = 2400f / ((float)radiationEmitter.emitRadiusX / 6f);
		go.AddOrGet<DevRadiationEmitter>();
	}

	// Token: 0x060001D4 RID: 468 RVA: 0x0000CCC5 File Offset: 0x0000AEC5
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000120 RID: 288
	public const string ID = "DevRadiationGenerator";
}
