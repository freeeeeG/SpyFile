using System;
using UnityEngine;

namespace Characters.Operations.Summon.SummonInRange.Policy
{
	// Token: 0x02000F5E RID: 3934
	[Serializable]
	public class IntervalRegularly : ISummonPosition
	{
		// Token: 0x06004C73 RID: 19571 RVA: 0x000E2958 File Offset: 0x000E0B58
		public Vector2 GetPosition(Vector2 originPosition, float rangeX, int totalIndex, int currentIndex)
		{
			if (totalIndex == 1)
			{
				return originPosition;
			}
			if (this._interval == 0f)
			{
				this._interval = rangeX / (float)(totalIndex - 1);
			}
			Vector2 result = originPosition;
			if (!this._revese)
			{
				result.x += this._interval * (float)currentIndex - rangeX / 2f;
			}
			else
			{
				result.x += rangeX / 2f - this._interval * (float)currentIndex;
			}
			return result;
		}

		// Token: 0x04003C10 RID: 15376
		[SerializeField]
		private bool _revese;

		// Token: 0x04003C11 RID: 15377
		private float _interval;
	}
}
