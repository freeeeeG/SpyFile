using System;
using STRINGS;

// Token: 0x02000218 RID: 536
public class LogicGateAndConfig : LogicGateBaseConfig
{
	// Token: 0x06000AAE RID: 2734 RVA: 0x0003D952 File Offset: 0x0003BB52
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.And;
	}

	// Token: 0x17000014 RID: 20
	// (get) Token: 0x06000AAF RID: 2735 RVA: 0x0003D955 File Offset: 0x0003BB55
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				CellOffset.none,
				new CellOffset(0, 1)
			};
		}
	}

	// Token: 0x17000015 RID: 21
	// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0003D977 File Offset: 0x0003BB77
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

	// Token: 0x17000016 RID: 22
	// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x0003D98D File Offset: 0x0003BB8D
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000AB2 RID: 2738 RVA: 0x0003D990 File Offset: 0x0003BB90
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEAND.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000AB3 RID: 2739 RVA: 0x0003D9DD File Offset: 0x0003BBDD
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateAND", "logic_and_kanim", 2, 2);
	}

	// Token: 0x04000672 RID: 1650
	public const string ID = "LogicGateAND";
}
