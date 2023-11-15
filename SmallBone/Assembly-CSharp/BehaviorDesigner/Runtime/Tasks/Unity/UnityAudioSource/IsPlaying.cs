using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001600 RID: 5632
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Returns Success if the AudioClip is playing, otherwise Failure.")]
	public class IsPlaying : Conditional
	{
		// Token: 0x06006BA9 RID: 27561 RVA: 0x001345E8 File Offset: 0x001327E8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BAA RID: 27562 RVA: 0x00134628 File Offset: 0x00132828
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			if (!this.audioSource.isPlaying)
			{
				return TaskStatus.Failure;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006BAB RID: 27563 RVA: 0x00134654 File Offset: 0x00132854
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x0400575E RID: 22366
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400575F RID: 22367
		private AudioSource audioSource;

		// Token: 0x04005760 RID: 22368
		private GameObject prevGameObject;
	}
}
