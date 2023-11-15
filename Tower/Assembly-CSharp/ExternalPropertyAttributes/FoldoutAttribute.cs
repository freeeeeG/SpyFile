using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200003D RID: 61
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class FoldoutAttribute : MetaAttribute, IGroupAttribute
	{
		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000037AF File Offset: 0x000019AF
		// (set) Token: 0x060000A7 RID: 167 RVA: 0x000037B7 File Offset: 0x000019B7
		public string Name { get; private set; }

		// Token: 0x060000A8 RID: 168 RVA: 0x000037C0 File Offset: 0x000019C0
		public FoldoutAttribute(string name)
		{
			this.Name = name;
		}
	}
}
