using System;
using STRINGS;

// Token: 0x020000DA RID: 218
public class RobotDeathStates : GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>
{
	// Token: 0x060003E3 RID: 995 RVA: 0x0001E1F8 File Offset: 0x0001C3F8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.ToggleStatusItem(CREATURES.STATUSITEMS.DEAD.NAME, CREATURES.STATUSITEMS.DEAD.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).PlayAnim("death").OnAnimQueueComplete(this.pst);
		this.pst.TriggerOnEnter(GameHashes.DeathAnimComplete, null).TriggerOnEnter(GameHashes.Died, null).BehaviourComplete(GameTags.Creatures.Die, false);
	}

	// Token: 0x04000291 RID: 657
	private GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State loop;

	// Token: 0x04000292 RID: 658
	private GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.State pst;

	// Token: 0x02000F15 RID: 3861
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F16 RID: 3862
	public new class Instance : GameStateMachine<RobotDeathStates, RobotDeathStates.Instance, IStateMachineTarget, RobotDeathStates.Def>.GameInstance
	{
		// Token: 0x06007101 RID: 28929 RVA: 0x002BC056 File Offset: 0x002BA256
		public Instance(Chore<RobotDeathStates.Instance> chore, RobotDeathStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Die);
		}
	}
}
