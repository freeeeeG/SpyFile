using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015FD RID: 5629
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Stores the time value of the AudioSource. Returns Success.")]
	public class GetTime : Action
	{
		// Token: 0x06006B9D RID: 27549 RVA: 0x00134440 File Offset: 0x00132640
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B9E RID: 27550 RVA: 0x00134480 File Offset: 0x00132680
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.time;
			return TaskStatus.Success;
		}

		// Token: 0x06006B9F RID: 27551 RVA: 0x001344B3 File Offset: 0x001326B3
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 1f;
		}

		// Token: 0x04005752 RID: 22354
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005753 RID: 22355
		[Tooltip("The time value of the AudioSource")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005754 RID: 22356
		private AudioSource audioSource;

		// Token: 0x04005755 RID: 22357
		private GameObject prevGameObject;
	}
}
