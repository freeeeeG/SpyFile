using System;
using Klei.AI;

// Token: 0x02000868 RID: 2152
public class BladderMonitor : GameStateMachine<BladderMonitor, BladderMonitor.Instance>
{
	// Token: 0x06003F01 RID: 16129 RVA: 0x0015F794 File Offset: 0x0015D994
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.satisfied.Transition(this.urgentwant, (BladderMonitor.Instance smi) => smi.NeedsToPee(), UpdateRate.SIM_200ms).Transition(this.breakwant, (BladderMonitor.Instance smi) => smi.WantsToPee(), UpdateRate.SIM_200ms);
		this.urgentwant.InitializeStates(this.satisfied).ToggleThought(Db.Get().Thoughts.FullBladder, null).ToggleExpression(Db.Get().Expressions.FullBladder, null).ToggleStateMachine((BladderMonitor.Instance smi) => new PeeChoreMonitor.Instance(smi.master)).ToggleEffect("FullBladder");
		this.breakwant.InitializeStates(this.satisfied);
		this.breakwant.wanting.Transition(this.urgentwant, (BladderMonitor.Instance smi) => smi.NeedsToPee(), UpdateRate.SIM_200ms).EventTransition(GameHashes.ScheduleBlocksChanged, this.satisfied, (BladderMonitor.Instance smi) => !smi.WantsToPee());
		this.breakwant.peeing.ToggleThought(Db.Get().Thoughts.BreakBladder, null);
	}

	// Token: 0x040028CB RID: 10443
	public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State satisfied;

	// Token: 0x040028CC RID: 10444
	public BladderMonitor.WantsToPeeStates urgentwant;

	// Token: 0x040028CD RID: 10445
	public BladderMonitor.WantsToPeeStates breakwant;

	// Token: 0x02001643 RID: 5699
	public class WantsToPeeStates : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State
	{
		// Token: 0x06008A21 RID: 35361 RVA: 0x0031335C File Offset: 0x0031155C
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State InitializeStates(GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State donePeeingState)
		{
			base.DefaultState(this.wanting).ToggleUrge(Db.Get().Urges.Pee).ToggleStateMachine((BladderMonitor.Instance smi) => new ToiletMonitor.Instance(smi.master));
			this.wanting.EventTransition(GameHashes.BeginChore, this.peeing, (BladderMonitor.Instance smi) => smi.IsPeeing());
			this.peeing.EventTransition(GameHashes.EndChore, donePeeingState, (BladderMonitor.Instance smi) => !smi.IsPeeing());
			return this;
		}

		// Token: 0x04006B23 RID: 27427
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State wanting;

		// Token: 0x04006B24 RID: 27428
		public GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.State peeing;
	}

	// Token: 0x02001644 RID: 5700
	public new class Instance : GameStateMachine<BladderMonitor, BladderMonitor.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06008A23 RID: 35363 RVA: 0x0031341E File Offset: 0x0031161E
		public Instance(IStateMachineTarget master) : base(master)
		{
			this.bladder = Db.Get().Amounts.Bladder.Lookup(master.gameObject);
			this.choreDriver = base.GetComponent<ChoreDriver>();
		}

		// Token: 0x06008A24 RID: 35364 RVA: 0x00313454 File Offset: 0x00311654
		public bool NeedsToPee()
		{
			DebugUtil.DevAssert(base.master != null, "master ref null", null);
			DebugUtil.DevAssert(!base.master.isNull, "master isNull", null);
			KPrefabID component = base.master.GetComponent<KPrefabID>();
			DebugUtil.DevAssert(component, "kpid was null", null);
			return !component.HasTag(GameTags.Asleep) && this.bladder.value >= 100f;
		}

		// Token: 0x06008A25 RID: 35365 RVA: 0x003134CD File Offset: 0x003116CD
		public bool WantsToPee()
		{
			return this.NeedsToPee() || (this.IsPeeTime() && this.bladder.value >= 40f);
		}

		// Token: 0x06008A26 RID: 35366 RVA: 0x003134F8 File Offset: 0x003116F8
		public bool IsPeeing()
		{
			return this.choreDriver.HasChore() && this.choreDriver.GetCurrentChore().SatisfiesUrge(Db.Get().Urges.Pee);
		}

		// Token: 0x06008A27 RID: 35367 RVA: 0x00313528 File Offset: 0x00311728
		public bool IsPeeTime()
		{
			return base.master.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Hygiene);
		}

		// Token: 0x04006B25 RID: 27429
		private AmountInstance bladder;

		// Token: 0x04006B26 RID: 27430
		private ChoreDriver choreDriver;
	}
}
