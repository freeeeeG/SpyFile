using System;

// Token: 0x0200086F RID: 2159
public class CreatureDebugGoToMonitor : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>
{
	// Token: 0x06003F23 RID: 16163 RVA: 0x00160586 File Offset: 0x0015E786
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.HasDebugDestination, new StateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.Transition.ConditionCallback(CreatureDebugGoToMonitor.HasTargetCell), new Action<CreatureDebugGoToMonitor.Instance>(CreatureDebugGoToMonitor.ClearTargetCell));
	}

	// Token: 0x06003F24 RID: 16164 RVA: 0x001605B9 File Offset: 0x0015E7B9
	private static bool HasTargetCell(CreatureDebugGoToMonitor.Instance smi)
	{
		return smi.targetCell != Grid.InvalidCell;
	}

	// Token: 0x06003F25 RID: 16165 RVA: 0x001605CB File Offset: 0x0015E7CB
	private static void ClearTargetCell(CreatureDebugGoToMonitor.Instance smi)
	{
		smi.targetCell = Grid.InvalidCell;
	}

	// Token: 0x02001657 RID: 5719
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02001658 RID: 5720
	public new class Instance : GameStateMachine<CreatureDebugGoToMonitor, CreatureDebugGoToMonitor.Instance, IStateMachineTarget, CreatureDebugGoToMonitor.Def>.GameInstance
	{
		// Token: 0x06008A6C RID: 35436 RVA: 0x00313C4A File Offset: 0x00311E4A
		public Instance(IStateMachineTarget target, CreatureDebugGoToMonitor.Def def) : base(target, def)
		{
		}

		// Token: 0x06008A6D RID: 35437 RVA: 0x00313C5F File Offset: 0x00311E5F
		public void GoToCursor()
		{
			this.targetCell = DebugHandler.GetMouseCell();
		}

		// Token: 0x06008A6E RID: 35438 RVA: 0x00313C6C File Offset: 0x00311E6C
		public void GoToCell(int cellIndex)
		{
			this.targetCell = cellIndex;
		}

		// Token: 0x04006B58 RID: 27480
		public int targetCell = Grid.InvalidCell;
	}
}
