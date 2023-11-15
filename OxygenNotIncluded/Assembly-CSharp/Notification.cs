using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

// Token: 0x020004E3 RID: 1251
public class Notification
{
	// Token: 0x1700011B RID: 283
	// (get) Token: 0x06001CEE RID: 7406 RVA: 0x0009A393 File Offset: 0x00098593
	// (set) Token: 0x06001CEF RID: 7407 RVA: 0x0009A39B File Offset: 0x0009859B
	public NotificationType Type { get; set; }

	// Token: 0x1700011C RID: 284
	// (get) Token: 0x06001CF0 RID: 7408 RVA: 0x0009A3A4 File Offset: 0x000985A4
	// (set) Token: 0x06001CF1 RID: 7409 RVA: 0x0009A3AC File Offset: 0x000985AC
	public Notifier Notifier { get; set; }

	// Token: 0x1700011D RID: 285
	// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x0009A3B5 File Offset: 0x000985B5
	// (set) Token: 0x06001CF3 RID: 7411 RVA: 0x0009A3BD File Offset: 0x000985BD
	public Transform clickFocus { get; set; }

	// Token: 0x1700011E RID: 286
	// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x0009A3C6 File Offset: 0x000985C6
	// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x0009A3CE File Offset: 0x000985CE
	public float Time { get; set; }

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x0009A3D7 File Offset: 0x000985D7
	// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x0009A3DF File Offset: 0x000985DF
	public float GameTime { get; set; }

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x0009A3E8 File Offset: 0x000985E8
	// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x0009A3F0 File Offset: 0x000985F0
	public float Delay { get; set; }

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x06001CFA RID: 7418 RVA: 0x0009A3F9 File Offset: 0x000985F9
	// (set) Token: 0x06001CFB RID: 7419 RVA: 0x0009A401 File Offset: 0x00098601
	public int Idx { get; set; }

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x06001CFC RID: 7420 RVA: 0x0009A40A File Offset: 0x0009860A
	// (set) Token: 0x06001CFD RID: 7421 RVA: 0x0009A412 File Offset: 0x00098612
	public Func<List<Notification>, object, string> ToolTip { get; set; }

	// Token: 0x06001CFE RID: 7422 RVA: 0x0009A41B File Offset: 0x0009861B
	public bool IsReady()
	{
		return UnityEngine.Time.time >= this.GameTime + this.Delay;
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x06001CFF RID: 7423 RVA: 0x0009A434 File Offset: 0x00098634
	// (set) Token: 0x06001D00 RID: 7424 RVA: 0x0009A43C File Offset: 0x0009863C
	public string titleText { get; private set; }

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x06001D01 RID: 7425 RVA: 0x0009A445 File Offset: 0x00098645
	// (set) Token: 0x06001D02 RID: 7426 RVA: 0x0009A44D File Offset: 0x0009864D
	public string NotifierName
	{
		get
		{
			return this.notifierName;
		}
		set
		{
			this.notifierName = value;
			this.titleText = this.ReplaceTags(this.titleText);
		}
	}

	// Token: 0x06001D03 RID: 7427 RVA: 0x0009A468 File Offset: 0x00098668
	public Notification(string title, NotificationType type, Func<List<Notification>, object, string> tooltip = null, object tooltip_data = null, bool expires = true, float delay = 0f, Notification.ClickCallback custom_click_callback = null, object custom_click_data = null, Transform click_focus = null, bool volume_attenuation = true, bool clear_on_click = false, bool show_dismiss_button = false)
	{
		this.titleText = title;
		this.Type = type;
		this.ToolTip = tooltip;
		this.tooltipData = tooltip_data;
		this.expires = expires;
		this.Delay = delay;
		this.customClickCallback = custom_click_callback;
		this.customClickData = custom_click_data;
		this.clickFocus = click_focus;
		this.volume_attenuation = volume_attenuation;
		this.clearOnClick = clear_on_click;
		this.showDismissButton = show_dismiss_button;
		int num = this.notificationIncrement;
		this.notificationIncrement = num + 1;
		this.Idx = num;
	}

	// Token: 0x06001D04 RID: 7428 RVA: 0x0009A504 File Offset: 0x00098704
	public void Clear()
	{
		if (this.Notifier != null)
		{
			this.Notifier.Remove(this);
			return;
		}
		NotificationManager.Instance.RemoveNotification(this);
	}

	// Token: 0x06001D05 RID: 7429 RVA: 0x0009A52C File Offset: 0x0009872C
	private string ReplaceTags(string text)
	{
		DebugUtil.Assert(text != null);
		int num = text.IndexOf('{');
		int num2 = text.IndexOf('}');
		if (0 <= num && num < num2)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num3 = 0;
			while (0 <= num)
			{
				string value = text.Substring(num3, num - num3);
				stringBuilder.Append(value);
				num2 = text.IndexOf('}', num);
				if (num >= num2)
				{
					break;
				}
				string tag = text.Substring(num + 1, num2 - num - 1);
				string tagDescription = this.GetTagDescription(tag);
				stringBuilder.Append(tagDescription);
				num3 = num2 + 1;
				num = text.IndexOf('{', num2);
			}
			stringBuilder.Append(text.Substring(num3, text.Length - num3));
			return stringBuilder.ToString();
		}
		return text;
	}

	// Token: 0x06001D06 RID: 7430 RVA: 0x0009A5E0 File Offset: 0x000987E0
	private string GetTagDescription(string tag)
	{
		string result;
		if (tag == "NotifierName")
		{
			result = this.notifierName;
		}
		else
		{
			result = "UNKNOWN TAG: " + tag;
		}
		return result;
	}

	// Token: 0x04001011 RID: 4113
	public object tooltipData;

	// Token: 0x04001012 RID: 4114
	public bool expires = true;

	// Token: 0x04001013 RID: 4115
	public bool playSound = true;

	// Token: 0x04001014 RID: 4116
	public bool volume_attenuation = true;

	// Token: 0x04001015 RID: 4117
	public Notification.ClickCallback customClickCallback;

	// Token: 0x04001016 RID: 4118
	public bool clearOnClick;

	// Token: 0x04001017 RID: 4119
	public bool showDismissButton;

	// Token: 0x04001018 RID: 4120
	public object customClickData;

	// Token: 0x04001019 RID: 4121
	private int notificationIncrement;

	// Token: 0x0400101B RID: 4123
	private string notifierName;

	// Token: 0x02001184 RID: 4484
	// (Invoke) Token: 0x060079DF RID: 31199
	public delegate void ClickCallback(object data);
}
