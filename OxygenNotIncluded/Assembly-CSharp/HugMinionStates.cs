using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000CD RID: 205
public class HugMinionStates : GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>
{
	// Token: 0x060003A9 RID: 937 RVA: 0x0001C818 File Offset: 0x0001AA18
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.moving;
		this.moving.MoveTo(new Func<HugMinionStates.Instance, int>(HugMinionStates.FindFlopLocation), this.waiting, this.behaviourcomplete, false);
		this.waiting.Enter(delegate(HugMinionStates.Instance smi)
		{
			smi.GetComponent<Navigator>().SetCurrentNavType(NavType.Floor);
		}).ParamTransition<float>(this.timeout, this.behaviourcomplete, (HugMinionStates.Instance smi, float p) => p > 60f && !smi.GetSMI<HugMonitor.Instance>().IsHugging()).Update(delegate(HugMinionStates.Instance smi, float dt)
		{
			smi.sm.timeout.Delta(dt, smi);
		}, UpdateRate.SIM_200ms, false).PlayAnim("waiting_pre").QueueAnim("waiting_loop", true, null).ToggleStatusItem(CREATURES.STATUSITEMS.HUGMINIONWAITING.NAME, CREATURES.STATUSITEMS.HUGMINIONWAITING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsAHug, false);
	}

	// Token: 0x060003AA RID: 938 RVA: 0x0001C93C File Offset: 0x0001AB3C
	private static int FindFlopLocation(HugMinionStates.Instance smi)
	{
		Navigator component = smi.GetComponent<Navigator>();
		FloorCellQuery floorCellQuery = PathFinderQueries.floorCellQuery.Reset(1, 1);
		component.RunQuery(floorCellQuery);
		if (floorCellQuery.result_cells.Count > 0)
		{
			smi.targetFlopCell = floorCellQuery.result_cells[UnityEngine.Random.Range(0, floorCellQuery.result_cells.Count)];
		}
		else
		{
			smi.targetFlopCell = Grid.InvalidCell;
		}
		return smi.targetFlopCell;
	}

	// Token: 0x04000270 RID: 624
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.ApproachSubState<EggIncubator> moving;

	// Token: 0x04000271 RID: 625
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State waiting;

	// Token: 0x04000272 RID: 626
	public GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.State behaviourcomplete;

	// Token: 0x04000273 RID: 627
	public StateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.FloatParameter timeout;

	// Token: 0x02000EEF RID: 3823
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EF0 RID: 3824
	public new class Instance : GameStateMachine<HugMinionStates, HugMinionStates.Instance, IStateMachineTarget, HugMinionStates.Def>.GameInstance
	{
		// Token: 0x06007096 RID: 28822 RVA: 0x002BB0BD File Offset: 0x002B92BD
		public Instance(Chore<HugMinionStates.Instance> chore, HugMinionStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsAHug);
		}

		// Token: 0x04005488 RID: 21640
		public int targetFlopCell;
	}
}
