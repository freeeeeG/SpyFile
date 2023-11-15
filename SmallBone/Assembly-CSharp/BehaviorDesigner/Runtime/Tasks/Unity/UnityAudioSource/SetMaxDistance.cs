using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x0200160A RID: 5642
	[TaskDescription("Sets the max distance value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class SetMaxDistance : Action
	{
		// Token: 0x06006BD1 RID: 27601 RVA: 0x00134B74 File Offset: 0x00132D74
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BD2 RID: 27602 RVA: 0x00134BB4 File Offset: 0x00132DB4
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.maxDistance = this.maxDistance.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BD3 RID: 27603 RVA: 0x00134BE7 File Offset: 0x00132DE7
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.maxDistance = 1f;
		}

		// Token: 0x04005784 RID: 22404
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005785 RID: 22405
		[Tooltip("The max distance value of the AudioSource")]
		public SharedFloat maxDistance;

		// Token: 0x04005786 RID: 22406
		private AudioSource audioSource;

		// Token: 0x04005787 RID: 22407
		private GameObject prevGameObject;
	}
}
