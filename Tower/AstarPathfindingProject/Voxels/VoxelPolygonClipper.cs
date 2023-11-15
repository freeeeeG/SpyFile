using System;
using UnityEngine;

namespace Pathfinding.Voxels
{
	// Token: 0x020000B0 RID: 176
	internal struct VoxelPolygonClipper
	{
		// Token: 0x060007CA RID: 1994 RVA: 0x00033400 File Offset: 0x00031600
		public VoxelPolygonClipper(int capacity)
		{
			this.x = new float[capacity];
			this.y = new float[capacity];
			this.z = new float[capacity];
			this.n = 0;
		}

		// Token: 0x17000119 RID: 281
		public Vector3 this[int i]
		{
			set
			{
				this.x[i] = value.x;
				this.y[i] = value.y;
				this.z[i] = value.z;
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x0003345C File Offset: 0x0003165C
		public void ClipPolygonAlongX(ref VoxelPolygonClipper result, float multi, float offset)
		{
			int num = 0;
			float num2 = multi * this.x[this.n - 1] + offset;
			int i = 0;
			int num3 = this.n - 1;
			while (i < this.n)
			{
				float num4 = multi * this.x[i] + offset;
				bool flag = num2 >= 0f;
				bool flag2 = num4 >= 0f;
				if (flag != flag2)
				{
					float num5 = num2 / (num2 - num4);
					result.x[num] = this.x[num3] + (this.x[i] - this.x[num3]) * num5;
					result.y[num] = this.y[num3] + (this.y[i] - this.y[num3]) * num5;
					result.z[num] = this.z[num3] + (this.z[i] - this.z[num3]) * num5;
					num++;
				}
				if (flag2)
				{
					result.x[num] = this.x[i];
					result.y[num] = this.y[i];
					result.z[num] = this.z[i];
					num++;
				}
				num2 = num4;
				num3 = i;
				i++;
			}
			result.n = num;
		}

		// Token: 0x060007CD RID: 1997 RVA: 0x00033594 File Offset: 0x00031794
		public void ClipPolygonAlongZWithYZ(ref VoxelPolygonClipper result, float multi, float offset)
		{
			int num = 0;
			float num2 = multi * this.z[this.n - 1] + offset;
			int i = 0;
			int num3 = this.n - 1;
			while (i < this.n)
			{
				float num4 = multi * this.z[i] + offset;
				bool flag = num2 >= 0f;
				bool flag2 = num4 >= 0f;
				if (flag != flag2)
				{
					float num5 = num2 / (num2 - num4);
					result.y[num] = this.y[num3] + (this.y[i] - this.y[num3]) * num5;
					result.z[num] = this.z[num3] + (this.z[i] - this.z[num3]) * num5;
					num++;
				}
				if (flag2)
				{
					result.y[num] = this.y[i];
					result.z[num] = this.z[i];
					num++;
				}
				num2 = num4;
				num3 = i;
				i++;
			}
			result.n = num;
		}

		// Token: 0x060007CE RID: 1998 RVA: 0x00033690 File Offset: 0x00031890
		public void ClipPolygonAlongZWithY(ref VoxelPolygonClipper result, float multi, float offset)
		{
			int num = 0;
			float num2 = multi * this.z[this.n - 1] + offset;
			int i = 0;
			int num3 = this.n - 1;
			while (i < this.n)
			{
				float num4 = multi * this.z[i] + offset;
				bool flag = num2 >= 0f;
				bool flag2 = num4 >= 0f;
				if (flag != flag2)
				{
					float num5 = num2 / (num2 - num4);
					result.y[num] = this.y[num3] + (this.y[i] - this.y[num3]) * num5;
					num++;
				}
				if (flag2)
				{
					result.y[num] = this.y[i];
					num++;
				}
				num2 = num4;
				num3 = i;
				i++;
			}
			result.n = num;
		}

		// Token: 0x040004A0 RID: 1184
		public float[] x;

		// Token: 0x040004A1 RID: 1185
		public float[] y;

		// Token: 0x040004A2 RID: 1186
		public float[] z;

		// Token: 0x040004A3 RID: 1187
		public int n;
	}
}
