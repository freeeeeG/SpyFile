using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015FC RID: 5628
	[TaskDescription("Stores the spread value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class GetSpread : Action
	{
		// Token: 0x06006B99 RID: 27545 RVA: 0x001343B4 File Offset: 0x001325B4
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B9A RID: 27546 RVA: 0x001343F4 File Offset: 0x001325F4
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.spread;
			return TaskStatus.Success;
		}

		// Token: 0x06006B9B RID: 27547 RVA: 0x00134427 File Offset: 0x00132627
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 1f;
		}

		// Token: 0x0400574E RID: 22350
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400574F RID: 22351
		[Tooltip("The spread value of the AudioSource")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005750 RID: 22352
		private AudioSource audioSource;

		// Token: 0x04005751 RID: 22353
		private GameObject prevGameObject;
	}
}
