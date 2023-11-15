using System;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x020005AB RID: 1451
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/BuildingEnabledButton")]
public class BuildingEnabledButton : KMonoBehaviour, ISaveLoadable, IToggleHandler
{
	// Token: 0x170001A5 RID: 421
	// (get) Token: 0x0600237F RID: 9087 RVA: 0x000C27D3 File Offset: 0x000C09D3
	// (set) Token: 0x06002380 RID: 9088 RVA: 0x000C27F8 File Offset: 0x000C09F8
	public bool IsEnabled
	{
		get
		{
			return this.Operational != null && this.Operational.GetFlag(BuildingEnabledButton.EnabledFlag);
		}
		set
		{
			this.Operational.SetFlag(BuildingEnabledButton.EnabledFlag, value);
			Game.Instance.userMenu.Refresh(base.gameObject);
			this.buildingEnabled = value;
			base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.BuildingDisabled, !this.buildingEnabled, null);
			base.Trigger(1088293757, this.buildingEnabled);
		}
	}

	// Token: 0x170001A6 RID: 422
	// (get) Token: 0x06002381 RID: 9089 RVA: 0x000C286D File Offset: 0x000C0A6D
	public bool WaitingForDisable
	{
		get
		{
			return this.IsEnabled && this.Toggleable.IsToggleQueued(this.ToggleIdx);
		}
	}

	// Token: 0x06002382 RID: 9090 RVA: 0x000C288A File Offset: 0x000C0A8A
	protected override void OnPrefabInit()
	{
		this.ToggleIdx = this.Toggleable.SetTarget(this);
		base.Subscribe<BuildingEnabledButton>(493375141, BuildingEnabledButton.OnRefreshUserMenuDelegate);
	}

	// Token: 0x06002383 RID: 9091 RVA: 0x000C28AF File Offset: 0x000C0AAF
	protected override void OnSpawn()
	{
		this.IsEnabled = this.buildingEnabled;
		if (this.queuedToggle)
		{
			this.OnMenuToggle();
		}
	}

	// Token: 0x06002384 RID: 9092 RVA: 0x000C28CB File Offset: 0x000C0ACB
	public void HandleToggle()
	{
		this.queuedToggle = false;
		Prioritizable.RemoveRef(base.gameObject);
		this.OnToggle();
	}

	// Token: 0x06002385 RID: 9093 RVA: 0x000C28E5 File Offset: 0x000C0AE5
	public bool IsHandlerOn()
	{
		return this.IsEnabled;
	}

	// Token: 0x06002386 RID: 9094 RVA: 0x000C28ED File Offset: 0x000C0AED
	private void OnToggle()
	{
		this.IsEnabled = !this.IsEnabled;
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06002387 RID: 9095 RVA: 0x000C2914 File Offset: 0x000C0B14
	private void OnMenuToggle()
	{
		if (!this.Toggleable.IsToggleQueued(this.ToggleIdx))
		{
			if (this.IsEnabled)
			{
				base.Trigger(2108245096, "BuildingDisabled");
			}
			this.queuedToggle = true;
			Prioritizable.AddRef(base.gameObject);
		}
		else
		{
			this.queuedToggle = false;
			Prioritizable.RemoveRef(base.gameObject);
		}
		this.Toggleable.Toggle(this.ToggleIdx);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x06002388 RID: 9096 RVA: 0x000C2998 File Offset: 0x000C0B98
	private void OnRefreshUserMenu(object data)
	{
		bool isEnabled = this.IsEnabled;
		bool flag = this.Toggleable.IsToggleQueued(this.ToggleIdx);
		KIconButtonMenu.ButtonInfo button;
		if ((isEnabled && !flag) || (!isEnabled && flag))
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.ENABLEBUILDING.NAME, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP, true);
		}
		else
		{
			button = new KIconButtonMenu.ButtonInfo("action_building_disabled", UI.USERMENUACTIONS.ENABLEBUILDING.NAME_OFF, new System.Action(this.OnMenuToggle), global::Action.ToggleEnabled, null, null, null, UI.USERMENUACTIONS.ENABLEBUILDING.TOOLTIP_OFF, true);
		}
		Game.Instance.userMenu.AddButton(base.gameObject, button, 1f);
	}

	// Token: 0x04001443 RID: 5187
	[MyCmpAdd]
	private Toggleable Toggleable;

	// Token: 0x04001444 RID: 5188
	[MyCmpReq]
	private Operational Operational;

	// Token: 0x04001445 RID: 5189
	private int ToggleIdx;

	// Token: 0x04001446 RID: 5190
	[Serialize]
	private bool buildingEnabled = true;

	// Token: 0x04001447 RID: 5191
	[Serialize]
	private bool queuedToggle;

	// Token: 0x04001448 RID: 5192
	public static readonly Operational.Flag EnabledFlag = new Operational.Flag("building_enabled", Operational.Flag.Type.Functional);

	// Token: 0x04001449 RID: 5193
	private static readonly EventSystem.IntraObjectHandler<BuildingEnabledButton> OnRefreshUserMenuDelegate = new EventSystem.IntraObjectHandler<BuildingEnabledButton>(delegate(BuildingEnabledButton component, object data)
	{
		component.OnRefreshUserMenu(data);
	});
}
