using System;

namespace Pathfinding
{
	// Token: 0x02000049 RID: 73
	public interface IWorkItemContext
	{
		// Token: 0x06000368 RID: 872
		[Obsolete("Avoid using. This will force a full recalculation of the connected components. In most cases the HierarchicalGraph class takes care of things automatically behind the scenes now. In pretty much all cases you should be able to remove the call to this function.")]
		void QueueFloodFill();

		// Token: 0x06000369 RID: 873
		void EnsureValidFloodFill();

		// Token: 0x0600036A RID: 874
		void SetGraphDirty(NavGraph graph);
	}
}
