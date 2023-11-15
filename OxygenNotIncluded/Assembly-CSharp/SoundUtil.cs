using System;
using UnityEngine;

// Token: 0x02000985 RID: 2437
public static class SoundUtil
{
	// Token: 0x060047F6 RID: 18422 RVA: 0x0019641C File Offset: 0x0019461C
	public static float GetLiquidDepth(int cell)
	{
		float num = 0f;
		num += Grid.Mass[cell] * (Grid.Element[cell].IsLiquid ? 1f : 0f);
		int num2 = Grid.CellBelow(cell);
		if (Grid.IsValidCell(num2))
		{
			num += Grid.Mass[num2] * (Grid.Element[num2].IsLiquid ? 1f : 0f);
		}
		return Mathf.Min(num / 1000f, 1f);
	}

	// Token: 0x060047F7 RID: 18423 RVA: 0x001964A1 File Offset: 0x001946A1
	public static float GetLiquidVolume(float mass)
	{
		return Mathf.Min(mass / 100f, 1f);
	}
}
