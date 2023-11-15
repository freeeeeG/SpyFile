using System;
using UnityEngine;

namespace Characters.Operations.Summon.SummonInRange.Policy
{
	// Token: 0x02000F5F RID: 3935
	[Serializable]
	public class Randomly : ISummonPosition
	{
		// Token: 0x06004C75 RID: 19573 RVA: 0x000E29CC File Offset: 0x000E0BCC
		public Vector2 GetPosition(Vector2 originPosition, float rangeX, int totalIndex, int currentIndex)
		{
			if (this._prevRandomRange == 0f)
			{
				this._prevRandomRange = rangeX;
			}
			if (this._prevRandomRange >= rangeX / 2f)
			{
				this._prevRandomRange = UnityEngine.Random.Range(0f, this._prevRandomRange);
			}
			else
			{
				this._prevRandomRange = UnityEngine.Random.Range(this._prevRandomRange, rangeX);
			}
			originPosition.x += this._prevRandomRange - rangeX / 2f;
			return originPosition;
		}

		// Token: 0x04003C12 RID: 15378
		private float _prevRandomRange;
	}
}
