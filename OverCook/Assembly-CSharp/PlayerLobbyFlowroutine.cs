using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000BB7 RID: 2999
public class PlayerLobbyFlowroutine
{
	// Token: 0x06003D4D RID: 15693 RVA: 0x00125064 File Offset: 0x00123464
	public PlayerLobbyFlowroutine(Generic<LobbyUIController, GameSession.GameType> _uiBuilder, AmbiControlsMappingData _sidedAmbiMapping, AmbiControlsMappingData _unsidedAmbiMapping, PauseMenuManager _pauseManager, MapAvatarControls _controls, int _levelIndex, bool skipLobby = false)
	{
		this.m_uiBuilder = _uiBuilder;
		this.m_unsidedAmbiMapping = _unsidedAmbiMapping;
		this.m_sceneDirectory = GameUtils.GetGameSession().Progress.GetSceneDirectory();
		if (skipLobby || GameUtils.GetDebugConfig().m_skipPlayerJoining)
		{
			this.SkipPlayerLobby(_levelIndex);
			return;
		}
		this.StartPlayerLobbySession(_pauseManager, _controls, _levelIndex);
	}

	// Token: 0x06003D4E RID: 15694 RVA: 0x001250CC File Offset: 0x001234CC
	public IEnumerator Run()
	{
		while (this.m_active)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003D4F RID: 15695 RVA: 0x001250E8 File Offset: 0x001234E8
	private void StartPlayerLobbySession(PauseMenuManager _pauseManager, MapAvatarControls _controls, int _levelIndex)
	{
		this.m_pauseManager = _pauseManager;
		this.m_avatar = _controls;
		this.m_levelIndex = _levelIndex;
		if (this.m_pauseManager != null)
		{
			this.m_pauseManager.enabled = false;
		}
		if (this.m_avatar != null)
		{
			this.m_avatar.enabled = false;
		}
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = this.m_sceneDirectory.Scenes[_levelIndex];
		GameSession gameSession = GameUtils.GetGameSession();
		this.m_lobby = this.m_uiBuilder(gameSession.TypeSettings.Type);
		GameProgress.GameProgressData.LevelProgress progress = gameSession.Progress.GetProgress(_levelIndex);
		this.m_lobby.SetSceneData(gameSession.TypeSettings.Type, sceneDirectoryEntry, progress);
		this.m_lobby.RegisterForCompletedMessage(new CallbackVoid(this.OnLobbyCompleted));
		this.m_lobby.RegisterForCanceledMessage(new CallbackVoid(this.OnPlayerSelectUIClosed));
	}

	// Token: 0x06003D50 RID: 15696 RVA: 0x001251C8 File Offset: 0x001235C8
	private void SkipPlayerLobby(int _levelIndex)
	{
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = this.m_sceneDirectory.Scenes[_levelIndex];
		int num = sceneDirectoryEntry.SceneVarients.FindIndex_Predicate((SceneDirectoryData.PerPlayerCountDirectoryEntry x) => x.PlayerCount == 4);
		SceneDirectoryData.PerPlayerCountDirectoryEntry perPlayerCountDirectoryEntry = sceneDirectoryEntry.SceneVarients[num];
		if (perPlayerCountDirectoryEntry != null)
		{
			GameInputConfig inputConfig = this.ConstructDebugInputConfig();
			Dictionary<PlayerInputLookup.Player, GameSession.SelectedChefData> dictionary = new Dictionary<PlayerInputLookup.Player, GameSession.SelectedChefData>();
			AvatarDirectoryData avatarDirectory = GameUtils.GetGameSession().Progress.GetAvatarDirectory();
			ChefAvatarData[] avatars = avatarDirectory.Avatars;
			ChefColourData[] colours = avatarDirectory.Colours;
			GameSession.SelectedChefData value = new GameSession.SelectedChefData(avatars.TryAtIndex(0, avatars[0]), colours.TryAtIndex(0, colours[0]));
			GameSession.SelectedChefData value2 = new GameSession.SelectedChefData(avatars.TryAtIndex(1, avatars[0]), colours.TryAtIndex(1, colours[0]));
			GameSession.SelectedChefData value3 = new GameSession.SelectedChefData(avatars.TryAtIndex(2, avatars[0]), colours.TryAtIndex(2, colours[0]));
			GameSession.SelectedChefData value4 = new GameSession.SelectedChefData(avatars.TryAtIndex(3, avatars[0]), colours.TryAtIndex(3, colours[0]));
			dictionary.Add(PlayerInputLookup.Player.One, value);
			dictionary.Add(PlayerInputLookup.Player.Two, value2);
			dictionary.Add(PlayerInputLookup.Player.Three, value3);
			dictionary.Add(PlayerInputLookup.Player.Four, value4);
			this.LoadKitchen(_levelIndex, num, perPlayerCountDirectoryEntry.SceneName, sceneDirectoryEntry.LoadScreenOverride, inputConfig, dictionary, perPlayerCountDirectoryEntry.LevelConfig);
		}
	}

	// Token: 0x06003D51 RID: 15697 RVA: 0x00125310 File Offset: 0x00123710
	private GameInputConfig ConstructDebugInputConfig()
	{
		GameInputConfig.ConfigEntry[] entries = new GameInputConfig.ConfigEntry[0];
		GameDebugConfig.SkipPlayerJoiningConfig skipJoiningConfig = GameUtils.GetDebugConfig().m_skipJoiningConfig;
		if (skipJoiningConfig != GameDebugConfig.SkipPlayerJoiningConfig.ControllerEach)
		{
			if (skipJoiningConfig == GameDebugConfig.SkipPlayerJoiningConfig.TwoSidedPads)
			{
				entries = new GameInputConfig.ConfigEntry[]
				{
					new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.One, ControlPadInput.PadNum.One, PadSide.Left, ClientUserSystem.s_LocalMachineId, this.m_unsidedAmbiMapping),
					new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Two, ControlPadInput.PadNum.Two, PadSide.Left, ClientUserSystem.s_LocalMachineId, this.m_unsidedAmbiMapping),
					new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Three, ControlPadInput.PadNum.One, PadSide.Right, ClientUserSystem.s_LocalMachineId, this.m_unsidedAmbiMapping),
					new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Four, ControlPadInput.PadNum.Two, PadSide.Right, ClientUserSystem.s_LocalMachineId, this.m_unsidedAmbiMapping)
				};
			}
		}
		else
		{
			entries = new GameInputConfig.ConfigEntry[]
			{
				new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.One, ControlPadInput.PadNum.One, PadSide.Both, ClientUserSystem.s_LocalMachineId, this.m_unsidedAmbiMapping),
				new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Two, ControlPadInput.PadNum.Two, PadSide.Both, ClientUserSystem.s_LocalMachineId, this.m_unsidedAmbiMapping),
				new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Three, ControlPadInput.PadNum.Three, PadSide.Both, ClientUserSystem.s_LocalMachineId, this.m_unsidedAmbiMapping),
				new GameInputConfig.ConfigEntry(PlayerInputLookup.Player.Four, ControlPadInput.PadNum.Four, PadSide.Both, ClientUserSystem.s_LocalMachineId, this.m_unsidedAmbiMapping)
			};
		}
		return new GameInputConfig(entries);
	}

	// Token: 0x06003D52 RID: 15698 RVA: 0x00125410 File Offset: 0x00123810
	private void OnPlayerSelectUIClosed()
	{
		UnityEngine.Object.Destroy(this.m_lobby.gameObject);
		if (this.m_avatar != null)
		{
			this.m_avatar.enabled = true;
		}
		if (this.m_pauseManager != null)
		{
			this.m_pauseManager.enabled = true;
		}
		this.m_active = false;
	}

	// Token: 0x06003D53 RID: 15699 RVA: 0x00125470 File Offset: 0x00123870
	private void OnLobbyCompleted()
	{
		LobbyUIController lobby = this.m_lobby;
		Dictionary<PlayerInputLookup.Player, LobbyUIController.AvatarCardData> selectedAvatars = lobby.GetSelectedAvatars();
		int levelIndex = this.m_levelIndex;
		int playerCount = selectedAvatars.Count;
		if (GameUtils.GetGameSession().TypeSettings.Type == GameSession.GameType.Competitive)
		{
			playerCount = 4;
		}
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = this.m_sceneDirectory.Scenes[levelIndex];
		SceneDirectoryData.PerPlayerCountDirectoryEntry sceneVarient = sceneDirectoryEntry.GetSceneVarient(playerCount);
		if (sceneVarient != null)
		{
			string sceneName = sceneVarient.SceneName;
			if (sceneName != string.Empty)
			{
				GameSession.GameLevelSettings gameLevelSettings = new GameSession.GameLevelSettings();
				gameLevelSettings.SceneDirectoryVarientEntry = sceneVarient;
				PlayerLobbyFlowroutine.LoadKitchen(levelIndex, sceneName, sceneDirectoryEntry.LoadScreenOverride, gameLevelSettings);
				return;
			}
		}
		Vector2 vector = new Vector2(0.5f * (float)Camera.main.pixelWidth, 0.5f * (float)Camera.main.pixelHeight);
	}

	// Token: 0x06003D54 RID: 15700 RVA: 0x00125538 File Offset: 0x00123938
	private void LoadKitchen(int _levelIndex, int _varientIndex, string _actualScene, Sprite _loadingScreen, GameInputConfig _inputConfig, Dictionary<PlayerInputLookup.Player, GameSession.SelectedChefData> _chef, LevelConfigBase _levelConfig)
	{
		SceneDirectoryData sceneDirectory = GameUtils.GetGameSession().Progress.GetSceneDirectory();
		PlayerLobbyFlowroutine.LoadKitchen(_levelIndex, _actualScene, _loadingScreen, new GameSession.GameLevelSettings
		{
			SceneDirectoryVarientEntry = sceneDirectory.Scenes[_levelIndex].SceneVarients[_varientIndex]
		});
	}

	// Token: 0x06003D55 RID: 15701 RVA: 0x0012557C File Offset: 0x0012397C
	public static void LoadKitchen(int _levelIndex, string _actualScene, Sprite _loadingScreen, GameSession.GameLevelSettings _levelSettings)
	{
		GameSession gameSession = GameUtils.GetGameSession();
		gameSession.LevelSettings = _levelSettings;
		gameSession.Progress.SaveData.LastLevelEntered = _levelIndex;
		GameUtils.GetGameSession().SaveSession(null);
		LoadingScreenFlow.LoadScene(_actualScene, GameState.RunKitchen);
	}

	// Token: 0x06003D56 RID: 15702 RVA: 0x001255BC File Offset: 0x001239BC
	private GameInputConfig BuildInputConfig(Dictionary<PlayerInputLookup.Player, LobbyUIController.AvatarCardData> _avatars)
	{
		GameInputConfig.ConfigEntry[] array = new GameInputConfig.ConfigEntry[_avatars.Count];
		PlayerInputLookup.Player[] array2 = new PlayerInputLookup.Player[_avatars.Count];
		_avatars.Keys.CopyTo(array2, 0);
		Array.Sort<PlayerInputLookup.Player>(array2);
		for (int i = 0; i < array2.Length; i++)
		{
			LobbyUIController.AvatarCardData avatarCardData = _avatars[array2[i]];
			PlayerInputLookup.Player player = (PlayerInputLookup.Player)i;
			array[i] = new GameInputConfig.ConfigEntry(player, avatarCardData.PlayerInput.Pad, avatarCardData.PlayerInput.Side, ClientUserSystem.s_LocalMachineId, avatarCardData.PlayerInput.AmbiControlsMapping);
		}
		return new GameInputConfig(array);
	}

	// Token: 0x04003142 RID: 12610
	private Generic<LobbyUIController, GameSession.GameType> m_uiBuilder;

	// Token: 0x04003143 RID: 12611
	private SceneDirectoryData m_sceneDirectory;

	// Token: 0x04003144 RID: 12612
	private AmbiControlsMappingData m_unsidedAmbiMapping;

	// Token: 0x04003145 RID: 12613
	private MapAvatarControls m_avatar;

	// Token: 0x04003146 RID: 12614
	private int m_levelIndex;

	// Token: 0x04003147 RID: 12615
	private LobbyUIController m_lobby;

	// Token: 0x04003148 RID: 12616
	private PauseMenuManager m_pauseManager;

	// Token: 0x04003149 RID: 12617
	private bool m_active = true;
}
