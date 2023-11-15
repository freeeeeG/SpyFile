using System;
using STRINGS;

// Token: 0x020000B8 RID: 184
public class DebugGoToStates : GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>
{
	// Token: 0x06000352 RID: 850 RVA: 0x0001A6A0 File Offset: 0x000188A0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		this.moving.MoveTo(new Func<DebugGoToStates.Instance, int>(DebugGoToStates.GetTargetCell), this.behaviourcomplete, this.behaviourcomplete, true).ToggleStatusItem(CREATURES.STATUSITEMS.DEBUGGOTO.NAME, CREATURES.STATUSITEMS.DEBUGGOTO.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.behaviourcomplete.BehaviourComplete(GameTags.HasDebugDestination, false);
	}

	// Token: 0x06000353 RID: 851 RVA: 0x0001A72C File Offset: 0x0001892C
	private static int GetTargetCell(DebugGoToStates.Instance smi)
	{
		return smi.GetSMI<CreatureDebugGoToMonitor.Instance>().targetCell;
	}

	// Token: 0x0400023A RID: 570
	public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State moving;

	// Token: 0x0400023B RID: 571
	public GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.State behaviourcomplete;

	// Token: 0x02000EAD RID: 3757
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EAE RID: 3758
	public new class Instance : GameStateMachine<DebugGoToStates, DebugGoToStates.Instance, IStateMachineTarget, DebugGoToStates.Def>.GameInstance
	{
		// Token: 0x06007011 RID: 28689 RVA: 0x002BA3DF File Offset: 0x002B85DF
		public Instance(Chore<DebugGoToStates.Instance> chore, DebugGoToStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.HasDebugDestination);
		}
	}
}
