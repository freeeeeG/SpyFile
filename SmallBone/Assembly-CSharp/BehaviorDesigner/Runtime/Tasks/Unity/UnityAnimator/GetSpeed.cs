using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001620 RID: 5664
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Stores the playback speed of the animator. 1 is normal playback speed. Returns Success.")]
	public class GetSpeed : Action
	{
		// Token: 0x06006C29 RID: 27689 RVA: 0x00135840 File Offset: 0x00133A40
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C2A RID: 27690 RVA: 0x00135880 File Offset: 0x00133A80
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animator.speed;
			return TaskStatus.Success;
		}

		// Token: 0x06006C2B RID: 27691 RVA: 0x001358B3 File Offset: 0x00133AB3
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 0f;
		}

		// Token: 0x040057E2 RID: 22498
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057E3 RID: 22499
		[RequiredField]
		[Tooltip("The playback speed of the Animator")]
		public SharedFloat storeValue;

		// Token: 0x040057E4 RID: 22500
		private Animator animator;

		// Token: 0x040057E5 RID: 22501
		private GameObject prevGameObject;
	}
}
