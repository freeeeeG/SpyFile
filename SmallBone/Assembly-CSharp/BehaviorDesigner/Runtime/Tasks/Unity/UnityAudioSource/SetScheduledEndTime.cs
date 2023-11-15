using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001610 RID: 5648
	[TaskDescription("Changes the time at which a sound that has already been scheduled to play will end. Notice that depending on the timing not all rescheduling requests can be fulfilled. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class SetScheduledEndTime : Action
	{
		// Token: 0x06006BE9 RID: 27625 RVA: 0x00134EA8 File Offset: 0x001330A8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BEA RID: 27626 RVA: 0x00134EE8 File Offset: 0x001330E8
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.SetScheduledEndTime((double)this.time.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006BEB RID: 27627 RVA: 0x00134F1C File Offset: 0x0013311C
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.time = 0f;
		}

		// Token: 0x0400579C RID: 22428
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400579D RID: 22429
		[Tooltip("Time in seconds")]
		public SharedFloat time = 0f;

		// Token: 0x0400579E RID: 22430
		private AudioSource audioSource;

		// Token: 0x0400579F RID: 22431
		private GameObject prevGameObject;
	}
}
