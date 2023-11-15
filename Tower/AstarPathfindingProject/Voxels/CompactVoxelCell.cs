using System;

namespace Pathfinding.Voxels
{
	// Token: 0x020000AC RID: 172
	public struct CompactVoxelCell
	{
		// Token: 0x0600078F RID: 1935 RVA: 0x0002EC19 File Offset: 0x0002CE19
		public CompactVoxelCell(uint i, uint c)
		{
			this.index = i;
			this.count = c;
		}

		// Token: 0x04000477 RID: 1143
		public uint index;

		// Token: 0x04000478 RID: 1144
		public uint count;
	}
}
