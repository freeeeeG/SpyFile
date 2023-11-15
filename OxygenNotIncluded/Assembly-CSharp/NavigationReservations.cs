using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004DC RID: 1244
[AddComponentMenu("KMonoBehaviour/scripts/NavigationReservations")]
public class NavigationReservations : KMonoBehaviour
{
	// Token: 0x06001CA4 RID: 7332 RVA: 0x0009936A File Offset: 0x0009756A
	public static void DestroyInstance()
	{
		NavigationReservations.Instance = null;
	}

	// Token: 0x06001CA5 RID: 7333 RVA: 0x00099372 File Offset: 0x00097572
	public int GetOccupancyCount(int cell)
	{
		if (this.cellOccupancyDensity.ContainsKey(cell))
		{
			return this.cellOccupancyDensity[cell];
		}
		return 0;
	}

	// Token: 0x06001CA6 RID: 7334 RVA: 0x00099390 File Offset: 0x00097590
	public void AddOccupancy(int cell)
	{
		if (!this.cellOccupancyDensity.ContainsKey(cell))
		{
			this.cellOccupancyDensity.Add(cell, 1);
			return;
		}
		Dictionary<int, int> dictionary = this.cellOccupancyDensity;
		dictionary[cell]++;
	}

	// Token: 0x06001CA7 RID: 7335 RVA: 0x000993D4 File Offset: 0x000975D4
	public void RemoveOccupancy(int cell)
	{
		int num = 0;
		if (this.cellOccupancyDensity.TryGetValue(cell, out num))
		{
			if (num == 1)
			{
				this.cellOccupancyDensity.Remove(cell);
				return;
			}
			this.cellOccupancyDensity[cell] = num - 1;
		}
	}

	// Token: 0x06001CA8 RID: 7336 RVA: 0x00099414 File Offset: 0x00097614
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NavigationReservations.Instance = this;
	}

	// Token: 0x04000FCE RID: 4046
	public static NavigationReservations Instance;

	// Token: 0x04000FCF RID: 4047
	public static int InvalidReservation = -1;

	// Token: 0x04000FD0 RID: 4048
	private Dictionary<int, int> cellOccupancyDensity = new Dictionary<int, int>();
}
