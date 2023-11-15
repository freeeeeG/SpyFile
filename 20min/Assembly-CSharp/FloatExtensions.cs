using System;
using System.Collections.Generic;

// Token: 0x02000035 RID: 53
public static class FloatExtensions
{
	// Token: 0x060003C8 RID: 968 RVA: 0x00014B10 File Offset: 0x00012D10
	public static float NotifyModifiers<T>(this float value, string notification, object notifier, T e)
	{
		List<ValueModifier> list = new List<ValueModifier>();
		Info<List<ValueModifier>, T> e2 = new Info<List<ValueModifier>, T>(list, e);
		notifier.PostNotification(notification, e2);
		list.Sort(new Comparison<ValueModifier>(FloatExtensions.Compare));
		float num = value;
		for (int i = 0; i < list.Count; i++)
		{
			num = list[i].Modify(value, num);
		}
		return num;
	}

	// Token: 0x060003C9 RID: 969 RVA: 0x00014B68 File Offset: 0x00012D68
	public static float NotifyModifiers(this float value, string notification, object notifier)
	{
		List<ValueModifier> list = new List<ValueModifier>();
		notifier.PostNotification(notification, list);
		list.Sort(new Comparison<ValueModifier>(FloatExtensions.Compare));
		float num = value;
		for (int i = 0; i < list.Count; i++)
		{
			num = list[i].Modify(value, num);
		}
		return num;
	}

	// Token: 0x060003CA RID: 970 RVA: 0x00014BB8 File Offset: 0x00012DB8
	private static int Compare(ValueModifier x, ValueModifier y)
	{
		return x.sortOrder.CompareTo(y.sortOrder);
	}
}
