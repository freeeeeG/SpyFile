using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001609 RID: 5641
	[TaskDescription("Sets the loop value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class SetLoop : Action
	{
		// Token: 0x06006BCD RID: 27597 RVA: 0x00134AEC File Offset: 0x00132CEC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BCE RID: 27598 RVA: 0x00134B2C File Offset: 0x00132D2C
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.loop = this.loop.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BCF RID: 27599 RVA: 0x00134B5F File Offset: 0x00132D5F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.loop = false;
		}

		// Token: 0x04005780 RID: 22400
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005781 RID: 22401
		[Tooltip("The loop value of the AudioSource")]
		public SharedBool loop;

		// Token: 0x04005782 RID: 22402
		private AudioSource audioSource;

		// Token: 0x04005783 RID: 22403
		private GameObject prevGameObject;
	}
}
