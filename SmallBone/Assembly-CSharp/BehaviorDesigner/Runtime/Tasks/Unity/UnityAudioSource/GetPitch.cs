using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015FA RID: 5626
	[TaskDescription("Stores the pitch value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class GetPitch : Action
	{
		// Token: 0x06006B91 RID: 27537 RVA: 0x001342A0 File Offset: 0x001324A0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B92 RID: 27538 RVA: 0x001342E0 File Offset: 0x001324E0
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.pitch;
			return TaskStatus.Success;
		}

		// Token: 0x06006B93 RID: 27539 RVA: 0x00134313 File Offset: 0x00132513
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 1f;
		}

		// Token: 0x04005746 RID: 22342
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005747 RID: 22343
		[RequiredField]
		[Tooltip("The pitch value of the AudioSource")]
		public SharedFloat storeValue;

		// Token: 0x04005748 RID: 22344
		private AudioSource audioSource;

		// Token: 0x04005749 RID: 22345
		private GameObject prevGameObject;
	}
}
