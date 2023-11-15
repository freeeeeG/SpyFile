using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001618 RID: 5656
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Stores if root motion is applied. Returns Success.")]
	public class GetApplyRootMotion : Action
	{
		// Token: 0x06006C09 RID: 27657 RVA: 0x0013537C File Offset: 0x0013357C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C0A RID: 27658 RVA: 0x001353BC File Offset: 0x001335BC
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animator.applyRootMotion;
			return TaskStatus.Success;
		}

		// Token: 0x06006C0B RID: 27659 RVA: 0x001353EF File Offset: 0x001335EF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = false;
		}

		// Token: 0x040057BE RID: 22462
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057BF RID: 22463
		[RequiredField]
		[Tooltip("Is root motion applied?")]
		public SharedBool storeValue;

		// Token: 0x040057C0 RID: 22464
		private Animator animator;

		// Token: 0x040057C1 RID: 22465
		private GameObject prevGameObject;
	}
}
