using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015F9 RID: 5625
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Stores the mute value of the AudioSource. Returns Success.")]
	public class GetMute : Action
	{
		// Token: 0x06006B8D RID: 27533 RVA: 0x00134218 File Offset: 0x00132418
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B8E RID: 27534 RVA: 0x00134258 File Offset: 0x00132458
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.mute;
			return TaskStatus.Success;
		}

		// Token: 0x06006B8F RID: 27535 RVA: 0x0013428B File Offset: 0x0013248B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = false;
		}

		// Token: 0x04005742 RID: 22338
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005743 RID: 22339
		[RequiredField]
		[Tooltip("The mute value of the AudioSource")]
		public SharedBool storeValue;

		// Token: 0x04005744 RID: 22340
		private AudioSource audioSource;

		// Token: 0x04005745 RID: 22341
		private GameObject prevGameObject;
	}
}
