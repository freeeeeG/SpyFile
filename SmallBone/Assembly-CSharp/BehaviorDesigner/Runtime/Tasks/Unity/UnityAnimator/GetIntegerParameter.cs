using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x0200161E RID: 5662
	[TaskDescription("Stores the integer parameter on an animator. Returns Success.")]
	[TaskCategory("Unity/Animator")]
	public class GetIntegerParameter : Action
	{
		// Token: 0x06006C21 RID: 27681 RVA: 0x001356F8 File Offset: 0x001338F8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C22 RID: 27682 RVA: 0x00135738 File Offset: 0x00133938
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animator.GetInteger(this.paramaterName.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006C23 RID: 27683 RVA: 0x00135776 File Offset: 0x00133976
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.paramaterName = "";
			this.storeValue = 0;
		}

		// Token: 0x040057D8 RID: 22488
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057D9 RID: 22489
		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

		// Token: 0x040057DA RID: 22490
		[RequiredField]
		[Tooltip("The value of the integer parameter")]
		public SharedInt storeValue;

		// Token: 0x040057DB RID: 22491
		private Animator animator;

		// Token: 0x040057DC RID: 22492
		private GameObject prevGameObject;
	}
}
