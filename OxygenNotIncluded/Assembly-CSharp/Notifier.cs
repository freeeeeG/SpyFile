using System;
using UnityEngine;

// Token: 0x020004E5 RID: 1253
[SkipSaveFileSerialization]
[AddComponentMenu("KMonoBehaviour/scripts/Notifier")]
public class Notifier : KMonoBehaviour
{
	// Token: 0x06001D08 RID: 7432 RVA: 0x0009A730 File Offset: 0x00098930
	protected override void OnPrefabInit()
	{
		Components.Notifiers.Add(this);
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x0009A73D File Offset: 0x0009893D
	protected override void OnCleanUp()
	{
		Components.Notifiers.Remove(this);
	}

	// Token: 0x06001D0A RID: 7434 RVA: 0x0009A74C File Offset: 0x0009894C
	public void Add(Notification notification, string suffix = "")
	{
		if (KScreenManager.Instance == null)
		{
			return;
		}
		if (this.DisableNotifications)
		{
			return;
		}
		if (DebugHandler.NotificationsDisabled)
		{
			return;
		}
		DebugUtil.DevAssert(notification != null, "Trying to add null notification. It's safe to continue playing, the notification won't be displayed.", null);
		if (notification == null)
		{
			return;
		}
		if (notification.Notifier == null)
		{
			if (this.Selectable != null)
			{
				notification.NotifierName = "• " + this.Selectable.GetName() + suffix;
			}
			else
			{
				notification.NotifierName = "• " + base.name + suffix;
			}
			notification.Notifier = this;
			if (this.AutoClickFocus && notification.clickFocus == null)
			{
				notification.clickFocus = base.transform;
			}
			NotificationManager.Instance.AddNotification(notification);
			notification.GameTime = Time.time;
		}
		else
		{
			DebugUtil.Assert(notification.Notifier == this);
		}
		notification.Time = KTime.Instance.UnscaledGameTime;
	}

	// Token: 0x06001D0B RID: 7435 RVA: 0x0009A841 File Offset: 0x00098A41
	public void Remove(Notification notification)
	{
		if (notification == null)
		{
			return;
		}
		if (notification.Notifier != null)
		{
			notification.Notifier = null;
		}
		if (NotificationManager.Instance != null)
		{
			NotificationManager.Instance.RemoveNotification(notification);
		}
	}

	// Token: 0x0400101C RID: 4124
	[MyCmpGet]
	private KSelectable Selectable;

	// Token: 0x0400101D RID: 4125
	public bool DisableNotifications;

	// Token: 0x0400101E RID: 4126
	public bool AutoClickFocus = true;
}
