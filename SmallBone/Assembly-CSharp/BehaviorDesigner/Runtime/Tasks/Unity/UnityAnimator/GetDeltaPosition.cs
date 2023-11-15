using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x0200161A RID: 5658
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Gets the avatar delta position for the last evaluated frame. Returns Success.")]
	public class GetDeltaPosition : Action
	{
		// Token: 0x06006C11 RID: 27665 RVA: 0x001354A8 File Offset: 0x001336A8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C12 RID: 27666 RVA: 0x001354E8 File Offset: 0x001336E8
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animator.deltaPosition;
			return TaskStatus.Success;
		}

		// Token: 0x06006C13 RID: 27667 RVA: 0x0013551B File Offset: 0x0013371B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = Vector3.zero;
		}

		// Token: 0x040057C7 RID: 22471
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057C8 RID: 22472
		[Tooltip("The avatar delta position")]
		[RequiredField]
		public SharedVector3 storeValue;

		// Token: 0x040057C9 RID: 22473
		private Animator animator;

		// Token: 0x040057CA RID: 22474
		private GameObject prevGameObject;
	}
}
