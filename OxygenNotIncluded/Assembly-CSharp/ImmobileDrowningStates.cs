using System;
using STRINGS;

// Token: 0x020000D1 RID: 209
public class ImmobileDrowningStates : GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>
{
	// Token: 0x060003BC RID: 956 RVA: 0x0001CFCC File Offset: 0x0001B1CC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.drown;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.DROWNING.NAME, CREATURES.STATUSITEMS.DROWNING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).TagTransition(GameTags.Creatures.Drowning, this.drown_pst, true);
		this.drown.PlayAnim("drown_pre").QueueAnim("drown_loop", true, null);
		this.drown_pst.PlayAnim("drown_pst").OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.Drowning, false);
	}

	// Token: 0x04000277 RID: 631
	public GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State drown;

	// Token: 0x04000278 RID: 632
	public GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State drown_pst;

	// Token: 0x04000279 RID: 633
	public GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.State behaviourcomplete;

	// Token: 0x02000EF8 RID: 3832
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EF9 RID: 3833
	public new class Instance : GameStateMachine<ImmobileDrowningStates, ImmobileDrowningStates.Instance, IStateMachineTarget, ImmobileDrowningStates.Def>.GameInstance
	{
		// Token: 0x060070AE RID: 28846 RVA: 0x002BB2FA File Offset: 0x002B94FA
		public Instance(Chore<ImmobileDrowningStates.Instance> chore, ImmobileDrowningStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Creatures.Drowning);
		}
	}
}
