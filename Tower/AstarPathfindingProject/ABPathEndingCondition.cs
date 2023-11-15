using System;

namespace Pathfinding
{
	// Token: 0x02000096 RID: 150
	public class ABPathEndingCondition : PathEndingCondition
	{
		// Token: 0x0600071B RID: 1819 RVA: 0x0002AE39 File Offset: 0x00029039
		public ABPathEndingCondition(ABPath p)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			this.abPath = p;
			this.path = p;
		}

		// Token: 0x0600071C RID: 1820 RVA: 0x0002AE5D File Offset: 0x0002905D
		public override bool TargetFound(PathNode node)
		{
			return node.node == this.abPath.endNode;
		}

		// Token: 0x04000416 RID: 1046
		protected ABPath abPath;
	}
}
