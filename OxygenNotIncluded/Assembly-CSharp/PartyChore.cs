using System;
using Klei.AI;
using TUNING;

// Token: 0x020003BE RID: 958
public class PartyChore : Chore<PartyChore.StatesInstance>, IWorkerPrioritizable
{
	// Token: 0x060013E8 RID: 5096 RVA: 0x0006A160 File Offset: 0x00068360
	public PartyChore(IStateMachineTarget master, Workable chat_workable, Action<Chore> on_complete = null, Action<Chore> on_begin = null, Action<Chore> on_end = null) : base(Db.Get().ChoreTypes.Party, master, master.GetComponent<ChoreProvider>(), true, on_complete, on_begin, on_end, PriorityScreen.PriorityClass.high, 5, false, true, 0, false, ReportManager.ReportType.PersonalTime)
	{
		base.smi = new PartyChore.StatesInstance(this);
		base.smi.sm.chitchatlocator.Set(chat_workable, base.smi);
		base.AddPrecondition(ChorePreconditions.instance.CanMoveTo, chat_workable);
		base.AddPrecondition(ChorePreconditions.instance.IsNotRedAlert, null);
	}

	// Token: 0x060013E9 RID: 5097 RVA: 0x0006A1EC File Offset: 0x000683EC
	public override void Begin(Chore.Precondition.Context context)
	{
		base.smi.sm.partyer.Set(context.consumerState.gameObject, base.smi, false);
		base.Begin(context);
		base.smi.sm.partyer.Get(base.smi).gameObject.AddTag(GameTags.Partying);
	}

	// Token: 0x060013EA RID: 5098 RVA: 0x0006A254 File Offset: 0x00068454
	protected override void End(string reason)
	{
		if (base.smi.sm.partyer.Get(base.smi) != null)
		{
			base.smi.sm.partyer.Get(base.smi).gameObject.RemoveTag(GameTags.Partying);
		}
		base.End(reason);
	}

	// Token: 0x060013EB RID: 5099 RVA: 0x0006A2B5 File Offset: 0x000684B5
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		return true;
	}

	// Token: 0x04000AC0 RID: 2752
	public int basePriority = RELAXATION.PRIORITY.SPECIAL_EVENT;

	// Token: 0x04000AC1 RID: 2753
	public const string specificEffect = "Socialized";

	// Token: 0x04000AC2 RID: 2754
	public const string trackingEffect = "RecentlySocialized";

	// Token: 0x02001013 RID: 4115
	public class States : GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore>
	{
		// Token: 0x0600749A RID: 29850 RVA: 0x002C9054 File Offset: 0x002C7254
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.stand;
			base.Target(this.partyer);
			this.stand.InitializeStates(this.partyer, this.masterTarget, this.chat, null, null, null);
			this.chat_move.InitializeStates(this.partyer, this.chitchatlocator, this.chat, null, null, null);
			this.chat.ToggleWork<Workable>(this.chitchatlocator, this.success, null, null);
			this.success.Enter(delegate(PartyChore.StatesInstance smi)
			{
				this.partyer.Get(smi).gameObject.GetComponent<Effects>().Add("RecentlyPartied", true);
			}).ReturnSuccess();
		}

		// Token: 0x04005816 RID: 22550
		public StateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.TargetParameter partyer;

		// Token: 0x04005817 RID: 22551
		public StateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.TargetParameter chitchatlocator;

		// Token: 0x04005818 RID: 22552
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.ApproachSubState<IApproachable> stand;

		// Token: 0x04005819 RID: 22553
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.ApproachSubState<IApproachable> chat_move;

		// Token: 0x0400581A RID: 22554
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.State chat;

		// Token: 0x0400581B RID: 22555
		public GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.State success;
	}

	// Token: 0x02001014 RID: 4116
	public class StatesInstance : GameStateMachine<PartyChore.States, PartyChore.StatesInstance, PartyChore, object>.GameInstance
	{
		// Token: 0x0600749D RID: 29853 RVA: 0x002C911A File Offset: 0x002C731A
		public StatesInstance(PartyChore master) : base(master)
		{
		}
	}
}
