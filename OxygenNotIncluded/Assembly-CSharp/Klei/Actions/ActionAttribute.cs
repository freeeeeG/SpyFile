using System;

namespace Klei.Actions
{
	// Token: 0x02000E20 RID: 3616
	[AttributeUsage(AttributeTargets.Class)]
	public class ActionAttribute : Attribute
	{
		// Token: 0x06006EBC RID: 28348 RVA: 0x002B86C6 File Offset: 0x002B68C6
		public ActionAttribute(string actionName)
		{
			this.ActionName = actionName;
		}

		// Token: 0x040052E5 RID: 21221
		public readonly string ActionName;
	}
}
