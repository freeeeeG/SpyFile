using System;

namespace ExternalPropertyAttributes
{
	// Token: 0x02000036 RID: 54
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ShowNonSerializedFieldAttribute : SpecialCaseDrawerAttribute
	{
	}
}
