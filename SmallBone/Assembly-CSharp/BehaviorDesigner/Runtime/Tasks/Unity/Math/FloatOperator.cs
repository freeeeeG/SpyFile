using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015A3 RID: 5539
	[TaskCategory("Unity/Math")]
	[TaskDescription("Performs a math operation on two floats: Add, Subtract, Multiply, Divide, Min, or Max.")]
	public class FloatOperator : Action
	{
		// Token: 0x06006A76 RID: 27254 RVA: 0x00131E64 File Offset: 0x00130064
		public override TaskStatus OnUpdate()
		{
			switch (this.operation)
			{
			case FloatOperator.Operation.Add:
				this.storeResult.Value = this.float1.Value + this.float2.Value;
				break;
			case FloatOperator.Operation.Subtract:
				this.storeResult.Value = this.float1.Value - this.float2.Value;
				break;
			case FloatOperator.Operation.Multiply:
				this.storeResult.Value = this.float1.Value * this.float2.Value;
				break;
			case FloatOperator.Operation.Divide:
				this.storeResult.Value = this.float1.Value / this.float2.Value;
				break;
			case FloatOperator.Operation.Min:
				this.storeResult.Value = Mathf.Min(this.float1.Value, this.float2.Value);
				break;
			case FloatOperator.Operation.Max:
				this.storeResult.Value = Mathf.Max(this.float1.Value, this.float2.Value);
				break;
			case FloatOperator.Operation.Modulo:
				this.storeResult.Value = this.float1.Value % this.float2.Value;
				break;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A77 RID: 27255 RVA: 0x00131FAB File Offset: 0x001301AB
		public override void OnReset()
		{
			this.operation = FloatOperator.Operation.Add;
			this.float1 = 0f;
			this.float2 = 0f;
			this.storeResult = 0f;
		}

		// Token: 0x0400563F RID: 22079
		[Tooltip("The operation to perform")]
		public FloatOperator.Operation operation;

		// Token: 0x04005640 RID: 22080
		[Tooltip("The first float")]
		public SharedFloat float1;

		// Token: 0x04005641 RID: 22081
		[Tooltip("The second float")]
		public SharedFloat float2;

		// Token: 0x04005642 RID: 22082
		[Tooltip("The variable to store the result")]
		public SharedFloat storeResult;

		// Token: 0x020015A4 RID: 5540
		public enum Operation
		{
			// Token: 0x04005644 RID: 22084
			Add,
			// Token: 0x04005645 RID: 22085
			Subtract,
			// Token: 0x04005646 RID: 22086
			Multiply,
			// Token: 0x04005647 RID: 22087
			Divide,
			// Token: 0x04005648 RID: 22088
			Min,
			// Token: 0x04005649 RID: 22089
			Max,
			// Token: 0x0400564A RID: 22090
			Modulo
		}
	}
}
