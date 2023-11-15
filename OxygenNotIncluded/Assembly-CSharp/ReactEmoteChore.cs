using System;
using UnityEngine;

// Token: 0x020003C2 RID: 962
public class ReactEmoteChore : Chore<ReactEmoteChore.StatesInstance>
{
	// Token: 0x060013F1 RID: 5105 RVA: 0x0006A4EC File Offset: 0x000686EC
	public ReactEmoteChore(IStateMachineTarget target, ChoreType chore_type, EmoteReactable reactable, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode play_mode, Func<StatusItem> get_status_item) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.basic, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.AddPrecondition(ChorePreconditions.instance.IsMoving, null);
		base.AddPrecondition(ChorePreconditions.instance.IsOffLadder, null);
		base.AddPrecondition(ChorePreconditions.instance.NotInTube, null);
		base.AddPrecondition(ChorePreconditions.instance.IsAwake, null);
		this.getStatusItem = get_status_item;
		base.smi = new ReactEmoteChore.StatesInstance(this, target.gameObject, reactable, emote_kanim, emote_anims, play_mode);
	}

	// Token: 0x060013F2 RID: 5106 RVA: 0x0006A578 File Offset: 0x00068778
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x060013F3 RID: 5107 RVA: 0x0006A594 File Offset: 0x00068794
	public override string ToString()
	{
		HashedString hashedString;
		if (base.smi.emoteKAnim.IsValid)
		{
			string str = "ReactEmoteChore<";
			hashedString = base.smi.emoteKAnim;
			return str + hashedString.ToString() + ">";
		}
		string str2 = "ReactEmoteChore<";
		hashedString = base.smi.emoteAnims[0];
		return str2 + hashedString.ToString() + ">";
	}

	// Token: 0x04000AC4 RID: 2756
	private Func<StatusItem> getStatusItem;

	// Token: 0x0200101C RID: 4124
	public class StatesInstance : GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.GameInstance
	{
		// Token: 0x060074B6 RID: 29878 RVA: 0x002C99D0 File Offset: 0x002C7BD0
		public StatesInstance(ReactEmoteChore master, GameObject emoter, EmoteReactable reactable, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode mode) : base(master)
		{
			this.emoteKAnim = emote_kanim;
			this.emoteAnims = emote_anims;
			this.mode = mode;
			base.sm.reactable.Set(reactable, base.smi, false);
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x0400582D RID: 22573
		public HashedString[] emoteAnims;

		// Token: 0x0400582E RID: 22574
		public HashedString emoteKAnim;

		// Token: 0x0400582F RID: 22575
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x0200101D RID: 4125
	public class States : GameStateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore>
	{
		// Token: 0x060074B7 RID: 29879 RVA: 0x002C9A38 File Offset: 0x002C7C38
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleThought((ReactEmoteChore.StatesInstance smi) => this.reactable.Get(smi).thought).ToggleExpression((ReactEmoteChore.StatesInstance smi) => this.reactable.Get(smi).expression).ToggleAnims((ReactEmoteChore.StatesInstance smi) => smi.emoteKAnim).ToggleThought(Db.Get().Thoughts.Unhappy, null).PlayAnims((ReactEmoteChore.StatesInstance smi) => smi.emoteAnims, (ReactEmoteChore.StatesInstance smi) => smi.mode).OnAnimQueueComplete(null).Enter(delegate(ReactEmoteChore.StatesInstance smi)
			{
				smi.master.GetComponent<Facing>().Face(Grid.CellToPos(this.reactable.Get(smi).sourceCell));
			});
		}

		// Token: 0x04005830 RID: 22576
		public StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.TargetParameter emoter;

		// Token: 0x04005831 RID: 22577
		public StateMachine<ReactEmoteChore.States, ReactEmoteChore.StatesInstance, ReactEmoteChore, object>.ObjectParameter<EmoteReactable> reactable;
	}
}
