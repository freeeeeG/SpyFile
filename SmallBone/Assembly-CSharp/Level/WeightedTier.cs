using System;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Level
{
	// Token: 0x02000543 RID: 1347
	[Serializable]
	public class WeightedTier
	{
		// Token: 0x17000555 RID: 1365
		public int this[int index]
		{
			get
			{
				return this._possibilities[index];
			}
		}

		// Token: 0x17000556 RID: 1366
		public int this[Tier size]
		{
			get
			{
				return this._possibilities[(int)size];
			}
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x00052F2F File Offset: 0x0005112F
		public WeightedTier(params int[] possibilities)
		{
			this._possibilities = possibilities;
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x00052F40 File Offset: 0x00051140
		public Tier Get(System.Random random)
		{
			Tier? tier = WeightedTier.Evaluate(this._possibilities, random);
			if (tier != null)
			{
				return tier.Value;
			}
			return Tier.Low;
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x00052F6C File Offset: 0x0005116C
		public static Tier? Evaluate(int[] possibilities)
		{
			int maxExclusive = Mathf.Max(possibilities.Sum(), 100);
			int num = UnityEngine.Random.Range(0, maxExclusive) + 1;
			for (int i = 0; i < possibilities.Length; i++)
			{
				num -= possibilities[i];
				if (num <= 0)
				{
					return new Tier?(WeightedTier.values[i]);
				}
			}
			return null;
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00052FC4 File Offset: 0x000511C4
		public static Tier? Evaluate(int[] possibilities, System.Random random)
		{
			int maxValue = Mathf.Max(possibilities.Sum(), 100);
			int num = random.Next(0, maxValue) + 1;
			for (int i = 0; i < possibilities.Length; i++)
			{
				num -= possibilities[i];
				if (num <= 0)
				{
					return new Tier?(WeightedTier.values[i]);
				}
			}
			return null;
		}

		// Token: 0x0400170E RID: 5902
		public static readonly ReadOnlyCollection<Tier> values = EnumValues<Tier>.Values;

		// Token: 0x0400170F RID: 5903
		[SerializeField]
		[Range(0f, 100f)]
		private int[] _possibilities;
	}
}
