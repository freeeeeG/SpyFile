using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000223 RID: 547
public class LogicInterasteroidReceiverConfig : IBuildingConfig
{
	// Token: 0x06000AFF RID: 2815 RVA: 0x0003E47D File Offset: 0x0003C67D
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000B00 RID: 2816 RVA: 0x0003E484 File Offset: 0x0003C684
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicInterasteroidReceiver", 1, 1, "inter_asteroid_automation_signal_receiver_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AlwaysOperational = false;
		buildingDef.LogicOutputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.OutputPort("OutputPort", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDRECEIVER.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicInterasteroidReceiver");
		return buildingDef;
	}

	// Token: 0x06000B01 RID: 2817 RVA: 0x0003E55F File Offset: 0x0003C75F
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x06000B02 RID: 2818 RVA: 0x0003E567 File Offset: 0x0003C767
	public override void DoPostConfigureComplete(GameObject go)
	{
		go.AddOrGet<LogicBroadcastReceiver>().PORT_ID = "OutputPort";
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x06000B03 RID: 2819 RVA: 0x0003E57F File Offset: 0x0003C77F
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LogicInterasteroidReceiverConfig.AddVisualizer(go);
	}

	// Token: 0x06000B04 RID: 2820 RVA: 0x0003E587 File Offset: 0x0003C787
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x0400067D RID: 1661
	public const string ID = "LogicInterasteroidReceiver";

	// Token: 0x0400067E RID: 1662
	public const string OUTPUT_PORT_ID = "OutputPort";
}
