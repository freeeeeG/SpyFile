using System;
using UnityEngine;

// Token: 0x02000A24 RID: 2596
public class WindTunnelWorkerStateMachine : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker>
{
	// Token: 0x06004DD2 RID: 19922 RVA: 0x001B466C File Offset: 0x001B286C
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre_front;
		base.Target(this.worker);
		this.root.ToggleAnims((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.OverrideAnim);
		this.pre_front.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PreFrontAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pre_back);
		this.pre_back.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PreBackAnim, KAnim.PlayMode.Once).Enter(delegate(WindTunnelWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.loop);
		this.loop.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.LoopAnim, KAnim.PlayMode.Loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (WindTunnelWorkerStateMachine.StatesInstance smi) => smi.GetComponent<Worker>().state == Worker.State.PendingCompletion);
		this.pst_back.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PstBackAnim, KAnim.PlayMode.Once).OnAnimQueueComplete(this.pst_front);
		this.pst_front.PlayAnim((WindTunnelWorkerStateMachine.StatesInstance smi) => smi.PstFrontAnim, KAnim.PlayMode.Once).Enter(delegate(WindTunnelWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.complete);
	}

	// Token: 0x040032B6 RID: 12982
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State pre_front;

	// Token: 0x040032B7 RID: 12983
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State pre_back;

	// Token: 0x040032B8 RID: 12984
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State loop;

	// Token: 0x040032B9 RID: 12985
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State pst_back;

	// Token: 0x040032BA RID: 12986
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State pst_front;

	// Token: 0x040032BB RID: 12987
	private GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.State complete;

	// Token: 0x040032BC RID: 12988
	public StateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.TargetParameter worker;

	// Token: 0x020018AD RID: 6317
	public class StatesInstance : GameStateMachine<WindTunnelWorkerStateMachine, WindTunnelWorkerStateMachine.StatesInstance, Worker, object>.GameInstance
	{
		// Token: 0x0600926A RID: 37482 RVA: 0x0032BF23 File Offset: 0x0032A123
		public StatesInstance(Worker master, VerticalWindTunnelWorkable workable) : base(master)
		{
			this.workable = workable;
			base.sm.worker.Set(master, base.smi);
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x0600926B RID: 37483 RVA: 0x0032BF4A File Offset: 0x0032A14A
		public HashedString OverrideAnim
		{
			get
			{
				return this.workable.overrideAnim;
			}
		}

		// Token: 0x170009B3 RID: 2483
		// (get) Token: 0x0600926C RID: 37484 RVA: 0x0032BF57 File Offset: 0x0032A157
		public string PreFrontAnim
		{
			get
			{
				return this.workable.preAnims[0];
			}
		}

		// Token: 0x170009B4 RID: 2484
		// (get) Token: 0x0600926D RID: 37485 RVA: 0x0032BF66 File Offset: 0x0032A166
		public string PreBackAnim
		{
			get
			{
				return this.workable.preAnims[1];
			}
		}

		// Token: 0x170009B5 RID: 2485
		// (get) Token: 0x0600926E RID: 37486 RVA: 0x0032BF75 File Offset: 0x0032A175
		public string LoopAnim
		{
			get
			{
				return this.workable.loopAnim;
			}
		}

		// Token: 0x170009B6 RID: 2486
		// (get) Token: 0x0600926F RID: 37487 RVA: 0x0032BF82 File Offset: 0x0032A182
		public string PstBackAnim
		{
			get
			{
				return this.workable.pstAnims[0];
			}
		}

		// Token: 0x170009B7 RID: 2487
		// (get) Token: 0x06009270 RID: 37488 RVA: 0x0032BF91 File Offset: 0x0032A191
		public string PstFrontAnim
		{
			get
			{
				return this.workable.pstAnims[1];
			}
		}

		// Token: 0x040072A1 RID: 29345
		private VerticalWindTunnelWorkable workable;
	}
}
