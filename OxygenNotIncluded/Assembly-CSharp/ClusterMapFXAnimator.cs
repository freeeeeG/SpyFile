using System;

// Token: 0x020009C6 RID: 2502
public class ClusterMapFXAnimator : GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x06004ADB RID: 19163 RVA: 0x001A55B4 File Offset: 0x001A37B4
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.play;
		this.play.OnSignal(this.onAnimComplete, this.finished);
		this.finished.Enter(delegate(ClusterMapFXAnimator.StatesInstance smi)
		{
			smi.DestroyEntity();
		});
	}

	// Token: 0x04003115 RID: 12565
	private KBatchedAnimController animController;

	// Token: 0x04003116 RID: 12566
	public StateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x04003117 RID: 12567
	public GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.State play;

	// Token: 0x04003118 RID: 12568
	public GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.State finished;

	// Token: 0x04003119 RID: 12569
	public StateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.Signal onAnimComplete;

	// Token: 0x02001855 RID: 6229
	public class StatesInstance : GameStateMachine<ClusterMapFXAnimator, ClusterMapFXAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x06009181 RID: 37249 RVA: 0x00329B88 File Offset: 0x00327D88
		public StatesInstance(ClusterMapVisualizer visualizer, ClusterGridEntity entity) : base(visualizer)
		{
			base.sm.entityTarget.Set(entity, this);
			visualizer.GetFirstAnimController().gameObject.Subscribe(-1061186183, new Action<object>(this.OnAnimQueueComplete));
		}

		// Token: 0x06009182 RID: 37250 RVA: 0x00329BC5 File Offset: 0x00327DC5
		private void OnAnimQueueComplete(object data)
		{
			base.sm.onAnimComplete.Trigger(this);
		}

		// Token: 0x06009183 RID: 37251 RVA: 0x00329BD8 File Offset: 0x00327DD8
		public void DestroyEntity()
		{
			base.sm.entityTarget.Get(this).DeleteObject();
		}
	}
}
