using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x0200161B RID: 5659
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Gets the avatar delta rotation for the last evaluated frame. Returns Success.")]
	public class GetDeltaRotation : Action
	{
		// Token: 0x06006C15 RID: 27669 RVA: 0x00135534 File Offset: 0x00133734
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C16 RID: 27670 RVA: 0x00135574 File Offset: 0x00133774
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animator.deltaRotation;
			return TaskStatus.Success;
		}

		// Token: 0x06006C17 RID: 27671 RVA: 0x001355A7 File Offset: 0x001337A7
		public override void OnReset()
		{
			if (this.storeValue != null)
			{
				this.storeValue.Value = Quaternion.identity;
			}
		}

		// Token: 0x040057CB RID: 22475
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057CC RID: 22476
		[Tooltip("The avatar delta rotation")]
		[RequiredField]
		public SharedQuaternion storeValue;

		// Token: 0x040057CD RID: 22477
		private Animator animator;

		// Token: 0x040057CE RID: 22478
		private GameObject prevGameObject;
	}
}
