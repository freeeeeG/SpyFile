using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B07 RID: 2823
public class SelectSaveDialog : FrontendMenuBehaviour
{
	// Token: 0x170003E8 RID: 1000
	// (get) Token: 0x06003918 RID: 14616 RVA: 0x0010EC7B File Offset: 0x0010D07B
	// (set) Token: 0x06003919 RID: 14617 RVA: 0x0010EC83 File Offset: 0x0010D083
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

	// Token: 0x170003E9 RID: 1001
	// (get) Token: 0x0600391A RID: 14618 RVA: 0x0010EC8C File Offset: 0x0010D08C
	// (set) Token: 0x0600391B RID: 14619 RVA: 0x0010EC94 File Offset: 0x0010D094
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

	// Token: 0x0600391C RID: 14620 RVA: 0x0010EC9D File Offset: 0x0010D09D
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		this.m_gamepadEngagementManager = GameUtils.RequireManager<GamepadEngagementManager>();
	}

	// Token: 0x0600391D RID: 14621 RVA: 0x0010ECB0 File Offset: 0x0010D0B0
	public override bool Show(GamepadUser _currentGamer, BaseMenuBehaviour _parent, GameObject _invoker, bool _hideInvoker = true)
	{
		if (!base.Show(_currentGamer, _parent, _invoker, _hideInvoker))
		{
			return false;
		}
		GameSession gameSession = GameUtils.GetGameSession();
		SelectSaveDialog.s_cachedHighScores = ((!(gameSession != null)) ? null : gameSession.HighScoreRepository);
		if (this.m_gamepadEngagementManager != null)
		{
			this.m_engagementSuppressor = this.m_gamepadEngagementManager.Suppressor.AddSuppressor(this);
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = true;
		}
		if (null != this.m_DontSaveButton)
		{
			this.m_DontSaveButton.SetActive(false);
		}
		this.m_header.SetLocalisedTextCatchAll((this.m_mode != SaveDialogMode.NewGame) ? this.m_loadHeader : this.m_newGameHeader);
		IPlayerManager playerManager = GameUtils.RequestManagerInterface<IPlayerManager>();
		GamepadUser user = playerManager.GetUser(EngagementSlot.One);
		this.m_eventSystem = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user);
		if (this.m_eventSystem == null)
		{
			playerManager.EngagementChangeCallback += this.OnEngagementChanged;
		}
		this.m_slotUpdate = this.ShowSlotElements(this.m_mode, this.m_dlcNum, _currentGamer, this, _invoker, _hideInvoker);
		InviteMonitor.InviteJoinComplete = (GenericVoid)Delegate.Combine(InviteMonitor.InviteJoinComplete, new GenericVoid(this.OnInviteJoinComplete));
		if (this.m_legend)
		{
			this.m_legend.SetActive(false);
		}
		if (this.m_info != null)
		{
			DLCFrontendData dlcfrontendData = this.GetDLCFrontendData(this.m_dlcNum);
			string replaceWith = Localization.Get((!(dlcfrontendData == null)) ? dlcfrontendData.m_NameLocalizationKey : "Text.Menu.Story", new LocToken[0]).ToUpperInvariant();
			string nonLocalizedText = Localization.Get("Text.Menu.SaveSlotInfo", new LocToken[]
			{
				new LocToken("[DLCNAME]", replaceWith)
			});
			this.m_info.SetNonLocalizedText(nonLocalizedText);
		}
		this.m_pendingForceClose = false;
		return true;
	}

	// Token: 0x0600391E RID: 14622 RVA: 0x0010EEA0 File Offset: 0x0010D2A0
	private DLCFrontendData GetDLCFrontendData(int dlcNum)
	{
		DLCManager dlcmanager = GameUtils.RequestManager<DLCManager>();
		if (dlcmanager == null)
		{
			return null;
		}
		List<DLCFrontendData> allDlc = dlcmanager.AllDlc;
		for (int i = 0; i < allDlc.Count; i++)
		{
			if (allDlc[i].m_DLCID == this.m_dlcNum)
			{
				return allDlc[i];
			}
		}
		return null;
	}

	// Token: 0x0600391F RID: 14623 RVA: 0x0010EF00 File Offset: 0x0010D300
	private void OnEngagementChanged(EngagementSlot _s, GamepadUser _p, GamepadUser _n)
	{
		IPlayerManager playerManager = GameUtils.RequestManagerInterface<IPlayerManager>();
		GamepadUser user = playerManager.GetUser(EngagementSlot.One);
		T17EventSystem eventSystemForGamepadUser = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user);
		if (eventSystemForGamepadUser == null)
		{
			return;
		}
		playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		this.m_eventSystem = eventSystemForGamepadUser;
		this.m_eventSystem.SetSelectedGameObject(this.m_saveElements[0].gameObject);
	}

	// Token: 0x06003920 RID: 14624 RVA: 0x0010EF68 File Offset: 0x0010D368
	protected IEnumerator ShowSlotElements(SaveDialogMode _mode, int _dlcNum, GamepadUser _currentGamer, BaseMenuBehaviour _parent, GameObject _invoker, bool _hideInvoker)
	{
		if (this.m_eventSystem != null)
		{
			this.m_suppressor = this.m_eventSystem.Disable(this);
		}
		for (int j = 0; j < this.m_saveElements.Length; j++)
		{
			this.m_saveElements[j].Hide(false, false);
		}
		for (int i = 0; i < this.m_saveElements.Length; i++)
		{
			IEnumerator slotLoad = this.m_saveElements[i].LoadSlotData(_dlcNum);
			while (slotLoad.MoveNext())
			{
				yield return null;
			}
		}
		if (null != this.m_DontSaveButton)
		{
			this.m_DontSaveButton.SetActive(true);
		}
		T17FrontendFlow.Instance.StartClientCountdown();
		for (int k = 0; k < this.m_saveElements.Length; k++)
		{
			this.m_saveElements[k].Mode = _mode;
			this.m_saveElements[k].DLC = _dlcNum;
			this.m_saveElements[k].Show(_currentGamer, this, _invoker, _hideInvoker);
		}
		if (this.m_eventSystem != null)
		{
			this.ReleaseHolder();
			this.m_eventSystem.SetSelectedGameObject(this.m_saveElements[0].gameObject);
		}
		GameSession session = SelectSaveDialog.CreateFreshGameSessionForSlot(this.DLC, -1);
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost() && session != null)
		{
			session.Progress.UseSlaveSlot = false;
		}
		yield break;
	}

	// Token: 0x06003921 RID: 14625 RVA: 0x0010EFA8 File Offset: 0x0010D3A8
	protected override void Update()
	{
		base.Update();
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			if (null != this.m_Timer && T17FrontendFlow.Instance != null)
			{
				this.m_Timer.text = Mathf.FloorToInt(T17FrontendFlow.Instance.ClientCountdown + 0.99f).ToString();
			}
			if (T17FrontendFlow.Instance.ClientCountdownRunning && T17FrontendFlow.Instance.ClientCountdown <= 0f)
			{
				ClientMessenger.GameState(GameState.LoadedCampaignMapSave);
				this.ForceClose();
			}
		}
		if (this.m_slotUpdate != null && !this.m_slotUpdate.MoveNext())
		{
			this.m_slotUpdate = null;
		}
		if (this.m_pendingForceClose)
		{
			Debug.Log("Retrying force close...");
			base.Close();
		}
	}

	// Token: 0x06003922 RID: 14626 RVA: 0x0010F08C File Offset: 0x0010D48C
	public void OnDontSaveSelected()
	{
		ClientMessenger.GameState(GameState.LoadedCampaignMapSave);
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession != null)
		{
			gameSession.Progress.UseSlaveSlot = false;
		}
		this.ForceClose();
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.ShowWaitingForPlayers();
		}
	}

	// Token: 0x06003923 RID: 14627 RVA: 0x0010F0E0 File Offset: 0x0010D4E0
	public bool CanHide()
	{
		for (int i = 0; i < this.m_saveElements.Length; i++)
		{
			if (!this.m_saveElements[i].CanHide())
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06003924 RID: 14628 RVA: 0x0010F11B File Offset: 0x0010D51B
	public void HandleDisconnection(GenericVoid _disconnectionHandledCallback = null)
	{
		this.m_closeSuccessCallback = (GenericVoid)Delegate.Combine(this.m_closeSuccessCallback, _disconnectionHandledCallback);
		this.ForceClose();
	}

	// Token: 0x06003925 RID: 14629 RVA: 0x0010F13C File Offset: 0x0010D53C
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (!this.CanHide())
		{
			return false;
		}
		for (int i = 0; i < this.m_saveElements.Length; i++)
		{
			SaveSlotElement saveSlotElement = this.m_saveElements[i];
			if (!saveSlotElement.CleanUp())
			{
				return false;
			}
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.HideWaitingForPlayers();
		}
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		for (int j = 0; j < this.m_saveElements.Length; j++)
		{
			this.m_saveElements[j].Hide(restoreInvokerState, isTabSwitch);
		}
		if (null != this.m_DontSaveButton)
		{
			this.m_DontSaveButton.SetActive(false);
		}
		if (this.m_closeSuccessCallback != null)
		{
			this.m_closeSuccessCallback();
			this.m_closeSuccessCallback = null;
		}
		this.ReleaseHolder();
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = false;
		}
		if (this.m_engagementSuppressor != null)
		{
			this.m_engagementSuppressor.Release();
			this.m_engagementSuppressor = null;
		}
		if (this.m_legend)
		{
			this.m_legend.SetActive(true);
		}
		InviteMonitor.InviteJoinComplete = (GenericVoid)Delegate.Remove(InviteMonitor.InviteJoinComplete, new GenericVoid(this.OnInviteJoinComplete));
		this.m_pendingForceClose = false;
		SelectSaveDialog.s_cachedHighScores = null;
		return true;
	}

	// Token: 0x06003926 RID: 14630 RVA: 0x0010F299 File Offset: 0x0010D699
	protected void ReleaseHolder()
	{
		if (this.m_eventSystem != null && this.m_suppressor != null)
		{
			this.m_eventSystem.ReleaseSuppressor(this.m_suppressor);
			this.m_suppressor = null;
		}
	}

	// Token: 0x06003927 RID: 14631 RVA: 0x0010F2CF File Offset: 0x0010D6CF
	public override void Close()
	{
		if (!T17DialogBoxManager.HasAnyOpenDialogs() && this.CanHide())
		{
			this.ForceClose();
		}
	}

	// Token: 0x06003928 RID: 14632 RVA: 0x0010F2EC File Offset: 0x0010D6EC
	private void ForceClose()
	{
		IPlayerManager playerManager = GameUtils.RequestManagerInterface<IPlayerManager>();
		if (playerManager != null)
		{
			playerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		}
		if (this.m_legend)
		{
			this.m_legend.SetActive(true);
		}
		this.m_pendingForceClose = true;
		for (int i = 0; i < this.m_saveElements.Length; i++)
		{
			this.m_saveElements[i].InformImpendingForceClose();
		}
		base.Close();
	}

	// Token: 0x06003929 RID: 14633 RVA: 0x0010F366 File Offset: 0x0010D766
	private void OnInviteJoinComplete()
	{
		this.ForceClose();
	}

	// Token: 0x0600392A RID: 14634 RVA: 0x0010F370 File Offset: 0x0010D770
	public IEnumerator LoadSlot(int _dlcNum, int _slotNum)
	{
		if (this.m_gamepadEngagementManager != null)
		{
			this.m_engagementSuppressor = this.m_gamepadEngagementManager.Suppressor.AddSuppressor(this);
		}
		for (int i = 0; i < this.m_saveElements.Length; i++)
		{
			SaveSlotElement saveSlot = this.m_saveElements[i];
			if (saveSlot.Slot == _slotNum)
			{
				saveSlot.Mode = SaveDialogMode.LoadGame;
				saveSlot.DLC = _dlcNum;
				IEnumerator loadSlot = saveSlot.TriggerSlotRoutine();
				while (loadSlot.MoveNext())
				{
					yield return null;
				}
				break;
			}
		}
		if (this.m_engagementSuppressor != null)
		{
			this.m_engagementSuppressor.Release();
			this.m_engagementSuppressor = null;
		}
		yield break;
	}

	// Token: 0x0600392B RID: 14635 RVA: 0x0010F39C File Offset: 0x0010D79C
	public bool LoadReadySlot()
	{
		for (int i = 0; i < this.m_saveElements.Length; i++)
		{
			SaveSlotElement saveSlotElement = this.m_saveElements[i];
			if (saveSlotElement.SaveGameReady)
			{
				saveSlotElement.ServerLoadCampaign(GameUtils.GetGameSession());
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600392C RID: 14636 RVA: 0x0010F3E4 File Offset: 0x0010D7E4
	public static GameSession CreateFreshGameSessionForSlot(int _dlcNum, int _slotNum)
	{
		GameSession gameSession = GameUtils.GetGameSession();
		GameSession gameSession2 = T17FrontendFlow.Instance.StartEmptySession(GameSession.GameType.Cooperative, _dlcNum);
		gameSession2.SaveSlot = _slotNum;
		if (SelectSaveDialog.s_cachedHighScores != null && SelectSaveDialog.s_cachedHighScores.DLC == _dlcNum)
		{
			gameSession2.HighScoreRepository.Fill(SelectSaveDialog.s_cachedHighScores, false);
		}
		SelectSaveDialog.s_cachedHighScores = gameSession2.HighScoreRepository;
		return gameSession2;
	}

	// Token: 0x0600392D RID: 14637 RVA: 0x0010F442 File Offset: 0x0010D842
	protected override void OnDestroy()
	{
		base.OnDestroy();
		InviteMonitor.InviteJoinComplete = (GenericVoid)Delegate.Remove(InviteMonitor.InviteJoinComplete, new GenericVoid(this.OnInviteJoinComplete));
		this.ForceClose();
	}

	// Token: 0x04002DD6 RID: 11734
	[Header("Select Save Dialog")]
	[SerializeField]
	private string m_newGameHeader;

	// Token: 0x04002DD7 RID: 11735
	[SerializeField]
	private string m_loadHeader;

	// Token: 0x04002DD8 RID: 11736
	[SerializeField]
	private T17Text m_header;

	// Token: 0x04002DD9 RID: 11737
	[SerializeField]
	private SaveSlotElement[] m_saveElements;

	// Token: 0x04002DDA RID: 11738
	[SerializeField]
	public GameObject m_PleaseWaitMenu;

	// Token: 0x04002DDB RID: 11739
	[SerializeField]
	public T17Text m_Timer;

	// Token: 0x04002DDC RID: 11740
	[SerializeField]
	public GameObject m_DontSaveButton;

	// Token: 0x04002DDD RID: 11741
	private SaveDialogMode m_mode;

	// Token: 0x04002DDE RID: 11742
	private int m_dlcNum;

	// Token: 0x04002DDF RID: 11743
	private IEnumerator m_slotUpdate;

	// Token: 0x04002DE0 RID: 11744
	private T17EventSystem m_eventSystem;

	// Token: 0x04002DE1 RID: 11745
	private Suppressor m_suppressor;

	// Token: 0x04002DE2 RID: 11746
	private GamepadEngagementManager m_gamepadEngagementManager;

	// Token: 0x04002DE3 RID: 11747
	private Suppressor m_engagementSuppressor;

	// Token: 0x04002DE4 RID: 11748
	private GenericVoid m_closeSuccessCallback;

	// Token: 0x04002DE5 RID: 11749
	[SerializeField]
	private GameObject m_legend;

	// Token: 0x04002DE6 RID: 11750
	[SerializeField]
	private T17Text m_info;

	// Token: 0x04002DE7 RID: 11751
	private bool m_pendingForceClose;

	// Token: 0x04002DE8 RID: 11752
	private static HighScoreRepository s_cachedHighScores;
}
