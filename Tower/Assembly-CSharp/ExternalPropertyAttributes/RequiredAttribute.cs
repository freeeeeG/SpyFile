using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200004A RID: 74
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class RequiredAttribute : ValidatorAttribute
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x060000C5 RID: 197 RVA: 0x00003AB2 File Offset: 0x00001CB2
		// (set) Token: 0x060000C6 RID: 198 RVA: 0x00003ABA File Offset: 0x00001CBA
		public string Message { get; private set; }

		// Token: 0x060000C7 RID: 199 RVA: 0x00003AC3 File Offset: 0x00001CC3
		public RequiredAttribute(string message = null)
		{
			this.Message = message;
		}
	}
}
