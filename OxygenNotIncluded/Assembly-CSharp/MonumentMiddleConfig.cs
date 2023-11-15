using System;
using Database;
using TUNING;
using UnityEngine;

// Token: 0x020002A5 RID: 677
public class MonumentMiddleConfig : IBuildingConfig
{
	// Token: 0x06000DD3 RID: 3539 RVA: 0x0004CA48 File Offset: 0x0004AC48
	public override BuildingDef CreateBuildingDef()
	{
		string id = "MonumentMiddle";
		int width = 5;
		int height = 5;
		string anim = "monument_mid_a_kanim";
		int hitpoints = 1000;
		float construction_time = 60f;
		float[] construction_mass = new float[]
		{
			2500f,
			2500f,
			5000f
		};
		string[] construction_materials = new string[]
		{
			SimHashes.Ceramic.ToString(),
			SimHashes.Polypropylene.ToString(),
			SimHashes.Steel.ToString()
		};
		float melting_point = 9999f;
		BuildLocationRule build_location_rule = BuildLocationRule.BuildingAttachPoint;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.MONUMENT.INCOMPLETE, tier, 0.2f);
		BuildingTemplates.CreateMonumentBuildingDef(buildingDef);
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.OverheatTemperature = 2273.15f;
		buildingDef.Floodable = false;
		buildingDef.AttachmentSlotTag = "MonumentMiddle";
		buildingDef.ObjectLayer = ObjectLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.FlipH;
		buildingDef.attachablePosition = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.CanMove = false;
		return buildingDef;
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x0004CB3C File Offset: 0x0004AD3C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<BuildingAttachPoint>().points = new BuildingAttachPoint.HardPoint[]
		{
			new BuildingAttachPoint.HardPoint(new CellOffset(0, 5), "MonumentTop", null)
		};
		go.AddOrGet<MonumentPart>().part = MonumentPartResource.Part.Middle;
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0004CBA0 File Offset: 0x0004ADA0
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x0004CBA2 File Offset: 0x0004ADA2
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0004CBA4 File Offset: 0x0004ADA4
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<KBatchedAnimController>().initialAnim = "option_a";
		go.GetComponent<KPrefabID>().prefabSpawnFn += delegate(GameObject game_object)
		{
			MonumentPart monumentPart = game_object.AddOrGet<MonumentPart>();
			monumentPart.part = MonumentPartResource.Part.Middle;
			monumentPart.stateUISymbol = "mid";
		};
	}

	// Token: 0x040007EE RID: 2030
	public const string ID = "MonumentMiddle";
}
