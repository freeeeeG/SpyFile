using System;
using UnityEngine;

namespace Level
{
	// Token: 0x020004A9 RID: 1193
	[Serializable]
	public class CurrencyRangeByRarity
	{
		// Token: 0x060016EE RID: 5870 RVA: 0x000483B0 File Offset: 0x000465B0
		public int Evaluate(Rarity rarity)
		{
			switch (rarity)
			{
			case Rarity.Common:
				return this.Evaluate(this._commonRange);
			case Rarity.Rare:
				return this.Evaluate(this._rareRange);
			case Rarity.Unique:
				return this.Evaluate(this._uniqueRange);
			case Rarity.Legendary:
				return this.Evaluate(this._legendaryRange);
			default:
				return this.Evaluate(this._commonRange);
			}
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00048415 File Offset: 0x00046615
		private int Evaluate(Vector2Int range)
		{
			return UnityEngine.Random.Range(range.x, range.y);
		}

		// Token: 0x04001418 RID: 5144
		[SerializeField]
		private Vector2Int _commonRange;

		// Token: 0x04001419 RID: 5145
		[SerializeField]
		private Vector2Int _rareRange;

		// Token: 0x0400141A RID: 5146
		[SerializeField]
		private Vector2Int _uniqueRange;

		// Token: 0x0400141B RID: 5147
		[SerializeField]
		private Vector2Int _legendaryRange;
	}
}
