using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	// Token: 0x020014B1 RID: 5297
	public class HealthComparison : Conditional
	{
		// Token: 0x06006729 RID: 26409 RVA: 0x0012A9D8 File Offset: 0x00128BD8
		public override TaskStatus OnUpdate()
		{
			float num = (this._healthType == HealthComparison.HealthType.Constant) ? ((float)this._owner.Value.health.currentHealth) : ((float)this._owner.Value.health.percent * 100f);
			switch (this._operation)
			{
			case HealthComparison.Operation.LessThan:
				if (num >= this._value.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case HealthComparison.Operation.LessThanOrEqualTo:
				if (num > this._value.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case HealthComparison.Operation.EqualTo:
				if (!Mathf.Approximately(num, this._value.Value))
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case HealthComparison.Operation.NotEqualTo:
				if (Mathf.Approximately(num, this._value.Value))
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case HealthComparison.Operation.GreaterThanOrEqualTo:
				if (num < this._value.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			case HealthComparison.Operation.GreaterThan:
				if (num <= this._value.Value)
				{
					return TaskStatus.Failure;
				}
				return TaskStatus.Success;
			default:
				return TaskStatus.Failure;
			}
		}

		// Token: 0x04005328 RID: 21288
		[SerializeField]
		private HealthComparison.Operation _operation;

		// Token: 0x04005329 RID: 21289
		[SerializeField]
		private SharedCharacter _owner;

		// Token: 0x0400532A RID: 21290
		[SerializeField]
		private SharedFloat _value;

		// Token: 0x0400532B RID: 21291
		[SerializeField]
		private HealthComparison.HealthType _healthType;

		// Token: 0x020014B2 RID: 5298
		public enum Operation
		{
			// Token: 0x0400532D RID: 21293
			LessThan,
			// Token: 0x0400532E RID: 21294
			LessThanOrEqualTo,
			// Token: 0x0400532F RID: 21295
			EqualTo,
			// Token: 0x04005330 RID: 21296
			NotEqualTo,
			// Token: 0x04005331 RID: 21297
			GreaterThanOrEqualTo,
			// Token: 0x04005332 RID: 21298
			GreaterThan
		}

		// Token: 0x020014B3 RID: 5299
		public enum HealthType
		{
			// Token: 0x04005334 RID: 21300
			Percent,
			// Token: 0x04005335 RID: 21301
			Constant
		}
	}
}
