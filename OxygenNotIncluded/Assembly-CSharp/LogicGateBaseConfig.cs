using System;
using TUNING;
using UnityEngine;

// Token: 0x02000220 RID: 544
public abstract class LogicGateBaseConfig : IBuildingConfig
{
	// Token: 0x06000AEA RID: 2794 RVA: 0x0003DFF0 File Offset: 0x0003C1F0
	protected BuildingDef CreateBuildingDef(string ID, string anim, int width = 2, int height = 2)
	{
		int hitpoints = 10;
		float construction_time = 3f;
		float[] tier = BUILDINGS.CONSTRUCTION_MASS_KG.TIER0;
		string[] refined_METALS = MATERIALS.REFINED_METALS;
		float melting_point = 1600f;
		BuildLocationRule build_location_rule = BuildLocationRule.Anywhere;
		EffectorValues none = NOISE_POLLUTION.NONE;
		BuildingDef buildingDef = BuildingTemplates.CreateBuildingDef(ID, width, height, anim, hitpoints, construction_time, tier, refined_METALS, melting_point, build_location_rule, BUILDINGS.DECOR.PENALTY.TIER0, none, 0.2f);
		buildingDef.ViewMode = OverlayModes.Logic.ID;
		buildingDef.ObjectLayer = ObjectLayer.LogicGate;
		buildingDef.SceneLayer = Grid.SceneLayer.LogicGates;
		buildingDef.ThermalConductivity = 0.05f;
		buildingDef.Floodable = false;
		buildingDef.Overheatable = false;
		buildingDef.Entombable = false;
		buildingDef.AudioCategory = "Metal";
		buildingDef.AudioSize = "small";
		buildingDef.BaseTimeUntilRepair = -1f;
		buildingDef.PermittedRotations = PermittedRotations.R360;
		buildingDef.DragBuild = true;
		LogicGateBase.uiSrcData = Assets.instance.logicModeUIData;
		GeneratedBuildings.RegisterWithOverlay(OverlayModes.Logic.HighlightItemIDs, ID);
		return buildingDef;
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x06000AEB RID: 2795
	protected abstract CellOffset[] InputPortOffsets { get; }

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x06000AEC RID: 2796
	protected abstract CellOffset[] OutputPortOffsets { get; }

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x06000AED RID: 2797
	protected abstract CellOffset[] ControlPortOffsets { get; }

	// Token: 0x06000AEE RID: 2798
	protected abstract LogicGateBase.Op GetLogicOp();

	// Token: 0x06000AEF RID: 2799
	protected abstract LogicGate.LogicGateDescriptions GetDescriptions();

	// Token: 0x06000AF0 RID: 2800 RVA: 0x0003E0B3 File Offset: 0x0003C2B3
	public override void ConfigureBuildingTemplate(GameObject go, Tag prefab_tag)
	{
		GeneratedBuildings.MakeBuildingAlwaysOperational(go);
		BuildingConfigManager.Instance.IgnoreDefaultKComponent(typeof(RequiresFoundation), prefab_tag);
	}

	// Token: 0x06000AF1 RID: 2801 RVA: 0x0003E0D0 File Offset: 0x0003C2D0
	public override void DoPostConfigurePreview(BuildingDef def, GameObject go)
	{
		base.DoPostConfigurePreview(def, go);
		MoveableLogicGateVisualizer moveableLogicGateVisualizer = go.AddComponent<MoveableLogicGateVisualizer>();
		moveableLogicGateVisualizer.op = this.GetLogicOp();
		moveableLogicGateVisualizer.inputPortOffsets = this.InputPortOffsets;
		moveableLogicGateVisualizer.outputPortOffsets = this.OutputPortOffsets;
		moveableLogicGateVisualizer.controlPortOffsets = this.ControlPortOffsets;
	}

	// Token: 0x06000AF2 RID: 2802 RVA: 0x0003E10F File Offset: 0x0003C30F
	public override void DoPostConfigureUnderConstruction(GameObject go)
	{
		base.DoPostConfigureUnderConstruction(go);
		LogicGateVisualizer logicGateVisualizer = go.AddComponent<LogicGateVisualizer>();
		logicGateVisualizer.op = this.GetLogicOp();
		logicGateVisualizer.inputPortOffsets = this.InputPortOffsets;
		logicGateVisualizer.outputPortOffsets = this.OutputPortOffsets;
		logicGateVisualizer.controlPortOffsets = this.ControlPortOffsets;
	}

	// Token: 0x06000AF3 RID: 2803 RVA: 0x0003E150 File Offset: 0x0003C350
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGate logicGate = go.AddComponent<LogicGate>();
		logicGate.op = this.GetLogicOp();
		logicGate.inputPortOffsets = this.InputPortOffsets;
		logicGate.outputPortOffsets = this.OutputPortOffsets;
		logicGate.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGate>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}
}
