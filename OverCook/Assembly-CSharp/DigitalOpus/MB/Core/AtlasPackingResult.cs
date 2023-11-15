using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000029 RID: 41
	public class AtlasPackingResult
	{
		// Token: 0x04000083 RID: 131
		public int atlasX;

		// Token: 0x04000084 RID: 132
		public int atlasY;

		// Token: 0x04000085 RID: 133
		public int usedW;

		// Token: 0x04000086 RID: 134
		public int usedH;

		// Token: 0x04000087 RID: 135
		public Rect[] rects;

		// Token: 0x04000088 RID: 136
		public int[] srcImgIdxs;

		// Token: 0x04000089 RID: 137
		public object data;
	}
}
