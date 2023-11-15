using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200021D RID: 541
public class LogicGateFilterConfig : LogicGateBaseConfig
{
	// Token: 0x06000AD3 RID: 2771 RVA: 0x0003DD01 File Offset: 0x0003BF01
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.CustomSingle;
	}

	// Token: 0x17000023 RID: 35
	// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0003DD04 File Offset: 0x0003BF04
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none
			};
		}
	}

	// Token: 0x17000024 RID: 36
	// (get) Token: 0x06000AD5 RID: 2773 RVA: 0x0003DD18 File Offset: 0x0003BF18
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x17000025 RID: 37
	// (get) Token: 0x06000AD6 RID: 2774 RVA: 0x0003DD2E File Offset: 0x0003BF2E
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000AD7 RID: 2775 RVA: 0x0003DD34 File Offset: 0x0003BF34
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEFILTER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000AD8 RID: 2776 RVA: 0x0003DD81 File Offset: 0x0003BF81
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateFILTER", "logic_filter_kanim", 2, 1);
	}

	// Token: 0x06000AD9 RID: 2777 RVA: 0x0003DD98 File Offset: 0x0003BF98
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGateFilter logicGateFilter = go.AddComponent<LogicGateFilter>();
		logicGateFilter.op = this.GetLogicOp();
		logicGateFilter.inputPortOffsets = this.InputPortOffsets;
		logicGateFilter.outputPortOffsets = this.OutputPortOffsets;
		logicGateFilter.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGateFilter>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000677 RID: 1655
	public const string ID = "LogicGateFILTER";
}
