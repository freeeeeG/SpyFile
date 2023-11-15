using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000049 RID: 73
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class MinValueAttribute : ValidatorAttribute
	{
		// Token: 0x17000026 RID: 38
		// (get) Token: 0x060000C1 RID: 193 RVA: 0x00003A82 File Offset: 0x00001C82
		// (set) Token: 0x060000C2 RID: 194 RVA: 0x00003A8A File Offset: 0x00001C8A
		public float MinValue { get; private set; }

		// Token: 0x060000C3 RID: 195 RVA: 0x00003A93 File Offset: 0x00001C93
		public MinValueAttribute(float minValue)
		{
			this.MinValue = minValue;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00003AA2 File Offset: 0x00001CA2
		public MinValueAttribute(int minValue)
		{
			this.MinValue = (float)minValue;
		}
	}
}
