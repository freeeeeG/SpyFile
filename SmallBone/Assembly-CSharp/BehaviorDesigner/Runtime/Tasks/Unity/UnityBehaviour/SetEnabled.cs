using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBehaviour
{
	// Token: 0x020015F3 RID: 5619
	[TaskDescription("Enables/Disables the object. Returns Success.")]
	[TaskCategory("Unity/Behaviour")]
	public class SetEnabled : Action
	{
		// Token: 0x06006B76 RID: 27510 RVA: 0x00133F1B File Offset: 0x0013211B
		public override TaskStatus OnUpdate()
		{
			if (this.specifiedObject == null)
			{
				Debug.LogWarning("SpecifiedObject is null");
				return TaskStatus.Failure;
			}
			this.specifiedObject.Value.enabled = this.enabled.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006B77 RID: 27511 RVA: 0x00133F4D File Offset: 0x0013214D
		public override void OnReset()
		{
			this.specifiedObject.Value = null;
			this.enabled = false;
		}

		// Token: 0x0400572C RID: 22316
		[Tooltip("The Behavior to use")]
		public SharedBehaviour specifiedObject;

		// Token: 0x0400572D RID: 22317
		[Tooltip("The enabled/disabled state")]
		public SharedBool enabled;
	}
}
