using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityAudioSource
{
	// Token: 0x020015FE RID: 5630
	[TaskCategory("Unity/AudioSource")]
	[TaskDescription("Stores the time samples value of the AudioSource. Returns Success.")]
	public class GetTimeSamples : Action
	{
		// Token: 0x06006BA1 RID: 27553 RVA: 0x001344CC File Offset: 0x001326CC
		public override void OnStart()
		{
			GameObject defaultGameObject = base.GetDefaultGameObject(this.targetGameObject.Value);
			if (defaultGameObject != this.prevGameObject)
			{
				this.audioSource = defaultGameObject.GetComponent<AudioSource>();
				this.prevGameObject = defaultGameObject;
			}
		}

		// Token: 0x06006BA2 RID: 27554 RVA: 0x0013450C File Offset: 0x0013270C
		public override TaskStatus OnUpdate()
		{
			if (this.audioSource == null)
			{
				Debug.LogWarning("AudioSource is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = (float)this.audioSource.timeSamples;
			return TaskStatus.Success;
		}

		// Token: 0x06006BA3 RID: 27555 RVA: 0x00134540 File Offset: 0x00132740
		public override void OnReset()
		{
			this.targetGameObject = null;
			this.storeValue = 1f;
		}

		// Token: 0x04005756 RID: 22358
		[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
		public SharedGameObject targetGameObject;

		// Token: 0x04005757 RID: 22359
		[Tooltip("The time samples value of the AudioSource")]
		[RequiredField]
		public SharedFloat storeValue;

		// Token: 0x04005758 RID: 22360
		private AudioSource audioSource;

		// Token: 0x04005759 RID: 22361
		private GameObject prevGameObject;
	}
}
