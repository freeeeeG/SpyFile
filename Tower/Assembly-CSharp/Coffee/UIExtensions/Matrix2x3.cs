using System;
using UnityEngine;

namespace Coffee.UIExtensions
{
	// Token: 0x020000EC RID: 236
	internal struct Matrix2x3
	{
		// Token: 0x06000371 RID: 881 RVA: 0x0000F630 File Offset: 0x0000D830
		public Matrix2x3(Rect rect, float cos, float sin)
		{
			float num = -rect.xMin / rect.width - 0.5f;
			float num2 = -rect.yMin / rect.height - 0.5f;
			this.m00 = cos / rect.width;
			this.m01 = -sin / rect.height;
			this.m02 = num * cos - num2 * sin + 0.5f;
			this.m10 = sin / rect.width;
			this.m11 = cos / rect.height;
			this.m12 = num * sin + num2 * cos + 0.5f;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x0000F6D0 File Offset: 0x0000D8D0
		public static Vector2 operator *(Matrix2x3 m, Vector2 v)
		{
			return new Vector2(m.m00 * v.x + m.m01 * v.y + m.m02, m.m10 * v.x + m.m11 * v.y + m.m12);
		}

		// Token: 0x0400034B RID: 843
		public float m00;

		// Token: 0x0400034C RID: 844
		public float m01;

		// Token: 0x0400034D RID: 845
		public float m02;

		// Token: 0x0400034E RID: 846
		public float m10;

		// Token: 0x0400034F RID: 847
		public float m11;

		// Token: 0x04000350 RID: 848
		public float m12;
	}
}
