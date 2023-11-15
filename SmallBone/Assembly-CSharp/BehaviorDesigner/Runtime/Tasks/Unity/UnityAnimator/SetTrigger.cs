using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001633 RID: 5683
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Sets a trigger parameter to active or inactive. Returns Success.")]
	public class SetTrigger : Action
	{
		// Token: 0x06006C7F RID: 27775 RVA: 0x001364D0 File Offset: 0x001346D0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C80 RID: 27776 RVA: 0x00136510 File Offset: 0x00134710
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.SetTrigger(this.paramaterName.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006C81 RID: 27777 RVA: 0x00136543 File Offset: 0x00134743
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.paramaterName = "";
		}

		// Token: 0x04005841 RID: 22593
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005842 RID: 22594
		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

		// Token: 0x04005843 RID: 22595
		private Animator animator;

		// Token: 0x04005844 RID: 22596
		private GameObject prevGameObject;
	}
}
