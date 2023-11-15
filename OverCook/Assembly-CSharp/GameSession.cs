using System;
using System.Collections;
using System.Collections.Generic;
using GameModes;
using UnityEngine;

// Token: 0x02000706 RID: 1798
public class GameSession : MonoBehaviour
{
	// Token: 0x170002A5 RID: 677
	// (get) Token: 0x06002228 RID: 8744 RVA: 0x000A5291 File Offset: 0x000A3691
	public HighScoreRepository HighScoreRepository
	{
		get
		{
			return this.m_HighScoreRepository;
		}
	}

	// Token: 0x170002A6 RID: 678
	// (get) Token: 0x06002229 RID: 8745 RVA: 0x000A5299 File Offset: 0x000A3699
	public int DLC
	{
		get
		{
			return this.m_DLCId;
		}
	}

	// Token: 0x170002A7 RID: 679
	// (get) Token: 0x0600222A RID: 8746 RVA: 0x000A52A1 File Offset: 0x000A36A1
	// (set) Token: 0x0600222B RID: 8747 RVA: 0x000A52A9 File Offset: 0x000A36A9
	public int SaveSlot
	{
		get
		{
			return this.m_saveSlot;
		}
		set
		{
			this.m_saveSlot = value;
		}
	}

	// Token: 0x170002A8 RID: 680
	// (get) Token: 0x0600222C RID: 8748 RVA: 0x000A52B2 File Offset: 0x000A36B2
	public GameProgress Progress
	{
		get
		{
			return this.m_progress;
		}
	}

	// Token: 0x170002A9 RID: 681
	// (get) Token: 0x0600222D RID: 8749 RVA: 0x000A52BA File Offset: 0x000A36BA
	// (set) Token: 0x0600222E RID: 8750 RVA: 0x000A52C2 File Offset: 0x000A36C2
	public GameSession.GameLevelSettings LevelSettings
	{
		get
		{
			return this.m_levelSettings;
		}
		set
		{
			this.m_levelSettings = value;
		}
	}

	// Token: 0x170002AA RID: 682
	// (get) Token: 0x0600222F RID: 8751 RVA: 0x000A52CB File Offset: 0x000A36CB
	public GameSession.GameTypeSettings TypeSettings
	{
		get
		{
			return this.m_gameTypeSettings;
		}
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x000A52D3 File Offset: 0x000A36D3
	private void Awake()
	{
		this.m_saveManager = GameUtils.RequireManager<SaveManager>();
		this.m_progress = base.gameObject.RequireComponentRecursive<GameProgress>();
		this.m_progress.SetSession(this);
		this.m_HighScoreRepository.Initialise(this.m_DLCId);
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x000A530E File Offset: 0x000A370E
	private void OnDestroy()
	{
		this.m_HighScoreRepository.Shutdown();
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x000A531C File Offset: 0x000A371C
	private IEnumerator SaveSessionRoutine(SaveSystemCallback _callback = null)
	{
		this.m_internalGameModeSessionConfig.Save(this.m_saveManager.GetMetaGameProgress().SaveData);
		yield return this.m_saveManager.SaveData(this.GetSaveMode(), this.m_saveSlot, this.m_DLCId, _callback);
		yield break;
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x000A533E File Offset: 0x000A373E
	public void SaveSession(SaveSystemCallback _callback = null)
	{
		if (this.IsSaveable())
		{
			base.StartCoroutine(this.SaveSessionRoutine(_callback));
		}
		else if (_callback != null)
		{
			_callback(new SaveSystemStatus(SaveSystemStatus.SaveStatus.Complete, SaveLoadResult.NotSaveable));
		}
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x000A5374 File Offset: 0x000A3774
	private bool IsLoadable()
	{
		return this.m_saveManager != null && this.m_saveManager.ProfileLoaded && this.m_saveSlot != -1 && this.m_gameTypeSettings.Type == GameSession.GameType.Cooperative && ClientGameSetup.Mode != GameMode.Party && ClientGameSetup.Mode != GameMode.Versus;
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x000A53DC File Offset: 0x000A37DC
	private bool IsSaveable()
	{
		return this.m_saveManager != null && this.m_saveManager.ProfileLoaded && this.m_saveSlot != -1 && (this.m_progress.UseSlaveSlot || !ConnectionStatus.IsInSession() || ConnectionStatus.IsHost()) && this.m_gameTypeSettings.Type == GameSession.GameType.Cooperative && ClientGameSetup.Mode != GameMode.Party && ClientGameSetup.Mode != GameMode.Versus;
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x000A5466 File Offset: 0x000A3866
	private SaveMode GetSaveMode()
	{
		if (this.m_gameTypeSettings.Type == GameSession.GameType.Cooperative)
		{
			return SaveMode.Main;
		}
		return SaveMode.Main;
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x000A547C File Offset: 0x000A387C
	public IEnumerator HasSaveFile(ReturnValue<SaveLoadResult> _return)
	{
		if (this.IsLoadable())
		{
			IEnumerator run = this.m_saveManager.HasSaveFile(this.GetSaveMode(), this.m_saveSlot, this.m_DLCId, _return);
			while (run.MoveNext())
			{
				yield return null;
			}
		}
		else
		{
			_return.Value = SaveLoadResult.NotSaveable;
		}
		yield break;
	}

	// Token: 0x06002238 RID: 8760 RVA: 0x000A549E File Offset: 0x000A389E
	public void DeleteSave()
	{
		if (this.IsSaveable())
		{
			this.m_saveManager.DeleteSave(this.GetSaveMode(), this.m_saveSlot, this.m_DLCId, null);
		}
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x000A54CC File Offset: 0x000A38CC
	public IEnumerator<SaveLoadResult?> LoadSession()
	{
		if (this.IsLoadable())
		{
			IEnumerator<SaveLoadResult?> routine = this.m_saveManager.LoadSave(this.GetSaveMode(), this.m_saveSlot, this.m_DLCId);
			while (routine.MoveNext())
			{
				yield return null;
			}
			yield return routine.Current;
			this.m_internalGameModeSessionConfig.Load(this.m_saveManager.GetMetaGameProgress().SaveData);
			this.m_gameModeSessionConfig.Copy(this.m_internalGameModeSessionConfig);
		}
		else
		{
			yield return new SaveLoadResult?(SaveLoadResult.Exists);
		}
		yield break;
	}

	// Token: 0x170002AB RID: 683
	// (get) Token: 0x0600223A RID: 8762 RVA: 0x000A54E7 File Offset: 0x000A38E7
	// (set) Token: 0x0600223B RID: 8763 RVA: 0x000A54EF File Offset: 0x000A38EF
	public bool PendingGameModeSessionConfigChanges { get; private set; }

	// Token: 0x170002AC RID: 684
	// (get) Token: 0x0600223C RID: 8764 RVA: 0x000A54F8 File Offset: 0x000A38F8
	// (set) Token: 0x0600223D RID: 8765 RVA: 0x000A5500 File Offset: 0x000A3900
	public SessionConfig GameModeSessionConfig
	{
		get
		{
			return this.m_gameModeSessionConfig;
		}
		set
		{
			this.PendingGameModeSessionConfigChanges = false;
			this.m_gameModeSessionConfig = value;
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				this.m_internalGameModeSessionConfig.Copy(this.m_gameModeSessionConfig);
				this.m_saveManager.RegisterOnIdle(delegate
				{
					this.SaveSession(null);
				});
			}
			if (this.OnGameModeSessionConfigChanged != null)
			{
				this.OnGameModeSessionConfigChanged(this.m_gameModeSessionConfig);
			}
		}
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x000A5574 File Offset: 0x000A3974
	public void CommitGameModeSessionConfig()
	{
		if (this.PendingGameModeSessionConfigChanges)
		{
			this.PendingGameModeSessionConfigChanges = false;
			this.m_internalGameModeSessionConfig.Copy(this.m_gameModeSessionConfig);
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				this.m_saveManager.RegisterOnIdle(delegate
				{
					this.SaveSession(null);
				});
				ServerMessenger.SendHostModeConfigChanged(this.m_internalGameModeSessionConfig);
				if (this.OnGameModeSessionConfigChanged != null)
				{
					this.OnGameModeSessionConfigChanged(this.m_internalGameModeSessionConfig);
				}
			}
		}
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x000A55F8 File Offset: 0x000A39F8
	public void RevertGameModeSessionConfig()
	{
		if (this.PendingGameModeSessionConfigChanges)
		{
			this.PendingGameModeSessionConfigChanges = false;
			this.m_gameModeSessionConfig.Copy(this.m_internalGameModeSessionConfig);
			if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
			{
				this.m_saveManager.RegisterOnIdle(delegate
				{
					this.SaveSession(null);
				});
			}
		}
	}

	// Token: 0x170002AD RID: 685
	// (get) Token: 0x06002240 RID: 8768 RVA: 0x000A5653 File Offset: 0x000A3A53
	// (set) Token: 0x06002241 RID: 8769 RVA: 0x000A5660 File Offset: 0x000A3A60
	public Kind GameModeKind
	{
		get
		{
			return this.m_gameModeSessionConfig.m_kind;
		}
		set
		{
			this.PendingGameModeSessionConfigChanges = true;
			this.m_gameModeSessionConfig.m_kind = value;
		}
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x000A5675 File Offset: 0x000A3A75
	public bool GetGameModeSetting(SettingKind kind)
	{
		return this.m_gameModeSessionConfig.m_settings[(int)kind];
	}

	// Token: 0x06002243 RID: 8771 RVA: 0x000A5684 File Offset: 0x000A3A84
	public bool SetGameModeSetting(SettingKind kind, bool value)
	{
		bool result = this.m_gameModeSessionConfig.m_settings[(int)kind];
		this.m_gameModeSessionConfig.m_settings[(int)kind] = value;
		this.PendingGameModeSessionConfigChanges = true;
		return result;
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x000A56B8 File Offset: 0x000A3AB8
	public IServerMode GetGameModeServer(KitchenLevelConfigBase levelConfig)
	{
		switch (this.GameModeKind)
		{
		case Kind.Campaign:
			return new ServerCampaignMode(levelConfig.m_campaignConfig);
		case Kind.Practice:
			return new ServerPracticeMode(levelConfig.m_practiceConfig);
		case Kind.Survival:
			return new ServerSurvivalMode(levelConfig.m_survivalConfig);
		default:
			return null;
		}
	}

	// Token: 0x06002245 RID: 8773 RVA: 0x000A5708 File Offset: 0x000A3B08
	public IClientMode GetGameModeClient(KitchenLevelConfigBase levelConfig)
	{
		switch (this.GameModeKind)
		{
		case Kind.Campaign:
			return new ClientCampaignMode(levelConfig.m_campaignConfig);
		case Kind.Practice:
			return new ClientPracticeMode(levelConfig.m_practiceConfig);
		case Kind.Survival:
			return new ClientSurvivalMode(levelConfig.m_survivalConfig);
		default:
			return null;
		}
	}

	// Token: 0x06002246 RID: 8774 RVA: 0x000A5758 File Offset: 0x000A3B58
	public void FillShownMetaDialogStatus()
	{
		MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
		for (int i = 0; i < 2; i++)
		{
			this.m_shownMetaDialogs[i] = metaGameProgress.HasShownMetaDialog((MetaGameProgress.MetaDialogType)i);
		}
	}

	// Token: 0x06002247 RID: 8775 RVA: 0x000A578C File Offset: 0x000A3B8C
	public bool HasShownMetaDialog(MetaGameProgress.MetaDialogType _type)
	{
		return this.m_shownMetaDialogs[(int)_type];
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x000A5796 File Offset: 0x000A3B96
	public void SetMetaDialogShown(MetaGameProgress.MetaDialogType _type)
	{
		this.m_shownMetaDialogs[(int)_type] = true;
	}

	// Token: 0x04001A54 RID: 6740
	[HideInInspector]
	[SerializeField]
	private uint m_DLCAppId;

	// Token: 0x04001A55 RID: 6741
	[HideInInspector]
	[SerializeField]
	private string m_DLCXB1ProductId = string.Empty;

	// Token: 0x04001A56 RID: 6742
	[SerializeField]
	private GameSession.GameTypeSettings m_gameTypeSettings = new GameSession.GameTypeSettings();

	// Token: 0x04001A57 RID: 6743
	[SerializeField]
	private int m_DLCId;

	// Token: 0x04001A58 RID: 6744
	private SaveManager m_saveManager;

	// Token: 0x04001A59 RID: 6745
	private GameProgress m_progress;

	// Token: 0x04001A5A RID: 6746
	private GameSession.GameLevelSettings m_levelSettings = new GameSession.GameLevelSettings();

	// Token: 0x04001A5B RID: 6747
	private int m_saveSlot;

	// Token: 0x04001A5C RID: 6748
	public const int c_invalidSaveSlot = -1;

	// Token: 0x04001A5D RID: 6749
	[NonSerialized]
	public bool[] m_shownMetaDialogs = new bool[2];

	// Token: 0x04001A5E RID: 6750
	private HighScoreRepository m_HighScoreRepository = new HighScoreRepository();

	// Token: 0x04001A5F RID: 6751
	public bool MarkedForDeath;

	// Token: 0x04001A60 RID: 6752
	public OnSessionConfigChanged OnGameModeSessionConfigChanged;

	// Token: 0x04001A61 RID: 6753
	private SessionConfig m_gameModeSessionConfig = new SessionConfig();

	// Token: 0x04001A62 RID: 6754
	private SessionConfig m_internalGameModeSessionConfig = new SessionConfig();

	// Token: 0x02000707 RID: 1799
	public enum GameType
	{
		// Token: 0x04001A65 RID: 6757
		Cooperative,
		// Token: 0x04001A66 RID: 6758
		Competitive
	}

	// Token: 0x02000708 RID: 1800
	[Serializable]
	public class GameTypeSettings
	{
		// Token: 0x04001A67 RID: 6759
		public GameSession.GameType Type;

		// Token: 0x04001A68 RID: 6760
		[SceneName]
		public string WorldMapScene = "WorldMap";
	}

	// Token: 0x02000709 RID: 1801
	[Serializable]
	public class GameLevelSettings
	{
		// Token: 0x04001A69 RID: 6761
		public SceneDirectoryData.PerPlayerCountDirectoryEntry SceneDirectoryVarientEntry;
	}

	// Token: 0x0200070A RID: 1802
	[Serializable]
	public class SelectedChefData
	{
		// Token: 0x0600224E RID: 8782 RVA: 0x000A57D7 File Offset: 0x000A3BD7
		public SelectedChefData(ChefAvatarData _chefAvatarData, ChefColourData _colourData)
		{
			this.Character = _chefAvatarData;
			this.Colour = _colourData;
		}

		// Token: 0x04001A6A RID: 6762
		public ChefAvatarData Character;

		// Token: 0x04001A6B RID: 6763
		public ChefColourData Colour;
	}
}
