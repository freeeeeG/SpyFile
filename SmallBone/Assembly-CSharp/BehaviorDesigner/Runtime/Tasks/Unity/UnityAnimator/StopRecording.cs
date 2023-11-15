using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001637 RID: 5687
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Stops animator record mode. Returns Success.")]
	public class StopRecording : Action
	{
		// Token: 0x06006C8F RID: 27791 RVA: 0x001366C4 File Offset: 0x001348C4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C90 RID: 27792 RVA: 0x00136704 File Offset: 0x00134904
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.StopRecording();
			return TaskStatus.Success;
		}

		// Token: 0x06006C91 RID: 27793 RVA: 0x0013672C File Offset: 0x0013492C
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x0400584F RID: 22607
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005850 RID: 22608
		private Animator animator;

		// Token: 0x04005851 RID: 22609
		private GameObject prevGameObject;
	}
}
