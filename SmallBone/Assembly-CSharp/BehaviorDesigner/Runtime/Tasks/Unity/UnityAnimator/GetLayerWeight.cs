using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x0200161F RID: 5663
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Stores the layer's weight. Returns Success.")]
	public class GetLayerWeight : Action
	{
		// Token: 0x06006C25 RID: 27685 RVA: 0x0013579C File Offset: 0x0013399C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C26 RID: 27686 RVA: 0x001357DC File Offset: 0x001339DC
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animator.GetLayerWeight(this.index.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006C27 RID: 27687 RVA: 0x0013581A File Offset: 0x00133A1A
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.index = 0;
			this.storeValue = 0f;
		}

		// Token: 0x040057DD RID: 22493
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057DE RID: 22494
		[Tooltip("The index of the layer")]
		public SharedInt index;

		// Token: 0x040057DF RID: 22495
		[RequiredField]
		[Tooltip("The value of the float parameter")]
		public SharedFloat storeValue;

		// Token: 0x040057E0 RID: 22496
		private Animator animator;

		// Token: 0x040057E1 RID: 22497
		private GameObject prevGameObject;
	}
}
