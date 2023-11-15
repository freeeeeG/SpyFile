using System;

namespace Pathfinding
{
	// Token: 0x02000088 RID: 136
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class UniqueComponentAttribute : Attribute
	{
		// Token: 0x040003E3 RID: 995
		public string tag;
	}
}
