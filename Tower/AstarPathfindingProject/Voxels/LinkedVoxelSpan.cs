using System;

namespace Pathfinding.Voxels
{
	// Token: 0x020000A6 RID: 166
	public struct LinkedVoxelSpan
	{
		// Token: 0x06000786 RID: 1926 RVA: 0x0002E99C File Offset: 0x0002CB9C
		public LinkedVoxelSpan(uint bottom, uint top, int area)
		{
			this.bottom = bottom;
			this.top = top;
			this.area = area;
			this.next = -1;
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x0002E9BA File Offset: 0x0002CBBA
		public LinkedVoxelSpan(uint bottom, uint top, int area, int next)
		{
			this.bottom = bottom;
			this.top = top;
			this.area = area;
			this.next = next;
		}

		// Token: 0x0400045F RID: 1119
		public uint bottom;

		// Token: 0x04000460 RID: 1120
		public uint top;

		// Token: 0x04000461 RID: 1121
		public int next;

		// Token: 0x04000462 RID: 1122
		public int area;
	}
}
