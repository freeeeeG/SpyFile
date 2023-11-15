using System;

// Token: 0x020004A4 RID: 1188
public class WorldSpawnableMonitor : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>
{
	// Token: 0x06001ABC RID: 6844 RVA: 0x0008F161 File Offset: 0x0008D361
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.root;
	}

	// Token: 0x02001153 RID: 4435
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x04005BFE RID: 23550
		public Func<int, int> adjustSpawnLocationCb;
	}

	// Token: 0x02001154 RID: 4436
	public new class Instance : GameStateMachine<WorldSpawnableMonitor, WorldSpawnableMonitor.Instance, IStateMachineTarget, WorldSpawnableMonitor.Def>.GameInstance
	{
		// Token: 0x0600791C RID: 31004 RVA: 0x002D8670 File Offset: 0x002D6870
		public Instance(IStateMachineTarget master, WorldSpawnableMonitor.Def def) : base(master, def)
		{
		}
	}
}
