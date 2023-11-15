using System;
using STRINGS;

// Token: 0x020000DF RID: 223
public class StunnedStates : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>
{
	// Token: 0x0600040E RID: 1038 RVA: 0x0001F79C File Offset: 0x0001D99C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.stunned;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.GETTING_WRANGLED.NAME, CREATURES.STATUSITEMS.GETTING_WRANGLED.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.stunned.PlayAnim("idle_loop", KAnim.PlayMode.Loop).TagTransition(GameTags.Creatures.Stunned, null, true);
	}

	// Token: 0x040002A5 RID: 677
	public GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.State stunned;

	// Token: 0x02000F27 RID: 3879
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F28 RID: 3880
	public new class Instance : GameStateMachine<StunnedStates, StunnedStates.Instance, IStateMachineTarget, StunnedStates.Def>.GameInstance
	{
		// Token: 0x06007132 RID: 28978 RVA: 0x002BCB22 File Offset: 0x002BAD22
		public Instance(Chore<StunnedStates.Instance> chore, StunnedStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(StunnedStates.Instance.IsStunned, null);
		}

		// Token: 0x0400552D RID: 21805
		public static readonly Chore.Precondition IsStunned = new Chore.Precondition
		{
			id = "IsStunned",
			fn = delegate(ref Chore.Precondition.Context context, object data)
			{
				return context.consumerState.prefabid.HasTag(GameTags.Creatures.Stunned);
			}
		};
	}
}
