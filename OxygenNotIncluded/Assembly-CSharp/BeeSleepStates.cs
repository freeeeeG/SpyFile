using System;
using STRINGS;
using UnityEngine;

// Token: 0x020000AE RID: 174
public class BeeSleepStates : GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>
{
	// Token: 0x06000316 RID: 790 RVA: 0x00018A60 File Offset: 0x00016C60
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findSleepLocation;
		this.root.ToggleStatusItem(CREATURES.STATUSITEMS.SLEEPING.NAME, CREATURES.STATUSITEMS.SLEEPING.TOOLTIP, "", StatusItem.IconType.Info, NotificationType.Neutral, false, default(HashedString), 129022, null, null, Db.Get().StatusItemCategories.Main);
		this.findSleepLocation.Enter(delegate(BeeSleepStates.Instance smi)
		{
			BeeSleepStates.FindSleepLocation(smi);
			if (smi.targetSleepCell != Grid.InvalidCell)
			{
				smi.GoTo(this.moveToSleepLocation);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.moveToSleepLocation.MoveTo((BeeSleepStates.Instance smi) => smi.targetSleepCell, this.sleep.pre, this.behaviourcomplete, false);
		this.sleep.Enter("EnableGravity", delegate(BeeSleepStates.Instance smi)
		{
			GameComps.Gravities.Add(smi.gameObject, Vector2.zero, delegate()
			{
				if (GameComps.Gravities.Has(smi.gameObject))
				{
					GameComps.Gravities.Remove(smi.gameObject);
				}
			});
		}).TriggerOnEnter(GameHashes.SleepStarted, null).TriggerOnExit(GameHashes.SleepFinished, null).Transition(this.sleep.pst, new StateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.Transition.ConditionCallback(BeeSleepStates.ShouldWakeUp), UpdateRate.SIM_1000ms);
		this.sleep.pre.QueueAnim("sleep_pre", false, null).OnAnimQueueComplete(this.sleep.loop);
		this.sleep.loop.Enter(delegate(BeeSleepStates.Instance smi)
		{
			smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Bee_wings_LP", false), true);
		}).QueueAnim("sleep_loop", true, null).Exit(delegate(BeeSleepStates.Instance smi)
		{
			smi.GetComponent<LoopingSounds>().PauseSound(GlobalAssets.GetSound("Bee_wings_LP", false), false);
		});
		this.sleep.pst.QueueAnim("sleep_pst", false, null).OnAnimQueueComplete(this.behaviourcomplete);
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.BeeWantsToSleep, false);
	}

	// Token: 0x06000317 RID: 791 RVA: 0x00018C34 File Offset: 0x00016E34
	private static void FindSleepLocation(BeeSleepStates.Instance smi)
	{
		smi.targetSleepCell = Grid.InvalidCell;
		FloorCellQuery floorCellQuery = PathFinderQueries.floorCellQuery.Reset(1, 0);
		smi.GetComponent<Navigator>().RunQuery(floorCellQuery);
		if (floorCellQuery.result_cells.Count > 0)
		{
			smi.targetSleepCell = floorCellQuery.result_cells[UnityEngine.Random.Range(0, floorCellQuery.result_cells.Count)];
		}
	}

	// Token: 0x06000318 RID: 792 RVA: 0x00018C95 File Offset: 0x00016E95
	public static bool ShouldWakeUp(BeeSleepStates.Instance smi)
	{
		return smi.GetSMI<BeeSleepMonitor.Instance>().CO2Exposure <= 0f;
	}

	// Token: 0x04000218 RID: 536
	public BeeSleepStates.SleepStates sleep;

	// Token: 0x04000219 RID: 537
	public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State findSleepLocation;

	// Token: 0x0400021A RID: 538
	public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State moveToSleepLocation;

	// Token: 0x0400021B RID: 539
	public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State behaviourcomplete;

	// Token: 0x02000E87 RID: 3719
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E88 RID: 3720
	public new class Instance : GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.GameInstance
	{
		// Token: 0x06006FB1 RID: 28593 RVA: 0x002B9B1A File Offset: 0x002B7D1A
		public Instance(Chore<BeeSleepStates.Instance> chore, BeeSleepStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.BeeWantsToSleep);
		}

		// Token: 0x040053C1 RID: 21441
		public int targetSleepCell;

		// Token: 0x040053C2 RID: 21442
		public float co2Exposure;
	}

	// Token: 0x02000E89 RID: 3721
	public class SleepStates : GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State
	{
		// Token: 0x040053C3 RID: 21443
		public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State pre;

		// Token: 0x040053C4 RID: 21444
		public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State loop;

		// Token: 0x040053C5 RID: 21445
		public GameStateMachine<BeeSleepStates, BeeSleepStates.Instance, IStateMachineTarget, BeeSleepStates.Def>.State pst;
	}
}
