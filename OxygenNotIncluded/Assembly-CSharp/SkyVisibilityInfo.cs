using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x02000975 RID: 2421
public readonly struct SkyVisibilityInfo
{
	// Token: 0x0600474F RID: 18255 RVA: 0x00192F14 File Offset: 0x00191114
	public SkyVisibilityInfo(CellOffset scanLeftOffset, int scanLeftCount, CellOffset scanRightOffset, int scanRightCount, int verticalStep)
	{
		this.scanLeftOffset = scanLeftOffset;
		this.scanLeftCount = scanLeftCount;
		this.scanRightOffset = scanRightOffset;
		this.scanRightCount = scanRightCount;
		this.verticalStep = verticalStep;
		this.totalColumnsCount = scanLeftCount + scanRightCount + (scanRightOffset.x - scanLeftOffset.x + 1);
	}

	// Token: 0x06004750 RID: 18256 RVA: 0x00192F60 File Offset: 0x00191160
	[return: TupleElementNames(new string[]
	{
		"isAnyVisible",
		"percentVisible01"
	})]
	public ValueTuple<bool, float> GetVisibilityOf(GameObject gameObject)
	{
		return this.GetVisibilityOf(Grid.PosToCell(gameObject));
	}

	// Token: 0x06004751 RID: 18257 RVA: 0x00192F70 File Offset: 0x00191170
	[return: TupleElementNames(new string[]
	{
		"isAnyVisible",
		"percentVisible01"
	})]
	public ValueTuple<bool, float> GetVisibilityOf(int buildingCenterCellId)
	{
		int num = 0;
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[buildingCenterCellId]);
		num += SkyVisibilityInfo.ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, this.scanLeftOffset), -1, this.verticalStep, this.scanLeftCount, world);
		num += SkyVisibilityInfo.ScanAndGetVisibleCellCount(Grid.OffsetCell(buildingCenterCellId, this.scanRightOffset), 1, this.verticalStep, this.scanRightCount, world);
		if (this.scanLeftOffset.x == this.scanRightOffset.x)
		{
			num = Mathf.Max(0, num - 1);
		}
		return new ValueTuple<bool, float>(num > 0, (float)num / (float)this.totalColumnsCount);
	}

	// Token: 0x06004752 RID: 18258 RVA: 0x0019300C File Offset: 0x0019120C
	public void CollectVisibleCellsTo(HashSet<int> visibleCells, int buildingBottomLeftCellId, WorldContainer originWorld)
	{
		SkyVisibilityInfo.ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, this.scanLeftOffset), -1, this.verticalStep, this.scanLeftCount, originWorld);
		SkyVisibilityInfo.ScanAndCollectVisibleCellsTo(visibleCells, Grid.OffsetCell(buildingBottomLeftCellId, this.scanRightOffset), 1, this.verticalStep, this.scanRightCount, originWorld);
	}

	// Token: 0x06004753 RID: 18259 RVA: 0x0019305C File Offset: 0x0019125C
	private static void ScanAndCollectVisibleCellsTo(HashSet<int> visibleCells, int originCellId, int stepX, int stepY, int stepCountInclusive, WorldContainer originWorld)
	{
		for (int i = 0; i <= stepCountInclusive; i++)
		{
			int num = Grid.OffsetCell(originCellId, i * stepX, i * stepY);
			if (!SkyVisibilityInfo.IsVisible(num, originWorld))
			{
				break;
			}
			visibleCells.Add(num);
		}
	}

	// Token: 0x06004754 RID: 18260 RVA: 0x00193098 File Offset: 0x00191298
	private static int ScanAndGetVisibleCellCount(int originCellId, int stepX, int stepY, int stepCountInclusive, WorldContainer originWorld)
	{
		for (int i = 0; i <= stepCountInclusive; i++)
		{
			if (!SkyVisibilityInfo.IsVisible(Grid.OffsetCell(originCellId, i * stepX, i * stepY), originWorld))
			{
				return i;
			}
		}
		return stepCountInclusive + 1;
	}

	// Token: 0x06004755 RID: 18261 RVA: 0x001930CC File Offset: 0x001912CC
	public static bool IsVisible(int cellId, WorldContainer originWorld)
	{
		if (!Grid.IsValidCell(cellId))
		{
			return false;
		}
		if (Grid.ExposedToSunlight[cellId] > 0)
		{
			return true;
		}
		WorldContainer world = ClusterManager.Instance.GetWorld((int)Grid.WorldIdx[cellId]);
		if (world != null && world.IsModuleInterior)
		{
			return true;
		}
		originWorld != world;
		return false;
	}

	// Token: 0x04002F3A RID: 12090
	public readonly CellOffset scanLeftOffset;

	// Token: 0x04002F3B RID: 12091
	public readonly int scanLeftCount;

	// Token: 0x04002F3C RID: 12092
	public readonly CellOffset scanRightOffset;

	// Token: 0x04002F3D RID: 12093
	public readonly int scanRightCount;

	// Token: 0x04002F3E RID: 12094
	public readonly int verticalStep;

	// Token: 0x04002F3F RID: 12095
	public readonly int totalColumnsCount;
}
