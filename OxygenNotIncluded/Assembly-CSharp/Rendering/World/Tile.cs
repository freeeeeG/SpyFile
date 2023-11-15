using System;

namespace Rendering.World
{
	// Token: 0x02000CBC RID: 3260
	public struct Tile
	{
		// Token: 0x06006849 RID: 26697 RVA: 0x00276F99 File Offset: 0x00275199
		public Tile(int idx, int tile_x, int tile_y, int mask_count)
		{
			this.Idx = idx;
			this.TileCells = new TileCells(tile_x, tile_y);
			this.MaskCount = mask_count;
		}

		// Token: 0x04004808 RID: 18440
		public int Idx;

		// Token: 0x04004809 RID: 18441
		public TileCells TileCells;

		// Token: 0x0400480A RID: 18442
		public int MaskCount;
	}
}
