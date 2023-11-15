using System;
using System.Collections.Generic;

// Token: 0x02000BAD RID: 2989
public class NotificationManager : KMonoBehaviour
{
	// Token: 0x17000694 RID: 1684
	// (get) Token: 0x06005D3D RID: 23869 RVA: 0x00221E17 File Offset: 0x00220017
	// (set) Token: 0x06005D3E RID: 23870 RVA: 0x00221E1E File Offset: 0x0022001E
	public static NotificationManager Instance { get; private set; }

	// Token: 0x1400002A RID: 42
	// (add) Token: 0x06005D3F RID: 23871 RVA: 0x00221E28 File Offset: 0x00220028
	// (remove) Token: 0x06005D40 RID: 23872 RVA: 0x00221E60 File Offset: 0x00220060
	public event Action<Notification> notificationAdded;

	// Token: 0x1400002B RID: 43
	// (add) Token: 0x06005D41 RID: 23873 RVA: 0x00221E98 File Offset: 0x00220098
	// (remove) Token: 0x06005D42 RID: 23874 RVA: 0x00221ED0 File Offset: 0x002200D0
	public event Action<Notification> notificationRemoved;

	// Token: 0x06005D43 RID: 23875 RVA: 0x00221F05 File Offset: 0x00220105
	protected override void OnPrefabInit()
	{
		Debug.Assert(NotificationManager.Instance == null);
		NotificationManager.Instance = this;
	}

	// Token: 0x06005D44 RID: 23876 RVA: 0x00221F1D File Offset: 0x0022011D
	protected override void OnForcedCleanUp()
	{
		NotificationManager.Instance = null;
	}

	// Token: 0x06005D45 RID: 23877 RVA: 0x00221F25 File Offset: 0x00220125
	public void AddNotification(Notification notification)
	{
		this.pendingNotifications.Add(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.AddPendingNotification(notification);
		}
	}

	// Token: 0x06005D46 RID: 23878 RVA: 0x00221F4C File Offset: 0x0022014C
	public void RemoveNotification(Notification notification)
	{
		this.pendingNotifications.Remove(notification);
		if (NotificationScreen.Instance != null)
		{
			NotificationScreen.Instance.RemovePendingNotification(notification);
		}
		if (this.notifications.Remove(notification))
		{
			this.notificationRemoved(notification);
		}
	}

	// Token: 0x06005D47 RID: 23879 RVA: 0x00221F98 File Offset: 0x00220198
	private void Update()
	{
		int i = 0;
		while (i < this.pendingNotifications.Count)
		{
			if (this.pendingNotifications[i].IsReady())
			{
				this.DoAddNotification(this.pendingNotifications[i]);
				this.pendingNotifications.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
	}

	// Token: 0x06005D48 RID: 23880 RVA: 0x00221FEE File Offset: 0x002201EE
	private void DoAddNotification(Notification notification)
	{
		this.notifications.Add(notification);
		if (this.notificationAdded != null)
		{
			this.notificationAdded(notification);
		}
	}

	// Token: 0x04003EC3 RID: 16067
	private List<Notification> pendingNotifications = new List<Notification>();

	// Token: 0x04003EC4 RID: 16068
	private List<Notification> notifications = new List<Notification>();
}
