using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001619 RID: 5657
	[TaskDescription("Stores the bool parameter on an animator. Returns Success.")]
	[TaskCategory("Unity/Animator")]
	public class GetBoolParameter : Action
	{
		// Token: 0x06006C0D RID: 27661 RVA: 0x00135404 File Offset: 0x00133604
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C0E RID: 27662 RVA: 0x00135444 File Offset: 0x00133644
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animator.GetBool(this.paramaterName.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006C0F RID: 27663 RVA: 0x00135482 File Offset: 0x00133682
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.paramaterName = "";
			this.storeValue = false;
		}

		// Token: 0x040057C2 RID: 22466
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057C3 RID: 22467
		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

		// Token: 0x040057C4 RID: 22468
		[Tooltip("The value of the bool parameter")]
		[RequiredField]
		public SharedBool storeValue;

		// Token: 0x040057C5 RID: 22469
		private Animator animator;

		// Token: 0x040057C6 RID: 22470
		private GameObject prevGameObject;
	}
}
