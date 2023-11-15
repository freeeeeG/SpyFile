using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x02001570 RID: 5488
	[TaskCategory("Unity/Quaternion")]
	[TaskDescription("Spherically lerp between two quaternions.")]
	public class Slerp : Action
	{
		// Token: 0x060069C8 RID: 27080 RVA: 0x0013049B File Offset: 0x0012E69B
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Slerp(this.fromQuaternion.Value, this.toQuaternion.Value, this.amount.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069C9 RID: 27081 RVA: 0x001304D0 File Offset: 0x0012E6D0
		public override void OnReset()
		{
			this.fromQuaternion = (this.toQuaternion = (this.storeResult = Quaternion.identity));
			this.amount = 0f;
		}

		// Token: 0x0400558C RID: 21900
		[Tooltip("The from rotation")]
		public SharedQuaternion fromQuaternion;

		// Token: 0x0400558D RID: 21901
		[Tooltip("The to rotation")]
		public SharedQuaternion toQuaternion;

		// Token: 0x0400558E RID: 21902
		[Tooltip("The amount to lerp")]
		public SharedFloat amount;

		// Token: 0x0400558F RID: 21903
		[Tooltip("The stored result")]
		[RequiredField]
		public SharedQuaternion storeResult;
	}
}
