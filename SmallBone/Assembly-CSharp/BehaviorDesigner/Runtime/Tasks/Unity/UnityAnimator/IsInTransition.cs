using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001623 RID: 5667
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Returns success if the specified AnimatorController layer in a transition.")]
	public class IsInTransition : Conditional
	{
		// Token: 0x06006C34 RID: 27700 RVA: 0x00135998 File Offset: 0x00133B98
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C35 RID: 27701 RVA: 0x001359D8 File Offset: 0x00133BD8
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			if (!this.animator.IsInTransition(this.index.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006C36 RID: 27702 RVA: 0x00135A0F File Offset: 0x00133C0F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.index = 0;
		}

		// Token: 0x040057EC RID: 22508
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057ED RID: 22509
		[Tooltip("The layer's index")]
		public SharedInt index;

		// Token: 0x040057EE RID: 22510
		private Animator animator;

		// Token: 0x040057EF RID: 22511
		private GameObject prevGameObject;
	}
}
