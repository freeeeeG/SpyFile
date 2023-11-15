using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Pope.Summon
{
	// Token: 0x0200121B RID: 4635
	public class OneRandomFanaticPolicy : FanaticPolicy
	{
		// Token: 0x06005AE0 RID: 23264 RVA: 0x0010D200 File Offset: 0x0010B400
		protected override void GetToSummons(ref List<FanaticFactory.SummonType> results, int count)
		{
			results.Clear();
			FanaticFactory.SummonType item = this._summonTypes.Random<FanaticFactory.SummonType>();
			for (int i = 0; i < count; i++)
			{
				results.Add(item);
			}
		}

		// Token: 0x04004950 RID: 18768
		[SerializeField]
		private FanaticFactory.SummonType[] _summonTypes = new FanaticFactory.SummonType[]
		{
			FanaticFactory.SummonType.Fanatic,
			FanaticFactory.SummonType.AgedFanatic,
			FanaticFactory.SummonType.MartyrFanatic
		};
	}
}
