using System;

// Token: 0x02000722 RID: 1826
public class FishOvercrowdingMonitor : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>
{
	// Token: 0x06003228 RID: 12840 RVA: 0x0010A7BC File Offset: 0x001089BC
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.satisfied;
		this.root.Enter(new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Register)).Exit(new StateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State.Callback(FishOvercrowdingMonitor.Unregister));
		this.satisfied.DoNothing();
		this.overcrowded.DoNothing();
	}

	// Token: 0x06003229 RID: 12841 RVA: 0x0010A812 File Offset: 0x00108A12
	private static void Register(FishOvercrowdingMonitor.Instance smi)
	{
		FishOvercrowingManager.Instance.Add(smi);
	}

	// Token: 0x0600322A RID: 12842 RVA: 0x0010A820 File Offset: 0x00108A20
	private static void Unregister(FishOvercrowdingMonitor.Instance smi)
	{
		FishOvercrowingManager instance = FishOvercrowingManager.Instance;
		if (instance == null)
		{
			return;
		}
		instance.Remove(smi);
	}

	// Token: 0x04001E18 RID: 7704
	public GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State satisfied;

	// Token: 0x04001E19 RID: 7705
	public GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.State overcrowded;

	// Token: 0x0200149D RID: 5277
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x0200149E RID: 5278
	public new class Instance : GameStateMachine<FishOvercrowdingMonitor, FishOvercrowdingMonitor.Instance, IStateMachineTarget, FishOvercrowdingMonitor.Def>.GameInstance
	{
		// Token: 0x06008551 RID: 34129 RVA: 0x003054C9 File Offset: 0x003036C9
		public Instance(IStateMachineTarget master, FishOvercrowdingMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x06008552 RID: 34130 RVA: 0x003054D3 File Offset: 0x003036D3
		public void SetOvercrowdingInfo(int cell_count, int fish_count)
		{
			this.cellCount = cell_count;
			this.fishCount = fish_count;
		}

		// Token: 0x040065F3 RID: 26099
		public int cellCount;

		// Token: 0x040065F4 RID: 26100
		public int fishCount;
	}
}
