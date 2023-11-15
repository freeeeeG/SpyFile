using System;
using UnityEngine;

// Token: 0x020003C9 RID: 969
public class StressIdleChore : Chore<StressIdleChore.StatesInstance>
{
	// Token: 0x06001407 RID: 5127 RVA: 0x0006AD28 File Offset: 0x00068F28
	public StressIdleChore(IStateMachineTarget target) : base(Db.Get().ChoreTypes.StressIdle, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.personalNeeds, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new StressIdleChore.StatesInstance(this, target.gameObject);
	}

	// Token: 0x0200102D RID: 4141
	public class StatesInstance : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.GameInstance
	{
		// Token: 0x060074F6 RID: 29942 RVA: 0x002CB33E File Offset: 0x002C953E
		public StatesInstance(StressIdleChore master, GameObject idler) : base(master)
		{
			base.sm.idler.Set(idler, base.smi, false);
		}
	}

	// Token: 0x0200102E RID: 4142
	public class States : GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore>
	{
		// Token: 0x060074F7 RID: 29943 RVA: 0x002CB360 File Offset: 0x002C9560
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			base.Target(this.idler);
			this.idle.PlayAnim("idle_default", KAnim.PlayMode.Loop);
		}

		// Token: 0x04005860 RID: 22624
		public StateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.TargetParameter idler;

		// Token: 0x04005861 RID: 22625
		public GameStateMachine<StressIdleChore.States, StressIdleChore.StatesInstance, StressIdleChore, object>.State idle;
	}
}
