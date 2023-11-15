using System;

namespace Pathfinding.Voxels
{
	// Token: 0x020000AD RID: 173
	public struct CompactVoxelSpan
	{
		// Token: 0x06000790 RID: 1936 RVA: 0x0002EC29 File Offset: 0x0002CE29
		public CompactVoxelSpan(ushort bottom, uint height)
		{
			this.con = 24U;
			this.y = bottom;
			this.h = height;
			this.reg = 0;
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x0002EC48 File Offset: 0x0002CE48
		public void SetConnection(int dir, uint value)
		{
			int num = dir * 6;
			this.con = (uint)(((ulong)this.con & (ulong)(~(63L << (num & 31)))) | (ulong)((ulong)(value & 63U) << num));
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x0002EC7C File Offset: 0x0002CE7C
		public int GetConnection(int dir)
		{
			return (int)this.con >> dir * 6 & 63;
		}

		// Token: 0x04000479 RID: 1145
		public ushort y;

		// Token: 0x0400047A RID: 1146
		public uint con;

		// Token: 0x0400047B RID: 1147
		public uint h;

		// Token: 0x0400047C RID: 1148
		public int reg;
	}
}
