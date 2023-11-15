using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000040 RID: 64
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class LabelAttribute : MetaAttribute
	{
		// Token: 0x17000020 RID: 32
		// (get) Token: 0x060000AB RID: 171 RVA: 0x000037F0 File Offset: 0x000019F0
		// (set) Token: 0x060000AC RID: 172 RVA: 0x000037F8 File Offset: 0x000019F8
		public string Label { get; private set; }

		// Token: 0x060000AD RID: 173 RVA: 0x00003801 File Offset: 0x00001A01
		public LabelAttribute(string label)
		{
			this.Label = label;
		}
	}
}
