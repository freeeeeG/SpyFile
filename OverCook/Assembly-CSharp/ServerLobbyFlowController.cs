using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020006E4 RID: 1764
public class ServerLobbyFlowController : MonoBehaviour
{
	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06002144 RID: 8516 RVA: 0x0009F01F File Offset: 0x0009D41F
	public static ServerLobbyFlowController Instance
	{
		get
		{
			return ServerLobbyFlowController.s_instance;
		}
	}

	// Token: 0x06002145 RID: 8517 RVA: 0x0009F028 File Offset: 0x0009D428
	private void Awake()
	{
		if (ServerLobbyFlowController.s_instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			ServerLobbyFlowController.s_instance = this;
		}
		this.m_lobbyFlow = LobbyFlowController.Instance;
		this.m_IPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.m_lobbyInfo = LobbySetupInfo.Instance;
		if (this.m_lobbyInfo == null)
		{
			GameObject gameObject = new GameObject("LobbySetupInfo");
			this.m_lobbyInfo = gameObject.AddComponent<LobbySetupInfo>();
			this.m_lobbyInfo.m_visiblity = OnlineMultiplayerSessionVisibility.eMatchmaking;
			this.m_lobbyInfo.m_gameType = GameSession.GameType.Cooperative;
		}
		ServerUserSystem.OnUserAdded = (GenericVoid<User>)Delegate.Combine(ServerUserSystem.OnUserAdded, new GenericVoid<User>(this.OnUserAdded));
		ServerUserSystem.OnUserRemovedWithIndex = (GenericVoid<User, int>)Delegate.Combine(ServerUserSystem.OnUserRemovedWithIndex, new GenericVoid<User, int>(this.OnUserRemoved));
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
		Mailbox.Server.RegisterForMessageType(MessageType.LobbyClient, new OrderedMessageReceivedCallback(this.OnLobbyClientMessage));
		ClientLobbyFlowController clientLobbyFlowController = base.gameObject.RequireComponent<ClientLobbyFlowController>();
		clientLobbyFlowController.OnLeave = (GenericVoid)Delegate.Combine(clientLobbyFlowController.OnLeave, new GenericVoid(this.OnLeave));
		this.PreSetup();
	}

	// Token: 0x06002146 RID: 8518 RVA: 0x0009F164 File Offset: 0x0009D564
	protected void OnLobbyClientMessage(IOnlineMultiplayerSessionUserId _sender, Serialisable _data)
	{
		LobbyClientMessage lobbyClientMessage = (LobbyClientMessage)_data;
		if (lobbyClientMessage != null)
		{
			switch (lobbyClientMessage.m_type)
			{
			case LobbyClientMessage.LobbyMessageType.ThemeSelected:
			{
				SceneDirectoryData.LevelTheme theme = lobbyClientMessage.m_theme;
				int chefIndex = lobbyClientMessage.m_chefIndex;
				this.SelectTheme(theme, chefIndex);
				break;
			}
			case LobbyClientMessage.LobbyMessageType.StateRequest:
				this.SetState(this.m_state);
				this.InformAllOfCurrentSelections();
				break;
			case LobbyClientMessage.LobbyMessageType.TeamChangeRequest:
				if (this.m_state == LobbyFlowController.LobbyState.LocalThemeSelection || this.m_state == LobbyFlowController.LobbyState.OnlineThemeSelection)
				{
					this.ChangeTeam(lobbyClientMessage.m_chefIndex);
				}
				break;
			}
		}
	}

	// Token: 0x06002147 RID: 8519 RVA: 0x0009F1FC File Offset: 0x0009D5FC
	protected void PreSetup()
	{
		this.m_bIsCoop = (this.m_lobbyInfo.m_gameType == GameSession.GameType.Cooperative);
		for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
		{
			ServerUserSystem.m_Users._items[i].Team = TeamID.None;
		}
		if (!this.m_bIsCoop)
		{
			for (int j = 0; j < ServerUserSystem.m_Users.Count; j++)
			{
				ServerUserSystem.m_Users._items[j].Colour = this.m_lobbyFlow.m_noTeamColourIndex;
			}
		}
		this.ResetTimer(this.m_lobbyFlow.m_timeLimit);
		if (ConnectionStatus.IsInSession())
		{
			this.SetState(LobbyFlowController.LobbyState.OnlineSetup);
		}
		else
		{
			this.SetState(LobbyFlowController.LobbyState.LocalSetup);
		}
	}

	// Token: 0x06002148 RID: 8520 RVA: 0x0009F2BC File Offset: 0x0009D6BC
	protected void SetState(LobbyFlowController.LobbyState _state)
	{
		LobbyFlowController.LobbyState state = this.m_state;
		this.m_state = _state;
		OnlineMultiplayerSessionVisibility sessionVisibility = this.m_lobbyInfo.m_visiblity;
		if (this.m_state == LobbyFlowController.LobbyState.OnlineThemeSelected)
		{
			sessionVisibility = OnlineMultiplayerSessionVisibility.eClosed;
		}
		this.m_message.m_type = LobbyServerMessage.LobbyMessageType.StateChange;
		this.m_message.m_stateChange.m_state = _state;
		this.m_message.m_stateChange.m_bIsCoop = this.m_bIsCoop;
		this.m_message.m_stateChange.m_sessionVisibility = sessionVisibility;
		this.m_message.m_stateChange.m_connectionMode = this.m_lobbyInfo.m_connectionMode;
		ServerMessenger.LobbyMessage(this.m_message);
		if (state == this.m_state)
		{
			return;
		}
		switch (this.m_state)
		{
		case LobbyFlowController.LobbyState.LocalSetup:
			this.m_userChoices = new LobbyFlowController.ThemeChoice[OnlineMultiplayerConfig.MaxPlayers];
			this.ResetTimer(this.m_lobbyFlow.m_timeLimit);
			this.SetState(LobbyFlowController.LobbyState.LocalThemeSelection);
			break;
		case LobbyFlowController.LobbyState.OnlineSetup:
			this.m_userChoices = new LobbyFlowController.ThemeChoice[OnlineMultiplayerConfig.MaxPlayers];
			this.ResetTimer(this.m_lobbyFlow.m_timeLimit);
			this.SetState(LobbyFlowController.LobbyState.OnlineThemeSelection);
			break;
		case LobbyFlowController.LobbyState.LocalThemeSelected:
			this.ResetTimer(this.m_lobbyFlow.m_themeSelectionDuration);
			this.StartLevel();
			break;
		case LobbyFlowController.LobbyState.OnlineThemeSelected:
		{
			IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
			ServerOptions serverOptions = new ServerOptions
			{
				gameMode = ((!this.m_bIsCoop) ? GameMode.Versus : GameMode.Party),
				visibility = OnlineMultiplayerSessionVisibility.eClosed,
				hostUser = playerManager.GetUser(EngagementSlot.One),
				connectionMode = this.m_lobbyInfo.m_connectionMode
			};
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, serverOptions, null);
			OnlineMultiplayerSessionVisibility visiblity = this.m_lobbyInfo.m_visiblity;
			this.m_lobbyInfo.m_visiblity = serverOptions.visibility;
			this.m_lobbyFlow.m_uiPlayerRoot.ReCreateUIPlayers();
			this.m_lobbyInfo.m_visiblity = visiblity;
			this.ResetTimer(this.m_lobbyFlow.m_themeSelectionDuration);
			this.StartLevel();
			break;
		}
		}
	}

	// Token: 0x06002149 RID: 8521 RVA: 0x0009F4D0 File Offset: 0x0009D8D0
	protected void FillUndecidedUserChoices()
	{
		for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
		{
			if (ServerUserSystem.m_Users._items[i] != null && (this.m_userChoices[i] == null || this.m_userChoices[i].m_theme == SceneDirectoryData.LevelTheme.Count))
			{
				this.SelectTheme(SceneDirectoryData.LevelTheme.Random, i);
			}
		}
	}

	// Token: 0x0600214A RID: 8522 RVA: 0x0009F534 File Offset: 0x0009D934
	protected void ResetTimer(float time)
	{
		this.m_timeLeft = time;
		this.m_message.m_type = LobbyServerMessage.LobbyMessageType.ResetTimer;
		this.m_message.m_timerInfo = default(LobbyServerMessage.TimerInfo);
		this.m_message.m_timerInfo.m_timerVal = this.m_timeLeft;
		ServerMessenger.LobbyMessage(this.m_message);
	}

	// Token: 0x0600214B RID: 8523 RVA: 0x0009F58C File Offset: 0x0009D98C
	public void SelectTheme(SceneDirectoryData.LevelTheme _theme, int _chefIndex)
	{
		if (_chefIndex > -1 && _chefIndex < this.m_userChoices.Length)
		{
			this.m_userChoices[_chefIndex] = new LobbyFlowController.ThemeChoice();
			this.m_userChoices[_chefIndex].m_theme = _theme;
			this.m_userChoices[_chefIndex].m_chefIndex = _chefIndex;
			this.SendAllUserThemeSelection(_theme, _chefIndex);
		}
		if (this.AllUsersSelected() && (!UserSystemUtils.AnyRemoteUsers() || this.m_lobbyInfo.m_visiblity == OnlineMultiplayerSessionVisibility.ePrivate || ServerUserSystem.m_Users.Count == 4))
		{
			if (this.m_state == LobbyFlowController.LobbyState.LocalThemeSelection)
			{
				this.SetState(LobbyFlowController.LobbyState.LocalThemeSelected);
			}
			else if (this.m_state == LobbyFlowController.LobbyState.OnlineThemeSelection)
			{
				this.SetState(LobbyFlowController.LobbyState.OnlineThemeSelected);
			}
		}
	}

	// Token: 0x0600214C RID: 8524 RVA: 0x0009F640 File Offset: 0x0009DA40
	protected void SendAllUserThemeSelection(SceneDirectoryData.LevelTheme _theme, int _chefIndex)
	{
		this.m_message.m_type = LobbyServerMessage.LobbyMessageType.SelectionUpdate;
		this.m_message.m_selectionUpdate = default(LobbyServerMessage.SelectionUpdate);
		this.m_message.m_selectionUpdate.m_theme = _theme;
		this.m_message.m_selectionUpdate.m_chefIndex = _chefIndex;
		ServerMessenger.LobbyMessage(this.m_message);
	}

	// Token: 0x0600214D RID: 8525 RVA: 0x0009F69C File Offset: 0x0009DA9C
	protected void ChangeTeam(int _chefIndex)
	{
		if (this.m_bIsCoop)
		{
			return;
		}
		if (_chefIndex > -1 && _chefIndex < ServerUserSystem.m_Users.Count)
		{
			User user = ServerUserSystem.m_Users._items[_chefIndex];
			if (user != null)
			{
				int num = 0;
				int num2 = 0;
				for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
				{
					if (ServerUserSystem.m_Users._items[i].Team == TeamID.One)
					{
						num++;
					}
					else if (ServerUserSystem.m_Users._items[i].Team == TeamID.Two)
					{
						num2++;
					}
				}
				TeamID team = user.Team;
				if (team != TeamID.None)
				{
					if (team != TeamID.One)
					{
						if (team == TeamID.Two)
						{
							user.Team = TeamID.None;
							user.Colour = this.m_lobbyFlow.m_noTeamColourIndex;
						}
					}
					else if (num2 < Mathf.CeilToInt((float)ServerUserSystem.m_Users.Count / 2f))
					{
						user.Team = TeamID.Two;
						user.Colour = this.m_lobbyFlow.m_blueTeamColourIndex;
					}
					else
					{
						user.Team = TeamID.None;
						user.Colour = this.m_lobbyFlow.m_noTeamColourIndex;
					}
				}
				else if (num < Mathf.CeilToInt((float)ServerUserSystem.m_Users.Count / 2f))
				{
					user.Team = TeamID.One;
					user.Colour = this.m_lobbyFlow.m_redTeamColourIndex;
				}
				else
				{
					user.Team = TeamID.Two;
					user.Colour = this.m_lobbyFlow.m_blueTeamColourIndex;
				}
			}
		}
	}

	// Token: 0x0600214E RID: 8526 RVA: 0x0009F828 File Offset: 0x0009DC28
	protected void OnUserAdded(User _user)
	{
		if (!_user.IsLocal)
		{
			FastList<User> fastList = ServerUserSystem.m_Users.FindAll((User x) => !x.IsLocal && x != _user);
			if (fastList.Count == 0)
			{
				this.ResetTimer(this.m_lobbyFlow.m_timeLimit);
			}
		}
		int num = ServerUserSystem.m_Users.FindIndex((User x) => x == _user);
		if (num != -1)
		{
			this.m_userChoices[num] = null;
		}
		this.m_lobbyFlow.RefreshUserColours(this.m_bIsCoop);
		this.InformAllOfCurrentSelections();
		this.SetState(this.m_state);
	}

	// Token: 0x0600214F RID: 8527 RVA: 0x0009F8D0 File Offset: 0x0009DCD0
	protected void OnUserRemoved(User _user, int _idx)
	{
		if (this.m_state == LobbyFlowController.LobbyState.OnlineThemeSelected)
		{
			bool flag = false;
			for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
			{
				if (ServerUserSystem.m_Users._items[i] != _user)
				{
					if (!ServerUserSystem.m_Users._items[i].IsLocal)
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				if (this.m_delayedLevelLoad != null)
				{
					base.StopCoroutine(this.m_delayedLevelLoad);
					this.m_delayedLevelLoad = null;
				}
				if (this.m_bIsCoop)
				{
					ServerMessenger.LoadLevel("Lobbies", GameState.PartyLobby, false, GameState.NotSet);
				}
				else
				{
					ServerMessenger.LoadLevel("Lobbies", GameState.VSLobby, false, GameState.NotSet);
				}
				UnityEngine.Object.DestroyImmediate(base.gameObject);
				return;
			}
		}
		for (int j = _idx; j < this.m_userChoices.Length - 1; j++)
		{
			this.m_userChoices[j] = this.m_userChoices[j + 1];
		}
		for (int k = ServerUserSystem.m_Users.Count; k < this.m_userChoices.Length; k++)
		{
			this.m_userChoices[k] = new LobbyFlowController.ThemeChoice();
		}
		this.InformAllOfCurrentSelections();
		this.EnsureTeamsCanBeBalanced();
	}

	// Token: 0x06002150 RID: 8528 RVA: 0x0009F9F8 File Offset: 0x0009DDF8
	private void EnsureTeamsCanBeBalanced()
	{
		if (!this.m_bIsCoop)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
			{
				switch (ServerUserSystem.m_Users._items[i].Team)
				{
				case TeamID.One:
					num++;
					break;
				case TeamID.Two:
					num2++;
					break;
				case TeamID.None:
				case TeamID.Count:
					num3++;
					break;
				}
			}
			if (num3 == 0 && (num == 0 || num2 == 0))
			{
				for (int j = 0; j < ServerUserSystem.m_Users.Count; j++)
				{
					User user = ServerUserSystem.m_Users._items[j];
					user.Team = TeamID.None;
					user.Colour = this.m_lobbyFlow.m_noTeamColourIndex;
				}
			}
		}
	}

	// Token: 0x06002151 RID: 8529 RVA: 0x0009FAD8 File Offset: 0x0009DED8
	protected void OnUsersChanged()
	{
		bool flag = UserSystemUtils.AnyRemoteUsers();
		if (flag)
		{
			this.SetState(this.m_state);
		}
		for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
		{
			this.m_lobbyFlow.RefreshUserColour(ServerUserSystem.m_Users._items[i], this.m_bIsCoop);
		}
	}

	// Token: 0x06002152 RID: 8530 RVA: 0x0009FB38 File Offset: 0x0009DF38
	protected void InformAllOfCurrentSelections()
	{
		for (int i = 0; i < this.m_userChoices.Length; i++)
		{
			this.SendAllUserThemeSelection((this.m_userChoices[i] == null) ? SceneDirectoryData.LevelTheme.Count : this.m_userChoices[i].m_theme, i);
		}
	}

	// Token: 0x06002153 RID: 8531 RVA: 0x0009FB88 File Offset: 0x0009DF88
	public void Update()
	{
		switch (this.m_state)
		{
		case LobbyFlowController.LobbyState.OnlineThemeSelection:
		{
			float timeLeft = this.m_timeLeft;
			if (this.m_lobbyInfo.m_visiblity == OnlineMultiplayerSessionVisibility.ePrivate && this.AllUsersSelected())
			{
				this.SetState(LobbyFlowController.LobbyState.OnlineThemeSelected);
			}
			else if (ServerUserSystem.m_Users.Count > 1 && UserSystemUtils.AnyRemoteUsers())
			{
				this.m_timeLeft -= TimeManager.GetDeltaTime(base.gameObject);
				if (this.m_timeLeft > 0f)
				{
					if (Mathf.FloorToInt(this.m_timeLeft) < Mathf.FloorToInt(timeLeft))
					{
						this.m_message.m_type = LobbyServerMessage.LobbyMessageType.TimerUpdate;
						this.m_message.m_timerInfo = default(LobbyServerMessage.TimerInfo);
						this.m_message.m_timerInfo.m_timerVal = this.m_timeLeft;
						ServerMessenger.LobbyMessage(this.m_message);
					}
				}
				else
				{
					this.SetState(LobbyFlowController.LobbyState.OnlineThemeSelected);
				}
			}
			break;
		}
		case LobbyFlowController.LobbyState.OnlineThemeSelected:
		{
			float timeLeft2 = this.m_timeLeft;
			this.m_timeLeft -= TimeManager.GetDeltaTime(base.gameObject);
			if (this.m_timeLeft > 0f && Mathf.FloorToInt(this.m_timeLeft) < Mathf.FloorToInt(timeLeft2))
			{
				this.m_message.m_type = LobbyServerMessage.LobbyMessageType.TimerUpdate;
				this.m_message.m_timerInfo = default(LobbyServerMessage.TimerInfo);
				this.m_message.m_timerInfo.m_timerVal = this.m_timeLeft;
				ServerMessenger.LobbyMessage(this.m_message);
			}
			break;
		}
		}
	}

	// Token: 0x06002154 RID: 8532 RVA: 0x0009FD2C File Offset: 0x0009E12C
	protected void StartLevel()
	{
		this.FillUndecidedUserChoices();
		SceneDirectoryData.LevelTheme levelTheme = this.PickTheme();
		List<int> list = new List<int>();
		for (int i = 0; i < this.m_userChoices.Length; i++)
		{
			if (this.m_userChoices[i] != null && this.m_userChoices[i].m_theme == levelTheme)
			{
				list.Add(i);
			}
		}
		int chefIndex = 0;
		if (!this.m_lobbyFlow.UnanimousSelection(this.m_userChoices) && UserSystemUtils.AnyRemoteUsers())
		{
			chefIndex = list[UnityEngine.Random.Range(0, list.Count)];
		}
		if (this.m_lobbyInfo.m_gameType == GameSession.GameType.Competitive)
		{
			this.AssignUsersToTeams();
		}
		this.m_message.m_type = LobbyServerMessage.LobbyMessageType.FinalSelection;
		this.m_message.m_selectionUpdate = default(LobbyServerMessage.SelectionUpdate);
		this.m_message.m_selectionUpdate.m_theme = levelTheme;
		this.m_message.m_selectionUpdate.m_chefIndex = chefIndex;
		ServerMessenger.LobbyMessage(this.m_message);
		this.PickLevel(levelTheme);
	}

	// Token: 0x06002155 RID: 8533 RVA: 0x0009FE30 File Offset: 0x0009E230
	protected void AssignUsersToTeams()
	{
		int[] array = new int[ClientUserSystem.m_Users.Count];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = i;
		}
		array.ShuffleContents<int>();
		foreach (int num in array)
		{
			if (ClientUserSystem.m_Users._items[num].Team == TeamID.None)
			{
				this.ChangeTeam(num);
			}
		}
	}

	// Token: 0x06002156 RID: 8534 RVA: 0x0009FEA4 File Offset: 0x0009E2A4
	protected SceneDirectoryData.LevelTheme PickTheme()
	{
		if (this.m_state == LobbyFlowController.LobbyState.LocalThemeSelected)
		{
			ThemeSelectButton buttonForTheme = this.m_lobbyFlow.m_themeSelectMenu.GetButtonForTheme(this.m_userChoices[0].m_theme);
			return buttonForTheme.Theme;
		}
		if (this.m_state == LobbyFlowController.LobbyState.OnlineThemeSelected)
		{
			LobbyFlowController.ThemeChoice[] items = this.m_userChoices.FindAll((LobbyFlowController.ThemeChoice x) => x != null && x.m_theme != SceneDirectoryData.LevelTheme.Count);
			LobbyFlowController.ThemeChoice randomElement = items.GetRandomElement<LobbyFlowController.ThemeChoice>();
			if (randomElement != null)
			{
				ThemeSelectButton buttonForTheme2 = this.m_lobbyFlow.m_themeSelectMenu.GetButtonForTheme(randomElement.m_theme);
				return buttonForTheme2.Theme;
			}
		}
		return SceneDirectoryData.LevelTheme.Random;
	}

	// Token: 0x06002157 RID: 8535 RVA: 0x0009FF44 File Offset: 0x0009E344
	protected void PickLevel(SceneDirectoryData.LevelTheme _theme)
	{
		Predicate<SceneDirectoryData.SceneDirectoryEntry> match = (SceneDirectoryData.SceneDirectoryEntry entry) => entry.AvailableInLobby && entry.Theme != SceneDirectoryData.LevelTheme.Null && entry.Theme != SceneDirectoryData.LevelTheme.Count && (entry.Theme == _theme || _theme == SceneDirectoryData.LevelTheme.Random);
		FastList<SceneDirectoryData.SceneDirectoryEntry> fastList = new FastList<SceneDirectoryData.SceneDirectoryEntry>(60);
		SceneDirectoryData[] sceneDirectories = this.m_lobbyFlow.GetSceneDirectories();
		DLCManager dlcmanager = GameUtils.RequireManager<DLCManager>();
		List<DLCFrontendData> allDlc = dlcmanager.AllDlc;
		GameSession.GameType gameType = (!this.m_bIsCoop) ? GameSession.GameType.Competitive : GameSession.GameType.Cooperative;
		int[] array = new int[sceneDirectories.Length];
		for (int i = 0; i < sceneDirectories.Length; i++)
		{
			DLCFrontendData dlcfrontendData = null;
			int dlcidfromSceneDirIndex = this.m_lobbyFlow.GetDLCIDFromSceneDirIndex(gameType, i);
			if (_theme == SceneDirectoryData.LevelTheme.Random)
			{
				for (int j = 0; j < allDlc.Count; j++)
				{
					DLCFrontendData dlcfrontendData2 = allDlc[j];
					if (dlcfrontendData2.m_DLCID == dlcidfromSceneDirIndex)
					{
						dlcfrontendData = dlcfrontendData2;
						break;
					}
				}
			}
			if (dlcfrontendData == null || dlcmanager.IsDLCAvailable(dlcfrontendData))
			{
				fastList.AddRange(sceneDirectories[i].Scenes.FindAll(match));
			}
			array[i] = fastList.Count;
		}
		int num = UnityEngine.Random.Range(0, fastList.Count);
		int idx = -1;
		for (int k = 0; k < array.Length; k++)
		{
			if (num < array[k])
			{
				idx = k;
				break;
			}
		}
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = fastList._items[num];
		int dlcidfromSceneDirIndex2 = this.m_lobbyFlow.GetDLCIDFromSceneDirIndex(gameType, idx);
		SceneDirectoryData.PerPlayerCountDirectoryEntry sceneVarient = sceneDirectoryEntry.GetSceneVarient(ServerUserSystem.m_Users.Count);
		if (sceneVarient == null)
		{
			if (!this.m_bIsCoop)
			{
				T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
				dialog.Initialize("Text.Versus.NotEnoughPlayers.Title", "Text.Versus.NotEnoughPlayers.Message", "Text.Button.Confirm", null, null, T17DialogBox.Symbols.Warning, true, true, false);
				T17DialogBox t17DialogBox = dialog;
				t17DialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox.OnConfirm, new T17DialogBox.DialogEvent(delegate()
				{
					ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, delegate(IConnectionModeSwitchStatus _status)
					{
						if (_status.GetProgress() == eConnectionModeSwitchProgress.Complete)
						{
							ServerGameSetup.Mode = GameMode.OnlineKitchen;
							ServerMessenger.LoadLevel("StartScreen", GameState.MainMenu, true, GameState.NotSet);
						}
					});
				}));
				dialog.Show();
			}
			return;
		}
		this.m_delayedLevelLoad = base.StartCoroutine(this.DelayedLevelLoad(sceneVarient.SceneName, dlcidfromSceneDirIndex2));
	}

	// Token: 0x06002158 RID: 8536 RVA: 0x000A0160 File Offset: 0x0009E560
	private IEnumerator DelayedLevelLoad(string _levelName, int _dlcID)
	{
		this.m_message.m_type = LobbyServerMessage.LobbyMessageType.CreateGameSession;
		this.m_message.m_dlcID = _dlcID;
		ServerMessenger.LobbyMessage(this.m_message);
		IEnumerator delay = CoroutineUtils.TimerRoutine(this.m_lobbyFlow.m_themeSelectionDuration + 1f, base.gameObject.layer);
		while (delay.MoveNext())
		{
			yield return null;
		}
		if (InviteMonitor.CheckStatus(InviteMonitor.StatusFlags.HandlerIsWaitingOnUserInput))
		{
			InviteMonitor.SwitchHandlerType(InviteMonitor.HandlerType.None);
		}
		else if (!InviteMonitor.CheckStatus(InviteMonitor.StatusFlags.HandlerIsValid | InviteMonitor.StatusFlags.HandlerIsIdle) || ServerGameSetup.Mode == GameMode.OnlineKitchen)
		{
			this.m_delayedLevelLoad = null;
			yield break;
		}
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession != null)
		{
			PersistentObject persistentObject = gameSession.gameObject.RequestComponent<PersistentObject>();
			if (persistentObject != null)
			{
				persistentObject.AddPersistingLevel("Lobbies");
				persistentObject.AddPersistingLevel("Loading");
				persistentObject.AddPersistingLevel(_levelName);
			}
			gameSession.TypeSettings.WorldMapScene = "Lobbies";
		}
		PersistentObject lobbyPersist = this.m_lobbyInfo.gameObject.RequireComponent<PersistentObject>();
		lobbyPersist.AddPersistingLevel(_levelName);
		ServerMessenger.LoadLevel(_levelName, GameState.LoadKitchen, true, GameState.RunKitchen);
		yield break;
	}

	// Token: 0x06002159 RID: 8537 RVA: 0x000A018C File Offset: 0x0009E58C
	protected bool AllUsersSelected()
	{
		if (this.m_lobbyFlow.IsLocalState(this.m_state))
		{
			return this.m_bIsCoop || ServerUserSystem.m_Users.Count > 1;
		}
		if (UserSystemUtils.AnyRemoteUsers())
		{
			for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
			{
				if (this.m_userChoices[i] == null)
				{
					return false;
				}
				if (this.m_userChoices[i].m_theme == SceneDirectoryData.LevelTheme.Count)
				{
					return false;
				}
				if (this.m_userChoices[i].m_chefIndex < 0 || this.m_userChoices[i].m_chefIndex > this.m_userChoices.Length - 1)
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600215A RID: 8538 RVA: 0x000A024E File Offset: 0x0009E64E
	protected void OnLeave()
	{
		if (this.m_delayedLevelLoad != null)
		{
			base.StopCoroutine(this.m_delayedLevelLoad);
			this.m_delayedLevelLoad = null;
		}
		UnityEngine.Object.DestroyImmediate(base.gameObject);
	}

	// Token: 0x0600215B RID: 8539 RVA: 0x000A027C File Offset: 0x0009E67C
	private void OnDestroy()
	{
		if (ServerLobbyFlowController.s_instance == this)
		{
			ServerLobbyFlowController.s_instance = null;
		}
		Mailbox.Server.UnregisterForMessageType(MessageType.LobbyClient, new OrderedMessageReceivedCallback(this.OnLobbyClientMessage));
		ClientLobbyFlowController clientLobbyFlowController = base.gameObject.RequestComponent<ClientLobbyFlowController>();
		if (clientLobbyFlowController != null)
		{
			ClientLobbyFlowController clientLobbyFlowController2 = clientLobbyFlowController;
			clientLobbyFlowController2.OnLeave = (GenericVoid)Delegate.Remove(clientLobbyFlowController2.OnLeave, new GenericVoid(this.OnLeave));
		}
		ServerUserSystem.OnUserAdded = (GenericVoid<User>)Delegate.Remove(ServerUserSystem.OnUserAdded, new GenericVoid<User>(this.OnUserAdded));
		ServerUserSystem.OnUserRemovedWithIndex = (GenericVoid<User, int>)Delegate.Remove(ServerUserSystem.OnUserRemovedWithIndex, new GenericVoid<User, int>(this.OnUserRemoved));
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChanged));
	}

	// Token: 0x04001977 RID: 6519
	private LobbyFlowController m_lobbyFlow;

	// Token: 0x04001978 RID: 6520
	private static ServerLobbyFlowController s_instance;

	// Token: 0x04001979 RID: 6521
	private IPlayerManager m_IPlayerManager;

	// Token: 0x0400197A RID: 6522
	private LobbySetupInfo m_lobbyInfo;

	// Token: 0x0400197B RID: 6523
	private LobbyServerMessage m_message = new LobbyServerMessage();

	// Token: 0x0400197C RID: 6524
	private float m_timeLeft;

	// Token: 0x0400197D RID: 6525
	protected LobbyFlowController.LobbyState m_state;

	// Token: 0x0400197E RID: 6526
	protected LobbyFlowController.ThemeChoice[] m_userChoices;

	// Token: 0x0400197F RID: 6527
	protected bool m_bIsCoop = true;

	// Token: 0x04001980 RID: 6528
	private Coroutine m_delayedLevelLoad;
}
