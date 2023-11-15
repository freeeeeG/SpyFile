using System;
using System.Collections.Generic;

// Token: 0x02000BAA RID: 2986
public abstract class NotificationDisplayer : KMonoBehaviour
{
	// Token: 0x06005D27 RID: 23847 RVA: 0x0022196B File Offset: 0x0021FB6B
	protected override void OnSpawn()
	{
		this.displayedNotifications = new List<Notification>();
		NotificationManager.Instance.notificationAdded += this.NotificationAdded;
		NotificationManager.Instance.notificationRemoved += this.NotificationRemoved;
	}

	// Token: 0x06005D28 RID: 23848 RVA: 0x002219A4 File Offset: 0x0021FBA4
	public void NotificationAdded(Notification notification)
	{
		if (this.ShouldDisplayNotification(notification))
		{
			this.displayedNotifications.Add(notification);
			this.OnNotificationAdded(notification);
		}
	}

	// Token: 0x06005D29 RID: 23849
	protected abstract void OnNotificationAdded(Notification notification);

	// Token: 0x06005D2A RID: 23850 RVA: 0x002219C2 File Offset: 0x0021FBC2
	public void NotificationRemoved(Notification notification)
	{
		if (this.displayedNotifications.Contains(notification))
		{
			this.displayedNotifications.Remove(notification);
			this.OnNotificationRemoved(notification);
		}
	}

	// Token: 0x06005D2B RID: 23851
	protected abstract void OnNotificationRemoved(Notification notification);

	// Token: 0x06005D2C RID: 23852
	protected abstract bool ShouldDisplayNotification(Notification notification);

	// Token: 0x04003EB9 RID: 16057
	protected List<Notification> displayedNotifications;
}
