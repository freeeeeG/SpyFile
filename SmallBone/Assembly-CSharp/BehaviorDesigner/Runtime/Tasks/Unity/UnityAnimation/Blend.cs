using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x02001639 RID: 5689
	[TaskDescription("Blends the animation. Returns Success.")]
	[TaskCategory("Unity/Animation")]
	public class Blend : Action
	{
		// Token: 0x06006C98 RID: 27800 RVA: 0x0013686C File Offset: 0x00134A6C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C99 RID: 27801 RVA: 0x001368AC File Offset: 0x00134AAC
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			this.animation.Blend(this.animationName.Value, this.targetWeight, this.fadeLength);
			return TaskStatus.Success;
		}

		// Token: 0x06006C9A RID: 27802 RVA: 0x001368EB File Offset: 0x00134AEB
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animationName = "";
			this.targetWeight = 1f;
			this.fadeLength = 0.3f;
		}

		// Token: 0x04005858 RID: 22616
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005859 RID: 22617
		[Tooltip("The name of the animation")]
		public SharedString animationName;

		// Token: 0x0400585A RID: 22618
		[Tooltip("The weight the animation should blend to")]
		public float targetWeight = 1f;

		// Token: 0x0400585B RID: 22619
		[Tooltip("The amount of time it takes to blend")]
		public float fadeLength = 0.3f;

		// Token: 0x0400585C RID: 22620
		private Animation animation;

		// Token: 0x0400585D RID: 22621
		private GameObject prevGameObject;
	}
}
