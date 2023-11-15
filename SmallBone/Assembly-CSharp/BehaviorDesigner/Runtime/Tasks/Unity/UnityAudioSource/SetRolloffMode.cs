using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x0200160F RID: 5647
	[TaskDescription("Sets the rolloff mode of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class SetRolloffMode : Action
	{
		// Token: 0x06006BE5 RID: 27621 RVA: 0x00134E28 File Offset: 0x00133028
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BE6 RID: 27622 RVA: 0x00134E68 File Offset: 0x00133068
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.rolloffMode = this.rolloffMode;
			return TaskStatus.Success;
		}

		// Token: 0x06006BE7 RID: 27623 RVA: 0x00134E96 File Offset: 0x00133096
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.rolloffMode = AudioRolloffMode.Logarithmic;
		}

		// Token: 0x04005798 RID: 22424
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005799 RID: 22425
		[Tooltip("The rolloff mode of the AudioSource")]
		public AudioRolloffMode rolloffMode;

		// Token: 0x0400579A RID: 22426
		private AudioSource audioSource;

		// Token: 0x0400579B RID: 22427
		private GameObject prevGameObject;
	}
}
