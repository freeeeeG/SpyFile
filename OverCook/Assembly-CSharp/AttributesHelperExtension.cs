using System;
using System.ComponentModel;
using System.Reflection;

// Token: 0x0200073D RID: 1853
public static class AttributesHelperExtension
{
	// Token: 0x060023AB RID: 9131 RVA: 0x000AAD38 File Offset: 0x000A9138
	public static string ToDescription(this Enum value)
	{
		FieldInfo field = value.GetType().GetField(value.ToString());
		if (field != null)
		{
			DescriptionAttribute[] array = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false);
			return (array.Length <= 0) ? value.ToString() : array[0].Description;
		}
		return value.ToString();
	}
}
