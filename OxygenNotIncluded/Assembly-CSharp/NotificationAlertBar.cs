using System;
using System.Collections.Generic;

// Token: 0x02000BA9 RID: 2985
public class NotificationAlertBar : KMonoBehaviour
{
	// Token: 0x06005D23 RID: 23843 RVA: 0x00221894 File Offset: 0x0021FA94
	public void Init(ManagementMenuNotification notification)
	{
		this.notification = notification;
		this.thisButton.onClick += this.OnThisButtonClicked;
		this.background.colorStyleSetting = this.alertColorStyle[(int)notification.valence];
		this.background.ApplyColorStyleSetting();
		this.text.text = notification.titleText;
		this.tooltip.SetSimpleTooltip(notification.ToolTip(null, notification.tooltipData));
		this.muteButton.onClick += this.OnMuteButtonClicked;
	}

	// Token: 0x06005D24 RID: 23844 RVA: 0x0022192C File Offset: 0x0021FB2C
	private void OnThisButtonClicked()
	{
		NotificationHighlightController componentInParent = base.GetComponentInParent<NotificationHighlightController>();
		if (componentInParent != null)
		{
			componentInParent.SetActiveTarget(this.notification);
			return;
		}
		this.notification.View();
	}

	// Token: 0x06005D25 RID: 23845 RVA: 0x00221961 File Offset: 0x0021FB61
	private void OnMuteButtonClicked()
	{
	}

	// Token: 0x04003EB2 RID: 16050
	public ManagementMenuNotification notification;

	// Token: 0x04003EB3 RID: 16051
	public KButton thisButton;

	// Token: 0x04003EB4 RID: 16052
	public KImage background;

	// Token: 0x04003EB5 RID: 16053
	public LocText text;

	// Token: 0x04003EB6 RID: 16054
	public ToolTip tooltip;

	// Token: 0x04003EB7 RID: 16055
	public KButton muteButton;

	// Token: 0x04003EB8 RID: 16056
	public List<ColorStyleSetting> alertColorStyle;
}
