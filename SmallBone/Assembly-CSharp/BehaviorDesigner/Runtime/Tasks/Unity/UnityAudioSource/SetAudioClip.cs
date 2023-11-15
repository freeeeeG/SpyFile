using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001606 RID: 5638
	[TaskDescription("Sets the clip value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class SetAudioClip : Action
	{
		// Token: 0x06006BC1 RID: 27585 RVA: 0x0013495C File Offset: 0x00132B5C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BC2 RID: 27586 RVA: 0x0013499C File Offset: 0x00132B9C
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.clip = this.audioClip;
			return TaskStatus.Success;
		}

		// Token: 0x06006BC3 RID: 27587 RVA: 0x001349CA File Offset: 0x00132BCA
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.audioClip = null;
		}

		// Token: 0x04005774 RID: 22388
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005775 RID: 22389
		[Tooltip("The AudioSource clip")]
		public AudioClip audioClip;

		// Token: 0x04005776 RID: 22390
		private AudioSource audioSource;

		// Token: 0x04005777 RID: 22391
		private GameObject prevGameObject;
	}
}
