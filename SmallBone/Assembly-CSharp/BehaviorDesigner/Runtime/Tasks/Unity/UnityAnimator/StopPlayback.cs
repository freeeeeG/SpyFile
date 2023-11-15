using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001636 RID: 5686
	[TaskDescription("Stops the animator playback mode. Returns Success.")]
	[TaskCategory("Unity/Animator")]
	public class StopPlayback : Action
	{
		// Token: 0x06006C8B RID: 27787 RVA: 0x00136650 File Offset: 0x00134850
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C8C RID: 27788 RVA: 0x00136690 File Offset: 0x00134890
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.StopPlayback();
			return TaskStatus.Success;
		}

		// Token: 0x06006C8D RID: 27789 RVA: 0x001366B8 File Offset: 0x001348B8
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x0400584C RID: 22604
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400584D RID: 22605
		private Animator animator;

		// Token: 0x0400584E RID: 22606
		private GameObject prevGameObject;
	}
}
