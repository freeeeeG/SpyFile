using System;
using UnityEngine;

// Token: 0x020007EA RID: 2026
public struct GridArea
{
	// Token: 0x17000422 RID: 1058
	// (get) Token: 0x060039A1 RID: 14753 RVA: 0x001417E1 File Offset: 0x0013F9E1
	public Vector2I Min
	{
		get
		{
			return this.min;
		}
	}

	// Token: 0x17000423 RID: 1059
	// (get) Token: 0x060039A2 RID: 14754 RVA: 0x001417E9 File Offset: 0x0013F9E9
	public Vector2I Max
	{
		get
		{
			return this.max;
		}
	}

	// Token: 0x060039A3 RID: 14755 RVA: 0x001417F4 File Offset: 0x0013F9F4
	public void SetArea(int cell, int width, int height)
	{
		Vector2I vector2I = Grid.CellToXY(cell);
		Vector2I vector2I2 = new Vector2I(vector2I.x + width, vector2I.y + height);
		this.SetExtents(vector2I.x, vector2I.y, vector2I2.x, vector2I2.y);
	}

	// Token: 0x060039A4 RID: 14756 RVA: 0x00141840 File Offset: 0x0013FA40
	public void SetExtents(int min_x, int min_y, int max_x, int max_y)
	{
		this.min.x = Math.Max(min_x, 0);
		this.min.y = Math.Max(min_y, 0);
		this.max.x = Math.Min(max_x, Grid.WidthInCells);
		this.max.y = Math.Min(max_y, Grid.HeightInCells);
		this.MinCell = Grid.XYToCell(this.min.x, this.min.y);
		this.MaxCell = Grid.XYToCell(this.max.x, this.max.y);
	}

	// Token: 0x060039A5 RID: 14757 RVA: 0x001418E0 File Offset: 0x0013FAE0
	public bool Contains(int cell)
	{
		if (cell >= this.MinCell && cell < this.MaxCell)
		{
			int num = cell % Grid.WidthInCells;
			return num >= this.Min.x && num < this.Max.x;
		}
		return false;
	}

	// Token: 0x060039A6 RID: 14758 RVA: 0x00141927 File Offset: 0x0013FB27
	public bool Contains(int x, int y)
	{
		return x >= this.min.x && x < this.max.x && y >= this.min.y && y < this.max.y;
	}

	// Token: 0x060039A7 RID: 14759 RVA: 0x00141964 File Offset: 0x0013FB64
	public bool Contains(Vector3 pos)
	{
		return (float)this.min.x <= pos.x && pos.x < (float)this.max.x && (float)this.min.y <= pos.y && pos.y <= (float)this.max.y;
	}

	// Token: 0x060039A8 RID: 14760 RVA: 0x001419C6 File Offset: 0x0013FBC6
	public void RunIfInside(int cell, Action<int> action)
	{
		if (this.Contains(cell))
		{
			action(cell);
		}
	}

	// Token: 0x060039A9 RID: 14761 RVA: 0x001419D8 File Offset: 0x0013FBD8
	public void Run(Action<int> action)
	{
		for (int i = this.min.y; i < this.max.y; i++)
		{
			for (int j = this.min.x; j < this.max.x; j++)
			{
				int obj = Grid.XYToCell(j, i);
				action(obj);
			}
		}
	}

	// Token: 0x060039AA RID: 14762 RVA: 0x00141A34 File Offset: 0x0013FC34
	public void RunOnDifference(GridArea subtract_area, Action<int> action)
	{
		for (int i = this.min.y; i < this.max.y; i++)
		{
			for (int j = this.min.x; j < this.max.x; j++)
			{
				if (!subtract_area.Contains(j, i))
				{
					int obj = Grid.XYToCell(j, i);
					action(obj);
				}
			}
		}
	}

	// Token: 0x060039AB RID: 14763 RVA: 0x00141A9B File Offset: 0x0013FC9B
	public int GetCellCount()
	{
		return (this.max.x - this.min.x) * (this.max.y - this.min.y);
	}

	// Token: 0x04002670 RID: 9840
	private Vector2I min;

	// Token: 0x04002671 RID: 9841
	private Vector2I max;

	// Token: 0x04002672 RID: 9842
	private int MinCell;

	// Token: 0x04002673 RID: 9843
	private int MaxCell;
}
