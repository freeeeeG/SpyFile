using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000631 RID: 1585
[SkipSaveFileSerialization]
public class LogicGateVisualizer : LogicGateBase
{
	// Token: 0x060028A8 RID: 10408 RVA: 0x000DD083 File Offset: 0x000DB283
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
	}

	// Token: 0x060028A9 RID: 10409 RVA: 0x000DD091 File Offset: 0x000DB291
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		this.Unregister();
	}

	// Token: 0x060028AA RID: 10410 RVA: 0x000DD0A0 File Offset: 0x000DB2A0
	private void Register()
	{
		this.Unregister();
		this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellOne, false));
		if (base.RequiresFourOutputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellTwo, false));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellThree, false));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.OutputCellFour, false));
		}
		this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellOne, true));
		if (base.RequiresTwoInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellTwo, true));
		}
		else if (base.RequiresFourInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellTwo, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellThree, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.InputCellFour, true));
		}
		if (base.RequiresControlInputs)
		{
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.ControlCellOne, true));
			this.visChildren.Add(new LogicGateVisualizer.IOVisualizer(base.ControlCellTwo, true));
		}
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		foreach (LogicGateVisualizer.IOVisualizer elem in this.visChildren)
		{
			logicCircuitManager.AddVisElem(elem);
		}
	}

	// Token: 0x060028AB RID: 10411 RVA: 0x000DD224 File Offset: 0x000DB424
	private void Unregister()
	{
		LogicCircuitManager logicCircuitManager = Game.Instance.logicCircuitManager;
		foreach (LogicGateVisualizer.IOVisualizer elem in this.visChildren)
		{
			logicCircuitManager.RemoveVisElem(elem);
		}
		this.visChildren.Clear();
	}

	// Token: 0x040017F3 RID: 6131
	private List<LogicGateVisualizer.IOVisualizer> visChildren = new List<LogicGateVisualizer.IOVisualizer>();

	// Token: 0x020012FD RID: 4861
	private class IOVisualizer : ILogicUIElement, IUniformGridObject
	{
		// Token: 0x06007F33 RID: 32563 RVA: 0x002EB9F7 File Offset: 0x002E9BF7
		public IOVisualizer(int cell, bool input)
		{
			this.cell = cell;
			this.input = input;
		}

		// Token: 0x06007F34 RID: 32564 RVA: 0x002EBA0D File Offset: 0x002E9C0D
		public int GetLogicUICell()
		{
			return this.cell;
		}

		// Token: 0x06007F35 RID: 32565 RVA: 0x002EBA15 File Offset: 0x002E9C15
		public LogicPortSpriteType GetLogicPortSpriteType()
		{
			if (!this.input)
			{
				return LogicPortSpriteType.Output;
			}
			return LogicPortSpriteType.Input;
		}

		// Token: 0x06007F36 RID: 32566 RVA: 0x002EBA22 File Offset: 0x002E9C22
		public Vector2 PosMin()
		{
			return Grid.CellToPos2D(this.cell);
		}

		// Token: 0x06007F37 RID: 32567 RVA: 0x002EBA34 File Offset: 0x002E9C34
		public Vector2 PosMax()
		{
			return this.PosMin();
		}

		// Token: 0x04006147 RID: 24903
		private int cell;

		// Token: 0x04006148 RID: 24904
		private bool input;
	}
}
