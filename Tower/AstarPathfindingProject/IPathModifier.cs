using System;

namespace Pathfinding
{
	// Token: 0x0200007A RID: 122
	public interface IPathModifier
	{
		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000646 RID: 1606
		int Order { get; }

		// Token: 0x06000647 RID: 1607
		void Apply(Path path);

		// Token: 0x06000648 RID: 1608
		void PreProcess(Path path);
	}
}
