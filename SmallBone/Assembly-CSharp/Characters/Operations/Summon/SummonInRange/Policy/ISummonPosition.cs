using System;
using UnityEngine;

namespace Characters.Operations.Summon.SummonInRange.Policy
{
	// Token: 0x02000F5D RID: 3933
	public interface ISummonPosition
	{
		// Token: 0x06004C72 RID: 19570
		Vector2 GetPosition(Vector2 originPosition, float rangeX, int totalIndex, int currentIndex);
	}
}
