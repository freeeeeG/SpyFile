using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015FF RID: 5631
	[TaskDescription("Stores the volume value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class GetVolume : Action
	{
		// Token: 0x06006BA5 RID: 27557 RVA: 0x0013455C File Offset: 0x0013275C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BA6 RID: 27558 RVA: 0x0013459C File Offset: 0x0013279C
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.volume;
			return TaskStatus.Success;
		}

		// Token: 0x06006BA7 RID: 27559 RVA: 0x001345CF File Offset: 0x001327CF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 1f;
		}

		// Token: 0x0400575A RID: 22362
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400575B RID: 22363
		[Tooltip("The volume value of the AudioSource")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x0400575C RID: 22364
		private AudioSource audioSource;

		// Token: 0x0400575D RID: 22365
		private GameObject prevGameObject;
	}
}
