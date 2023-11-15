using System;

// Token: 0x020000D5 RID: 213
public class MinionChoreBrokerStates : GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>
{
	// Token: 0x060003CE RID: 974 RVA: 0x0001D754 File Offset: 0x0001B954
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.hasChore;
		this.root.DoNothing();
		this.hasChore.Enter(delegate(MinionChoreBrokerStates.Instance smi)
		{
		});
	}

	// Token: 0x04000284 RID: 644
	private GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.State hasChore;

	// Token: 0x02000F03 RID: 3843
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000F04 RID: 3844
	public new class Instance : GameStateMachine<MinionChoreBrokerStates, MinionChoreBrokerStates.Instance, IStateMachineTarget, MinionChoreBrokerStates.Def>.GameInstance
	{
		// Token: 0x060070C8 RID: 28872 RVA: 0x002BB68E File Offset: 0x002B988E
		public Instance(Chore<MinionChoreBrokerStates.Instance> chore, MinionChoreBrokerStates.Def def) : base(chore, def)
		{
		}
	}
}
