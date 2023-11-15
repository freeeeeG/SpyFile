using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001626 RID: 5670
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Automatically adjust the gameobject position and rotation so that the AvatarTarget reaches the matchPosition when the current state is at the specified progress. Returns Success.")]
	public class MatchTarget : Action
	{
		// Token: 0x06006C40 RID: 27712 RVA: 0x00135B70 File Offset: 0x00133D70
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C41 RID: 27713 RVA: 0x00135BB0 File Offset: 0x00133DB0
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.MatchTarget(this.matchPosition.Value, this.matchRotation.Value, this.targetBodyPart, new MatchTargetWeightMask(this.weightMaskPosition, this.weightMaskRotation), this.startNormalizedTime, this.targetNormalizedTime);
			return TaskStatus.Success;
		}

		// Token: 0x06006C42 RID: 27714 RVA: 0x00135C1C File Offset: 0x00133E1C
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.matchPosition = Vector3.zero;
			this.matchRotation = Quaternion.identity;
			this.targetBodyPart = AvatarTarget.Root;
			this.weightMaskPosition = Vector3.zero;
			this.weightMaskRotation = 0f;
			this.startNormalizedTime = 0f;
			this.targetNormalizedTime = 1f;
		}

		// Token: 0x040057F9 RID: 22521
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057FA RID: 22522
		[Tooltip("The position we want the body part to reach")]
		public SharedVector3 matchPosition;

		// Token: 0x040057FB RID: 22523
		[Tooltip("The rotation in which we want the body part to be")]
		public SharedQuaternion matchRotation;

		// Token: 0x040057FC RID: 22524
		[Tooltip("The body part that is involved in the match")]
		public AvatarTarget targetBodyPart;

		// Token: 0x040057FD RID: 22525
		[Tooltip("Weights for matching position")]
		public Vector3 weightMaskPosition;

		// Token: 0x040057FE RID: 22526
		[Tooltip("Weights for matching rotation")]
		public float weightMaskRotation;

		// Token: 0x040057FF RID: 22527
		[Tooltip("Start time within the animation clip")]
		public float startNormalizedTime;

		// Token: 0x04005800 RID: 22528
		[Tooltip("End time within the animation clip")]
		public float targetNormalizedTime = 1f;

		// Token: 0x04005801 RID: 22529
		private Animator animator;

		// Token: 0x04005802 RID: 22530
		private GameObject prevGameObject;
	}
}
