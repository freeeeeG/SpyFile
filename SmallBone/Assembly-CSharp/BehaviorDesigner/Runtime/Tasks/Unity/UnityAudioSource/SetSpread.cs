using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001612 RID: 5650
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Sets the spread value of the AudioSource. Returns Success.")]
	public class SetSpread : Action
	{
		// Token: 0x06006BF1 RID: 27633 RVA: 0x00134FF8 File Offset: 0x001331F8
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BF2 RID: 27634 RVA: 0x00135038 File Offset: 0x00133238
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.spread = this.spread.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BF3 RID: 27635 RVA: 0x0013506B File Offset: 0x0013326B
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.spread = 1f;
		}

		// Token: 0x040057A4 RID: 22436
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x040057A5 RID: 22437
		[Tooltip("The spread value of the AudioSource")]
		public SharedFloat spread;

		// Token: 0x040057A6 RID: 22438
		private AudioSource audioSource;

		// Token: 0x040057A7 RID: 22439
		private GameObject prevGameObject;
	}
}
