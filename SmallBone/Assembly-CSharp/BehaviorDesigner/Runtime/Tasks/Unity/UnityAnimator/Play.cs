using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimator
{
	// Token: 0x02001627 RID: 5671
	[TaskDescription("Plays an animator state. Returns Success.")]
	[TaskCategory("Unity/Animator")]
	public class Play : Action
	{
		// Token: 0x06006C44 RID: 27716 RVA: 0x00135C98 File Offset: 0x00133E98
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animator = defaultGameObject.GetComponent<Animator>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C45 RID: 27717 RVA: 0x00135CD8 File Offset: 0x00133ED8
		public override TaskStatus OnUpdate()
		{
			if (this.animator == null)
			{
				Debug.LogWarning("Animator is null");
				return TaskStatus.Failure;
			}
			this.animator.Play(this.stateName.Value, this.layer, this.normalizedTime);
			return TaskStatus.Success;
		}

		// Token: 0x06006C46 RID: 27718 RVA: 0x00135D17 File Offset: 0x00133F17
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.stateName = "";
			this.layer = -1;
			this.normalizedTime = float.NegativeInfinity;
		}

		// Token: 0x04005803 RID: 22531
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005804 RID: 22532
		[Tooltip("The name of the state")]
		public SharedString stateName;

		// Token: 0x04005805 RID: 22533
		[Tooltip("The layer where the state is")]
		public int layer = -1;

		// Token: 0x04005806 RID: 22534
		[Tooltip("The normalized time at which the state will play")]
		public float normalizedTime = float.NegativeInfinity;

		// Token: 0x04005807 RID: 22535
		private Animator animator;

		// Token: 0x04005808 RID: 22536
		private GameObject prevGameObject;
	}
}
