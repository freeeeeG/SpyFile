using System;

// Token: 0x0200003F RID: 63
public static class NotificationExtensions
{
	// Token: 0x060003E0 RID: 992 RVA: 0x000150C4 File Offset: 0x000132C4
	public static void PostNotification(this object obj, string notificationName)
	{
		NotificationCenter.instance.PostNotification(notificationName, obj);
	}

	// Token: 0x060003E1 RID: 993 RVA: 0x000150D2 File Offset: 0x000132D2
	public static void PostNotification(this object obj, string notificationName, object e)
	{
		NotificationCenter.instance.PostNotification(notificationName, obj, e);
	}

	// Token: 0x060003E2 RID: 994 RVA: 0x000150E1 File Offset: 0x000132E1
	public static void AddObserver(this object obj, Action<object, object> handler, string notificationName)
	{
		NotificationCenter.instance.AddObserver(handler, notificationName);
	}

	// Token: 0x060003E3 RID: 995 RVA: 0x000150EF File Offset: 0x000132EF
	public static void AddObserver(this object obj, Action<object, object> handler, string notificationName, object sender)
	{
		NotificationCenter.instance.AddObserver(handler, notificationName, sender);
	}

	// Token: 0x060003E4 RID: 996 RVA: 0x000150FE File Offset: 0x000132FE
	public static void RemoveObserver(this object obj, Action<object, object> handler, string notificationName)
	{
		NotificationCenter.instance.RemoveObserver(handler, notificationName);
	}

	// Token: 0x060003E5 RID: 997 RVA: 0x0001510C File Offset: 0x0001330C
	public static void RemoveObserver(this object obj, Action<object, object> handler, string notificationName, object sender)
	{
		NotificationCenter.instance.RemoveObserver(handler, notificationName, sender);
	}
}
