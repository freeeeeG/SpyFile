using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityQuaternion
{
	// Token: 0x0200156D RID: 5485
	[TaskCategory("Unity/Quaternion")]
	[TaskDescription("Lerps between two quaternions.")]
	public class Lerp : Action
	{
		// Token: 0x060069BF RID: 27071 RVA: 0x00130372 File Offset: 0x0012E572
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Quaternion.Lerp(this.fromQuaternion.Value, this.toQuaternion.Value, this.amount.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060069C0 RID: 27072 RVA: 0x001303A8 File Offset: 0x0012E5A8
		public override void OnReset()
		{
			this.fromQuaternion = (this.toQuaternion = (this.storeResult = Quaternion.identity));
			this.amount = 0f;
		}

		// Token: 0x04005581 RID: 21889
		[Tooltip("The from rotation")]
		public SharedQuaternion fromQuaternion;

		// Token: 0x04005582 RID: 21890
		[Tooltip("The to rotation")]
		public SharedQuaternion toQuaternion;

		// Token: 0x04005583 RID: 21891
		[Tooltip("The amount to lerp")]
		public SharedFloat amount;

		// Token: 0x04005584 RID: 21892
		[RequiredField]
		[Tooltip("The stored result")]
		public SharedQuaternion storeResult;
	}
}
