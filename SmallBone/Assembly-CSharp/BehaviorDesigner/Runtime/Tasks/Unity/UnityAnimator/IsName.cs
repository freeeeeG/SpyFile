using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001624 RID: 5668
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Returns success if the specified name matches the name of the active state.")]
	public class IsName : Conditional
	{
		// Token: 0x06006C38 RID: 27704 RVA: 0x00135A24 File Offset: 0x00133C24
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C39 RID: 27705 RVA: 0x00135A64 File Offset: 0x00133C64
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			if (!this.animator.GetCurrentAnimatorStateInfo(this.index.Value).IsName(this.name.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006C3A RID: 27706 RVA: 0x00135AB9 File Offset: 0x00133CB9
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.index = 0;
			this.name = "";
		}

		// Token: 0x040057F0 RID: 22512
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057F1 RID: 22513
		[Tooltip("The layer's index")]
		public SharedInt index;

		// Token: 0x040057F2 RID: 22514
		[Tooltip("The state name to compare")]
		public SharedString name;

		// Token: 0x040057F3 RID: 22515
		private Animator animator;

		// Token: 0x040057F4 RID: 22516
		private GameObject prevGameObject;
	}
}
