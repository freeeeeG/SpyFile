using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001602 RID: 5634
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Plays the audio clip. Returns Success.")]
	public class Play : Action
	{
		// Token: 0x06006BB1 RID: 27569 RVA: 0x001346D4 File Offset: 0x001328D4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BB2 RID: 27570 RVA: 0x00134714 File Offset: 0x00132914
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.Play();
			return TaskStatus.Success;
		}

		// Token: 0x06006BB3 RID: 27571 RVA: 0x0013473C File Offset: 0x0013293C
		public override void OnReset()
		{
			this.targetGameObject = null;
		}

		// Token: 0x04005764 RID: 22372
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005765 RID: 22373
		private AudioSource audioSource;

		// Token: 0x04005766 RID: 22374
		private GameObject prevGameObject;
	}
}
