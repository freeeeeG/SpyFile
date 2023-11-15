using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015A1 RID: 5537
	[TaskDescription("Performs comparison between two floats: less than, less than or equal to, equal to, not equal to, greater than or equal to, or greater than.")]
	[TaskCategory("Unity/Math")]
	[TaskIcon("Assets/Behavior Designer/Icon/FloatComparison.png")]
	public class FloatComparison : Conditional
	{
		// Token: 0x06006A73 RID: 27251 RVA: 0x00131D50 File Offset: 0x0012FF50
		public override TaskStatus OnUpdate()
		{
			switch (this.operation)
			{
			case FloatComparison.Operation.LessThan:
				if (this.float1.Value >= this.float2.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case FloatComparison.Operation.LessThanOrEqualTo:
				if (this.float1.Value > this.float2.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case FloatComparison.Operation.EqualTo:
				if (!Mathf.Approximately(this.float1.Value, this.float2.Value))
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case FloatComparison.Operation.NotEqualTo:
				if (Mathf.Approximately(this.float1.Value, this.float2.Value))
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case FloatComparison.Operation.GreaterThanOrEqualTo:
				if (this.float1.Value < this.float2.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case FloatComparison.Operation.GreaterThan:
				if (this.float1.Value <= this.float2.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			default:
				return TaskStatus.Failure;
			}
		}

		// Token: 0x06006A74 RID: 27252 RVA: 0x00131E3A File Offset: 0x0013003A
		public override void OnReset()
		{
			this.operation = FloatComparison.Operation.LessThan;
			this.float1.Value = 0f;
			this.float2.Value = 0f;
		}

		// Token: 0x04005635 RID: 22069
		[Tooltip("The operation to perform")]
		public FloatComparison.Operation operation;

		// Token: 0x04005636 RID: 22070
		[Tooltip("The first float")]
		public SharedFloat float1;

		// Token: 0x04005637 RID: 22071
		[Tooltip("The second float")]
		public SharedFloat float2;

		// Token: 0x020015A2 RID: 5538
		public enum Operation
		{
			// Token: 0x04005639 RID: 22073
			LessThan,
			// Token: 0x0400563A RID: 22074
			LessThanOrEqualTo,
			// Token: 0x0400563B RID: 22075
			EqualTo,
			// Token: 0x0400563C RID: 22076
			NotEqualTo,
			// Token: 0x0400563D RID: 22077
			GreaterThanOrEqualTo,
			// Token: 0x0400563E RID: 22078
			GreaterThan
		}
	}
}
