using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200021C RID: 540
public class LogicGateBufferConfig : LogicGateBaseConfig
{
	// Token: 0x06000ACA RID: 2762 RVA: 0x0003DBE5 File Offset: 0x0003BDE5
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.CustomSingle;
	}

	// Token: 0x17000020 RID: 32
	// (get) Token: 0x06000ACB RID: 2763 RVA: 0x0003DBE8 File Offset: 0x0003BDE8
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

	// Token: 0x17000021 RID: 33
	// (get) Token: 0x06000ACC RID: 2764 RVA: 0x0003DBFC File Offset: 0x0003BDFC
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

	// Token: 0x17000022 RID: 34
	// (get) Token: 0x06000ACD RID: 2765 RVA: 0x0003DC12 File Offset: 0x0003BE12
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000ACE RID: 2766 RVA: 0x0003DC18 File Offset: 0x0003BE18
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEBUFFER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000ACF RID: 2767 RVA: 0x0003DC65 File Offset: 0x0003BE65
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateBUFFER", "logic_buffer_kanim", 2, 1);
	}

	// Token: 0x06000AD0 RID: 2768 RVA: 0x0003DC7C File Offset: 0x0003BE7C
	public override void DoPostConfigureComplete(GameObject go)
	{
		LogicGateBuffer logicGateBuffer = go.AddComponent<LogicGateBuffer>();
		logicGateBuffer.op = this.GetLogicOp();
		logicGateBuffer.inputPortOffsets = this.InputPortOffsets;
		logicGateBuffer.outputPortOffsets = this.OutputPortOffsets;
		logicGateBuffer.controlPortOffsets = this.ControlPortOffsets;
		go.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject game_object)
		{
			game_object.GetComponent<LogicGateBuffer>().SetPortDescriptions(this.GetDescriptions());
		};
		go.GetComponent<KPrefabID>().AddTag(GameTags.OverlayBehindConduits, false);
	}

	// Token: 0x04000676 RID: 1654
	public const string ID = "LogicGateBUFFER";
}
