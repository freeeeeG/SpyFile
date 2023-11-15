using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001603 RID: 5635
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Plays the audio clip with a delay specified in seconds. Returns Success.")]
	public class PlayDelayed : Action
	{
		// Token: 0x06006BB5 RID: 27573 RVA: 0x00134748 File Offset: 0x00132948
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BB6 RID: 27574 RVA: 0x00134788 File Offset: 0x00132988
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.PlayDelayed(this.delay.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006BB7 RID: 27575 RVA: 0x001347BB File Offset: 0x001329BB
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.delay = 0f;
		}

		// Token: 0x04005767 RID: 22375
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005768 RID: 22376
		[Tooltip("Delay time specified in seconds")]
		public SharedFloat delay = 0f;

		// Token: 0x04005769 RID: 22377
		private AudioSource audioSource;

		// Token: 0x0400576A RID: 22378
		private GameObject prevGameObject;
	}
}
