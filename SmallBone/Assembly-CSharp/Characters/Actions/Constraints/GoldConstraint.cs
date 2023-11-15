using System;
using Data;
using UnityEngine;

namespace Characters.Actions.Constraints
{
	// Token: 0x0200097B RID: 2427
	public class GoldConstraint : Constraint
	{
		// Token: 0x0600342D RID: 13357 RVA: 0x0009A7B7 File Offset: 0x000989B7
		public override bool Pass()
		{
			return GameData.Currency.gold.Has(this._amount);
		}

		// Token: 0x04002A3E RID: 10814
		[SerializeField]
		[Tooltip("필요 골드량, ConsumeGold 오퍼레이션으로 같은 Amount만큼 소모되게 해주어야 함")]
		private int _amount;
	}
}
