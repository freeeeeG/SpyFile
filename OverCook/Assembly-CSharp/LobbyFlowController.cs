using System;
using Team17.Online;
using UnityEngine;

// Token: 0x020006E7 RID: 1767
public class LobbyFlowController : MonoBehaviour
{
	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06002189 RID: 8585 RVA: 0x000A27EF File Offset: 0x000A0BEF
	public static LobbyFlowController Instance
	{
		get
		{
			return LobbyFlowController.s_instance;
		}
	}

	// Token: 0x0600218A RID: 8586 RVA: 0x000A27F8 File Offset: 0x000A0BF8
	private void Awake()
	{
		if (LobbyFlowController.s_instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			LobbyFlowController.s_instance = this;
		}
		this.m_noTeamColourIndex = this.GetColourIndex(this.m_noTeam);
		this.m_redTeamColourIndex = this.GetColourIndex(this.m_red);
		this.m_blueTeamColourIndex = this.GetColourIndex(this.m_blue);
		this.SetupSceneDirectories();
		if (base.gameObject.GetComponent<ClientLobbyFlowController>() == null)
		{
			base.gameObject.AddComponent<ClientLobbyFlowController>();
		}
		PlayerInputLookup.ResetToDefaultInputConfig();
		InviteMonitor.SwitchHandlerType(InviteMonitor.HandlerType.Gameplay);
	}

	// Token: 0x0600218B RID: 8587 RVA: 0x000A288F File Offset: 0x000A0C8F
	private void Start()
	{
		this.DisableUnusableThemes();
	}

	// Token: 0x0600218C RID: 8588 RVA: 0x000A2897 File Offset: 0x000A0C97
	public bool IsLocalState(LobbyFlowController.LobbyState _state)
	{
		return _state == LobbyFlowController.LobbyState.LocalSetup || _state == LobbyFlowController.LobbyState.LocalThemeSelection || _state == LobbyFlowController.LobbyState.LocalThemeSelected;
	}

	// Token: 0x0600218D RID: 8589 RVA: 0x000A28AE File Offset: 0x000A0CAE
	private void OnDestroy()
	{
		if (LobbyFlowController.s_instance == this)
		{
			LobbyFlowController.s_instance = null;
		}
	}

	// Token: 0x0600218E RID: 8590 RVA: 0x000A28C8 File Offset: 0x000A0CC8
	public void UpdateLegend(bool coop)
	{
		if (coop)
		{
			if (!UserSystemUtils.AnySplitPadUsers())
			{
				this.m_legend.SetLocalisedTextCatchAll(this.m_CoopLegend);
			}
			else
			{
				this.m_legend.SetLocalisedTextCatchAll(this.m_CoopLegendNoEmote);
			}
		}
		else if (!UserSystemUtils.AnySplitPadUsers())
		{
			this.m_legend.SetLocalisedTextCatchAll(this.m_versusLegend);
		}
		else
		{
			this.m_legend.SetLocalisedTextCatchAll(this.m_VersusLegendNoEmote);
		}
	}

	// Token: 0x0600218F RID: 8591 RVA: 0x000A2944 File Offset: 0x000A0D44
	public uint GetColourIndex(ChefColourData _colourData)
	{
		AvatarDirectoryData avatarDirectoryData = GameUtils.GetAvatarDirectoryData();
		uint num = 0U;
		while ((ulong)num < (ulong)((long)avatarDirectoryData.Colours.Length))
		{
			if (avatarDirectoryData.Colours[(int)((UIntPtr)num)] == _colourData)
			{
				return num;
			}
			num += 1U;
		}
		return 7U;
	}

	// Token: 0x06002190 RID: 8592 RVA: 0x000A298C File Offset: 0x000A0D8C
	public void RefreshUserColour(User _user, bool _isCoop)
	{
		if (_user == null)
		{
			return;
		}
		uint colour = _user.Colour;
		if (_isCoop)
		{
			AvatarDirectoryData avatarDirectoryData = GameUtils.GetAvatarDirectoryData();
			if (avatarDirectoryData != null)
			{
				int num = ClientUserSystem.m_Users._items.FindIndex_Predicate((User u) => u == _user);
				if (num != -1)
				{
					colour = (uint)num;
				}
			}
		}
		else
		{
			TeamID team = _user.Team;
			if (team != TeamID.None)
			{
				if (team != TeamID.One)
				{
					if (team == TeamID.Two)
					{
						colour = this.m_blueTeamColourIndex;
					}
				}
				else
				{
					colour = this.m_redTeamColourIndex;
				}
			}
			else
			{
				colour = this.m_noTeamColourIndex;
			}
		}
		_user.Colour = colour;
	}

	// Token: 0x06002191 RID: 8593 RVA: 0x000A2A5C File Offset: 0x000A0E5C
	public void RefreshUserColours(bool _isCoop)
	{
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			this.RefreshUserColour(ClientUserSystem.m_Users._items[i], _isCoop);
		}
	}

	// Token: 0x06002192 RID: 8594 RVA: 0x000A2A98 File Offset: 0x000A0E98
	public bool UnanimousSelection(LobbyFlowController.ThemeChoice[] _userChoices)
	{
		SceneDirectoryData.LevelTheme levelTheme = SceneDirectoryData.LevelTheme.Count;
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			LobbyFlowController.ThemeChoice themeChoice = _userChoices[i];
			if (themeChoice != null)
			{
				if (themeChoice.m_theme == SceneDirectoryData.LevelTheme.Count || themeChoice.m_theme == SceneDirectoryData.LevelTheme.Null)
				{
					return false;
				}
				if (levelTheme == SceneDirectoryData.LevelTheme.Count)
				{
					levelTheme = themeChoice.m_theme;
				}
				else if (levelTheme != themeChoice.m_theme)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002193 RID: 8595 RVA: 0x000A2B0B File Offset: 0x000A0F0B
	private bool SessionListCleanUp(GameSession _session)
	{
		return _session == null;
	}

	// Token: 0x06002194 RID: 8596 RVA: 0x000A2B1C File Offset: 0x000A0F1C
	private void SetupSceneDirectories()
	{
		Converter<GameSession, SceneDirectoryData> converter = delegate(GameSession _session)
		{
			GameProgress gameProgress = _session.gameObject.RequireComponentRecursive<GameProgress>();
			return gameProgress.GetSceneDirectory();
		};
		this.m_CoopGameSessionPrefabs = this.m_CoopGameSessionData.AllData.AllRemoved_Predicate(new Predicate<GameSession>(this.SessionListCleanUp));
		this.m_coopSceneDirectories = this.m_CoopGameSessionPrefabs.ConvertAll(converter).AllRemoved_Predicate((SceneDirectoryData x) => x == null);
		this.m_CompetitiveGameSessionPrefabs = this.m_CompetitiveGameSessionData.AllData.AllRemoved_Predicate(new Predicate<GameSession>(this.SessionListCleanUp));
		this.m_vsSceneDirectories = this.m_CompetitiveGameSessionPrefabs.ConvertAll(converter).AllRemoved_Predicate((SceneDirectoryData x) => x == null);
	}

	// Token: 0x06002195 RID: 8597 RVA: 0x000A2BF4 File Offset: 0x000A0FF4
	public SceneDirectoryData[] GetSceneDirectories()
	{
		GameMode mode = ClientGameSetup.Mode;
		if (mode == GameMode.Party)
		{
			return this.m_coopSceneDirectories;
		}
		if (mode != GameMode.Versus)
		{
			return null;
		}
		return this.m_vsSceneDirectories;
	}

	// Token: 0x06002196 RID: 8598 RVA: 0x000A2C2C File Offset: 0x000A102C
	public GameSession CreateLobbySession(GameSession.GameType _gameType, int _dlcID)
	{
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession != null)
		{
			UnityEngine.Object.DestroyImmediate(gameSession.gameObject);
		}
		GameSession[] array = null;
		if (_gameType != GameSession.GameType.Cooperative)
		{
			if (_gameType == GameSession.GameType.Competitive)
			{
				array = this.m_CompetitiveGameSessionPrefabs;
			}
		}
		else
		{
			array = this.m_CoopGameSessionPrefabs;
		}
		GameSession gameSession2 = Array.Find<GameSession>(array, (GameSession x) => x.DLC == _dlcID);
		if (gameSession2 != null)
		{
			GameObject gameObject = gameSession2.gameObject.InstantiateOnParent(null, true);
			gameObject.name = string.Format("LobbyGameSession_{0}_{1}", _gameType.ToString(), _dlcID);
			GameSession gameSession3 = gameObject.RequireComponent<GameSession>();
			gameSession3.TypeSettings.Type = _gameType;
			gameSession3.TypeSettings.WorldMapScene = "Lobbies";
			return gameSession3;
		}
		return null;
	}

	// Token: 0x06002197 RID: 8599 RVA: 0x000A2D14 File Offset: 0x000A1114
	public int GetDLCIDFromSceneDirIndex(GameSession.GameType _gameType, int _idx)
	{
		GameSession[] array = null;
		if (_gameType != GameSession.GameType.Cooperative)
		{
			if (_gameType == GameSession.GameType.Competitive)
			{
				array = this.m_CompetitiveGameSessionPrefabs;
			}
		}
		else
		{
			array = this.m_CoopGameSessionPrefabs;
		}
		return array[_idx].DLC;
	}

	// Token: 0x06002198 RID: 8600 RVA: 0x000A2D58 File Offset: 0x000A1158
	private void DisableUnusableThemes()
	{
		int num = 64;
		foreach (SceneDirectoryData sceneDirectoryData in this.GetSceneDirectories())
		{
			for (int j = 0; j < sceneDirectoryData.Scenes.Length; j++)
			{
				num |= 1 << (int)sceneDirectoryData.Scenes[j].Theme;
			}
		}
		for (int k = 0; k < 22; k++)
		{
			SceneDirectoryData.LevelTheme levelTheme = (SceneDirectoryData.LevelTheme)k;
			if (!MaskUtils.HasFlag<SceneDirectoryData.LevelTheme>(num, levelTheme))
			{
				this.m_themeSelectMenu.DisallowTheme(levelTheme);
			}
		}
	}

	// Token: 0x0400199B RID: 6555
	private static LobbyFlowController s_instance;

	// Token: 0x0400199C RID: 6556
	public LobbyFlowController.LobbyName m_lobbyNames;

	// Token: 0x0400199D RID: 6557
	public LobbyFlowController.StateStrings m_stateStrings;

	// Token: 0x0400199E RID: 6558
	public T17Text m_timerText;

	// Token: 0x0400199F RID: 6559
	public GameObject m_chosenThemes;

	// Token: 0x040019A0 RID: 6560
	public ThemeChoiceElement[] m_themeSelections = new ThemeChoiceElement[4];

	// Token: 0x040019A1 RID: 6561
	[HideInInspector]
	public ThemeSelectButton m_selectedTheme;

	// Token: 0x040019A2 RID: 6562
	public float m_themeSelectionDuration = 2f;

	// Token: 0x040019A3 RID: 6563
	public GameObject m_localPlayerNotification;

	// Token: 0x040019A4 RID: 6564
	public GameObject m_netPlayerNotification;

	// Token: 0x040019A5 RID: 6565
	public ThemeSelectRootMenu m_themeSelectMenu;

	// Token: 0x040019A6 RID: 6566
	public UIPlayerRootMenu m_uiPlayerRoot;

	// Token: 0x040019A7 RID: 6567
	[SerializeField]
	public float m_timeLimit = 45f;

	// Token: 0x040019A8 RID: 6568
	[SerializeField]
	[AssignResource("No_Team", Editorbility.Editable)]
	public ChefColourData m_noTeam;

	// Token: 0x040019A9 RID: 6569
	[SerializeField]
	[AssignResource("Red", Editorbility.Editable)]
	public ChefColourData m_red;

	// Token: 0x040019AA RID: 6570
	[SerializeField]
	[AssignResource("Blue", Editorbility.Editable)]
	public ChefColourData m_blue;

	// Token: 0x040019AB RID: 6571
	[HideInInspector]
	public uint m_noTeamColourIndex = 7U;

	// Token: 0x040019AC RID: 6572
	[HideInInspector]
	public uint m_redTeamColourIndex = 7U;

	// Token: 0x040019AD RID: 6573
	[HideInInspector]
	public uint m_blueTeamColourIndex = 7U;

	// Token: 0x040019AE RID: 6574
	[Space]
	[Header("Game Sessions")]
	[SerializeField]
	public LobbyFlowController.DLCSerializedGameSessionData m_CoopGameSessionData = new LobbyFlowController.DLCSerializedGameSessionData();

	// Token: 0x040019AF RID: 6575
	[SerializeField]
	public LobbyFlowController.DLCSerializedGameSessionData m_CompetitiveGameSessionData = new LobbyFlowController.DLCSerializedGameSessionData();

	// Token: 0x040019B0 RID: 6576
	private GameSession[] m_CoopGameSessionPrefabs;

	// Token: 0x040019B1 RID: 6577
	private GameSession[] m_CompetitiveGameSessionPrefabs;

	// Token: 0x040019B2 RID: 6578
	private SceneDirectoryData[] m_coopSceneDirectories;

	// Token: 0x040019B3 RID: 6579
	private SceneDirectoryData[] m_vsSceneDirectories;

	// Token: 0x040019B4 RID: 6580
	[Space]
	[SerializeField]
	public float m_selectedEndScale = 1.3f;

	// Token: 0x040019B5 RID: 6581
	public const int c_ThemeSelectionSteps = 40;

	// Token: 0x040019B6 RID: 6582
	public string m_versusLegend;

	// Token: 0x040019B7 RID: 6583
	public string m_CoopLegend;

	// Token: 0x040019B8 RID: 6584
	public string m_VersusLegendNoEmote;

	// Token: 0x040019B9 RID: 6585
	public string m_CoopLegendNoEmote;

	// Token: 0x040019BA RID: 6586
	public T17Text m_legend;

	// Token: 0x020006E8 RID: 1768
	public enum LobbyState
	{
		// Token: 0x040019BF RID: 6591
		PreSetup,
		// Token: 0x040019C0 RID: 6592
		Matchmake,
		// Token: 0x040019C1 RID: 6593
		LocalSetup,
		// Token: 0x040019C2 RID: 6594
		OnlineSetup,
		// Token: 0x040019C3 RID: 6595
		LocalThemeSelection,
		// Token: 0x040019C4 RID: 6596
		OnlineThemeSelection,
		// Token: 0x040019C5 RID: 6597
		LocalThemeSelected,
		// Token: 0x040019C6 RID: 6598
		OnlineThemeSelected
	}

	// Token: 0x020006E9 RID: 1769
	public class ThemeChoice
	{
		// Token: 0x040019C7 RID: 6599
		public SceneDirectoryData.LevelTheme m_theme = SceneDirectoryData.LevelTheme.Count;

		// Token: 0x040019C8 RID: 6600
		public int m_chefIndex = -1;
	}

	// Token: 0x020006EA RID: 1770
	[Serializable]
	public class LobbyName
	{
		// Token: 0x0600219F RID: 8607 RVA: 0x000A2E44 File Offset: 0x000A1244
		public void Update(bool _isCoop)
		{
			this.m_coop.SetActive(_isCoop);
			this.m_versus.SetActive(!_isCoop);
		}

		// Token: 0x040019C9 RID: 6601
		public GameObject m_coop;

		// Token: 0x040019CA RID: 6602
		public GameObject m_versus;
	}

	// Token: 0x020006EB RID: 1771
	[Serializable]
	public class StateStrings
	{
		// Token: 0x060021A1 RID: 8609 RVA: 0x000A2E69 File Offset: 0x000A1269
		public void Update(LobbyFlowController.StateStrings.State _state)
		{
			this.m_chooseTheme.SetActive(_state == LobbyFlowController.StateStrings.State.ChooseTheme);
			this.m_waitingOthers.SetActive(_state == LobbyFlowController.StateStrings.State.WaitingForOthers);
			this.m_pickingLevel.SetActive(_state == LobbyFlowController.StateStrings.State.PickingLevel);
		}

		// Token: 0x040019CB RID: 6603
		public GameObject m_chooseTheme;

		// Token: 0x040019CC RID: 6604
		public GameObject m_waitingOthers;

		// Token: 0x040019CD RID: 6605
		public GameObject m_pickingLevel;

		// Token: 0x020006EC RID: 1772
		public enum State
		{
			// Token: 0x040019CF RID: 6607
			None,
			// Token: 0x040019D0 RID: 6608
			ChooseTheme,
			// Token: 0x040019D1 RID: 6609
			WaitingForOthers,
			// Token: 0x040019D2 RID: 6610
			PickingLevel
		}
	}

	// Token: 0x020006ED RID: 1773
	[Serializable]
	public class DLCSerializedGameSessionData : DLCSerializedData<GameSession>
	{
	}
}
