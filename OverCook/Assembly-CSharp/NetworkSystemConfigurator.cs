using System;
using Team17.Online;
using Team17.Online.Multiplayer;
using Team17.Online.Multiplayer.Connection;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x02000848 RID: 2120
public static class NetworkSystemConfigurator
{
	// Token: 0x060028DA RID: 10458 RVA: 0x000BFB52 File Offset: 0x000BDF52
	public static void Server(Server server, Client client, OnlineMultiplayerLocalUserId localUserId)
	{
		ClientUserSystem.ClearAvatarImageCache();
		NetworkSystemConfigurator.SetupServer(server, client);
		ServerGameSetup.Mode = ServerSessionPropertyValuesProvider.GetGameMode();
		server.GetUserSystem().ResetUsersToOnlineState(localUserId);
		Mailbox.Server.ResetSequenceNumbers();
		Mailbox.Client.ResetSequenceNumbers();
	}

	// Token: 0x060028DB RID: 10459 RVA: 0x000BFB8C File Offset: 0x000BDF8C
	public static void Client(Client client, JoinData joinData, IOnlineMultiplayerSessionCoordinator session)
	{
		ClientUserSystem.ClearAvatarImageCache();
		RemoteConnection remoteConnection = new RemoteConnection();
		remoteConnection.Initialise(client, session, UserSystemUtils.GetSessionHostUser());
		client.Initialise(true);
		client.HandleOutgoingServerConnectionAccepted(remoteConnection);
		ClientMessenger.OnClientStarted(client);
		MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
		multiplayerController.SwitchNodeType(MultiplayerController.NodeType.Client);
		if (joinData != null)
		{
			ClientUserSystem.SetMachineId(joinData.machine);
			client.HandleManuallyDeserialisedMessage(UserSystemUtils.GetSessionHostUser(), MessageType.TimeSync, joinData.timeSync);
			client.HandleManuallyDeserialisedMessage(UserSystemUtils.GetSessionHostUser(), MessageType.UsersChanged, joinData.usersChanged);
			client.HandleManuallyDeserialisedMessage(UserSystemUtils.GetSessionHostUser(), MessageType.GameSetup, joinData.gameSetup);
		}
		Mailbox.Server.ResetSequenceNumbers();
		Mailbox.Client.ResetSequenceNumbers();
		ServerGameSetup.BecomeClient();
	}

	// Token: 0x060028DC RID: 10460 RVA: 0x000BFC34 File Offset: 0x000BE034
	public static void Offline(Server server, Client client)
	{
		ClientUserSystem.ClearAvatarImageCache();
		NetworkSystemConfigurator.SetupServer(server, client);
		server.GetUserSystem().ResetUsersToOfflineState();
		Mailbox.Server.ResetSequenceNumbers();
		Mailbox.Client.ResetSequenceNumbers();
	}

	// Token: 0x060028DD RID: 10461 RVA: 0x000BFC64 File Offset: 0x000BE064
	private static void SetupServer(Server server, Client client)
	{
		ServerUserSystem.s_LocalMachineId = User.MachineID.One;
		ClientUserSystem.SetMachineId(User.MachineID.One);
		LocalLoopbackConnection localLoopbackConnection = new LocalLoopbackConnection();
		LocalLoopbackConnection localLoopbackConnection2 = new LocalLoopbackConnection();
		localLoopbackConnection.Initialise(server, localLoopbackConnection2);
		localLoopbackConnection2.Initialise(client, localLoopbackConnection);
		server.EnsureLocalLoopbackClientConnection(localLoopbackConnection);
		client.HandleOutgoingServerConnectionAccepted(localLoopbackConnection2);
		ServerMessenger.OnServerStarted(server);
		ClientMessenger.OnClientStarted(client);
		MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
		multiplayerController.SwitchNodeType(MultiplayerController.NodeType.Server);
		ServerTime.StartTime();
	}
}
