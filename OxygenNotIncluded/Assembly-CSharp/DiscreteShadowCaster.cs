using System;
using System.Collections.Generic;

// Token: 0x0200090B RID: 2315
public static class DiscreteShadowCaster
{
	// Token: 0x06004324 RID: 17188 RVA: 0x001774E4 File Offset: 0x001756E4
	public static void GetVisibleCells(int cell, List<int> visiblePoints, int range, LightShape shape, bool canSeeThroughTransparent = true)
	{
		visiblePoints.Add(cell);
		Vector2I cellPos = Grid.CellToXY(cell);
		if (shape == LightShape.Circle)
		{
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.N_NW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.N_NE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.E_NE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.E_SE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.S_SE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.S_SW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.W_SW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.W_NW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			return;
		}
		if (shape == LightShape.Cone)
		{
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.S_SE, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
			DiscreteShadowCaster.ScanOctant(cellPos, range, 1, DiscreteShadowCaster.Octant.S_SW, 1.0, 0.0, visiblePoints, canSeeThroughTransparent);
		}
	}

	// Token: 0x06004325 RID: 17189 RVA: 0x00177638 File Offset: 0x00175838
	private static bool DoesOcclude(int x, int y, bool canSeeThroughTransparent = false)
	{
		int num = Grid.XYToCell(x, y);
		return Grid.IsValidCell(num) && (!canSeeThroughTransparent || !Grid.Transparent[num]) && Grid.Solid[num];
	}

	// Token: 0x06004326 RID: 17190 RVA: 0x00177674 File Offset: 0x00175874
	private static void ScanOctant(Vector2I cellPos, int range, int depth, DiscreteShadowCaster.Octant octant, double startSlope, double endSlope, List<int> visiblePoints, bool canSeeThroughTransparent = true)
	{
		int num = range * range;
		int num2 = 0;
		int num3 = 0;
		switch (octant)
		{
		case DiscreteShadowCaster.Octant.N_NW:
			num3 = cellPos.y + depth;
			if (num3 >= Grid.HeightInCells)
			{
				return;
			}
			num2 = cellPos.x - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 < 0)
			{
				num2 = 0;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, false) <= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num2 - 1 >= 0 && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3 - 1, canSeeThroughTransparent))
						{
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, false), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num2 - 1 >= 0 && DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent))
						{
							startSlope = -DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, false);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num2++;
			}
			num2--;
			break;
		case DiscreteShadowCaster.Octant.N_NE:
			num3 = cellPos.y + depth;
			if (num3 >= Grid.HeightInCells)
			{
				return;
			}
			num2 = cellPos.x + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 >= Grid.WidthInCells)
			{
				num2 = Grid.WidthInCells - 1;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, false) >= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num2 + 1 < Grid.HeightInCells && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3 - 1, canSeeThroughTransparent))
						{
							double slope = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, false);
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope, visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num2 + 1 < Grid.HeightInCells && DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent))
						{
							startSlope = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, false);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num2--;
			}
			num2++;
			break;
		case DiscreteShadowCaster.Octant.E_NE:
			num2 = cellPos.x + depth;
			if (num2 >= Grid.WidthInCells)
			{
				return;
			}
			num3 = cellPos.y + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 >= Grid.HeightInCells)
			{
				num3 = Grid.HeightInCells - 1;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, true) >= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num3 + 1 < Grid.HeightInCells && !DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3 + 1, canSeeThroughTransparent))
						{
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, true), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num3 + 1 < Grid.HeightInCells && DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent))
						{
							startSlope = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, true);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num3--;
			}
			num3++;
			break;
		case DiscreteShadowCaster.Octant.E_SE:
			num2 = cellPos.x + depth;
			if (num2 >= Grid.WidthInCells)
			{
				return;
			}
			num3 = cellPos.y - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 < 0)
			{
				num3 = 0;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, true) <= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num3 - 1 >= 0 && !DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3 - 1, canSeeThroughTransparent))
						{
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, true), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num3 - 1 >= 0 && DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent))
						{
							startSlope = -DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, true);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num3++;
			}
			num3--;
			break;
		case DiscreteShadowCaster.Octant.S_SE:
			num3 = cellPos.y - depth;
			if (num3 < 0)
			{
				return;
			}
			num2 = cellPos.x + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 >= Grid.WidthInCells)
			{
				num2 = Grid.WidthInCells - 1;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, false) <= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num2 + 1 < Grid.WidthInCells && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3 + 1, canSeeThroughTransparent))
						{
							double slope2 = DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, false);
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope2, visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num2 + 1 < Grid.WidthInCells && DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent))
						{
							startSlope = -DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, false);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num2--;
			}
			num2++;
			break;
		case DiscreteShadowCaster.Octant.S_SW:
			num3 = cellPos.y - depth;
			if (num3 < 0)
			{
				return;
			}
			num2 = cellPos.x - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num2 < 0)
			{
				num2 = 0;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, false) >= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num2 - 1 >= 0 && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 - 1, num3 + 1, canSeeThroughTransparent))
						{
							double slope3 = DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, false);
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, slope3, visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num2 - 1 >= 0 && DiscreteShadowCaster.DoesOcclude(num2 - 1, num3, canSeeThroughTransparent))
						{
							startSlope = DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, false);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num2++;
			}
			num2--;
			break;
		case DiscreteShadowCaster.Octant.W_SW:
			num2 = cellPos.x - depth;
			if (num2 < 0)
			{
				return;
			}
			num3 = cellPos.y - Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 < 0)
			{
				num3 = 0;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, true) >= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num3 - 1 >= 0 && !DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3 - 1, canSeeThroughTransparent))
						{
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, true), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num3 - 1 >= 0 && DiscreteShadowCaster.DoesOcclude(num2, num3 - 1, canSeeThroughTransparent))
						{
							startSlope = DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 - 0.5, (double)cellPos.x, (double)cellPos.y, true);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num3++;
			}
			num3--;
			break;
		case DiscreteShadowCaster.Octant.W_NW:
			num2 = cellPos.x - depth;
			if (num2 < 0)
			{
				return;
			}
			num3 = cellPos.y + Convert.ToInt32(startSlope * Convert.ToDouble(depth));
			if (num3 >= Grid.HeightInCells)
			{
				num3 = Grid.HeightInCells - 1;
			}
			while (DiscreteShadowCaster.GetSlope((double)num2, (double)num3, (double)cellPos.x, (double)cellPos.y, true) <= endSlope)
			{
				if (DiscreteShadowCaster.GetVisDistance(num2, num3, cellPos.x, cellPos.y) <= num)
				{
					if (DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
					{
						if (num3 + 1 < Grid.HeightInCells && !DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent) && !DiscreteShadowCaster.DoesOcclude(num2 + 1, num3 + 1, canSeeThroughTransparent))
						{
							DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, DiscreteShadowCaster.GetSlope((double)num2 + 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, true), visiblePoints, canSeeThroughTransparent);
						}
					}
					else
					{
						if (num3 + 1 < Grid.HeightInCells && DiscreteShadowCaster.DoesOcclude(num2, num3 + 1, canSeeThroughTransparent))
						{
							startSlope = -DiscreteShadowCaster.GetSlope((double)num2 - 0.5, (double)num3 + 0.5, (double)cellPos.x, (double)cellPos.y, true);
						}
						if (!DiscreteShadowCaster.DoesOcclude(num2 + 1, num3, canSeeThroughTransparent) && !visiblePoints.Contains(Grid.XYToCell(num2, num3)))
						{
							visiblePoints.Add(Grid.XYToCell(num2, num3));
						}
					}
				}
				num3--;
			}
			num3++;
			break;
		}
		if (num2 < 0)
		{
			num2 = 0;
		}
		else if (num2 >= Grid.WidthInCells)
		{
			num2 = Grid.WidthInCells - 1;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		else if (num3 >= Grid.HeightInCells)
		{
			num3 = Grid.HeightInCells - 1;
		}
		if (depth < range & !DiscreteShadowCaster.DoesOcclude(num2, num3, canSeeThroughTransparent))
		{
			DiscreteShadowCaster.ScanOctant(cellPos, range, depth + 1, octant, startSlope, endSlope, visiblePoints, canSeeThroughTransparent);
		}
	}

	// Token: 0x06004327 RID: 17191 RVA: 0x0017822B File Offset: 0x0017642B
	private static double GetSlope(double pX1, double pY1, double pX2, double pY2, bool pInvert)
	{
		if (pInvert)
		{
			return (pY1 - pY2) / (pX1 - pX2);
		}
		return (pX1 - pX2) / (pY1 - pY2);
	}

	// Token: 0x06004328 RID: 17192 RVA: 0x00178240 File Offset: 0x00176440
	private static int GetVisDistance(int pX1, int pY1, int pX2, int pY2)
	{
		return (pX1 - pX2) * (pX1 - pX2) + (pY1 - pY2) * (pY1 - pY2);
	}

	// Token: 0x0200175C RID: 5980
	public enum Octant
	{
		// Token: 0x04006E7A RID: 28282
		N_NW,
		// Token: 0x04006E7B RID: 28283
		N_NE,
		// Token: 0x04006E7C RID: 28284
		E_NE,
		// Token: 0x04006E7D RID: 28285
		E_SE,
		// Token: 0x04006E7E RID: 28286
		S_SE,
		// Token: 0x04006E7F RID: 28287
		S_SW,
		// Token: 0x04006E80 RID: 28288
		W_SW,
		// Token: 0x04006E81 RID: 28289
		W_NW
	}
}
