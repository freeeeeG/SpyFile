using System;

// Token: 0x02000955 RID: 2389
public struct Extents
{
	// Token: 0x0600460C RID: 17932 RVA: 0x0018C358 File Offset: 0x0018A558
	public static Extents OneCell(int cell)
	{
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		return new Extents(num, num2, 1, 1);
	}

	// Token: 0x0600460D RID: 17933 RVA: 0x0018C378 File Offset: 0x0018A578
	public Extents(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
	}

	// Token: 0x0600460E RID: 17934 RVA: 0x0018C398 File Offset: 0x0018A598
	public Extents(int cell, int radius)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		this.x = num - radius;
		this.y = num2 - radius;
		this.width = radius * 2 + 1;
		this.height = radius * 2 + 1;
	}

	// Token: 0x0600460F RID: 17935 RVA: 0x0018C3DB File Offset: 0x0018A5DB
	public Extents(int center_x, int center_y, int radius)
	{
		this.x = center_x - radius;
		this.y = center_y - radius;
		this.width = radius * 2 + 1;
		this.height = radius * 2 + 1;
	}

	// Token: 0x06004610 RID: 17936 RVA: 0x0018C408 File Offset: 0x0018A608
	public Extents(int cell, CellOffset[] offsets)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset offset in offsets)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, offset), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06004611 RID: 17937 RVA: 0x0018C4A4 File Offset: 0x0018A6A4
	public Extents(int cell, CellOffset[] offsets, Orientation orientation)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		for (int i = 0; i < offsets.Length; i++)
		{
			CellOffset rotatedCellOffset = Rotatable.GetRotatedCellOffset(offsets[i], orientation);
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, rotatedCellOffset), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06004612 RID: 17938 RVA: 0x0018C544 File Offset: 0x0018A744
	public Extents(int cell, CellOffset[][] offset_table)
	{
		int num = 0;
		int num2 = 0;
		Grid.CellToXY(cell, out num, out num2);
		int num3 = num;
		int num4 = num2;
		foreach (CellOffset[] array in offset_table)
		{
			int val = 0;
			int val2 = 0;
			Grid.CellToXY(Grid.OffsetCell(cell, array[0]), out val, out val2);
			num = Math.Min(num, val);
			num2 = Math.Min(num2, val2);
			num3 = Math.Max(num3, val);
			num4 = Math.Max(num4, val2);
		}
		this.x = num;
		this.y = num2;
		this.width = num3 - num + 1;
		this.height = num4 - num2 + 1;
	}

	// Token: 0x06004613 RID: 17939 RVA: 0x0018C5E0 File Offset: 0x0018A7E0
	public bool Contains(Vector2I pos)
	{
		return this.x <= pos.x && pos.x < this.x + this.width && this.y <= pos.y && pos.y < this.y + this.height;
	}

	// Token: 0x04002E67 RID: 11879
	public int x;

	// Token: 0x04002E68 RID: 11880
	public int y;

	// Token: 0x04002E69 RID: 11881
	public int width;

	// Token: 0x04002E6A RID: 11882
	public int height;
}
