using System;
using STRINGS;

// Token: 0x020000C0 RID: 192
public class ExitBurrowStates : GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>
{
	// Token: 0x06000379 RID: 889 RVA: 0x0001B458 File Offset: 0x00019658
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.exiting;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.EMERGING.NAME, CREATURES.STATUSITEMS.EMERGING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.exiting.PlayAnim("emerge").Enter(new StateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.State.Callback(ExitBurrowStates.MoveToCellAbove)).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToExitBurrow, false);
	}

	// Token: 0x0600037A RID: 890 RVA: 0x0001B4F3 File Offset: 0x000196F3
	private static void MoveToCellAbove(ExitBurrowStates.Instance smi)
	{
		smi.transform.SetPosition(Grid.CellToPosCBC(Grid.CellAbove(Grid.PosToCell(smi.transform.GetPosition())), Grid.SceneLayer.Creatures));
	}

	// Token: 0x04000252 RID: 594
	private GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.State exiting;

	// Token: 0x04000253 RID: 595
	private GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.State behaviourcomplete;

	// Token: 0x02000EC7 RID: 3783
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EC8 RID: 3784
	public new class Instance : GameStateMachine<ExitBurrowStates, ExitBurrowStates.Instance, IStateMachineTarget, ExitBurrowStates.Def>.GameInstance
	{
		// Token: 0x0600703C RID: 28732 RVA: 0x002BA709 File Offset: 0x002B8909
		public Instance(Chore<ExitBurrowStates.Instance> chore, ExitBurrowStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToExitBurrow);
		}
	}
}
