using System;

namespace Pathfinding
{
	// Token: 0x02000095 RID: 149
	public abstract class PathEndingCondition
	{
		// Token: 0x06000718 RID: 1816 RVA: 0x0002AE14 File Offset: 0x00029014
		protected PathEndingCondition()
		{
		}

		// Token: 0x06000719 RID: 1817 RVA: 0x0002AE1C File Offset: 0x0002901C
		public PathEndingCondition(Path p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			this.path = p;
		}

		// Token: 0x0600071A RID: 1818
		public abstract bool TargetFound(PathNode node);

		// Token: 0x04000415 RID: 1045
		protected Path path;
	}
}
