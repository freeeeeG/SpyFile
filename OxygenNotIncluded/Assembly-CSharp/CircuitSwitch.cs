using System;
using KSerialization;
using UnityEngine;

// Token: 0x020005C9 RID: 1481
[SerializationConfig(MemberSerialization.OptIn)]
public class CircuitSwitch : Switch, IPlayerControlledToggle, ISim33ms
{
	// Token: 0x06002483 RID: 9347 RVA: 0x000C70B8 File Offset: 0x000C52B8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.Subscribe<CircuitSwitch>(-905833192, CircuitSwitch.OnCopySettingsDelegate);
	}

	// Token: 0x06002484 RID: 9348 RVA: 0x000C70D4 File Offset: 0x000C52D4
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.CircuitOnToggle;
		int cell = Grid.PosToCell(base.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, (int)this.objectLayer];
		Wire wire = (gameObject != null) ? gameObject.GetComponent<Wire>() : null;
		if (wire == null)
		{
			this.wireConnectedGUID = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, null);
		}
		this.AttachWire(wire);
		this.wasOn = this.switchedOn;
		this.UpdateCircuit(true);
		base.GetComponent<KBatchedAnimController>().Play(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
	}

	// Token: 0x06002485 RID: 9349 RVA: 0x000C71A4 File Offset: 0x000C53A4
	protected override void OnCleanUp()
	{
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
		bool switchedOn = this.switchedOn;
		this.switchedOn = true;
		this.UpdateCircuit(false);
		this.switchedOn = switchedOn;
	}

	// Token: 0x06002486 RID: 9350 RVA: 0x000C71E8 File Offset: 0x000C53E8
	private void OnCopySettings(object data)
	{
		CircuitSwitch component = ((GameObject)data).GetComponent<CircuitSwitch>();
		if (component != null)
		{
			this.switchedOn = component.switchedOn;
			this.UpdateCircuit(true);
		}
	}

	// Token: 0x06002487 RID: 9351 RVA: 0x000C7220 File Offset: 0x000C5420
	public bool IsConnected()
	{
		int cell = Grid.PosToCell(base.transform.GetPosition());
		GameObject gameObject = Grid.Objects[cell, (int)this.objectLayer];
		return gameObject != null && gameObject.GetComponent<IDisconnectable>() != null;
	}

	// Token: 0x06002488 RID: 9352 RVA: 0x000C7264 File Offset: 0x000C5464
	private void CircuitOnToggle(bool on)
	{
		this.UpdateCircuit(true);
	}

	// Token: 0x06002489 RID: 9353 RVA: 0x000C7270 File Offset: 0x000C5470
	public void AttachWire(Wire wire)
	{
		if (wire == this.attachedWire)
		{
			return;
		}
		if (this.attachedWire != null)
		{
			this.UnsubscribeFromWire(this.attachedWire);
		}
		this.attachedWire = wire;
		if (this.attachedWire != null)
		{
			this.SubscribeToWire(this.attachedWire);
			this.UpdateCircuit(true);
			this.wireConnectedGUID = base.GetComponent<KSelectable>().RemoveStatusItem(this.wireConnectedGUID, false);
			return;
		}
		if (this.wireConnectedGUID == Guid.Empty)
		{
			this.wireConnectedGUID = base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.NoWireConnected, null);
		}
	}

	// Token: 0x0600248A RID: 9354 RVA: 0x000C731A File Offset: 0x000C551A
	private void OnWireDestroyed(object data)
	{
		if (this.attachedWire != null)
		{
			this.attachedWire.Unsubscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		}
	}

	// Token: 0x0600248B RID: 9355 RVA: 0x000C7346 File Offset: 0x000C5546
	private void OnWireStateChanged(object data)
	{
		this.UpdateCircuit(true);
	}

	// Token: 0x0600248C RID: 9356 RVA: 0x000C7350 File Offset: 0x000C5550
	private void SubscribeToWire(Wire wire)
	{
		wire.Subscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		wire.Subscribe(-1735440190, new Action<object>(this.OnWireStateChanged));
		wire.Subscribe(774203113, new Action<object>(this.OnWireStateChanged));
	}

	// Token: 0x0600248D RID: 9357 RVA: 0x000C73A8 File Offset: 0x000C55A8
	private void UnsubscribeFromWire(Wire wire)
	{
		wire.Unsubscribe(1969584890, new Action<object>(this.OnWireDestroyed));
		wire.Unsubscribe(-1735440190, new Action<object>(this.OnWireStateChanged));
		wire.Unsubscribe(774203113, new Action<object>(this.OnWireStateChanged));
	}

	// Token: 0x0600248E RID: 9358 RVA: 0x000C73FC File Offset: 0x000C55FC
	private void UpdateCircuit(bool should_update_anim = true)
	{
		if (this.attachedWire != null)
		{
			if (this.switchedOn)
			{
				this.attachedWire.Connect();
			}
			else
			{
				this.attachedWire.Disconnect();
			}
		}
		if (should_update_anim && this.wasOn != this.switchedOn)
		{
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
			Game.Instance.userMenu.Refresh(base.gameObject);
		}
		this.wasOn = this.switchedOn;
	}

	// Token: 0x0600248F RID: 9359 RVA: 0x000C74C3 File Offset: 0x000C56C3
	public void Sim33ms(float dt)
	{
		if (this.ToggleRequested)
		{
			this.Toggle();
			this.ToggleRequested = false;
			this.GetSelectable().SetStatusItem(Db.Get().StatusItemCategories.Main, null, null);
		}
	}

	// Token: 0x06002490 RID: 9360 RVA: 0x000C74F7 File Offset: 0x000C56F7
	public void ToggledByPlayer()
	{
		this.Toggle();
	}

	// Token: 0x06002491 RID: 9361 RVA: 0x000C74FF File Offset: 0x000C56FF
	public bool ToggledOn()
	{
		return this.switchedOn;
	}

	// Token: 0x06002492 RID: 9362 RVA: 0x000C7507 File Offset: 0x000C5707
	public KSelectable GetSelectable()
	{
		return base.GetComponent<KSelectable>();
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x06002493 RID: 9363 RVA: 0x000C750F File Offset: 0x000C570F
	public string SideScreenTitleKey
	{
		get
		{
			return "STRINGS.BUILDINGS.PREFABS.SWITCH.SIDESCREEN_TITLE";
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x06002494 RID: 9364 RVA: 0x000C7516 File Offset: 0x000C5716
	// (set) Token: 0x06002495 RID: 9365 RVA: 0x000C751E File Offset: 0x000C571E
	public bool ToggleRequested { get; set; }

	// Token: 0x040014F0 RID: 5360
	[SerializeField]
	public ObjectLayer objectLayer;

	// Token: 0x040014F1 RID: 5361
	[MyCmpAdd]
	private CopyBuildingSettings copyBuildingSettings;

	// Token: 0x040014F2 RID: 5362
	private static readonly EventSystem.IntraObjectHandler<CircuitSwitch> OnCopySettingsDelegate = new EventSystem.IntraObjectHandler<CircuitSwitch>(delegate(CircuitSwitch component, object data)
	{
		component.OnCopySettings(data);
	});

	// Token: 0x040014F3 RID: 5363
	private Wire attachedWire;

	// Token: 0x040014F4 RID: 5364
	private Guid wireConnectedGUID;

	// Token: 0x040014F5 RID: 5365
	private bool wasOn;
}
