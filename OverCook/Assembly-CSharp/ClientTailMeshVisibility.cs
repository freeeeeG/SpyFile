using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000A25 RID: 2597
public class ClientTailMeshVisibility : ClientMeshVisibilityBase<TailMeshVisibility.VisState>
{
	// Token: 0x06003377 RID: 13175 RVA: 0x000F262B File Offset: 0x000F0A2B
	private void Awake()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003378 RID: 13176 RVA: 0x000F2645 File Offset: 0x000F0A45
	protected override void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		base.OnDestroy();
	}

	// Token: 0x06003379 RID: 13177 RVA: 0x000F2668 File Offset: 0x000F0A68
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			TailMeshVisibility.VisState initialVisState = base.gameObject.RequireComponent<TailMeshVisibility>().m_initialVisState;
			base.Setup(initialVisState);
		}
	}

	// Token: 0x0600337A RID: 13178 RVA: 0x000F26A1 File Offset: 0x000F0AA1
	public void ForceSetup()
	{
		base.Setup(base.gameObject.RequireComponent<TailMeshVisibility>().m_initialVisState);
	}

	// Token: 0x0600337B RID: 13179 RVA: 0x000F26B9 File Offset: 0x000F0AB9
	public void SetVisState(TailMeshVisibility.VisState _visState)
	{
		base.SetState(_visState);
	}
}
