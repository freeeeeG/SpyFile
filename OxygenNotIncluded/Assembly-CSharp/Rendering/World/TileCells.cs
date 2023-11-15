using System;

namespace Rendering.World
{
	// Token: 0x02000CBB RID: 3259
	public struct TileCells
	{
		// Token: 0x06006848 RID: 26696 RVA: 0x00276EF8 File Offset: 0x002750F8
		public TileCells(int tile_x, int tile_y)
		{
			int val = Grid.WidthInCells - 1;
			int val2 = Grid.HeightInCells - 1;
			this.Cell0 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val), Math.Min(Math.Max(tile_y - 1, 0), val2));
			this.Cell1 = Grid.XYToCell(Math.Min(tile_x, val), Math.Min(Math.Max(tile_y - 1, 0), val2));
			this.Cell2 = Grid.XYToCell(Math.Min(Math.Max(tile_x - 1, 0), val), Math.Min(tile_y, val2));
			this.Cell3 = Grid.XYToCell(Math.Min(tile_x, val), Math.Min(tile_y, val2));
		}

		// Token: 0x04004804 RID: 18436
		public int Cell0;

		// Token: 0x04004805 RID: 18437
		public int Cell1;

		// Token: 0x04004806 RID: 18438
		public int Cell2;

		// Token: 0x04004807 RID: 18439
		public int Cell3;
	}
}
