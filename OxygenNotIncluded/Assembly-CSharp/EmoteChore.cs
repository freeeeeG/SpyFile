using System;
using Klei.AI;
using UnityEngine;

// Token: 0x020003AF RID: 943
public class EmoteChore : Chore<EmoteChore.StatesInstance>
{
	// Token: 0x060013A0 RID: 5024 RVA: 0x00068640 File Offset: 0x00066840
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, Emote emote, int emoteIterations = 1, Func<StatusItem> get_status_item = null) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, emote, KAnim.PlayMode.Once, emoteIterations, false);
		this.getStatusItem = get_status_item;
	}

	// Token: 0x060013A1 RID: 5025 RVA: 0x00068688 File Offset: 0x00066888
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, Emote emote, KAnim.PlayMode play_mode, int emoteIterations = 1, bool flip_x = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, emote, play_mode, emoteIterations, flip_x);
	}

	// Token: 0x060013A2 RID: 5026 RVA: 0x000686C8 File Offset: 0x000668C8
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString animFile, HashedString[] anims, Func<StatusItem> get_status_item = null) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, animFile, anims, KAnim.PlayMode.Once, false);
		this.getStatusItem = get_status_item;
	}

	// Token: 0x060013A3 RID: 5027 RVA: 0x00068710 File Offset: 0x00066910
	public EmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString animFile, HashedString[] anims, KAnim.PlayMode play_mode, bool flip_x = false) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.smi = new EmoteChore.StatesInstance(this, target.gameObject, animFile, anims, play_mode, flip_x);
	}

	// Token: 0x060013A4 RID: 5028 RVA: 0x00068750 File Offset: 0x00066950
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x060013A5 RID: 5029 RVA: 0x0006876C File Offset: 0x0006696C
	public override string ToString()
	{
		if (base.smi.animFile != null)
		{
			return "EmoteChore<" + base.smi.animFile.GetData().name + ">";
		}
		string str = "EmoteChore<";
		HashedString hashedString = base.smi.emoteAnims[0];
		return str + hashedString.ToString() + ">";
	}

	// Token: 0x060013A6 RID: 5030 RVA: 0x000687DF File Offset: 0x000669DF
	public void PairReactable(SelfEmoteReactable reactable)
	{
		this.reactable = reactable;
	}

	// Token: 0x060013A7 RID: 5031 RVA: 0x000687E8 File Offset: 0x000669E8
	protected new virtual void End(string reason)
	{
		if (this.reactable != null)
		{
			this.reactable.PairEmote(null);
			this.reactable.Cleanup();
			this.reactable = null;
		}
		base.End(reason);
	}

	// Token: 0x04000AA9 RID: 2729
	private Func<StatusItem> getStatusItem;

	// Token: 0x04000AAA RID: 2730
	private SelfEmoteReactable reactable;

	// Token: 0x02000FF0 RID: 4080
	public class StatesInstance : GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.GameInstance
	{
		// Token: 0x0600742B RID: 29739 RVA: 0x002C5B88 File Offset: 0x002C3D88
		public StatesInstance(EmoteChore master, GameObject emoter, Emote emote, KAnim.PlayMode mode, int emoteIterations, bool flip_x) : base(master)
		{
			this.mode = mode;
			this.animFile = emote.AnimSet;
			emote.CollectStepAnims(out this.emoteAnims, emoteIterations);
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x0600742C RID: 29740 RVA: 0x002C5BE0 File Offset: 0x002C3DE0
		public StatesInstance(EmoteChore master, GameObject emoter, HashedString animFile, HashedString[] anims, KAnim.PlayMode mode, bool flip_x) : base(master)
		{
			this.mode = mode;
			this.animFile = Assets.GetAnim(animFile);
			this.emoteAnims = anims;
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x04005795 RID: 22421
		public KAnimFile animFile;

		// Token: 0x04005796 RID: 22422
		public HashedString[] emoteAnims;

		// Token: 0x04005797 RID: 22423
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x02000FF1 RID: 4081
	public class States : GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore>
	{
		// Token: 0x0600742D RID: 29741 RVA: 0x002C5C30 File Offset: 0x002C3E30
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleAnims((EmoteChore.StatesInstance smi) => smi.animFile).PlayAnims((EmoteChore.StatesInstance smi) => smi.emoteAnims, (EmoteChore.StatesInstance smi) => smi.mode).ScheduleGoTo(10f, this.finish).OnAnimQueueComplete(this.finish);
			this.finish.ReturnSuccess();
		}

		// Token: 0x04005798 RID: 22424
		public StateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.TargetParameter emoter;

		// Token: 0x04005799 RID: 22425
		public GameStateMachine<EmoteChore.States, EmoteChore.StatesInstance, EmoteChore, object>.State finish;
	}
}
