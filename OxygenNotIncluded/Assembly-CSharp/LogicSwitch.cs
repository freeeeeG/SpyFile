using System;
using System.Collections;
using KSerialization;
using UnityEngine;

// Token: 0x02000640 RID: 1600
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
	// Token: 0x060029BB RID: 10683 RVA: 0x000E04E6 File Offset: 0x000DE6E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<LogicSwitch>(-905833192, LogicSwitch.OnCopySettingsDelegate);
	}

	// Token: 0x060029BC RID: 10684 RVA: 0x000E0500 File Offset: 0x000DE700
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.wasOn = this.switchedOn;
		this.UpdateLogicCircuit();
		base.GetComponent<KBatchedAnimController>().Play(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x060029BD RID: 10685 RVA: 0x000E0554 File Offset: 0x000DE754
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
	}

	// Token: 0x060029BE RID: 10686 RVA: 0x000E055C File Offset: 0x000DE75C
	private void OnCopySettings(object data)
	{
		LogicSwitch component = ((GameObject)data).GetComponent<LogicSwitch>();
		if (component != null && this.switchedOn != component.switchedOn)
		{
			this.switchedOn = component.switchedOn;
			this.UpdateVisualization();
			this.UpdateLogicCircuit();
		}
	}

	// Token: 0x060029BF RID: 10687 RVA: 0x000E05A4 File Offset: 0x000DE7A4
	protected override void Toggle()
	{
		base.Toggle();
		this.UpdateVisualization();
		this.UpdateLogicCircuit();
	}

	// Token: 0x060029C0 RID: 10688 RVA: 0x000E05B8 File Offset: 0x000DE7B8
	private void UpdateVisualization()
	{
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		if (this.wasOn != this.switchedOn)
		{
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
		this.wasOn = this.switchedOn;
	}

	// Token: 0x060029C1 RID: 10689 RVA: 0x000E063A File Offset: 0x000DE83A
	private void UpdateLogicCircuit()
	{
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, this.switchedOn ? 1 : 0);
	}

	// Token: 0x060029C2 RID: 10690 RVA: 0x000E0658 File Offset: 0x000DE858
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSwitchStatusActive : Db.Get().BuildingStatusItems.LogicSwitchStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x060029C3 RID: 10691 RVA: 0x000E06AB File Offset: 0x000DE8AB
	public void Sim33ms(float dt)
	{
		if (this.ToggleRequested)
		{
			this.Toggle();
			this.ToggleRequested = false;
			this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x060029C4 RID: 10692 RVA: 0x000E06DF File Offset: 0x000DE8DF
	public void SetFirstFrameCallback(System.Action ffCb)
	{
		this.firstFrameCallback = ffCb;
		base.StartCoroutine(this.RunCallback());
	}

	// Token: 0x060029C5 RID: 10693 RVA: 0x000E06F5 File Offset: 0x000DE8F5
	private IEnumerator RunCallback()
	{
		yield return null;
		if (this.firstFrameCallback != null)
		{
			this.firstFrameCallback();
			this.firstFrameCallback = null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060029C6 RID: 10694 RVA: 0x000E0704 File Offset: 0x000DE904
	public void ToggledByPlayer()
	{
		this.Toggle();
	}

	// Token: 0x060029C7 RID: 10695 RVA: 0x000E070C File Offset: 0x000DE90C
	public bool ToggledOn()
	{
		return this.switchedOn;
	}

	// Token: 0x060029C8 RID: 10696 RVA: 0x000E0714 File Offset: 0x000DE914
	public KSelectable GetSelectable()
	{
		return base.GetComponent<KSelectable>();
	}

	// Token: 0x1700028C RID: 652
	// (get) Token: 0x060029C9 RID: 10697 RVA: 0x000E071C File Offset: 0x000DE91C
	public string SideScreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.LOGICSWITCH.SIDESCREEN_TITLE";
		}
	}

	// Token: 0x1700028D RID: 653
	// (get) Token: 0x060029CA RID: 10698 RVA: 0x000E0723 File Offset: 0x000DE923
	// (set) Token: 0x060029CB RID: 10699 RVA: 0x000E072B File Offset: 0x000DE92B
	public bool ToggleRequested { get; set; }

	// Token: 0x0400187D RID: 6269
	public static readonly HashedString PORT_ID = "LogicSwitch";

	// Token: 0x0400187E RID: 6270
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x0400187F RID: 6271
	private static readonly EventSystem.IntraObjectHandler<LogicSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<LogicSwitch>(delegate(LogicSwitch component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x04001880 RID: 6272
	private bool wasOn;

	// Token: 0x04001881 RID: 6273
	private System.Action firstFrameCallback;
}
