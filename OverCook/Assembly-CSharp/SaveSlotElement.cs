using System;
using System.Collections;
using System.Collections.Generic;
using GameModes;
using Team17.Online;
using UnityEngine;

// Token: 0x02000B04 RID: 2820
public class SaveSlotElement : BaseMenuBehaviour
{
	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x060038FC RID: 14588 RVA: 0x0010D851 File Offset: 0x0010BC51
	public int Slot
	{
		get
		{
			return this.m_slotNum;
		}
	}

	// Token: 0x170003E5 RID: 997
	// (get) Token: 0x060038FD RID: 14589 RVA: 0x0010D859 File Offset: 0x0010BC59
	// (set) Token: 0x060038FE RID: 14590 RVA: 0x0010D861 File Offset: 0x0010BC61
	public SaveDialogMode Mode
	{
		get
		{
			return this.m_mode;
		}
		set
		{
			this.m_mode = value;
		}
	}

	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x060038FF RID: 14591 RVA: 0x0010D86A File Offset: 0x0010BC6A
	// (set) Token: 0x06003900 RID: 14592 RVA: 0x0010D872 File Offset: 0x0010BC72
	public int DLC
	{
		get
		{
			return this.m_dlcNum;
		}
		set
		{
			this.m_dlcNum = value;
		}
	}

	// Token: 0x170003E7 RID: 999
	// (get) Token: 0x06003901 RID: 14593 RVA: 0x0010D87B File Offset: 0x0010BC7B
	public bool SaveGameReady
	{
		get
		{
			return this.m_bSaveGameReady;
		}
	}

	// Token: 0x06003902 RID: 14594 RVA: 0x0010D884 File Offset: 0x0010BC84
	public override bool Show(GamepadUser _gamepadUser, BaseMenuBehaviour _parent, GameObject _invoker, bool _hideInvoker = true)
	{
		if (!base.Show(_gamepadUser, _parent, _invoker, _hideInvoker))
		{
			return false;
		}
		this.UpdateUI();
		this.m_bTriggeredLoad = false;
		this.m_bSaveGameReady = false;
		GameSession gameSession = GameUtils.GetGameSession();
		return true;
	}

	// Token: 0x06003903 RID: 14595 RVA: 0x0010D8C0 File Offset: 0x0010BCC0
	public IEnumerator LoadSlotData(int _dlcNum)
	{
		GameSession session = SelectSaveDialog.CreateFreshGameSessionForSlot(_dlcNum, this.m_slotNum);
		ReturnValue<SaveLoadResult> result = new ReturnValue<SaveLoadResult>();
		IEnumerator hasSaveRoutine = session.HasSaveFile(result);
		while (hasSaveRoutine.MoveNext())
		{
			yield return null;
		}
		this.m_slotState = new SaveLoadResult?(result.Value);
		if (this.m_slotState == SaveLoadResult.Exists)
		{
			IEnumerator sessionLoad = session.LoadSession();
			while (sessionLoad.MoveNext())
			{
				yield return null;
			}
			this.m_slotInfo.m_saveData = session.Progress.SaveableData;
			if (session.Progress.CanUnlockNewGamePlus(session.Progress.SaveableData))
			{
				session.Progress.UnlockNewGamePlus(session.Progress.SaveableData);
			}
		}
		else
		{
			this.m_slotInfo.m_saveData = null;
		}
		this.m_slotInfo.m_sceneDirectory = session.Progress.GetSceneDirectory();
		this.m_slotInfo.m_firstSceneIndex = session.Progress.FirstSceneIndex;
		if (base.gameObject.activeInHierarchy)
		{
			this.UpdateUI();
		}
		yield break;
	}

	// Token: 0x06003904 RID: 14596 RVA: 0x0010D8E4 File Offset: 0x0010BCE4
	public void UpdateUI()
	{
		if (this.m_slotState == SaveLoadResult.Corrupted)
		{
			this.UpdateUICorrupted();
			return;
		}
		if (this.m_slotState == SaveLoadResult.NotExist || this.m_slotState == SaveLoadResult.NoSpace)
		{
			this.UpdateUIEmpty();
			return;
		}
		if (this.m_slotInfo.m_saveData != null)
		{
			bool flag = this.m_slotInfo.m_saveData.IsNGPEnabledForAnyLevel();
			if (this.m_newGamePlusMarker != null)
			{
				this.m_newGamePlusMarker.SetActive(flag);
			}
			else if (this.m_newGamePlusMarkerPrefab != null && flag)
			{
				this.m_newGamePlusMarker = UnityEngine.Object.Instantiate<GameObject>(this.m_newGamePlusMarkerPrefab, this.m_validSaveSlot.transform);
				this.m_newGamePlusMarker.SetActive(true);
			}
			this.m_validSaveSlot.SetActive(true);
			this.m_corruptSaveSlot.SetActive(false);
			this.m_noSaveSlot.SetActive(false);
			int num = this.m_slotInfo.m_saveData.FarthestProgressedLevel(true, true);
			SceneDirectoryData sceneDirectory = this.m_slotInfo.m_sceneDirectory;
			string localisedTextCatchAll = string.Empty;
			if (num != -1 && num < sceneDirectory.Scenes.Length)
			{
				localisedTextCatchAll = sceneDirectory.Scenes[num].Label;
			}
			else
			{
				localisedTextCatchAll = sceneDirectory.Scenes[this.m_slotInfo.m_firstSceneIndex].Label;
			}
			this.m_levelName.SetLocalisedTextCatchAll(localisedTextCatchAll);
			this.m_starCount.m_bNeedsLocalization = false;
			string text = this.m_slotInfo.m_saveData.GetStarTotal().ToString();
			text = text.PadLeft(this.m_maxStarCharacters, '0');
			this.m_starCount.text = text;
		}
		else
		{
			this.UpdateUIEmpty();
		}
	}

	// Token: 0x06003905 RID: 14597 RVA: 0x0010DAD8 File Offset: 0x0010BED8
	protected void UpdateUICorrupted()
	{
		this.m_validSaveSlot.SetActive(false);
		this.m_corruptSaveSlot.SetActive(true);
		this.m_noSaveSlot.SetActive(false);
	}

	// Token: 0x06003906 RID: 14598 RVA: 0x0010DAFE File Offset: 0x0010BEFE
	protected void UpdateUIEmpty()
	{
		this.m_validSaveSlot.SetActive(false);
		this.m_corruptSaveSlot.SetActive(false);
		this.m_noSaveSlot.SetActive(true);
	}

	// Token: 0x06003907 RID: 14599 RVA: 0x0010DB24 File Offset: 0x0010BF24
	public void OnSlotClicked()
	{
		this.m_slotClickedRoutine = this.TriggerSlotRoutine();
	}

	// Token: 0x06003908 RID: 14600 RVA: 0x0010DB34 File Offset: 0x0010BF34
	public IEnumerator TriggerSlotRoutine()
	{
		if (ConnectionModeSwitcher.GetStatus().GetProgress() == eConnectionModeSwitchProgress.InProgress)
		{
			yield break;
		}
		this.m_processingSlotTrigger = true;
		this.SuppressEventSystem();
		this.UpdateConnectionState();
		while (ConnectionModeSwitcher.GetStatus().GetProgress() != eConnectionModeSwitchProgress.Complete)
		{
			yield return null;
		}
		SaveLoadResult? slotState = this.m_slotState;
		if (slotState == null)
		{
			IEnumerator load = this.LoadSlotData(this.m_dlcNum);
			while (load.MoveNext())
			{
				yield return null;
			}
		}
		if (this.m_mode == SaveDialogMode.NewGame)
		{
			if (this.m_slotState == SaveLoadResult.Exists || this.m_slotState == SaveLoadResult.Corrupted)
			{
				this.ReleaseActiveSuppression(true);
				bool? overwriteSave = null;
				this.ShowOverwriteDialog(delegate
				{
					overwriteSave = new bool?(true);
					this.m_overwriteDialog = null;
				}, delegate
				{
					overwriteSave = new bool?(false);
					this.m_overwriteDialog = null;
				});
				while (overwriteSave == null)
				{
					yield return null;
				}
				if (overwriteSave.Value)
				{
					this.SuppressEventSystem();
					bool deleted = false;
					SaveManager saveManager = GameUtils.RequireManager<SaveManager>();
					saveManager.DeleteSave(SaveMode.Main, this.m_slotNum, this.m_dlcNum, delegate
					{
						deleted = true;
					});
					while (!deleted)
					{
						yield return null;
					}
					this.ReleaseActiveSuppression(false);
				}
				else if (!overwriteSave.Value)
				{
					this.m_processingSlotTrigger = false;
					yield break;
				}
			}
			this.SuppressEventSystem();
			IEnumerator save = this.SaveNewGame(new CallbackVoid(this.OnSaveGameReady));
			while (save.MoveNext())
			{
				yield return null;
			}
		}
		else
		{
			this.SuppressEventSystem();
			if (this.m_slotInfo.m_saveData != null || this.m_slotState == SaveLoadResult.Corrupted)
			{
				GameUtils.GetMetaGameProgress().SetLastSaveSlot(this.m_dlcNum, this.m_slotNum);
				IEnumerator loadRoutine = this.LoadSaveRoutine();
				while (loadRoutine.MoveNext())
				{
					yield return null;
				}
			}
			else
			{
				IEnumerator save2 = this.SaveNewGame(new CallbackVoid(this.OnSaveGameReady));
				while (save2.MoveNext())
				{
					yield return null;
				}
			}
		}
		this.ReleaseActiveSuppression(false);
		this.m_processingSlotTrigger = false;
		yield break;
	}

	// Token: 0x06003909 RID: 14601 RVA: 0x0010DB50 File Offset: 0x0010BF50
	private void SuppressEventSystem()
	{
		if (this.m_suppressor != null)
		{
			return;
		}
		GamepadUser user = GameUtils.RequireManagerInterface<IPlayerManager>().GetUser(EngagementSlot.One);
		if (user != null)
		{
			T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user);
			if (eventSystemForGamepadUser != null)
			{
				this.m_suppressor = eventSystemForGamepadUser.Disable(this);
			}
		}
	}

	// Token: 0x0600390A RID: 14602 RVA: 0x0010DBA8 File Offset: 0x0010BFA8
	private void ReleaseActiveSuppression(bool _force = false)
	{
		if (this.m_suppressor != null)
		{
			if (_force)
			{
				GamepadUser user = GameUtils.RequireManagerInterface<IPlayerManager>().GetUser(EngagementSlot.One);
				if (user != null)
				{
					T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user);
					if (eventSystemForGamepadUser != null)
					{
						eventSystemForGamepadUser.ReleaseSuppressor(this.m_suppressor);
						this.m_suppressor = null;
					}
				}
				if (this.m_suppressor != null)
				{
					this.m_suppressor.Release();
					this.m_suppressor = null;
				}
			}
			else
			{
				this.m_suppressor.Release();
				this.m_suppressor = null;
			}
		}
	}

	// Token: 0x0600390B RID: 14603 RVA: 0x0010DC40 File Offset: 0x0010C040
	private void UpdateConnectionState()
	{
		if (ConnectionModeSwitcher.GetRequestedConnectionState() == NetConnectionState.Server)
		{
			if (!UserSystemUtils.AnyRemoteUsers())
			{
				OnlineMultiplayerConnectionMode value;
				if (ConnectionStatus.CurrentConnectionMode() == OnlineMultiplayerConnectionMode.eInternet)
				{
					value = OnlineMultiplayerConnectionMode.eInternet;
				}
				else
				{
					value = OnlineMultiplayerConnectionMode.eNone;
				}
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, new OfflineOptions
				{
					hostUser = GameUtils.RequireManagerInterface<IPlayerManager>().GetUser(EngagementSlot.One),
					connectionMode = new OnlineMultiplayerConnectionMode?(value)
				}, null);
			}
			else
			{
				ServerOptions serverOptions = (ServerOptions)ConnectionModeSwitcher.GetAgentData();
				serverOptions.visibility = OnlineMultiplayerSessionVisibility.eClosed;
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Server, serverOptions, null);
			}
		}
	}

	// Token: 0x0600390C RID: 14604 RVA: 0x0010DCD4 File Offset: 0x0010C0D4
	private IEnumerator LoadSaveRoutine()
	{
		GameSession session = SelectSaveDialog.CreateFreshGameSessionForSlot(this.m_dlcNum, this.m_slotNum);
		IEnumerator<SaveLoadResult?> loadSession = session.LoadSession();
		while (loadSession.MoveNext())
		{
			if (T17DialogBoxManager.HasAnyOpenDialogs())
			{
				this.ReleaseActiveSuppression(false);
			}
			yield return null;
		}
		this.SuppressEventSystem();
		SaveLoadResult? saveLoadResult = loadSession.Current;
		if (saveLoadResult.Value == SaveLoadResult.Exists)
		{
			if (session.Progress.CanUnlockNewGamePlus(session.Progress.SaveableData))
			{
				session.Progress.UnlockNewGamePlus(session.Progress.SaveableData);
			}
			if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
			{
				session.Progress.UseSlaveSlot = true;
			}
			this.OnSaveGameReady();
		}
		else
		{
			SaveLoadResult? saveLoadResult2 = loadSession.Current;
			if (saveLoadResult2.Value == SaveLoadResult.NotExist)
			{
				IEnumerator save = this.SaveNewGame(new CallbackVoid(this.OnSaveGameReady));
				while (save.MoveNext())
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x0600390D RID: 14605 RVA: 0x0010DCF0 File Offset: 0x0010C0F0
	public void ServerLoadCampaign(GameSession session)
	{
		session.FillShownMetaDialogStatus();
		if (!session.Progress.LoadFirstScene || session.Progress.SaveData.IsLevelComplete(this.m_slotInfo.m_firstSceneIndex) || GameUtils.GetDebugConfig().m_skipTutorial)
		{
			ServerMessenger.SetupCoopSession(this.m_dlcNum, session.Progress.SaveData, session.m_shownMetaDialogs, session.GameModeSessionConfig);
			ServerGameSetup.Mode = GameMode.Campaign;
			ServerMessenger.LoadLevel(session.TypeSettings.WorldMapScene, GameState.CampaignMap, true, GameState.RunMapUnfoldRoutine);
		}
		else
		{
			SessionConfig sessionConfig = new SessionConfig();
			sessionConfig.Copy(session.GameModeSessionConfig);
			sessionConfig.m_kind = Kind.Campaign;
			ServerMessenger.SetupCoopSession(this.m_dlcNum, session.Progress.SaveData, session.m_shownMetaDialogs, sessionConfig);
			this.LoadFirstLevel();
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.HideWaitingForPlayers();
		}
		this.m_bTriggeredLoad = true;
	}

	// Token: 0x0600390E RID: 14606 RVA: 0x0010DDE4 File Offset: 0x0010C1E4
	private void LoadFirstLevel()
	{
		int firstSceneIndex = this.m_slotInfo.m_firstSceneIndex;
		SceneDirectoryData.SceneDirectoryEntry sceneDirectoryEntry = this.m_slotInfo.m_sceneDirectory.Scenes[firstSceneIndex];
		SceneDirectoryData.PerPlayerCountDirectoryEntry sceneVarient = sceneDirectoryEntry.GetSceneVarient(ClientUserSystem.m_Users.Count);
		if (sceneVarient != null)
		{
			string sceneName = sceneVarient.SceneName;
			if (sceneName != string.Empty)
			{
				GameSession.GameLevelSettings gameLevelSettings = new GameSession.GameLevelSettings();
				gameLevelSettings.SceneDirectoryVarientEntry = sceneVarient;
				ServerGameSetup.Mode = GameMode.Campaign;
				ServerMessenger.LoadLevel((uint)firstSceneIndex, (uint)ClientUserSystem.m_Users.Count, GameState.LoadKitchen, GameState.RunKitchen);
				return;
			}
		}
		Vector2 vector = new Vector2(0.5f * (float)Camera.main.pixelWidth, 0.5f * (float)Camera.main.pixelHeight);
	}

	// Token: 0x0600390F RID: 14607 RVA: 0x0010DE90 File Offset: 0x0010C290
	private IEnumerator SaveNewGame(CallbackVoid _onSaveSuccess)
	{
		GameSession session = SelectSaveDialog.CreateFreshGameSessionForSlot(this.m_dlcNum, this.m_slotNum);
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			session.Progress.UseSlaveSlot = true;
		}
		SaveLoadResult? saveResult = null;
		SaveSystemCallback callback = delegate(SaveSystemStatus _status)
		{
			if (_status.Status == SaveSystemStatus.SaveStatus.Complete)
			{
				saveResult = new SaveLoadResult?(_status.Result);
			}
		};
		GameUtils.RequireManager<SaveManager>().RegisterOnIdle(delegate
		{
			session.SaveSession(callback);
		});
		while (saveResult == null)
		{
			if (T17DialogBoxManager.HasAnyOpenDialogs())
			{
				this.ReleaseActiveSuppression(false);
			}
			else
			{
				this.SuppressEventSystem();
			}
			yield return null;
		}
		this.SuppressEventSystem();
		if (saveResult.GetValueOrDefault() != SaveLoadResult.Cancel || saveResult == null)
		{
			_onSaveSuccess();
		}
		yield break;
	}

	// Token: 0x06003910 RID: 14608 RVA: 0x0010DEB4 File Offset: 0x0010C2B4
	private void OnSaveGameReady()
	{
		if (!this.m_impendingForceClose)
		{
			GameUtils.GetMetaGameProgress().SetLastSaveSlot(this.m_dlcNum, this.m_slotNum);
			GameProgress.HighScores highScores = new GameProgress.HighScores();
			GameSession gameSession = GameUtils.GetGameSession();
			gameSession.Progress.GetLocalScores(ref highScores);
			gameSession.HighScoreRepository.SetScoresForMachine(ClientUserSystem.s_LocalMachineId, gameSession.DLC, highScores);
			if (ConnectionStatus.IsInSession())
			{
				if (ConnectionStatus.IsHost())
				{
					GameStateMessage.ClientSavePayload clientSavePayload = new GameStateMessage.ClientSavePayload();
					clientSavePayload.Initialise(this.m_dlcNum);
					UserSystemUtils.ChangeGameState(GameState.SelectCampaignMapSave, clientSavePayload);
				}
				else
				{
					ClientMessenger.HighScores(highScores, gameSession.DLC);
				}
				ClientMessenger.GameState(GameState.LoadedCampaignMapSave);
				if (T17FrontendFlow.Instance != null)
				{
					T17FrontendFlow.Instance.ShowWaitingForPlayers();
				}
			}
			else
			{
				this.ServerLoadCampaign(gameSession);
			}
			this.m_bSaveGameReady = true;
		}
	}

	// Token: 0x06003911 RID: 14609 RVA: 0x0010DF8A File Offset: 0x0010C38A
	protected override void Update()
	{
		base.Update();
		if (this.m_slotClickedRoutine != null && !this.m_slotClickedRoutine.MoveNext())
		{
			this.m_slotClickedRoutine = null;
		}
	}

	// Token: 0x06003912 RID: 14610 RVA: 0x0010DFB4 File Offset: 0x0010C3B4
	public void ShowOverwriteDialog(T17DialogBox.DialogEvent _confirmCallback, T17DialogBox.DialogEvent _cancelCallback)
	{
		this.m_overwriteDialog = T17DialogBoxManager.GetDialog(false);
		if (this.m_overwriteDialog != null)
		{
			this.m_overwriteDialog.Initialize("Text.OverwriteSlot.Title", "Text.OverwriteSlot.Body", "Text.Button.Confirm", null, "Text.Button.Cancel", T17DialogBox.Symbols.Warning, true, true, false);
			T17DialogBox overwriteDialog = this.m_overwriteDialog;
			overwriteDialog.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(overwriteDialog.OnConfirm, _confirmCallback);
			T17DialogBox overwriteDialog2 = this.m_overwriteDialog;
			overwriteDialog2.OnCancel = (T17DialogBox.DialogEvent)Delegate.Combine(overwriteDialog2.OnCancel, _cancelCallback);
			this.m_overwriteDialog.Show();
		}
		else
		{
			_cancelCallback();
		}
	}

	// Token: 0x06003913 RID: 14611 RVA: 0x0010E050 File Offset: 0x0010C450
	public void InformImpendingForceClose()
	{
		this.m_impendingForceClose = true;
	}

	// Token: 0x06003914 RID: 14612 RVA: 0x0010E05C File Offset: 0x0010C45C
	public bool CanHide()
	{
		if (this.m_processingSlotTrigger)
		{
			bool flag = GameUtils.RequireManager<SaveManager>().HasActiveInputDialog();
			if (!(flag | (this.m_overwriteDialog != null && this.m_overwriteDialog.IsActive)))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003915 RID: 14613 RVA: 0x0010E0AC File Offset: 0x0010C4AC
	public bool CleanUp()
	{
		if (this.m_slotClickedRoutine != null)
		{
			if (this.m_overwriteDialog != null)
			{
				this.m_overwriteDialog.Cancel();
			}
			GameUtils.RequireManager<SaveManager>().CancelActiveInputDialog();
			if (this.m_slotClickedRoutine.MoveNext())
			{
				return false;
			}
		}
		this.m_slotClickedRoutine = null;
		this.m_slotState = null;
		this.m_slotInfo.m_saveData = null;
		return true;
	}

	// Token: 0x06003916 RID: 14614 RVA: 0x0010E11F File Offset: 0x0010C51F
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (!this.CanHide())
		{
			return false;
		}
		if (!this.CleanUp())
		{
			return false;
		}
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		this.ReleaseActiveSuppression(false);
		this.m_impendingForceClose = false;
		return true;
	}

	// Token: 0x04002DBC RID: 11708
	[SerializeField]
	private T17Text m_levelName;

	// Token: 0x04002DBD RID: 11709
	[SerializeField]
	private T17Text m_starCount;

	// Token: 0x04002DBE RID: 11710
	[SerializeField]
	private int m_slotNum;

	// Token: 0x04002DBF RID: 11711
	[SerializeField]
	private GameObject m_validSaveSlot;

	// Token: 0x04002DC0 RID: 11712
	[SerializeField]
	[AssignResource("NGPlusMarker", Editorbility.Editable)]
	private GameObject m_newGamePlusMarkerPrefab;

	// Token: 0x04002DC1 RID: 11713
	private GameObject m_newGamePlusMarker;

	// Token: 0x04002DC2 RID: 11714
	[SerializeField]
	private GameObject m_corruptSaveSlot;

	// Token: 0x04002DC3 RID: 11715
	[SerializeField]
	private GameObject m_noSaveSlot;

	// Token: 0x04002DC4 RID: 11716
	[SerializeField]
	[Range(1f, 20f)]
	private int m_maxStarCharacters = 3;

	// Token: 0x04002DC5 RID: 11717
	protected SaveLoadResult? m_slotState;

	// Token: 0x04002DC6 RID: 11718
	private SaveSlotElement.SlotInfo m_slotInfo;

	// Token: 0x04002DC7 RID: 11719
	protected SaveDialogMode m_mode;

	// Token: 0x04002DC8 RID: 11720
	protected int m_dlcNum;

	// Token: 0x04002DC9 RID: 11721
	private Suppressor m_suppressor;

	// Token: 0x04002DCA RID: 11722
	private IEnumerator m_slotClickedRoutine;

	// Token: 0x04002DCB RID: 11723
	private bool m_processingSlotTrigger;

	// Token: 0x04002DCC RID: 11724
	private bool m_bTriggeredLoad;

	// Token: 0x04002DCD RID: 11725
	private bool m_bSaveGameReady;

	// Token: 0x04002DCE RID: 11726
	private T17DialogBox m_overwriteDialog;

	// Token: 0x04002DCF RID: 11727
	private bool m_impendingForceClose;

	// Token: 0x02000B05 RID: 2821
	protected struct SlotInfo
	{
		// Token: 0x04002DD0 RID: 11728
		public GameProgress.GameProgressData m_saveData;

		// Token: 0x04002DD1 RID: 11729
		public SceneDirectoryData m_sceneDirectory;

		// Token: 0x04002DD2 RID: 11730
		[LevelIndex]
		public int m_firstSceneIndex;
	}
}
