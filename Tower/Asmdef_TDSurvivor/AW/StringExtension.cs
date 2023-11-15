using System;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace AW
{
	// Token: 0x020001C4 RID: 452
	public static class StringExtension
	{
		// Token: 0x06000BE3 RID: 3043 RVA: 0x0002E88C File Offset: 0x0002CA8C
		public static string Truncate(this string value, int maxLength)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			if (value.Length > maxLength)
			{
				return value.Substring(0, maxLength);
			}
			return value;
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x0002E8AB File Offset: 0x0002CAAB
		public static string Reverse(this string value)
		{
			char[] array = value.ToCharArray();
			Array.Reverse<char>(array);
			return new string(array);
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x0002E8BE File Offset: 0x0002CABE
		public static string ToString(this object anObject, string aFormat)
		{
			return anObject.ToString(aFormat, null);
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x0002E8C8 File Offset: 0x0002CAC8
		public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Type type = anObject.GetType();
			MatchCollection matchCollection = new Regex("({)([^}]+)(})", RegexOptions.IgnoreCase).Matches(aFormat);
			int num = 0;
			foreach (object obj in matchCollection)
			{
				Group group = ((Match)obj).Groups[2];
				int length = group.Index - num - 1;
				stringBuilder.Append(aFormat.Substring(num, length));
				string name = string.Empty;
				string text = string.Empty;
				int num2 = group.Value.IndexOf(":");
				if (num2 == -1)
				{
					name = group.Value;
				}
				else
				{
					name = group.Value.Substring(0, num2);
					text = group.Value.Substring(num2 + 1);
				}
				PropertyInfo property = type.GetProperty(name);
				Type type2 = null;
				object target = null;
				if (property != null)
				{
					type2 = property.PropertyType;
					target = property.GetValue(anObject, null);
				}
				else
				{
					FieldInfo field = type.GetField(name);
					if (field != null)
					{
						type2 = field.FieldType;
						target = field.GetValue(anObject);
					}
				}
				if (type2 != null)
				{
					string value = string.Empty;
					if (text == string.Empty)
					{
						value = (type2.InvokeMember("ToString", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, target, null) as string);
					}
					else
					{
						value = (type2.InvokeMember("ToString", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, target, new object[]
						{
							text,
							formatProvider
						}) as string);
					}
					stringBuilder.Append(value);
				}
				else
				{
					stringBuilder.Append("{");
					stringBuilder.Append(group.Value);
					stringBuilder.Append("}");
				}
				num = group.Index + group.Length + 1;
			}
			if (num < aFormat.Length)
			{
				stringBuilder.Append(aFormat.Substring(num));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x0002EAE4 File Offset: 0x0002CCE4
		public static string GetStringOfByteLength(this string str, int targetLength)
		{
			string text = "";
			int num = 0;
			int num2 = 0;
			do
			{
				string text2 = str.Substring(num2, 1);
				byte[] bytes = Encoding.Default.GetBytes(text2);
				num += bytes.Length;
				if (num <= targetLength)
				{
					text += text2;
				}
				num2++;
			}
			while (num2 < str.Length && num <= targetLength);
			return text;
		}

		// Token: 0x06000BE8 RID: 3048 RVA: 0x0002EB3C File Offset: 0x0002CD3C
		public static int GetByteLength(this string str)
		{
			int num = 0;
			for (int i = 0; i < str.Length; i++)
			{
				byte[] bytes = Encoding.Default.GetBytes(str.Substring(i, 1));
				num += bytes.Length;
			}
			return num;
		}
	}
}
