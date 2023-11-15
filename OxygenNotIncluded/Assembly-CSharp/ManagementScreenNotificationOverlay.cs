using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000BA8 RID: 2984
public class ManagementScreenNotificationOverlay : KMonoBehaviour
{
	// Token: 0x06005D1E RID: 23838 RVA: 0x00221848 File Offset: 0x0021FA48
	protected void OnEnable()
	{
	}

	// Token: 0x06005D1F RID: 23839 RVA: 0x0022184A File Offset: 0x0021FA4A
	protected override void OnDisable()
	{
	}

	// Token: 0x06005D20 RID: 23840 RVA: 0x0022184C File Offset: 0x0021FA4C
	private NotificationAlertBar CreateAlertBar(ManagementMenuNotification notification)
	{
		NotificationAlertBar notificationAlertBar = Util.KInstantiateUI<NotificationAlertBar>(this.alertBarPrefab.gameObject, this.alertContainer.gameObject, false);
		notificationAlertBar.Init(notification);
		notificationAlertBar.gameObject.SetActive(true);
		return notificationAlertBar;
	}

	// Token: 0x06005D21 RID: 23841 RVA: 0x0022187D File Offset: 0x0021FA7D
	private void NotificationsChanged()
	{
	}

	// Token: 0x04003EAE RID: 16046
	public global::Action currentMenu;

	// Token: 0x04003EAF RID: 16047
	public NotificationAlertBar alertBarPrefab;

	// Token: 0x04003EB0 RID: 16048
	public RectTransform alertContainer;

	// Token: 0x04003EB1 RID: 16049
	private List<NotificationAlertBar> alertBars = new List<NotificationAlertBar>();
}
