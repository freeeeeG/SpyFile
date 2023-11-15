using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001604 RID: 5636
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Plays an AudioClip, and scales the AudioSource volume by volumeScale. Returns Success.")]
	public class PlayOneShot : Action
	{
		// Token: 0x06006BB9 RID: 27577 RVA: 0x001347EC File Offset: 0x001329EC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BBA RID: 27578 RVA: 0x0013482C File Offset: 0x00132A2C
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.PlayOneShot((AudioClip)this.clip.Value, this.volumeScale.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006BBB RID: 27579 RVA: 0x0013487A File Offset: 0x00132A7A
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.clip = null;
			this.volumeScale = 1f;
		}

		// Token: 0x0400576B RID: 22379
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400576C RID: 22380
		[Tooltip("The clip being played")]
		public SharedObject clip;

		// Token: 0x0400576D RID: 22381
		[Tooltip("The scale of the volume (0-1)")]
		public SharedFloat volumeScale = 1f;

		// Token: 0x0400576E RID: 22382
		private AudioSource audioSource;

		// Token: 0x0400576F RID: 22383
		private GameObject prevGameObject;
	}
}
