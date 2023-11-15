using System;

namespace Pathfinding.Voxels
{
	// Token: 0x020000B1 RID: 177
	internal struct Int3PolygonClipper
	{
		// Token: 0x060007CF RID: 1999 RVA: 0x00033751 File Offset: 0x00031951
		public void Init()
		{
			if (this.clipPolygonCache == null)
			{
				this.clipPolygonCache = new float[21];
				this.clipPolygonIntCache = new int[21];
			}
		}

		// Token: 0x060007D0 RID: 2000 RVA: 0x00033778 File Offset: 0x00031978
		public int ClipPolygon(Int3[] vIn, int n, Int3[] vOut, int multi, int offset, int axis)
		{
			this.Init();
			int[] array = this.clipPolygonIntCache;
			for (int i = 0; i < n; i++)
			{
				array[i] = multi * vIn[i][axis] + offset;
			}
			int num = 0;
			int j = 0;
			int num2 = n - 1;
			while (j < n)
			{
				bool flag = array[num2] >= 0;
				bool flag2 = array[j] >= 0;
				if (flag != flag2)
				{
					double rhs = (double)array[num2] / (double)(array[num2] - array[j]);
					vOut[num] = vIn[num2] + (vIn[j] - vIn[num2]) * rhs;
					num++;
				}
				if (flag2)
				{
					vOut[num] = vIn[j];
					num++;
				}
				num2 = j;
				j++;
			}
			return num;
		}

		// Token: 0x040004A4 RID: 1188
		private float[] clipPolygonCache;

		// Token: 0x040004A5 RID: 1189
		private int[] clipPolygonIntCache;
	}
}
