using System;

// Token: 0x02000877 RID: 2167
public class EmoteHighPriorityMonitor : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance>
{
	// Token: 0x06003F3F RID: 16191 RVA: 0x00161064 File Offset: 0x0015F264
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.ready;
		base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
		this.ready.ToggleUrge(Db.Get().Urges.EmoteHighPriority).EventHandler(GameHashes.BeginChore, delegate(EmoteHighPriorityMonitor.Instance smi, object o)
		{
			smi.OnStartChore(o);
		});
		this.resetting.GoTo(this.ready);
	}

	// Token: 0x040028F9 RID: 10489
	public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State ready;

	// Token: 0x040028FA RID: 10490
	public GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.State resetting;

	// Token: 0x0200166D RID: 5741
	public new class Instance : GameStateMachine<EmoteHighPriorityMonitor, EmoteHighPriorityMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008ABA RID: 35514 RVA: 0x003147BC File Offset: 0x003129BC
		public Instance(IStateMachineTarget master) : base(master)
		{
		}

		// Token: 0x06008ABB RID: 35515 RVA: 0x003147C5 File Offset: 0x003129C5
		public void OnStartChore(object o)
		{
			if (((Chore)o).SatisfiesUrge(Db.Get().Urges.EmoteHighPriority))
			{
				this.GoTo(base.sm.resetting);
			}
		}
	}
}
