using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000048 RID: 72
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class MaxValueAttribute : ValidatorAttribute
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060000BD RID: 189 RVA: 0x00003A52 File Offset: 0x00001C52
		// (set) Token: 0x060000BE RID: 190 RVA: 0x00003A5A File Offset: 0x00001C5A
		public float MaxValue { get; private set; }

		// Token: 0x060000BF RID: 191 RVA: 0x00003A63 File Offset: 0x00001C63
		public MaxValueAttribute(float maxValue)
		{
			this.MaxValue = maxValue;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00003A72 File Offset: 0x00001C72
		public MaxValueAttribute(int maxValue)
		{
			this.MaxValue = (float)maxValue;
		}
	}
}
