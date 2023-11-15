using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001608 RID: 5640
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Sets the ignore listener pause value of the AudioSource. Returns Success.")]
	public class SetIgnoreListenerPause : Action
	{
		// Token: 0x06006BC9 RID: 27593 RVA: 0x00134A64 File Offset: 0x00132C64
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BCA RID: 27594 RVA: 0x00134AA4 File Offset: 0x00132CA4
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.ignoreListenerPause = this.ignoreListenerPause.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BCB RID: 27595 RVA: 0x00134AD7 File Offset: 0x00132CD7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.ignoreListenerPause = false;
		}

		// Token: 0x0400577C RID: 22396
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400577D RID: 22397
		[Tooltip("The ignore listener pause value of the AudioSource")]
		public SharedBool ignoreListenerPause;

		// Token: 0x0400577E RID: 22398
		private AudioSource audioSource;

		// Token: 0x0400577F RID: 22399
		private GameObject prevGameObject;
	}
}
