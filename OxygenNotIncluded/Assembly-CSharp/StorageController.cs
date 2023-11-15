using System;

// Token: 0x02000051 RID: 81
public class StorageController : GameStateMachine<StorageController, StorageController.Instance>
{
	// Token: 0x06000174 RID: 372 RVA: 0x0000A438 File Offset: 0x00008638
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.off;
		this.root.EventTransition(GameHashes.OnStorageInteracted, this.working, null);
		this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (StorageController.Instance smi) => smi.GetComponent<Operational>().IsOperational);
		this.on.PlayAnim("on").EventTransition(GameHashes.OperationalChanged, this.off, (StorageController.Instance smi) => !smi.GetComponent<Operational>().IsOperational);
		this.working.PlayAnim("working").OnAnimQueueComplete(this.off);
	}

	// Token: 0x040000E5 RID: 229
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State off;

	// Token: 0x040000E6 RID: 230
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State on;

	// Token: 0x040000E7 RID: 231
	public GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.State working;

	// Token: 0x02000E48 RID: 3656
	public class Def : StateMachine.BaseDef
	{
	}

	// Token: 0x02000E49 RID: 3657
	public new class Instance : GameStateMachine<StorageController, StorageController.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06006F26 RID: 28454 RVA: 0x002B8E16 File Offset: 0x002B7016
		public Instance(IStateMachineTarget master, StorageController.Def def) : base(master)
		{
		}
	}
}
