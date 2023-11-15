using System;
using TUNING;
using UnityEngine;

// Token: 0x0200035E RID: 862
public class SunLampConfig : IBuildingConfig
{
	// Token: 0x060011A1 RID: 4513 RVA: 0x0005F464 File Offset: 0x0005D664
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SunLamp";
		int width = 2;
		int height = 4;
		string anim = "sun_lamp_kanim";
		int hitpoints = 10;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			200f,
			50f
		};
		string[] construction_materials = new string[]
		{
			"RefinedMetal",
			"Glass"
		};
		float melting_point = 800f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER3, none, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 960f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.ViewMode = OverlayModes.Light.ID;
		buildingDef.AudioCategory = "Metal";
		return buildingDef;
	}

	// Token: 0x060011A2 RID: 4514 RVA: 0x0005F50C File Offset: 0x0005D70C
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LightShapePreview lightShapePreview = go.AddComponent<LightShapePreview>();
		lightShapePreview.lux = 40000;
		lightShapePreview.radius = 16f;
		lightShapePreview.shape = global::LightShape.Cone;
		lightShapePreview.offset = new CellOffset((int)LIGHT2D.SUNLAMP_OFFSET.x, (int)LIGHT2D.SUNLAMP_OFFSET.y);
	}

	// Token: 0x060011A3 RID: 4515 RVA: 0x0005F55C File Offset: 0x0005D75C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.LightSource, false);
	}

	// Token: 0x060011A4 RID: 4516 RVA: 0x0005F570 File Offset: 0x0005D770
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<EnergyConsumer>();
		go.AddOrGet<LoopingSounds>();
		Light2D light2D = go.AddOrGet<Light2D>();
		light2D.Lux = 40000;
		light2D.overlayColour = LIGHT2D.SUNLAMP_OVERLAYCOLOR;
		light2D.Color = LIGHT2D.SUNLAMP_COLOR;
		light2D.Range = 16f;
		light2D.Angle = 5.2f;
		light2D.Direction = LIGHT2D.SUNLAMP_DIRECTION;
		light2D.Offset = LIGHT2D.SUNLAMP_OFFSET;
		light2D.shape = global::LightShape.Cone;
		light2D.drawOverlay = true;
		go.AddOrGetDef<LightController.Def>();
	}

	// Token: 0x04000996 RID: 2454
	public const string ID = "SunLamp";
}
