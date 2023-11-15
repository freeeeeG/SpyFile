using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x0200160E RID: 5646
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Sets the priority value of the AudioSource. Returns Success.")]
	public class SetPriority : Action
	{
		// Token: 0x06006BE1 RID: 27617 RVA: 0x00134DA0 File Offset: 0x00132FA0
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BE2 RID: 27618 RVA: 0x00134DE0 File Offset: 0x00132FE0
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.priority = this.priority.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BE3 RID: 27619 RVA: 0x00134E13 File Offset: 0x00133013
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.priority = 1;
		}

		// Token: 0x04005794 RID: 22420
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005795 RID: 22421
		[Tooltip("The priority value of the AudioSource")]
		public SharedInt priority;

		// Token: 0x04005796 RID: 22422
		private AudioSource audioSource;

		// Token: 0x04005797 RID: 22423
		private GameObject prevGameObject;
	}
}
