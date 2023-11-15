using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014FD RID: 5373
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Normalize the Vector2.")]
	public class Normalize : Action
	{
		// Token: 0x06006836 RID: 26678 RVA: 0x0012D0D0 File Offset: 0x0012B2D0
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = this.vector2Variable.Value.normalized;
			return TaskStatus.Success;
		}

		// Token: 0x06006837 RID: 26679 RVA: 0x0012D0FC File Offset: 0x0012B2FC
		public override void OnReset()
		{
			this.vector2Variable = Vector2.zero;
			this.storeResult = Vector2.zero;
		}

		// Token: 0x04005421 RID: 21537
		[Tooltip("The Vector2 to normalize")]
		public SharedVector2 vector2Variable;

		// Token: 0x04005422 RID: 21538
		[RequiredField]
		[Tooltip("The normalized resut")]
		public SharedVector2 storeResult;
	}
}
