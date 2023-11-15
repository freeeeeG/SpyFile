using System;
using KSerialization;

// Token: 0x02000735 RID: 1845
public class RoverChoreMonitor : GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>
{
	// Token: 0x060032B0 RID: 12976 RVA: 0x0010D278 File Offset: 0x0010B478
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		this.loop.ToggleBehaviour(GameTags.Creatures.Tunnel, (RoverChoreMonitor.Instance smi) => true, null).ToggleBehaviour(GameTags.Creatures.Builder, (RoverChoreMonitor.Instance smi) => true, null);
	}

	// Token: 0x04001E6F RID: 7791
	public GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.State loop;

	// Token: 0x020014C7 RID: 5319
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x020014C8 RID: 5320
	public new class Instance : GameStateMachine<RoverChoreMonitor, RoverChoreMonitor.Instance, IStateMachineTarget, RoverChoreMonitor.Def>.GameInstance
	{
		// Token: 0x060085D7 RID: 34263 RVA: 0x003075AB File Offset: 0x003057AB
		public Instance(IStateMachineTarget master, RoverChoreMonitor.Def def) : base(master, def)
		{
		}

		// Token: 0x060085D8 RID: 34264 RVA: 0x003075BC File Offset: 0x003057BC
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
		}

		// Token: 0x04006661 RID: 26209
		[Serialize]
		public int lastDigCell = -1;

		// Token: 0x04006662 RID: 26210
		private Action<object> OnDestinationReachedDelegate;
	}
}
