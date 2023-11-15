﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000044 RID: 68
public class GasConduitElementSensorConfig : ConduitSensorConfig
{
	// Token: 0x1700000C RID: 12
	// (get) Token: 0x0600014A RID: 330 RVA: 0x0000955E File Offset: 0x0000775E
	protected override ConduitType ConduitType
	{
		get
		{
			return ConduitType.Gas;
		}
	}

	// Token: 0x0600014B RID: 331 RVA: 0x00009564 File Offset: 0x00007764
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef result = base.CreateBuildingDef(GasConduitElementSensorConfig.ID, "gas_element_sensor_kanim", TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.GASCONDUITELEMENTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.GASCONDUITELEMENTSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.GASCONDUITELEMENTSENSOR.LOGIC_PORT_INACTIVE, true, false)
		});
		GeneratedBuildings.RegisterWithOverlay(OverlayScreen.GasVentIDs, GasConduitElementSensorConfig.ID);
		return result;
	}

	// Token: 0x0600014C RID: 332 RVA: 0x000095D8 File Offset: 0x000077D8
	public override void DoPostConfigureComplete(GameObject go)
	{
		base.DoPostConfigureComplete(go);
		go.AddOrGet<Filterable>().filterElementState = Filterable.ElementState.Gas;
		ConduitElementSensor conduitElementSensor = go.AddOrGet<ConduitElementSensor>();
		conduitElementSensor.manuallyControlled = false;
		conduitElementSensor.conduitType = this.ConduitType;
		conduitElementSensor.defaultState = false;
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x040000BE RID: 190
	public static string ID = "GasConduitElementSensor";
}
