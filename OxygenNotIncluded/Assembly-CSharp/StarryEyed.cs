using System;

// Token: 0x020009EA RID: 2538
[SkipSaveFileSerialization]
public class StarryEyed : StateMachineComponent<StarryEyed.StatesInstance>
{
	// Token: 0x06004BC0 RID: 19392 RVA: 0x001A9485 File Offset: 0x001A7685
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x0200186C RID: 6252
	public class StatesInstance : GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.GameInstance
	{
		// Token: 0x060091C3 RID: 37315 RVA: 0x0032A53A File Offset: 0x0032873A
		public StatesInstance(StarryEyed master) : base(master)
		{
		}

		// Token: 0x060091C4 RID: 37316 RVA: 0x0032A544 File Offset: 0x00328744
		public bool IsInSpace()
		{
			WorldContainer myWorld = this.GetMyWorld();
			if (!myWorld)
			{
				return false;
			}
			int parentWorldId = myWorld.ParentWorldId;
			int id = myWorld.id;
			return myWorld.GetComponent<Clustercraft>() && parentWorldId == id;
		}
	}

	// Token: 0x0200186D RID: 6253
	public class States : GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed>
	{
		// Token: 0x060091C5 RID: 37317 RVA: 0x0032A584 File Offset: 0x00328784
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Enter(delegate(StarryEyed.StatesInstance smi)
			{
				if (smi.IsInSpace())
				{
					smi.GoTo(this.inSpace);
				}
			});
			this.idle.EventTransition(GameHashes.MinionMigration, (StarryEyed.StatesInstance smi) => Game.Instance, this.inSpace, (StarryEyed.StatesInstance smi) => smi.IsInSpace());
			this.inSpace.EventTransition(GameHashes.MinionMigration, (StarryEyed.StatesInstance smi) => Game.Instance, this.idle, (StarryEyed.StatesInstance smi) => !smi.IsInSpace()).ToggleEffect("StarryEyed");
		}

		// Token: 0x040071EA RID: 29162
		public GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State idle;

		// Token: 0x040071EB RID: 29163
		public GameStateMachine<StarryEyed.States, StarryEyed.StatesInstance, StarryEyed, object>.State inSpace;
	}
}
