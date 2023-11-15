using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityVector3
{
	// Token: 0x020014EC RID: 5356
	[TaskDescription("Performs a math operation on two Vector3s: Add, Subtract, Multiply, Divide, Min, or Max.")]
	[TaskCategory("Unity/Vector3")]
	public class Operator : Action
	{
		// Token: 0x06006806 RID: 26630 RVA: 0x0012C9E8 File Offset: 0x0012ABE8
		public override TaskStatus OnUpdate()
		{
			switch (this.operation)
			{
			case Operator.Operation.Add:
				this.storeResult.Value = this.firstVector3.Value + this.secondVector3.Value;
				break;
			case Operator.Operation.Subtract:
				this.storeResult.Value = this.firstVector3.Value - this.secondVector3.Value;
				break;
			case Operator.Operation.Scale:
				this.storeResult.Value = Vector3.Scale(this.firstVector3.Value, this.secondVector3.Value);
				break;
			}
			return TaskStatus.Success;
		}

		// Token: 0x06006807 RID: 26631 RVA: 0x0012CA87 File Offset: 0x0012AC87
		public override void OnReset()
		{
			this.operation = Operator.Operation.Add;
			this.firstVector3 = Vector3.zero;
			this.secondVector3 = Vector3.zero;
			this.storeResult = Vector3.zero;
		}

		// Token: 0x040053EE RID: 21486
		[Tooltip("The operation to perform")]
		public Operator.Operation operation;

		// Token: 0x040053EF RID: 21487
		[Tooltip("The first Vector3")]
		public SharedVector3 firstVector3;

		// Token: 0x040053F0 RID: 21488
		[Tooltip("The second Vector3")]
		public SharedVector3 secondVector3;

		// Token: 0x040053F1 RID: 21489
		[Tooltip("The variable to store the result")]
		public SharedVector3 storeResult;

		// Token: 0x020014ED RID: 5357
		public enum Operation
		{
			// Token: 0x040053F3 RID: 21491
			Add,
			// Token: 0x040053F4 RID: 21492
			Subtract,
			// Token: 0x040053F5 RID: 21493
			Scale
		}
	}
}
