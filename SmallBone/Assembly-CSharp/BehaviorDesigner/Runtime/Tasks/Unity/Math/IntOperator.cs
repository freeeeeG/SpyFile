using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015A9 RID: 5545
	[TaskCategory("Unity/Math")]
	[TaskDescription("Performs a math operation on two integers: Add, Subtract, Multiply, Divide, Min, or Max.")]
	public class IntOperator : Action
	{
		// Token: 0x06006A82 RID: 27266 RVA: 0x00132170 File Offset: 0x00130370
		public override TaskStatus OnUpdate()
		{
			switch (this.operation)
			{
			case IntOperator.Operation.Add:
				this.storeResult.Value = this.integer1.Value + this.integer2.Value;
				break;
			case IntOperator.Operation.Subtract:
				this.storeResult.Value = this.integer1.Value - this.integer2.Value;
				break;
			case IntOperator.Operation.Multiply:
				this.storeResult.Value = this.integer1.Value * this.integer2.Value;
				break;
			case IntOperator.Operation.Divide:
				this.storeResult.Value = this.integer1.Value / this.integer2.Value;
				break;
			case IntOperator.Operation.Min:
				this.storeResult.Value = Mathf.Min(this.integer1.Value, this.integer2.Value);
				break;
			case IntOperator.Operation.Max:
				this.storeResult.Value = Mathf.Max(this.integer1.Value, this.integer2.Value);
				break;
			case IntOperator.Operation.Modulo:
				this.storeResult.Value = this.integer1.Value % this.integer2.Value;
				break;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A83 RID: 27267 RVA: 0x001322B7 File Offset: 0x001304B7
		public override void OnReset()
		{
			this.operation = IntOperator.Operation.Add;
			this.integer1 = 0;
			this.integer2 = 0;
			this.storeResult = 0;
		}

		// Token: 0x04005659 RID: 22105
		[Tooltip("The operation to perform")]
		public IntOperator.Operation operation;

		// Token: 0x0400565A RID: 22106
		[Tooltip("The first integer")]
		public SharedInt integer1;

		// Token: 0x0400565B RID: 22107
		[Tooltip("The second integer")]
		public SharedInt integer2;

		// Token: 0x0400565C RID: 22108
		[Tooltip("The variable to store the result")]
		[RequiredField]
		public SharedInt storeResult;

		// Token: 0x020015AA RID: 5546
		public enum Operation
		{
			// Token: 0x0400565E RID: 22110
			Add,
			// Token: 0x0400565F RID: 22111
			Subtract,
			// Token: 0x04005660 RID: 22112
			Multiply,
			// Token: 0x04005661 RID: 22113
			Divide,
			// Token: 0x04005662 RID: 22114
			Min,
			// Token: 0x04005663 RID: 22115
			Max,
			// Token: 0x04005664 RID: 22116
			Modulo
		}
	}
}
