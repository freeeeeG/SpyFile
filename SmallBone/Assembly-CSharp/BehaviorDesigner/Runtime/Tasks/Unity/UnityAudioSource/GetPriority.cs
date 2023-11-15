using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015FB RID: 5627
	[TaskDescription("Stores the priority value of the AudioSource. Returns Success.")]
	[TaskCategory("Unity/AudioSource")]
	public class GetPriority : Action
	{
		// Token: 0x06006B95 RID: 27541 RVA: 0x0013432C File Offset: 0x0013252C
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006B96 RID: 27542 RVA: 0x0013436C File Offset: 0x0013256C
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.audioSource.priority;
			return TaskStatus.Success;
		}

		// Token: 0x06006B97 RID: 27543 RVA: 0x0013439F File Offset: 0x0013259F
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 1;
		}

		// Token: 0x0400574A RID: 22346
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x0400574B RID: 22347
		[RequiredField]
		[Tooltip("The priority value of the AudioSource")]
		public SharedInt storeValue;

		// Token: 0x0400574C RID: 22348
		private AudioSource audioSource;

		// Token: 0x0400574D RID: 22349
		private GameObject prevGameObject;
	}
}
