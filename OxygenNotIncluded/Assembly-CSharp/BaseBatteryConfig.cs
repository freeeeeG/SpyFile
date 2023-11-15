using System;
using TUNING;
using UnityEngine;

// Token: 0x0200001F RID: 31
public abstract class BaseBatteryConfig : IBuildingConfig
{
	// Token: 0x06000086 RID: 134 RVA: 0x000056AC File Offset: 0x000038AC
	public BuildingDef CreateBuildingDef(string id, int width, int height, int hitpoints, string anim, float construction_time, float[] construction_mass, string[] construction_materials, float melting_point, float exhaust_temperature_active, float self_heat_kilowatts_active, EffectorValues decor, EffectorValues noise)
	{
		BuildLocationRule build_location_rule = BuildLocationRule.OnFloor;
		EffectorValues tier = NOISE_POLLUTION.NOISY.TIER0;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(id, width, height, anim, hitpoints, construction_time, construction_mass, construction_materials, melting_point, build_location_rule, decor, tier, 0.2f);
		buildingDef.ExhaustKilowattsWhenActive = exhaust_temperature_active;
		buildingDef.SelfHeatKilowattsWhenActive = self_heat_kilowatts_active;
		buildingDef.Entombable = false;
		buildingDef.ViewMode = OverlayModes.Power.ID;
		buildingDef.AudioCategory = "Metal";
		buildingDef.RequiresPowerOutput = true;
		buildingDef.UseWhitePowerOutputConnectorColour = true;
		return buildingDef;
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00005719 File Offset: 0x00003919
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		go.AddComponent<RequireInputs>();
	}

	// Token: 0x06000088 RID: 136 RVA: 0x00005722 File Offset: 0x00003922
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<Battery>().powerSortOrder = 1000;
		go.AddOrGetDef<PoweredActiveController.Def>();
	}
}
