using System;
using TUNING;
using UnityEngine;

// Token: 0x02000060 RID: 96
public class DevHEPSpawnerConfig : IBuildingConfig
{
	// Token: 0x060001B8 RID: 440 RVA: 0x0000C3BD File Offset: 0x0000A5BD
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x0000C3C4 File Offset: 0x0000A5C4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "DevHEPSpawner";
		int width = 1;
		int height = 1;
		string anim = "dev_radbolt_generator_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Invincible = true;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 0);
		buildingDef.RequiresPowerInput = false;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(0, 0));
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "DevHEPSpawner");
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		buildingDef.DebugOnly = true;
		return buildingDef;
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0000C49C File Offset: 0x0000A69C
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddTag(GameTags.DevBuilding);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		Prioritizable.AddRef(go);
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<DevHEPSpawner>().boltAmount = 50f;
		go.AddOrGet<LogicOperationalController>();
	}

	// Token: 0x060001BB RID: 443 RVA: 0x0000C4ED File Offset: 0x0000A6ED
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x04000110 RID: 272
	public const string ID = "DevHEPSpawner";
}
