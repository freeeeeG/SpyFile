using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000914 RID: 2324
public static class LightGridManager
{
	// Token: 0x06004370 RID: 17264 RVA: 0x00179449 File Offset: 0x00177649
	private static int CalculateFalloff(float falloffRate, int cell, int origin)
	{
		return Mathf.Max(1, Mathf.RoundToInt(falloffRate * (float)Mathf.Max(Grid.GetCellDistance(origin, cell), 1)));
	}

	// Token: 0x06004371 RID: 17265 RVA: 0x00179466 File Offset: 0x00177666
	public static void Initialise()
	{
		LightGridManager.previewLux = new int[Grid.CellCount];
	}

	// Token: 0x06004372 RID: 17266 RVA: 0x00179477 File Offset: 0x00177677
	public static void Shutdown()
	{
		LightGridManager.previewLux = null;
		LightGridManager.previewLightCells.Clear();
	}

	// Token: 0x06004373 RID: 17267 RVA: 0x0017948C File Offset: 0x0017768C
	public static void DestroyPreview()
	{
		foreach (global::Tuple<int, int> tuple in LightGridManager.previewLightCells)
		{
			LightGridManager.previewLux[tuple.first] = 0;
		}
		LightGridManager.previewLightCells.Clear();
	}

	// Token: 0x06004374 RID: 17268 RVA: 0x001794F0 File Offset: 0x001776F0
	public static void CreatePreview(int origin_cell, float radius, global::LightShape shape, int lux)
	{
		LightGridManager.previewLightCells.Clear();
		ListPool<int, LightGridManager.LightGridEmitter>.PooledList pooledList = ListPool<int, LightGridManager.LightGridEmitter>.Allocate();
		pooledList.Add(origin_cell);
		DiscreteShadowCaster.GetVisibleCells(origin_cell, pooledList, (int)radius, shape, true);
		foreach (int num in pooledList)
		{
			if (Grid.IsValidCell(num))
			{
				int num2 = lux / LightGridManager.CalculateFalloff(0.5f, num, origin_cell);
				LightGridManager.previewLightCells.Add(new global::Tuple<int, int>(num, num2));
				LightGridManager.previewLux[num] = num2;
			}
		}
		pooledList.Recycle();
	}

	// Token: 0x04002BF4 RID: 11252
	public const float DEFAULT_FALLOFF_RATE = 0.5f;

	// Token: 0x04002BF5 RID: 11253
	public static List<global::Tuple<int, int>> previewLightCells = new List<global::Tuple<int, int>>();

	// Token: 0x04002BF6 RID: 11254
	public static int[] previewLux;

	// Token: 0x02001761 RID: 5985
	public class LightGridEmitter
	{
		// Token: 0x06008E31 RID: 36401 RVA: 0x0031ECAA File Offset: 0x0031CEAA
		public void UpdateLitCells()
		{
			DiscreteShadowCaster.GetVisibleCells(this.state.origin, this.litCells, (int)this.state.radius, this.state.shape, true);
		}

		// Token: 0x06008E32 RID: 36402 RVA: 0x0031ECDC File Offset: 0x0031CEDC
		public void AddToGrid(bool update_lit_cells)
		{
			DebugUtil.DevAssert(!update_lit_cells || this.litCells.Count == 0, "adding an already added emitter", null);
			if (update_lit_cells)
			{
				this.UpdateLitCells();
			}
			foreach (int num in this.litCells)
			{
				if (Grid.IsValidCell(num))
				{
					int num2 = Mathf.Max(0, Grid.LightCount[num] + this.ComputeLux(num));
					Grid.LightCount[num] = num2;
					LightGridManager.previewLux[num] = num2;
				}
			}
		}

		// Token: 0x06008E33 RID: 36403 RVA: 0x0031ED80 File Offset: 0x0031CF80
		public void RemoveFromGrid()
		{
			foreach (int num in this.litCells)
			{
				if (Grid.IsValidCell(num))
				{
					Grid.LightCount[num] = Mathf.Max(0, Grid.LightCount[num] - this.ComputeLux(num));
					LightGridManager.previewLux[num] = 0;
				}
			}
			this.litCells.Clear();
		}

		// Token: 0x06008E34 RID: 36404 RVA: 0x0031EE04 File Offset: 0x0031D004
		public bool Refresh(LightGridManager.LightGridEmitter.State state, bool force = false)
		{
			if (!force && EqualityComparer<LightGridManager.LightGridEmitter.State>.Default.Equals(this.state, state))
			{
				return false;
			}
			this.RemoveFromGrid();
			this.state = state;
			this.AddToGrid(true);
			return true;
		}

		// Token: 0x06008E35 RID: 36405 RVA: 0x0031EE33 File Offset: 0x0031D033
		private int ComputeLux(int cell)
		{
			return this.state.intensity / this.ComputeFalloff(cell);
		}

		// Token: 0x06008E36 RID: 36406 RVA: 0x0031EE48 File Offset: 0x0031D048
		private int ComputeFalloff(int cell)
		{
			return LightGridManager.CalculateFalloff(this.state.falloffRate, this.state.origin, cell);
		}

		// Token: 0x04006E8A RID: 28298
		private LightGridManager.LightGridEmitter.State state = LightGridManager.LightGridEmitter.State.DEFAULT;

		// Token: 0x04006E8B RID: 28299
		private List<int> litCells = new List<int>();

		// Token: 0x020021D5 RID: 8661
		[Serializable]
		public struct State : IEquatable<LightGridManager.LightGridEmitter.State>
		{
			// Token: 0x0600ABDC RID: 43996 RVA: 0x0037623C File Offset: 0x0037443C
			public bool Equals(LightGridManager.LightGridEmitter.State rhs)
			{
				return this.origin == rhs.origin && this.shape == rhs.shape && this.radius == rhs.radius && this.intensity == rhs.intensity && this.falloffRate == rhs.falloffRate && this.colour == rhs.colour;
			}

			// Token: 0x0400978B RID: 38795
			public int origin;

			// Token: 0x0400978C RID: 38796
			public global::LightShape shape;

			// Token: 0x0400978D RID: 38797
			public float radius;

			// Token: 0x0400978E RID: 38798
			public int intensity;

			// Token: 0x0400978F RID: 38799
			public float falloffRate;

			// Token: 0x04009790 RID: 38800
			public Color colour;

			// Token: 0x04009791 RID: 38801
			public static readonly LightGridManager.LightGridEmitter.State DEFAULT = new LightGridManager.LightGridEmitter.State
			{
				origin = Grid.InvalidCell,
				shape = global::LightShape.Circle,
				radius = 4f,
				intensity = 1,
				falloffRate = 0.5f,
				colour = Color.white
			};
		}
	}
}
