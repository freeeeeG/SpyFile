using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015F7 RID: 5623
	[TaskDescription("Stores the max distance value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class GetMaxDistance : Action
	{
		// Token: 0x06006B85 RID: 27525 RVA: 0x00134100 File Offset: 0x00132300
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B86 RID: 27526 RVA: 0x00134140 File Offset: 0x00132340
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.maxDistance;
			return TaskStatus.Success;
		}

		// Token: 0x06006B87 RID: 27527 RVA: 0x00134173 File Offset: 0x00132373
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 1f;
		}

		// Token: 0x0400573A RID: 22330
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400573B RID: 22331
		[Tooltip("The max distance value of the AudioSource")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x0400573C RID: 22332
		private AudioSource audioSource;

		// Token: 0x0400573D RID: 22333
		private GameObject prevGameObject;
	}
}
