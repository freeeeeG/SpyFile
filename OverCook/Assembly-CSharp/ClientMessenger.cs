using System;
using Team17.Online;
using Team17.Online.Multiplayer;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020008AA RID: 2218
internal class ClientMessenger
{
	// Token: 0x06002B2F RID: 11055 RVA: 0x000CA58B File Offset: 0x000C898B
	public static void ControllerState(ControllerStateMessage _controllerState)
	{
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.Input, _controllerState, true);
	}

	// Token: 0x06002B30 RID: 11056 RVA: 0x000CA59B File Offset: 0x000C899B
	public static void Example(float fFloat, bool bBool)
	{
		ClientMessenger.m_Example.Initialise(fFloat, bBool);
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.Example, ClientMessenger.m_Example, true);
	}

	// Token: 0x06002B31 RID: 11057 RVA: 0x000CA5BA File Offset: 0x000C89BA
	public static void GameState(GameState state)
	{
		ClientMessenger.m_GameState.Initialise(state, ClientUserSystem.s_LocalMachineId);
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.GameState, ClientMessenger.m_GameState, true);
	}

	// Token: 0x06002B32 RID: 11058 RVA: 0x000CA5DE File Offset: 0x000C89DE
	public static void ChefAvatar(uint chefAvatar, User user)
	{
		ClientMessenger.m_ChefAvatar.Initialise(chefAvatar, user.Machine, user.Engagement, user.Split);
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.ChefAvatar, ClientMessenger.m_ChefAvatar, true);
	}

	// Token: 0x06002B33 RID: 11059 RVA: 0x000CA610 File Offset: 0x000C8A10
	public static void ChefEventMessage(ChefEventMessage.ChefEventType _type, GameObject _chef, MonoBehaviour _object)
	{
		uint entityID = 0U;
		if (_object != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_object.gameObject);
			if (entry != null)
			{
				entityID = entry.m_Header.m_uEntityID;
			}
		}
		uint chefEntityID = 0U;
		EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(_chef);
		if (entry2 != null)
		{
			chefEntityID = entry2.m_Header.m_uEntityID;
		}
		ClientMessenger.m_ChefEvent.Initialise(_type, chefEntityID, entityID);
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.ChefEvent, ClientMessenger.m_ChefEvent, true);
	}

	// Token: 0x06002B34 RID: 11060 RVA: 0x000CA684 File Offset: 0x000C8A84
	public static void ChefEventMessage(ChefEventMessage.ChefEventType _type, GameObject _chef, GameObject _object)
	{
		uint entityID = 0U;
		if (_object != null)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_object);
			if (entry != null)
			{
				entityID = entry.m_Header.m_uEntityID;
			}
		}
		EntitySerialisationEntry entry2 = EntitySerialisationRegistry.GetEntry(_chef);
		if (entry2 == null)
		{
			return;
		}
		uint uEntityID = entry2.m_Header.m_uEntityID;
		ClientMessenger.m_ChefEvent.Initialise(_type, uEntityID, entityID);
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.ChefEvent, ClientMessenger.m_ChefEvent, true);
	}

	// Token: 0x06002B35 RID: 11061 RVA: 0x000CA6F4 File Offset: 0x000C8AF4
	public static void ChefEventMessage(ChefEventMessage.ChefEventType _type, GameObject _chef)
	{
		uint entityID = 0U;
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(_chef);
		uint uEntityID = entry.m_Header.m_uEntityID;
		ClientMessenger.m_ChefEvent.Initialise(_type, uEntityID, entityID);
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.ChefEvent, ClientMessenger.m_ChefEvent, true);
	}

	// Token: 0x06002B36 RID: 11062 RVA: 0x000CA738 File Offset: 0x000C8B38
	public static void ChefKnockbackEventMessage(ChefEventMessage.KnockbackType _knockbackType, uint _chef, Vector2 _knockbackForce, Vector3 _relativeContactPoint)
	{
		uint entityID = 0U;
		ClientMessenger.m_ChefEvent.Initialise(Team17.Online.Multiplayer.Messaging.ChefEventMessage.ChefEventType.KnockBack, _chef, entityID);
		ClientMessenger.m_ChefEvent.Knockback_Type = _knockbackType;
		ClientMessenger.m_ChefEvent.KnockbackForce = _knockbackForce;
		ClientMessenger.m_ChefEvent.RelativeContactPoint = _relativeContactPoint;
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.ChefEvent, ClientMessenger.m_ChefEvent, true);
	}

	// Token: 0x06002B37 RID: 11063 RVA: 0x000CA787 File Offset: 0x000C8B87
	public static void LobbyMessage(LobbyClientMessage _message)
	{
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.LobbyClient, _message, true);
	}

	// Token: 0x06002B38 RID: 11064 RVA: 0x000CA797 File Offset: 0x000C8B97
	public static void MapAvatarHorn(int _playerIdx)
	{
		ClientMessenger.m_mapAvatarMessage.m_playerIdx = _playerIdx;
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.MapAvatarHorn, ClientMessenger.m_mapAvatarMessage, true);
	}

	// Token: 0x06002B39 RID: 11065 RVA: 0x000CA7B6 File Offset: 0x000C8BB6
	public static void EmoteWheelMessage(EmoteWheelMessage _message)
	{
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.EmoteWheel, _message, true);
	}

	// Token: 0x06002B3A RID: 11066 RVA: 0x000CA7C8 File Offset: 0x000C8BC8
	public static void SendResumeEntitySync(GameObject go)
	{
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(go);
		ClientMessenger.m_resumeEntitySyncMessage.Initialise(entry.m_Header);
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.ResumeEntitySync, ClientMessenger.m_resumeEntitySyncMessage, true);
	}

	// Token: 0x06002B3B RID: 11067 RVA: 0x000CA7FE File Offset: 0x000C8BFE
	public static void ControllerSettings(PadSide side, User user)
	{
		ClientMessenger.m_ControllerSettings.Initialise(side, user.Machine, user.Engagement, user.Split);
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.ControllerSettings, ClientMessenger.m_ControllerSettings, true);
	}

	// Token: 0x06002B3C RID: 11068 RVA: 0x000CA82F File Offset: 0x000C8C2F
	public static void HighScores(GameProgress.HighScores highScores, int DLC)
	{
		ClientMessenger.m_HighScores.HighScores = highScores;
		ClientMessenger.m_HighScores.DLC = DLC;
		ClientMessenger.m_HighScores.m_Machine = ClientUserSystem.s_LocalMachineId;
		ClientMessenger.m_LocalClient.SendMessageToServer(MessageType.HighScores, ClientMessenger.m_HighScores, true);
	}

	// Token: 0x06002B3D RID: 11069 RVA: 0x000CA868 File Offset: 0x000C8C68
	public static void OnClientStarted(Client client)
	{
		ClientMessenger.m_LocalClient = client;
	}

	// Token: 0x04002235 RID: 8757
	private static ExampleNetworkMessage m_Example = new ExampleNetworkMessage();

	// Token: 0x04002236 RID: 8758
	private static GameStateMessage m_GameState = new GameStateMessage();

	// Token: 0x04002237 RID: 8759
	private static ChefAvatarMessage m_ChefAvatar = new ChefAvatarMessage();

	// Token: 0x04002238 RID: 8760
	private static ChefEventMessage m_ChefEvent = new ChefEventMessage();

	// Token: 0x04002239 RID: 8761
	public static MapAvatarHornMessage m_mapAvatarMessage = new MapAvatarHornMessage();

	// Token: 0x0400223A RID: 8762
	public static ResumeEntitySyncMessage m_resumeEntitySyncMessage = new ResumeEntitySyncMessage();

	// Token: 0x0400223B RID: 8763
	private static ControllerSettingsMessage m_ControllerSettings = new ControllerSettingsMessage();

	// Token: 0x0400223C RID: 8764
	private static HighScoresMessage m_HighScores = new HighScoresMessage();

	// Token: 0x0400223D RID: 8765
	private static Client m_LocalClient = null;
}
