using System;

namespace Pathfinding.Voxels
{
	// Token: 0x020000AE RID: 174
	public class VoxelSpan
	{
		// Token: 0x06000793 RID: 1939 RVA: 0x0002EC8E File Offset: 0x0002CE8E
		public VoxelSpan(uint b, uint t, int area)
		{
			this.bottom = b;
			this.top = t;
			this.area = area;
		}

		// Token: 0x0400047D RID: 1149
		public uint bottom;

		// Token: 0x0400047E RID: 1150
		public uint top;

		// Token: 0x0400047F RID: 1151
		public VoxelSpan next;

		// Token: 0x04000480 RID: 1152
		public int area;
	}
}
