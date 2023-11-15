using System;

namespace Pathfinding
{
	// Token: 0x0200004E RID: 78
	public interface ITraversalProvider
	{
		// Token: 0x060003B6 RID: 950
		bool CanTraverse(Path path, GraphNode node);

		// Token: 0x060003B7 RID: 951
		uint GetTraversalCost(Path path, GraphNode node);
	}
}
