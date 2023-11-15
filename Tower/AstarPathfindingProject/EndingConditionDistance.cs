using System;

namespace Pathfinding
{
	// Token: 0x0200008D RID: 141
	public class EndingConditionDistance : PathEndingCondition
	{
		// Token: 0x060006D7 RID: 1751 RVA: 0x000292BD File Offset: 0x000274BD
		public EndingConditionDistance(Path p, int maxGScore) : base(p)
		{
			this.maxGScore = maxGScore;
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x000292D5 File Offset: 0x000274D5
		public override bool TargetFound(PathNode node)
		{
			return (ulong)node.G >= (ulong)((long)this.maxGScore);
		}

		// Token: 0x040003F6 RID: 1014
		public int maxGScore = 100;
	}
}
