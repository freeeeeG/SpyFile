using System;
using STRINGS;

// Token: 0x0200021B RID: 539
public class LogicGateNotConfig : LogicGateBaseConfig
{
	// Token: 0x06000AC3 RID: 2755 RVA: 0x0003DB49 File Offset: 0x0003BD49
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Not;
	}

	// Token: 0x1700001D RID: 29
	// (get) Token: 0x06000AC4 RID: 2756 RVA: 0x0003DB4C File Offset: 0x0003BD4C
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

	// Token: 0x1700001E RID: 30
	// (get) Token: 0x06000AC5 RID: 2757 RVA: 0x0003DB60 File Offset: 0x0003BD60
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

	// Token: 0x1700001F RID: 31
	// (get) Token: 0x06000AC6 RID: 2758 RVA: 0x0003DB76 File Offset: 0x0003BD76
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x0003DB7C File Offset: 0x0003BD7C
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATENOT.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x0003DBC9 File Offset: 0x0003BDC9
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateNOT", "logic_not_kanim", 2, 1);
	}

	// Token: 0x04000675 RID: 1653
	public const string ID = "LogicGateNOT";
}
