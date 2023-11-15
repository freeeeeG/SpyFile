using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200069F RID: 1695
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/Switch")]
public class Switch : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x17000317 RID: 791
	// (get) Token: 0x06002DC3 RID: 11715 RVA: 0x000F24A2 File Offset: 0x000F06A2
	public bool IsSwitchedOn
	{
		get
		{
			return this.switchedOn;
		}
	}

	// Token: 0x14000012 RID: 18
	// (add) Token: 0x06002DC4 RID: 11716 RVA: 0x000F24AC File Offset: 0x000F06AC
	// (remove) Token: 0x06002DC5 RID: 11717 RVA: 0x000F24E4 File Offset: 0x000F06E4
	public event Action<bool> OnToggle;

	// Token: 0x06002DC6 RID: 11718 RVA: 0x000F2519 File Offset: 0x000F0719
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.switchedOn = this.defaultState;
	}

	// Token: 0x06002DC7 RID: 11719 RVA: 0x000F2530 File Offset: 0x000F0730
	protected override void OnSpawn()
	{
		this.openToggleIndex = this.openSwitch.SetTarget(this);
		if (this.OnToggle != null)
		{
			this.OnToggle(this.switchedOn);
		}
		if (this.manuallyControlled)
		{
			base.Subscribe<Switch>(493375141, Switch.OnRefreshUserMenuDelegate);
		}
		this.UpdateSwitchStatus();
	}

	// Token: 0x06002DC8 RID: 11720 RVA: 0x000F2587 File Offset: 0x000F0787
	public void HandleToggle()
	{
		this.Toggle();
	}

	// Token: 0x06002DC9 RID: 11721 RVA: 0x000F258F File Offset: 0x000F078F
	public bool IsHandlerOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06002DCA RID: 11722 RVA: 0x000F2597 File Offset: 0x000F0797
	private void OnMinionToggle()
	{
		if (!DebugHandler.InstantBuildMode)
		{
			this.openSwitch.Toggle(this.openToggleIndex);
			return;
		}
		this.Toggle();
	}

	// Token: 0x06002DCB RID: 11723 RVA: 0x000F25B8 File Offset: 0x000F07B8
	protected virtual void Toggle()
	{
		this.SetState(!this.switchedOn);
	}

	// Token: 0x06002DCC RID: 11724 RVA: 0x000F25CC File Offset: 0x000F07CC
	protected virtual void SetState(bool on)
	{
		if (this.switchedOn != on)
		{
			this.switchedOn = on;
			this.UpdateSwitchStatus();
			if (this.OnToggle != null)
			{
				this.OnToggle(this.switchedOn);
			}
			if (this.manuallyControlled)
			{
				Game.Instance.userMenu.Refresh(base.gameObject);
			}
		}
	}

	// Token: 0x06002DCD RID: 11725 RVA: 0x000F2628 File Offset: 0x000F0828
	protected virtual void OnRefreshUserMenu(object data)
	{
		LocString loc_string = this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF : BUILDINGS.PREFABS.SWITCH.TURN_ON;
		LocString loc_string2 = this.switchedOn ? BUILDINGS.PREFABS.SWITCH.TURN_OFF_TOOLTIP : BUILDINGS.PREFABS.SWITCH.TURN_ON_TOOLTIP;
		Game.Instance.userMenu.AddButton(base.gameObject, new KIconButtonMenu.ButtonInfo("action_power", loc_string, new System.Action(this.OnMinionToggle), global::Action.ToggleEnabled, null, null, null, loc_string2, true), 1f);
	}

	// Token: 0x06002DCE RID: 11726 RVA: 0x000F26A4 File Offset: 0x000F08A4
	protected virtual void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.SwitchStatusActive : Db.Get().BuildingStatusItems.SwitchStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04001AFC RID: 6908
	[SerializeField]
	public bool manuallyControlled = true;

	// Token: 0x04001AFD RID: 6909
	[SerializeField]
	public bool defaultState = true;

	// Token: 0x04001AFE RID: 6910
	[Serialize]
	protected bool switchedOn = true;

	// Token: 0x04001AFF RID: 6911
	[MyCmpAdd]
	private Toggleable openSwitch;

	// Token: 0x04001B00 RID: 6912
	private int openToggleIndex;

	// Token: 0x04001B02 RID: 6914
	private static readonly EventSystem.IntraObjectHandler<Switch> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<Switch>(delegate(Switch component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
