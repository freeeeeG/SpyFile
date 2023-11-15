using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BA6 RID: 2982
public class NotificationScreen : KScreen
{
	// Token: 0x17000691 RID: 1681
	// (get) Token: 0x06005CFA RID: 23802 RVA: 0x00220982 File Offset: 0x0021EB82
	// (set) Token: 0x06005CFB RID: 23803 RVA: 0x00220989 File Offset: 0x0021EB89
	public static NotificationScreen Instance { get; private set; }

	// Token: 0x06005CFC RID: 23804 RVA: 0x00220991 File Offset: 0x0021EB91
	public static void DestroyInstance()
	{
		NotificationScreen.Instance = null;
	}

	// Token: 0x06005CFD RID: 23805 RVA: 0x00220999 File Offset: 0x0021EB99
	public void AddPendingNotification(Notification notification)
	{
		this.pendingNotifications.Add(notification);
	}

	// Token: 0x06005CFE RID: 23806 RVA: 0x002209A7 File Offset: 0x0021EBA7
	public void RemovePendingNotification(Notification notification)
	{
		this.dirty = true;
		this.pendingNotifications.Remove(notification);
		this.RemoveNotification(notification);
	}

	// Token: 0x06005CFF RID: 23807 RVA: 0x002209C4 File Offset: 0x0021EBC4
	public void RemoveNotification(Notification notification)
	{
		NotificationScreen.Entry entry = null;
		this.entriesByMessage.TryGetValue(notification.titleText, out entry);
		if (entry == null)
		{
			return;
		}
		this.notifications.Remove(notification);
		entry.Remove(notification);
		if (entry.notifications.Count == 0)
		{
			UnityEngine.Object.Destroy(entry.label);
			this.entriesByMessage[notification.titleText] = null;
			this.entries.Remove(entry);
		}
	}

	// Token: 0x06005D00 RID: 23808 RVA: 0x00220A36 File Offset: 0x0021EC36
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NotificationScreen.Instance = this;
		this.MessagesPrefab.gameObject.SetActive(false);
		this.LabelPrefab.gameObject.SetActive(false);
		this.InitNotificationSounds();
	}

	// Token: 0x06005D01 RID: 23809 RVA: 0x00220A6C File Offset: 0x0021EC6C
	private void OnNewMessage(object data)
	{
		Message m = (Message)data;
		this.notifier.Add(new MessageNotification(m), "");
	}

	// Token: 0x06005D02 RID: 23810 RVA: 0x00220A98 File Offset: 0x0021EC98
	private void ShowMessage(MessageNotification mn)
	{
		mn.message.OnClick();
		if (mn.message.ShowDialog())
		{
			for (int i = 0; i < this.dialogPrefabs.Count; i++)
			{
				if (this.dialogPrefabs[i].CanDisplay(mn.message))
				{
					if (this.messageDialog != null)
					{
						UnityEngine.Object.Destroy(this.messageDialog.gameObject);
						this.messageDialog = null;
					}
					this.messageDialog = global::Util.KInstantiateUI<MessageDialogFrame>(ScreenPrefabs.Instance.MessageDialogFrame.gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
					MessageDialog dialog = global::Util.KInstantiateUI<MessageDialog>(this.dialogPrefabs[i].gameObject, GameScreenManager.Instance.ssOverlayCanvas.gameObject, false);
					this.messageDialog.SetMessage(dialog, mn.message);
					this.messageDialog.Show(true);
					break;
				}
			}
		}
		Messenger.Instance.RemoveMessage(mn.message);
		mn.Clear();
	}

	// Token: 0x06005D03 RID: 23811 RVA: 0x00220BA4 File Offset: 0x0021EDA4
	public void OnClickNextMessage()
	{
		Notification notification2 = this.notifications.Find((Notification notification) => notification.Type == NotificationType.Messages);
		this.ShowMessage((MessageNotification)notification2);
	}

	// Token: 0x06005D04 RID: 23812 RVA: 0x00220BE8 File Offset: 0x0021EDE8
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.initTime = KTime.Instance.UnscaledGameTime;
		LocText[] componentsInChildren = this.LabelPrefab.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = this.normalColor;
		}
		componentsInChildren = this.MessagesPrefab.GetComponentsInChildren<LocText>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = this.normalColor;
		}
		base.Subscribe(Messenger.Instance.gameObject, 1558809273, new Action<object>(this.OnNewMessage));
		foreach (Message m in Messenger.Instance.Messages)
		{
			Notification notification = new MessageNotification(m);
			notification.playSound = false;
			this.notifier.Add(notification, "");
		}
	}

	// Token: 0x06005D05 RID: 23813 RVA: 0x00220CD8 File Offset: 0x0021EED8
	protected override void OnActivate()
	{
		base.OnActivate();
		this.dirty = true;
	}

	// Token: 0x06005D06 RID: 23814 RVA: 0x00220CE8 File Offset: 0x0021EEE8
	public void AddNotification(Notification notification)
	{
		if (DebugHandler.NotificationsDisabled)
		{
			return;
		}
		this.notifications.Add(notification);
		NotificationScreen.Entry entry;
		this.entriesByMessage.TryGetValue(notification.titleText, out entry);
		if (entry == null)
		{
			HierarchyReferences hierarchyReferences;
			if (notification.Type == NotificationType.Messages)
			{
				hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.MessagesPrefab, this.MessagesFolder, false);
			}
			else
			{
				hierarchyReferences = global::Util.KInstantiateUI<HierarchyReferences>(this.LabelPrefab, this.LabelsFolder, false);
			}
			Button reference = hierarchyReferences.GetReference<Button>("DismissButton");
			reference.gameObject.SetActive(notification.showDismissButton);
			if (notification.showDismissButton)
			{
				reference.onClick.AddListener(delegate()
				{
					NotificationScreen.Entry entry;
					if (!this.entriesByMessage.TryGetValue(notification.titleText, out entry))
					{
						return;
					}
					for (int i = entry.notifications.Count - 1; i >= 0; i--)
					{
						Notification notification2 = entry.notifications[i];
						MessageNotification messageNotification2 = notification2 as MessageNotification;
						if (messageNotification2 != null)
						{
							Messenger.Instance.RemoveMessage(messageNotification2.message);
						}
						notification2.Clear();
					}
				});
			}
			hierarchyReferences.GetReference<NotificationAnimator>("Animator").Begin(true);
			hierarchyReferences.gameObject.SetActive(true);
			if (notification.ToolTip != null)
			{
				ToolTip tooltip = hierarchyReferences.GetReference<ToolTip>("ToolTip");
				tooltip.OnToolTip = delegate()
				{
					tooltip.ClearMultiStringTooltip();
					tooltip.AddMultiStringTooltip(notification.ToolTip(entry.notifications, notification.tooltipData), this.TooltipTextStyle);
					return "";
				};
			}
			KImage reference2 = hierarchyReferences.GetReference<KImage>("Icon");
			LocText reference3 = hierarchyReferences.GetReference<LocText>("Text");
			Button reference4 = hierarchyReferences.GetReference<Button>("MainButton");
			ColorBlock colors = reference4.colors;
			switch (notification.Type)
			{
			case NotificationType.Bad:
			case NotificationType.DuplicantThreatening:
				colors.normalColor = this.badColorBG;
				reference3.color = this.badColor;
				reference2.color = this.badColor;
				reference2.sprite = ((notification.Type == NotificationType.Bad) ? this.icon_bad : this.icon_threatening);
				goto IL_300;
			case NotificationType.Tutorial:
				colors.normalColor = this.warningColorBG;
				reference3.color = this.warningColor;
				reference2.color = this.warningColor;
				reference2.sprite = this.icon_warning;
				goto IL_300;
			case NotificationType.Messages:
			{
				colors.normalColor = this.messageColorBG;
				reference3.color = this.messageColor;
				reference2.color = this.messageColor;
				reference2.sprite = this.icon_message;
				MessageNotification messageNotification = notification as MessageNotification;
				if (messageNotification == null)
				{
					goto IL_300;
				}
				TutorialMessage tutorialMessage = messageNotification.message as TutorialMessage;
				if (tutorialMessage != null && !string.IsNullOrEmpty(tutorialMessage.videoClipId))
				{
					reference2.sprite = this.icon_video;
					goto IL_300;
				}
				goto IL_300;
			}
			case NotificationType.Event:
				colors.normalColor = this.eventColorBG;
				reference3.color = this.eventColor;
				reference2.color = this.eventColor;
				reference2.sprite = this.icon_event;
				goto IL_300;
			}
			colors.normalColor = this.normalColorBG;
			reference3.color = this.normalColor;
			reference2.color = this.normalColor;
			reference2.sprite = this.icon_normal;
			IL_300:
			reference4.colors = colors;
			reference4.onClick.AddListener(delegate()
			{
				this.OnClick(entry);
			});
			string str = "";
			if (KTime.Instance.UnscaledGameTime - this.initTime > 5f && notification.playSound)
			{
				this.PlayDingSound(notification, 0);
			}
			else
			{
				str = "too early";
			}
			if (AudioDebug.Get().debugNotificationSounds)
			{
				global::Debug.Log("Notification(" + notification.titleText + "):" + str);
			}
			entry = new NotificationScreen.Entry(hierarchyReferences.gameObject);
			this.entriesByMessage[notification.titleText] = entry;
			this.entries.Add(entry);
		}
		entry.Add(notification);
		this.dirty = true;
		this.SortNotifications();
	}

	// Token: 0x06005D07 RID: 23815 RVA: 0x002210E4 File Offset: 0x0021F2E4
	private void SortNotifications()
	{
		this.notifications.Sort(delegate(Notification n1, Notification n2)
		{
			if (n1.Type == n2.Type)
			{
				return n1.Idx - n2.Idx;
			}
			return n1.Type - n2.Type;
		});
		foreach (Notification notification in this.notifications)
		{
			NotificationScreen.Entry entry = null;
			this.entriesByMessage.TryGetValue(notification.titleText, out entry);
			if (entry != null)
			{
				entry.label.GetComponent<RectTransform>().SetAsLastSibling();
			}
		}
	}

	// Token: 0x06005D08 RID: 23816 RVA: 0x00221184 File Offset: 0x0021F384
	private void PlayDingSound(Notification notification, int count)
	{
		string text;
		if (!this.notificationSounds.TryGetValue(notification.Type, out text))
		{
			text = "Notification";
		}
		float num;
		if (!this.timeOfLastNotification.TryGetValue(text, out num))
		{
			num = 0f;
		}
		float value = notification.volume_attenuation ? ((Time.time - num) / this.soundDecayTime) : 1f;
		this.timeOfLastNotification[text] = Time.time;
		string sound;
		if (count > 1)
		{
			sound = GlobalAssets.GetSound(text + "_AddCount", true);
			if (sound == null)
			{
				sound = GlobalAssets.GetSound(text, false);
			}
		}
		else
		{
			sound = GlobalAssets.GetSound(text, false);
		}
		if (notification.playSound)
		{
			EventInstance instance = KFMOD.BeginOneShot(sound, Vector3.zero, 1f);
			instance.setParameterByName("timeSinceLast", value, false);
			KFMOD.EndOneShot(instance);
		}
	}

	// Token: 0x06005D09 RID: 23817 RVA: 0x00221250 File Offset: 0x0021F450
	private void Update()
	{
		int i = 0;
		while (i < this.pendingNotifications.Count)
		{
			if (this.pendingNotifications[i].IsReady())
			{
				this.AddNotification(this.pendingNotifications[i]);
				this.pendingNotifications.RemoveAt(i);
			}
			else
			{
				i++;
			}
		}
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < this.notifications.Count; j++)
		{
			Notification notification = this.notifications[j];
			if (notification.Type == NotificationType.Messages)
			{
				num2++;
			}
			else
			{
				num++;
			}
			if (notification.expires && KTime.Instance.UnscaledGameTime - notification.Time > this.lifetime)
			{
				this.dirty = true;
				if (notification.Notifier == null)
				{
					this.RemovePendingNotification(notification);
				}
				else
				{
					notification.Clear();
				}
			}
		}
	}

	// Token: 0x06005D0A RID: 23818 RVA: 0x0022132C File Offset: 0x0021F52C
	private void OnClick(NotificationScreen.Entry entry)
	{
		Notification nextClickedNotification = entry.NextClickedNotification;
		base.PlaySound3D(GlobalAssets.GetSound("HUD_Click_Open", false));
		if (nextClickedNotification.customClickCallback != null)
		{
			nextClickedNotification.customClickCallback(nextClickedNotification.customClickData);
		}
		else
		{
			if (nextClickedNotification.clickFocus != null)
			{
				Vector3 position = nextClickedNotification.clickFocus.GetPosition();
				position.z = -40f;
				ClusterGridEntity component = nextClickedNotification.clickFocus.GetComponent<ClusterGridEntity>();
				KSelectable component2 = nextClickedNotification.clickFocus.GetComponent<KSelectable>();
				int myWorldId = nextClickedNotification.clickFocus.gameObject.GetMyWorldId();
				if (myWorldId != -1)
				{
					CameraController.Instance.ActiveWorldStarWipe(myWorldId, position, 10f, null);
				}
				else if (DlcManager.FeatureClusterSpaceEnabled() && component != null && component.IsVisible)
				{
					ManagementMenu.Instance.OpenClusterMap();
					ClusterMapScreen.Instance.SetTargetFocusPosition(component.Location, 0.5f);
				}
				if (component2 != null)
				{
					if (DlcManager.FeatureClusterSpaceEnabled() && component != null && component.IsVisible)
					{
						ClusterMapSelectTool.Instance.Select(component2, false);
					}
					else
					{
						SelectTool.Instance.Select(component2, false);
					}
				}
			}
			else if (nextClickedNotification.Notifier != null)
			{
				SelectTool.Instance.Select(nextClickedNotification.Notifier.GetComponent<KSelectable>(), false);
			}
			if (nextClickedNotification.Type == NotificationType.Messages)
			{
				this.ShowMessage((MessageNotification)nextClickedNotification);
			}
		}
		if (nextClickedNotification.clearOnClick)
		{
			nextClickedNotification.Clear();
		}
	}

	// Token: 0x06005D0B RID: 23819 RVA: 0x00221497 File Offset: 0x0021F697
	private void PositionLocatorIcon()
	{
	}

	// Token: 0x06005D0C RID: 23820 RVA: 0x0022149C File Offset: 0x0021F69C
	private void InitNotificationSounds()
	{
		this.notificationSounds[NotificationType.Good] = "Notification";
		this.notificationSounds[NotificationType.BadMinor] = "Notification";
		this.notificationSounds[NotificationType.Bad] = "Warning";
		this.notificationSounds[NotificationType.Neutral] = "Notification";
		this.notificationSounds[NotificationType.Tutorial] = "Notification";
		this.notificationSounds[NotificationType.Messages] = "Message";
		this.notificationSounds[NotificationType.DuplicantThreatening] = "Warning_DupeThreatening";
		this.notificationSounds[NotificationType.Event] = "Message";
	}

	// Token: 0x17000692 RID: 1682
	// (get) Token: 0x06005D0D RID: 23821 RVA: 0x00221531 File Offset: 0x0021F731
	public Color32 BadColorBG
	{
		get
		{
			return this.badColorBG;
		}
	}

	// Token: 0x06005D0E RID: 23822 RVA: 0x00221540 File Offset: 0x0021F740
	public Sprite GetNotificationIcon(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return this.icon_bad;
		case NotificationType.Tutorial:
			return this.icon_warning;
		case NotificationType.Messages:
			return this.icon_message;
		case NotificationType.DuplicantThreatening:
			return this.icon_threatening;
		case NotificationType.Event:
			return this.icon_event;
		}
		return this.icon_normal;
	}

	// Token: 0x06005D0F RID: 23823 RVA: 0x002215A0 File Offset: 0x0021F7A0
	public Color GetNotificationColour(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return this.badColor;
		case NotificationType.Tutorial:
			return this.warningColor;
		case NotificationType.Messages:
			return this.messageColor;
		case NotificationType.DuplicantThreatening:
			return this.badColor;
		case NotificationType.Event:
			return this.eventColor;
		}
		return this.normalColor;
	}

	// Token: 0x06005D10 RID: 23824 RVA: 0x00221600 File Offset: 0x0021F800
	public Color GetNotificationBGColour(NotificationType type)
	{
		switch (type)
		{
		case NotificationType.Bad:
			return this.badColorBG;
		case NotificationType.Tutorial:
			return this.warningColorBG;
		case NotificationType.Messages:
			return this.messageColorBG;
		case NotificationType.DuplicantThreatening:
			return this.badColorBG;
		case NotificationType.Event:
			return this.eventColorBG;
		}
		return this.normalColorBG;
	}

	// Token: 0x06005D11 RID: 23825 RVA: 0x00221660 File Offset: 0x0021F860
	public string GetNotificationSound(NotificationType type)
	{
		return this.notificationSounds[type];
	}

	// Token: 0x04003E89 RID: 16009
	public float lifetime;

	// Token: 0x04003E8A RID: 16010
	public bool dirty;

	// Token: 0x04003E8B RID: 16011
	public GameObject LabelPrefab;

	// Token: 0x04003E8C RID: 16012
	public GameObject LabelsFolder;

	// Token: 0x04003E8D RID: 16013
	public GameObject MessagesPrefab;

	// Token: 0x04003E8E RID: 16014
	public GameObject MessagesFolder;

	// Token: 0x04003E8F RID: 16015
	private MessageDialogFrame messageDialog;

	// Token: 0x04003E90 RID: 16016
	private float initTime;

	// Token: 0x04003E91 RID: 16017
	[MyCmpAdd]
	private Notifier notifier;

	// Token: 0x04003E92 RID: 16018
	[SerializeField]
	private List<MessageDialog> dialogPrefabs = new List<MessageDialog>();

	// Token: 0x04003E93 RID: 16019
	[SerializeField]
	private Color badColorBG;

	// Token: 0x04003E94 RID: 16020
	[SerializeField]
	private Color badColor = Color.red;

	// Token: 0x04003E95 RID: 16021
	[SerializeField]
	private Color normalColorBG;

	// Token: 0x04003E96 RID: 16022
	[SerializeField]
	private Color normalColor = Color.white;

	// Token: 0x04003E97 RID: 16023
	[SerializeField]
	private Color warningColorBG;

	// Token: 0x04003E98 RID: 16024
	[SerializeField]
	private Color warningColor;

	// Token: 0x04003E99 RID: 16025
	[SerializeField]
	private Color messageColorBG;

	// Token: 0x04003E9A RID: 16026
	[SerializeField]
	private Color messageColor;

	// Token: 0x04003E9B RID: 16027
	[SerializeField]
	private Color eventColorBG;

	// Token: 0x04003E9C RID: 16028
	[SerializeField]
	private Color eventColor;

	// Token: 0x04003E9D RID: 16029
	public Sprite icon_normal;

	// Token: 0x04003E9E RID: 16030
	public Sprite icon_warning;

	// Token: 0x04003E9F RID: 16031
	public Sprite icon_bad;

	// Token: 0x04003EA0 RID: 16032
	public Sprite icon_threatening;

	// Token: 0x04003EA1 RID: 16033
	public Sprite icon_message;

	// Token: 0x04003EA2 RID: 16034
	public Sprite icon_video;

	// Token: 0x04003EA3 RID: 16035
	public Sprite icon_event;

	// Token: 0x04003EA4 RID: 16036
	private List<Notification> pendingNotifications = new List<Notification>();

	// Token: 0x04003EA5 RID: 16037
	private List<Notification> notifications = new List<Notification>();

	// Token: 0x04003EA6 RID: 16038
	public TextStyleSetting TooltipTextStyle;

	// Token: 0x04003EA7 RID: 16039
	private Dictionary<NotificationType, string> notificationSounds = new Dictionary<NotificationType, string>();

	// Token: 0x04003EA8 RID: 16040
	private Dictionary<string, float> timeOfLastNotification = new Dictionary<string, float>();

	// Token: 0x04003EA9 RID: 16041
	private float soundDecayTime = 10f;

	// Token: 0x04003EAA RID: 16042
	private List<NotificationScreen.Entry> entries = new List<NotificationScreen.Entry>();

	// Token: 0x04003EAB RID: 16043
	private Dictionary<string, NotificationScreen.Entry> entriesByMessage = new Dictionary<string, NotificationScreen.Entry>();

	// Token: 0x02001AD3 RID: 6867
	private class Entry
	{
		// Token: 0x06009841 RID: 38977 RVA: 0x00340D10 File Offset: 0x0033EF10
		public Entry(GameObject label)
		{
			this.label = label;
		}

		// Token: 0x06009842 RID: 38978 RVA: 0x00340D2A File Offset: 0x0033EF2A
		public void Add(Notification notification)
		{
			this.notifications.Add(notification);
			this.UpdateMessage(notification, true);
		}

		// Token: 0x06009843 RID: 38979 RVA: 0x00340D40 File Offset: 0x0033EF40
		public void Remove(Notification notification)
		{
			this.notifications.Remove(notification);
			this.UpdateMessage(notification, false);
		}

		// Token: 0x06009844 RID: 38980 RVA: 0x00340D58 File Offset: 0x0033EF58
		public void UpdateMessage(Notification notification, bool playSound = true)
		{
			if (Game.IsQuitting())
			{
				return;
			}
			this.message = notification.titleText;
			if (this.notifications.Count > 1)
			{
				if (playSound && (notification.Type == NotificationType.Bad || notification.Type == NotificationType.DuplicantThreatening))
				{
					NotificationScreen.Instance.PlayDingSound(notification, this.notifications.Count);
				}
				this.message = this.message + " (" + this.notifications.Count.ToString() + ")";
			}
			if (this.label != null)
			{
				this.label.GetComponent<HierarchyReferences>().GetReference<LocText>("Text").text = this.message;
			}
		}

		// Token: 0x17000A20 RID: 2592
		// (get) Token: 0x06009845 RID: 38981 RVA: 0x00340E10 File Offset: 0x0033F010
		public Notification NextClickedNotification
		{
			get
			{
				List<Notification> list = this.notifications;
				int num = this.clickIdx;
				this.clickIdx = num + 1;
				return list[num % this.notifications.Count];
			}
		}

		// Token: 0x04007AAA RID: 31402
		public string message;

		// Token: 0x04007AAB RID: 31403
		public int clickIdx;

		// Token: 0x04007AAC RID: 31404
		public GameObject label;

		// Token: 0x04007AAD RID: 31405
		public List<Notification> notifications = new List<Notification>();
	}
}
