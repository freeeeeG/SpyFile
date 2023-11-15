using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014E8 RID: 5352
	[TaskCategory("Unity/Vector3")]
	[TaskDescription("Lerp the Vector3 by an amount.")]
	public class Lerp : Action
	{
		// Token: 0x060067FA RID: 26618 RVA: 0x0012C84D File Offset: 0x0012AA4D
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector3.Lerp(this.fromVector3.Value, this.toVector3.Value, this.lerpAmount.Value);
			return TaskStatus.Success;
		}

		// Token: 0x060067FB RID: 26619 RVA: 0x0012C884 File Offset: 0x0012AA84
		public override void OnReset()
		{
			this.fromVector3 = (this.toVector3 = (this.storeResult = Vector3.zero));
			this.lerpAmount = 0f;
		}

		// Token: 0x040053E1 RID: 21473
		[Tooltip("The from value")]
		public SharedVector3 fromVector3;

		// Token: 0x040053E2 RID: 21474
		[Tooltip("The to value")]
		public SharedVector3 toVector3;

		// Token: 0x040053E3 RID: 21475
		[Tooltip("The amount to lerp")]
		public SharedFloat lerpAmount;

		// Token: 0x040053E4 RID: 21476
		[Tooltip("The lerp resut")]
		[RequiredField]
		public SharedVector3 storeResult;
	}
}
