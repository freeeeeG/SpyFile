﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000062 RID: 98
public class DevLightGeneratorConfig : IBuildingConfig
{
	// Token: 0x060001C1 RID: 449 RVA: 0x0000C680 File Offset: 0x0000A880
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevLightGenerator";
		int width = 1;
		int height = 1;
		string anim = "dev_generator_kanim";
		int hitpoints = 100;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.RequiresPowerInput = false;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		buildingDef.Floodable = false;
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x060001C2 RID: 450 RVA: 0x0000C6FC File Offset: 0x0000A8FC
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 1800;
		lightShapePreview.radius = 8f;
		lightShapePreview.shape = global::LightShape.Circle;
	}

	// Token: 0x060001C3 RID: 451 RVA: 0x0000C720 File Offset: 0x0000A920
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		go.AddTag(RoomConstraints.ConstraintTags.LightSource);
	}

	// Token: 0x060001C4 RID: 452 RVA: 0x0000C738 File Offset: 0x0000A938
	public override void DoPostConfigureComplete(GameObject go)
	{
		DevLightGenerator devLightGenerator = go.AddOrGet<DevLightGenerator>();
		devLightGenerator.overlayColour = LIGHT2D.CEILINGLIGHT_OVERLAYCOLOR;
		devLightGenerator.Color = LIGHT2D.CEILINGLIGHT_COLOR;
		devLightGenerator.Range = 8f;
		devLightGenerator.Angle = 2.6f;
		devLightGenerator.Direction = LIGHT2D.CEILINGLIGHT_DIRECTION;
		devLightGenerator.Offset = new Vector2(0f, 0.5f);
		devLightGenerator.shape = global::LightShape.Circle;
		devLightGenerator.drawOverlay = true;
		devLightGenerator.Lux = 1800;
		go.AddOrGetDef<LightController.Def>();
	}

	// Token: 0x04000116 RID: 278
	public const string ID = "DevLightGenerator";
}
