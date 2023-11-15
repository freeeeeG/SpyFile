﻿using System;
using TUNING;
using UnityEngine;

// Token: 0x02000024 RID: 36
public class BatteryMediumConfig : BaseBatteryConfig
{
	// Token: 0x0600009E RID: 158 RVA: 0x00005E18 File Offset: 0x00004018
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BatteryMedium";
		int width = 2;
		int height = 2;
		int hitpoints = 30;
		string anim = "batterymed_kanim";
		float construction_time = 60f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER4;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0.25f;
		float self_heat_kilowatts_active = 1f;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef result = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, all_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, BUILDINGS.DECOR.PENALTY.TIER2, tier2);
		SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", NOISE_POLLUTION.NOISY.TIER2);
		return result;
	}

	// Token: 0x0600009F RID: 159 RVA: 0x00005E7C File Offset: 0x0000407C
	public override void DoPostConfigureComplete(GameObject go)
	{
		Battery battery = go.AddOrGet<Battery>();
		battery.capacity = 40000f;
		battery.joulesLostPerSecond = 3.3333333f;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x0400006D RID: 109
	public const string ID = "BatteryMedium";
}
