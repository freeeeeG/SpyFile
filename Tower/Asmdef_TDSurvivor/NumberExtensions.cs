using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public static class NumberExtensions
{
	// Token: 0x06000622 RID: 1570 RVA: 0x000178C7 File Offset: 0x00015AC7
	public static string Abbreviated(this int value)
	{
		return NumberExtensions.AbbreviateNumber((float)value);
	}

	// Token: 0x06000623 RID: 1571 RVA: 0x000178D0 File Offset: 0x00015AD0
	public static string Abbreviated(this long value)
	{
		return NumberExtensions.AbbreviateNumber((float)value);
	}

	// Token: 0x06000624 RID: 1572 RVA: 0x000178D9 File Offset: 0x00015AD9
	public static string Abbreviated(this float value)
	{
		return NumberExtensions.AbbreviateNumber(value);
	}

	// Token: 0x06000625 RID: 1573 RVA: 0x000178E4 File Offset: 0x00015AE4
	public static string AbbreviateNumber(float number)
	{
		for (int i = NumberExtensions.abbrevations.Count - 1; i >= 0; i--)
		{
			KeyValuePair<long, string> keyValuePair = NumberExtensions.abbrevations.ElementAt(i);
			if (Mathf.Abs(number) >= (float)keyValuePair.Key)
			{
				return Mathf.FloorToInt(number / (float)keyValuePair.Key).ToString() + keyValuePair.Value;
			}
		}
		return number.ToString();
	}

	// Token: 0x0400054D RID: 1357
	private static readonly SortedDictionary<long, string> abbrevations = new SortedDictionary<long, string>
	{
		{
			1000L,
			"K"
		},
		{
			1000000L,
			"M"
		},
		{
			1000000000L,
			"B"
		},
		{
			1000000000000L,
			"T"
		}
	};
}
