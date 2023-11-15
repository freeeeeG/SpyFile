using System;
using System.Collections.Generic;
using Pathfinding.Util;

namespace Pathfinding
{
	// Token: 0x0200009D RID: 157
	public static class GraphUpdateUtilities
	{
		// Token: 0x0600074D RID: 1869 RVA: 0x0002C74C File Offset: 0x0002A94C
		public static bool UpdateGraphsNoBlock(GraphUpdateObject guo, GraphNode node1, GraphNode node2, bool alwaysRevert = false)
		{
			List<GraphNode> list = ListPool<GraphNode>.Claim();
			list.Add(node1);
			list.Add(node2);
			bool result = GraphUpdateUtilities.UpdateGraphsNoBlock(guo, list, alwaysRevert);
			ListPool<GraphNode>.Release(ref list);
			return result;
		}

		// Token: 0x0600074E RID: 1870 RVA: 0x0002C77C File Offset: 0x0002A97C
		public static bool UpdateGraphsNoBlock(GraphUpdateObject guo, List<GraphNode> nodes, bool alwaysRevert = false)
		{
			PathProcessor.GraphUpdateLock graphUpdateLock = AstarPath.active.PausePathfinding();
			bool flag;
			try
			{
				AstarPath.active.FlushGraphUpdates();
				for (int i = 0; i < nodes.Count; i++)
				{
					if (!nodes[i].Walkable)
					{
						return false;
					}
				}
				guo.trackChangedNodes = true;
				AstarPath.active.UpdateGraphs(guo);
				AstarPath.active.FlushGraphUpdates();
				flag = PathUtilities.IsPathPossible(nodes);
				if (!flag || alwaysRevert)
				{
					guo.RevertFromBackup();
					AstarPath.active.hierarchicalGraph.RecalculateIfNecessary();
				}
			}
			finally
			{
				graphUpdateLock.Release();
			}
			guo.trackChangedNodes = false;
			return flag;
		}
	}
}
