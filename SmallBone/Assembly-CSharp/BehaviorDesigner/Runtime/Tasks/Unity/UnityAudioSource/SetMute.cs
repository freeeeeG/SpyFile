using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x0200160C RID: 5644
	[TaskDescription("Sets the mute value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class SetMute : Action
	{
		// Token: 0x06006BD9 RID: 27609 RVA: 0x00134C8C File Offset: 0x00132E8C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BDA RID: 27610 RVA: 0x00134CCC File Offset: 0x00132ECC
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.mute = this.mute.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BDB RID: 27611 RVA: 0x00134CFF File Offset: 0x00132EFF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.mute = false;
		}

		// Token: 0x0400578C RID: 22412
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400578D RID: 22413
		[Tooltip("The mute value of the AudioSource")]
		public SharedBool mute;

		// Token: 0x0400578E RID: 22414
		private AudioSource audioSource;

		// Token: 0x0400578F RID: 22415
		private GameObject prevGameObject;
	}
}
