using System;
using UnityEngine;

// Token: 0x020003F5 RID: 1013
public static class NavTypeHelper
{
	// Token: 0x06001567 RID: 5479 RVA: 0x000711B0 File Offset: 0x0006F3B0
	public static Vector3 GetNavPos(int cell, NavType nav_type)
	{
		Vector3 zero = Vector3.zero;
		switch (nav_type)
		{
		case NavType.Floor:
			return Grid.CellToPosCBC(cell, Grid.SceneLayer.Move);
		case NavType.LeftWall:
			return Grid.CellToPosLCC(cell, Grid.SceneLayer.Move);
		case NavType.RightWall:
			return Grid.CellToPosRCC(cell, Grid.SceneLayer.Move);
		case NavType.Ceiling:
			return Grid.CellToPosCTC(cell, Grid.SceneLayer.Move);
		case NavType.Ladder:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		case NavType.Pole:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		case NavType.Tube:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		case NavType.Solid:
			return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
		}
		return Grid.CellToPosCCC(cell, Grid.SceneLayer.Move);
	}

	// Token: 0x06001568 RID: 5480 RVA: 0x00071258 File Offset: 0x0006F458
	public static int GetAnchorCell(NavType nav_type, int cell)
	{
		int result = Grid.InvalidCell;
		if (Grid.IsValidCell(cell))
		{
			switch (nav_type)
			{
			case NavType.Floor:
				result = Grid.CellBelow(cell);
				break;
			case NavType.LeftWall:
				result = Grid.CellLeft(cell);
				break;
			case NavType.RightWall:
				result = Grid.CellRight(cell);
				break;
			case NavType.Ceiling:
				result = Grid.CellAbove(cell);
				break;
			default:
				if (nav_type == NavType.Solid)
				{
					result = cell;
				}
				break;
			}
		}
		return result;
	}
}
