using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x0200163D RID: 5693
	[TaskDescription("Returns Success if the animation is currently playing.")]
	[TaskCategory("Unity/Animation")]
	public class IsPlaying : Conditional
	{
		// Token: 0x06006CA8 RID: 27816 RVA: 0x00136C10 File Offset: 0x00134E10
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006CA9 RID: 27817 RVA: 0x00136C50 File Offset: 0x00134E50
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			if (string.IsNullOrEmpty(this.animationName.Value))
			{
				if (!this.animation.isPlaying)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			}
			else
			{
				if (!this.animation.IsPlaying(this.animationName.Value))
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			}
		}

		// Token: 0x06006CAA RID: 27818 RVA: 0x00136CB5 File Offset: 0x00134EB5
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animationName.Value = "";
		}

		// Token: 0x04005870 RID: 22640
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005871 RID: 22641
		[Tooltip("The name of the animation")]
		public SharedString animationName;

		// Token: 0x04005872 RID: 22642
		private Animation animation;

		// Token: 0x04005873 RID: 22643
		private GameObject prevGameObject;
	}
}
