using System;
using TUNING;
using UnityEngine;

// Token: 0x02000335 RID: 821
public class SolarPanelConfig : IBuildingConfig
{
	// Token: 0x060010AF RID: 4271 RVA: 0x0005A4A4 File Offset: 0x000586A4
	public override BuildingDef CreateBuildingDef()
	{
		string id = "SolarPanel";
		int width = 7;
		int height = 3;
		string anim = "solar_panel_kanim";
		int hitpoints = 100;
		float construction_time = 120f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] glasses = MATERIALS.GLASSES;
		float melting_point = 2400f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER5;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, tier, glasses, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER2, tier2, 0.2f);
		buildingDef.GeneratorWattageRating = 380f;
		buildingDef.GeneratorBaseCapacity = buildingDef.GeneratorWattageRating;
		buildingDef.ExhaustKilowattsWhenActive = 0f;
		buildingDef.SelfHeatKilowattsWhenActive = 0f;
		buildingDef.BuildLocationRule = BuildLocationRule.Anywhere;
		buildingDef.HitPoints = 10;
		buildingDef.RequiresPowerOutput = true;
		buildingDef.PowerOutputOffset = new CellOffset(0, 0);
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "HollowMetal";
		buildingDef.AudioSize = "large";
		return buildingDef;
	}

	// Token: 0x060010B0 RID: 4272 RVA: 0x0005A55B File Offset: 0x0005875B
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.GetComponent<KPrefabID>().AddTag(RoomConstraints.ConstraintTags.IndustrialMachinery, false);
		go.AddOrGet<LoopingSounds>();
		Prioritizable.AddRef(go);
	}

	// Token: 0x060010B1 RID: 4273 RVA: 0x0005A57C File Offset: 0x0005877C
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Repairable>().expectedRepairTime = 52.5f;
		go.AddOrGet<SolarPanel>().powerDistributionOrder = 9;
		go.AddOrGetDef<PoweredActiveController.Def>();
		MakeBaseSolid.Def def = go.AddOrGetDef<MakeBaseSolid.Def>();
		def.occupyFoundationLayer = false;
		def.solidOffsets = new CellOffset[7];
		for (int i = 0; i < 7; i++)
		{
			def.solidOffsets[i] = new CellOffset(i - 3, 0);
		}
	}

	// Token: 0x04000920 RID: 2336
	public const string ID = "SolarPanel";

	// Token: 0x04000921 RID: 2337
	public const float WATTS_PER_LUX = 0.00053f;

	// Token: 0x04000922 RID: 2338
	public const float MAX_WATTS = 380f;

	// Token: 0x04000923 RID: 2339
	private const int WIDTH = 7;
}
