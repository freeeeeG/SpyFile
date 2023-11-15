using System;
using UnityEngine;

// Token: 0x020008C5 RID: 2245
public class OffsetTracker
{
	// Token: 0x0600410D RID: 16653 RVA: 0x0016C7A0 File Offset: 0x0016A9A0
	public virtual CellOffset[] GetOffsets(int current_cell)
	{
		if (current_cell != this.previousCell)
		{
			global::Debug.Assert(!OffsetTracker.isExecutingWithinJob, "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
			this.UpdateCell(this.previousCell, current_cell);
			this.previousCell = current_cell;
		}
		if (this.offsets == null)
		{
			global::Debug.Assert(!OffsetTracker.isExecutingWithinJob, "OffsetTracker.GetOffsets() is making a mutating call but is currently executing within a job");
			this.UpdateOffsets(this.previousCell);
		}
		return this.offsets;
	}

	// Token: 0x0600410E RID: 16654 RVA: 0x0016C808 File Offset: 0x0016AA08
	public void ForceRefresh()
	{
		int cell = this.previousCell;
		this.previousCell = Grid.InvalidCell;
		this.Refresh(cell);
	}

	// Token: 0x0600410F RID: 16655 RVA: 0x0016C82E File Offset: 0x0016AA2E
	public void Refresh(int cell)
	{
		this.GetOffsets(cell);
	}

	// Token: 0x06004110 RID: 16656 RVA: 0x0016C838 File Offset: 0x0016AA38
	protected virtual void UpdateCell(int previous_cell, int current_cell)
	{
	}

	// Token: 0x06004111 RID: 16657 RVA: 0x0016C83A File Offset: 0x0016AA3A
	protected virtual void UpdateOffsets(int current_cell)
	{
	}

	// Token: 0x06004112 RID: 16658 RVA: 0x0016C83C File Offset: 0x0016AA3C
	public virtual void Clear()
	{
	}

	// Token: 0x06004113 RID: 16659 RVA: 0x0016C83E File Offset: 0x0016AA3E
	public virtual void DebugDrawExtents()
	{
	}

	// Token: 0x06004114 RID: 16660 RVA: 0x0016C840 File Offset: 0x0016AA40
	public virtual void DebugDrawEditor()
	{
	}

	// Token: 0x06004115 RID: 16661 RVA: 0x0016C844 File Offset: 0x0016AA44
	public virtual void DebugDrawOffsets(int cell)
	{
		foreach (CellOffset offset in this.GetOffsets(cell))
		{
			int cell2 = Grid.OffsetCell(cell, offset);
			Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
			Gizmos.DrawWireCube(Grid.CellToPosCCC(cell2, Grid.SceneLayer.Move), new Vector3(0.95f, 0.95f, 0.95f));
		}
	}

	// Token: 0x04002A5D RID: 10845
	public static bool isExecutingWithinJob;

	// Token: 0x04002A5E RID: 10846
	protected CellOffset[] offsets;

	// Token: 0x04002A5F RID: 10847
	protected int previousCell = Grid.InvalidCell;
}
