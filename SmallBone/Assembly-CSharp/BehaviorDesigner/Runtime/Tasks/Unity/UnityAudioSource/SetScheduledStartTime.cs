using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001611 RID: 5649
	[TaskDescription("Changes the time at which a sound that has already been scheduled to play will start. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class SetScheduledStartTime : Action
	{
		// Token: 0x06006BED RID: 27629 RVA: 0x00134F50 File Offset: 0x00133150
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BEE RID: 27630 RVA: 0x00134F90 File Offset: 0x00133190
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.SetScheduledStartTime((double)this.time.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006BEF RID: 27631 RVA: 0x00134FC4 File Offset: 0x001331C4
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.time = 0f;
		}

		// Token: 0x040057A0 RID: 22432
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057A1 RID: 22433
		[Tooltip("Time in seconds")]
		public SharedFloat time = 0f;

		// Token: 0x040057A2 RID: 22434
		private AudioSource audioSource;

		// Token: 0x040057A3 RID: 22435
		private GameObject prevGameObject;
	}
}
