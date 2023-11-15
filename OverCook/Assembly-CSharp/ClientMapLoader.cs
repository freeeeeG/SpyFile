using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200088F RID: 2191
public class ClientMapLoader : MonoBehaviour
{
	// Token: 0x06002AA5 RID: 10917 RVA: 0x000C7CFB File Offset: 0x000C60FB
	private void Awake()
	{
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x000C7D15 File Offset: 0x000C6115
	private void OnDestroy()
	{
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x000C7D30 File Offset: 0x000C6130
	private void Update()
	{
		GameState gameState = this.m_GameState;
		if (gameState != GameState.MapScanNetworkEntities)
		{
			if (gameState == GameState.MapStartEntities)
			{
				if (this.CheckStarted())
				{
					this.StartEntities();
				}
				else
				{
					this.m_fSynchroniserStartupTimer += Time.deltaTime;
					if (this.m_fSynchroniserStartupTimer >= 1f)
					{
						this.StartEntities();
					}
				}
			}
		}
		else if (this.CheckScanned())
		{
			this.ScannedEntities();
		}
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x000C7DB7 File Offset: 0x000C61B7
	public void Initialise(Client client, MultiplayerController controller)
	{
		this.m_Client = client;
		this.m_Controller = controller;
		ClientMessenger.GameState(GameState.LoadedMap);
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x000C7DCE File Offset: 0x000C61CE
	private void ScannedEntities()
	{
		ClientMessenger.GameState(GameState.MapScannedNetworkEntities);
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x000C7DD7 File Offset: 0x000C61D7
	private bool CheckScanned()
	{
		return !this.m_Controller.ScanActive && !ComponentCacheRegistry.ScanActive;
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x000C7DF8 File Offset: 0x000C61F8
	private void StartEntities()
	{
		Camera main = Camera.main;
		WorldMapCamera worldMapCamera = main.gameObject.RequireComponent<WorldMapCamera>();
		worldMapCamera.Initialise();
		ClientMessenger.GameState(GameState.MapStartedEntities);
		this.m_GameState = GameState.MapStartedEntities;
	}

	// Token: 0x06002AAC RID: 10924 RVA: 0x000C7E2C File Offset: 0x000C622C
	private bool CheckStarted()
	{
		bool flag = true;
		FastList<EntitySerialisationEntry> entitiesList = EntitySerialisationRegistry.m_EntitiesList;
		int num = 0;
		while (flag && num < entitiesList.Count)
		{
			EntitySerialisationEntry entitySerialisationEntry = entitiesList._items[num];
			int num2 = 0;
			while (flag && num2 < entitySerialisationEntry.m_ClientSynchronisedComponents.Count)
			{
				ClientSynchroniser clientSynchroniser = entitySerialisationEntry.m_ClientSynchronisedComponents._items[num2];
				if (clientSynchroniser.GetEntityType() == EntityType.WorldObject || clientSynchroniser.GetEntityType() == EntityType.WorldMapVanAvatar)
				{
					ClientWorldObjectSynchroniser clientWorldObjectSynchroniser = clientSynchroniser as ClientWorldObjectSynchroniser;
					if (null != clientWorldObjectSynchroniser && !clientWorldObjectSynchroniser.m_bHasEverReceived)
					{
						flag = false;
						break;
					}
				}
				num2++;
			}
			num++;
		}
		return flag;
	}

	// Token: 0x06002AAD RID: 10925 RVA: 0x000C7EE4 File Offset: 0x000C62E4
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		this.m_GameState = gameStateMessage.m_State;
		GameState state = gameStateMessage.m_State;
		if (state != GameState.MapScanNetworkEntities)
		{
			if (state != GameState.MapStartSynchronising)
			{
				if (state == GameState.MapStartEntities)
				{
					if (this.CheckStarted())
					{
						this.StartEntities();
					}
				}
			}
			else
			{
				this.m_Controller.StartSynchronisation();
				this.m_fSynchroniserStartupTimer = 0f;
				ClientMessenger.GameState(GameState.MapStartedSyncronising);
			}
		}
		else
		{
			this.m_Controller.ScanEntities(null);
			if (this.CheckScanned())
			{
				this.ScannedEntities();
			}
		}
	}

	// Token: 0x04002194 RID: 8596
	private MultiplayerController m_Controller;

	// Token: 0x04002195 RID: 8597
	private Client m_Client;

	// Token: 0x04002196 RID: 8598
	private GameState m_GameState;

	// Token: 0x04002197 RID: 8599
	private const float m_fSynchroniserStartupTimerDuration = 1f;

	// Token: 0x04002198 RID: 8600
	private float m_fSynchroniserStartupTimer;
}
