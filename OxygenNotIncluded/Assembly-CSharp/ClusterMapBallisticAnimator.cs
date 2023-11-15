using System;
using UnityEngine;

// Token: 0x020009C5 RID: 2501
public class ClusterMapBallisticAnimator : GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer>
{
	// Token: 0x06004AD9 RID: 19161 RVA: 0x001A54D0 File Offset: 0x001A36D0
	public override void InitializeStates(out StateMachine.BaseState defaultState)
	{
		defaultState = this.moving;
		this.root.Target(this.entityTarget).TagTransition(GameTags.BallisticEntityLaunching, this.launching, false).TagTransition(GameTags.BallisticEntityLanding, this.landing, false).TagTransition(GameTags.BallisticEntityMoving, this.moving, false);
		this.moving.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("inflight_loop", KAnim.PlayMode.Loop);
		});
		this.landing.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("landing", KAnim.PlayMode.Loop);
		});
		this.launching.Enter(delegate(ClusterMapBallisticAnimator.StatesInstance smi)
		{
			smi.PlayVisAnim("launching", KAnim.PlayMode.Loop);
		});
	}

	// Token: 0x04003111 RID: 12561
	public StateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.TargetParameter entityTarget;

	// Token: 0x04003112 RID: 12562
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State launching;

	// Token: 0x04003113 RID: 12563
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State moving;

	// Token: 0x04003114 RID: 12564
	public GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.State landing;

	// Token: 0x02001853 RID: 6227
	public class StatesInstance : GameStateMachine<ClusterMapBallisticAnimator, ClusterMapBallisticAnimator.StatesInstance, ClusterMapVisualizer, object>.GameInstance
	{
		// Token: 0x06009176 RID: 37238 RVA: 0x00329A4E File Offset: 0x00327C4E
		public StatesInstance(ClusterMapVisualizer master, ClusterGridEntity entity) : base(master)
		{
			this.entity = entity;
			base.sm.entityTarget.Set(entity, this);
		}

		// Token: 0x06009177 RID: 37239 RVA: 0x00329A77 File Offset: 0x00327C77
		public void PlayVisAnim(string animName, KAnim.PlayMode playMode)
		{
			base.GetComponent<ClusterMapVisualizer>().PlayAnim(animName, playMode);
		}

		// Token: 0x06009178 RID: 37240 RVA: 0x00329A88 File Offset: 0x00327C88
		public void ToggleVisAnim(bool on)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			if (!on)
			{
				component.GetFirstAnimController().Play("grounded", KAnim.PlayMode.Once, 1f, 0f);
			}
		}

		// Token: 0x06009179 RID: 37241 RVA: 0x00329AC0 File Offset: 0x00327CC0
		public void SubscribeOnVisAnimComplete(Action<object> action)
		{
			ClusterMapVisualizer component = base.GetComponent<ClusterMapVisualizer>();
			this.UnsubscribeOnVisAnimComplete();
			this.animCompleteSubscriber = component.GetFirstAnimController().gameObject;
			this.animCompleteHandle = this.animCompleteSubscriber.Subscribe(-1061186183, action);
		}

		// Token: 0x0600917A RID: 37242 RVA: 0x00329B02 File Offset: 0x00327D02
		public void UnsubscribeOnVisAnimComplete()
		{
			if (this.animCompleteHandle != -1)
			{
				DebugUtil.DevAssert(this.animCompleteSubscriber != null, "ClustermapBallisticAnimator animCompleteSubscriber GameObject is null. Whatever the previous gameObject in this variable was, it may not have unsubscribed from an event properly", null);
				this.animCompleteSubscriber.Unsubscribe(this.animCompleteHandle);
				this.animCompleteHandle = -1;
			}
		}

		// Token: 0x0600917B RID: 37243 RVA: 0x00329B3C File Offset: 0x00327D3C
		protected override void OnCleanUp()
		{
			base.OnCleanUp();
			this.UnsubscribeOnVisAnimComplete();
		}

		// Token: 0x040071AE RID: 29102
		public ClusterGridEntity entity;

		// Token: 0x040071AF RID: 29103
		private int animCompleteHandle = -1;

		// Token: 0x040071B0 RID: 29104
		private GameObject animCompleteSubscriber;
	}
}
