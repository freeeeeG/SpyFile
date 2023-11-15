using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200090C RID: 2316
public class ClientInputTransmitter : ClientSynchroniserBase
{
	// Token: 0x06002D63 RID: 11619 RVA: 0x000D7108 File Offset: 0x000D5508
	public void Awake()
	{
		this.m_CanButtonBePressed = new Generic<bool>(base.GetComponent<PlayerControls>().CanButtonBePressed);
		this.m_Transform = base.transform;
	}

	// Token: 0x06002D64 RID: 11620 RVA: 0x000D712D File Offset: 0x000D552D
	protected override void OnDestroy()
	{
		this.m_SerialisationEntry = null;
		this.m_CanButtonBePressed = null;
		base.OnDestroy();
	}

	// Token: 0x06002D65 RID: 11621 RVA: 0x000D7144 File Offset: 0x000D5544
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_SerialisationEntry = EntitySerialisationRegistry.GetEntry(base.gameObject);
		if (this.m_SerialisationEntry != null)
		{
			this.m_ControllerState.m_uChefEntityID = this.m_SerialisationEntry.m_Header.m_uEntityID;
			this.m_PrevControllerState.m_uChefEntityID = this.m_SerialisationEntry.m_Header.m_uEntityID;
		}
		this.m_bForceSend = true;
		this.m_bIsServer = (base.GetComponent<ServerInputReceiver>() != null);
	}

	// Token: 0x06002D66 RID: 11622 RVA: 0x000D71BC File Offset: 0x000D55BC
	public override void UpdateSynchronising()
	{
		if (this.m_bIsServer)
		{
			return;
		}
		if (this.IsInControl() && this.m_Pad != null)
		{
			float value = this.m_Pad.m_X.GetValue();
			float value2 = this.m_Pad.m_Y.GetValue();
			bool flag = this.m_Pad.m_Dash.IsDown();
			bool flag2 = this.m_Pad.m_Curse.IsDown();
			bool flag3 = this.m_Pad.m_Pickup.IsDown();
			bool flag4 = this.m_Pad.m_Worksurface.IsDown();
			this.m_ControllerState.m_AxisX = value;
			this.m_ControllerState.m_AxisY = value2;
			if (flag != this.m_PrevControllerState.IsButtonDown(PlayerInputLookup.LogicalButtonID.Dash))
			{
				this.m_ControllerState.SetButtonPressed(PlayerInputLookup.LogicalButtonID.Dash, flag);
			}
			if (flag2 != this.m_PrevControllerState.IsButtonDown(PlayerInputLookup.LogicalButtonID.Curse))
			{
				this.m_ControllerState.SetButtonPressed(PlayerInputLookup.LogicalButtonID.Curse, flag2);
			}
			if (flag3 != this.m_PrevControllerState.IsButtonDown(PlayerInputLookup.LogicalButtonID.PickupAndDrop))
			{
				this.m_ControllerState.SetButtonPressed(PlayerInputLookup.LogicalButtonID.PickupAndDrop, flag3);
			}
			if (flag4 != this.m_PrevControllerState.IsButtonDown(PlayerInputLookup.LogicalButtonID.WorkstationInteract))
			{
				this.m_ControllerState.SetButtonPressed(PlayerInputLookup.LogicalButtonID.WorkstationInteract, flag4);
			}
			this.m_ControllerState.m_underControl = this.m_playerControls.GetDirectlyUnderPlayerControl();
			if (this.m_PrevControllerState.IsDifferent(this.m_ControllerState) || this.m_bForceSend)
			{
				this.m_ControllerState.rotation = this.m_Transform.rotation;
				this.m_ControllerState.m_Time = Time.time;
				ClientMessenger.ControllerState(this.m_ControllerState);
				this.m_PrevControllerState.Copy(this.m_ControllerState);
				this.m_bForceSend = false;
			}
		}
	}

	// Token: 0x06002D67 RID: 11623 RVA: 0x000D736D File Offset: 0x000D576D
	public bool IsInControl()
	{
		return this.m_bInControl;
	}

	// Token: 0x06002D68 RID: 11624 RVA: 0x000D7378 File Offset: 0x000D5778
	public void Setup()
	{
		PlayerIDProvider component = base.GetComponent<PlayerIDProvider>();
		PlayerInputLookup.Player id = component.GetID();
		this.m_bInControl = (id != PlayerInputLookup.Player.Count);
		if (this.m_bInControl)
		{
			this.m_Pad = new ClientInputTransmitter.Pad();
			this.m_Pad.m_Y = this.GetGated(PlayerInputLookup.GetValue(PlayerInputLookup.LogicalValueID.MovementY, id));
			this.m_Pad.m_X = this.GetGated(PlayerInputLookup.GetValue(PlayerInputLookup.LogicalValueID.MovementX, id));
			this.m_Pad.m_Dash = this.GetGated(PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.Dash, id));
			this.m_Pad.m_Curse = this.GetGated(PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.Curse, id));
			this.m_Pad.m_Worksurface = this.GetGated(PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.WorkstationInteract, id));
			this.m_Pad.m_Pickup = this.GetGated(PlayerInputLookup.GetButton(PlayerInputLookup.LogicalButtonID.PickupAndDrop, id));
			this.m_playerControls = base.GetComponent<PlayerControls>();
			if (this.m_playerControls != null)
			{
				PlayerControls.ControlSchemeData controlSchemeData = new PlayerControls.ControlSchemeData(id, this.m_playerControls);
				controlSchemeData.m_dashButton = this.m_Pad.m_Dash;
				controlSchemeData.m_curseButton = this.m_Pad.m_Curse;
				controlSchemeData.m_pickupButton = this.m_Pad.m_Pickup;
				controlSchemeData.m_worksurfaceUseButton = this.m_Pad.m_Worksurface;
				controlSchemeData.m_moveX = this.m_Pad.m_X;
				controlSchemeData.m_moveY = this.m_Pad.m_Y;
				this.m_playerControls.SetControlSchemeData(controlSchemeData);
			}
		}
	}

	// Token: 0x06002D69 RID: 11625 RVA: 0x000D74E4 File Offset: 0x000D58E4
	private ILogicalButton GetGated(ILogicalButton _toProtect)
	{
		return new GateLogicalButton(_toProtect, this.m_CanButtonBePressed);
	}

	// Token: 0x06002D6A RID: 11626 RVA: 0x000D74F2 File Offset: 0x000D58F2
	private ILogicalValue GetGated(ILogicalValue _toProtect)
	{
		return new GateLogicalValue(_toProtect, this.m_CanButtonBePressed);
	}

	// Token: 0x0400246C RID: 9324
	private ClientInputTransmitter.Pad m_Pad;

	// Token: 0x0400246D RID: 9325
	private bool m_bForceSend;

	// Token: 0x0400246E RID: 9326
	private bool m_bInControl;

	// Token: 0x0400246F RID: 9327
	private EntitySerialisationEntry m_SerialisationEntry;

	// Token: 0x04002470 RID: 9328
	private ControllerStateMessage m_PrevControllerState = new ControllerStateMessage();

	// Token: 0x04002471 RID: 9329
	private ControllerStateMessage m_ControllerState = new ControllerStateMessage();

	// Token: 0x04002472 RID: 9330
	private Generic<bool> m_CanButtonBePressed;

	// Token: 0x04002473 RID: 9331
	private Transform m_Transform;

	// Token: 0x04002474 RID: 9332
	private PlayerControls m_playerControls;

	// Token: 0x04002475 RID: 9333
	private bool m_bIsServer;

	// Token: 0x0200090D RID: 2317
	private class Pad
	{
		// Token: 0x04002476 RID: 9334
		public ILogicalValue m_Y;

		// Token: 0x04002477 RID: 9335
		public ILogicalValue m_X;

		// Token: 0x04002478 RID: 9336
		public ILogicalButton m_Dash;

		// Token: 0x04002479 RID: 9337
		public ILogicalButton m_Curse;

		// Token: 0x0400247A RID: 9338
		public ILogicalButton m_Worksurface;

		// Token: 0x0400247B RID: 9339
		public ILogicalButton m_Pickup;
	}
}
