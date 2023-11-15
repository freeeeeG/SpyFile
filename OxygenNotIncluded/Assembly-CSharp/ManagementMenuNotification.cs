using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020004E2 RID: 1250
public class ManagementMenuNotification : Notification
{
	// Token: 0x17000119 RID: 281
	// (get) Token: 0x06001CE8 RID: 7400 RVA: 0x0009A319 File Offset: 0x00098519
	// (set) Token: 0x06001CE9 RID: 7401 RVA: 0x0009A321 File Offset: 0x00098521
	public bool hasBeenViewed { get; private set; }

	// Token: 0x1700011A RID: 282
	// (get) Token: 0x06001CEA RID: 7402 RVA: 0x0009A32A File Offset: 0x0009852A
	// (set) Token: 0x06001CEB RID: 7403 RVA: 0x0009A332 File Offset: 0x00098532
	public string highlightTarget { get; set; }

	// Token: 0x06001CEC RID: 7404 RVA: 0x0009A33C File Offset: 0x0009853C
	public ManagementMenuNotification(global::Action targetMenu, NotificationValence valence, string highlightTarget, string title, NotificationType type, Func<List<Notification>, object, string> tooltip = null, object tooltip_data = null, bool expires = true, float delay = 0f, Notification.ClickCallback custom_click_callback = null, object custom_click_data = null, Transform click_focus = null, bool volume_attenuation = true) : base(title, type, tooltip, tooltip_data, expires, delay, custom_click_callback, custom_click_data, click_focus, volume_attenuation, false, false)
	{
		this.targetMenu = targetMenu;
		this.valence = valence;
		this.highlightTarget = highlightTarget;
	}

	// Token: 0x06001CED RID: 7405 RVA: 0x0009A37A File Offset: 0x0009857A
	public void View()
	{
		this.hasBeenViewed = true;
		ManagementMenu.Instance.notificationDisplayer.NotificationWasViewed(this);
	}

	// Token: 0x04001005 RID: 4101
	public global::Action targetMenu;

	// Token: 0x04001006 RID: 4102
	public NotificationValence valence;
}
