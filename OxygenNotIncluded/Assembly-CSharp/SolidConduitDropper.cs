using System;
using KSerialization;
using UnityEngine;

// Token: 0x0200068E RID: 1678
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidConduitDropper : StateMachineComponent<SolidConduitDropper.SMInstance>
{
	// Token: 0x06002CEF RID: 11503 RVA: 0x000EEB7D File Offset: 0x000ECD7D
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002CF0 RID: 11504 RVA: 0x000EEB85 File Offset: 0x000ECD85
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06002CF1 RID: 11505 RVA: 0x000EEB98 File Offset: 0x000ECD98
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x06002CF2 RID: 11506 RVA: 0x000EEBA0 File Offset: 0x000ECDA0
	private void Update()
	{
		base.smi.sm.consuming.Set(this.consumer.IsConsuming, base.smi, false);
		base.smi.sm.isclosed.Set(!this.operational.IsOperational, base.smi, false);
		this.storage.DropAll(false, false, default(Vector3), true, null);
	}

	// Token: 0x04001A76 RID: 6774
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001A77 RID: 6775
	[MyCmpReq]
	private SolidConduitConsumer consumer;

	// Token: 0x04001A78 RID: 6776
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x020013A3 RID: 5027
	public class SMInstance : GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.GameInstance
	{
		// Token: 0x060081D7 RID: 33239 RVA: 0x002F68A2 File Offset: 0x002F4AA2
		public SMInstance(SolidConduitDropper master) : base(master)
		{
		}
	}

	// Token: 0x020013A4 RID: 5028
	public class States : GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper>
	{
		// Token: 0x060081D8 RID: 33240 RVA: 0x002F68AC File Offset: 0x002F4AAC
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.root.Update("Update", delegate(SolidConduitDropper.SMInstance smi, float dt)
			{
				smi.master.Update();
			}, UpdateRate.SIM_1000ms, false);
			this.idle.PlayAnim("on").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue).ParamTransition<bool>(this.isclosed, this.closed, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue);
			this.working.PlayAnim("working_pre").QueueAnim("working_loop", true, null).ParamTransition<bool>(this.consuming, this.post, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsFalse);
			this.post.PlayAnim("working_pst").OnAnimQueueComplete(this.idle);
			this.closed.PlayAnim("closed").ParamTransition<bool>(this.consuming, this.working, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsTrue).ParamTransition<bool>(this.isclosed, this.idle, GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.IsFalse);
		}

		// Token: 0x04006302 RID: 25346
		public StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.BoolParameter consuming;

		// Token: 0x04006303 RID: 25347
		public StateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.BoolParameter isclosed;

		// Token: 0x04006304 RID: 25348
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State idle;

		// Token: 0x04006305 RID: 25349
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State working;

		// Token: 0x04006306 RID: 25350
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State post;

		// Token: 0x04006307 RID: 25351
		public GameStateMachine<SolidConduitDropper.States, SolidConduitDropper.SMInstance, SolidConduitDropper, object>.State closed;
	}
}
