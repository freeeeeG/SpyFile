using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAnimation
{
	// Token: 0x0200163B RID: 5691
	[TaskCategory("Unity/Animation")]
	[TaskDescription("Cross fades an animation after previous animations has finished playing. Returns Success.")]
	public class CrossFadeQueued : Action
	{
		// Token: 0x06006CA0 RID: 27808 RVA: 0x00136AB0 File Offset: 0x00134CB0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.animation = defaultGameObject.GetComponent<Animation>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006CA1 RID: 27809 RVA: 0x00136AF0 File Offset: 0x00134CF0
		public override TaskStatus OnUpdate()
		{
			if (this.animation == null)
			{
				Debug.LogWarning("Animation is null");
				return TaskStatus.Failure;
			}
			this.animation.CrossFadeQueued(this.animationName.Value, this.fadeLength, this.queue, this.playMode);
			return TaskStatus.Success;
		}

		// Token: 0x06006CA2 RID: 27810 RVA: 0x00136B41 File Offset: 0x00134D41
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.animationName.Value = "";
			this.fadeLength = 0.3f;
			this.queue = QueueMode.CompleteOthers;
			this.playMode = PlayMode.StopSameLayer;
		}

		// Token: 0x04005865 RID: 22629
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005866 RID: 22630
		[Tooltip("The name of the animation")]
		public SharedString animationName;

		// Token: 0x04005867 RID: 22631
		[Tooltip("The amount of time it takes to blend")]
		public float fadeLength = 0.3f;

		// Token: 0x04005868 RID: 22632
		[Tooltip("Specifies when the animation should start playing")]
		public QueueMode queue;

		// Token: 0x04005869 RID: 22633
		[Tooltip("The play mode of the animation")]
		public PlayMode playMode;

		// Token: 0x0400586A RID: 22634
		private Animation animation;

		// Token: 0x0400586B RID: 22635
		private GameObject prevGameObject;
	}
}
