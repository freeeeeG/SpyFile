using System;

// Token: 0x020003F7 RID: 1015
public static class PathFinderQueries
{
	// Token: 0x06001579 RID: 5497 RVA: 0x00071A9C File Offset: 0x0006FC9C
	public static void Reset()
	{
		PathFinderQueries.cellQuery = new CellQuery();
		PathFinderQueries.cellCostQuery = new CellCostQuery();
		PathFinderQueries.cellArrayQuery = new CellArrayQuery();
		PathFinderQueries.cellOffsetQuery = new CellOffsetQuery();
		PathFinderQueries.safeCellQuery = new SafeCellQuery();
		PathFinderQueries.idleCellQuery = new IdleCellQuery();
		PathFinderQueries.breathableCellQuery = new BreathableCellQuery();
		PathFinderQueries.drawNavGridQuery = new DrawNavGridQuery();
		PathFinderQueries.plantableCellQuery = new PlantableCellQuery();
		PathFinderQueries.mineableCellQuery = new MineableCellQuery();
		PathFinderQueries.staterpillarCellQuery = new StaterpillarCellQuery();
		PathFinderQueries.floorCellQuery = new FloorCellQuery();
		PathFinderQueries.buildingPlacementQuery = new BuildingPlacementQuery();
	}

	// Token: 0x04000BBC RID: 3004
	public static CellQuery cellQuery = new CellQuery();

	// Token: 0x04000BBD RID: 3005
	public static CellCostQuery cellCostQuery = new CellCostQuery();

	// Token: 0x04000BBE RID: 3006
	public static CellArrayQuery cellArrayQuery = new CellArrayQuery();

	// Token: 0x04000BBF RID: 3007
	public static CellOffsetQuery cellOffsetQuery = new CellOffsetQuery();

	// Token: 0x04000BC0 RID: 3008
	public static SafeCellQuery safeCellQuery = new SafeCellQuery();

	// Token: 0x04000BC1 RID: 3009
	public static IdleCellQuery idleCellQuery = new IdleCellQuery();

	// Token: 0x04000BC2 RID: 3010
	public static BreathableCellQuery breathableCellQuery = new BreathableCellQuery();

	// Token: 0x04000BC3 RID: 3011
	public static DrawNavGridQuery drawNavGridQuery = new DrawNavGridQuery();

	// Token: 0x04000BC4 RID: 3012
	public static PlantableCellQuery plantableCellQuery = new PlantableCellQuery();

	// Token: 0x04000BC5 RID: 3013
	public static MineableCellQuery mineableCellQuery = new MineableCellQuery();

	// Token: 0x04000BC6 RID: 3014
	public static StaterpillarCellQuery staterpillarCellQuery = new StaterpillarCellQuery();

	// Token: 0x04000BC7 RID: 3015
	public static FloorCellQuery floorCellQuery = new FloorCellQuery();

	// Token: 0x04000BC8 RID: 3016
	public static BuildingPlacementQuery buildingPlacementQuery = new BuildingPlacementQuery();
}
