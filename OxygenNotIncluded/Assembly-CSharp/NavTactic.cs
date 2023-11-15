using System;
using UnityEngine;

// Token: 0x020004DE RID: 1246
public class NavTactic
{
	// Token: 0x06001CAC RID: 7340 RVA: 0x0009946E File Offset: 0x0009766E
	public NavTactic(int preferredRange, int rangePenalty = 1, int overlapPenalty = 1, int pathCostPenalty = 1)
	{
		this._overlapPenalty = overlapPenalty;
		this._preferredRange = preferredRange;
		this._rangePenalty = rangePenalty;
		this._pathCostPenalty = pathCostPenalty;
	}

	// Token: 0x06001CAD RID: 7341 RVA: 0x000994A8 File Offset: 0x000976A8
	public int GetCellPreferences(int root, CellOffset[] offsets, Navigator navigator)
	{
		int result = NavigationReservations.InvalidReservation;
		int num = int.MaxValue;
		for (int i = 0; i < offsets.Length; i++)
		{
			int num2 = Grid.OffsetCell(root, offsets[i]);
			int num3 = 0;
			num3 += this._overlapPenalty * NavigationReservations.Instance.GetOccupancyCount(num2);
			num3 += this._rangePenalty * Mathf.Abs(this._preferredRange - Grid.GetCellDistance(root, num2));
			num3 += this._pathCostPenalty * Mathf.Max(navigator.GetNavigationCost(num2), 0);
			if (num3 < num && navigator.CanReach(num2))
			{
				num = num3;
				result = num2;
			}
		}
		return result;
	}

	// Token: 0x04000FD4 RID: 4052
	private int _overlapPenalty = 3;

	// Token: 0x04000FD5 RID: 4053
	private int _preferredRange;

	// Token: 0x04000FD6 RID: 4054
	private int _rangePenalty = 2;

	// Token: 0x04000FD7 RID: 4055
	private int _pathCostPenalty = 1;
}
