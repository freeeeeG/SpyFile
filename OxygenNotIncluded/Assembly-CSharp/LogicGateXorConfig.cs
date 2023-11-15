using System;
using STRINGS;

// Token: 0x0200021A RID: 538
public class LogicGateXorConfig : LogicGateBaseConfig
{
	// Token: 0x06000ABC RID: 2748 RVA: 0x0003DAA1 File Offset: 0x0003BCA1
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Xor;
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x06000ABD RID: 2749 RVA: 0x0003DAA4 File Offset: 0x0003BCA4
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

	// Token: 0x1700001B RID: 27
	// (get) Token: 0x06000ABE RID: 2750 RVA: 0x0003DAC6 File Offset: 0x0003BCC6
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

	// Token: 0x1700001C RID: 28
	// (get) Token: 0x06000ABF RID: 2751 RVA: 0x0003DADC File Offset: 0x0003BCDC
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000AC0 RID: 2752 RVA: 0x0003DAE0 File Offset: 0x0003BCE0
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

	// Token: 0x06000AC1 RID: 2753 RVA: 0x0003DB2D File Offset: 0x0003BD2D
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateXOR", "logic_xor_kanim", 2, 2);
	}

	// Token: 0x04000674 RID: 1652
	public const string ID = "LogicGateXOR";
}
