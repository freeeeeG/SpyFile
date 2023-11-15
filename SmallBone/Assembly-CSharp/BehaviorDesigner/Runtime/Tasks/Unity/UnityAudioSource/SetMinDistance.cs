using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x0200160B RID: 5643
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Sets the min distance value of the AudioSource. Returns Success.")]
	public class SetMinDistance : Action
	{
		// Token: 0x06006BD5 RID: 27605 RVA: 0x00134C00 File Offset: 0x00132E00
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BD6 RID: 27606 RVA: 0x00134C40 File Offset: 0x00132E40
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.minDistance = this.minDistance.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BD7 RID: 27607 RVA: 0x00134C73 File Offset: 0x00132E73
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.minDistance = 1f;
		}

		// Token: 0x04005788 RID: 22408
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005789 RID: 22409
		[Tooltip("The min distance value of the AudioSource")]
		public SharedFloat minDistance;

		// Token: 0x0400578A RID: 22410
		private AudioSource audioSource;

		// Token: 0x0400578B RID: 22411
		private GameObject prevGameObject;
	}
}
