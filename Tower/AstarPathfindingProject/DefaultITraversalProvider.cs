using System;

namespace Pathfinding
{
	// Token: 0x0200004F RID: 79
	public static class DefaultITraversalProvider
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x00013D70 File Offset: 0x00011F70
		public static bool CanTraverse(Path path, GraphNode node)
		{
			return node.Walkable && (path.enabledTags >> (int)node.Tag & 1) != 0;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00013D91 File Offset: 0x00011F91
		public static uint GetTraversalCost(Path path, GraphNode node)
		{
			return path.GetTagPenalty((int)node.Tag) + node.Penalty;
		}
	}
}
