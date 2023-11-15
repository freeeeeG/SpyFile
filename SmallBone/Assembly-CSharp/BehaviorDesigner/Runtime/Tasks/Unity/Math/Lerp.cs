using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015AD RID: 5549
	[TaskCategory("Unity/Math")]
	[TaskDescription("Lerp the float by an amount.")]
	public class Lerp : Action
	{
		// Token: 0x06006A8B RID: 27275 RVA: 0x0013232E File Offset: 0x0013052E
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Mathf.Lerp(this.fromValue.Value, this.toValue.Value, this.lerpAmount.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006A8C RID: 27276 RVA: 0x00132364 File Offset: 0x00130564
		public override void OnReset()
		{
			this.fromValue = 0f;
			this.toValue = 0f;
			this.lerpAmount = 0f;
			this.storeResult = 0f;
		}

		// Token: 0x04005667 RID: 22119
		[Tooltip("The from value")]
		public SharedFloat fromValue;

		// Token: 0x04005668 RID: 22120
		[Tooltip("The to value")]
		public SharedFloat toValue;

		// Token: 0x04005669 RID: 22121
		[Tooltip("The amount to lerp")]
		public SharedFloat lerpAmount;

		// Token: 0x0400566A RID: 22122
		[Tooltip("The lerp resut")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
