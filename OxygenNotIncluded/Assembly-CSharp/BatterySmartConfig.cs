using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000026 RID: 38
public class BatterySmartConfig : BaseBatteryConfig
{
	// Token: 0x060000A7 RID: 167 RVA: 0x00006050 File Offset: 0x00004250
	public override BuildingDef CreateBuildingDef()
	{
		string id = "BatterySmart";
		int width = 2;
		int height = 2;
		int hitpoints = 30;
		string anim = "smartbattery_kanim";
		float construction_time = 60f;
		float[] tier = TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER3;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 800f;
		float exhaust_temperature_active = 0f;
		float self_heat_kilowatts_active = 0.5f;
		EffectorValues tier2 = NOISE_POLLUTION.NOISY.TIER1;
		BuildingDef buildingDef = base.CreateBuildingDef(id, width, height, hitpoints, anim, construction_time, tier, refined_METALS, melting_point, exhaust_temperature_active, self_heat_kilowatts_active, TUNING.BUILDINGS.DECOR.PENALTY.TIER2, tier2);
		SoundEventVolumeCache.instance.AddVolume("batterymed_kanim", "Battery_med_rattle", NOISE_POLLUTION.NOISY.TIER2);
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(BatterySmart.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.BATTERYSMART.LOGIC_PORT_INACTIVE, true, false)
		};
		return buildingDef;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x000060F6 File Offset: 0x000042F6
	public override void DoPostConfigureComplete(GameObject go)
	{
		BatterySmart batterySmart = go.AddOrGet<BatterySmart>();
		batterySmart.capacity = 20000f;
		batterySmart.joulesLostPerSecond = 0.6666667f;
		batterySmart.powerSortOrder = 1000;
		base.DoPostConfigureComplete(go);
	}

	// Token: 0x04000071 RID: 113
	public const string ID = "BatterySmart";
}
