using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014F3 RID: 5363
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Stores the dot product of two Vector2 values.")]
	public class Dot : Action
	{
		// Token: 0x06006818 RID: 26648 RVA: 0x0012CD71 File Offset: 0x0012AF71
		public override TaskStatus OnUpdate()
		{
			this.storeResult.Value = Vector2.Dot(this.leftHandSide.Value, this.rightHandSide.Value);
			return TaskStatus.Success;
		}

		// Token: 0x06006819 RID: 26649 RVA: 0x0012CD9A File Offset: 0x0012AF9A
		public override void OnReset()
		{
			this.leftHandSide = Vector2.zero;
			this.rightHandSide = Vector2.zero;
			this.storeResult = 0f;
		}

		// Token: 0x04005408 RID: 21512
		[Tooltip("The left hand side of the dot product")]
		public SharedVector2 leftHandSide;

		// Token: 0x04005409 RID: 21513
		[Tooltip("The right hand side of the dot product")]
		public SharedVector2 rightHandSide;

		// Token: 0x0400540A RID: 21514
		[Tooltip("The dot product result")]
		[RequiredField]
		public SharedFloat storeResult;
	}
}
