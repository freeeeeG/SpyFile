using System;
using KSerialization;

// Token: 0x02000691 RID: 1681
[SerializationConfig(MemberSerialization.OptIn)]
public class SolidLogicValve : StateMachineComponent<SolidLogicValve.StatesInstance>
{
	// Token: 0x06002D01 RID: 11521 RVA: 0x000EED88 File Offset: 0x000ECF88
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002D02 RID: 11522 RVA: 0x000EED90 File Offset: 0x000ECF90
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
	}

	// Token: 0x06002D03 RID: 11523 RVA: 0x000EEDA3 File Offset: 0x000ECFA3
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x04001A82 RID: 6786
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001A83 RID: 6787
	[MyCmpReq]
	private SolidConduitBridge bridge;

	// Token: 0x020013AA RID: 5034
	public class States : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve>
	{
		// Token: 0x060081E3 RID: 33251 RVA: 0x002F6C30 File Offset: 0x002F4E30
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.off;
			this.root.DoNothing();
			this.off.PlayAnim("off").EventTransition(GameHashes.OperationalChanged, this.on, (SolidLogicValve.StatesInstance smi) => smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidLogicValve.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(false, false);
			});
			this.on.DefaultState(this.on.idle).EventTransition(GameHashes.OperationalChanged, this.off, (SolidLogicValve.StatesInstance smi) => !smi.GetComponent<Operational>().IsOperational).Enter(delegate(SolidLogicValve.StatesInstance smi)
			{
				smi.GetComponent<Operational>().SetActive(true, false);
			});
			this.on.idle.PlayAnim("on").Transition(this.on.working, (SolidLogicValve.StatesInstance smi) => smi.IsDispensing(), UpdateRate.SIM_200ms);
			this.on.working.PlayAnim("on_flow", KAnim.PlayMode.Loop).Transition(this.on.idle, (SolidLogicValve.StatesInstance smi) => !smi.IsDispensing(), UpdateRate.SIM_200ms);
		}

		// Token: 0x0400630F RID: 25359
		public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State off;

		// Token: 0x04006310 RID: 25360
		public SolidLogicValve.States.ReadyStates on;

		// Token: 0x02002125 RID: 8485
		public class ReadyStates : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State
		{
			// Token: 0x0400948A RID: 38026
			public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State idle;

			// Token: 0x0400948B RID: 38027
			public GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.State working;
		}
	}

	// Token: 0x020013AB RID: 5035
	public class StatesInstance : GameStateMachine<SolidLogicValve.States, SolidLogicValve.StatesInstance, SolidLogicValve, object>.GameInstance
	{
		// Token: 0x060081E5 RID: 33253 RVA: 0x002F6DB4 File Offset: 0x002F4FB4
		public StatesInstance(SolidLogicValve master) : base(master)
		{
		}

		// Token: 0x060081E6 RID: 33254 RVA: 0x002F6DBD File Offset: 0x002F4FBD
		public bool IsDispensing()
		{
			return base.master.bridge.IsDispensing;
		}
	}
}
