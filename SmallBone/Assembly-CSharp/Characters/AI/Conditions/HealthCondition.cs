using System;
using UnityEngine;

namespace Characters.AI.Conditions
{
	// Token: 0x020011D1 RID: 4561
	public class HealthCondition : Condition
	{
		// Token: 0x0600599B RID: 22939 RVA: 0x0010A8E0 File Offset: 0x00108AE0
		protected override bool Check(AIController controller)
		{
			HealthCondition.Comparer compare = this._compare;
			if (compare != HealthCondition.Comparer.GreaterThan)
			{
				return compare == HealthCondition.Comparer.LessThan && controller.character.health.percent <= (double)this._percent;
			}
			return controller.character.health.percent >= (double)this._percent;
		}

		// Token: 0x0400485C RID: 18524
		[SerializeField]
		private HealthCondition.Comparer _compare;

		// Token: 0x0400485D RID: 18525
		[SerializeField]
		[Range(0f, 1f)]
		private float _percent;

		// Token: 0x020011D2 RID: 4562
		private enum Comparer
		{
			// Token: 0x0400485F RID: 18527
			GreaterThan,
			// Token: 0x04004860 RID: 18528
			LessThan
		}
	}
}
