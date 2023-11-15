using System;
using UnityEngine;

// Token: 0x0200062F RID: 1583
[AddComponentMenu("KMonoBehaviour/scripts/LogicGateBase")]
public class LogicGateBase : KMonoBehaviour
{
	// Token: 0x06002874 RID: 10356 RVA: 0x000DAD28 File Offset: 0x000D8F28
	private int GetActualCell(CellOffset offset)
	{
		Rotatable component = base.GetComponent<Rotatable>();
		if (component != null)
		{
			offset = component.GetRotatedCellOffset(offset);
		}
		return Grid.OffsetCell(Grid.PosToCell(base.transform.GetPosition()), offset);
	}

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06002875 RID: 10357 RVA: 0x000DAD64 File Offset: 0x000D8F64
	public int InputCellOne
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[0]);
		}
	}

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06002876 RID: 10358 RVA: 0x000DAD78 File Offset: 0x000D8F78
	public int InputCellTwo
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[1]);
		}
	}

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06002877 RID: 10359 RVA: 0x000DAD8C File Offset: 0x000D8F8C
	public int InputCellThree
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[2]);
		}
	}

	// Token: 0x17000239 RID: 569
	// (get) Token: 0x06002878 RID: 10360 RVA: 0x000DADA0 File Offset: 0x000D8FA0
	public int InputCellFour
	{
		get
		{
			return this.GetActualCell(this.inputPortOffsets[3]);
		}
	}

	// Token: 0x1700023A RID: 570
	// (get) Token: 0x06002879 RID: 10361 RVA: 0x000DADB4 File Offset: 0x000D8FB4
	public int OutputCellOne
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[0]);
		}
	}

	// Token: 0x1700023B RID: 571
	// (get) Token: 0x0600287A RID: 10362 RVA: 0x000DADC8 File Offset: 0x000D8FC8
	public int OutputCellTwo
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[1]);
		}
	}

	// Token: 0x1700023C RID: 572
	// (get) Token: 0x0600287B RID: 10363 RVA: 0x000DADDC File Offset: 0x000D8FDC
	public int OutputCellThree
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[2]);
		}
	}

	// Token: 0x1700023D RID: 573
	// (get) Token: 0x0600287C RID: 10364 RVA: 0x000DADF0 File Offset: 0x000D8FF0
	public int OutputCellFour
	{
		get
		{
			return this.GetActualCell(this.outputPortOffsets[3]);
		}
	}

	// Token: 0x1700023E RID: 574
	// (get) Token: 0x0600287D RID: 10365 RVA: 0x000DAE04 File Offset: 0x000D9004
	public int ControlCellOne
	{
		get
		{
			return this.GetActualCell(this.controlPortOffsets[0]);
		}
	}

	// Token: 0x1700023F RID: 575
	// (get) Token: 0x0600287E RID: 10366 RVA: 0x000DAE18 File Offset: 0x000D9018
	public int ControlCellTwo
	{
		get
		{
			return this.GetActualCell(this.controlPortOffsets[1]);
		}
	}

	// Token: 0x0600287F RID: 10367 RVA: 0x000DAE2C File Offset: 0x000D902C
	public int PortCell(LogicGateBase.PortId port)
	{
		switch (port)
		{
		case LogicGateBase.PortId.InputOne:
			return this.InputCellOne;
		case LogicGateBase.PortId.InputTwo:
			return this.InputCellTwo;
		case LogicGateBase.PortId.InputThree:
			return this.InputCellThree;
		case LogicGateBase.PortId.InputFour:
			return this.InputCellFour;
		case LogicGateBase.PortId.OutputOne:
			return this.OutputCellOne;
		case LogicGateBase.PortId.OutputTwo:
			return this.OutputCellTwo;
		case LogicGateBase.PortId.OutputThree:
			return this.OutputCellThree;
		case LogicGateBase.PortId.OutputFour:
			return this.OutputCellFour;
		case LogicGateBase.PortId.ControlOne:
			return this.ControlCellOne;
		case LogicGateBase.PortId.ControlTwo:
			return this.ControlCellTwo;
		default:
			return this.OutputCellOne;
		}
	}

	// Token: 0x06002880 RID: 10368 RVA: 0x000DAEB8 File Offset: 0x000D90B8
	public bool TryGetPortAtCell(int cell, out LogicGateBase.PortId port)
	{
		if (cell == this.InputCellOne)
		{
			port = LogicGateBase.PortId.InputOne;
			return true;
		}
		if ((this.RequiresTwoInputs || this.RequiresFourInputs) && cell == this.InputCellTwo)
		{
			port = LogicGateBase.PortId.InputTwo;
			return true;
		}
		if (this.RequiresFourInputs && cell == this.InputCellThree)
		{
			port = LogicGateBase.PortId.InputThree;
			return true;
		}
		if (this.RequiresFourInputs && cell == this.InputCellFour)
		{
			port = LogicGateBase.PortId.InputFour;
			return true;
		}
		if (cell == this.OutputCellOne)
		{
			port = LogicGateBase.PortId.OutputOne;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellTwo)
		{
			port = LogicGateBase.PortId.OutputTwo;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellThree)
		{
			port = LogicGateBase.PortId.OutputThree;
			return true;
		}
		if (this.RequiresFourOutputs && cell == this.OutputCellFour)
		{
			port = LogicGateBase.PortId.OutputFour;
			return true;
		}
		if (this.RequiresControlInputs && cell == this.ControlCellOne)
		{
			port = LogicGateBase.PortId.ControlOne;
			return true;
		}
		if (this.RequiresControlInputs && cell == this.ControlCellTwo)
		{
			port = LogicGateBase.PortId.ControlTwo;
			return true;
		}
		port = LogicGateBase.PortId.InputOne;
		return false;
	}

	// Token: 0x17000240 RID: 576
	// (get) Token: 0x06002881 RID: 10369 RVA: 0x000DAF9E File Offset: 0x000D919E
	public bool RequiresTwoInputs
	{
		get
		{
			return LogicGateBase.OpRequiresTwoInputs(this.op);
		}
	}

	// Token: 0x17000241 RID: 577
	// (get) Token: 0x06002882 RID: 10370 RVA: 0x000DAFAB File Offset: 0x000D91AB
	public bool RequiresFourInputs
	{
		get
		{
			return LogicGateBase.OpRequiresFourInputs(this.op);
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06002883 RID: 10371 RVA: 0x000DAFB8 File Offset: 0x000D91B8
	public bool RequiresFourOutputs
	{
		get
		{
			return LogicGateBase.OpRequiresFourOutputs(this.op);
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06002884 RID: 10372 RVA: 0x000DAFC5 File Offset: 0x000D91C5
	public bool RequiresControlInputs
	{
		get
		{
			return LogicGateBase.OpRequiresControlInputs(this.op);
		}
	}

	// Token: 0x06002885 RID: 10373 RVA: 0x000DAFD2 File Offset: 0x000D91D2
	public static bool OpRequiresTwoInputs(LogicGateBase.Op op)
	{
		return op != LogicGateBase.Op.Not && op - LogicGateBase.Op.CustomSingle > 2;
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x000DAFE1 File Offset: 0x000D91E1
	public static bool OpRequiresFourInputs(LogicGateBase.Op op)
	{
		return op == LogicGateBase.Op.Multiplexer;
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x000DAFEA File Offset: 0x000D91EA
	public static bool OpRequiresFourOutputs(LogicGateBase.Op op)
	{
		return op == LogicGateBase.Op.Demultiplexer;
	}

	// Token: 0x06002888 RID: 10376 RVA: 0x000DAFF3 File Offset: 0x000D91F3
	public static bool OpRequiresControlInputs(LogicGateBase.Op op)
	{
		return op - LogicGateBase.Op.Multiplexer <= 1;
	}

	// Token: 0x04001780 RID: 6016
	public static LogicModeUI uiSrcData;

	// Token: 0x04001781 RID: 6017
	public static readonly HashedString OUTPUT_TWO_PORT_ID = new HashedString("LogicGateOutputTwo");

	// Token: 0x04001782 RID: 6018
	public static readonly HashedString OUTPUT_THREE_PORT_ID = new HashedString("LogicGateOutputThree");

	// Token: 0x04001783 RID: 6019
	public static readonly HashedString OUTPUT_FOUR_PORT_ID = new HashedString("LogicGateOutputFour");

	// Token: 0x04001784 RID: 6020
	[SerializeField]
	public LogicGateBase.Op op;

	// Token: 0x04001785 RID: 6021
	public static CellOffset[] portOffsets = new CellOffset[]
	{
		CellOffset.none,
		new CellOffset(0, 1),
		new CellOffset(1, 0)
	};

	// Token: 0x04001786 RID: 6022
	public CellOffset[] inputPortOffsets;

	// Token: 0x04001787 RID: 6023
	public CellOffset[] outputPortOffsets;

	// Token: 0x04001788 RID: 6024
	public CellOffset[] controlPortOffsets;

	// Token: 0x020012F9 RID: 4857
	public enum PortId
	{
		// Token: 0x0400612A RID: 24874
		InputOne,
		// Token: 0x0400612B RID: 24875
		InputTwo,
		// Token: 0x0400612C RID: 24876
		InputThree,
		// Token: 0x0400612D RID: 24877
		InputFour,
		// Token: 0x0400612E RID: 24878
		OutputOne,
		// Token: 0x0400612F RID: 24879
		OutputTwo,
		// Token: 0x04006130 RID: 24880
		OutputThree,
		// Token: 0x04006131 RID: 24881
		OutputFour,
		// Token: 0x04006132 RID: 24882
		ControlOne,
		// Token: 0x04006133 RID: 24883
		ControlTwo
	}

	// Token: 0x020012FA RID: 4858
	public enum Op
	{
		// Token: 0x04006135 RID: 24885
		And,
		// Token: 0x04006136 RID: 24886
		Or,
		// Token: 0x04006137 RID: 24887
		Not,
		// Token: 0x04006138 RID: 24888
		Xor,
		// Token: 0x04006139 RID: 24889
		CustomSingle,
		// Token: 0x0400613A RID: 24890
		Multiplexer,
		// Token: 0x0400613B RID: 24891
		Demultiplexer
	}
}
