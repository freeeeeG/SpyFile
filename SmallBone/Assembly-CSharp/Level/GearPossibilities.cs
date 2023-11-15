using System;
using System.Collections.ObjectModel;
using System.Linq;
using Characters.Gear;
using UnityEngine;

namespace Level
{
	// Token: 0x020004E1 RID: 1249
	[Serializable]
	public class GearPossibilities
	{
		// Token: 0x170004D3 RID: 1235
		public int this[int index]
		{
			get
			{
				return this._possibilities[index];
			}
		}

		// Token: 0x170004D4 RID: 1236
		public int this[Gear.Type size]
		{
			get
			{
				return this._possibilities[(int)size];
			}
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x0004C3D7 File Offset: 0x0004A5D7
		public Gear.Type? Evaluate(System.Random random)
		{
			return GearPossibilities.Evaluate(random, this._possibilities);
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x0004C3E5 File Offset: 0x0004A5E5
		public Gear.Type? Evaluate()
		{
			return GearPossibilities.Evaluate(new System.Random(), this._possibilities);
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x0004C3F7 File Offset: 0x0004A5F7
		public GearPossibilities(params int[] possibilities)
		{
			this._possibilities = possibilities;
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x0004C408 File Offset: 0x0004A608
		public static Gear.Type? Evaluate(System.Random random, int[] possibilities)
		{
			int maxValue = Mathf.Max(possibilities.Sum(), 100);
			int num = random.Next(0, maxValue) + 1;
			for (int i = 0; i < possibilities.Length; i++)
			{
				num -= possibilities[i];
				if (num <= 0)
				{
					return new Gear.Type?(GearPossibilities.values[i]);
				}
			}
			return new Gear.Type?(GearPossibilities.values.Random<Gear.Type>());
		}

		// Token: 0x04001539 RID: 5433
		public static readonly ReadOnlyCollection<Gear.Type> values = EnumValues<Gear.Type>.Values;

		// Token: 0x0400153A RID: 5434
		[SerializeField]
		[Range(0f, 100f)]
		private int[] _possibilities;
	}
}
