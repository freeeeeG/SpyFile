using System;
using System.Collections.ObjectModel;
using System.Linq;
using Data;
using UnityEngine;

namespace Level
{
	// Token: 0x020004A8 RID: 1192
	[Serializable]
	public class CurrencyPossibilities
	{
		// Token: 0x17000481 RID: 1153
		public int this[int index]
		{
			get
			{
				return this._possibilities[index];
			}
		}

		// Token: 0x17000482 RID: 1154
		public int this[GameData.Currency.Type size]
		{
			get
			{
				return this._possibilities[(int)size];
			}
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x000482C9 File Offset: 0x000464C9
		public GameData.Currency.Type? Evaluate(System.Random random)
		{
			return this.Evaluate(random, this._possibilities);
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x000482D8 File Offset: 0x000464D8
		public GameData.Currency.Type? Evaluate()
		{
			return CurrencyPossibilities.Evaluate(this._possibilities);
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x000482E5 File Offset: 0x000464E5
		public CurrencyPossibilities(params int[] possibilities)
		{
			this._possibilities = possibilities;
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x000482F4 File Offset: 0x000464F4
		public GameData.Currency.Type? Evaluate(System.Random random, int[] possibilities)
		{
			int maxValue = Mathf.Max(possibilities.Sum(), 100);
			int num = random.Next(0, maxValue) + 1;
			for (int i = 0; i < possibilities.Length; i++)
			{
				num -= possibilities[i];
				if (num <= 0)
				{
					return new GameData.Currency.Type?(CurrencyPossibilities.values[i]);
				}
			}
			return null;
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x0004834C File Offset: 0x0004654C
		public static GameData.Currency.Type? Evaluate(int[] possibilities)
		{
			int maxExclusive = Mathf.Max(possibilities.Sum(), 100);
			int num = UnityEngine.Random.Range(0, maxExclusive) + 1;
			for (int i = 0; i < possibilities.Length; i++)
			{
				num -= possibilities[i];
				if (num <= 0)
				{
					return new GameData.Currency.Type?(CurrencyPossibilities.values[i]);
				}
			}
			return null;
		}

		// Token: 0x04001416 RID: 5142
		public static readonly ReadOnlyCollection<GameData.Currency.Type> values = EnumValues<GameData.Currency.Type>.Values;

		// Token: 0x04001417 RID: 5143
		[Range(0f, 100f)]
		[SerializeField]
		private int[] _possibilities;
	}
}
