using System;
using System.Text;

// Token: 0x0200021F RID: 543
public static class IntUtils
{
	// Token: 0x06000924 RID: 2340 RVA: 0x00035FD8 File Offset: 0x000343D8
	public static string ToTimeString(this int _value)
	{
		IntUtils.s_stringBuilder.Length = 0;
		int num = _value / 60;
		int num2 = _value % 60;
		if (num < 10)
		{
			IntUtils.s_stringBuilder.Append("0");
			IntUtils.s_stringBuilder.Append(IntUtils.m_digits[num]);
		}
		else
		{
			int num3 = num / 10;
			int num4 = num % 10;
			IntUtils.s_stringBuilder.Append(IntUtils.m_digits[num3]);
			IntUtils.s_stringBuilder.Append(IntUtils.m_digits[num4]);
		}
		IntUtils.s_stringBuilder.Append(":");
		if (num2 < 10)
		{
			IntUtils.s_stringBuilder.Append("0");
			IntUtils.s_stringBuilder.Append(IntUtils.m_digits[num2]);
		}
		else
		{
			int num5 = num2 / 10;
			int num6 = num2 % 10;
			IntUtils.s_stringBuilder.Append(IntUtils.m_digits[num5]);
			IntUtils.s_stringBuilder.Append(IntUtils.m_digits[num6]);
		}
		return IntUtils.s_stringBuilder.ToString();
	}

	// Token: 0x040007E8 RID: 2024
	private static StringBuilder s_stringBuilder = new StringBuilder(16, 16);

	// Token: 0x040007E9 RID: 2025
	private static string[] m_digits = new string[]
	{
		"0",
		"1",
		"2",
		"3",
		"4",
		"5",
		"6",
		"7",
		"8",
		"9"
	};
}
