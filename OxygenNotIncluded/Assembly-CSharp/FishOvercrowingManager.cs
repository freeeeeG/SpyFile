using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007C5 RID: 1989
[AddComponentMenu("KMonoBehaviour/scripts/FishOvercrowingManager")]
public class FishOvercrowingManager : KMonoBehaviour, ISim1000ms
{
	// Token: 0x06003744 RID: 14148 RVA: 0x0012B0A9 File Offset: 0x001292A9
	public static void DestroyInstance()
	{
		FishOvercrowingManager.Instance = null;
	}

	// Token: 0x06003745 RID: 14149 RVA: 0x0012B0B1 File Offset: 0x001292B1
	protected override void OnPrefabInit()
	{
		FishOvercrowingManager.Instance = this;
		this.cells = new FishOvercrowingManager.Cell[Grid.CellCount];
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x0012B0C9 File Offset: 0x001292C9
	public void Add(FishOvercrowdingMonitor.Instance fish)
	{
		this.fishes.Add(fish);
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x0012B0D7 File Offset: 0x001292D7
	public void Remove(FishOvercrowdingMonitor.Instance fish)
	{
		this.fishes.Remove(fish);
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x0012B0E8 File Offset: 0x001292E8
	public void Sim1000ms(float dt)
	{
		int num = this.versionCounter;
		this.versionCounter = num + 1;
		int num2 = num;
		int num3 = 1;
		this.cavityIdToCavityInfo.Clear();
		this.cellToFishCount.Clear();
		ListPool<FishOvercrowingManager.FishInfo, FishOvercrowingManager>.PooledList pooledList = ListPool<FishOvercrowingManager.FishInfo, FishOvercrowingManager>.Allocate();
		foreach (FishOvercrowdingMonitor.Instance instance in this.fishes)
		{
			int num4 = Grid.PosToCell(instance);
			if (Grid.IsValidCell(num4))
			{
				FishOvercrowingManager.FishInfo item = new FishOvercrowingManager.FishInfo
				{
					cell = num4,
					fish = instance
				};
				pooledList.Add(item);
				int num5 = 0;
				this.cellToFishCount.TryGetValue(num4, out num5);
				num5++;
				this.cellToFishCount[num4] = num5;
			}
		}
		foreach (FishOvercrowingManager.FishInfo fishInfo in pooledList)
		{
			ListPool<int, FishOvercrowingManager>.PooledList pooledList2 = ListPool<int, FishOvercrowingManager>.Allocate();
			pooledList2.Add(fishInfo.cell);
			int i = 0;
			int num6 = num3++;
			while (i < pooledList2.Count)
			{
				int num7 = pooledList2[i++];
				if (Grid.IsValidCell(num7))
				{
					FishOvercrowingManager.Cell cell = this.cells[num7];
					if (cell.version != num2 && Grid.IsLiquid(num7))
					{
						cell.cavityId = num6;
						cell.version = num2;
						int num8 = 0;
						this.cellToFishCount.TryGetValue(num7, out num8);
						FishOvercrowingManager.CavityInfo value = default(FishOvercrowingManager.CavityInfo);
						if (!this.cavityIdToCavityInfo.TryGetValue(num6, out value))
						{
							value = default(FishOvercrowingManager.CavityInfo);
						}
						value.fishCount += num8;
						value.cellCount++;
						this.cavityIdToCavityInfo[num6] = value;
						pooledList2.Add(Grid.CellLeft(num7));
						pooledList2.Add(Grid.CellRight(num7));
						pooledList2.Add(Grid.CellAbove(num7));
						pooledList2.Add(Grid.CellBelow(num7));
						this.cells[num7] = cell;
					}
				}
			}
			pooledList2.Recycle();
		}
		foreach (FishOvercrowingManager.FishInfo fishInfo2 in pooledList)
		{
			FishOvercrowingManager.Cell cell2 = this.cells[fishInfo2.cell];
			FishOvercrowingManager.CavityInfo cavityInfo = default(FishOvercrowingManager.CavityInfo);
			this.cavityIdToCavityInfo.TryGetValue(cell2.cavityId, out cavityInfo);
			fishInfo2.fish.SetOvercrowdingInfo(cavityInfo.cellCount, cavityInfo.fishCount);
		}
		pooledList.Recycle();
	}

	// Token: 0x04002200 RID: 8704
	public static FishOvercrowingManager Instance;

	// Token: 0x04002201 RID: 8705
	private List<FishOvercrowdingMonitor.Instance> fishes = new List<FishOvercrowdingMonitor.Instance>();

	// Token: 0x04002202 RID: 8706
	private Dictionary<int, FishOvercrowingManager.CavityInfo> cavityIdToCavityInfo = new Dictionary<int, FishOvercrowingManager.CavityInfo>();

	// Token: 0x04002203 RID: 8707
	private Dictionary<int, int> cellToFishCount = new Dictionary<int, int>();

	// Token: 0x04002204 RID: 8708
	private FishOvercrowingManager.Cell[] cells;

	// Token: 0x04002205 RID: 8709
	private int versionCounter = 1;

	// Token: 0x02001537 RID: 5431
	private struct Cell
	{
		// Token: 0x04006793 RID: 26515
		public int version;

		// Token: 0x04006794 RID: 26516
		public int cavityId;
	}

	// Token: 0x02001538 RID: 5432
	private struct FishInfo
	{
		// Token: 0x04006795 RID: 26517
		public int cell;

		// Token: 0x04006796 RID: 26518
		public FishOvercrowdingMonitor.Instance fish;
	}

	// Token: 0x02001539 RID: 5433
	private struct CavityInfo
	{
		// Token: 0x04006797 RID: 26519
		public int fishCount;

		// Token: 0x04006798 RID: 26520
		public int cellCount;
	}
}
