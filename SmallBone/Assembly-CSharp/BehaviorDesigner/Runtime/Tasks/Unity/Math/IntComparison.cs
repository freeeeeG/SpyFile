using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x020015A7 RID: 5543
	[TaskCategory("Unity/Math")]
	[TaskDescription("Performs comparison between two integers: less than, less than or equal to, equal to, not equal to, greater than or equal to, or greater than.")]
	public class IntComparison : Conditional
	{
		// Token: 0x06006A7F RID: 27263 RVA: 0x0013206C File Offset: 0x0013026C
		public override TaskStatus OnUpdate()
		{
			switch (this.operation)
			{
			case IntComparison.Operation.LessThan:
				if (this.integer1.Value >= this.integer2.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case IntComparison.Operation.LessThanOrEqualTo:
				if (this.integer1.Value > this.integer2.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case IntComparison.Operation.EqualTo:
				if (this.integer1.Value != this.integer2.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case IntComparison.Operation.NotEqualTo:
				if (this.integer1.Value == this.integer2.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case IntComparison.Operation.GreaterThanOrEqualTo:
				if (this.integer1.Value < this.integer2.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case IntComparison.Operation.GreaterThan:
				if (this.integer1.Value <= this.integer2.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			default:
				return TaskStatus.Failure;
			}
		}

		// Token: 0x06006A80 RID: 27264 RVA: 0x0013214C File Offset: 0x0013034C
		public override void OnReset()
		{
			this.operation = IntComparison.Operation.LessThan;
			this.integer1.Value = 0;
			this.integer2.Value = 0;
		}

		// Token: 0x0400564F RID: 22095
		[Tooltip("The operation to perform")]
		public IntComparison.Operation operation;

		// Token: 0x04005650 RID: 22096
		[Tooltip("The first integer")]
		public SharedInt integer1;

		// Token: 0x04005651 RID: 22097
		[Tooltip("The second integer")]
		public SharedInt integer2;

		// Token: 0x020015A8 RID: 5544
		public enum Operation
		{
			// Token: 0x04005653 RID: 22099
			LessThan,
			// Token: 0x04005654 RID: 22100
			LessThanOrEqualTo,
			// Token: 0x04005655 RID: 22101
			EqualTo,
			// Token: 0x04005656 RID: 22102
			NotEqualTo,
			// Token: 0x04005657 RID: 22103
			GreaterThanOrEqualTo,
			// Token: 0x04005658 RID: 22104
			GreaterThan
		}
	}
}
