using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200040D RID: 1037
public class StaterpillarCellQuery : PathFinderQuery
{
	// Token: 0x060015D6 RID: 5590 RVA: 0x000731EC File Offset: 0x000713EC
	public StaterpillarCellQuery Reset(int max_results, GameObject tester, ObjectLayer conduitLayer)
	{
		this.max_results = max_results;
		this.tester = tester;
		this.result_cells.Clear();
		ObjectLayer objectLayer;
		if (conduitLayer <= ObjectLayer.LiquidConduit)
		{
			if (conduitLayer == ObjectLayer.GasConduit)
			{
				objectLayer = ObjectLayer.GasConduitConnection;
				goto IL_4A;
			}
			if (conduitLayer == ObjectLayer.LiquidConduit)
			{
				objectLayer = ObjectLayer.LiquidConduitConnection;
				goto IL_4A;
			}
		}
		else
		{
			if (conduitLayer == ObjectLayer.SolidConduit)
			{
				objectLayer = ObjectLayer.SolidConduitConnection;
				goto IL_4A;
			}
			if (conduitLayer == ObjectLayer.Wire)
			{
				objectLayer = ObjectLayer.WireConnectors;
				goto IL_4A;
			}
		}
		objectLayer = conduitLayer;
		IL_4A:
		this.connectorLayer = objectLayer;
		return this;
	}

	// Token: 0x060015D7 RID: 5591 RVA: 0x0007324B File Offset: 0x0007144B
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (!this.result_cells.Contains(cell) && this.CheckValidRoofCell(cell))
		{
			this.result_cells.Add(cell);
		}
		return this.result_cells.Count >= this.max_results;
	}

	// Token: 0x060015D8 RID: 5592 RVA: 0x00073288 File Offset: 0x00071488
	private bool CheckValidRoofCell(int testCell)
	{
		if (!this.tester.GetComponent<Navigator>().NavGrid.NavTable.IsValid(testCell, NavType.Ceiling))
		{
			return false;
		}
		int cellInDirection = Grid.GetCellInDirection(testCell, Direction.Down);
		return !Grid.ObjectLayers[1].ContainsKey(testCell) && !Grid.ObjectLayers[1].ContainsKey(cellInDirection) && !Grid.Objects[cellInDirection, (int)this.connectorLayer] && Grid.IsValidBuildingCell(testCell) && Grid.IsValidCell(cellInDirection) && Grid.IsValidBuildingCell(cellInDirection) && !Grid.IsSolidCell(cellInDirection);
	}

	// Token: 0x04000C2D RID: 3117
	public List<int> result_cells = new List<int>();

	// Token: 0x04000C2E RID: 3118
	private int max_results;

	// Token: 0x04000C2F RID: 3119
	private GameObject tester;

	// Token: 0x04000C30 RID: 3120
	private ObjectLayer connectorLayer;
}
