using System;
using UnityEngine;

// Token: 0x020007FC RID: 2044
public class HotTubWorkerStateMachine : GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker>
{
	// Token: 0x06003A61 RID: 14945 RVA: 0x00144DB0 File Offset: 0x00142FB0
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.pre_front;
		base.Target(this.worker);
		this.root.ToggleAnims("anim_interacts_hottub_kanim", 0f, "");
		this.pre_front.PlayAnim("working_pre_front").OnAnimQueueComplete(this.pre_back);
		this.pre_back.PlayAnim("working_pre_back").Enter(delegate(HotTubWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.BuildingUse);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.loop);
		this.loop.PlayAnim((HotTubWorkerStateMachine.StatesInstance smi) => HotTubWorkerStateMachine.workAnimLoopVariants[UnityEngine.Random.Range(0, HotTubWorkerStateMachine.workAnimLoopVariants.Length)], KAnim.PlayMode.Once).OnAnimQueueComplete(this.loop_reenter).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (HotTubWorkerStateMachine.StatesInstance smi) => smi.GetComponent<Worker>().state == Worker.State.PendingCompletion);
		this.loop_reenter.GoTo(this.loop).EventTransition(GameHashes.WorkerPlayPostAnim, this.pst_back, (HotTubWorkerStateMachine.StatesInstance smi) => smi.GetComponent<Worker>().state == Worker.State.PendingCompletion);
		this.pst_back.PlayAnim("working_pst_back").OnAnimQueueComplete(this.pst_front);
		this.pst_front.PlayAnim("working_pst_front").Enter(delegate(HotTubWorkerStateMachine.StatesInstance smi)
		{
			Vector3 position = smi.transform.GetPosition();
			position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
			smi.transform.SetPosition(position);
		}).OnAnimQueueComplete(this.complete);
	}

	// Token: 0x040026DD RID: 9949
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State pre_front;

	// Token: 0x040026DE RID: 9950
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State pre_back;

	// Token: 0x040026DF RID: 9951
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State loop;

	// Token: 0x040026E0 RID: 9952
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State loop_reenter;

	// Token: 0x040026E1 RID: 9953
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State pst_back;

	// Token: 0x040026E2 RID: 9954
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State pst_front;

	// Token: 0x040026E3 RID: 9955
	private GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.State complete;

	// Token: 0x040026E4 RID: 9956
	public StateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.TargetParameter worker;

	// Token: 0x040026E5 RID: 9957
	public static string[] workAnimLoopVariants = new string[]
	{
		"working_loop1",
		"working_loop2",
		"working_loop3"
	};

	// Token: 0x020015DF RID: 5599
	public class StatesInstance : GameStateMachine<HotTubWorkerStateMachine, HotTubWorkerStateMachine.StatesInstance, Worker, object>.GameInstance
	{
		// Token: 0x060088B9 RID: 35001 RVA: 0x0030F757 File Offset: 0x0030D957
		public StatesInstance(Worker master) : base(master)
		{
			base.sm.worker.Set(master, base.smi);
		}
	}
}
