using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x0200160D RID: 5645
	[TaskDescription("Sets the pitch value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class SetPitch : Action
	{
		// Token: 0x06006BDD RID: 27613 RVA: 0x00134D14 File Offset: 0x00132F14
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BDE RID: 27614 RVA: 0x00134D54 File Offset: 0x00132F54
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.pitch = this.pitch.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BDF RID: 27615 RVA: 0x00134D87 File Offset: 0x00132F87
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.pitch = 1f;
		}

		// Token: 0x04005790 RID: 22416
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005791 RID: 22417
		[Tooltip("The pitch value of the AudioSource")]
		public SharedFloat pitch;

		// Token: 0x04005792 RID: 22418
		private AudioSource audioSource;

		// Token: 0x04005793 RID: 22419
		private GameObject prevGameObject;
	}
}
