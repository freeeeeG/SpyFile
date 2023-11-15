using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001601 RID: 5633
	[TaskDescription("Pauses the audio clip. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class Pause : Action
	{
		// Token: 0x06006BAD RID: 27565 RVA: 0x00134660 File Offset: 0x00132860
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BAE RID: 27566 RVA: 0x001346A0 File Offset: 0x001328A0
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.Pause();
			return TaskStatus.Success;
		}

		// Token: 0x06006BAF RID: 27567 RVA: 0x001346C8 File Offset: 0x001328C8
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x04005761 RID: 22369
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005762 RID: 22370
		private AudioSource audioSource;

		// Token: 0x04005763 RID: 22371
		private GameObject prevGameObject;
	}
}
