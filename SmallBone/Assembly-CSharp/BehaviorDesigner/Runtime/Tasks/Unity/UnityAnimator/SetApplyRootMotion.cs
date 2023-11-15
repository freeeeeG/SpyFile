using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001628 RID: 5672
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Sets if root motion is applied. Returns Success.")]
	public class SetApplyRootMotion : Action
	{
		// Token: 0x06006C48 RID: 27720 RVA: 0x00135D5C File Offset: 0x00133F5C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C49 RID: 27721 RVA: 0x00135D9C File Offset: 0x00133F9C
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.applyRootMotion = this.rootMotion.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006C4A RID: 27722 RVA: 0x00135DCF File Offset: 0x00133FCF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.rootMotion = false;
		}

		// Token: 0x04005809 RID: 22537
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400580A RID: 22538
		[Tooltip("Is root motion applied?")]
		public SharedBool rootMotion;

		// Token: 0x0400580B RID: 22539
		private Animator animator;

		// Token: 0x0400580C RID: 22540
		private GameObject prevGameObject;
	}
}
