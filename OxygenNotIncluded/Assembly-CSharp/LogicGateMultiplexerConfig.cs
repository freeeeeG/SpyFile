using System;
using STRINGS;

// Token: 0x0200021E RID: 542
public class LogicGateMultiplexerConfig : LogicGateBaseConfig
{
	// Token: 0x06000ADC RID: 2780 RVA: 0x0003DE1D File Offset: 0x0003C01D
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Multiplexer;
	}

	// Token: 0x17000026 RID: 38
	// (get) Token: 0x06000ADD RID: 2781 RVA: 0x0003DE20 File Offset: 0x0003C020
	protected override CellOffset[] InputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(-1, 3),
				new CellOffset(-1, 2),
				new CellOffset(-1, 1),
				new CellOffset(-1, 0)
			};
		}
	}

	// Token: 0x17000027 RID: 39
	// (get) Token: 0x06000ADE RID: 2782 RVA: 0x0003DE60 File Offset: 0x0003C060
	protected override CellOffset[] OutputPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(1, 3)
			};
		}
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x06000ADF RID: 2783 RVA: 0x0003DE76 File Offset: 0x0003C076
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return new CellOffset[]
			{
				new CellOffset(0, 0),
				new CellOffset(1, 0)
			};
		}
	}

	// Token: 0x06000AE0 RID: 2784 RVA: 0x0003DE9C File Offset: 0x0003C09C
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEMULTIPLEXER.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000AE1 RID: 2785 RVA: 0x0003DEE9 File Offset: 0x0003C0E9
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateMultiplexer", "logic_multiplexer_kanim", 3, 4);
	}

	// Token: 0x04000678 RID: 1656
	public const string ID = "LogicGateMultiplexer";
}
