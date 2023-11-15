using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x02001567 RID: 5479
	[TaskDescription("Stores the rotation which rotates the specified degrees around the specified axis.")]
	[TaskCategory("Unity/Quaternion")]
	public class AngleAxis : Action
	{
		// Token: 0x060069AD RID: 27053 RVA: 0x001301AA File Offset: 0x0012E3AA
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.AngleAxis(this.degrees.Value, this.axis.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069AE RID: 27054 RVA: 0x001301D3 File Offset: 0x0012E3D3
		public override void OnReset()
		{
			this.degrees = 0f;
			this.axis = Vector3.zero;
			this.storeResult = Quaternion.identity;
		}

		// Token: 0x04005573 RID: 21875
		[Tooltip("The number of degrees")]
		public SharedFloat degrees;

		// Token: 0x04005574 RID: 21876
		[Tooltip("The axis direction")]
		public SharedVector3 axis;

		// Token: 0x04005575 RID: 21877
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedQuaternion storeResult;
	}
}
