using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020009E5 RID: 2533
public class ClientHatMeshVisibility : ClientMeshVisibilityBase<HatMeshVisibility.VisState>
{
	// Token: 0x06003188 RID: 12680 RVA: 0x000E83C2 File Offset: 0x000E67C2
	private void Awake()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06003189 RID: 12681 RVA: 0x000E83DC File Offset: 0x000E67DC
	protected override void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		base.OnDestroy();
	}

	// Token: 0x0600318A RID: 12682 RVA: 0x000E83FC File Offset: 0x000E67FC
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			HatMeshVisibility.VisState initialVisState = base.gameObject.RequireComponent<HatMeshVisibility>().m_initialVisState;
			base.Setup(initialVisState);
		}
	}

	// Token: 0x0600318B RID: 12683 RVA: 0x000E8435 File Offset: 0x000E6835
	public void ForceSetup()
	{
		base.Setup(base.gameObject.RequireComponent<HatMeshVisibility>().m_initialVisState);
	}

	// Token: 0x0600318C RID: 12684 RVA: 0x000E844D File Offset: 0x000E684D
	public void SetVisState(HatMeshVisibility.VisState _visState)
	{
		base.SetState(_visState);
	}
}
