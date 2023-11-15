using System;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math
{
	// Token: 0x0200159D RID: 5533
	[TaskCategory("Unity/Math")]
	[TaskDescription("Performs a math operation on two bools: AND, OR, NAND, or XOR.")]
	public class BoolOperator : Action
	{
		// Token: 0x06006A6A RID: 27242 RVA: 0x00131BCC File Offset: 0x0012FDCC
		public override TaskStatus OnUpdate()
		{
			switch (this.operation)
			{
			case BoolOperator.Operation.AND:
				this.storeResult.Value = (this.bool1.Value && this.bool2.Value);
				break;
			case BoolOperator.Operation.OR:
				this.storeResult.Value = (this.bool1.Value || this.bool2.Value);
				break;
			case BoolOperator.Operation.NAND:
				this.storeResult.Value = (!this.bool1.Value || !this.bool2.Value);
				break;
			case BoolOperator.Operation.XOR:
				this.storeResult.Value = (this.bool1.Value ^ this.bool2.Value);
				break;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006A6B RID: 27243 RVA: 0x00131C99 File Offset: 0x0012FE99
		public override void OnReset()
		{
			this.operation = BoolOperator.Operation.AND;
			this.bool1 = false;
			this.bool2 = false;
		}

		// Token: 0x04005628 RID: 22056
		[Tooltip("The operation to perform")]
		public BoolOperator.Operation operation;

		// Token: 0x04005629 RID: 22057
		[Tooltip("The first bool")]
		public SharedBool bool1;

		// Token: 0x0400562A RID: 22058
		[Tooltip("The second bool")]
		public SharedBool bool2;

		// Token: 0x0400562B RID: 22059
		[Tooltip("The variable to store the result")]
		public SharedBool storeResult;

		// Token: 0x0200159E RID: 5534
		public enum Operation
		{
			// Token: 0x0400562D RID: 22061
			AND,
			// Token: 0x0400562E RID: 22062
			OR,
			// Token: 0x0400562F RID: 22063
			NAND,
			// Token: 0x04005630 RID: 22064
			XOR
		}
	}
}
