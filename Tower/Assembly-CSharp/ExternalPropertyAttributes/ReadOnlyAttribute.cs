using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x0200002D RID: 45
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ReadOnlyAttribute : DrawerAttribute
	{
	}
}
