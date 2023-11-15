using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001617 RID: 5655
	[TaskCategory("Unity/Animator")]
	[TaskDescription("Creates a dynamic transition between the current state and the destination state. Returns Success.")]
	public class CrossFade : Action
	{
		// Token: 0x06006C05 RID: 27653 RVA: 0x00135290 File Offset: 0x00133490
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C06 RID: 27654 RVA: 0x001352D0 File Offset: 0x001334D0
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.CrossFade(this.stateName.Value, this.transitionDuration.Value, this.layer, this.normalizedTime);
			return TaskStatus.Success;
		}

		// Token: 0x06006C07 RID: 27655 RVA: 0x00135325 File Offset: 0x00133525
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.stateName = "";
			this.transitionDuration = 0f;
			this.layer = -1;
			this.normalizedTime = float.NegativeInfinity;
		}

		// Token: 0x040057B7 RID: 22455
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057B8 RID: 22456
		[Tooltip("The name of the state")]
		public SharedString stateName;

		// Token: 0x040057B9 RID: 22457
		[Tooltip("The duration of the transition. Value is in source state normalized time")]
		public SharedFloat transitionDuration;

		// Token: 0x040057BA RID: 22458
		[Tooltip("The layer where the state is")]
		public int layer = -1;

		// Token: 0x040057BB RID: 22459
		[Tooltip("The normalized time at which the state will play")]
		public float normalizedTime = float.NegativeInfinity;

		// Token: 0x040057BC RID: 22460
		private Animator animator;

		// Token: 0x040057BD RID: 22461
		private GameObject prevGameObject;
	}
}
