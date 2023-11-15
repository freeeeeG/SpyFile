using System;
using UnityEngine;

// Token: 0x020003BA RID: 954
public class MoveChore : Chore<MoveChore.StatesInstance>
{
	// Token: 0x060013E1 RID: 5089 RVA: 0x00069D70 File Offset: 0x00067F70
	public MoveChore(IStateMachineTarget target, ChoreType chore_type, Func<MoveChore.StatesInstance, int> get_cell_callback, bool update_cell = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new MoveChore.StatesInstance(this, target.gameObject, get_cell_callback, update_cell);
	}

	// Token: 0x0200100B RID: 4107
	public class StatesInstance : GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.GameInstance
	{
		// Token: 0x06007484 RID: 29828 RVA: 0x002C88D0 File Offset: 0x002C6AD0
		public StatesInstance(MoveChore master, GameObject mover, Func<MoveChore.StatesInstance, int> get_cell_callback, bool update_cell = false) : base(master)
		{
			this.getCellCallback = get_cell_callback;
			base.sm.mover.Set(mover, base.smi, false);
		}

		// Token: 0x040057FD RID: 22525
		public Func<MoveChore.StatesInstance, int> getCellCallback;
	}

	// Token: 0x0200100C RID: 4108
	public class States : GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore>
	{
		// Token: 0x06007485 RID: 29829 RVA: 0x002C88FC File Offset: 0x002C6AFC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.approach;
			base.Target(this.mover);
			this.root.MoveTo((MoveChore.StatesInstance smi) => smi.getCellCallback(smi), null, null, false);
		}

		// Token: 0x040057FE RID: 22526
		public GameStateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.ApproachSubState<IApproachable> approach;

		// Token: 0x040057FF RID: 22527
		public StateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.TargetParameter mover;

		// Token: 0x04005800 RID: 22528
		public StateMachine<MoveChore.States, MoveChore.StatesInstance, MoveChore, object>.TargetParameter locator;
	}
}
