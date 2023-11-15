using System;
using TUNING;
using UnityEngine;

// Token: 0x02000023 RID: 35
public class BatteryConfig : BaseBatteryConfig
{
	// Token: 0x0600009B RID: 155 RVA: 0x00005D80 File Offset: 0x00003F80
	public override BuildingDef CreateBuildingDef()
	{
		string id = "Battery";
		int width = 1;
		int height = 2;
		int hitpoints = 30;
		string anim = "batterysm_kanim";
		float construction_time = 30f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] all_METALS = MATERIALS.ALL_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0.25f;
		float self_heat_kilowatts_active = 1f;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, all_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, BUILDINGS.DECOR.PENALTY.TIER1, none);
		buildingDef.Breakable = true;
		SoundEventVolumeCache.instance.AddVolume("batterysm_kanim", "Battery_rattle", NOISE_POLLUTION.NOISY.TIER1);
		return buildingDef;
	}

	// Token: 0x0600009C RID: 156 RVA: 0x00005DEB File Offset: 0x00003FEB
	public override void DoPostConfigureComplete(GameObject go)
	{
		Battery battery = go.AddOrGet<Battery>();
		battery.capacity = 10000f;
		battery.joulesLostPerSecond = 1.6666666f;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x0400006C RID: 108
	public const string ID = "Battery";
}
