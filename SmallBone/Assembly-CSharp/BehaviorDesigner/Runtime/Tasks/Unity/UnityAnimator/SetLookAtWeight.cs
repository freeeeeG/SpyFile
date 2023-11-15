using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001631 RID: 5681
	[TaskDescription("Sets the look at weight. Returns success immediately after.")]
	[TaskCategory("Unity/Animator")]
	public class SetLookAtWeight : Action
	{
		// Token: 0x06006C76 RID: 27766 RVA: 0x00136357 File Offset: 0x00134557
		public override void OnStart()
		{
			this.animator = base.GetComponent<Animator>();
			this.weightSet = false;
		}

		// Token: 0x06006C77 RID: 27767 RVA: 0x0013636C File Offset: 0x0013456C
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			if (!this.weightSet)
			{
				return TaskStatus.Running;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006C78 RID: 27768 RVA: 0x00136394 File Offset: 0x00134594
		public override void OnAnimatorIK()
		{
			if (this.animator == null)
			{
				return;
			}
			this.animator.SetLookAtWeight(this.weight.Value, this.bodyWeight, this.headWeight, this.eyesWeight, this.clampWeight);
			this.weightSet = true;
		}

		// Token: 0x06006C79 RID: 27769 RVA: 0x001363E5 File Offset: 0x001345E5
		public override void OnReset()
		{
			this.weight = 0f;
			this.bodyWeight = 0f;
			this.headWeight = 1f;
			this.eyesWeight = 0f;
			this.clampWeight = 0.5f;
		}

		// Token: 0x04005836 RID: 22582
		[Tooltip("(0-1) the global weight of the LookAt, multiplier for other parameters.")]
		public SharedFloat weight;

		// Token: 0x04005837 RID: 22583
		[Tooltip("(0-1) determines how much the body is involved in the LookAt.")]
		public float bodyWeight;

		// Token: 0x04005838 RID: 22584
		[Tooltip("(0-1) determines how much the head is involved in the LookAt.")]
		public float headWeight = 1f;

		// Token: 0x04005839 RID: 22585
		[Tooltip("(0-1) determines how much the eyes are involved in the LookAt.")]
		public float eyesWeight;

		// Token: 0x0400583A RID: 22586
		[Tooltip("(0-1) 0.0 means the character is completely unrestrained in motion, 1.0 means he's completely clamped (look at becomes impossible), and 0.5 means he'll be able to move on half of the possible range (180 degrees).")]
		public float clampWeight = 0.5f;

		// Token: 0x0400583B RID: 22587
		private Animator animator;

		// Token: 0x0400583C RID: 22588
		private bool weightSet;
	}
}
