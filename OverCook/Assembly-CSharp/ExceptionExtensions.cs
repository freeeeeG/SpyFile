using System;
using System.Reflection;

// Token: 0x0200063A RID: 1594
public static class ExceptionExtensions
{
	// Token: 0x06001E5A RID: 7770 RVA: 0x00092E84 File Offset: 0x00091284
	public static int HResultPublic(this Exception exception)
	{
		PropertyInfo[] array = exception.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FindAll((PropertyInfo x) => x.Name.Equals("HResult"));
		if (array.Length > 0)
		{
			return (int)array[0].GetValue(exception, null);
		}
		return 0;
	}
}
