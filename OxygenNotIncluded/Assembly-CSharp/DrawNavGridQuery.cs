using System;
using UnityEngine;

// Token: 0x02000403 RID: 1027
public class DrawNavGridQuery : PathFinderQuery
{
	// Token: 0x060015B0 RID: 5552 RVA: 0x00072562 File Offset: 0x00070762
	public DrawNavGridQuery Reset(MinionBrain brain)
	{
		return this;
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x00072568 File Offset: 0x00070768
	public override bool IsMatch(int cell, int parent_cell, int cost)
	{
		if (parent_cell == Grid.InvalidCell || (int)Grid.WorldIdx[parent_cell] != ClusterManager.Instance.activeWorldId || (int)Grid.WorldIdx[cell] != ClusterManager.Instance.activeWorldId)
		{
			return false;
		}
		GL.Color(Color.white);
		GL.Vertex(Grid.CellToPosCCC(parent_cell, Grid.SceneLayer.Move));
		GL.Vertex(Grid.CellToPosCCC(cell, Grid.SceneLayer.Move));
		return false;
	}
}
