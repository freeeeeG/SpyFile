using System;

// Token: 0x02000401 RID: 1025
public class CellOffsetQuery : CellArrayQuery
{
	// Token: 0x060015AB RID: 5547 RVA: 0x00072500 File Offset: 0x00070700
	public CellArrayQuery Reset(int cell, CellOffset[] offsets)
	{
		int[] array = new int[offsets.Length];
		for (int i = 0; i < offsets.Length; i++)
		{
			array[i] = Grid.OffsetCell(cell, offsets[i]);
		}
		base.Reset(array);
		return this;
	}
}
