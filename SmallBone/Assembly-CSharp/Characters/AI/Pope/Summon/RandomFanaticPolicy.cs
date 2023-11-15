using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Pope.Summon
{
	// Token: 0x0200121C RID: 4636
	public class RandomFanaticPolicy : FanaticPolicy
	{
		// Token: 0x06005AE2 RID: 23266 RVA: 0x0010D250 File Offset: 0x0010B450
		protected override void GetToSummons(ref List<FanaticFactory.SummonType> results, int count)
		{
			results.Clear();
			for (int i = 0; i < count; i++)
			{
				results.Add(this._summonTypes.Random<FanaticFactory.SummonType>());
			}
		}

		// Token: 0x04004951 RID: 18769
		[SerializeField]
		private FanaticFactory.SummonType[] _summonTypes = new FanaticFactory.SummonType[]
		{
			FanaticFactory.SummonType.Fanatic,
			FanaticFactory.SummonType.AgedFanatic,
			FanaticFactory.SummonType.MartyrFanatic
		};
	}
}
