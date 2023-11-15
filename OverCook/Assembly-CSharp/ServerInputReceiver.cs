using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000920 RID: 2336
public class ServerInputReceiver : ServerSynchroniserBase
{
	// Token: 0x06002DD9 RID: 11737 RVA: 0x000DA3E0 File Offset: 0x000D87E0
	public void Awake()
	{
		this.m_NetworkInputMessageReceived = new OrderedMessageReceivedCallback(this.InputServerMessageReceived);
		Mailbox.Server.RegisterForMessageType(MessageType.Input, this.m_NetworkInputMessageReceived);
		this.m_Transform = base.transform;
		this.m_PlayerIDProvider = base.GetComponent<PlayerIDProvider>();
		this.m_PlayerControls = base.GetComponent<PlayerControls>();
		this.m_gameStateChanged = new OrderedMessageReceivedCallback(this.OnGameStateChanged);
		Mailbox.Server.RegisterForMessageType(MessageType.GameState, this.m_gameStateChanged);
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x000DA45C File Offset: 0x000D885C
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.AssignedChefsToUsers)
		{
			this.Setup();
		}
	}

	// Token: 0x06002DDB RID: 11739 RVA: 0x000DA484 File Offset: 0x000D8884
	public override void StartSynchronising(Component synchronisedObject)
	{
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(base.gameObject);
		if (entry != null)
		{
			this.m_ChefEntityID = entry.m_Header.m_uEntityID;
		}
	}

	// Token: 0x06002DDC RID: 11740 RVA: 0x000DA4B4 File Offset: 0x000D88B4
	public override void OnDestroy()
	{
		Mailbox.Server.UnregisterForMessageType(MessageType.Input, this.m_NetworkInputMessageReceived);
		Mailbox.Server.UnregisterForMessageType(MessageType.GameState, this.m_gameStateChanged);
	}

	// Token: 0x06002DDD RID: 11741 RVA: 0x000DA4DC File Offset: 0x000D88DC
	private void InputServerMessageReceived(IOnlineMultiplayerSessionUserId _userID, Serialisable _serialisable)
	{
		ControllerStateMessage controllerStateMessage = _serialisable as ControllerStateMessage;
		bool flag = false;
		if (controllerStateMessage != null && controllerStateMessage.m_uChefEntityID == this.m_ChefEntityID)
		{
			if (this.m_DashNetworkButton != null)
			{
				this.m_DashNetworkButton.SetIsDown(controllerStateMessage.IsButtonDown(PlayerInputLookup.LogicalButtonID.Dash));
				flag = true;
			}
			if (this.m_CurseNetworkButton != null)
			{
				this.m_CurseNetworkButton.SetIsDown(controllerStateMessage.IsButtonDown(PlayerInputLookup.LogicalButtonID.Curse));
			}
			if (this.m_PickupNetworkButton != null)
			{
				this.m_PickupNetworkButton.SetIsDown(controllerStateMessage.IsButtonDown(PlayerInputLookup.LogicalButtonID.PickupAndDrop));
			}
			if (this.m_WorkstationNetworkButton != null)
			{
				this.m_WorkstationNetworkButton.SetIsDown(controllerStateMessage.IsButtonDown(PlayerInputLookup.LogicalButtonID.WorkstationInteract));
			}
			if (this.m_NetworkAxisX != null)
			{
				this.m_NetworkAxisX.SetValue(controllerStateMessage.m_AxisX);
				flag = true;
			}
			if (this.m_NetworkAxisY != null)
			{
				this.m_NetworkAxisY.SetValue(controllerStateMessage.m_AxisY);
				flag = true;
			}
			if (null != this.m_Transform && this.m_PlayerControls != null && this.m_PlayerControls.enabled)
			{
				this.m_Transform.rotation = controllerStateMessage.rotation;
			}
			if (flag)
			{
				if (controllerStateMessage.m_Time >= this.m_LastReceivedTimeStamp)
				{
					if (controllerStateMessage.m_Time < this.GetLastClientInputTimeStamp())
					{
						if (Time.time > this.m_lastUpdatedTime + 0.005f)
						{
							this.m_LastReceivedTimeStamp = controllerStateMessage.m_Time;
							this.m_LastReceivedTimeStampServerTime = Time.time;
						}
						else
						{
							Debug.Log("SHOULD BE IMPOSSIBLE");
						}
					}
					else if (controllerStateMessage.m_Time > this.GetLastClientInputTimeStamp() + 0.45f)
					{
						this.m_LastReceivedTimeStamp = controllerStateMessage.m_Time;
						this.m_LastReceivedTimeStampServerTime = Time.time;
					}
					else if (controllerStateMessage.m_Time > this.GetLastClientInputTimeStamp() + 0.015f)
					{
						Debug.Log("AheadMessageRecived");
						if (Time.time > this.m_lastUpdatedTime + 0.005f)
						{
							this.m_LastReceivedTimeStampServerTime -= 0.015f;
						}
						else
						{
							Debug.Log("MultiMessageRecived");
						}
					}
					else
					{
						this.m_LastReceivedTimeStamp = controllerStateMessage.m_Time;
						this.m_LastReceivedTimeStampServerTime = Time.time;
					}
					this.m_lastUpdatedTime = Time.time;
				}
			}
			if (this.m_PlayerControls != null && !this.m_PlayerIDProvider.IsLocallyControlled())
			{
				this.m_PlayerControls.SetDirectlyUnderPlayerControl(controllerStateMessage.m_underControl);
			}
		}
	}

	// Token: 0x06002DDE RID: 11742 RVA: 0x000DA754 File Offset: 0x000D8B54
	private void Setup()
	{
		bool flag = this.m_PlayerIDProvider.IsLocallyControlled();
		PlayerControls component = base.GetComponent<PlayerControls>();
		if (component != null && !flag)
		{
			PlayerInputLookup.Player id = this.m_PlayerIDProvider.GetID();
			PlayerControls.ControlSchemeData controlSchemeData = new PlayerControls.ControlSchemeData(id, component);
			bool buttonRequiresAppFocus = this.m_PlayerIDProvider.IsLocallyControlled();
			NetworkLogicalButton networkLogicalButton = new NetworkLogicalButton(controlSchemeData.m_dashButton, PlayerInputLookup.LogicalButtonID.Dash, buttonRequiresAppFocus);
			controlSchemeData.m_dashButton = networkLogicalButton;
			this.m_DashNetworkButton = networkLogicalButton;
			networkLogicalButton = new NetworkLogicalButton(controlSchemeData.m_curseButton, PlayerInputLookup.LogicalButtonID.Curse, buttonRequiresAppFocus);
			controlSchemeData.m_curseButton = networkLogicalButton;
			this.m_CurseNetworkButton = networkLogicalButton;
			networkLogicalButton = new NetworkLogicalButton(controlSchemeData.m_pickupButton, PlayerInputLookup.LogicalButtonID.PickupAndDrop, buttonRequiresAppFocus);
			controlSchemeData.m_pickupButton = networkLogicalButton;
			this.m_PickupNetworkButton = networkLogicalButton;
			networkLogicalButton = new NetworkLogicalButton(controlSchemeData.m_worksurfaceUseButton, PlayerInputLookup.LogicalButtonID.WorkstationInteract, buttonRequiresAppFocus);
			controlSchemeData.m_worksurfaceUseButton = networkLogicalButton;
			this.m_WorkstationNetworkButton = networkLogicalButton;
			NetworkLogicalValue networkLogicalValue = new NetworkLogicalValue(controlSchemeData.m_moveX, PlayerInputLookup.LogicalValueID.MovementX);
			controlSchemeData.m_moveX = networkLogicalValue;
			this.m_NetworkAxisX = networkLogicalValue;
			networkLogicalValue = new NetworkLogicalValue(controlSchemeData.m_moveY, PlayerInputLookup.LogicalValueID.MovementY);
			controlSchemeData.m_moveY = networkLogicalValue;
			this.m_NetworkAxisY = networkLogicalValue;
			component.SetControlSchemeData(controlSchemeData);
			component.SetServerControlled(true);
		}
	}

	// Token: 0x06002DDF RID: 11743 RVA: 0x000DA872 File Offset: 0x000D8C72
	public float GetLastClientInputTimeStamp()
	{
		return this.m_LastReceivedTimeStamp + (Time.time - this.m_LastReceivedTimeStampServerTime);
	}

	// Token: 0x040024F0 RID: 9456
	private OrderedMessageReceivedCallback m_NetworkInputMessageReceived;

	// Token: 0x040024F1 RID: 9457
	private uint m_ChefEntityID;

	// Token: 0x040024F2 RID: 9458
	private NetworkLogicalButton m_DashNetworkButton;

	// Token: 0x040024F3 RID: 9459
	private NetworkLogicalButton m_CurseNetworkButton;

	// Token: 0x040024F4 RID: 9460
	private NetworkLogicalButton m_PickupNetworkButton;

	// Token: 0x040024F5 RID: 9461
	private NetworkLogicalButton m_WorkstationNetworkButton;

	// Token: 0x040024F6 RID: 9462
	private NetworkLogicalValue m_NetworkAxisX;

	// Token: 0x040024F7 RID: 9463
	private NetworkLogicalValue m_NetworkAxisY;

	// Token: 0x040024F8 RID: 9464
	private Transform m_Transform;

	// Token: 0x040024F9 RID: 9465
	private PlayerIDProvider m_PlayerIDProvider;

	// Token: 0x040024FA RID: 9466
	private PlayerControls m_PlayerControls;

	// Token: 0x040024FB RID: 9467
	private float m_LastReceivedTimeStamp;

	// Token: 0x040024FC RID: 9468
	private float m_LastReceivedTimeStampServerTime;

	// Token: 0x040024FD RID: 9469
	private OrderedMessageReceivedCallback m_gameStateChanged;

	// Token: 0x040024FE RID: 9470
	private float m_lastUpdatedTime;
}
