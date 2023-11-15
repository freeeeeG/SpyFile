using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000022 RID: 34
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class DropdownAttribute : DrawerAttribute
	{
		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600005F RID: 95 RVA: 0x00003414 File Offset: 0x00001614
		// (set) Token: 0x06000060 RID: 96 RVA: 0x0000341C File Offset: 0x0000161C
		public string ValuesName { get; private set; }

		// Token: 0x06000061 RID: 97 RVA: 0x00003425 File Offset: 0x00001625
		public DropdownAttribute(string valuesName)
		{
			this.ValuesName = valuesName;
		}
	}
}
