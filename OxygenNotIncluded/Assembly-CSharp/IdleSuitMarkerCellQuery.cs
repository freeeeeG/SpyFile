using System;

// Token: 0x02000406 RID: 1030
public class IdleSuitMarkerCellQuery : PathFinderQuery
{
	// Token: 0x060015BB RID: 5563 RVA: 0x00072761 File Offset: 0x00070961
	public IdleSuitMarkerCellQuery(bool is_rotated, int marker_x)
	{
		this.targetCell = Grid.InvalidCell;
		this.isRotated = is_rotated;
		this.markerX = marker_x;
	}

	// Token: 0x060015BC RID: 5564 RVA: 0x00072784 File Offset: 0x00070984
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!Grid.PreventIdleTraversal[cell] && Grid.CellToXY(cell).x < this.markerX != this.isRotated)
		{
			this.targetCell = cell;
		}
		return this.targetCell != Grid.InvalidCell;
	}

	// Token: 0x060015BD RID: 5565 RVA: 0x000727D0 File Offset: 0x000709D0
	public override int GetResultCell()
	{
		return this.targetCell;
	}

	// Token: 0x04000C03 RID: 3075
	private int targetCell;

	// Token: 0x04000C04 RID: 3076
	private bool isRotated;

	// Token: 0x04000C05 RID: 3077
	private int markerX;
}
