using System;
using STRINGS;

// Token: 0x0200021F RID: 543
public class LogicGateDemultiplexerConfig : LogicGateBaseConfig
{
	// Token: 0x06000AE3 RID: 2787 RVA: 0x0003DF05 File Offset: 0x0003C105
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Demultiplexer;
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x06000AE4 RID: 2788 RVA: 0x0003DF08 File Offset: 0x0003C108
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 3)
			};
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x06000AE5 RID: 2789 RVA: 0x0003DF1E File Offset: 0x0003C11E
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 3),
				new CellOffset(1, 2),
				new CellOffset(1, 1),
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x06000AE6 RID: 2790 RVA: 0x0003DF5E File Offset: 0x0003C15E
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 0),
				new CellOffset(0, 0)
			};
		}
	}

	// Token: 0x06000AE7 RID: 2791 RVA: 0x0003DF84 File Offset: 0x0003C184
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEXOR.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000AE8 RID: 2792 RVA: 0x0003DFD1 File Offset: 0x0003C1D1
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateDemultiplexer", "logic_demultiplexer_kanim", 3, 4);
	}

	// Token: 0x04000679 RID: 1657
	public const string ID = "LogicGateDemultiplexer";
}
