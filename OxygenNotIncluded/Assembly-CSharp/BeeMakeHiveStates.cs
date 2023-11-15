using System;
using UnityEngine;

// Token: 0x020000AC RID: 172
public class BeeMakeHiveStates : GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>
{
	// Token: 0x0600030D RID: 781 RVA: 0x000187B0 File Offset: 0x000169B0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.findBuildLocation;
		this.root.DoNothing();
		this.findBuildLocation.Enter(delegate(BeeMakeHiveStates.Instance smi)
		{
			this.FindBuildLocation(smi);
			if (smi.targetBuildCell != Grid.InvalidCell)
			{
				smi.GoTo(this.moveToBuildLocation);
				return;
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.moveToBuildLocation.MoveTo((BeeMakeHiveStates.Instance smi) => smi.targetBuildCell, this.doBuild, this.behaviourcomplete, false);
		this.doBuild.PlayAnim("hive_grow_pre").EventHandler(GameHashes.AnimQueueComplete, delegate(BeeMakeHiveStates.Instance smi)
		{
			if (smi.gameObject.GetComponent<Bee>().FindHiveInRoom() == null)
			{
				smi.builtHome = true;
				smi.BuildHome();
			}
			smi.GoTo(this.behaviourcomplete);
		});
		this.behaviourcomplete.BehaviourComplete(GameTags.Creatures.WantsToMakeHome, false).Exit(delegate(BeeMakeHiveStates.Instance smi)
		{
			if (smi.builtHome)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			}
		});
	}

	// Token: 0x0600030E RID: 782 RVA: 0x00018880 File Offset: 0x00016A80
	private void FindBuildLocation(BeeMakeHiveStates.Instance smi)
	{
		smi.targetBuildCell = Grid.InvalidCell;
		GameObject prefab = Assets.GetPrefab("BeeHive".ToTag());
		BuildingPlacementQuery buildingPlacementQuery = PathFinderQueries.buildingPlacementQuery.Reset(1, prefab);
		smi.GetComponent<Navigator>().RunQuery(buildingPlacementQuery);
		if (buildingPlacementQuery.result_cells.Count > 0)
		{
			smi.targetBuildCell = buildingPlacementQuery.result_cells[UnityEngine.Random.Range(0, buildingPlacementQuery.result_cells.Count)];
		}
	}

	// Token: 0x04000212 RID: 530
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State findBuildLocation;

	// Token: 0x04000213 RID: 531
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State moveToBuildLocation;

	// Token: 0x04000214 RID: 532
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State doBuild;

	// Token: 0x04000215 RID: 533
	public GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.State behaviourcomplete;

	// Token: 0x02000E81 RID: 3713
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E82 RID: 3714
	public new class Instance : GameStateMachine<BeeMakeHiveStates, BeeMakeHiveStates.Instance, IStateMachineTarget, BeeMakeHiveStates.Def>.GameInstance
	{
		// Token: 0x06006FA2 RID: 28578 RVA: 0x002B99F6 File Offset: 0x002B7BF6
		public Instance(Chore<BeeMakeHiveStates.Instance> chore, BeeMakeHiveStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.WantsToMakeHome);
		}

		// Token: 0x06006FA3 RID: 28579 RVA: 0x002B9A1C File Offset: 0x002B7C1C
		public void BuildHome()
		{
			Vector3 position = Grid.CellToPos(this.targetBuildCell, CellAlignment.Bottom, Grid.SceneLayer.Creatures);
			GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("BeeHive".ToTag()), position, Quaternion.identity, null, null, true, 0);
			PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
			component.ElementID = SimHashes.Creature;
			component.Temperature = base.gameObject.GetComponent<PrimaryElement>().Temperature;
			gameObject.SetActive(true);
			gameObject.GetSMI<BeeHive.StatesInstance>().SetUpNewHive();
		}

		// Token: 0x040053B7 RID: 21431
		public int targetBuildCell;

		// Token: 0x040053B8 RID: 21432
		public bool builtHome;
	}
}
