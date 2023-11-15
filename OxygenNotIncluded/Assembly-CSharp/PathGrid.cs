using System;
using System.Collections.Generic;

// Token: 0x020003FB RID: 1019
public class PathGrid
{
	// Token: 0x06001586 RID: 5510 RVA: 0x00071C2A File Offset: 0x0006FE2A
	public void SetGroupProber(IGroupProber group_prober)
	{
		this.groupProber = group_prober;
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x00071C34 File Offset: 0x0006FE34
	public PathGrid(int width_in_cells, int height_in_cells, bool apply_offset, NavType[] valid_nav_types)
	{
		this.applyOffset = apply_offset;
		this.widthInCells = width_in_cells;
		this.heightInCells = height_in_cells;
		this.ValidNavTypes = valid_nav_types;
		int num = 0;
		this.NavTypeTable = new int[11];
		for (int i = 0; i < this.NavTypeTable.Length; i++)
		{
			this.NavTypeTable[i] = -1;
			for (int j = 0; j < this.ValidNavTypes.Length; j++)
			{
				if (this.ValidNavTypes[j] == (NavType)i)
				{
					this.NavTypeTable[i] = num++;
					break;
				}
			}
		}
		DebugUtil.DevAssert(true, "Cell packs nav type into 4 bits!", null);
		this.Cells = new PathFinder.Cell[width_in_cells * height_in_cells * this.ValidNavTypes.Length];
		this.ProberCells = new PathGrid.ProberCell[width_in_cells * height_in_cells];
		this.serialNo = 0;
		this.previousSerialNo = -1;
		this.isUpdating = false;
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x00071D0E File Offset: 0x0006FF0E
	public void OnCleanUp()
	{
		if (this.groupProber != null)
		{
			this.groupProber.ReleaseProber(this);
		}
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x00071D25 File Offset: 0x0006FF25
	public void ResetUpdate()
	{
		this.previousSerialNo = -1;
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x00071D30 File Offset: 0x0006FF30
	public void BeginUpdate(int root_cell, bool isContinuation)
	{
		this.isUpdating = true;
		this.freshlyOccupiedCells.Clear();
		if (isContinuation)
		{
			return;
		}
		if (this.applyOffset)
		{
			Grid.CellToXY(root_cell, out this.rootX, out this.rootY);
			this.rootX -= this.widthInCells / 2;
			this.rootY -= this.heightInCells / 2;
		}
		this.serialNo += 1;
		if (this.groupProber != null)
		{
			this.groupProber.SetValidSerialNos(this, this.previousSerialNo, this.serialNo);
		}
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x00071DC8 File Offset: 0x0006FFC8
	public void EndUpdate(bool isComplete)
	{
		this.isUpdating = false;
		if (this.groupProber != null)
		{
			this.groupProber.Occupy(this, this.serialNo, this.freshlyOccupiedCells);
		}
		if (!isComplete)
		{
			return;
		}
		if (this.groupProber != null)
		{
			this.groupProber.SetValidSerialNos(this, this.serialNo, this.serialNo);
		}
		this.previousSerialNo = this.serialNo;
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x00071E2C File Offset: 0x0007002C
	private bool IsValidSerialNo(short serialNo)
	{
		return serialNo == this.serialNo || (!this.isUpdating && this.previousSerialNo != -1 && serialNo == this.previousSerialNo);
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x00071E55 File Offset: 0x00070055
	public PathFinder.Cell GetCell(PathFinder.PotentialPath potential_path, out bool is_cell_in_range)
	{
		return this.GetCell(potential_path.cell, potential_path.navType, out is_cell_in_range);
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x00071E6C File Offset: 0x0007006C
	public PathFinder.Cell GetCell(int cell, NavType nav_type, out bool is_cell_in_range)
	{
		int num = this.OffsetCell(cell);
		is_cell_in_range = (-1 != num);
		if (!is_cell_in_range)
		{
			return PathGrid.InvalidCell;
		}
		PathFinder.Cell cell2 = this.Cells[num * this.ValidNavTypes.Length + this.NavTypeTable[(int)nav_type]];
		if (!this.IsValidSerialNo(cell2.queryId))
		{
			return PathGrid.InvalidCell;
		}
		return cell2;
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x00071EC8 File Offset: 0x000700C8
	public void SetCell(PathFinder.PotentialPath potential_path, ref PathFinder.Cell cell_data)
	{
		int num = this.OffsetCell(potential_path.cell);
		if (-1 == num)
		{
			return;
		}
		cell_data.queryId = this.serialNo;
		int num2 = this.NavTypeTable[(int)potential_path.navType];
		int num3 = num * this.ValidNavTypes.Length + num2;
		this.Cells[num3] = cell_data;
		if (potential_path.navType != NavType.Tube)
		{
			PathGrid.ProberCell proberCell = this.ProberCells[num];
			if (cell_data.queryId != proberCell.queryId || cell_data.cost < proberCell.cost)
			{
				proberCell.queryId = cell_data.queryId;
				proberCell.cost = cell_data.cost;
				this.ProberCells[num] = proberCell;
				this.freshlyOccupiedCells.Add(potential_path.cell);
			}
		}
	}

	// Token: 0x06001590 RID: 5520 RVA: 0x00071F8C File Offset: 0x0007018C
	public int GetCostIgnoreProberOffset(int cell, CellOffset[] offsets)
	{
		int num = -1;
		foreach (CellOffset offset in offsets)
		{
			int num2 = Grid.OffsetCell(cell, offset);
			if (Grid.IsValidCell(num2))
			{
				PathGrid.ProberCell proberCell = this.ProberCells[num2];
				if (this.IsValidSerialNo(proberCell.queryId) && (num == -1 || proberCell.cost < num))
				{
					num = proberCell.cost;
				}
			}
		}
		return num;
	}

	// Token: 0x06001591 RID: 5521 RVA: 0x00071FFC File Offset: 0x000701FC
	public int GetCost(int cell)
	{
		int num = this.OffsetCell(cell);
		if (-1 == num)
		{
			return -1;
		}
		PathGrid.ProberCell proberCell = this.ProberCells[num];
		if (!this.IsValidSerialNo(proberCell.queryId))
		{
			return -1;
		}
		return proberCell.cost;
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x0007203C File Offset: 0x0007023C
	private int OffsetCell(int cell)
	{
		if (!this.applyOffset)
		{
			return cell;
		}
		int num;
		int num2;
		Grid.CellToXY(cell, out num, out num2);
		if (num < this.rootX || num >= this.rootX + this.widthInCells || num2 < this.rootY || num2 >= this.rootY + this.heightInCells)
		{
			return -1;
		}
		int num3 = num - this.rootX;
		return (num2 - this.rootY) * this.widthInCells + num3;
	}

	// Token: 0x04000BDA RID: 3034
	private PathFinder.Cell[] Cells;

	// Token: 0x04000BDB RID: 3035
	private PathGrid.ProberCell[] ProberCells;

	// Token: 0x04000BDC RID: 3036
	private List<int> freshlyOccupiedCells = new List<int>();

	// Token: 0x04000BDD RID: 3037
	private NavType[] ValidNavTypes;

	// Token: 0x04000BDE RID: 3038
	private int[] NavTypeTable;

	// Token: 0x04000BDF RID: 3039
	private int widthInCells;

	// Token: 0x04000BE0 RID: 3040
	private int heightInCells;

	// Token: 0x04000BE1 RID: 3041
	private bool applyOffset;

	// Token: 0x04000BE2 RID: 3042
	private int rootX;

	// Token: 0x04000BE3 RID: 3043
	private int rootY;

	// Token: 0x04000BE4 RID: 3044
	private short serialNo;

	// Token: 0x04000BE5 RID: 3045
	private short previousSerialNo;

	// Token: 0x04000BE6 RID: 3046
	private bool isUpdating;

	// Token: 0x04000BE7 RID: 3047
	private IGroupProber groupProber;

	// Token: 0x04000BE8 RID: 3048
	public static readonly PathFinder.Cell InvalidCell = new PathFinder.Cell
	{
		cost = -1
	};

	// Token: 0x0200108A RID: 4234
	private struct ProberCell
	{
		// Token: 0x0400596E RID: 22894
		public int cost;

		// Token: 0x0400596F RID: 22895
		public short queryId;
	}
}
