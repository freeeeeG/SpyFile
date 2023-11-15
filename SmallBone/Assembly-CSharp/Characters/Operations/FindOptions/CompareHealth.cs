using System;
using UnityEngine;

namespace Characters.Operations.FindOptions
{
	// Token: 0x02000E97 RID: 3735
	[Serializable]
	public class CompareHealth : ICondition
	{
		// Token: 0x060049CE RID: 18894 RVA: 0x000D78CC File Offset: 0x000D5ACC
		public bool Satisfied(Character character)
		{
			float num = (this._healthType == CompareHealth.HealthType.Constant) ? ((float)character.health.currentHealth) : ((float)character.health.percent * 100f);
			switch (this._operation)
			{
			case CompareHealth.Operation.LessThan:
				return num < this._value.value;
			case CompareHealth.Operation.LessThanOrEqualTo:
				return num <= this._value.value;
			case CompareHealth.Operation.EqualTo:
				return Mathf.Approximately(num, this._value.value);
			case CompareHealth.Operation.NotEqualTo:
				return !Mathf.Approximately(num, this._value.value);
			case CompareHealth.Operation.GreaterThanOrEqualTo:
				return num >= this._value.value;
			case CompareHealth.Operation.GreaterThan:
				return num > this._value.value;
			default:
				return false;
			}
		}

		// Token: 0x0400390B RID: 14603
		[SerializeField]
		private CompareHealth.Operation _operation;

		// Token: 0x0400390C RID: 14604
		[SerializeField]
		private CompareHealth.HealthType _healthType;

		// Token: 0x0400390D RID: 14605
		[SerializeField]
		private CustomFloat _value;

		// Token: 0x02000E98 RID: 3736
		public enum Operation
		{
			// Token: 0x0400390F RID: 14607
			LessThan,
			// Token: 0x04003910 RID: 14608
			LessThanOrEqualTo,
			// Token: 0x04003911 RID: 14609
			EqualTo,
			// Token: 0x04003912 RID: 14610
			NotEqualTo,
			// Token: 0x04003913 RID: 14611
			GreaterThanOrEqualTo,
			// Token: 0x04003914 RID: 14612
			GreaterThan
		}

		// Token: 0x02000E99 RID: 3737
		public enum HealthType
		{
			// Token: 0x04003916 RID: 14614
			Percent,
			// Token: 0x04003917 RID: 14615
			Constant
		}
	}
}
