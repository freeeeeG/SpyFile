using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001615 RID: 5653
	[TaskDescription("Sets the volume value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class SetVolume : Action
	{
		// Token: 0x06006BFD RID: 27645 RVA: 0x00135190 File Offset: 0x00133390
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BFE RID: 27646 RVA: 0x001351D0 File Offset: 0x001333D0
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.volume = this.volume.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BFF RID: 27647 RVA: 0x00135203 File Offset: 0x00133403
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.volume = 1f;
		}

		// Token: 0x040057B0 RID: 22448
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057B1 RID: 22449
		[Tooltip("The volume value of the AudioSource")]
		public SharedFloat volume;

		// Token: 0x040057B2 RID: 22450
		private AudioSource audioSource;

		// Token: 0x040057B3 RID: 22451
		private GameObject prevGameObject;
	}
}
