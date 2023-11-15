using System;

// Token: 0x020003F3 RID: 1011
public class NavTable
{
	// Token: 0x0600155F RID: 5471 RVA: 0x0007103C File Offset: 0x0006F23C
	public NavTable(int cell_count)
	{
		this.ValidCells = new short[cell_count];
		this.NavTypeMasks = new short[11];
		for (short num = 0; num < 11; num += 1)
		{
			this.NavTypeMasks[(int)num] = (short)(1 << (int)num);
		}
	}

	// Token: 0x06001560 RID: 5472 RVA: 0x00071085 File Offset: 0x0006F285
	public bool IsValid(int cell, NavType nav_type = NavType.Floor)
	{
		return Grid.IsValidCell(cell) && (this.NavTypeMasks[(int)nav_type] & this.ValidCells[cell]) != 0;
	}

	// Token: 0x06001561 RID: 5473 RVA: 0x000710A8 File Offset: 0x0006F2A8
	public void SetValid(int cell, NavType nav_type, bool is_valid)
	{
		short num = this.NavTypeMasks[(int)nav_type];
		short num2 = this.ValidCells[cell];
		if ((num2 & num) != 0 != is_valid)
		{
			if (is_valid)
			{
				this.ValidCells[cell] = (num | num2);
			}
			else
			{
				this.ValidCells[cell] = (~num & num2);
			}
			if (this.OnValidCellChanged != null)
			{
				this.OnValidCellChanged(cell, nav_type);
			}
		}
	}

	// Token: 0x04000BB3 RID: 2995
	public Action<int, NavType> OnValidCellChanged;

	// Token: 0x04000BB4 RID: 2996
	private short[] NavTypeMasks;

	// Token: 0x04000BB5 RID: 2997
	private short[] ValidCells;
}
