using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200004B RID: 75
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ValidateInputAttribute : ValidatorAttribute
	{
		// Token: 0x17000028 RID: 40
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00003AD2 File Offset: 0x00001CD2
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x00003ADA File Offset: 0x00001CDA
		public string CallbackName { get; private set; }

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x060000CA RID: 202 RVA: 0x00003AE3 File Offset: 0x00001CE3
		// (set) Token: 0x060000CB RID: 203 RVA: 0x00003AEB File Offset: 0x00001CEB
		public string Message { get; private set; }

		// Token: 0x060000CC RID: 204 RVA: 0x00003AF4 File Offset: 0x00001CF4
		public ValidateInputAttribute(string callbackName, string message = null)
		{
			this.CallbackName = callbackName;
			this.Message = message;
		}
	}
}
