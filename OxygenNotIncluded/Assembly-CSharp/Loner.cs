using System;

// Token: 0x02000853 RID: 2131
[SkipSaveFileSerialization]
public class Loner : StateMachineComponent<Loner.StatesInstance>
{
	// Token: 0x06003E47 RID: 15943 RVA: 0x0015A274 File Offset: 0x00158474
	protected override void OnSpawn()
	{
		base.smi.StartSM();
	}

	// Token: 0x02001625 RID: 5669
	public class StatesInstance : GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.GameInstance
	{
		// Token: 0x060089D4 RID: 35284 RVA: 0x0031204F File Offset: 0x0031024F
		public StatesInstance(Loner master) : base(master)
		{
		}

		// Token: 0x060089D5 RID: 35285 RVA: 0x00312058 File Offset: 0x00310258
		public bool IsAlone()
		{
			WorldContainer myWorld = this.GetMyWorld();
			if (!myWorld)
			{
				return false;
			}
			int parentWorldId = myWorld.ParentWorldId;
			int id = myWorld.id;
			MinionIdentity component = base.GetComponent<MinionIdentity>();
			foreach (object obj in Components.LiveMinionIdentities)
			{
				MinionIdentity minionIdentity = (MinionIdentity)obj;
				if (component != minionIdentity)
				{
					int myWorldId = minionIdentity.GetMyWorldId();
					if (id == myWorldId || parentWorldId == myWorldId)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	// Token: 0x02001626 RID: 5670
	public class States : GameStateMachine<Loner.States, Loner.StatesInstance, Loner>
	{
		// Token: 0x060089D6 RID: 35286 RVA: 0x00312100 File Offset: 0x00310300
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Enter(delegate(Loner.StatesInstance smi)
			{
				if (smi.IsAlone())
				{
					smi.GoTo(this.alone);
				}
			});
			this.idle.EventTransition(GameHashes.MinionMigration, (Loner.StatesInstance smi) => Game.Instance, this.alone, (Loner.StatesInstance smi) => smi.IsAlone()).EventTransition(GameHashes.MinionDelta, (Loner.StatesInstance smi) => Game.Instance, this.alone, (Loner.StatesInstance smi) => smi.IsAlone());
			this.alone.EventTransition(GameHashes.MinionMigration, (Loner.StatesInstance smi) => Game.Instance, this.idle, (Loner.StatesInstance smi) => !smi.IsAlone()).EventTransition(GameHashes.MinionDelta, (Loner.StatesInstance smi) => Game.Instance, this.idle, (Loner.StatesInstance smi) => !smi.IsAlone()).ToggleEffect("Loner");
		}

		// Token: 0x04006ACF RID: 27343
		public GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State idle;

		// Token: 0x04006AD0 RID: 27344
		public GameStateMachine<Loner.States, Loner.StatesInstance, Loner, object>.State alone;
	}
}
