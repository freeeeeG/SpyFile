using System;
using TUNING;
using UnityEngine;

// Token: 0x020001CF RID: 463
public class HighEnergyParticleSpawnerConfig : IBuildingConfig
{
	// Token: 0x0600093B RID: 2363 RVA: 0x00036980 File Offset: 0x00034B80
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x0600093C RID: 2364 RVA: 0x00036988 File Offset: 0x00034B88
	public override BuildingDef CreateBuildingDef()
	{
		string id = "HighEnergyParticleSpawner";
		int width = 1;
		int height = 2;
		string anim = "radiation_collector_kanim";
		int hitpoints = 30;
		float construction_time = 10f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] raw_MINERALS = MATERIALS.RAW_MINERALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.NotInTiles;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, raw_MINERALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER1, none, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.Overheatable = false;
		buildingDef.ViewMode = OverlayModes.Radiation.ID;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.UseHighEnergyParticleOutputPort = true;
		buildingDef.HighEnergyParticleOutputOffset = new CellOffset(0, 1);
		buildingDef.RequiresPowerInput = true;
		buildingDef.PowerInputOffset = new CellOffset(0, 0);
		buildingDef.EnergyConsumptionWhenActive = 480f;
		buildingDef.ExhaustKilowattsWhenActive = 1f;
		buildingDef.SelfHeatKilowattsWhenActive = 4f;
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.RadiationIDs, "HighEnergyParticleSpawner");
		buildingDef.Deprecated = !Sim.IsRadiationEnabled();
		return buildingDef;
	}

	// Token: 0x0600093D RID: 2365 RVA: 0x00036A60 File Offset: 0x00034C60
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
		Prioritizable.AddRef(go);
		go.AddOrGet<HighEnergyParticleStorage>().capacity = 500f;
		go.AddOrGet<LoopingSounds>();
		HighEnergyParticleSpawner highEnergyParticleSpawner = go.AddOrGet<HighEnergyParticleSpawner>();
		highEnergyParticleSpawner.minLaunchInterval = 2f;
		highEnergyParticleSpawner.radiationSampleRate = 0.2f;
		highEnergyParticleSpawner.minSlider = 50;
		highEnergyParticleSpawner.maxSlider = 500;
	}

	// Token: 0x0600093E RID: 2366 RVA: 0x00036ACD File Offset: 0x00034CCD
	public override void DoPostConfigureComplete(GameObject go)
	{
	}

	// Token: 0x040005C9 RID: 1481
	public const string ID = "HighEnergyParticleSpawner";

	// Token: 0x040005CA RID: 1482
	public const float MIN_LAUNCH_INTERVAL = 2f;

	// Token: 0x040005CB RID: 1483
	public const float RADIATION_SAMPLE_RATE = 0.2f;

	// Token: 0x040005CC RID: 1484
	public const float HEP_PER_RAD = 0.1f;

	// Token: 0x040005CD RID: 1485
	public const int MIN_SLIDER = 50;

	// Token: 0x040005CE RID: 1486
	public const int MAX_SLIDER = 500;

	// Token: 0x040005CF RID: 1487
	public const float DISABLED_CONSUMPTION_RATE = 1f;
}
