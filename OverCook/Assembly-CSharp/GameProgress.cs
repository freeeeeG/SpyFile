using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x020006F9 RID: 1785
public class GameProgress : MonoBehaviour
{
	// Token: 0x1700029D RID: 669
	// (get) Token: 0x060021D7 RID: 8663 RVA: 0x000A371C File Offset: 0x000A1B1C
	public bool LoadFirstScene
	{
		get
		{
			return this.m_loadFirstScene;
		}
	}

	// Token: 0x1700029E RID: 670
	// (get) Token: 0x060021D8 RID: 8664 RVA: 0x000A3724 File Offset: 0x000A1B24
	public int FirstSceneIndex
	{
		get
		{
			return this.m_firstLevel;
		}
	}

	// Token: 0x1700029F RID: 671
	// (get) Token: 0x060021D9 RID: 8665 RVA: 0x000A372C File Offset: 0x000A1B2C
	// (set) Token: 0x060021DA RID: 8666 RVA: 0x000A3734 File Offset: 0x000A1B34
	public bool UseSlaveSlot
	{
		get
		{
			return this.m_useSlaveSlot;
		}
		set
		{
			this.m_useSlaveSlot = value;
		}
	}

	// Token: 0x170002A0 RID: 672
	// (get) Token: 0x060021DB RID: 8667 RVA: 0x000A373D File Offset: 0x000A1B3D
	public int[] LevelChainEnds
	{
		get
		{
			return this.m_levelChainEnds;
		}
	}

	// Token: 0x060021DC RID: 8668 RVA: 0x000A3745 File Offset: 0x000A1B45
	public void SetSession(GameSession _session)
	{
		this.m_session = _session;
	}

	// Token: 0x060021DD RID: 8669 RVA: 0x000A374E File Offset: 0x000A1B4E
	private void Awake()
	{
		this.m_localSaveData.FillOut(this.m_sceneDirectory);
		this.SetupLevelChainEnds();
	}

	// Token: 0x060021DE RID: 8670 RVA: 0x000A3768 File Offset: 0x000A1B68
	private void SetupLevelChainEnds()
	{
		List<int> list = new List<int>();
		for (int i = 0; i < this.m_sceneDirectory.Scenes.Length; i++)
		{
			if (this.m_sceneDirectory.Scenes[i].LevelChainEnd)
			{
				list.Add(i);
			}
		}
		this.m_levelChainEnds = list.ToArray();
	}

	// Token: 0x060021DF RID: 8671 RVA: 0x000A37C4 File Offset: 0x000A1BC4
	private int GetSceneIndex(string _sceneName)
	{
		return this.m_sceneDirectory.Scenes.FindIndex_Predicate((SceneDirectoryData.SceneDirectoryEntry x) => x.SceneVarients.FindIndex_Predicate((SceneDirectoryData.PerPlayerCountDirectoryEntry y) => y.SceneName == _sceneName) != -1);
	}

	// Token: 0x060021E0 RID: 8672 RVA: 0x000A37FC File Offset: 0x000A1BFC
	public GameProgress.GameProgressData.LevelProgress GetProgress(string _sceneName)
	{
		GameProgress.GameProgressData saveData = this.SaveData;
		int sceneIndex = this.GetSceneIndex(_sceneName);
		return saveData.GetLevelProgress(sceneIndex);
	}

	// Token: 0x060021E1 RID: 8673 RVA: 0x000A3820 File Offset: 0x000A1C20
	public GameProgress.GameProgressData.LevelProgress GetProgress(int _levelId)
	{
		GameProgress.GameProgressData saveData = this.SaveData;
		return saveData.GetLevelProgress(_levelId);
	}

	// Token: 0x060021E2 RID: 8674 RVA: 0x000A383C File Offset: 0x000A1C3C
	public GameProgress.GameProgressData.SwitchState GetSwitchState(int _switchId)
	{
		GameProgress.GameProgressData saveData = this.SaveData;
		return saveData.GetSwitchState(_switchId);
	}

	// Token: 0x060021E3 RID: 8675 RVA: 0x000A3858 File Offset: 0x000A1C58
	public GameProgress.GameProgressData.TeleportalState GetTeleportalState(SceneDirectoryData.World _world)
	{
		GameProgress.GameProgressData saveData = this.SaveData;
		return saveData.GetTeleportalState(_world);
	}

	// Token: 0x060021E4 RID: 8676 RVA: 0x000A3873 File Offset: 0x000A1C73
	public SceneDirectoryData GetSceneDirectory()
	{
		return this.m_sceneDirectory;
	}

	// Token: 0x060021E5 RID: 8677 RVA: 0x000A387B File Offset: 0x000A1C7B
	public AvatarDirectoryData GetAvatarDirectory()
	{
		return this.m_avatarDirectory;
	}

	// Token: 0x060021E6 RID: 8678 RVA: 0x000A3883 File Offset: 0x000A1C83
	public void SetLastLevelEntered(int _levelIndex)
	{
		this.SaveData.LastLevelEntered = _levelIndex;
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			this.m_localSaveData.LastLevelEntered = _levelIndex;
		}
	}

	// Token: 0x060021E7 RID: 8679 RVA: 0x000A38B4 File Offset: 0x000A1CB4
	public void RecordLevelScore(GameProgress.HighScores.Score score)
	{
		GameProgress.GameProgressData.LevelProgress levelProgress = this.SaveableData.GetLevelProgress(score.iLevelID);
		GameUtils.GetGameSession().HighScoreRepository.LevelProgress(score);
		levelProgress.HighScore = Mathf.Max(levelProgress.HighScore, score.iHighScore);
		levelProgress.SurvivalModeTime = Mathf.Max(levelProgress.SurvivalModeTime, score.iSurvivalModeTime);
	}

	// Token: 0x060021E8 RID: 8680 RVA: 0x000A3914 File Offset: 0x000A1D14
	public void RecordLevelProgress(int _levelIndex, int _starRating, ref GameProgress.UnlockData[] _unlocks)
	{
		bool flag = _starRating > 0 || !this.m_sceneDirectory.Scenes[_levelIndex].HasScoreBoundaries;
		if (ClientGameSetup.Mode == GameMode.Campaign)
		{
			this.ApplyLevelProgress(this.SaveData, _levelIndex, _starRating, flag);
			if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
			{
				this.ApplyLevelProgress(this.m_localSaveData, _levelIndex, _starRating, flag);
			}
			_unlocks = this.FindNewUnlocks();
			for (int i = 0; i < _unlocks.Length; i++)
			{
				_unlocks[i].Unlock(this);
			}
			if (this.CanUnlockNewGamePlus(this.SaveData))
			{
				this.UnlockNewGamePlus(this.SaveData);
			}
		}
		if (flag)
		{
			SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = this.m_sceneDirectory.Scenes[_levelIndex];
			OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
			if (overcookedAchievementManager != null && ClientGameSetup.Mode == GameMode.Campaign && sceneDirectoryEntry.World != SceneDirectoryData.World.Invalid)
			{
				if (sceneDirectoryEntry.World == SceneDirectoryData.World.Tutorial)
				{
					overcookedAchievementManager.AddIDStat(50, 1, ControlPadInput.PadNum.One);
				}
				else if (sceneDirectoryEntry.World >= SceneDirectoryData.World.One && sceneDirectoryEntry.World < SceneDirectoryData.World.Seven)
				{
					overcookedAchievementManager.AddIDStat(23, _levelIndex, ControlPadInput.PadNum.One);
				}
				int num = 3;
				if (_starRating >= num)
				{
					overcookedAchievementManager.AddIDStat((int)(50 + sceneDirectoryEntry.World), _levelIndex, ControlPadInput.PadNum.One);
				}
			}
		}
	}

	// Token: 0x060021E9 RID: 8681 RVA: 0x000A3A58 File Offset: 0x000A1E58
	private void ApplyLevelProgress(GameProgress.GameProgressData _saveData, int _levelIndex, int _starRating, bool _complete)
	{
		GameProgress.GameProgressData.LevelProgress levelProgress = _saveData.GetLevelProgress(_levelIndex);
		if (levelProgress == null || levelProgress.LevelId == -1)
		{
			levelProgress = new GameProgress.GameProgressData.LevelProgress();
			levelProgress.LevelId = _levelIndex;
			ArrayUtils.PushBack<GameProgress.GameProgressData.LevelProgress>(ref _saveData.Levels, levelProgress);
		}
		levelProgress.Completed = _complete;
		if (_complete)
		{
			levelProgress.Purchased = true;
			levelProgress.Revealed = true;
		}
		int num = _starRating;
		if (num > 3 && !levelProgress.NGPEnabled)
		{
			num = 3;
		}
		levelProgress.ScoreStars = Mathf.Max(levelProgress.ScoreStars, num);
		ObjectivesManager objectivesManager = GameUtils.RequestManager<ObjectivesManager>();
		if (objectivesManager != null)
		{
			if (objectivesManager.HasObjectives(true))
			{
				if (objectivesManager.AllObjectivesComplete())
				{
					levelProgress.ObjectivesCompleted = true;
				}
			}
			else
			{
				levelProgress.ObjectivesCompleted = true;
			}
		}
	}

	// Token: 0x060021EA RID: 8682 RVA: 0x000A3B28 File Offset: 0x000A1F28
	public void RecordSwitchRevealed(int _switchId)
	{
		this.ApplySwitchRevealed(this.SaveData, _switchId);
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			this.ApplySwitchRevealed(this.m_localSaveData, _switchId);
		}
		if (ConnectionStatus.IsHost())
		{
			GameSession gameSession = GameUtils.GetGameSession();
			gameSession.FillShownMetaDialogStatus();
			ServerMessenger.GameProgressData(this.SaveData, gameSession.m_shownMetaDialogs);
		}
	}

	// Token: 0x060021EB RID: 8683 RVA: 0x000A3B8C File Offset: 0x000A1F8C
	private void ApplySwitchRevealed(GameProgress.GameProgressData _saveData, int _switchId)
	{
		GameProgress.GameProgressData.SwitchState switchState = _saveData.GetSwitchState(_switchId);
		if (switchState == null || switchState.SwitchId == -1)
		{
			ArrayUtils.PushBack<GameProgress.GameProgressData.SwitchState>(ref _saveData.Switches, new GameProgress.GameProgressData.SwitchState
			{
				SwitchId = _switchId
			});
		}
	}

	// Token: 0x060021EC RID: 8684 RVA: 0x000A3BCC File Offset: 0x000A1FCC
	public void RecordSwitchActivated(int _switchId)
	{
		this.ApplySwitchActivated(this.SaveData, _switchId);
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			this.ApplySwitchActivated(this.m_localSaveData, _switchId);
		}
		if (ConnectionStatus.IsHost())
		{
			GameSession gameSession = GameUtils.GetGameSession();
			gameSession.FillShownMetaDialogStatus();
			ServerMessenger.GameProgressData(this.SaveData, gameSession.m_shownMetaDialogs);
		}
	}

	// Token: 0x060021ED RID: 8685 RVA: 0x000A3C30 File Offset: 0x000A2030
	private void ApplySwitchActivated(GameProgress.GameProgressData _saveData, int _switchId)
	{
		GameProgress.GameProgressData.SwitchState switchState = _saveData.GetSwitchState(_switchId);
		if (switchState == null || switchState.SwitchId == -1)
		{
			switchState = new GameProgress.GameProgressData.SwitchState();
			switchState.SwitchId = _switchId;
			ArrayUtils.PushBack<GameProgress.GameProgressData.SwitchState>(ref _saveData.Switches, switchState);
		}
		switchState.Activated = true;
	}

	// Token: 0x060021EE RID: 8686 RVA: 0x000A3C78 File Offset: 0x000A2078
	public void RecordTeleportalRevealed(SceneDirectoryData.World _world)
	{
		this.ApplyTeleportalRevealed(this.SaveData, _world);
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			this.ApplyTeleportalRevealed(this.m_localSaveData, _world);
		}
		if (ConnectionStatus.IsHost())
		{
			GameSession gameSession = GameUtils.GetGameSession();
			gameSession.FillShownMetaDialogStatus();
			ServerMessenger.GameProgressData(this.SaveData, gameSession.m_shownMetaDialogs);
		}
	}

	// Token: 0x060021EF RID: 8687 RVA: 0x000A3CDC File Offset: 0x000A20DC
	private void ApplyTeleportalRevealed(GameProgress.GameProgressData _saveData, SceneDirectoryData.World _world)
	{
		GameProgress.GameProgressData.TeleportalState teleportalState = _saveData.GetTeleportalState(_world);
		if (teleportalState == null || teleportalState.World == SceneDirectoryData.World.COUNT)
		{
			ArrayUtils.PushBack<GameProgress.GameProgressData.TeleportalState>(ref _saveData.Teleportals, new GameProgress.GameProgressData.TeleportalState
			{
				World = _world
			});
		}
	}

	// Token: 0x060021F0 RID: 8688 RVA: 0x000A3D20 File Offset: 0x000A2120
	public int GetStarTotal()
	{
		GameProgress.GameProgressData saveData = this.SaveData;
		return saveData.GetStarTotal();
	}

	// Token: 0x060021F1 RID: 8689 RVA: 0x000A3D3C File Offset: 0x000A213C
	public int GetCompletedLevelsCount()
	{
		GameProgress.GameProgressData saveData = this.SaveData;
		int num = 0;
		for (int i = 0; i < saveData.Levels.Length; i++)
		{
			if (saveData.Levels[i].Completed)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x060021F2 RID: 8690 RVA: 0x000A3D84 File Offset: 0x000A2184
	public void GetLocalScores(ref GameProgress.HighScores highScores)
	{
		highScores.Scores.Clear();
		GameProgress.GameProgressData localSaveData = this.m_localSaveData;
		for (int i = 0; i < localSaveData.Levels.Length; i++)
		{
			GameProgress.GameProgressData.LevelProgress levelProgress = localSaveData.Levels[i];
			int iHighScore = (levelProgress.HighScore == int.MinValue) ? 65535 : levelProgress.HighScore;
			highScores.Scores.Add(new GameProgress.HighScores.Score
			{
				iLevelID = levelProgress.LevelId,
				iHighScore = iHighScore,
				iSurvivalModeTime = levelProgress.SurvivalModeTime
			});
		}
	}

	// Token: 0x170002A1 RID: 673
	// (get) Token: 0x060021F3 RID: 8691 RVA: 0x000A3E1E File Offset: 0x000A221E
	public GameProgress.GameProgressData SaveData
	{
		get
		{
			if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
			{
				return this.m_remoteSaveData;
			}
			return this.m_localSaveData;
		}
	}

	// Token: 0x170002A2 RID: 674
	// (get) Token: 0x060021F4 RID: 8692 RVA: 0x000A3E41 File Offset: 0x000A2241
	public GameProgress.GameProgressData SaveableData
	{
		get
		{
			return this.m_localSaveData;
		}
	}

	// Token: 0x060021F5 RID: 8693 RVA: 0x000A3E49 File Offset: 0x000A2249
	public void Load(GameProgress.GameProgressData _data)
	{
		this.m_localSaveData = _data;
		this.m_localSaveData.FillOut(this.m_sceneDirectory);
	}

	// Token: 0x060021F6 RID: 8694 RVA: 0x000A3E63 File Offset: 0x000A2263
	public void LoadFromNetwork(GameProgress.GameProgressData _data)
	{
		this.m_remoteSaveData = _data;
	}

	// Token: 0x060021F7 RID: 8695 RVA: 0x000A3E6C File Offset: 0x000A226C
	public bool Load(byte[] _data)
	{
		GameProgress.GameProgressData gameProgressData = new GameProgress.GameProgressData();
		if (!gameProgressData.ByteLoad(_data))
		{
			return false;
		}
		this.Load(gameProgressData);
		return true;
	}

	// Token: 0x060021F8 RID: 8696 RVA: 0x000A3E98 File Offset: 0x000A2298
	private GameProgress.UnlockData[] FindNewUnlocks()
	{
		GameProgress.<FindNewUnlocks>c__AnonStorey1 <FindNewUnlocks>c__AnonStorey = new GameProgress.<FindNewUnlocks>c__AnonStorey1();
		<FindNewUnlocks>c__AnonStorey.$this = this;
		<FindNewUnlocks>c__AnonStorey.debugConfig = GameUtils.GetDebugConfig();
		<FindNewUnlocks>c__AnonStorey.totalStars = this.GetStarTotal();
		Predicate<GameProgress.UnlockData> match = delegate(GameProgress.UnlockData _unlockData)
		{
			if (_unlockData.Type == GameProgress.UnlockData.UnlockType.CompetitiveLevel && <FindNewUnlocks>c__AnonStorey.debugConfig.m_supressCompetitveMode)
			{
				return false;
			}
			if (_unlockData.IsUnlocked(<FindNewUnlocks>c__AnonStorey.$this))
			{
				return false;
			}
			GameProgress.UnlockData.Condition condition = _unlockData.Requirement;
			GameProgress.UnlockData.Condition.ConditionType type = condition.Type;
			if (type != GameProgress.UnlockData.Condition.ConditionType.LevelComplete)
			{
				if (type != GameProgress.UnlockData.Condition.ConditionType.LevelStars)
				{
					if (type == GameProgress.UnlockData.Condition.ConditionType.TotalStars)
					{
						if (<FindNewUnlocks>c__AnonStorey.totalStars < condition.RequiredStars)
						{
							return false;
						}
					}
				}
				else if (condition.RequiredLevel != -1)
				{
					if (!<FindNewUnlocks>c__AnonStorey.$this.SaveableData.HasStarsForLevel(condition.RequiredLevel, condition.RequiredStars, <FindNewUnlocks>c__AnonStorey.$this.m_sceneDirectory))
					{
						return false;
					}
					if (condition.CheckPreviousLevels && !<FindNewUnlocks>c__AnonStorey.$this.SaveableData.IsTrueForLevelsRecursive(condition.RequiredLevel, <FindNewUnlocks>c__AnonStorey.$this.m_sceneDirectory, (int x) => <FindNewUnlocks>c__AnonStorey.SaveableData.HasStarsForLevel(x, condition.RequiredStars, <FindNewUnlocks>c__AnonStorey.m_sceneDirectory)))
					{
						return false;
					}
				}
			}
			else if (condition.RequiredLevel != -1)
			{
				if (!<FindNewUnlocks>c__AnonStorey.$this.SaveableData.IsLevelComplete(condition.RequiredLevel))
				{
					return false;
				}
				if (condition.CheckPreviousLevels && !<FindNewUnlocks>c__AnonStorey.$this.SaveableData.IsTrueForLevelsRecursive(condition.RequiredLevel, <FindNewUnlocks>c__AnonStorey.$this.m_sceneDirectory, new Generic<bool, int>(<FindNewUnlocks>c__AnonStorey.$this.SaveableData.IsLevelComplete)))
				{
					return false;
				}
			}
			return true;
		};
		return this.m_unlockData.FindAll(match);
	}

	// Token: 0x060021F9 RID: 8697 RVA: 0x000A3EE4 File Offset: 0x000A22E4
	public bool CanUnlockNewGamePlus(GameProgress.GameProgressData _saveData)
	{
		for (int i = 0; i < this.m_levelChainEnds.Length; i++)
		{
			if (this.CanUnlockNewGamePlusForChainEnd(_saveData, this.m_levelChainEnds[i]))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x060021FA RID: 8698 RVA: 0x000A3F24 File Offset: 0x000A2324
	private bool CanUnlockNewGamePlusForChainEnd(GameProgress.GameProgressData _saveData, int _levelID)
	{
		GameProgress.GameProgressData.LevelProgress levelProgress = _saveData.GetLevelProgress(_levelID);
		return !levelProgress.NGPEnabled && _saveData.IsLevelComplete(_levelID) && _saveData.IsLevelUnlocked(_levelID, false);
	}

	// Token: 0x060021FB RID: 8699 RVA: 0x000A3F60 File Offset: 0x000A2360
	public void UnlockNewGamePlus(GameProgress.GameProgressData _saveData)
	{
		HashSet<int> hashSet = new HashSet<int>();
		for (int i = 0; i < this.m_levelChainEnds.Length; i++)
		{
			int levelID = this.m_levelChainEnds[i];
			if (this.CanUnlockNewGamePlusForChainEnd(_saveData, levelID))
			{
				this.UnlockNewGamePlusChain(_saveData, levelID, ref hashSet);
				hashSet.Clear();
			}
		}
		bool flag = true;
		for (int j = 0; j < this.m_levelChainEnds.Length; j++)
		{
			int id = this.m_levelChainEnds[j];
			GameProgress.GameProgressData.LevelProgress levelProgress = _saveData.GetLevelProgress(id);
			if (!levelProgress.NGPEnabled)
			{
				flag = false;
				break;
			}
		}
		_saveData.NewGamePlusEnabled = (_saveData.NewGamePlusEnabled || flag);
	}

	// Token: 0x060021FC RID: 8700 RVA: 0x000A4008 File Offset: 0x000A2408
	private void UnlockNewGamePlusChain(GameProgress.GameProgressData _saveData, int _levelID, ref HashSet<int> _visited)
	{
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = this.m_sceneDirectory.Scenes.TryAtIndex(_levelID);
		GameProgress.GameProgressData.LevelProgress levelProgress = _saveData.GetLevelProgress(_levelID);
		levelProgress.NGPEnabled = true;
		_visited.Add(_levelID);
		for (int i = 0; i < this.m_sceneDirectory.Scenes.Length; i++)
		{
			SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry2 = this.m_sceneDirectory.Scenes[i];
			if (sceneDirectoryEntry2.PreviousEntriesToUnlock.Contains(_levelID) && sceneDirectoryEntry2.IsHidden && !_visited.Contains(i))
			{
				_saveData.GetLevelProgress(i).NGPEnabled = true;
				_visited.Add(i);
			}
		}
		foreach (int num in sceneDirectoryEntry.PreviousEntriesToUnlock)
		{
			if (num != -1 && !_visited.Contains(num))
			{
				this.UnlockNewGamePlusChain(_saveData, num, ref _visited);
			}
		}
	}

	// Token: 0x060021FD RID: 8701 RVA: 0x000A40F2 File Offset: 0x000A24F2
	public bool HasShownNGPlusDialog(GameProgress.GameProgressData _saveData)
	{
		return _saveData.NewGamePlusDialogShown;
	}

	// Token: 0x060021FE RID: 8702 RVA: 0x000A40FA File Offset: 0x000A24FA
	public void SetNGPlusDialogShown(GameProgress.GameProgressData _saveData)
	{
		_saveData.NewGamePlusDialogShown = true;
	}

	// Token: 0x060021FF RID: 8703 RVA: 0x000A4103 File Offset: 0x000A2503
	public GameProgress.GameProgressData GetRemoteProgress()
	{
		return this.m_remoteSaveData;
	}

	// Token: 0x04001A1A RID: 6682
	[SerializeField]
	private SceneDirectoryData m_sceneDirectory;

	// Token: 0x04001A1B RID: 6683
	[SerializeField]
	private AvatarDirectoryData m_avatarDirectory;

	// Token: 0x04001A1C RID: 6684
	[SerializeField]
	[FormerlySerializedAs("m_hasTutorialLevel")]
	private bool m_loadFirstScene;

	// Token: 0x04001A1D RID: 6685
	[SerializeField]
	[LevelIndex]
	[FormerlySerializedAs("m_tutorialLevel")]
	private int m_firstLevel = -1;

	// Token: 0x04001A1E RID: 6686
	[SerializeField]
	private GameProgress.UnlockData[] m_unlockData = new GameProgress.UnlockData[0];

	// Token: 0x04001A1F RID: 6687
	private GameSession m_session;

	// Token: 0x04001A20 RID: 6688
	private GameProgress.GameProgressData m_localSaveData = new GameProgress.GameProgressData();

	// Token: 0x04001A21 RID: 6689
	private GameProgress.GameProgressData m_remoteSaveData = new GameProgress.GameProgressData();

	// Token: 0x04001A22 RID: 6690
	private bool m_useSlaveSlot;

	// Token: 0x04001A23 RID: 6691
	private int[] m_levelChainEnds;

	// Token: 0x020006FA RID: 1786
	[Serializable]
	public class UnlockData
	{
		// Token: 0x06002201 RID: 8705 RVA: 0x000A4134 File Offset: 0x000A2534
		public bool IsUnlocked(GameProgress _progress)
		{
			MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
			GameProgress.UnlockData.UnlockType type = this.Type;
			if (type != GameProgress.UnlockData.UnlockType.Avatar)
			{
				return type == GameProgress.UnlockData.UnlockType.CompetitiveLevel;
			}
			return metaGameProgress.IsAvatarUnlocked(this.AvatarData.GetStorageId(_progress.m_avatarDirectory));
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x000A417C File Offset: 0x000A257C
		public void Unlock(GameProgress _progress)
		{
			if (!this.IsUnlocked(_progress))
			{
				MetaGameProgress metaGameProgress = GameUtils.GetMetaGameProgress();
				GameProgress.UnlockData.UnlockType type = this.Type;
				if (type == GameProgress.UnlockData.UnlockType.Avatar)
				{
					metaGameProgress.UnlockAvatar(this.AvatarData.GetStorageId(_progress.m_avatarDirectory));
					return;
				}
				if (type == GameProgress.UnlockData.UnlockType.CompetitiveLevel)
				{
					return;
				}
			}
		}

		// Token: 0x04001A24 RID: 6692
		public GameProgress.UnlockData.UnlockType Type;

		// Token: 0x04001A25 RID: 6693
		[HideInInspectorTest("Type", GameProgress.UnlockData.UnlockType.Avatar)]
		public GameProgress.UnlockData.AvatarDataType AvatarData = new GameProgress.UnlockData.AvatarDataType();

		// Token: 0x04001A26 RID: 6694
		[HideInInspectorTest("Type", GameProgress.UnlockData.UnlockType.CompetitiveLevel)]
		public GameProgress.UnlockData.SceneDataType SceneData = new GameProgress.UnlockData.SceneDataType();

		// Token: 0x04001A27 RID: 6695
		public GameProgress.UnlockData.Condition Requirement = new GameProgress.UnlockData.Condition();

		// Token: 0x020006FB RID: 1787
		[Serializable]
		public class Condition
		{
			// Token: 0x04001A28 RID: 6696
			public GameProgress.UnlockData.Condition.ConditionType Type;

			// Token: 0x04001A29 RID: 6697
			[LevelIndex]
			public int RequiredLevel = -1;

			// Token: 0x04001A2A RID: 6698
			public int RequiredStars;

			// Token: 0x04001A2B RID: 6699
			public bool CheckPreviousLevels;

			// Token: 0x020006FC RID: 1788
			public enum ConditionType
			{
				// Token: 0x04001A2D RID: 6701
				LevelComplete,
				// Token: 0x04001A2E RID: 6702
				LevelStars,
				// Token: 0x04001A2F RID: 6703
				TotalStars
			}
		}

		// Token: 0x020006FD RID: 1789
		public enum UnlockType
		{
			// Token: 0x04001A31 RID: 6705
			Avatar,
			// Token: 0x04001A32 RID: 6706
			CompetitiveLevel
		}

		// Token: 0x020006FE RID: 1790
		[Serializable]
		public class AvatarDataType
		{
			// Token: 0x06002205 RID: 8709 RVA: 0x000A41E4 File Offset: 0x000A25E4
			public static int GetStorageId(AvatarDirectoryData _directory, int _avatarID)
			{
				return _avatarID + _directory.DirectoryID * 100;
			}

			// Token: 0x06002206 RID: 8710 RVA: 0x000A41F1 File Offset: 0x000A25F1
			public int GetStorageId(AvatarDirectoryData _directory)
			{
				return GameProgress.UnlockData.AvatarDataType.GetStorageId(_directory, this.AvatarID);
			}

			// Token: 0x06002207 RID: 8711 RVA: 0x000A41FF File Offset: 0x000A25FF
			public ChefAvatarData GetAvatarData(AvatarDirectoryData _directory)
			{
				return _directory.Avatars[this.AvatarID];
			}

			// Token: 0x04001A33 RID: 6707
			[ArrayIndex("m_avatarDirectory", "Avatars", SerializationUtils.RootType.Top)]
			[SerializeField]
			private int AvatarID;

			// Token: 0x04001A34 RID: 6708
			private const int StorageSegmentation = 100;
		}

		// Token: 0x020006FF RID: 1791
		[Serializable]
		public class SceneDataType
		{
			// Token: 0x04001A35 RID: 6709
			[AssignResource("CompetitiveGameSceneDirectory", Editorbility.NonEditable)]
			public SceneDirectoryData m_competitiveSceneDirectory;

			// Token: 0x04001A36 RID: 6710
			[ArrayIndex("m_competitiveSceneDirectory", "Scenes")]
			public int SceneID;
		}
	}

	// Token: 0x02000700 RID: 1792
	[Serializable]
	public class GameProgressData : IByteSerialization
	{
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x0600220A RID: 8714 RVA: 0x000A4249 File Offset: 0x000A2649
		public uint SaveVersion
		{
			get
			{
				return 1U;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x0600220B RID: 8715 RVA: 0x000A424C File Offset: 0x000A264C
		public int ByteSaveSize
		{
			get
			{
				byte[] array = this.ByteSave();
				return array.Length;
			}
		}

		// Token: 0x0600220C RID: 8716 RVA: 0x000A4264 File Offset: 0x000A2664
		public GameProgress.GameProgressData.LevelProgress GetLevelProgress(int _id)
		{
			int index = this.Levels.FindIndex_Predicate((GameProgress.GameProgressData.LevelProgress x) => x.LevelId == _id);
			GameProgress.GameProgressData.LevelProgress levelProgress = this.Levels.TryAtIndex(index);
			if (levelProgress != null)
			{
				return levelProgress;
			}
			return new GameProgress.GameProgressData.LevelProgress();
		}

		// Token: 0x0600220D RID: 8717 RVA: 0x000A42B0 File Offset: 0x000A26B0
		public int GetStarTotal()
		{
			int num = 0;
			for (int i = 0; i < this.Levels.Length; i++)
			{
				GameProgress.GameProgressData.LevelProgress levelProgress = this.Levels[i];
				int b = (!levelProgress.NGPEnabled || !this.NewGamePlusDialogShown) ? 3 : 4;
				if (levelProgress.Completed)
				{
					num += Mathf.Min(levelProgress.ScoreStars, b);
				}
			}
			return num;
		}

		// Token: 0x0600220E RID: 8718 RVA: 0x000A431C File Offset: 0x000A271C
		public int FarthestProgressedLevel(bool _requireComplete = true, bool _excludeHidden = true)
		{
			SceneDirectoryData sceneDirectory = GameUtils.GetGameSession().Progress.GetSceneDirectory();
			int num = -1;
			for (int i = 0; i < this.Levels.Length; i++)
			{
				GameProgress.GameProgressData.LevelProgress levelProgress = this.Levels[i];
				if (levelProgress.LevelId < sceneDirectory.Scenes.Length)
				{
					SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes[levelProgress.LevelId];
					if ((!_requireComplete || levelProgress.Completed) && num < levelProgress.LevelId && this.IsLevelUnlocked(levelProgress.LevelId, true) && sceneDirectoryEntry.HasScoreBoundaries && (!_excludeHidden || !sceneDirectoryEntry.IsHidden))
					{
						num = levelProgress.LevelId;
					}
				}
			}
			return num;
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x000A43DC File Offset: 0x000A27DC
		private bool IsLevelChainComplete(int _levelIndex, bool _rootCompleteCheck, bool? _hiddenParent, ref HashSet<int> _visited)
		{
			SceneDirectoryData sceneDirectory = GameUtils.GetGameSession().Progress.GetSceneDirectory();
			SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = sceneDirectory.Scenes.TryAtIndex(_levelIndex);
			GameProgress.GameProgressData.LevelProgress levelProgress = this.GetLevelProgress(_levelIndex);
			_visited.Add(_levelIndex);
			if (_hiddenParent != null)
			{
				if (!levelProgress.Completed)
				{
					return false;
				}
				if (_hiddenParent.Value && !levelProgress.ObjectivesCompleted)
				{
					return false;
				}
			}
			else if (_rootCompleteCheck && levelProgress.Completed)
			{
				return true;
			}
			int[] previousEntriesToUnlock = sceneDirectoryEntry.PreviousEntriesToUnlock;
			bool flag = false;
			bool flag2 = true;
			foreach (int num in previousEntriesToUnlock)
			{
				if (num != -1)
				{
					if (!_visited.Contains(num))
					{
						flag |= this.IsLevelChainComplete(num, _rootCompleteCheck, new bool?(sceneDirectoryEntry.IsHidden), ref _visited);
					}
					flag2 = false;
				}
			}
			return flag || flag2;
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x000A44D0 File Offset: 0x000A28D0
		public bool IsLevelUnlocked(int _levelIndex, bool _rootCompleteCheck = true)
		{
			if (this.m_lvlUnlockedVisited == null)
			{
				this.m_lvlUnlockedVisited = new HashSet<int>();
			}
			bool result = this.IsLevelChainComplete(_levelIndex, _rootCompleteCheck, null, ref this.m_lvlUnlockedVisited);
			this.m_lvlUnlockedVisited.Clear();
			return result;
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x000A4518 File Offset: 0x000A2918
		public bool IsLevelComplete(int _levelIndex)
		{
			int num = this.Levels.FindIndex_Predicate((GameProgress.GameProgressData.LevelProgress x) => x.LevelId == _levelIndex);
			return num != -1 && this.Levels[num].Completed;
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x000A4568 File Offset: 0x000A2968
		public bool IsNGPEnabledForLevel(int _levelIndex)
		{
			int num = this.Levels.FindIndex_Predicate((GameProgress.GameProgressData.LevelProgress x) => x.LevelId == _levelIndex);
			return num != -1 && this.Levels[num].NGPEnabled;
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x000A45B8 File Offset: 0x000A29B8
		public bool IsNGPEnabledForAnyLevel()
		{
			for (int i = 0; i < this.Levels.Length; i++)
			{
				if (this.Levels[i].NGPEnabled)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x000A45F4 File Offset: 0x000A29F4
		public bool HasStarsForLevel(int _levelIndex, int _stars, SceneDirectoryData _sceneDirectory)
		{
			SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = _sceneDirectory.Scenes.TryAtIndex(_levelIndex);
			if (sceneDirectoryEntry.HasScoreBoundaries)
			{
				int num = this.Levels.FindIndex_Predicate((GameProgress.GameProgressData.LevelProgress x) => x.LevelId == _levelIndex);
				if (num == -1 || this.Levels[num].ScoreStars < _stars)
				{
					return false;
				}
			}
			else if (!this.IsLevelComplete(_levelIndex))
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x000A4678 File Offset: 0x000A2A78
		public bool IsTrueForLevelsRecursive(int _levelIndex, SceneDirectoryData _sceneDirectory, Generic<bool, int> _condition)
		{
			if (!_condition(_levelIndex))
			{
				return false;
			}
			SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = _sceneDirectory.Scenes.TryAtIndex(_levelIndex);
			foreach (int num in sceneDirectoryEntry.PreviousEntriesToUnlock)
			{
				if (num != -1 && !this.IsTrueForLevelsRecursive(num, _sceneDirectory, _condition))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x000A46D8 File Offset: 0x000A2AD8
		public GameProgress.GameProgressData.SwitchState GetSwitchState(int _id)
		{
			int index = this.Switches.FindIndex_Predicate((GameProgress.GameProgressData.SwitchState x) => x.SwitchId == _id);
			GameProgress.GameProgressData.SwitchState switchState = this.Switches.TryAtIndex(index);
			if (switchState != null)
			{
				return switchState;
			}
			return new GameProgress.GameProgressData.SwitchState();
		}

		// Token: 0x06002217 RID: 8727 RVA: 0x000A4724 File Offset: 0x000A2B24
		public GameProgress.GameProgressData.TeleportalState GetTeleportalState(SceneDirectoryData.World _world)
		{
			int index = this.Teleportals.FindIndex_Predicate((GameProgress.GameProgressData.TeleportalState x) => x.World == _world);
			GameProgress.GameProgressData.TeleportalState teleportalState = this.Teleportals.TryAtIndex(index);
			if (teleportalState != null)
			{
				return teleportalState;
			}
			return new GameProgress.GameProgressData.TeleportalState();
		}

		// Token: 0x06002218 RID: 8728 RVA: 0x000A4770 File Offset: 0x000A2B70
		public byte[] ByteSave()
		{
			GlobalSave globalSave = new GlobalSave();
			globalSave.Set("VERSION", this.SaveVersion);
			globalSave.Set("Level_Count", this.Levels.Length);
			for (int i = 0; i < this.Levels.Length; i++)
			{
				globalSave.Set("Level_" + this.Levels[i].LevelId, this.Levels[i].GetSaveableData());
			}
			if (this.Switches.Length > 0)
			{
				int[] array = new int[this.Switches.Length];
				for (int j = 0; j < this.Switches.Length; j++)
				{
					GameProgress.GameProgressData.SwitchState switchState = this.Switches[j];
					if (switchState != null)
					{
						array[j] = switchState.SwitchId;
						globalSave.Set("Switch_" + switchState.SwitchId, switchState.Activated);
					}
				}
				globalSave.Set("Switches_Revealed", array);
			}
			if (this.Teleportals.Length > 0)
			{
				int[] array2 = new int[this.Teleportals.Length];
				for (int k = 0; k < this.Teleportals.Length; k++)
				{
					GameProgress.GameProgressData.TeleportalState teleportalState = this.Teleportals[k];
					if (teleportalState != null)
					{
						array2[k] = (int)teleportalState.World;
						globalSave.Set("Teleportal_" + teleportalState.World, teleportalState.GetSaveableData());
					}
				}
				globalSave.Set("Teleportals_Revealed", array2);
			}
			globalSave.Set("LastLevelEntered", this.LastLevelEntered);
			globalSave.Set("NewGamePlusEnabled", this.NewGamePlusEnabled);
			globalSave.Set("NewGamePlusDialogShown", this.NewGamePlusDialogShown);
			return globalSave.ByteSave();
		}

		// Token: 0x06002219 RID: 8729 RVA: 0x000A4934 File Offset: 0x000A2D34
		public void FillOut(SceneDirectoryData _sceneDirectory)
		{
			int i;
			for (i = 0; i < _sceneDirectory.Scenes.Length; i++)
			{
				if (this.Levels.FindIndex_Predicate((GameProgress.GameProgressData.LevelProgress x) => x.LevelId == i) == -1)
				{
					ArrayUtils.PushBack<GameProgress.GameProgressData.LevelProgress>(ref this.Levels, new GameProgress.GameProgressData.LevelProgress
					{
						LevelId = i,
						Purchased = (_sceneDirectory.Scenes[i].StarCost == 0)
					});
				}
			}
		}

		// Token: 0x0600221A RID: 8730 RVA: 0x000A49CC File Offset: 0x000A2DCC
		public bool ByteLoad(byte[] _data)
		{
			GlobalSave globalSave = new GlobalSave();
			if (!globalSave.ByteLoad(_data))
			{
				return false;
			}
			int num;
			globalSave.Get("VERSION", out num, 1);
			if (num != (int)this.SaveVersion)
			{
				return false;
			}
			int num2;
			globalSave.Get("Level_Count", out num2, 0);
			this.Levels = new GameProgress.GameProgressData.LevelProgress[num2];
			for (int i = 0; i < num2; i++)
			{
				Dictionary<string, string> data;
				globalSave.Get("Level_" + i, out data, new Dictionary<string, string>());
				this.Levels[i] = new GameProgress.GameProgressData.LevelProgress();
				if (!this.Levels[i].SetData(data))
				{
					return false;
				}
			}
			int[] array = null;
			bool flag = globalSave.Get("Switches_Revealed", out array, null);
			if (flag)
			{
				int num3 = array.Length;
				this.Switches = new GameProgress.GameProgressData.SwitchState[num3];
				for (int j = 0; j < num3; j++)
				{
					this.Switches[j] = new GameProgress.GameProgressData.SwitchState();
					bool bActivated;
					globalSave.Get("Switch_" + array[j], out bActivated, false);
					this.Switches[j].SetSwitchState(array[j], bActivated);
				}
			}
			int[] array2 = null;
			bool flag2 = globalSave.Get("Teleportals_Revealed", out array2, null);
			if (flag2)
			{
				int num4 = array2.Length;
				this.Teleportals = new GameProgress.GameProgressData.TeleportalState[num4];
				for (int k = 0; k < num4; k++)
				{
					SceneDirectoryData.World world = (SceneDirectoryData.World)array2[k];
					Dictionary<string, string> data2;
					globalSave.Get("Teleportal_" + world, out data2, new Dictionary<string, string>());
					GameProgress.GameProgressData.TeleportalState teleportalState = new GameProgress.GameProgressData.TeleportalState();
					teleportalState.World = world;
					if (!teleportalState.SetData(data2))
					{
						return false;
					}
					this.Teleportals[k] = teleportalState;
				}
			}
			globalSave.Get("LastLevelEntered", out this.LastLevelEntered, -1);
			globalSave.Get("NewGamePlusEnabled", out this.NewGamePlusEnabled, false);
			globalSave.Get("NewGamePlusDialogShown", out this.NewGamePlusDialogShown, false);
			return true;
		}

		// Token: 0x0600221B RID: 8731 RVA: 0x000A4BCC File Offset: 0x000A2FCC
		public static bool Validate(byte[] _data)
		{
			return new GlobalSave().ByteLoad(_data);
		}

		// Token: 0x04001A37 RID: 6711
		public GameProgress.GameProgressData.LevelProgress[] Levels = new GameProgress.GameProgressData.LevelProgress[0];

		// Token: 0x04001A38 RID: 6712
		public GameProgress.GameProgressData.SwitchState[] Switches = new GameProgress.GameProgressData.SwitchState[0];

		// Token: 0x04001A39 RID: 6713
		public GameProgress.GameProgressData.TeleportalState[] Teleportals = new GameProgress.GameProgressData.TeleportalState[0];

		// Token: 0x04001A3A RID: 6714
		[LevelIndex]
		public int LastLevelEntered = -1;

		// Token: 0x04001A3B RID: 6715
		public bool NewGamePlusEnabled;

		// Token: 0x04001A3C RID: 6716
		public bool NewGamePlusDialogShown;

		// Token: 0x04001A3D RID: 6717
		private HashSet<int> m_lvlUnlockedVisited;

		// Token: 0x04001A3E RID: 6718
		private const string c_versionTag = "VERSION";

		// Token: 0x02000701 RID: 1793
		[Serializable]
		public class LevelProgress
		{
			// Token: 0x0600221D RID: 8733 RVA: 0x000A4BF4 File Offset: 0x000A2FF4
			public Dictionary<string, string> GetSaveableData()
			{
				return new Dictionary<string, string>
				{
					{
						"LevelID",
						this.LevelId.ToString()
					},
					{
						"Completed",
						this.Completed.ToString()
					},
					{
						"Purchased",
						this.Purchased.ToString()
					},
					{
						"Revealed",
						this.Revealed.ToString()
					},
					{
						"HighScore",
						this.HighScore.ToString()
					},
					{
						"SurvivalModeTime",
						this.SurvivalModeTime.ToString()
					},
					{
						"ScoreStars",
						this.ScoreStars.ToString()
					},
					{
						"ObjectivesCompleted",
						this.ObjectivesCompleted.ToString()
					},
					{
						"NGPEnabled",
						this.NGPEnabled.ToString()
					}
				};
			}

			// Token: 0x0600221E RID: 8734 RVA: 0x000A4D08 File Offset: 0x000A3108
			public bool SetData(Dictionary<string, string> _data)
			{
				int.TryParse(_data.SafeGet("LevelID", -1.ToString()), out this.LevelId);
				bool.TryParse(_data.SafeGet("Completed", false.ToString()), out this.Completed);
				bool.TryParse(_data.SafeGet("Purchased", false.ToString()), out this.Purchased);
				bool.TryParse(_data.SafeGet("Revealed", false.ToString()), out this.Revealed);
				int.TryParse(_data.SafeGet("HighScore", 65535.ToString()), out this.HighScore);
				int.TryParse(_data.SafeGet("SurvivalModeTime", 0.ToString()), out this.SurvivalModeTime);
				int.TryParse(_data.SafeGet("ScoreStars", false.ToString()), out this.ScoreStars);
				bool.TryParse(_data.SafeGet("ObjectivesCompleted", false.ToString()), out this.ObjectivesCompleted);
				bool.TryParse(_data.SafeGet("NGPEnabled", false.ToString()), out this.NGPEnabled);
				return true;
			}

			// Token: 0x04001A3F RID: 6719
			public const int MaxStars = 4;

			// Token: 0x04001A40 RID: 6720
			public const int InvalidScore = 65535;

			// Token: 0x04001A41 RID: 6721
			public const int MaxScore = 65534;

			// Token: 0x04001A42 RID: 6722
			public const int InvalidTime = 0;

			// Token: 0x04001A43 RID: 6723
			public const int MaxTime = 5999;

			// Token: 0x04001A44 RID: 6724
			public int LevelId = -1;

			// Token: 0x04001A45 RID: 6725
			public bool Completed;

			// Token: 0x04001A46 RID: 6726
			public bool Purchased;

			// Token: 0x04001A47 RID: 6727
			public bool Revealed;

			// Token: 0x04001A48 RID: 6728
			public int HighScore = int.MinValue;

			// Token: 0x04001A49 RID: 6729
			public int SurvivalModeTime;

			// Token: 0x04001A4A RID: 6730
			public int ScoreStars;

			// Token: 0x04001A4B RID: 6731
			public bool ObjectivesCompleted;

			// Token: 0x04001A4C RID: 6732
			public bool NGPEnabled;
		}

		// Token: 0x02000702 RID: 1794
		[Serializable]
		public class SwitchState
		{
			// Token: 0x06002220 RID: 8736 RVA: 0x000A4E84 File Offset: 0x000A3284
			public void SetSwitchState(int iSwitchID, bool bActivated)
			{
				this.SwitchId = iSwitchID;
				this.Activated = bActivated;
			}

			// Token: 0x04001A4D RID: 6733
			public int SwitchId = -1;

			// Token: 0x04001A4E RID: 6734
			public bool Activated;
		}

		// Token: 0x02000703 RID: 1795
		[Serializable]
		public class TeleportalState
		{
			// Token: 0x06002222 RID: 8738 RVA: 0x000A4EA4 File Offset: 0x000A32A4
			public Dictionary<string, string> GetSaveableData()
			{
				return new Dictionary<string, string>();
			}

			// Token: 0x06002223 RID: 8739 RVA: 0x000A4EAB File Offset: 0x000A32AB
			public bool SetData(Dictionary<string, string> _data)
			{
				return true;
			}

			// Token: 0x04001A4F RID: 6735
			public SceneDirectoryData.World World = SceneDirectoryData.World.COUNT;
		}
	}

	// Token: 0x02000704 RID: 1796
	public class HighScores
	{
		// Token: 0x06002225 RID: 8741 RVA: 0x000A4F6C File Offset: 0x000A336C
		public GameProgress.HighScores Copy()
		{
			GameProgress.HighScores highScores = new GameProgress.HighScores();
			for (int i = 0; i < this.Scores.Count; i++)
			{
				GameProgress.HighScores.Score score = this.Scores[i];
				highScores.Scores.Add(new GameProgress.HighScores.Score
				{
					iLevelID = score.iLevelID,
					iHighScore = score.iHighScore,
					iSurvivalModeTime = score.iSurvivalModeTime
				});
			}
			return highScores;
		}

		// Token: 0x04001A50 RID: 6736
		public List<GameProgress.HighScores.Score> Scores = new List<GameProgress.HighScores.Score>();

		// Token: 0x02000705 RID: 1797
		public class Score
		{
			// Token: 0x04001A51 RID: 6737
			public int iLevelID;

			// Token: 0x04001A52 RID: 6738
			public int iHighScore;

			// Token: 0x04001A53 RID: 6739
			public int iSurvivalModeTime;
		}
	}
}
