using System;
using UnityEngine;

// Token: 0x02000578 RID: 1400
public class FXAnim : GameStateMachine<FXAnim, FXAnim.Instance>
{
	// Token: 0x06002208 RID: 8712 RVA: 0x000BAC50 File Offset: 0x000B8E50
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.loop;
		base.Target(this.fx);
		this.loop.Enter(delegate(FXAnim.Instance smi)
		{
			smi.Enter();
		}).EventTransition(GameHashes.AnimQueueComplete, this.restart, null).Exit("Post", delegate(FXAnim.Instance smi)
		{
			smi.Exit();
		});
		this.restart.GoTo(this.loop);
	}

	// Token: 0x04001351 RID: 4945
	public StateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.TargetParameter fx;

	// Token: 0x04001352 RID: 4946
	public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State loop;

	// Token: 0x04001353 RID: 4947
	public GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.State restart;

	// Token: 0x0200120A RID: 4618
	public new class Instance : GameStateMachine<FXAnim, FXAnim.Instance, IStateMachineTarget, object>.GameInstance
	{
		// Token: 0x06007B9E RID: 31646 RVA: 0x002DE668 File Offset: 0x002DC868
		public Instance(IStateMachineTarget master, string kanim_file, string anim, KAnim.PlayMode mode, Vector3 offset, Color32 tint_colour) : base(master)
		{
			this.animController = FXHelpers.CreateEffect(kanim_file, base.smi.master.transform.GetPosition() + offset, base.smi.master.transform, false, Grid.SceneLayer.Front, false);
			this.animController.gameObject.Subscribe(-1061186183, new Action<object>(this.OnAnimQueueComplete));
			this.animController.TintColour = tint_colour;
			base.sm.fx.Set(this.animController.gameObject, base.smi, false);
			this.anim = anim;
			this.mode = mode;
		}

		// Token: 0x06007B9F RID: 31647 RVA: 0x002DE719 File Offset: 0x002DC919
		public void Enter()
		{
			this.animController.Play(this.anim, this.mode, 1f, 0f);
		}

		// Token: 0x06007BA0 RID: 31648 RVA: 0x002DE741 File Offset: 0x002DC941
		public void Exit()
		{
			this.DestroyFX();
		}

		// Token: 0x06007BA1 RID: 31649 RVA: 0x002DE749 File Offset: 0x002DC949
		private void OnAnimQueueComplete(object data)
		{
			this.DestroyFX();
		}

		// Token: 0x06007BA2 RID: 31650 RVA: 0x002DE751 File Offset: 0x002DC951
		private void DestroyFX()
		{
			Util.KDestroyGameObject(base.sm.fx.Get(base.smi));
		}

		// Token: 0x04005E48 RID: 24136
		private string anim;

		// Token: 0x04005E49 RID: 24137
		private KAnim.PlayMode mode;

		// Token: 0x04005E4A RID: 24138
		private KBatchedAnimController animController;
	}
}
