using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015F8 RID: 5624
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Stores the min distance value of the AudioSource. Returns Success.")]
	public class GetMinDistance : Action
	{
		// Token: 0x06006B89 RID: 27529 RVA: 0x0013418C File Offset: 0x0013238C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B8A RID: 27530 RVA: 0x001341CC File Offset: 0x001323CC
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.minDistance;
			return TaskStatus.Success;
		}

		// Token: 0x06006B8B RID: 27531 RVA: 0x001341FF File Offset: 0x001323FF
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 1f;
		}

		// Token: 0x0400573E RID: 22334
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400573F RID: 22335
		[RequiredField]
		[Tooltip("The min distance value of the AudioSource")]
		public SharedFloat storeValue;

		// Token: 0x04005740 RID: 22336
		private AudioSource audioSource;

		// Token: 0x04005741 RID: 22337
		private GameObject prevGameObject;
	}
}
