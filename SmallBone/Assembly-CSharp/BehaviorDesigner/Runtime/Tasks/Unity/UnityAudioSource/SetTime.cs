using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001613 RID: 5651
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Sets the time value of the AudioSource. Returns Success.")]
	public class SetTime : Action
	{
		// Token: 0x06006BF5 RID: 27637 RVA: 0x00135084 File Offset: 0x00133284
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BF6 RID: 27638 RVA: 0x001350C4 File Offset: 0x001332C4
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.time = this.time.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BF7 RID: 27639 RVA: 0x001350F7 File Offset: 0x001332F7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.time = 1f;
		}

		// Token: 0x040057A8 RID: 22440
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057A9 RID: 22441
		[Tooltip("The time value of the AudioSource")]
		public SharedFloat time;

		// Token: 0x040057AA RID: 22442
		private AudioSource audioSource;

		// Token: 0x040057AB RID: 22443
		private GameObject prevGameObject;
	}
}
