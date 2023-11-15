using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001605 RID: 5637
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Plays the audio clip with a delay specified in seconds. Returns Success.")]
	public class PlayScheduled : Action
	{
		// Token: 0x06006BBD RID: 27581 RVA: 0x001348B4 File Offset: 0x00132AB4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BBE RID: 27582 RVA: 0x001348F4 File Offset: 0x00132AF4
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.PlayScheduled((double)this.time.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006BBF RID: 27583 RVA: 0x00134928 File Offset: 0x00132B28
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.time = 0f;
		}

		// Token: 0x04005770 RID: 22384
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005771 RID: 22385
		[Tooltip("Time in seconds on the absolute time-line that AudioSettings.dspTime refers to for when the sound should start playing")]
		public SharedFloat time = 0f;

		// Token: 0x04005772 RID: 22386
		private AudioSource audioSource;

		// Token: 0x04005773 RID: 22387
		private GameObject prevGameObject;
	}
}
