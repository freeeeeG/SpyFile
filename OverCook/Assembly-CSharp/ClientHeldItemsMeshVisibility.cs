using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020009E9 RID: 2537
public class ClientHeldItemsMeshVisibility : ClientMeshVisibilityBase<HeldItemsMeshVisibility.VisState>
{
	// Token: 0x06003190 RID: 12688 RVA: 0x000E8475 File Offset: 0x000E6875
	private void Awake()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003191 RID: 12689 RVA: 0x000E848F File Offset: 0x000E688F
	protected override void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		base.OnDestroy();
	}

	// Token: 0x06003192 RID: 12690 RVA: 0x000E84B0 File Offset: 0x000E68B0
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			base.Setup(HeldItemsMeshVisibility.VisState.Idle);
		}
	}

	// Token: 0x06003193 RID: 12691 RVA: 0x000E84D8 File Offset: 0x000E68D8
	public void ForceSetup()
	{
		base.Setup(HeldItemsMeshVisibility.VisState.Idle);
	}

	// Token: 0x06003194 RID: 12692 RVA: 0x000E84E1 File Offset: 0x000E68E1
	public void SetVisState(HeldItemsMeshVisibility.VisState _visState)
	{
		base.SetState(_visState);
	}
}
