using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000036 RID: 54
public static class IntExtensions
{
	// Token: 0x060003CB RID: 971 RVA: 0x00014BDC File Offset: 0x00012DDC
	public static int NotifyModifiers<T>(this int value, string notification, object notifier, T e)
	{
		List<ValueModifier> list = new List<ValueModifier>();
		Info<List<ValueModifier>, T> e2 = new Info<List<ValueModifier>, T>(list, e);
		notifier.PostNotification(notification, e2);
		list.Sort(new Comparison<ValueModifier>(IntExtensions.Compare));
		float num = (float)value;
		for (int i = 0; i < list.Count; i++)
		{
			num = list[i].Modify((float)value, num);
		}
		return Mathf.CeilToInt(num);
	}

	// Token: 0x060003CC RID: 972 RVA: 0x00014C3C File Offset: 0x00012E3C
	public static int NotifyModifiers(this int value, string notification, object notifier)
	{
		List<ValueModifier> list = new List<ValueModifier>();
		notifier.PostNotification(notification, list);
		list.Sort(new Comparison<ValueModifier>(IntExtensions.Compare));
		float num = (float)value;
		for (int i = 0; i < list.Count; i++)
		{
			num = list[i].Modify((float)value, num);
		}
		return Mathf.FloorToInt(num);
	}

	// Token: 0x060003CD RID: 973 RVA: 0x00014C94 File Offset: 0x00012E94
	private static int Compare(ValueModifier x, ValueModifier y)
	{
		return x.sortOrder.CompareTo(y.sortOrder);
	}
}
