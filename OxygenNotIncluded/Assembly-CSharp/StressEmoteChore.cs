using System;
using UnityEngine;

// Token: 0x020003C8 RID: 968
public class StressEmoteChore : Chore<StressEmoteChore.StatesInstance>
{
	// Token: 0x06001404 RID: 5124 RVA: 0x0006AC08 File Offset: 0x00068E08
	public StressEmoteChore(IStateMachineTarget target, ChoreType chore_type, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode play_mode, Func<StatusItem> get_status_item) : base(chore_type, target, target.GetComponent<ChoreProvider>(), false, null, null, null, PriorityScreen.PriorityClass.compulsory, 5, false, true, 0, false, ReportManager.ReportType.WorkTime)
	{
		base.AddPrecondition(ChorePreconditions.instance.IsMoving, null);
		base.AddPrecondition(ChorePreconditions.instance.IsOffLadder, null);
		base.AddPrecondition(ChorePreconditions.instance.NotInTube, null);
		base.AddPrecondition(ChorePreconditions.instance.IsAwake, null);
		this.getStatusItem = get_status_item;
		base.smi = new StressEmoteChore.StatesInstance(this, target.gameObject, emote_kanim, emote_anims, play_mode);
	}

	// Token: 0x06001405 RID: 5125 RVA: 0x0006AC92 File Offset: 0x00068E92
	protected override StatusItem GetStatusItem()
	{
		if (this.getStatusItem == null)
		{
			return base.GetStatusItem();
		}
		return this.getStatusItem();
	}

	// Token: 0x06001406 RID: 5126 RVA: 0x0006ACB0 File Offset: 0x00068EB0
	public override string ToString()
	{
		HashedString hashedString;
		if (base.smi.emoteKAnim.IsValid)
		{
			string str = "StressEmoteChore<";
			hashedString = base.smi.emoteKAnim;
			return str + hashedString.ToString() + ">";
		}
		string str2 = "StressEmoteChore<";
		hashedString = base.smi.emoteAnims[0];
		return str2 + hashedString.ToString() + ">";
	}

	// Token: 0x04000AC9 RID: 2761
	private Func<StatusItem> getStatusItem;

	// Token: 0x0200102B RID: 4139
	public class StatesInstance : GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.GameInstance
	{
		// Token: 0x060074F3 RID: 29939 RVA: 0x002CB24B File Offset: 0x002C944B
		public StatesInstance(StressEmoteChore master, GameObject emoter, HashedString emote_kanim, HashedString[] emote_anims, KAnim.PlayMode mode) : base(master)
		{
			this.emoteKAnim = emote_kanim;
			this.emoteAnims = emote_anims;
			this.mode = mode;
			base.sm.emoter.Set(emoter, base.smi, false);
		}

		// Token: 0x0400585C RID: 22620
		public HashedString[] emoteAnims;

		// Token: 0x0400585D RID: 22621
		public HashedString emoteKAnim;

		// Token: 0x0400585E RID: 22622
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}

	// Token: 0x0200102C RID: 4140
	public class States : GameStateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore>
	{
		// Token: 0x060074F4 RID: 29940 RVA: 0x002CB28C File Offset: 0x002C948C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.root;
			base.Target(this.emoter);
			this.root.ToggleAnims((StressEmoteChore.StatesInstance smi) => smi.emoteKAnim).ToggleThought(Db.Get().Thoughts.Unhappy, null).PlayAnims((StressEmoteChore.StatesInstance smi) => smi.emoteAnims, (StressEmoteChore.StatesInstance smi) => smi.mode).OnAnimQueueComplete(null);
		}

		// Token: 0x0400585F RID: 22623
		public StateMachine<StressEmoteChore.States, StressEmoteChore.StatesInstance, StressEmoteChore, object>.TargetParameter emoter;
	}
}
