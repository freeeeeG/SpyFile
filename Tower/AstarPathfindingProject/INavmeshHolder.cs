using System;

namespace Pathfinding
{
	// Token: 0x0200006A RID: 106
	public interface INavmeshHolder : ITransformedGraph, INavmesh
	{
		// Token: 0x06000587 RID: 1415
		Int3 GetVertex(int i);

		// Token: 0x06000588 RID: 1416
		Int3 GetVertexInGraphSpace(int i);

		// Token: 0x06000589 RID: 1417
		int GetVertexArrayIndex(int index);

		// Token: 0x0600058A RID: 1418
		void GetTileCoordinates(int tileIndex, out int x, out int z);
	}
}
