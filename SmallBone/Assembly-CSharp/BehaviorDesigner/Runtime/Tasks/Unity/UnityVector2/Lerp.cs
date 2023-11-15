using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014FA RID: 5370
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Lerp the Vector2 by an amount.")]
	public class Lerp : Action
	{
		// Token: 0x0600682D RID: 26669 RVA: 0x0012CF62 File Offset: 0x0012B162
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector2.Lerp(this.fromVector2.Value, this.toVector2.Value, this.lerpAmount.Value);
			return TaskStatus.Success;
		}

		// Token: 0x0600682E RID: 26670 RVA: 0x0012CF98 File Offset: 0x0012B198
		public override void OnReset()
		{
			this.fromVector2 = Vector2.zero;
			this.toVector2 = Vector2.zero;
			this.storeResult = Vector2.zero;
			this.lerpAmount = 0f;
		}

		// Token: 0x04005416 RID: 21526
		[Tooltip("The from value")]
		public SharedVector2 fromVector2;

		// Token: 0x04005417 RID: 21527
		[Tooltip("The to value")]
		public SharedVector2 toVector2;

		// Token: 0x04005418 RID: 21528
		[Tooltip("The amount to lerp")]
		public SharedFloat lerpAmount;

		// Token: 0x04005419 RID: 21529
		[Tooltip("The lerp resut")]
		[RequiredField]
		public SharedVector2 storeResult;
	}
}
