using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x0200163E RID: 5694
	[TaskCategory("Unity/Animation")]
	[TaskDescription("Plays animation without any blending. Returns Success.")]
	public class Play : Action
	{
		// Token: 0x06006CAC RID: 27820 RVA: 0x00136CD0 File Offset: 0x00134ED0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006CAD RID: 27821 RVA: 0x00136D10 File Offset: 0x00134F10
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			if (string.IsNullOrEmpty(this.animationName.Value))
			{
				this.animation.Play();
			}
			else
			{
				this.animation.Play(this.animationName.Value, this.playMode);
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006CAE RID: 27822 RVA: 0x00136D75 File Offset: 0x00134F75
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animationName.Value = "";
			this.playMode = PlayMode.StopSameLayer;
		}

		// Token: 0x04005874 RID: 22644
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005875 RID: 22645
		[Tooltip("The name of the animation")]
		public SharedString animationName;

		// Token: 0x04005876 RID: 22646
		[Tooltip("The play mode of the animation")]
		public PlayMode playMode;

		// Token: 0x04005877 RID: 22647
		private Animation animation;

		// Token: 0x04005878 RID: 22648
		private GameObject prevGameObject;
	}
}
