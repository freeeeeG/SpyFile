using System;
using System.Collections.Generic;

// Token: 0x02000BA7 RID: 2983
public class ManagementMenuNotificationDisplayer : NotificationDisplayer
{
	// Token: 0x17000693 RID: 1683
	// (get) Token: 0x06005D13 RID: 23827 RVA: 0x002216F1 File Offset: 0x0021F8F1
	// (set) Token: 0x06005D14 RID: 23828 RVA: 0x002216F9 File Offset: 0x0021F8F9
	public List<ManagementMenuNotification> displayedManagementMenuNotifications { get; private set; }

	// Token: 0x14000029 RID: 41
	// (add) Token: 0x06005D15 RID: 23829 RVA: 0x00221704 File Offset: 0x0021F904
	// (remove) Token: 0x06005D16 RID: 23830 RVA: 0x0022173C File Offset: 0x0021F93C
	public event System.Action onNotificationsChanged;

	// Token: 0x06005D17 RID: 23831 RVA: 0x00221771 File Offset: 0x0021F971
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.displayedManagementMenuNotifications = new List<ManagementMenuNotification>();
	}

	// Token: 0x06005D18 RID: 23832 RVA: 0x00221784 File Offset: 0x0021F984
	public void NotificationWasViewed(ManagementMenuNotification notification)
	{
		this.onNotificationsChanged();
	}

	// Token: 0x06005D19 RID: 23833 RVA: 0x00221791 File Offset: 0x0021F991
	protected override void OnNotificationAdded(Notification notification)
	{
		this.displayedManagementMenuNotifications.Add(notification as ManagementMenuNotification);
		this.onNotificationsChanged();
	}

	// Token: 0x06005D1A RID: 23834 RVA: 0x002217AF File Offset: 0x0021F9AF
	protected override void OnNotificationRemoved(Notification notification)
	{
		this.displayedManagementMenuNotifications.Remove(notification as ManagementMenuNotification);
		this.onNotificationsChanged();
	}

	// Token: 0x06005D1B RID: 23835 RVA: 0x002217CE File Offset: 0x0021F9CE
	protected override bool ShouldDisplayNotification(Notification notification)
	{
		return notification is ManagementMenuNotification;
	}

	// Token: 0x06005D1C RID: 23836 RVA: 0x002217DC File Offset: 0x0021F9DC
	public List<ManagementMenuNotification> GetNotificationsForAction(global::Action hotKey)
	{
		List<ManagementMenuNotification> list = new List<ManagementMenuNotification>();
		foreach (ManagementMenuNotification managementMenuNotification in this.displayedManagementMenuNotifications)
		{
			if (managementMenuNotification.targetMenu == hotKey)
			{
				list.Add(managementMenuNotification);
			}
		}
		return list;
	}
}
