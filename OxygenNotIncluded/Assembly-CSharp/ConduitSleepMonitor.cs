using System;
using UnityEngine;

// Token: 0x020000B4 RID: 180
public class ConduitSleepMonitor : GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>
{
	// Token: 0x06000333 RID: 819 RVA: 0x000198F4 File Offset: 0x00017AF4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.idle.Enter(delegate(ConduitSleepMonitor.Instance smi)
		{
			this.targetSleepCell.Set(Grid.InvalidCell, smi, false);
			smi.GetComponent<Staterpillar>().DestroyOrphanedConnectorBuilding();
		}).EventTransition(GameHashes.NewBlock, (ConduitSleepMonitor.Instance smi) => GameClock.Instance, this.searching.looking, new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.Transition.ConditionCallback(ConduitSleepMonitor.IsSleepyTime));
		this.searching.Enter(new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State.Callback(this.TryRecoverSave)).EventTransition(GameHashes.NewBlock, (ConduitSleepMonitor.Instance smi) => GameClock.Instance, this.idle, GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.Not(new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.Transition.ConditionCallback(ConduitSleepMonitor.IsSleepyTime))).Exit(delegate(ConduitSleepMonitor.Instance smi)
		{
			this.targetSleepCell.Set(Grid.InvalidCell, smi, false);
			smi.GetComponent<Staterpillar>().DestroyOrphanedConnectorBuilding();
		});
		this.searching.looking.Update(delegate(ConduitSleepMonitor.Instance smi, float dt)
		{
			this.FindSleepLocation(smi);
		}, UpdateRate.SIM_1000ms, false).ToggleStatusItem(Db.Get().CreatureStatusItems.NoSleepSpot, null).ParamTransition<int>(this.targetSleepCell, this.searching.found, (ConduitSleepMonitor.Instance smi, int sleepCell) => sleepCell != Grid.InvalidCell);
		this.searching.found.Enter(delegate(ConduitSleepMonitor.Instance smi)
		{
			smi.GetComponent<Staterpillar>().SpawnConnectorBuilding(this.targetSleepCell.Get(smi));
		}).ParamTransition<int>(this.targetSleepCell, this.searching.looking, (ConduitSleepMonitor.Instance smi, int sleepCell) => sleepCell == Grid.InvalidCell).ToggleBehaviour(GameTags.Creatures.WantsConduitConnection, (ConduitSleepMonitor.Instance smi) => this.targetSleepCell.Get(smi) != Grid.InvalidCell && ConduitSleepMonitor.IsSleepyTime(smi), null);
	}

	// Token: 0x06000334 RID: 820 RVA: 0x00019AA3 File Offset: 0x00017CA3
	public static bool IsSleepyTime(ConduitSleepMonitor.Instance smi)
	{
		return GameClock.Instance.GetTimeSinceStartOfCycle() >= 500f;
	}

	// Token: 0x06000335 RID: 821 RVA: 0x00019ABC File Offset: 0x00017CBC
	private void TryRecoverSave(ConduitSleepMonitor.Instance smi)
	{
		Staterpillar component = smi.GetComponent<Staterpillar>();
		if (this.targetSleepCell.Get(smi) == Grid.InvalidCell && component.IsConnectorBuildingSpawned())
		{
			int value = Grid.PosToCell(component.GetConnectorBuilding());
			this.targetSleepCell.Set(value, smi, false);
		}
	}

	// Token: 0x06000336 RID: 822 RVA: 0x00019B08 File Offset: 0x00017D08
	private void FindSleepLocation(ConduitSleepMonitor.Instance smi)
	{
		StaterpillarCellQuery staterpillarCellQuery = PathFinderQueries.staterpillarCellQuery.Reset(10, smi.gameObject, smi.def.conduitLayer);
		smi.GetComponent<Navigator>().RunQuery(staterpillarCellQuery);
		if (staterpillarCellQuery.result_cells.Count > 0)
		{
			foreach (int num in staterpillarCellQuery.result_cells)
			{
				int cellInDirection = Grid.GetCellInDirection(num, Direction.Down);
				if (Grid.Objects[cellInDirection, (int)smi.def.conduitLayer] != null)
				{
					this.targetSleepCell.Set(num, smi, false);
					break;
				}
			}
			if (this.targetSleepCell.Get(smi) == Grid.InvalidCell)
			{
				this.targetSleepCell.Set(staterpillarCellQuery.result_cells[UnityEngine.Random.Range(0, staterpillarCellQuery.result_cells.Count)], smi, false);
			}
		}
	}

	// Token: 0x04000229 RID: 553
	private GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State idle;

	// Token: 0x0400022A RID: 554
	private ConduitSleepMonitor.SleepSearchStates searching;

	// Token: 0x0400022B RID: 555
	public StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.IntParameter targetSleepCell = new StateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.IntParameter(Grid.InvalidCell);

	// Token: 0x02000E9E RID: 3742
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040053FD RID: 21501
		public ObjectLayer conduitLayer;
	}

	// Token: 0x02000E9F RID: 3743
	private class SleepSearchStates : GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State
	{
		// Token: 0x040053FE RID: 21502
		public GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State looking;

		// Token: 0x040053FF RID: 21503
		public GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.State found;
	}

	// Token: 0x02000EA0 RID: 3744
	public new class Instance : GameStateMachine<ConduitSleepMonitor, ConduitSleepMonitor.Instance, IStateMachineTarget, ConduitSleepMonitor.Def>.GameInstance
	{
		// Token: 0x06006FEE RID: 28654 RVA: 0x002BA07B File Offset: 0x002B827B
		public Instance(IStateMachineTarget master, ConduitSleepMonitor.Def def) : base(master, def)
		{
		}
	}
}
