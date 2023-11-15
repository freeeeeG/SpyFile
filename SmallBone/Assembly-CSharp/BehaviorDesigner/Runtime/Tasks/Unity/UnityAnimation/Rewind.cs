using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x02001640 RID: 5696
	[TaskDescription("Rewinds an animation. Rewinds all animations if animationName is blank. Returns Success.")]
	[TaskCategory("Unity/Animation")]
	public class Rewind : Action
	{
		// Token: 0x06006CB4 RID: 27828 RVA: 0x00136E40 File Offset: 0x00135040
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006CB5 RID: 27829 RVA: 0x00136E80 File Offset: 0x00135080
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			if (string.IsNullOrEmpty(this.animationName.Value))
			{
				this.animation.Rewind();
			}
			else
			{
				this.animation.Rewind(this.animationName.Value);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006CB6 RID: 27830 RVA: 0x00136EDD File Offset: 0x001350DD
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animationName.Value = "";
		}

		// Token: 0x0400587F RID: 22655
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005880 RID: 22656
		[Tooltip("The name of the animation")]
		public SharedString animationName;

		// Token: 0x04005881 RID: 22657
		private Animation animation;

		// Token: 0x04005882 RID: 22658
		private GameObject prevGameObject;
	}
}
