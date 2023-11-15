using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001625 RID: 5669
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Returns success if the specified parameter is controlled by an additional curve on an animation.")]
	public class IsParameterControlledByCurve : Conditional
	{
		// Token: 0x06006C3C RID: 27708 RVA: 0x00135AE0 File Offset: 0x00133CE0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C3D RID: 27709 RVA: 0x00135B20 File Offset: 0x00133D20
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			if (!this.animator.IsParameterControlledByCurve(this.paramaterName.Value))
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006C3E RID: 27710 RVA: 0x00135B57 File Offset: 0x00133D57
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.paramaterName = "";
		}

		// Token: 0x040057F5 RID: 22517
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057F6 RID: 22518
		[Tooltip("The name of the parameter")]
		public SharedString paramaterName;

		// Token: 0x040057F7 RID: 22519
		private Animator animator;

		// Token: 0x040057F8 RID: 22520
		private GameObject prevGameObject;
	}
}
