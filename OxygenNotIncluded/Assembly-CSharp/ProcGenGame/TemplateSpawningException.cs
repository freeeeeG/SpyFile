using System;

namespace ProcGenGame
{
	// Token: 0x02000CAE RID: 3246
	public class TemplateSpawningException : Exception
	{
		// Token: 0x0600675C RID: 26460 RVA: 0x0026C7FA File Offset: 0x0026A9FA
		public TemplateSpawningException(string message, string userMessage) : base(message)
		{
			this.userMessage = userMessage;
		}

		// Token: 0x04004771 RID: 18289
		public readonly string userMessage;
	}
}
