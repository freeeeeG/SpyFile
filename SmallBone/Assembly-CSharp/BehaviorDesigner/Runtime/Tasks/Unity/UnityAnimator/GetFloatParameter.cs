using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x0200161C RID: 5660
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Stores the float parameter on an animator. Returns Success.")]
	public class GetFloatParameter : Action
	{
		// Token: 0x06006C19 RID: 27673 RVA: 0x001355C4 File Offset: 0x001337C4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C1A RID: 27674 RVA: 0x00135604 File Offset: 0x00133804
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animator.GetFloat(this.paramaterName.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006C1B RID: 27675 RVA: 0x00135642 File Offset: 0x00133842
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.paramaterName = "";
			this.storeValue = 0f;
		}

		// Token: 0x040057CF RID: 22479
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057D0 RID: 22480
		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

		// Token: 0x040057D1 RID: 22481
		[Tooltip("The value of the float parameter")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x040057D2 RID: 22482
		private Animator animator;

		// Token: 0x040057D3 RID: 22483
		private GameObject prevGameObject;
	}
}
