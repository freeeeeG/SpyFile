using System;

namespace Pathfinding
{
	// Token: 0x02000065 RID: 101
	public interface INavmesh
	{
		// Token: 0x06000534 RID: 1332
		void GetNodes(Action<GraphNode> del);
	}
}
