using System;

// Token: 0x0200089E RID: 2206
public class TiredMonitor : GameStateMachine<TiredMonitor, TiredMonitor.Instance>
{
	// Token: 0x06004000 RID: 16384 RVA: 0x00166478 File Offset: 0x00164678
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.EventTransition(GameHashes.SleepFail, this.tired, null);
		this.tired.Enter(delegate(TiredMonitor.Instance smi)
		{
			smi.SetInterruptDay();
		}).EventTransition(GameHashes.NewDay, (TiredMonitor.Instance smi) => GameClock.Instance, this.root, (TiredMonitor.Instance smi) => smi.AllowInterruptClear()).ToggleExpression(Db.Get().Expressions.Tired, null).ToggleAnims("anim_loco_walk_slouch_kanim", 0f, "").ToggleAnims("anim_idle_slouch_kanim", 0f, "");
	}

	// Token: 0x040029AF RID: 10671
	public GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.State tired;

	// Token: 0x020016CE RID: 5838
	public new class Instance : GameStateMachine<TiredMonitor, TiredMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008C80 RID: 35968 RVA: 0x003190A0 File Offset: 0x003172A0
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008C81 RID: 35969 RVA: 0x003190B7 File Offset: 0x003172B7
		public void SetInterruptDay()
		{
			this.interruptedDay = GameClock.Instance.GetCycle();
		}

		// Token: 0x06008C82 RID: 35970 RVA: 0x003190C9 File Offset: 0x003172C9
		public bool AllowInterruptClear()
		{
			bool flag = GameClock.Instance.GetCycle() > this.interruptedDay + 1;
			if (flag)
			{
				this.interruptedDay = -1;
			}
			return flag;
		}

		// Token: 0x04006CFA RID: 27898
		public int disturbedDay = -1;

		// Token: 0x04006CFB RID: 27899
		public int interruptedDay = -1;
	}
}
