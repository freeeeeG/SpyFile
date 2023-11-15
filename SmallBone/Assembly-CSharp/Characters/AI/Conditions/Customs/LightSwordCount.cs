using System;
using Characters.AI.Hero.LightSwords;
using UnityEngine;

namespace Characters.AI.Conditions.Customs
{
	// Token: 0x020011D6 RID: 4566
	public sealed class LightSwordCount : Condition
	{
		// Token: 0x060059A1 RID: 22945 RVA: 0x0010A9A8 File Offset: 0x00108BA8
		protected override bool Check(AIController controller)
		{
			int activatedSwordCount = this._helper.GetActivatedSwordCount();
			LightSwordCount.Comparer comparer = this._comparer;
			if (comparer != LightSwordCount.Comparer.GreaterThan)
			{
				return comparer == LightSwordCount.Comparer.LessThan && activatedSwordCount <= this._count;
			}
			return activatedSwordCount >= this._count;
		}

		// Token: 0x04004868 RID: 18536
		[SerializeField]
		private LightSwordFieldHelper _helper;

		// Token: 0x04004869 RID: 18537
		[SerializeField]
		private int _count;

		// Token: 0x0400486A RID: 18538
		[SerializeField]
		private LightSwordCount.Comparer _comparer;

		// Token: 0x020011D7 RID: 4567
		private enum Comparer
		{
			// Token: 0x0400486C RID: 18540
			GreaterThan,
			// Token: 0x0400486D RID: 18541
			LessThan
		}
	}
}
