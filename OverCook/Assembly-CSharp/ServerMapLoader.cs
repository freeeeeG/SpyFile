using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000893 RID: 2195
public class ServerMapLoader : MonoBehaviour
{
	// Token: 0x06002ABA RID: 10938 RVA: 0x000C814C File Offset: 0x000C654C
	private void Awake()
	{
		this.m_State = GameState.LoadMap;
	}

	// Token: 0x06002ABB RID: 10939 RVA: 0x000C8156 File Offset: 0x000C6556
	private void Start()
	{
	}

	// Token: 0x06002ABC RID: 10940 RVA: 0x000C8158 File Offset: 0x000C6558
	private void Update()
	{
		switch (this.m_State)
		{
		case GameState.LoadMap:
			if (this.AreAllUsersInGameState(GameState.LoadedMap))
			{
				this.ChangeGameState(GameState.MapScanNetworkEntities);
			}
			break;
		case GameState.MapScanNetworkEntities:
			if (this.AreAllUsersInGameState(GameState.MapScannedNetworkEntities))
			{
				this.ChangeGameState(GameState.MapStartSynchronising);
			}
			break;
		case GameState.MapStartSynchronising:
			if (this.AreAllUsersInGameState(GameState.MapStartedSyncronising))
			{
				this.ChangeGameState(GameState.MapStartEntities);
			}
			break;
		case GameState.MapStartEntities:
			if (this.AreAllUsersInGameState(GameState.MapStartedEntities))
			{
				this.ChangeGameState(GameState.RunMapUnfoldRoutine);
			}
			break;
		}
	}

	// Token: 0x06002ABD RID: 10941 RVA: 0x000C81FE File Offset: 0x000C65FE
	private void OnDestroy()
	{
	}

	// Token: 0x06002ABE RID: 10942 RVA: 0x000C8200 File Offset: 0x000C6600
	private void ChangeGameState(GameState state)
	{
		UserSystemUtils.ChangeGameState(state, null);
		this.m_State = state;
	}

	// Token: 0x06002ABF RID: 10943 RVA: 0x000C8210 File Offset: 0x000C6610
	private bool AreAllUsersInGameState(GameState state)
	{
		return UserSystemUtils.AreAllUsersInGameState(ServerUserSystem.m_Users, state);
	}

	// Token: 0x040021BF RID: 8639
	private GameState m_State;
}
