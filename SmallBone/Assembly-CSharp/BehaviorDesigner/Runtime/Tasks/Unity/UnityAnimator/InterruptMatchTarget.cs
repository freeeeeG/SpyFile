using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001622 RID: 5666
	[TaskDescription("Interrupts the automatic target matching. Returns Success.")]
	[TaskCategory("Unity/Animator")]
	public class InterruptMatchTarget : Action
	{
		// Token: 0x06006C30 RID: 27696 RVA: 0x00135908 File Offset: 0x00133B08
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C31 RID: 27697 RVA: 0x00135948 File Offset: 0x00133B48
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.InterruptMatchTarget(this.completeMatch);
			return TaskStatus.Success;
		}

		// Token: 0x06006C32 RID: 27698 RVA: 0x00135976 File Offset: 0x00133B76
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.completeMatch = true;
		}

		// Token: 0x040057E8 RID: 22504
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057E9 RID: 22505
		[Tooltip("CompleteMatch will make the gameobject match the target completely at the next frame")]
		public bool completeMatch = true;

		// Token: 0x040057EA RID: 22506
		private Animator animator;

		// Token: 0x040057EB RID: 22507
		private GameObject prevGameObject;
	}
}
