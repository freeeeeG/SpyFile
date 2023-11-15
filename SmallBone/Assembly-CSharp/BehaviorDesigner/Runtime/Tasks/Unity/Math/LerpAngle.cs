using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015AE RID: 5550
	[TaskCategory("Unity/Math")]
	[TaskDescription("Lerp the angle by an amount.")]
	public class LerpAngle : Action
	{
		// Token: 0x06006A8E RID: 27278 RVA: 0x001323B1 File Offset: 0x001305B1
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Mathf.LerpAngle(this.fromValue.Value, this.toValue.Value, this.lerpAmount.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006A8F RID: 27279 RVA: 0x001323E8 File Offset: 0x001305E8
		public override void OnReset()
		{
			this.fromValue = 0f;
			this.toValue = 0f;
			this.lerpAmount = 0f;
			this.storeResult = 0f;
		}

		// Token: 0x0400566B RID: 22123
		[Tooltip("The from value")]
		public SharedFloat fromValue;

		// Token: 0x0400566C RID: 22124
		[Tooltip("The to value")]
		public SharedFloat toValue;

		// Token: 0x0400566D RID: 22125
		[Tooltip("The amount to lerp")]
		public SharedFloat lerpAmount;

		// Token: 0x0400566E RID: 22126
		[RequiredField]
		[Tooltip("The lerp resut")]
		public SharedFloat storeResult;
	}
}
