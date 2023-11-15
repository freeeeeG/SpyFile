using System;

// Token: 0x020006C9 RID: 1737
public class CellVisibility
{
	// Token: 0x06002F46 RID: 12102 RVA: 0x000F974C File Offset: 0x000F794C
	public CellVisibility()
	{
		Grid.GetVisibleExtents(out this.MinX, out this.MinY, out this.MaxX, out this.MaxY);
	}

	// Token: 0x06002F47 RID: 12103 RVA: 0x000F9774 File Offset: 0x000F7974
	public bool IsVisible(int cell)
	{
		int num = Grid.CellColumn(cell);
		if (num < this.MinX || num > this.MaxX)
		{
			return false;
		}
		int num2 = Grid.CellRow(cell);
		return num2 >= this.MinY && num2 <= this.MaxY;
	}

	// Token: 0x04001C16 RID: 7190
	private int MinX;

	// Token: 0x04001C17 RID: 7191
	private int MinY;

	// Token: 0x04001C18 RID: 7192
	private int MaxX;

	// Token: 0x04001C19 RID: 7193
	private int MaxY;
}
