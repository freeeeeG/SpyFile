using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001634 RID: 5684
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Sets the animator in playback mode.")]
	public class StartPlayback : Action
	{
		// Token: 0x06006C83 RID: 27779 RVA: 0x0013655C File Offset: 0x0013475C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C84 RID: 27780 RVA: 0x0013659C File Offset: 0x0013479C
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.StartPlayback();
			return TaskStatus.Success;
		}

		// Token: 0x06006C85 RID: 27781 RVA: 0x001365C4 File Offset: 0x001347C4
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x04005845 RID: 22597
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005846 RID: 22598
		private Animator animator;

		// Token: 0x04005847 RID: 22599
		private GameObject prevGameObject;
	}
}
