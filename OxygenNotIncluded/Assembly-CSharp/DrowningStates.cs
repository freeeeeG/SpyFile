using System;
using STRINGS;

// Token: 0x020000BE RID: 190
public class DrowningStates : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>
{
	// Token: 0x0600036F RID: 879 RVA: 0x0001B03C File Offset: 0x0001923C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.drown;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.DROWNING.NAME, CREATURES.STATUSITEMS.DROWNING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main).TagTransition(GameTags.Creatures.Drowning, null, true);
		this.drown.PlayAnim("drown_pre").QueueAnim("drown_loop", true, null).Transition(this.drown_pst, new StateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.Transition.ConditionCallback(this.UpdateSafeCell), UpdateRate.SIM_1000ms);
		this.drown_pst.PlayAnim("drown_pst").OnAnimQueueComplete(this.move_to_safe);
		this.move_to_safe.MoveTo((DrowningStates.Instance smi) => smi.safeCell, null, null, false);
	}

	// Token: 0x06000370 RID: 880 RVA: 0x0001B124 File Offset: 0x00019324
	public bool UpdateSafeCell(DrowningStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		DrowningStates.EscapeCellQuery escapeCellQuery = new DrowningStates.EscapeCellQuery(smi.GetComponent<DrowningMonitor>());
		component.RunQuery(escapeCellQuery);
		smi.safeCell = escapeCellQuery.GetResultCell();
		return smi.safeCell != Grid.InvalidCell;
	}

	// Token: 0x0400024B RID: 587
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown;

	// Token: 0x0400024C RID: 588
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State drown_pst;

	// Token: 0x0400024D RID: 589
	public GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.State move_to_safe;

	// Token: 0x02000EC0 RID: 3776
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EC1 RID: 3777
	public new class Instance : GameStateMachine<DrowningStates, DrowningStates.Instance, IStateMachineTarget, DrowningStates.Def>.GameInstance
	{
		// Token: 0x06007031 RID: 28721 RVA: 0x002BA65D File Offset: 0x002B885D
		public Instance(Chore<DrowningStates.Instance> chore, DrowningStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.HasTag, GameTags.Creatures.Drowning);
		}

		// Token: 0x04005439 RID: 21561
		public int safeCell = Grid.InvalidCell;
	}

	// Token: 0x02000EC2 RID: 3778
	public class EscapeCellQuery : PathFinderQuery
	{
		// Token: 0x06007032 RID: 28722 RVA: 0x002BA68C File Offset: 0x002B888C
		public EscapeCellQuery(DrowningMonitor monitor)
		{
			this.monitor = monitor;
		}

		// Token: 0x06007033 RID: 28723 RVA: 0x002BA69B File Offset: 0x002B889B
		public override bool IsMatch(int cell, int parent_cell, int cost)
		{
			return this.monitor.IsCellSafe(cell);
		}

		// Token: 0x0400543A RID: 21562
		private DrowningMonitor monitor;
	}
}
