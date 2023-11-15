using System;

// Token: 0x020000AD RID: 173
public class BeeHiveMonitor : GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>
{
	// Token: 0x06000312 RID: 786 RVA: 0x0001895C File Offset: 0x00016B5C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.idle;
		this.idle.EventTransition(GameHashes.Nighttime, (BeeHiveMonitor.Instance smi) => GameClock.Instance, this.night, (BeeHiveMonitor.Instance smi) => GameClock.Instance.IsNighttime());
		this.night.EventTransition(GameHashes.NewDay, (BeeHiveMonitor.Instance smi) => GameClock.Instance, this.idle, (BeeHiveMonitor.Instance smi) => !GameClock.Instance.IsNighttime()).ToggleBehaviour(GameTags.Creatures.WantsToMakeHome, new StateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.Transition.ConditionCallback(this.ShouldMakeHome), null);
	}

	// Token: 0x06000313 RID: 787 RVA: 0x00018A32 File Offset: 0x00016C32
	public bool ShouldMakeHome(BeeHiveMonitor.Instance smi)
	{
		return !this.CanGoHome(smi);
	}

	// Token: 0x06000314 RID: 788 RVA: 0x00018A3E File Offset: 0x00016C3E
	public bool CanGoHome(BeeHiveMonitor.Instance smi)
	{
		return smi.gameObject.GetComponent<Bee>().FindHiveInRoom() != null;
	}

	// Token: 0x04000216 RID: 534
	public GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.State idle;

	// Token: 0x04000217 RID: 535
	public GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.State night;

	// Token: 0x02000E84 RID: 3716
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E85 RID: 3717
	public new class Instance : GameStateMachine<BeeHiveMonitor, BeeHiveMonitor.Instance, IStateMachineTarget, BeeHiveMonitor.Def>.GameInstance
	{
		// Token: 0x06006FA9 RID: 28585 RVA: 0x002B9ACB File Offset: 0x002B7CCB
		public Instance(IStateMachineTarget master, BeeHiveMonitor.Def def) : base(master, def)
		{
		}
	}
}
