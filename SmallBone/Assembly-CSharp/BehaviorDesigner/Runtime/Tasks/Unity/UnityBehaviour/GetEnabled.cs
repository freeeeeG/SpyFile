using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour
{
	// Token: 0x020015F1 RID: 5617
	[TaskCategory("Unity/Behaviour")]
	[TaskDescription("Stores the enabled state of the object. Returns Success.")]
	public class GetEnabled : Action
	{
		// Token: 0x06006B70 RID: 27504 RVA: 0x00133E6C File Offset: 0x0013206C
		public override TaskStatus OnUpdate()
		{
			if (this.specifiedObject == null)
			{
				Debug.LogWarning("SpecifiedObject is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.specifiedObject.Value.enabled;
			return TaskStatus.Success;
		}

		// Token: 0x06006B71 RID: 27505 RVA: 0x00133E9E File Offset: 0x0013209E
		public override void OnReset()
		{
			this.specifiedObject.Value = null;
			this.storeValue = false;
		}

		// Token: 0x04005729 RID: 22313
		[Tooltip("The Behavior to use")]
		public SharedBehaviour specifiedObject;

		// Token: 0x0400572A RID: 22314
		[Tooltip("The enabled/disabled state")]
		[RequiredField]
		public SharedBool storeValue;
	}
}
