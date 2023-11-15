using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Collider
{
	// Token: 0x020015EA RID: 5610
	[TaskCategory("Unity/Collider")]
	[TaskDescription("Enables/Disables the collider. Returns Success.")]
	public class SetEnabled : Action
	{
		// Token: 0x06006B55 RID: 27477 RVA: 0x00133AC7 File Offset: 0x00131CC7
		public override TaskStatus OnUpdate()
		{
			if (this.specifiedCollider == null)
			{
				Debug.LogWarning("SpecifiedCollider is null");
				return TaskStatus.Failure;
			}
			this.specifiedCollider.Value.enabled = this.enabled.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006B56 RID: 27478 RVA: 0x00133AF9 File Offset: 0x00131CF9
		public override void OnReset()
		{
			this.specifiedCollider.Value = null;
			this.enabled = false;
		}

		// Token: 0x0400570F RID: 22287
		[Tooltip("The Behavior to use")]
		public SharedCollider specifiedCollider;

		// Token: 0x04005710 RID: 22288
		[Tooltip("The enabled/disabled state")]
		public SharedBool enabled;
	}
}
