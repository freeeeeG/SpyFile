using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001614 RID: 5652
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Sets the rolloff mode of the AudioSource. Returns Success.")]
	public class SetVelocityUpdateMode : Action
	{
		// Token: 0x06006BF9 RID: 27641 RVA: 0x00135110 File Offset: 0x00133310
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BFA RID: 27642 RVA: 0x00135150 File Offset: 0x00133350
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.velocityUpdateMode = this.velocityUpdateMode;
			return TaskStatus.Success;
		}

		// Token: 0x06006BFB RID: 27643 RVA: 0x0013517E File Offset: 0x0013337E
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.velocityUpdateMode = AudioVelocityUpdateMode.Auto;
		}

		// Token: 0x040057AC RID: 22444
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057AD RID: 22445
		[Tooltip("The velocity update mode of the AudioSource")]
		public AudioVelocityUpdateMode velocityUpdateMode;

		// Token: 0x040057AE RID: 22446
		private AudioSource audioSource;

		// Token: 0x040057AF RID: 22447
		private GameObject prevGameObject;
	}
}
