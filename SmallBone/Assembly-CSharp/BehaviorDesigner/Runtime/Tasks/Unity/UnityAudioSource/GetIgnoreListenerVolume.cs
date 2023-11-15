using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015F5 RID: 5621
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Stores the ignore listener volume value of the AudioSource. Returns Success.")]
	public class GetIgnoreListenerVolume : Action
	{
		// Token: 0x06006B7D RID: 27517 RVA: 0x00133FF0 File Offset: 0x001321F0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B7E RID: 27518 RVA: 0x00134030 File Offset: 0x00132230
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.ignoreListenerVolume;
			return TaskStatus.Success;
		}

		// Token: 0x06006B7F RID: 27519 RVA: 0x00134063 File Offset: 0x00132263
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = false;
		}

		// Token: 0x04005732 RID: 22322
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005733 RID: 22323
		[RequiredField]
		[Tooltip("The ignore listener volume value of the AudioSource")]
		public SharedBool storeValue;

		// Token: 0x04005734 RID: 22324
		private AudioSource audioSource;

		// Token: 0x04005735 RID: 22325
		private GameObject prevGameObject;
	}
}
