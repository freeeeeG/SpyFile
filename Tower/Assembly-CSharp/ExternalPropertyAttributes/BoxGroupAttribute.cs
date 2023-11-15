using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000039 RID: 57
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class BoxGroupAttribute : MetaAttribute, IGroupAttribute
	{
		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000097 RID: 151 RVA: 0x000036E5 File Offset: 0x000018E5
		// (set) Token: 0x06000098 RID: 152 RVA: 0x000036ED File Offset: 0x000018ED
		public string Name { get; private set; }

		// Token: 0x06000099 RID: 153 RVA: 0x000036F6 File Offset: 0x000018F6
		public BoxGroupAttribute(string name = "")
		{
			this.Name = name;
		}
	}
}
