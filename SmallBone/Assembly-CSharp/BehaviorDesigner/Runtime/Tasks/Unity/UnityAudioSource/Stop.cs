using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001616 RID: 5654
	[TaskDescription("Stops playing the audio clip. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class Stop : Action
	{
		// Token: 0x06006C01 RID: 27649 RVA: 0x0013521C File Offset: 0x0013341C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006C02 RID: 27650 RVA: 0x0013525C File Offset: 0x0013345C
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.Stop();
			return TaskStatus.Success;
		}

		// Token: 0x06006C03 RID: 27651 RVA: 0x00135284 File Offset: 0x00133484
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x040057B4 RID: 22452
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057B5 RID: 22453
		private AudioSource audioSource;

		// Token: 0x040057B6 RID: 22454
		private GameObject prevGameObject;
	}
}
