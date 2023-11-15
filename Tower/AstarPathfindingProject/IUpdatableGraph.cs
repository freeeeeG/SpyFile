using System;

namespace Pathfinding
{
	// Token: 0x02000017 RID: 23
	public interface IUpdatableGraph
	{
		// Token: 0x060001A7 RID: 423
		void UpdateArea(GraphUpdateObject o);

		// Token: 0x060001A8 RID: 424
		void UpdateAreaInit(GraphUpdateObject o);

		// Token: 0x060001A9 RID: 425
		void UpdateAreaPost(GraphUpdateObject o);

		// Token: 0x060001AA RID: 426
		GraphUpdateThreading CanUpdateAsync(GraphUpdateObject o);
	}
}
