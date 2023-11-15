using System;
using KSerialization;

// Token: 0x0200068F RID: 1679
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitInbox : StateMachineComponent<SolidConduitInbox.SMInstance>, ISim1000ms
{
	// Token: 0x06002CF4 RID: 11508 RVA: 0x000EEC20 File Offset: 0x000ECE20
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.filteredStorage = new FilteredStorage(this, null, null, false, Db.Get().ChoreTypes.StorageFetch);
	}

	// Token: 0x06002CF5 RID: 11509 RVA: 0x000EEC46 File Offset: 0x000ECE46
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.filteredStorage.FilterChanged();
		base.smi.StartSM();
	}

	// Token: 0x06002CF6 RID: 11510 RVA: 0x000EEC64 File Offset: 0x000ECE64
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06002CF7 RID: 11511 RVA: 0x000EEC6C File Offset: 0x000ECE6C
	public void Sim1000ms(float dt)
	{
		if (this.operational.IsOperational && this.dispenser.IsDispensing)
		{
			this.operational.SetActive(true, false);
			return;
		}
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001A79 RID: 6777
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001A7A RID: 6778
	[MyCmpReq]
	private SolidConduitDispenser dispenser;

	// Token: 0x04001A7B RID: 6779
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x04001A7C RID: 6780
	private FilteredStorage filteredStorage;

	// Token: 0x020013A5 RID: 5029
	public class SMInstance : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.GameInstance
	{
		// Token: 0x060081DA RID: 33242 RVA: 0x002F69C4 File Offset: 0x002F4BC4
		public SMInstance(SolidConduitInbox master) : base(master)
		{
		}
	}

	// Token: 0x020013A6 RID: 5030
	public class States : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox>
	{
		// Token: 0x060081DB RID: 33243 RVA: 0x002F69D0 File Offset: 0x002F4BD0
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidConduitInbox.SMInstance smi) => smi.GetComponent<Operational>().IsOperational);
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidConduitInbox.SMInstance smi) => !smi.GetComponent<Operational>().IsOperational);
			this.on.idle.PlayAnim("on").EventTransition(GameHashes.ActiveChanged, this.on.working, (SolidConduitInbox.SMInstance smi) => smi.GetComponent<Operational>().IsActive);
			this.on.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).EventTransition(GameHashes.ActiveChanged, this.on.post, (SolidConduitInbox.SMInstance smi) => !smi.GetComponent<Operational>().IsActive);
			this.on.post.PlayAnim("working_pst").OnAnimQueueComplete(this.on);
		}

		// Token: 0x04006308 RID: 25352
		public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State off;

		// Token: 0x04006309 RID: 25353
		public SolidConduitInbox.States.ReadyStates on;

		// Token: 0x02002122 RID: 8482
		public class ReadyStates : GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State
		{
			// Token: 0x04009480 RID: 38016
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State idle;

			// Token: 0x04009481 RID: 38017
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State working;

			// Token: 0x04009482 RID: 38018
			public GameStateMachine<SolidConduitInbox.States, SolidConduitInbox.SMInstance, SolidConduitInbox, object>.State post;
		}
	}
}
