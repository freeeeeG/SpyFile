using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Level
{
	// Token: 0x02000531 RID: 1329
	[Serializable]
	public class ValueByRarity
	{
		// Token: 0x17000541 RID: 1345
		public float this[int index]
		{
			get
			{
				return this._values[index];
			}
		}

		// Token: 0x17000542 RID: 1346
		public float this[Rarity rarity]
		{
			get
			{
				return this._values[(int)rarity];
			}
		}

		// Token: 0x06001A19 RID: 6681 RVA: 0x00051C79 File Offset: 0x0004FE79
		public ValueByRarity(params float[] values)
		{
			this._values = values;
		}

		// Token: 0x040016D4 RID: 5844
		public static readonly ReadOnlyCollection<string> names = EnumValues<Rarity>.Names;

		// Token: 0x040016D5 RID: 5845
		public static readonly ReadOnlyCollection<Rarity> values = EnumValues<Rarity>.Values;

		// Token: 0x040016D6 RID: 5846
		[SerializeField]
		private float[] _values;
	}
}
