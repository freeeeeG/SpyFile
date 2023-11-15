using System;
using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Pope.Summon
{
	// Token: 0x0200121D RID: 4637
	public class WeightedFanaticPolicy : FanaticPolicy
	{
		// Token: 0x06005AE4 RID: 23268 RVA: 0x0010D2A0 File Offset: 0x0010B4A0
		private void Awake()
		{
			this._summonTypes = new List<FanaticFactory.SummonType>(this._fanaticWeight + this._agedFanaticWeight + this._martyrFanaticWeight);
			for (int i = 0; i < this._fanaticWeight; i++)
			{
				this._summonTypes.Add(FanaticFactory.SummonType.Fanatic);
			}
			for (int j = 0; j < this._agedFanaticWeight; j++)
			{
				this._summonTypes.Add(FanaticFactory.SummonType.AgedFanatic);
			}
			for (int k = 0; k < this._martyrFanaticWeight; k++)
			{
				this._summonTypes.Add(FanaticFactory.SummonType.MartyrFanatic);
			}
		}

		// Token: 0x06005AE5 RID: 23269 RVA: 0x0010D324 File Offset: 0x0010B524
		protected override void GetToSummons(ref List<FanaticFactory.SummonType> results, int count)
		{
			results.Clear();
			for (int i = 0; i < count; i++)
			{
				results.Add(this._summonTypes.Random<FanaticFactory.SummonType>());
			}
		}

		// Token: 0x04004952 RID: 18770
		private List<FanaticFactory.SummonType> _summonTypes;

		// Token: 0x04004953 RID: 18771
		[SerializeField]
		private int _fanaticWeight = 1;

		// Token: 0x04004954 RID: 18772
		[SerializeField]
		private int _agedFanaticWeight = 1;

		// Token: 0x04004955 RID: 18773
		[SerializeField]
		private int _martyrFanaticWeight = 1;
	}
}
