using System;
using Data;
using UnityEngine;

namespace Characters.Operations
{
	// Token: 0x02000DB8 RID: 3512
	public class ConsumeGold : Operation
	{
		// Token: 0x060046AE RID: 18094 RVA: 0x000CD280 File Offset: 0x000CB480
		public override void Run()
		{
			GameData.Currency.gold.Consume(this._amount);
		}

		// Token: 0x04003583 RID: 13699
		[Tooltip("소모할 골드량, GoldConstraint의 Amount와 값이 서로 다르지 않도록 주의")]
		[SerializeField]
		private int _amount;
	}
}
