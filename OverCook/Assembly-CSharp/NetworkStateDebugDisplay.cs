using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Connection;
using UnityEngine;

// Token: 0x02000888 RID: 2184
internal class NetworkStateDebugDisplay : DebugDisplay
{
	// Token: 0x06002A63 RID: 10851 RVA: 0x000C6400 File Offset: 0x000C4800
	public override void OnSetUp()
	{
		this.m_MultiplayerController = GameUtils.RequireManager<MultiplayerController>();
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_ConnectionModeCoordinator = onlinePlatformManager.OnlineMultiplayerConnectionModeCoordinator();
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x000C642A File Offset: 0x000C482A
	public override void OnUpdate()
	{
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x000C642C File Offset: 0x000C482C
	public override void OnDraw(ref Rect rect, GUIStyle style)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Server)
		{
			ServerOptions serverOptions = (ServerOptions)ConnectionModeSwitcher.GetAgentData();
			text = ", " + serverOptions.visibility.ToString();
			text2 = ", " + serverOptions.gameMode.ToString();
		}
		else if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Matchmake)
		{
			MatchmakeData matchmakeData = (MatchmakeData)ConnectionModeSwitcher.GetAgentData();
			if (ConnectionStatus.IsHost())
			{
				text = ", " + OnlineMultiplayerSessionVisibility.eMatchmaking;
			}
			text2 = ",  " + matchmakeData.gameMode.ToString();
		}
		base.DrawText(ref rect, style, string.Concat(new string[]
		{
			ConnectionModeSwitcher.GetRequestedConnectionState().ToString(),
			text,
			text2,
			", ",
			ConnectionModeSwitcher.GetStatus().GetProgress().ToString(),
			" ",
			ConnectionModeSwitcher.GetStatus().GetResult().ToString()
		}));
		base.DrawText(ref rect, style, ClientGameSetup.Mode + ", time: " + ClientTime.Time().ToString("0000.000"));
		if (ConnectionStatus.IsHost())
		{
			FastList<ConnectionStats> serverConnectionStats = this.m_MultiplayerController.GetServerConnectionStats(true);
			FastList<ConnectionStats> serverConnectionStats2 = this.m_MultiplayerController.GetServerConnectionStats(false);
			if (serverConnectionStats.Count > 0)
			{
				string empty = string.Empty;
				for (int i = 0; i < serverConnectionStats.Count; i++)
				{
					base.DrawText(ref rect, style, string.Concat(new object[]
					{
						"RLag: ",
						(serverConnectionStats._items[i].m_fLatency * 1000f).ToString("000"),
						" MaxWait: ",
						serverConnectionStats._items[i].m_fMaxTimeBetweenReceives.ToString("00.00"),
						" Sequence: I",
						serverConnectionStats._items[i].m_fIncomingSequenceNumber,
						" / O",
						serverConnectionStats._items[i].m_fOutgoingSequenceNumber
					}));
				}
				base.DrawText(ref rect, style, empty);
				empty = string.Empty;
				for (int j = 0; j < serverConnectionStats.Count; j++)
				{
					base.DrawText(ref rect, style, string.Concat(new object[]
					{
						"ULag: ",
						(serverConnectionStats2._items[j].m_fLatency * 1000f).ToString("000"),
						" MaxWait: ",
						serverConnectionStats2._items[j].m_fMaxTimeBetweenReceives.ToString("00.00"),
						" Sequence: I",
						serverConnectionStats2._items[j].m_fIncomingSequenceNumber,
						" / O",
						serverConnectionStats2._items[j].m_fOutgoingSequenceNumber
					}));
				}
			}
		}
		else if (ConnectionStatus.IsInSession())
		{
			ConnectionStats clientConnectionStats = this.m_MultiplayerController.GetClientConnectionStats(true);
			ConnectionStats clientConnectionStats2 = this.m_MultiplayerController.GetClientConnectionStats(false);
			base.DrawText(ref rect, style, string.Concat(new object[]
			{
				"RLag: ",
				(clientConnectionStats.m_fLatency * 1000f).ToString("000"),
				" MaxWait: ",
				clientConnectionStats.m_fMaxTimeBetweenReceives.ToString("00.00"),
				" Sequence: I",
				clientConnectionStats.m_fIncomingSequenceNumber,
				" / O",
				clientConnectionStats.m_fOutgoingSequenceNumber
			}));
			base.DrawText(ref rect, style, string.Concat(new object[]
			{
				"ULag: ",
				(clientConnectionStats2.m_fLatency * 1000f).ToString("000"),
				" MaxWait: ",
				clientConnectionStats2.m_fMaxTimeBetweenReceives.ToString("00.00"),
				" Sequence: I",
				clientConnectionStats2.m_fIncomingSequenceNumber,
				" / O",
				clientConnectionStats2.m_fOutgoingSequenceNumber
			}));
		}
		if (this.m_ConnectionModeCoordinator != null)
		{
			base.DrawText(ref rect, style, this.m_ConnectionModeCoordinator.DebugStatus());
		}
	}

	// Token: 0x04002170 RID: 8560
	private MultiplayerController m_MultiplayerController;

	// Token: 0x04002171 RID: 8561
	private IOnlineMultiplayerConnectionModeCoordinator m_ConnectionModeCoordinator;
}
