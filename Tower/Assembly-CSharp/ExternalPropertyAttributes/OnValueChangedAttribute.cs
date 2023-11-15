using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000042 RID: 66
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class OnValueChangedAttribute : MetaAttribute
	{
		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000AF RID: 175 RVA: 0x00003818 File Offset: 0x00001A18
		// (set) Token: 0x060000B0 RID: 176 RVA: 0x00003820 File Offset: 0x00001A20
		public string CallbackName { get; private set; }

		// Token: 0x060000B1 RID: 177 RVA: 0x00003829 File Offset: 0x00001A29
		public OnValueChangedAttribute(string callbackName)
		{
			this.CallbackName = callbackName;
		}
	}
}
