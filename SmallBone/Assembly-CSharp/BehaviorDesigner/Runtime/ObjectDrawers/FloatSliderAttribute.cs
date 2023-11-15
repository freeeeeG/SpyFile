using System;
using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.ObjectDrawers
{
	// Token: 0x02001655 RID: 5717
	public class FloatSliderAttribute : ObjectDrawerAttribute
	{
		// Token: 0x06006CF9 RID: 27897 RVA: 0x001377B8 File Offset: 0x001359B8
		public FloatSliderAttribute(float min, float max)
		{
			this.min = min;
			this.max = max;
		}

		// Token: 0x040058BD RID: 22717
		public float min;

		// Token: 0x040058BE RID: 22718
		public float max;
	}
}
