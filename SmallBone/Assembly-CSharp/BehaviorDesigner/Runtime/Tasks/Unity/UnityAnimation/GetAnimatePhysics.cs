using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x0200163C RID: 5692
	[TaskDescription("Stores the animate physics value. Returns Success.")]
	[TaskCategory("Unity/Animation")]
	public class GetAnimatePhysics : Action
	{
		// Token: 0x06006CA4 RID: 27812 RVA: 0x00136B88 File Offset: 0x00134D88
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006CA5 RID: 27813 RVA: 0x00136BC8 File Offset: 0x00134DC8
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.animation.animatePhysics;
			return TaskStatus.Success;
		}

		// Token: 0x06006CA6 RID: 27814 RVA: 0x00136BFB File Offset: 0x00134DFB
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue.Value = false;
		}

		// Token: 0x0400586C RID: 22636
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400586D RID: 22637
		[RequiredField]
		[Tooltip("Are the if animations are executed in the physics loop?")]
		public SharedBool storeValue;

		// Token: 0x0400586E RID: 22638
		private Animation animation;

		// Token: 0x0400586F RID: 22639
		private GameObject prevGameObject;
	}
}
