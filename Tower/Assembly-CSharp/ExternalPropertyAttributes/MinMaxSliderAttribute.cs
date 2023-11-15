using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200002B RID: 43
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class MinMaxSliderAttribute : DrawerAttribute
	{
		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003560 File Offset: 0x00001760
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003568 File Offset: 0x00001768
		public float MinValue { get; private set; }

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003571 File Offset: 0x00001771
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00003579 File Offset: 0x00001779
		public float MaxValue { get; private set; }

		// Token: 0x06000078 RID: 120 RVA: 0x00003582 File Offset: 0x00001782
		public MinMaxSliderAttribute(float minValue, float maxValue)
		{
			this.MinValue = minValue;
			this.MaxValue = maxValue;
		}
	}
}
