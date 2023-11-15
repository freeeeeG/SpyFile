using System;
using Pathfinding.Util;

namespace Pathfinding
{
	// Token: 0x02000071 RID: 113
	public class NavmeshTile : INavmeshHolder, ITransformedGraph, INavmesh
	{
		// Token: 0x06000605 RID: 1541 RVA: 0x00022FE1 File Offset: 0x000211E1
		public void GetTileCoordinates(int tileIndex, out int x, out int z)
		{
			x = this.x;
			z = this.z;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00022FF3 File Offset: 0x000211F3
		public int GetVertexArrayIndex(int index)
		{
			return index & 4095;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00022FFC File Offset: 0x000211FC
		public Int3 GetVertex(int index)
		{
			int num = index & 4095;
			return this.verts[num];
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0002301D File Offset: 0x0002121D
		public Int3 GetVertexInGraphSpace(int index)
		{
			return this.vertsInGraphSpace[index & 4095];
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x00023031 File Offset: 0x00021231
		public GraphTransform transform
		{
			get
			{
				return this.graph.transform;
			}
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x00023040 File Offset: 0x00021240
		public void GetNodes(Action<GraphNode> action)
		{
			if (this.nodes == null)
			{
				return;
			}
			for (int i = 0; i < this.nodes.Length; i++)
			{
				action(this.nodes[i]);
			}
		}

		// Token: 0x04000363 RID: 867
		public int[] tris;

		// Token: 0x04000364 RID: 868
		public Int3[] verts;

		// Token: 0x04000365 RID: 869
		public Int3[] vertsInGraphSpace;

		// Token: 0x04000366 RID: 870
		public int x;

		// Token: 0x04000367 RID: 871
		public int z;

		// Token: 0x04000368 RID: 872
		public int w;

		// Token: 0x04000369 RID: 873
		public int d;

		// Token: 0x0400036A RID: 874
		public TriangleMeshNode[] nodes;

		// Token: 0x0400036B RID: 875
		public BBTree bbTree;

		// Token: 0x0400036C RID: 876
		public bool flag;

		// Token: 0x0400036D RID: 877
		public NavmeshBase graph;
	}
}
