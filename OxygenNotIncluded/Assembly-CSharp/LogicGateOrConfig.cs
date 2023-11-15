using System;
using STRINGS;

// Token: 0x02000219 RID: 537
public class LogicGateOrConfig : LogicGateBaseConfig
{
	// Token: 0x06000AB5 RID: 2741 RVA: 0x0003D9F9 File Offset: 0x0003BBF9
	protected override LogicGateBase.Op GetLogicOp()
	{
		return LogicGateBase.Op.Or;
	}

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x06000AB6 RID: 2742 RVA: 0x0003D9FC File Offset: 0x0003BBFC
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

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0003DA1E File Offset: 0x0003BC1E
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

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x06000AB8 RID: 2744 RVA: 0x0003DA34 File Offset: 0x0003BC34
	protected override CellOffset[] ControlPortOffsets
	{
		get
		{
			return null;
		}
	}

	// Token: 0x06000AB9 RID: 2745 RVA: 0x0003DA38 File Offset: 0x0003BC38
	protected override LogicGate.LogicGateDescriptions GetDescriptions()
	{
		return new LogicGate.LogicGateDescriptions
		{
			outputOne = new LogicGate.LogicGateDescriptions.Description
			{
				name = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_NAME,
				active = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_ACTIVE,
				inactive = BUILDINGS.PREFABS.LOGICGATEOR.OUTPUT_INACTIVE
			}
		};
	}

	// Token: 0x06000ABA RID: 2746 RVA: 0x0003DA85 File Offset: 0x0003BC85
	public override BuildingDef CreateBuildingDef()
	{
		return base.CreateBuildingDef("LogicGateOR", "logic_or_kanim", 2, 2);
	}

	// Token: 0x04000673 RID: 1651
	public const string ID = "LogicGateOR";
}
