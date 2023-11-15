using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x02001607 RID: 5639
	[TaskDescription("Sets the ignore listener volume value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class SetIgnoreListenerVolume : Action
	{
		// Token: 0x06006BC5 RID: 27589 RVA: 0x001349DC File Offset: 0x00132BDC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BC6 RID: 27590 RVA: 0x00134A1C File Offset: 0x00132C1C
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.audioSource.ignoreListenerVolume = this.ignoreListenerVolume.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006BC7 RID: 27591 RVA: 0x00134A4F File Offset: 0x00132C4F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.ignoreListenerVolume = false;
		}

		// Token: 0x04005778 RID: 22392
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005779 RID: 22393
		[Tooltip("The ignore listener volume value of the AudioSource")]
		public SharedBool ignoreListenerVolume;

		// Token: 0x0400577A RID: 22394
		private AudioSource audioSource;

		// Token: 0x0400577B RID: 22395
		private GameObject prevGameObject;
	}
}
