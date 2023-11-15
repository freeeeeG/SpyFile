using System;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.ObjectDrawers
{
	// Token: 0x02001656 RID: 5718
	public class IntSliderAttribute : ObjectDrawerAttribute
	{
		// Token: 0x06006CFA RID: 27898 RVA: 0x001377CE File Offset: 0x001359CE
		public IntSliderAttribute(int min, int max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040058BF RID: 22719
		public int min;

		// Token: 0x040058C0 RID: 22720
		public int max;
	}
}
