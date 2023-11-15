using System;
using TUNING;
using UnityEngine;

// Token: 0x02000347 RID: 839
public class SpaceHeaterConfig : IBuildingConfig
{
	// Token: 0x0600110D RID: 4365 RVA: 0x0005BDE4 File Offset: 0x00059FE4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SpaceHeater";
		int width = 2;
		int height = 2;
		string anim = "spaceheater_kanim";
		int hitpoints = 30;
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER2;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, all_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.BONUS.TIER1, tier2, 0.2f);
		buildingDef.RequiresPowerInput = true;
		buildingDef.EnergyConsumptionWhenActive = 120f;
		buildingDef.ExhaustKilowattsWhenActive = 2f;
		buildingDef.SelfHeatKilowattsWhenActive = 16f;
		buildingDef.LogicInputPorts = LogicOperationalController.CreateSingleInputPortList(new CellOffset(1, 0));
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.OverheatTemperature = 398.15f;
		return buildingDef;
	}

	// Token: 0x0600110E RID: 4366 RVA: 0x0005BE85 File Offset: 0x0005A085
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddOrGet<LoopingSounds>();
		go.AddOrGet<SpaceHeater>().targetTemperature = 343.15f;
	}

	// Token: 0x0600110F RID: 4367 RVA: 0x0005BE9E File Offset: 0x0005A09E
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicOperationalController>();
		go.AddOrGetDef<PoweredActiveController.Def>();
	}

	// Token: 0x04000942 RID: 2370
	public const string ID = "SpaceHeater";
}
