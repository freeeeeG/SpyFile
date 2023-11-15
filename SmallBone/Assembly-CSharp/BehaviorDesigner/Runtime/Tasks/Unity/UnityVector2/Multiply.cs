using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014FC RID: 5372
	[TaskDescription("Multiply the Vector2 by a float.")]
	[TaskCategory("Unity/Vector2")]
	public class Multiply : Action
	{
		// Token: 0x06006833 RID: 26675 RVA: 0x0012D06D File Offset: 0x0012B26D
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector2Variable.Value * this.multiplyBy.Value;
			return TaskStatus.Success;
		}

		// Token: 0x06006834 RID: 26676 RVA: 0x0012D098 File Offset: 0x0012B298
		public override void OnReset()
		{
			this.vector2Variable = (this.storeResult = Vector2.zero);
			this.multiplyBy = 0f;
		}

		// Token: 0x0400541E RID: 21534
		[Tooltip("The Vector2 to multiply of")]
		public SharedVector2 vector2Variable;

		// Token: 0x0400541F RID: 21535
		[Tooltip("The value to multiply the Vector2 of")]
		public SharedFloat multiplyBy;

		// Token: 0x04005420 RID: 21536
		[Tooltip("The multiplication resut")]
		[RequiredField]
		public SharedVector2 storeResult;
	}
}
