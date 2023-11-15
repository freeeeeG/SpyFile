using System;
using System.Collections.Generic;

// Token: 0x020004D3 RID: 1235
public class MessageNotification : Notification
{
	// Token: 0x06001C39 RID: 7225 RVA: 0x00096ED9 File Offset: 0x000950D9
	private string OnToolTip(List<Notification> notifications, string tooltipText)
	{
		return tooltipText;
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x00096EDC File Offset: 0x000950DC
	public MessageNotification(Message m) : base(m.GetTitle(), NotificationType.Messages, null, null, false, 0f, null, null, null, true, false, true)
	{
		MessageNotification <>4__this = this;
		this.message = m;
		if (!this.message.PlayNotificationSound())
		{
			this.playSound = false;
		}
		base.ToolTip = ((List<Notification> notifications, object data) => <>4__this.OnToolTip(notifications, m.GetTooltip()));
		base.clickFocus = null;
	}

	// Token: 0x04000F8C RID: 3980
	public Message message;
}
