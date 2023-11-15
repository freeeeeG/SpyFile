using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000215 RID: 533
public class LogicDuplicantSensorConfig : IBuildingConfig
{
	// Token: 0x06000AA1 RID: 2721 RVA: 0x0003D568 File Offset: 0x0003B768
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicDuplicantSensor", 1, 1, "presence_sensor_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER0, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFoundationRotatable, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.AlwaysOperational = true;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort(LogicSwitch.PORT_ID, new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicDuplicantSensor");
		return buildingDef;
	}

	// Token: 0x06000AA2 RID: 2722 RVA: 0x0003D63F File Offset: 0x0003B83F
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicDuplicantSensorConfig.AddVisualizer(go, true);
	}

	// Token: 0x06000AA3 RID: 2723 RVA: 0x0003D648 File Offset: 0x0003B848
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicDuplicantSensor logicDuplicantSensor = go.AddOrGet<LogicDuplicantSensor>();
		logicDuplicantSensor.defaultState = false;
		logicDuplicantSensor.manuallyControlled = false;
		logicDuplicantSensor.pickupRange = 4;
		LogicDuplicantSensorConfig.AddVisualizer(go, false);
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayInFrontOfConduits, false);
	}

	// Token: 0x06000AA4 RID: 2724 RVA: 0x0003D67C File Offset: 0x0003B87C
	private static void AddVisualizer(GameObject prefab, bool movable)
	{
		RangeVisualizer rangeVisualizer = prefab.AddOrGet<RangeVisualizer>();
		rangeVisualizer.OriginOffset = new Vector2I(0, 0);
		rangeVisualizer.RangeMin.x = -2;
		rangeVisualizer.RangeMin.y = 0;
		rangeVisualizer.RangeMax.x = 2;
		rangeVisualizer.RangeMax.y = 4;
		rangeVisualizer.BlockingTileVisible = true;
	}

	// Token: 0x0400066E RID: 1646
	public const string ID = "LogicDuplicantSensor";

	// Token: 0x0400066F RID: 1647
	private const int RANGE = 4;
}
