using System;
using System.Collections.ObjectModel;
using System.Linq;
using GameResources;
using UnityEngine;

namespace Level
{
	// Token: 0x02000544 RID: 1348
	[Serializable]
	public class PotionPossibilities
	{
		// Token: 0x17000557 RID: 1367
		public int this[int index]
		{
			get
			{
				return this._possibilities[index];
			}
		}

		// Token: 0x17000558 RID: 1368
		public int this[Potion.Size size]
		{
			get
			{
				return this._possibilities[(int)size];
			}
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x00053034 File Offset: 0x00051234
		public Potion Get()
		{
			Potion.Size? size = PotionPossibilities.Evaluate(this._possibilities);
			if (size != null)
			{
				return CommonResource.instance.potions[size.Value];
			}
			return null;
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x0005306E File Offset: 0x0005126E
		public Potion.Size? Evaluate()
		{
			return PotionPossibilities.Evaluate(this._possibilities);
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x0005307B File Offset: 0x0005127B
		public PotionPossibilities(params int[] possibilities)
		{
			this._possibilities = possibilities;
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x0005308C File Offset: 0x0005128C
		public static Potion.Size? Evaluate(int[] possibilities)
		{
			int maxExclusive = Mathf.Max(possibilities.Sum(), 100);
			int num = UnityEngine.Random.Range(0, maxExclusive) + 1;
			for (int i = 0; i < possibilities.Length; i++)
			{
				num -= possibilities[i];
				if (num <= 0)
				{
					return new Potion.Size?(PotionPossibilities.values[i]);
				}
			}
			return null;
		}

		// Token: 0x04001710 RID: 5904
		public static readonly ReadOnlyCollection<Potion.Size> values = EnumValues<Potion.Size>.Values;

		// Token: 0x04001711 RID: 5905
		[SerializeField]
		[Range(0f, 100f)]
		private int[] _possibilities;
	}
}
