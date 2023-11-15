using System;

// Token: 0x02000841 RID: 2113
public class LiquidFetchMask
{
	// Token: 0x06003D78 RID: 15736 RVA: 0x00155110 File Offset: 0x00153310
	public LiquidFetchMask(CellOffset[][] offset_table)
	{
		for (int i = 0; i < offset_table.Length; i++)
		{
			for (int j = 0; j < offset_table[i].Length; j++)
			{
				this.maxOffset.x = Math.Max(this.maxOffset.x, Math.Abs(offset_table[i][j].x));
				this.maxOffset.y = Math.Max(this.maxOffset.y, Math.Abs(offset_table[i][j].y));
			}
		}
		this.isLiquidAvailable = new bool[Grid.CellCount];
		for (int k = 0; k < Grid.CellCount; k++)
		{
			this.RefreshCell(k);
		}
	}

	// Token: 0x06003D79 RID: 15737 RVA: 0x001551C4 File Offset: 0x001533C4
	private void RefreshCell(int cell)
	{
		CellOffset offset = Grid.GetOffset(cell);
		int num = Math.Max(0, offset.y - this.maxOffset.y);
		while (num < Grid.HeightInCells && num < offset.y + this.maxOffset.y)
		{
			int num2 = Math.Max(0, offset.x - this.maxOffset.x);
			while (num2 < Grid.WidthInCells && num2 < offset.x + this.maxOffset.x)
			{
				if (Grid.Element[Grid.XYToCell(num2, num)].IsLiquid)
				{
					this.isLiquidAvailable[cell] = true;
					return;
				}
				num2++;
			}
			num++;
		}
		this.isLiquidAvailable[cell] = false;
	}

	// Token: 0x06003D7A RID: 15738 RVA: 0x00155277 File Offset: 0x00153477
	public void MarkDirty(int cell)
	{
		this.RefreshCell(cell);
	}

	// Token: 0x06003D7B RID: 15739 RVA: 0x00155280 File Offset: 0x00153480
	public bool IsLiquidAvailable(int cell)
	{
		return this.isLiquidAvailable[cell];
	}

	// Token: 0x06003D7C RID: 15740 RVA: 0x0015528A File Offset: 0x0015348A
	public void Destroy()
	{
		this.isLiquidAvailable = null;
	}

	// Token: 0x04002819 RID: 10265
	private bool[] isLiquidAvailable;

	// Token: 0x0400281A RID: 10266
	private CellOffset maxOffset;
}
