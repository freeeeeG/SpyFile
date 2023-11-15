using System;

// Token: 0x020000CA RID: 202
public class HiveHarvestStates : GameStateMachine<HiveHarvestStates, HiveHarvestStates.Instance, IStateMachineTarget, HiveHarvestStates.Def>
{
	// Token: 0x0600039C RID: 924 RVA: 0x0001C2A1 File Offset: 0x0001A4A1
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.DoNothing();
	}

	// Token: 0x02000EE4 RID: 3812
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EE5 RID: 3813
	public new class Instance : GameStateMachine<HiveHarvestStates, HiveHarvestStates.Instance, IStateMachineTarget, HiveHarvestStates.Def>.GameInstance
	{
		// Token: 0x06007077 RID: 28791 RVA: 0x002BAD7E File Offset: 0x002B8F7E
		public Instance(Chore<HiveHarvestStates.Instance> chore, HiveHarvestStates.Def def) : base(chore, def)
		{
			chore.AddPrecondition(ChorePreconditions.instance.CheckBehaviourPrecondition, GameTags.Creatures.Behaviours.HarvestHiveBehaviour);
		}
	}
}
