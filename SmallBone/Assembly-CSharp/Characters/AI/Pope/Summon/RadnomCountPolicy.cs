using System;
using UnityEngine;

namespace Characters.AI.Pope.Summon
{
	// Token: 0x0200121A RID: 4634
	public sealed class RadnomCountPolicy : CountPolicy
	{
		// Token: 0x06005ADE RID: 23262 RVA: 0x0010D1DF File Offset: 0x0010B3DF
		public override int GetCount()
		{
			return UnityEngine.Random.Range(this._range.x, this._range.y + 1);
		}

		// Token: 0x0400494F RID: 18767
		[MinMaxSlider(0f, 100f)]
		[SerializeField]
		private Vector2Int _range;
	}
}
