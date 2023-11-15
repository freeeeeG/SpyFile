using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x02001643 RID: 5699
	[TaskDescription("Sets the wrap mode to the specified value. Returns Success.")]
	[TaskCategory("Unity/Animation")]
	public class SetWrapMode : Action
	{
		// Token: 0x06006CC0 RID: 27840 RVA: 0x00136FF4 File Offset: 0x001351F4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006CC1 RID: 27841 RVA: 0x00137034 File Offset: 0x00135234
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			this.animation.wrapMode = this.wrapMode;
			return TaskStatus.Success;
		}

		// Token: 0x06006CC2 RID: 27842 RVA: 0x00137062 File Offset: 0x00135262
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.wrapMode = WrapMode.Default;
		}

		// Token: 0x0400588A RID: 22666
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400588B RID: 22667
		[Tooltip("How should time beyond the playback range of the clip be treated?")]
		public WrapMode wrapMode;

		// Token: 0x0400588C RID: 22668
		private Animation animation;

		// Token: 0x0400588D RID: 22669
		private GameObject prevGameObject;
	}
}
