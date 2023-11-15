using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x0200163A RID: 5690
	[TaskCategory("Unity/Animation")]
	[TaskDescription("Fades the animation over a period of time and fades other animations out. Returns Success.")]
	public class CrossFade : Action
	{
		// Token: 0x06006C9C RID: 27804 RVA: 0x00136938 File Offset: 0x00134B38
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C9D RID: 27805 RVA: 0x00136978 File Offset: 0x00134B78
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			this.animation[this.animationName.Value].speed = this.animationSpeed.Value;
			if (this.animation[this.animationName.Value].speed < 0f)
			{
				this.animation[this.animationName.Value].time = this.animation[this.animationName.Value].length;
			}
			this.animation.CrossFade(this.animationName.Value, this.fadeLength.Value, this.playMode);
			return TaskStatus.Success;
		}

		// Token: 0x06006C9E RID: 27806 RVA: 0x00136A45 File Offset: 0x00134C45
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animationName.Value = "";
			this.animationSpeed = 1f;
			this.fadeLength = 0.3f;
			this.playMode = PlayMode.StopSameLayer;
		}

		// Token: 0x0400585E RID: 22622
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400585F RID: 22623
		[Tooltip("The name of the animation")]
		public SharedString animationName;

		// Token: 0x04005860 RID: 22624
		[Tooltip("The speed of the animation. Use a negative value to play the animation backwards")]
		public SharedFloat animationSpeed = 1f;

		// Token: 0x04005861 RID: 22625
		[Tooltip("The amount of time it takes to blend")]
		public SharedFloat fadeLength = 0.3f;

		// Token: 0x04005862 RID: 22626
		[Tooltip("The play mode of the animation")]
		public PlayMode playMode;

		// Token: 0x04005863 RID: 22627
		private Animation animation;

		// Token: 0x04005864 RID: 22628
		private GameObject prevGameObject;
	}
}
