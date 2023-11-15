using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer;

// Token: 0x02000843 RID: 2115
public class ConnectionModeSwitcher
{
	// Token: 0x060028C2 RID: 10434 RVA: 0x000BF524 File Offset: 0x000BD924
	public static void Initialise(Server server, Client client)
	{
		ConnectionModeSwitcher.m_LocalServer = server;
		ConnectionModeSwitcher.m_LocalClient = client;
		ConnectionModeSwitcher.m_AgentMap.Add(NetConnectionState.Offline, ConnectionModeSwitcher.m_OfflineAgent);
		ConnectionModeSwitcher.m_AgentMap.Add(NetConnectionState.Server, ConnectionModeSwitcher.m_ServerAgent);
		ConnectionModeSwitcher.m_AgentMap.Add(NetConnectionState.AcceptInvite, ConnectionModeSwitcher.m_AcceptInviteAgent);
		ConnectionModeSwitcher.m_AgentMap.Add(NetConnectionState.JoinEnumeratedRoom, ConnectionModeSwitcher.m_JoinEnumeratedRoomAgent);
		ConnectionModeSwitcher.m_AgentMap.Add(NetConnectionState.Matchmake, ConnectionModeSwitcher.m_MatchmakingAgent);
	}

	// Token: 0x060028C3 RID: 10435 RVA: 0x000BF590 File Offset: 0x000BD990
	public static bool RequestConnectionState(NetConnectionState state, object data = null, GenericVoid<IConnectionModeSwitchStatus> callback = null)
	{
		ConnectionModeAgent connectionModeAgent = ConnectionModeSwitcher.m_AgentMap[state];
		if (connectionModeAgent != ConnectionModeSwitcher.m_CurrentAgent)
		{
			MultiplayerController multiplayerController = GameUtils.RequireManager<MultiplayerController>();
			multiplayerController.StopSynchronisation();
			if (ConnectionModeSwitcher.m_CurrentAgent != null)
			{
				ConnectionModeSwitcher.m_CurrentAgent.Stop();
			}
			ConnectionModeSwitcher.m_CurrentAgent = connectionModeAgent;
			ConnectionModeSwitcher.m_CurrentAgentState = state;
		}
		return ConnectionModeSwitcher.m_CurrentAgent.Start(ConnectionModeSwitcher.m_LocalServer, ConnectionModeSwitcher.m_LocalClient, data, callback);
	}

	// Token: 0x060028C4 RID: 10436 RVA: 0x000BF5F6 File Offset: 0x000BD9F6
	public static void InvalidateCallback(GenericVoid<IConnectionModeSwitchStatus> callback)
	{
		ConnectionModeSwitcher.m_CurrentAgent.InvalidateCallback(callback);
	}

	// Token: 0x060028C5 RID: 10437 RVA: 0x000BF603 File Offset: 0x000BDA03
	public static NetConnectionState GetRequestedConnectionState()
	{
		return ConnectionModeSwitcher.m_CurrentAgentState;
	}

	// Token: 0x060028C6 RID: 10438 RVA: 0x000BF60A File Offset: 0x000BDA0A
	public static IConnectionModeSwitchStatus GetStatus()
	{
		return ConnectionModeSwitcher.m_CurrentAgent.GetStatus();
	}

	// Token: 0x060028C7 RID: 10439 RVA: 0x000BF616 File Offset: 0x000BDA16
	public static object GetAgentData()
	{
		return ConnectionModeSwitcher.m_CurrentAgent.GetAgentData();
	}

	// Token: 0x060028C8 RID: 10440 RVA: 0x000BF622 File Offset: 0x000BDA22
	public static void Update()
	{
		if (ConnectionModeSwitcher.m_CurrentAgent != null)
		{
			ConnectionModeSwitcher.m_CurrentAgent.Update();
		}
	}

	// Token: 0x0400203A RID: 8250
	private static OfflineAgent m_OfflineAgent = new OfflineAgent();

	// Token: 0x0400203B RID: 8251
	private static ServerAgent m_ServerAgent = new ServerAgent();

	// Token: 0x0400203C RID: 8252
	private static AcceptInviteAgent m_AcceptInviteAgent = new AcceptInviteAgent();

	// Token: 0x0400203D RID: 8253
	private static JoinEnumeratedRoomAgent m_JoinEnumeratedRoomAgent = new JoinEnumeratedRoomAgent();

	// Token: 0x0400203E RID: 8254
	private static MatchmakingAgent m_MatchmakingAgent = new MatchmakingAgent();

	// Token: 0x0400203F RID: 8255
	private static Dictionary<NetConnectionState, ConnectionModeAgent> m_AgentMap = new Dictionary<NetConnectionState, ConnectionModeAgent>(5);

	// Token: 0x04002040 RID: 8256
	private static ConnectionModeAgent m_CurrentAgent = ConnectionModeSwitcher.m_OfflineAgent;

	// Token: 0x04002041 RID: 8257
	private static NetConnectionState m_CurrentAgentState = NetConnectionState.Offline;

	// Token: 0x04002042 RID: 8258
	private static Server m_LocalServer = null;

	// Token: 0x04002043 RID: 8259
	private static Client m_LocalClient = null;
}
