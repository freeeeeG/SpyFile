using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015F4 RID: 5620
	[TaskDescription("Stores the ignore listener pause value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class GetIgnoreListenerPause : Action
	{
		// Token: 0x06006B79 RID: 27513 RVA: 0x00133F68 File Offset: 0x00132168
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B7A RID: 27514 RVA: 0x00133FA8 File Offset: 0x001321A8
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.ignoreListenerPause;
			return TaskStatus.Success;
		}

		// Token: 0x06006B7B RID: 27515 RVA: 0x00133FDB File Offset: 0x001321DB
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = false;
		}

		// Token: 0x0400572E RID: 22318
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400572F RID: 22319
		[Tooltip("The ignore listener pause value of the AudioSource")]
		[RequiredField]
		public SharedBool storeValue;

		// Token: 0x04005730 RID: 22320
		private AudioSource audioSource;

		// Token: 0x04005731 RID: 22321
		private GameObject prevGameObject;
	}
}
