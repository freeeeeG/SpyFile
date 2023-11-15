using System;

// Token: 0x02000872 RID: 2162
public class DebugGoToMonitor : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>
{
	// Token: 0x06003F30 RID: 16176 RVA: 0x00160A24 File Offset: 0x0015EC24
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.DoNothing();
		this.hastarget.ToggleChore((DebugGoToMonitor.Instance smi) => new MoveChore(smi.master, Db.Get().ChoreTypes.DebugGoTo, (MoveChore.StatesInstance smii) => smi.targetCellIndex, false), this.satisfied);
	}

	// Token: 0x040028F0 RID: 10480
	public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State satisfied;

	// Token: 0x040028F1 RID: 10481
	public GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.State hastarget;

	// Token: 0x0200165F RID: 5727
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001660 RID: 5728
	public new class Instance : GameStateMachine<DebugGoToMonitor, DebugGoToMonitor.Instance, IStateMachineTarget, DebugGoToMonitor.Def>.GameInstance
	{
		// Token: 0x06008A89 RID: 35465 RVA: 0x00313F74 File Offset: 0x00312174
		public Instance(IStateMachineTarget target, DebugGoToMonitor.Def def) : base(target, def)
		{
		}

		// Token: 0x06008A8A RID: 35466 RVA: 0x00313F8C File Offset: 0x0031218C
		public void GoToCursor()
		{
			this.targetCellIndex = DebugHandler.GetMouseCell();
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		// Token: 0x06008A8B RID: 35467 RVA: 0x00313FDC File Offset: 0x003121DC
		public void GoToCell(int cellIndex)
		{
			this.targetCellIndex = cellIndex;
			if (base.smi.GetCurrentState() == base.smi.sm.satisfied)
			{
				base.smi.GoTo(base.smi.sm.hastarget);
			}
		}

		// Token: 0x04006B68 RID: 27496
		public int targetCellIndex = Grid.InvalidCell;
	}
}
