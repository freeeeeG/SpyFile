using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020009D2 RID: 2514
public class ClientBodyMeshVisibility : ClientMeshVisibilityBase<BodyMeshVisibility.VisState>
{
	// Token: 0x0600313B RID: 12603 RVA: 0x000E6C43 File Offset: 0x000E5043
	private void Awake()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x0600313C RID: 12604 RVA: 0x000E6C5D File Offset: 0x000E505D
	protected override void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		base.OnDestroy();
	}

	// Token: 0x0600313D RID: 12605 RVA: 0x000E6C80 File Offset: 0x000E5080
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			BodyMeshVisibility.VisState initialVisState = base.gameObject.RequireComponent<BodyMeshVisibility>().m_initialVisState;
			base.Setup(initialVisState);
		}
	}

	// Token: 0x0600313E RID: 12606 RVA: 0x000E6CB9 File Offset: 0x000E50B9
	public void ForceSetup()
	{
		base.Setup(base.gameObject.RequireComponent<BodyMeshVisibility>().m_initialVisState);
	}

	// Token: 0x0600313F RID: 12607 RVA: 0x000E6CD1 File Offset: 0x000E50D1
	public void SetVisState(BodyMeshVisibility.VisState _visState)
	{
		base.SetState(_visState);
	}
}
