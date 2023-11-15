using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02000626 RID: 1574
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/LogicAlarm")]
public class LogicAlarm : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x060027D7 RID: 10199 RVA: 0x000D891C File Offset: 0x000D6B1C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicAlarm>(-905833192, LogicAlarm.OnCopySettingsDelegate);
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x000D8938 File Offset: 0x000D6B38
	private void OnCopySettings(object data)
	{
		LogicAlarm component = ((GameObject)data).GetComponent<LogicAlarm>();
		if (component != null)
		{
			this.notificationName = component.notificationName;
			this.notificationType = component.notificationType;
			this.pauseOnNotify = component.pauseOnNotify;
			this.zoomOnNotify = component.zoomOnNotify;
			this.cooldown = component.cooldown;
			this.notificationTooltip = component.notificationTooltip;
		}
	}

	// Token: 0x060027D9 RID: 10201 RVA: 0x000D89A4 File Offset: 0x000D6BA4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.notifier = base.gameObject.AddComponent<Notifier>();
		base.Subscribe<LogicAlarm>(-801688580, LogicAlarm.OnLogicValueChangedDelegate);
		if (string.IsNullOrEmpty(this.notificationName))
		{
			this.notificationName = UI.UISIDESCREENS.LOGICALARMSIDESCREEN.NAME_DEFAULT;
		}
		if (string.IsNullOrEmpty(this.notificationTooltip))
		{
			this.notificationTooltip = UI.UISIDESCREENS.LOGICALARMSIDESCREEN.TOOLTIP_DEFAULT;
		}
		this.UpdateVisualState();
		this.UpdateNotification(false);
	}

	// Token: 0x060027DA RID: 10202 RVA: 0x000D8A20 File Offset: 0x000D6C20
	private void UpdateVisualState()
	{
		base.GetComponent<KBatchedAnimController>().Play(this.wasOn ? LogicAlarm.ON_ANIMS : LogicAlarm.OFF_ANIMS, KAnim.PlayMode.Once);
	}

	// Token: 0x060027DB RID: 10203 RVA: 0x000D8A44 File Offset: 0x000D6C44
	public void OnLogicValueChanged(object data)
	{
		LogicValueChanged logicValueChanged = (LogicValueChanged)data;
		if (logicValueChanged.portID != LogicAlarm.INPUT_PORT_ID)
		{
			return;
		}
		int newValue = logicValueChanged.newValue;
		if (LogicCircuitNetwork.IsBitActive(0, newValue))
		{
			if (!this.wasOn)
			{
				this.PushNotification();
				this.wasOn = true;
				if (this.pauseOnNotify && !SpeedControlScreen.Instance.IsPaused)
				{
					SpeedControlScreen.Instance.Pause(false, false);
				}
				if (this.zoomOnNotify)
				{
					CameraController.Instance.ActiveWorldStarWipe(base.gameObject.GetMyWorldId(), base.transform.GetPosition(), 8f, null);
				}
				this.UpdateVisualState();
				return;
			}
		}
		else if (this.wasOn)
		{
			this.wasOn = false;
			this.UpdateVisualState();
		}
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x000D8AFA File Offset: 0x000D6CFA
	private void PushNotification()
	{
		this.notification.Clear();
		this.notifier.Add(this.notification, "");
	}

	// Token: 0x060027DD RID: 10205 RVA: 0x000D8B20 File Offset: 0x000D6D20
	public void UpdateNotification(bool clear)
	{
		if (this.notification != null && clear)
		{
			this.notification.Clear();
			this.lastNotificationCreated = null;
		}
		if (this.notification != this.lastNotificationCreated || this.lastNotificationCreated == null)
		{
			this.notification = this.CreateNotification();
		}
	}

	// Token: 0x060027DE RID: 10206 RVA: 0x000D8B70 File Offset: 0x000D6D70
	public Notification CreateNotification()
	{
		base.GetComponent<KSelectable>();
		Notification result = new Notification(this.notificationName, this.notificationType, (List<Notification> n, object d) => this.notificationTooltip, null, true, 0f, null, null, null, false, false, false);
		this.lastNotificationCreated = result;
		return result;
	}

	// Token: 0x0400171B RID: 5915
	[Serialize]
	public string notificationName;

	// Token: 0x0400171C RID: 5916
	[Serialize]
	public string notificationTooltip;

	// Token: 0x0400171D RID: 5917
	[Serialize]
	public NotificationType notificationType;

	// Token: 0x0400171E RID: 5918
	[Serialize]
	public bool pauseOnNotify;

	// Token: 0x0400171F RID: 5919
	[Serialize]
	public bool zoomOnNotify;

	// Token: 0x04001720 RID: 5920
	[Serialize]
	public float cooldown;

	// Token: 0x04001721 RID: 5921
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x04001722 RID: 5922
	private bool wasOn;

	// Token: 0x04001723 RID: 5923
	private Notifier notifier;

	// Token: 0x04001724 RID: 5924
	private Notification notification;

	// Token: 0x04001725 RID: 5925
	private Notification lastNotificationCreated;

	// Token: 0x04001726 RID: 5926
	private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>(delegate(LogicAlarm component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001727 RID: 5927
	private static readonly EventSystem.IntraObjectHandler<LogicAlarm> OnLogicValueChangedDelegate = new EventSystem.IntraObjectHandler<LogicAlarm>(delegate(LogicAlarm component, object data)
	{
		component.OnLogicValueChanged(data);
	});

	// Token: 0x04001728 RID: 5928
	public static readonly HashedString INPUT_PORT_ID = new HashedString("LogicAlarmInput");

	// Token: 0x04001729 RID: 5929
	protected static readonly HashedString[] ON_ANIMS = new HashedString[]
	{
		"on_pre",
		"on_loop"
	};

	// Token: 0x0400172A RID: 5930
	protected static readonly HashedString[] OFF_ANIMS = new HashedString[]
	{
		"on_pst",
		"off"
	};
}
