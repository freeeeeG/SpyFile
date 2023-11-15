using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x0200161D RID: 5661
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Stores the current gravity weight based on current animations that are played. Returns Success.")]
	public class GetGravityWeight : Action
	{
		// Token: 0x06006C1D RID: 27677 RVA: 0x0013566C File Offset: 0x0013386C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C1E RID: 27678 RVA: 0x001356AC File Offset: 0x001338AC
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animator.gravityWeight;
			return TaskStatus.Success;
		}

		// Token: 0x06006C1F RID: 27679 RVA: 0x001356DF File Offset: 0x001338DF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x040057D4 RID: 22484
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057D5 RID: 22485
		[Tooltip("The value of the gravity weight")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x040057D6 RID: 22486
		private Animator animator;

		// Token: 0x040057D7 RID: 22487
		private GameObject prevGameObject;
	}
}
