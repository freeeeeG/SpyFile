using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02000C15 RID: 3093
public class DoorToggleSideScreen : SideScreenContent
{
	// Token: 0x060061F1 RID: 25073 RVA: 0x002424D1 File Offset: 0x002406D1
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.InitButtons();
	}

	// Token: 0x060061F2 RID: 25074 RVA: 0x002424E0 File Offset: 0x002406E0
	private void InitButtons()
	{
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.openButton,
			state = Door.ControlState.Opened,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.OPEN_PENDING
		});
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.autoButton,
			state = Door.ControlState.Auto,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.AUTO_PENDING
		});
		this.buttonList.Add(new DoorToggleSideScreen.DoorButtonInfo
		{
			button = this.closeButton,
			state = Door.ControlState.Locked,
			currentString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE,
			pendingString = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.CLOSE_PENDING
		});
		using (List<DoorToggleSideScreen.DoorButtonInfo>.Enumerator enumerator = this.buttonList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				DoorToggleSideScreen.DoorButtonInfo info = enumerator.Current;
				info.button.onClick += delegate()
				{
					this.target.QueueStateChange(info.state);
					this.Refresh();
				};
			}
		}
	}

	// Token: 0x060061F3 RID: 25075 RVA: 0x0024263C File Offset: 0x0024083C
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<Door>() != null;
	}

	// Token: 0x060061F4 RID: 25076 RVA: 0x0024264C File Offset: 0x0024084C
	public override void SetTarget(GameObject target)
	{
		if (this.target != null)
		{
			this.ClearTarget();
		}
		base.SetTarget(target);
		this.target = target.GetComponent<Door>();
		this.accessTarget = target.GetComponent<AccessControl>();
		if (this.target == null)
		{
			return;
		}
		target.Subscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
		target.Subscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		this.Refresh();
		base.gameObject.SetActive(true);
	}

	// Token: 0x060061F5 RID: 25077 RVA: 0x002426E0 File Offset: 0x002408E0
	public override void ClearTarget()
	{
		if (this.target != null)
		{
			this.target.Unsubscribe(1734268753, new Action<object>(this.OnDoorStateChanged));
			this.target.Unsubscribe(-1525636549, new Action<object>(this.OnAccessControlChanged));
		}
		this.target = null;
	}

	// Token: 0x060061F6 RID: 25078 RVA: 0x0024273C File Offset: 0x0024093C
	private void Refresh()
	{
		string text = null;
		string text2 = null;
		if (this.buttonList == null || this.buttonList.Count == 0)
		{
			this.InitButtons();
		}
		foreach (DoorToggleSideScreen.DoorButtonInfo doorButtonInfo in this.buttonList)
		{
			if (this.target.CurrentState == doorButtonInfo.state && this.target.RequestedState == doorButtonInfo.state)
			{
				doorButtonInfo.button.isOn = true;
				text = doorButtonInfo.currentString;
				foreach (ImageToggleState imageToggleState in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState.SetActive();
					imageToggleState.SetActive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			}
			else if (this.target.RequestedState == doorButtonInfo.state)
			{
				doorButtonInfo.button.isOn = true;
				text2 = doorButtonInfo.pendingString;
				foreach (ImageToggleState imageToggleState2 in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState2.SetActive();
					imageToggleState2.SetActive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = true;
			}
			else
			{
				doorButtonInfo.button.isOn = false;
				foreach (ImageToggleState imageToggleState3 in doorButtonInfo.button.GetComponentsInChildren<ImageToggleState>())
				{
					imageToggleState3.SetInactive();
					imageToggleState3.SetInactive();
				}
				doorButtonInfo.button.GetComponent<ImageToggleStateThrobber>().enabled = false;
			}
		}
		string text3 = text;
		if (text2 != null)
		{
			text3 = string.Format(UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.PENDING_FORMAT, text3, text2);
		}
		if (this.accessTarget != null && !this.accessTarget.Online)
		{
			text3 = string.Format(UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.ACCESS_FORMAT, text3, UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.ACCESS_OFFLINE);
		}
		if (this.target.building.Def.PrefabID == POIDoorInternalConfig.ID)
		{
			text3 = UI.UISIDESCREENS.DOOR_TOGGLE_SIDE_SCREEN.POI_INTERNAL;
			using (List<DoorToggleSideScreen.DoorButtonInfo>.Enumerator enumerator = this.buttonList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DoorToggleSideScreen.DoorButtonInfo doorButtonInfo2 = enumerator.Current;
					doorButtonInfo2.button.gameObject.SetActive(false);
				}
				goto IL_2A1;
			}
		}
		foreach (DoorToggleSideScreen.DoorButtonInfo doorButtonInfo3 in this.buttonList)
		{
			bool active = doorButtonInfo3.state != Door.ControlState.Auto || this.target.allowAutoControl;
			doorButtonInfo3.button.gameObject.SetActive(active);
		}
		IL_2A1:
		this.description.text = text3;
		this.description.gameObject.SetActive(!string.IsNullOrEmpty(text3));
		this.ContentContainer.SetActive(!this.target.isSealed);
	}

	// Token: 0x060061F7 RID: 25079 RVA: 0x00242A74 File Offset: 0x00240C74
	private void OnDoorStateChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x060061F8 RID: 25080 RVA: 0x00242A7C File Offset: 0x00240C7C
	private void OnAccessControlChanged(object data)
	{
		this.Refresh();
	}

	// Token: 0x040042B7 RID: 17079
	[SerializeField]
	private KToggle openButton;

	// Token: 0x040042B8 RID: 17080
	[SerializeField]
	private KToggle autoButton;

	// Token: 0x040042B9 RID: 17081
	[SerializeField]
	private KToggle closeButton;

	// Token: 0x040042BA RID: 17082
	[SerializeField]
	private LocText description;

	// Token: 0x040042BB RID: 17083
	private Door target;

	// Token: 0x040042BC RID: 17084
	private AccessControl accessTarget;

	// Token: 0x040042BD RID: 17085
	private List<DoorToggleSideScreen.DoorButtonInfo> buttonList = new List<DoorToggleSideScreen.DoorButtonInfo>();

	// Token: 0x02001B63 RID: 7011
	private struct DoorButtonInfo
	{
		// Token: 0x04007CA5 RID: 31909
		public KToggle button;

		// Token: 0x04007CA6 RID: 31910
		public Door.ControlState state;

		// Token: 0x04007CA7 RID: 31911
		public string currentString;

		// Token: 0x04007CA8 RID: 31912
		public string pendingString;
	}
}
