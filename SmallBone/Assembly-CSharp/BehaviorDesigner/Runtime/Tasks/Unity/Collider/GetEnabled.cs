using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Collider
{
	// Token: 0x020015E9 RID: 5609
	[TaskDescription("Stores the enabled state of the collider. Returns Success.")]
	[TaskCategory("Unity/Collider")]
	public class GetEnabled : Action
	{
		// Token: 0x06006B52 RID: 27474 RVA: 0x00133A7B File Offset: 0x00131C7B
		public override TaskStatus OnUpdate()
		{
			if (this.specifiedCollider == null)
			{
				Debug.LogWarning("SpecifiedObject is null");
				return TaskStatus.Failure;
			}
			this.storeValue.Value = this.specifiedCollider.Value.enabled;
			return TaskStatus.Success;
		}

		// Token: 0x06006B53 RID: 27475 RVA: 0x00133AAD File Offset: 0x00131CAD
		public override void OnReset()
		{
			this.specifiedCollider.Value = null;
			this.storeValue = false;
		}

		// Token: 0x0400570D RID: 22285
		[Tooltip("The Collider to use")]
		public SharedCollider specifiedCollider;

		// Token: 0x0400570E RID: 22286
		[RequiredField]
		[Tooltip("The enabled/disabled state")]
		public SharedBool storeValue;
	}
}
