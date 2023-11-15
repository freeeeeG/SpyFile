using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001635 RID: 5685
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Sets the animator in recording mode. Returns Success.")]
	public class StartRecording : Action
	{
		// Token: 0x06006C87 RID: 27783 RVA: 0x001365D0 File Offset: 0x001347D0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C88 RID: 27784 RVA: 0x00136610 File Offset: 0x00134810
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.StartRecording(this.frameCount);
			return TaskStatus.Success;
		}

		// Token: 0x06006C89 RID: 27785 RVA: 0x0013663E File Offset: 0x0013483E
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.frameCount = 0;
		}

		// Token: 0x04005848 RID: 22600
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005849 RID: 22601
		[Tooltip("The number of frames (updates) that will be recorded")]
		public int frameCount;

		// Token: 0x0400584A RID: 22602
		private Animator animator;

		// Token: 0x0400584B RID: 22603
		private GameObject prevGameObject;
	}
}
