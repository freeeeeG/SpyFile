using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector2
{
	// Token: 0x020014FE RID: 5374
	[TaskCategory("Unity/Vector2")]
	[TaskDescription("Performs a math operation on two Vector2s: Add, Subtract, Multiply, Divide, Min, or Max.")]
	public class Operator : Action
	{
		// Token: 0x06006839 RID: 26681 RVA: 0x0012D120 File Offset: 0x0012B320
		public override TaskStatus OnUpdate()
		{
			switch (this.operation)
			{
			case Operator.Operation.Add:
				this.storeResult.Value = this.firstVector2.Value + this.secondVector2.Value;
				break;
			case Operator.Operation.Subtract:
				this.storeResult.Value = this.firstVector2.Value - this.secondVector2.Value;
				break;
			case Operator.Operation.Scale:
				this.storeResult.Value = Vector2.Scale(this.firstVector2.Value, this.secondVector2.Value);
				break;
			}
			return TaskStatus.Success;
		}

		// Token: 0x0600683A RID: 26682 RVA: 0x0012D1BF File Offset: 0x0012B3BF
		public override void OnReset()
		{
			this.operation = Operator.Operation.Add;
			this.firstVector2 = Vector2.zero;
			this.secondVector2 = Vector2.zero;
			this.storeResult = Vector2.zero;
		}

		// Token: 0x04005423 RID: 21539
		[Tooltip("The operation to perform")]
		public Operator.Operation operation;

		// Token: 0x04005424 RID: 21540
		[Tooltip("The first Vector2")]
		public SharedVector2 firstVector2;

		// Token: 0x04005425 RID: 21541
		[Tooltip("The second Vector2")]
		public SharedVector2 secondVector2;

		// Token: 0x04005426 RID: 21542
		[Tooltip("The variable to store the result")]
		public SharedVector2 storeResult;

		// Token: 0x020014FF RID: 5375
		public enum Operation
		{
			// Token: 0x04005428 RID: 21544
			Add,
			// Token: 0x04005429 RID: 21545
			Subtract,
			// Token: 0x0400542A RID: 21546
			Scale
		}
	}
}
