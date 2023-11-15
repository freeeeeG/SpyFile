using System;
using System.Collections.Generic;

namespace Rendering.World
{
	// Token: 0x02000CBD RID: 3261
	public abstract class TileRenderer : KMonoBehaviour
	{
		// Token: 0x0600684A RID: 26698 RVA: 0x00276FB8 File Offset: 0x002751B8
		protected override void OnSpawn()
		{
			this.Masks = this.GetMasks();
			this.TileGridWidth = Grid.WidthInCells + 1;
			this.TileGridHeight = Grid.HeightInCells + 1;
			this.BrushGrid = new int[this.TileGridWidth * this.TileGridHeight * 4];
			for (int i = 0; i < this.BrushGrid.Length; i++)
			{
				this.BrushGrid[i] = -1;
			}
			this.TileGrid = new Tile[this.TileGridWidth * this.TileGridHeight];
			for (int j = 0; j < this.TileGrid.Length; j++)
			{
				int tile_x = j % this.TileGridWidth;
				int tile_y = j / this.TileGridWidth;
				this.TileGrid[j] = new Tile(j, tile_x, tile_y, this.Masks.Length);
			}
			this.LoadBrushes();
			this.VisibleAreaUpdater = new VisibleAreaUpdater(new Action<int>(this.UpdateOutsideView), new Action<int>(this.UpdateInsideView), "TileRenderer");
		}

		// Token: 0x0600684B RID: 26699 RVA: 0x002770A8 File Offset: 0x002752A8
		protected virtual Mask[] GetMasks()
		{
			return new Mask[]
			{
				new Mask(this.Atlas, 0, false, false, false, false),
				new Mask(this.Atlas, 2, false, false, true, false),
				new Mask(this.Atlas, 2, false, true, true, false),
				new Mask(this.Atlas, 1, false, false, true, false),
				new Mask(this.Atlas, 2, false, false, false, false),
				new Mask(this.Atlas, 1, true, false, false, false),
				new Mask(this.Atlas, 3, false, false, false, false),
				new Mask(this.Atlas, 4, false, false, true, false),
				new Mask(this.Atlas, 2, false, true, false, false),
				new Mask(this.Atlas, 3, true, false, false, false),
				new Mask(this.Atlas, 1, true, false, true, false),
				new Mask(this.Atlas, 4, false, true, true, false),
				new Mask(this.Atlas, 1, false, false, false, false),
				new Mask(this.Atlas, 4, false, false, false, false),
				new Mask(this.Atlas, 4, false, true, false, false),
				new Mask(this.Atlas, 0, false, false, false, true)
			};
		}

		// Token: 0x0600684C RID: 26700 RVA: 0x00277234 File Offset: 0x00275434
		private void UpdateInsideView(int cell)
		{
			foreach (int item in this.GetCellTiles(cell))
			{
				this.ClearTiles.Add(item);
				this.DirtyTiles.Add(item);
			}
		}

		// Token: 0x0600684D RID: 26701 RVA: 0x00277278 File Offset: 0x00275478
		private void UpdateOutsideView(int cell)
		{
			foreach (int item in this.GetCellTiles(cell))
			{
				this.ClearTiles.Add(item);
			}
		}

		// Token: 0x0600684E RID: 26702 RVA: 0x002772AC File Offset: 0x002754AC
		private int[] GetCellTiles(int cell)
		{
			int num = 0;
			int num2 = 0;
			Grid.CellToXY(cell, out num, out num2);
			this.CellTiles[0] = num2 * this.TileGridWidth + num;
			this.CellTiles[1] = num2 * this.TileGridWidth + (num + 1);
			this.CellTiles[2] = (num2 + 1) * this.TileGridWidth + num;
			this.CellTiles[3] = (num2 + 1) * this.TileGridWidth + (num + 1);
			return this.CellTiles;
		}

		// Token: 0x0600684F RID: 26703
		public abstract void LoadBrushes();

		// Token: 0x06006850 RID: 26704 RVA: 0x0027731D File Offset: 0x0027551D
		public void MarkDirty(int cell)
		{
			this.VisibleAreaUpdater.UpdateCell(cell);
		}

		// Token: 0x06006851 RID: 26705 RVA: 0x0027732C File Offset: 0x0027552C
		private void LateUpdate()
		{
			foreach (int num in this.ClearTiles)
			{
				this.Clear(ref this.TileGrid[num], this.Brushes, this.BrushGrid);
			}
			this.ClearTiles.Clear();
			foreach (int num2 in this.DirtyTiles)
			{
				this.MarkDirty(ref this.TileGrid[num2], this.Brushes, this.BrushGrid);
			}
			this.DirtyTiles.Clear();
			this.VisibleAreaUpdater.Update();
			foreach (Brush brush in this.DirtyBrushes)
			{
				brush.Refresh();
			}
			this.DirtyBrushes.Clear();
			foreach (Brush brush2 in this.ActiveBrushes)
			{
				brush2.Render();
			}
		}

		// Token: 0x06006852 RID: 26706
		public abstract void MarkDirty(ref Tile tile, Brush[] brush_array, int[] brush_grid);

		// Token: 0x06006853 RID: 26707 RVA: 0x0027749C File Offset: 0x0027569C
		public void Clear(ref Tile tile, Brush[] brush_array, int[] brush_grid)
		{
			for (int i = 0; i < 4; i++)
			{
				int num = tile.Idx * 4 + i;
				if (brush_grid[num] != -1)
				{
					brush_array[brush_grid[num]].Remove(tile.Idx);
				}
			}
		}

		// Token: 0x0400480B RID: 18443
		private Tile[] TileGrid;

		// Token: 0x0400480C RID: 18444
		private int[] BrushGrid;

		// Token: 0x0400480D RID: 18445
		protected int TileGridWidth;

		// Token: 0x0400480E RID: 18446
		protected int TileGridHeight;

		// Token: 0x0400480F RID: 18447
		private int[] CellTiles = new int[4];

		// Token: 0x04004810 RID: 18448
		protected Brush[] Brushes;

		// Token: 0x04004811 RID: 18449
		protected Mask[] Masks;

		// Token: 0x04004812 RID: 18450
		protected List<Brush> DirtyBrushes = new List<Brush>();

		// Token: 0x04004813 RID: 18451
		protected List<Brush> ActiveBrushes = new List<Brush>();

		// Token: 0x04004814 RID: 18452
		private VisibleAreaUpdater VisibleAreaUpdater;

		// Token: 0x04004815 RID: 18453
		private HashSet<int> ClearTiles = new HashSet<int>();

		// Token: 0x04004816 RID: 18454
		private HashSet<int> DirtyTiles = new HashSet<int>();

		// Token: 0x04004817 RID: 18455
		public TextureAtlas Atlas;
	}
}
