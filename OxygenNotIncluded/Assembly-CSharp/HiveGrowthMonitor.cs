using System;

// Token: 0x020000C9 RID: 201
public class HiveGrowthMonitor : GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>
{
	// Token: 0x06000399 RID: 921 RVA: 0x0001C261 File Offset: 0x0001A461
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
		this.root.ToggleBehaviour(GameTags.Creatures.Behaviours.GrowUpBehaviour, new StateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.Transition.ConditionCallback(HiveGrowthMonitor.IsGrowing), null);
	}

	// Token: 0x0600039A RID: 922 RVA: 0x0001C289 File Offset: 0x0001A489
	public static bool IsGrowing(HiveGrowthMonitor.Instance smi)
	{
		return !smi.GetSMI<BeeHive.StatesInstance>().IsFullyGrown();
	}

	// Token: 0x02000EE2 RID: 3810
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000EE3 RID: 3811
	public new class Instance : GameStateMachine<HiveGrowthMonitor, HiveGrowthMonitor.Instance, IStateMachineTarget, HiveGrowthMonitor.Def>.GameInstance
	{
		// Token: 0x06007075 RID: 28789 RVA: 0x002BAD6C File Offset: 0x002B8F6C
		public Instance(IStateMachineTarget master, HiveGrowthMonitor.Def def) : base(master, def)
		{
		}
	}
}
