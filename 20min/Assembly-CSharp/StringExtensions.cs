using System;

// Token: 0x02000037 RID: 55
public static class StringExtensions
{
	// Token: 0x060003CE RID: 974 RVA: 0x00014CB5 File Offset: 0x00012EB5
	public static string Truncate(this string value, int maxChars)
	{
		if (value.Length > maxChars)
		{
			return value.Substring(0, maxChars) + "...";
		}
		return value;
	}
}
