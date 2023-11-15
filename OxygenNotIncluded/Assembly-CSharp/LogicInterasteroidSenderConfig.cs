using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000224 RID: 548
public class LogicInterasteroidSenderConfig : IBuildingConfig
{
	// Token: 0x06000B06 RID: 2822 RVA: 0x0003E5AB File Offset: 0x0003C7AB
	public override string[] GetDlcIds()
	{
		return DlcManager.AVAILABLE_EXPANSION1_ONLY;
	}

	// Token: 0x06000B07 RID: 2823 RVA: 0x0003E5B4 File Offset: 0x0003C7B4
	public override BuildingDef CreateBuildingDef()
	{
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef("LogicInterasteroidSender", 1, 1, "inter_asteroid_automation_signal_sender_kanim", 30, 30f, TUNING.BUILDINGS.CONSTRUCTION_MASS_KG.TIER2, MATERIALS.REFINED_METALS, 1600f, BuildLocationRule.OnFloor, TUNING.BUILDINGS.DECOR.PENALTY.TIER0, NOISE_POLLUTION.NOISY.TIER0, 0.2f);
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = true;
		buildingDef.AudioCategory = "Metal";
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.SceneLayer = Grid.SceneLayer.Building;
		buildingDef.PermittedRotations = PermittedRotations.Unrotatable;
		buildingDef.AlwaysOperational = false;
		buildingDef.LogicInputPorts = new List<LogicPorts.Port>
		{
			LogicPorts.Port.InputPort("InputPort", new CellOffset(0, 0), STRINGS.BUILDINGS.PREFABS.LOGICDUPLICANTSENSOR.LOGIC_PORT, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.LOGIC_PORT_ACTIVE, STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.LOGIC_PORT_INACTIVE, true, false)
		};
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, "LogicInterasteroidSender");
		return buildingDef;
	}

	// Token: 0x06000B08 RID: 2824 RVA: 0x0003E68F File Offset: 0x0003C88F
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		base.ConfigureBuildingTemplate(go, prefab_tag);
		go.AddOrGet<UserNameable>().savedName = STRINGS.BUILDINGS.PREFABS.LOGICINTERASTEROIDSENDER.DEFAULTNAME;
		go.AddOrGet<LogicBroadcaster>().PORT_ID = "InputPort";
	}

	// Token: 0x06000B09 RID: 2825 RVA: 0x0003E6BE File Offset: 0x0003C8BE
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06000B0A RID: 2826 RVA: 0x0003E6C6 File Offset: 0x0003C8C6
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06000B0B RID: 2827 RVA: 0x0003E6CE File Offset: 0x0003C8CE
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		LogicInterasteroidSenderConfig.AddVisualizer(go);
	}

	// Token: 0x06000B0C RID: 2828 RVA: 0x0003E6D6 File Offset: 0x0003C8D6
	private static void AddVisualizer(GameObject prefab)
	{
		SkyVisibilityVisualizer skyVisibilityVisualizer = prefab.AddOrGet<SkyVisibilityVisualizer>();
		skyVisibilityVisualizer.RangeMin = 0;
		skyVisibilityVisualizer.RangeMax = 0;
		skyVisibilityVisualizer.SkipOnModuleInteriors = true;
	}

	// Token: 0x0400067F RID: 1663
	public const string ID = "LogicInterasteroidSender";

	// Token: 0x04000680 RID: 1664
	public const string INPUT_PORT_ID = "InputPort";
}
