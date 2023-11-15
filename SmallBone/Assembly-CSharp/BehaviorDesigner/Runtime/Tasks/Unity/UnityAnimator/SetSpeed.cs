using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001632 RID: 5682
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Sets the playback speed of the Animator. 1 is normal playback speed. Returns Success.")]
	public class SetSpeed : Action
	{
		// Token: 0x06006C7B RID: 27771 RVA: 0x00136444 File Offset: 0x00134644
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C7C RID: 27772 RVA: 0x00136484 File Offset: 0x00134684
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.speed = this.speed.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006C7D RID: 27773 RVA: 0x001364B7 File Offset: 0x001346B7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.speed = 0f;
		}

		// Token: 0x0400583D RID: 22589
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400583E RID: 22590
		[Tooltip("The playback speed of the Animator")]
		public SharedFloat speed;

		// Token: 0x0400583F RID: 22591
		private Animator animator;

		// Token: 0x04005840 RID: 22592
		private GameObject prevGameObject;
	}
}
