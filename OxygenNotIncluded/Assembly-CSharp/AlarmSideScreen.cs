using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000BF6 RID: 3062
public class AlarmSideScreen : SideScreenContent
{
	// Token: 0x060060D3 RID: 24787 RVA: 0x0023C440 File Offset: 0x0023A640
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.nameInputField.onEndEdit += this.OnEndEditName;
		this.nameInputField.field.characterLimit = 30;
		this.tooltipInputField.onEndEdit += this.OnEndEditTooltip;
		this.tooltipInputField.field.characterLimit = 90;
		this.pauseToggle.onClick += this.TogglePause;
		this.zoomToggle.onClick += this.ToggleZoom;
		this.InitializeToggles();
	}

	// Token: 0x060060D4 RID: 24788 RVA: 0x0023C4D9 File Offset: 0x0023A6D9
	private void OnEndEditName()
	{
		this.targetAlarm.notificationName = this.nameInputField.field.text;
		this.UpdateNotification(true);
	}

	// Token: 0x060060D5 RID: 24789 RVA: 0x0023C4FD File Offset: 0x0023A6FD
	private void OnEndEditTooltip()
	{
		this.targetAlarm.notificationTooltip = this.tooltipInputField.field.text;
		this.UpdateNotification(true);
	}

	// Token: 0x060060D6 RID: 24790 RVA: 0x0023C521 File Offset: 0x0023A721
	private void TogglePause()
	{
		this.targetAlarm.pauseOnNotify = !this.targetAlarm.pauseOnNotify;
		this.pauseCheckmark.enabled = this.targetAlarm.pauseOnNotify;
		this.UpdateNotification(true);
	}

	// Token: 0x060060D7 RID: 24791 RVA: 0x0023C559 File Offset: 0x0023A759
	private void ToggleZoom()
	{
		this.targetAlarm.zoomOnNotify = !this.targetAlarm.zoomOnNotify;
		this.zoomCheckmark.enabled = this.targetAlarm.zoomOnNotify;
		this.UpdateNotification(true);
	}

	// Token: 0x060060D8 RID: 24792 RVA: 0x0023C591 File Offset: 0x0023A791
	private void SelectType(NotificationType type)
	{
		this.targetAlarm.notificationType = type;
		this.UpdateNotification(true);
		this.RefreshToggles();
	}

	// Token: 0x060060D9 RID: 24793 RVA: 0x0023C5AC File Offset: 0x0023A7AC
	private void InitializeToggles()
	{
		if (this.toggles_by_type.Count == 0)
		{
			using (List<NotificationType>.Enumerator enumerator = this.validTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NotificationType type = enumerator.Current;
					GameObject gameObject = Util.KInstantiateUI(this.typeButtonPrefab, this.typeButtonPrefab.transform.parent.gameObject, true);
					gameObject.name = "TypeButton: " + type.ToString();
					HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
					Color notificationBGColour = NotificationScreen.Instance.GetNotificationBGColour(type);
					Color notificationColour = NotificationScreen.Instance.GetNotificationColour(type);
					notificationBGColour.a = 1f;
					notificationColour.a = 1f;
					component.GetReference<KImage>("bg").color = notificationBGColour;
					component.GetReference<KImage>("icon").color = notificationColour;
					component.GetReference<KImage>("icon").sprite = NotificationScreen.Instance.GetNotificationIcon(type);
					ToolTip component2 = gameObject.GetComponent<ToolTip>();
					NotificationType type2 = type;
					if (type2 != NotificationType.Bad)
					{
						if (type2 != NotificationType.Neutral)
						{
							if (type2 == NotificationType.DuplicantThreatening)
							{
								component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.DUPLICANT_THREATENING);
							}
						}
						else
						{
							component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.NEUTRAL);
						}
					}
					else
					{
						component2.SetSimpleTooltip(UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIPS.BAD);
					}
					if (!this.toggles_by_type.ContainsKey(type))
					{
						this.toggles_by_type.Add(type, gameObject.GetComponent<MultiToggle>());
					}
					this.toggles_by_type[type].onClick = delegate()
					{
						this.SelectType(type);
					};
					for (int i = 0; i < this.toggles_by_type[type].states.Length; i++)
					{
						this.toggles_by_type[type].states[i].on_click_override_sound_path = NotificationScreen.Instance.GetNotificationSound(type);
					}
				}
			}
		}
	}

	// Token: 0x060060DA RID: 24794 RVA: 0x0023C7F0 File Offset: 0x0023A9F0
	private void RefreshToggles()
	{
		this.InitializeToggles();
		foreach (KeyValuePair<NotificationType, MultiToggle> keyValuePair in this.toggles_by_type)
		{
			if (this.targetAlarm.notificationType == keyValuePair.Key)
			{
				keyValuePair.Value.ChangeState(0);
			}
			else
			{
				keyValuePair.Value.ChangeState(1);
			}
		}
	}

	// Token: 0x060060DB RID: 24795 RVA: 0x0023C874 File Offset: 0x0023AA74
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<LogicAlarm>() != null;
	}

	// Token: 0x060060DC RID: 24796 RVA: 0x0023C882 File Offset: 0x0023AA82
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.targetAlarm = target.GetComponent<LogicAlarm>();
		this.RefreshToggles();
		this.UpdateVisuals();
	}

	// Token: 0x060060DD RID: 24797 RVA: 0x0023C8A3 File Offset: 0x0023AAA3
	private void UpdateNotification(bool clear)
	{
		this.targetAlarm.UpdateNotification(clear);
	}

	// Token: 0x060060DE RID: 24798 RVA: 0x0023C8B4 File Offset: 0x0023AAB4
	private void UpdateVisuals()
	{
		this.nameInputField.SetDisplayValue(this.targetAlarm.notificationName);
		this.tooltipInputField.SetDisplayValue(this.targetAlarm.notificationTooltip);
		this.pauseCheckmark.enabled = this.targetAlarm.pauseOnNotify;
		this.zoomCheckmark.enabled = this.targetAlarm.zoomOnNotify;
	}

	// Token: 0x040041F4 RID: 16884
	public LogicAlarm targetAlarm;

	// Token: 0x040041F5 RID: 16885
	[SerializeField]
	private KInputField nameInputField;

	// Token: 0x040041F6 RID: 16886
	[SerializeField]
	private KInputField tooltipInputField;

	// Token: 0x040041F7 RID: 16887
	[SerializeField]
	private KToggle pauseToggle;

	// Token: 0x040041F8 RID: 16888
	[SerializeField]
	private Image pauseCheckmark;

	// Token: 0x040041F9 RID: 16889
	[SerializeField]
	private KToggle zoomToggle;

	// Token: 0x040041FA RID: 16890
	[SerializeField]
	private Image zoomCheckmark;

	// Token: 0x040041FB RID: 16891
	[SerializeField]
	private GameObject typeButtonPrefab;

	// Token: 0x040041FC RID: 16892
	private List<NotificationType> validTypes = new List<NotificationType>
	{
		NotificationType.Bad,
		NotificationType.Neutral,
		NotificationType.DuplicantThreatening
	};

	// Token: 0x040041FD RID: 16893
	private Dictionary<NotificationType, MultiToggle> toggles_by_type = new Dictionary<NotificationType, MultiToggle>();
}
