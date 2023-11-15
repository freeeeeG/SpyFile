using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x0200163F RID: 5695
	[TaskCategory("Unity/Animation")]
	[TaskDescription("Plays an animation after previous animations has finished playing. Returns Success.")]
	public class PlayQueued : Action
	{
		// Token: 0x06006CB0 RID: 27824 RVA: 0x00136D98 File Offset: 0x00134F98
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006CB1 RID: 27825 RVA: 0x00136DD8 File Offset: 0x00134FD8
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			this.animation.PlayQueued(this.animationName.Value, this.queue, this.playMode);
			return TaskStatus.Success;
		}

		// Token: 0x06006CB2 RID: 27826 RVA: 0x00136E18 File Offset: 0x00135018
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animationName.Value = "";
			this.queue = QueueMode.CompleteOthers;
			this.playMode = PlayMode.StopSameLayer;
		}

		// Token: 0x04005879 RID: 22649
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400587A RID: 22650
		[Tooltip("The name of the animation")]
		public SharedString animationName;

		// Token: 0x0400587B RID: 22651
		[Tooltip("Specifies when the animation should start playing")]
		public QueueMode queue;

		// Token: 0x0400587C RID: 22652
		[Tooltip("The play mode of the animation")]
		public PlayMode playMode;

		// Token: 0x0400587D RID: 22653
		private Animation animation;

		// Token: 0x0400587E RID: 22654
		private GameObject prevGameObject;
	}
}
