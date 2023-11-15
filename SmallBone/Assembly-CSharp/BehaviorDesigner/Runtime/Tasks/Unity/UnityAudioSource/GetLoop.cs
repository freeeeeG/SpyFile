using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015F6 RID: 5622
	[TaskDescription("Stores the loop value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class GetLoop : Action
	{
		// Token: 0x06006B81 RID: 27521 RVA: 0x00134078 File Offset: 0x00132278
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B82 RID: 27522 RVA: 0x001340B8 File Offset: 0x001322B8
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.loop;
			return TaskStatus.Success;
		}

		// Token: 0x06006B83 RID: 27523 RVA: 0x001340EB File Offset: 0x001322EB
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = false;
		}

		// Token: 0x04005736 RID: 22326
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005737 RID: 22327
		[Tooltip("The loop value of the AudioSource")]
		[RequiredField]
		public SharedBool storeValue;

		// Token: 0x04005738 RID: 22328
		private AudioSource audioSource;

		// Token: 0x04005739 RID: 22329
		private GameObject prevGameObject;
	}
}
