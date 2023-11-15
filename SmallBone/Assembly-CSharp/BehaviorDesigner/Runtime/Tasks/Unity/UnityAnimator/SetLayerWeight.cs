using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x0200162F RID: 5679
	[TaskDescription("Sets the layer's current weight. Returns Success.")]
	[TaskCategory("Unity/Animator")]
	public class SetLayerWeight : Action
	{
		// Token: 0x06006C6D RID: 27757 RVA: 0x00136238 File Offset: 0x00134438
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C6E RID: 27758 RVA: 0x00136278 File Offset: 0x00134478
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.SetLayerWeight(this.index.Value, this.weight.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006C6F RID: 27759 RVA: 0x001362B6 File Offset: 0x001344B6
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.index = 0;
			this.weight = 0f;
		}

		// Token: 0x0400582E RID: 22574
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400582F RID: 22575
		[Tooltip("The layer's index")]
		public SharedInt index;

		// Token: 0x04005830 RID: 22576
		[Tooltip("The weight of the layer")]
		public SharedFloat weight;

		// Token: 0x04005831 RID: 22577
		private Animator animator;

		// Token: 0x04005832 RID: 22578
		private GameObject prevGameObject;
	}
}
