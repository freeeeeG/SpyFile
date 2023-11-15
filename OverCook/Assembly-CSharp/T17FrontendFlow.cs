using System;
using System.Collections;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000771 RID: 1905
public class T17FrontendFlow : MonoBehaviour
{
	// Token: 0x170002DD RID: 733
	// (get) Token: 0x06002497 RID: 9367 RVA: 0x000AD20F File Offset: 0x000AB60F
	public static T17FrontendFlow Instance
	{
		get
		{
			return T17FrontendFlow.s_Instance;
		}
	}

	// Token: 0x170002DE RID: 734
	// (get) Token: 0x06002498 RID: 9368 RVA: 0x000AD216 File Offset: 0x000AB616
	// (set) Token: 0x06002499 RID: 9369 RVA: 0x000AD21E File Offset: 0x000AB61E
	public float ServerCountdown { get; private set; }

	// Token: 0x170002DF RID: 735
	// (get) Token: 0x0600249A RID: 9370 RVA: 0x000AD227 File Offset: 0x000AB627
	// (set) Token: 0x0600249B RID: 9371 RVA: 0x000AD22F File Offset: 0x000AB62F
	public float ClientCountdown { get; private set; }

	// Token: 0x170002E0 RID: 736
	// (get) Token: 0x0600249C RID: 9372 RVA: 0x000AD238 File Offset: 0x000AB638
	public bool ClientCountdownRunning
	{
		get
		{
			return this.m_bShouldClientCountdown;
		}
	}

	// Token: 0x170002E1 RID: 737
	// (set) Token: 0x0600249D RID: 9373 RVA: 0x000AD240 File Offset: 0x000AB640
	public bool allowMultichefMenu
	{
		set
		{
			this.m_allowMultichefMenu = value;
		}
	}

	// Token: 0x170002E2 RID: 738
	// (get) Token: 0x0600249E RID: 9374 RVA: 0x000AD249 File Offset: 0x000AB649
	// (set) Token: 0x0600249F RID: 9375 RVA: 0x000AD251 File Offset: 0x000AB651
	public DLCFrontendData AutoOpenFrontendDlcData { get; private set; }

	// Token: 0x060024A0 RID: 9376 RVA: 0x000AD25A File Offset: 0x000AB65A
	public void AutoOpenChefSelectionMenu(DLCFrontendData data)
	{
		this.AutoOpenFrontendDlcData = data;
		if (data != null)
		{
			this.m_autoOpenChefSelectionMenu = true;
			this.m_CurrentCameraState = T17FrontendFlow.CameraState.eTransitioning;
		}
		else
		{
			this.m_autoOpenChefSelectionMenu = false;
		}
	}

	// Token: 0x060024A1 RID: 9377 RVA: 0x000AD289 File Offset: 0x000AB689
	public bool IsCameraTransitioning()
	{
		return this.m_CurrentCameraState == T17FrontendFlow.CameraState.eTransitioning;
	}

	// Token: 0x170002E3 RID: 739
	// (get) Token: 0x060024A2 RID: 9378 RVA: 0x000AD294 File Offset: 0x000AB694
	// (set) Token: 0x060024A3 RID: 9379 RVA: 0x000AD29C File Offset: 0x000AB69C
	public bool BlockFocusKitchen
	{
		get
		{
			return this.m_bBlockFocusKitchen;
		}
		set
		{
			this.m_bBlockFocusKitchen = value;
		}
	}

	// Token: 0x060024A4 RID: 9380 RVA: 0x000AD2A8 File Offset: 0x000AB6A8
	public void Awake()
	{
		if (T17FrontendFlow.s_Instance != null)
		{
			UnityEngine.Object.Destroy(this);
		}
		else
		{
			T17FrontendFlow.s_Instance = this;
		}
		if (this.m_Rootmenu != null)
		{
			this.m_Rootmenu.Hide(true, false);
			this.m_saveDialog = this.m_Rootmenu.SearchAllForMenuOfType<SelectSaveDialog>();
		}
		if (this.m_PlayerLobbySwitch != null)
		{
			UnityEngine.Object.Destroy(this.m_PlayerLobbySwitch.gameObject);
			this.m_PlayerLobbySwitch = null;
		}
		if (this.m_PlayerLobby != null)
		{
			this.m_PlayerLobby.SetupNetworking();
			this.m_PlayerLobby.Hide(true, false);
		}
		this.m_IPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		this.m_bServerCountdownRunning = false;
		this.m_ServerCountdownDuration = 71f;
		UserSystemUtils.OnServerChangedGameState = (GenericVoid<GameState, GameStateMessage.GameStatePayload>)Delegate.Combine(UserSystemUtils.OnServerChangedGameState, new GenericVoid<GameState, GameStateMessage.GameStatePayload>(this.OnServerGameStateChanged));
	}

	// Token: 0x060024A5 RID: 9381 RVA: 0x000AD398 File Offset: 0x000AB798
	private void OnDestroy()
	{
		if (this.m_gamepadEngagementManager != null)
		{
			this.m_gamepadEngagementManager.CanManuallyChangeEngagement = false;
		}
		this.m_IPlayerManager.EngagementChangeCallback -= this.OnEngagementChangedReopenMenus;
		DisconnectionHandler.SessionConnectionLostEvent = (GenericVoid)Delegate.Remove(DisconnectionHandler.SessionConnectionLostEvent, new GenericVoid(this.OnSessionConnectionLost));
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Remove(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Remove(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnKickedFromSession));
		DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Remove(DisconnectionHandler.LocalDisconnectionEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(this.OnLocalDisconnection));
		InviteMonitor.InviteAccepted = (GenericVoid)Delegate.Remove(InviteMonitor.InviteAccepted, new GenericVoid(this.OnInviteAccepted));
		InviteMonitor.InviteJoinComplete = (GenericVoid)Delegate.Remove(InviteMonitor.InviteJoinComplete, new GenericVoid(this.OnInviteJoinComplete));
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		this.m_OnlineClientSaveDialog.Hide(true, false);
		if (T17FrontendFlow.s_Instance != null)
		{
			UnityEngine.Object.Destroy(T17FrontendFlow.s_Instance);
			T17FrontendFlow.s_Instance = null;
		}
		UserSystemUtils.OnServerChangedGameState = (GenericVoid<GameState, GameStateMessage.GameStatePayload>)Delegate.Remove(UserSystemUtils.OnServerChangedGameState, new GenericVoid<GameState, GameStateMessage.GameStatePayload>(this.OnServerGameStateChanged));
	}

	// Token: 0x060024A6 RID: 9382 RVA: 0x000AD500 File Offset: 0x000AB900
	private void Start()
	{
		if (!this.m_IPlayerManager.HasPlayer())
		{
		}
		this.m_eventSystem = this.GetPrimaryEventSystem();
		GamepadUser user = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		if (user == null)
		{
			this.m_IPlayerManager.EngagementChangeCallback += this.OnEngagementChangedReopenMenus;
		}
		this.m_ToggleMultichefMenu = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UIMultiChefMenu, PlayerInputLookup.Player.One);
		this.m_UnfocusMultichefMenu = PlayerInputLookup.GetUIButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One);
		if (this.m_PlayerLobby != null)
		{
			this.m_PlayerLobby.Show(user, null, null, true);
		}
		if (this.m_Rootmenu != null)
		{
			this.m_Rootmenu.Show(user, null, null, true);
		}
		this.m_allowMultichefMenu = true;
		this.m_gamepadEngagementManager = GameUtils.RequireManager<GamepadEngagementManager>();
		if (this.m_gamepadEngagementManager != null)
		{
			this.m_gamepadEngagementManager.CanManuallyChangeEngagement = true;
		}
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
		DisconnectionHandler.SessionConnectionLostEvent = (GenericVoid)Delegate.Combine(DisconnectionHandler.SessionConnectionLostEvent, new GenericVoid(this.OnSessionConnectionLost));
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Combine(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Combine(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnKickedFromSession));
		DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Combine(DisconnectionHandler.LocalDisconnectionEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(this.OnLocalDisconnection));
		InviteMonitor.InviteAccepted = (GenericVoid)Delegate.Combine(InviteMonitor.InviteAccepted, new GenericVoid(this.OnInviteAccepted));
		InviteMonitor.InviteJoinComplete = (GenericVoid)Delegate.Combine(InviteMonitor.InviteJoinComplete, new GenericVoid(this.OnInviteJoinComplete));
	}

	// Token: 0x060024A7 RID: 9383 RVA: 0x000AD6BE File Offset: 0x000ABABE
	private void OnSessionConnectionLost()
	{
		this.HandleDisconnection();
	}

	// Token: 0x060024A8 RID: 9384 RVA: 0x000AD6C6 File Offset: 0x000ABAC6
	private void OnConnectionModeError(OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> _error)
	{
		this.HandleDisconnection();
	}

	// Token: 0x060024A9 RID: 9385 RVA: 0x000AD6CE File Offset: 0x000ABACE
	private void OnKickedFromSession()
	{
		this.HandleDisconnection();
	}

	// Token: 0x060024AA RID: 9386 RVA: 0x000AD6D6 File Offset: 0x000ABAD6
	private void OnLocalDisconnection(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> _error)
	{
		this.HandleDisconnection();
	}

	// Token: 0x060024AB RID: 9387 RVA: 0x000AD6DE File Offset: 0x000ABADE
	public void OnSessionLeft()
	{
		this.HandleDisconnection();
	}

	// Token: 0x060024AC RID: 9388 RVA: 0x000AD6E8 File Offset: 0x000ABAE8
	public void HandleDisconnection()
	{
		this.HideWaitingForPlayers();
		if (this.m_saveDialog.gameObject.activeInHierarchy)
		{
			this.m_saveDialog.HandleDisconnection(new GenericVoid(this.OnSaveDialogDisconnectionHandled));
		}
		else if (this.m_OnlineClientSaveDialog.gameObject.activeInHierarchy)
		{
			this.m_OnlineClientSaveDialog.HandleDisconnection(new GenericVoid(this.OnSaveDialogDisconnectionHandled));
		}
	}

	// Token: 0x060024AD RID: 9389 RVA: 0x000AD758 File Offset: 0x000ABB58
	private void OnSaveDialogDisconnectionHandled()
	{
		if (this.m_Rootmenu != null)
		{
			this.m_Rootmenu.HideMenuStack();
			this.m_Rootmenu.ExpandCurrentTab();
		}
		if (this.m_bServerCountdownRunning)
		{
			this.StopServerCountdown();
		}
	}

	// Token: 0x060024AE RID: 9390 RVA: 0x000AD794 File Offset: 0x000ABB94
	public void ShowClientSaveDialog()
	{
		if (this.m_ClientSavePayload != null && !ConnectionStatus.IsHost() && ConnectionStatus.IsInSession())
		{
			BaseMenuBehaviour currentOpenMenu = this.m_Rootmenu.GetCurrentOpenMenu();
			if (currentOpenMenu is FrontendSettingsTabOptions)
			{
				FrontendOptionsMenu frontendOptionsMenu = this.m_Rootmenu.SearchAllForMenuOfType<FrontendOptionsMenu>();
				if (null != frontendOptionsMenu)
				{
					frontendOptionsMenu.ResetAndClose();
				}
				FrontendControllerOptionsMenu frontendControllerOptionsMenu = this.m_Rootmenu.SearchAllForMenuOfType<FrontendControllerOptionsMenu>();
				if (null != frontendControllerOptionsMenu)
				{
					frontendControllerOptionsMenu.CancelAndCloseAllDialogs();
				}
			}
			this.m_OnlineClientSaveDialog.Mode = SaveDialogMode.LoadGame;
			this.m_OnlineClientSaveDialog.DLC = this.m_ClientSavePayload.DLCID;
			this.m_Rootmenu.HideMenuStack();
			this.m_Rootmenu.OpenFrontendMenu(this.m_OnlineClientSaveDialog);
			this.ClientCountdown = 60f;
			this.m_bShouldClientCountdown = false;
			this.m_ClientSavePayload = null;
		}
	}

	// Token: 0x060024AF RID: 9391 RVA: 0x000AD86C File Offset: 0x000ABC6C
	private bool CanShowClientSaveDialog()
	{
		return ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost() && this.m_ClientSavePayload != null && ConnectionModeSwitcher.GetStatus().GetProgress() == eConnectionModeSwitchProgress.Complete && ConnectionModeSwitcher.GetStatus().GetResult() == eConnectionModeSwitchResult.Success && false == this.m_PlayerLobby.IsKitchenConnectionTaskRunning();
	}

	// Token: 0x060024B0 RID: 9392 RVA: 0x000AD8CC File Offset: 0x000ABCCC
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.SelectCampaignMapSave)
		{
			if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
			{
				this.m_ClientSavePayload = (GameStateMessage.ClientSavePayload)gameStateMessage.Payload;
			}
			if (this.CanShowClientSaveDialog())
			{
				this.ShowClientSaveDialog();
			}
		}
		else if (gameStateMessage.m_State == GameState.CampaignMap)
		{
			this.m_OnlineClientSaveDialog.Hide(true, false);
		}
	}

	// Token: 0x060024B1 RID: 9393 RVA: 0x000AD942 File Offset: 0x000ABD42
	private void OnInviteAccepted()
	{
		if (!ConnectionStatus.IsInSession() || ConnectionStatus.IsHost())
		{
			this.StopServerCountdown();
		}
		this.m_bShouldClientCountdown = false;
	}

	// Token: 0x060024B2 RID: 9394 RVA: 0x000AD965 File Offset: 0x000ABD65
	private void OnInviteJoinComplete()
	{
		if (this.m_Rootmenu != null)
		{
			this.m_Rootmenu.ExpandCurrentTab();
		}
	}

	// Token: 0x060024B3 RID: 9395 RVA: 0x000AD984 File Offset: 0x000ABD84
	private void Update()
	{
		if (T17DialogBoxManager.HasAnyOpenDialogs())
		{
			if (this.m_ToggleMultichefMenu != null)
			{
				this.m_ToggleMultichefMenu.ClaimPressEvent();
				this.m_ToggleMultichefMenu.ClaimReleaseEvent();
			}
			if (this.m_UnfocusMultichefMenu != null)
			{
				this.m_UnfocusMultichefMenu.ClaimPressEvent();
				this.m_UnfocusMultichefMenu.ClaimReleaseEvent();
			}
			if (this.m_quitButton != null)
			{
				this.m_quitButton.ClaimPressEvent();
				this.m_quitButton.ClaimReleaseEvent();
			}
		}
		if (this.m_eventSystem == null)
		{
			this.m_eventSystem = this.GetPrimaryEventSystem();
		}
		bool flag = false;
		if (!this.m_bBlockFocusKitchen && this.m_CurrentCameraState != T17FrontendFlow.CameraState.eTransitioning && this.m_eventSystem != null && !this.m_eventSystem.IsDisabled())
		{
			if (this.m_ToggleMultichefMenu != null && this.m_ToggleMultichefMenu.JustPressed())
			{
				flag = true;
			}
			GameObject lastRequestedSelectedGameobject = this.m_eventSystem.GetLastRequestedSelectedGameobject();
			if (lastRequestedSelectedGameobject != null)
			{
				bool isFocused = this.m_PlayerLobby.IsFocused;
				if (isFocused && this.m_Rootmenu.IsChildMenuOpen())
				{
					flag = true;
				}
				else if (!isFocused && lastRequestedSelectedGameobject.IsInHierarchyOf(this.m_PlayerLobby.gameObject))
				{
					flag = true;
				}
			}
		}
		if (flag)
		{
			if (!this.m_PlayerLobby.IsFocused)
			{
				if (this.m_allowMultichefMenu)
				{
					this.FocusOnMultiplayerKitchen(false);
				}
			}
			else
			{
				this.FocusOnMainMenu();
			}
		}
		if (this.m_CurrentCameraState == T17FrontendFlow.CameraState.eFocusedOnKitchen && !this.m_PlayerLobby.IsFocused)
		{
			this.UnFocusKitchenLobby();
		}
		else if (this.m_CurrentCameraState != T17FrontendFlow.CameraState.eFocusedOnKitchen && this.m_PlayerLobby.IsFocused)
		{
			this.FocusOnKitchenLobby();
		}
		if (!this.m_bBlockFocusKitchen && this.m_UnfocusMultichefMenu != null && this.m_UnfocusMultichefMenu.JustPressed() && this.m_PlayerLobby.IsFocused && !this.m_PlayerLobby.IsPlayerSlotMenuOpen)
		{
			this.FocusOnMainMenu();
		}
		if (this.m_bServerCountdownRunning && ConnectionStatus.IsHost() && UserSystemUtils.AreAnyUsersInGameState(ServerUserSystem.m_Users, GameState.SelectCampaignMapSave))
		{
			this.ServerCountdown -= Time.deltaTime;
			this.ClientCountdown -= Time.deltaTime;
			if (this.ServerCountdown <= 0f)
			{
				this.ServerLoadCampaign();
			}
		}
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			if (this.CanShowClientSaveDialog())
			{
				this.ShowClientSaveDialog();
			}
			if (this.m_bShouldClientCountdown)
			{
				this.ClientCountdown -= Time.deltaTime;
			}
		}
		if (this.m_saveWaitDialog.gameObject.activeInHierarchy)
		{
			if (T17DialogBoxManager.HasAnyOpenDialogs())
			{
				if (this.m_ClientSaveWaitSuppressor != null)
				{
					this.m_ClientSaveWaitSuppressor.Release();
					this.m_ClientSaveWaitSuppressor = null;
				}
			}
			else if (this.m_ClientSaveWaitSuppressor == null)
			{
				this.m_ClientSaveWaitSuppressor = this.m_eventSystem.Disable(this.m_saveWaitDialog);
			}
		}
	}

	// Token: 0x060024B4 RID: 9396 RVA: 0x000ADCA0 File Offset: 0x000AC0A0
	private void LateUpdate()
	{
		if (this.m_CurrentCameraState == T17FrontendFlow.CameraState.eTransitioning)
		{
			if (!this.m_CameraAnimator.GetBool("Transitioning"))
			{
				this.m_CurrentCameraState = this.m_PostTransitionState;
				if (this.m_PostTransitionState == T17FrontendFlow.CameraState.ePulledBack && this.m_autoOpenChefSelectionMenu)
				{
					this.m_autoOpenChefSelectionMenu = false;
					FrontendMenuBehaviour frontendMenuBehaviour = this.m_Rootmenu.SearchAllForMenuOfType<FrontendChefMenu>();
					if (frontendMenuBehaviour != null)
					{
						this.m_Rootmenu.OpenFrontendMenu(frontendMenuBehaviour);
					}
				}
			}
		}
		else if (this.m_PendingCameraState != this.m_CurrentCameraState)
		{
			switch (this.m_PendingCameraState)
			{
			case T17FrontendFlow.CameraState.ePushedForward:
				this.DoPushForwardCamera();
				break;
			case T17FrontendFlow.CameraState.ePulledBack:
				this.DoPullBackCamera();
				break;
			case T17FrontendFlow.CameraState.eFocusedOnKitchen:
				this.DoFocusOnKitchenLobby();
				break;
			case T17FrontendFlow.CameraState.eUnFocusedOnKitchen:
				this.DoUnFocusKitchenLobby();
				break;
			}
		}
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x000ADD80 File Offset: 0x000AC180
	public void FocusOnMultiplayerKitchen(bool bForce = false)
	{
		if (bForce || (!this.m_PlayerLobby.IsFocused && this.m_allowMultichefMenu))
		{
			this.m_Rootmenu.CollapseCurrentTab();
			this.m_PlayerLobby.FocusMenu();
			this.FocusOnKitchenLobby();
			this.m_Rootmenu.SetLegendText(this.m_PlayerLobby.GetLegendText());
			if (this.m_FocusKitchenImage != null)
			{
				this.m_FocusKitchenImage.SetActive(false);
			}
		}
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x000ADE00 File Offset: 0x000AC200
	public void FocusOnMainMenu()
	{
		if (this.m_PlayerLobby.IsFocused)
		{
			this.m_PlayerLobby.UnFocusMenu();
			this.m_Rootmenu.ExpandCurrentTab();
			this.UnFocusKitchenLobby();
			if (this.m_FocusKitchenImage != null)
			{
				this.m_FocusKitchenImage.SetActive(true);
			}
		}
	}

	// Token: 0x060024B7 RID: 9399 RVA: 0x000ADE58 File Offset: 0x000AC258
	public GameSession StartEmptySession(GameSession.GameType gameType, int dlcNum)
	{
		int saveSlot = 0;
		GameSession gameSession = GameUtils.GetGameSession();
		if (gameSession != null)
		{
			saveSlot = gameSession.SaveSlot;
			UnityEngine.Object.DestroyImmediate(gameSession.gameObject);
		}
		GameSession[] array = null;
		if (gameType != GameSession.GameType.Cooperative)
		{
			if (gameType == GameSession.GameType.Competitive)
			{
				array = this.m_CompetitiveGameSessionPrefabs.AllData;
			}
		}
		else
		{
			array = this.m_CoopGameSessionPrefabs.AllData;
		}
		array = array.AllRemoved_Predicate((GameSession x) => x == null);
		GameSession gameSession2 = Array.Find<GameSession>(array, (GameSession x) => x.DLC == dlcNum);
		if (gameSession2 != null)
		{
			GameObject obj = gameSession2.gameObject.InstantiateOnParent(null, true);
			GameSession gameSession3 = obj.RequireComponent<GameSession>();
			gameSession3.TypeSettings.Type = gameType;
			gameSession3.SaveSlot = saveSlot;
			return gameSession3;
		}
		return null;
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x000ADF45 File Offset: 0x000AC345
	public void CheckSessionForSaveFile(GameSession session, GenericVoid<GameSession, SaveLoadResult> onComplete)
	{
		if (session != null)
		{
			base.StartCoroutine(this.CheckForSaveFile(session, onComplete));
		}
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x000ADF64 File Offset: 0x000AC364
	private IEnumerator CheckForSaveFile(GameSession session, GenericVoid<GameSession, SaveLoadResult> onComplete)
	{
		ReturnValue<SaveLoadResult> result = new ReturnValue<SaveLoadResult>();
		IEnumerator hasSaveRoutine = session.HasSaveFile(result);
		while (hasSaveRoutine.MoveNext())
		{
			yield return null;
		}
		if (onComplete != null)
		{
			onComplete(session, result.Value);
		}
		yield break;
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x000ADF86 File Offset: 0x000AC386
	public void PullBackCamera()
	{
		this.m_PendingCameraState = T17FrontendFlow.CameraState.ePulledBack;
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x000ADF90 File Offset: 0x000AC390
	private void DoPullBackCamera()
	{
		this.m_CameraAnimator.SetTrigger("Pull Back");
		this.m_CameraAnimator.ResetTrigger("Push Forward");
		this.m_CameraAnimator.ResetTrigger("Focus Kitchen");
		this.m_CameraAnimator.ResetTrigger("UnFocus Kitchen");
		this.m_CurrentCameraState = T17FrontendFlow.CameraState.eTransitioning;
		this.m_PostTransitionState = T17FrontendFlow.CameraState.ePulledBack;
		this.m_CameraAnimator.Update(0f);
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x000ADFFB File Offset: 0x000AC3FB
	public void PushForwardCamera()
	{
		this.m_PendingCameraState = T17FrontendFlow.CameraState.ePushedForward;
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x000AE004 File Offset: 0x000AC404
	private void DoPushForwardCamera()
	{
		this.m_CameraAnimator.SetTrigger("Push Forward");
		this.m_CameraAnimator.ResetTrigger("Pull Back");
		this.m_CameraAnimator.ResetTrigger("Focus Kitchen");
		this.m_CameraAnimator.ResetTrigger("UnFocus Kitchen");
		this.m_CurrentCameraState = T17FrontendFlow.CameraState.eTransitioning;
		this.m_PostTransitionState = T17FrontendFlow.CameraState.ePushedForward;
		this.m_CameraAnimator.Update(0f);
	}

	// Token: 0x060024BE RID: 9406 RVA: 0x000AE06F File Offset: 0x000AC46F
	public void FocusOnKitchenLobby()
	{
		this.m_PendingCameraState = T17FrontendFlow.CameraState.eFocusedOnKitchen;
	}

	// Token: 0x060024BF RID: 9407 RVA: 0x000AE078 File Offset: 0x000AC478
	private void DoFocusOnKitchenLobby()
	{
		this.m_CameraAnimator.SetTrigger("Focus Kitchen");
		this.m_CameraAnimator.ResetTrigger("Push Forward");
		this.m_CameraAnimator.ResetTrigger("Pull Back");
		this.m_CameraAnimator.ResetTrigger("UnFocus Kitchen");
		this.m_CurrentCameraState = T17FrontendFlow.CameraState.eTransitioning;
		this.m_PostTransitionState = T17FrontendFlow.CameraState.eFocusedOnKitchen;
		this.m_CameraAnimator.Update(0f);
	}

	// Token: 0x060024C0 RID: 9408 RVA: 0x000AE0E3 File Offset: 0x000AC4E3
	public void UnFocusKitchenLobby()
	{
		this.m_PendingCameraState = T17FrontendFlow.CameraState.eUnFocusedOnKitchen;
	}

	// Token: 0x060024C1 RID: 9409 RVA: 0x000AE0EC File Offset: 0x000AC4EC
	private void DoUnFocusKitchenLobby()
	{
		this.m_CameraAnimator.SetTrigger("UnFocus Kitchen");
		this.m_CameraAnimator.ResetTrigger("Push Forward");
		this.m_CameraAnimator.ResetTrigger("Pull Back");
		this.m_CameraAnimator.ResetTrigger("Focus Kitchen");
		this.m_CurrentCameraState = T17FrontendFlow.CameraState.eTransitioning;
		this.m_PostTransitionState = T17FrontendFlow.CameraState.ePulledBack;
		this.m_CameraAnimator.Update(0f);
		this.m_PendingCameraState = T17FrontendFlow.CameraState.ePulledBack;
	}

	// Token: 0x060024C2 RID: 9410 RVA: 0x000AE160 File Offset: 0x000AC560
	public void ResetCamera()
	{
		this.m_PostTransitionState = T17FrontendFlow.CameraState.ePushedForward;
		this.m_CurrentCameraState = T17FrontendFlow.CameraState.ePushedForward;
		this.m_CameraAnimator.SetTrigger("Reset");
		this.m_CameraAnimator.ResetTrigger("UnFocus Kitchen");
		this.m_CameraAnimator.ResetTrigger("Push Forward");
		this.m_CameraAnimator.ResetTrigger("Pull Back");
		this.m_CameraAnimator.ResetTrigger("Focus Kitchen");
	}

	// Token: 0x060024C3 RID: 9411 RVA: 0x000AE1CC File Offset: 0x000AC5CC
	protected T17EventSystem GetPrimaryEventSystem()
	{
		GamepadUser user = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		T17EventSystem t17EventSystem = null;
		if (user != null)
		{
			t17EventSystem = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user);
			if (t17EventSystem == null)
			{
				T17EventSystemsManager.Instance.AssignFreeEventSystemToGamepadUser(user);
				t17EventSystem = T17EventSystemsManager.Instance.GetEventSystemForGamepadUser(user);
			}
		}
		return t17EventSystem;
	}

	// Token: 0x060024C4 RID: 9412 RVA: 0x000AE228 File Offset: 0x000AC628
	private void OnEngagementChangedReopenMenus(EngagementSlot _slot, GamepadUser _prevUser, GamepadUser _newUser)
	{
		if (_slot == EngagementSlot.One && _prevUser == null && _newUser != null)
		{
			if (this.m_PlayerLobby != null)
			{
				this.m_PlayerLobby.Hide(true, false);
				this.m_PlayerLobby.Show(_newUser, null, null, true);
			}
			if (this.m_Rootmenu != null)
			{
				this.m_Rootmenu.Hide(true, false);
				this.m_Rootmenu.Show(_newUser, null, null, true);
			}
			if (this.ClientCountdownRunning)
			{
				this.m_Rootmenu.OpenFrontendMenu(this.m_OnlineClientSaveDialog);
			}
			this.m_IPlayerManager.EngagementChangeCallback -= this.OnEngagementChangedReopenMenus;
		}
	}

	// Token: 0x060024C5 RID: 9413 RVA: 0x000AE2E4 File Offset: 0x000AC6E4
	private void OnServerGameStateChanged(GameState state, GameStateMessage.GameStatePayload payload)
	{
		if (state == GameState.SelectCampaignMapSave)
		{
			this.StartServerCountdown();
		}
	}

	// Token: 0x060024C6 RID: 9414 RVA: 0x000AE2F4 File Offset: 0x000AC6F4
	protected void OnServerReceivedClientGameState(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = message as GameStateMessage;
		if (gameStateMessage.m_State == GameState.LoadedCampaignMapSave)
		{
			this.ServerTryLoadCampaign();
		}
	}

	// Token: 0x060024C7 RID: 9415 RVA: 0x000AE31C File Offset: 0x000AC71C
	private void ServerTryLoadCampaign()
	{
		if (this.m_bServerCountdownRunning)
		{
			FastList<User> users = ServerUserSystem.m_Users;
			User.MachineID s_LocalMachineId = ServerUserSystem.s_LocalMachineId;
			if (UserSystemUtils.FindUser(users, null, s_LocalMachineId, EngagementSlot.One, TeamID.Count, User.SplitStatus.Count) == null)
			{
				return;
			}
			if (!ConnectionStatus.IsInSession() || (ConnectionStatus.IsHost() && UserSystemUtils.AreAllUsersInGameState(ServerUserSystem.m_Users, GameState.LoadedCampaignMapSave)))
			{
				this.ServerLoadCampaign();
			}
		}
	}

	// Token: 0x060024C8 RID: 9416 RVA: 0x000AE37E File Offset: 0x000AC77E
	private void ServerLoadCampaign()
	{
		this.StopServerCountdown();
		this.m_saveDialog.LoadReadySlot();
	}

	// Token: 0x060024C9 RID: 9417 RVA: 0x000AE392 File Offset: 0x000AC792
	private void OnUserRemoved(User removed)
	{
		this.ServerTryLoadCampaign();
	}

	// Token: 0x060024CA RID: 9418 RVA: 0x000AE39C File Offset: 0x000AC79C
	private void StartServerCountdown()
	{
		if (!this.m_bRegisteredForGameStateMessage)
		{
			this.ServerCountdown = this.m_ServerCountdownDuration;
			this.ClientCountdown = 60f;
			ServerUserSystem.OnUserRemoved = (GenericVoid<User>)Delegate.Combine(ServerUserSystem.OnUserRemoved, new GenericVoid<User>(this.OnUserRemoved));
			Mailbox.Server.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnServerReceivedClientGameState));
			this.m_bRegisteredForGameStateMessage = true;
			this.m_bServerCountdownRunning = true;
		}
	}

	// Token: 0x060024CB RID: 9419 RVA: 0x000AE414 File Offset: 0x000AC814
	private void StopServerCountdown()
	{
		if (this.m_bRegisteredForGameStateMessage)
		{
			ServerUserSystem.OnUserRemoved = (GenericVoid<User>)Delegate.Remove(ServerUserSystem.OnUserRemoved, new GenericVoid<User>(this.OnUserRemoved));
			Mailbox.Server.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnServerReceivedClientGameState));
			this.m_bRegisteredForGameStateMessage = false;
			this.m_bServerCountdownRunning = false;
		}
	}

	// Token: 0x060024CC RID: 9420 RVA: 0x000AE472 File Offset: 0x000AC872
	public void StartClientCountdown()
	{
		this.m_bShouldClientCountdown = true;
	}

	// Token: 0x060024CD RID: 9421 RVA: 0x000AE47C File Offset: 0x000AC87C
	public void ShowWaitingForPlayers()
	{
		this.m_saveDialog.Hide(true, false);
		this.m_Rootmenu.OpenFrontendMenu(this.m_saveWaitDialog);
		if (this.m_ClientSaveWaitSuppressor == null)
		{
			this.m_ClientSaveWaitSuppressor = this.m_eventSystem.Disable(this.m_saveWaitDialog);
		}
	}

	// Token: 0x060024CE RID: 9422 RVA: 0x000AE4CC File Offset: 0x000AC8CC
	public void HideWaitingForPlayers()
	{
		if (this.m_bServerCountdownRunning)
		{
			this.StopServerCountdown();
		}
		this.m_saveWaitDialog.Hide(true, false);
		if (this.m_ClientSaveWaitSuppressor != null)
		{
			this.m_eventSystem.ReleaseSuppressor(this.m_ClientSaveWaitSuppressor);
			this.m_ClientSaveWaitSuppressor = null;
		}
	}

	// Token: 0x060024CF RID: 9423 RVA: 0x000AE51C File Offset: 0x000AC91C
	public void PromptGameExit()
	{
		if (T17FrontendFlow.Instance.IsCameraTransitioning() || (this.m_PlayerLobby != null && this.m_PlayerLobby.IsFocused))
		{
			if (!this.m_bBlockFocusKitchen && this.m_PlayerLobby != null && this.m_PlayerLobby.IsFocused && !this.m_PlayerLobby.m_slotMenuClosedThisFrame)
			{
				this.FocusOnMainMenu();
			}
			return;
		}
		T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
		if (dialog != null)
		{
			dialog.Initialize("Text.Menu.QuitTitle", "Text.Menu.QuitBody", "Text.Button.Quit", "Text.Button.Cancel", string.Empty, T17DialogBox.Symbols.Warning, true, true, false);
			T17DialogBox t17DialogBox = dialog;
			t17DialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox.OnConfirm, new T17DialogBox.DialogEvent(this.DoExit));
			dialog.Show();
		}
	}

	// Token: 0x060024D0 RID: 9424 RVA: 0x000AE5FA File Offset: 0x000AC9FA
	private void DoExit()
	{
		Application.Quit();
	}

	// Token: 0x04001C45 RID: 7237
	private static T17FrontendFlow s_Instance;

	// Token: 0x04001C46 RID: 7238
	[Header("Game Sessions")]
	[SerializeField]
	private T17FrontendFlow.DLCSerializedGameSessions m_CoopGameSessionPrefabs = new T17FrontendFlow.DLCSerializedGameSessions();

	// Token: 0x04001C47 RID: 7239
	[SerializeField]
	private T17FrontendFlow.DLCSerializedGameSessions m_CompetitiveGameSessionPrefabs = new T17FrontendFlow.DLCSerializedGameSessions();

	// Token: 0x04001C48 RID: 7240
	[Header("Scene References")]
	[SerializeField]
	private Animator m_CameraAnimator;

	// Token: 0x04001C49 RID: 7241
	public FrontendRootMenu m_Rootmenu;

	// Token: 0x04001C4A RID: 7242
	public FrontendPlayerLobby m_PlayerLobby;

	// Token: 0x04001C4B RID: 7243
	public FrontendPlayerLobby m_PlayerLobbySwitch;

	// Token: 0x04001C4C RID: 7244
	[SerializeField]
	private GameObject m_FocusKitchenImage;

	// Token: 0x04001C4D RID: 7245
	[Space]
	[SerializeField]
	private SelectSaveDialog m_OnlineClientSaveDialog;

	// Token: 0x04001C4E RID: 7246
	[SerializeField]
	private FrontendMenuBehaviour m_saveWaitDialog;

	// Token: 0x04001C4F RID: 7247
	private const float c_ClientVisibleCountdownDuration = 60f;

	// Token: 0x04001C50 RID: 7248
	private const float c_LoadSaveSlotsAllowanceCountdownDuration = 5f;

	// Token: 0x04001C51 RID: 7249
	private const float c_LoadSaveGameAllowanceCountdownDuration = 5f;

	// Token: 0x04001C53 RID: 7251
	private float m_ServerCountdownDuration;

	// Token: 0x04001C54 RID: 7252
	private bool m_bServerCountdownRunning;

	// Token: 0x04001C55 RID: 7253
	private bool m_bRegisteredForGameStateMessage;

	// Token: 0x04001C57 RID: 7255
	private bool m_bShouldClientCountdown;

	// Token: 0x04001C58 RID: 7256
	private SelectSaveDialog m_saveDialog;

	// Token: 0x04001C59 RID: 7257
	private Suppressor m_ClientSaveWaitSuppressor;

	// Token: 0x04001C5A RID: 7258
	private IPlayerManager m_IPlayerManager;

	// Token: 0x04001C5B RID: 7259
	private T17EventSystem m_eventSystem;

	// Token: 0x04001C5C RID: 7260
	private GamepadEngagementManager m_gamepadEngagementManager;

	// Token: 0x04001C5D RID: 7261
	private ILogicalButton m_ToggleMultichefMenu;

	// Token: 0x04001C5E RID: 7262
	private ILogicalButton m_UnfocusMultichefMenu;

	// Token: 0x04001C5F RID: 7263
	private ILogicalButton m_quitButton;

	// Token: 0x04001C60 RID: 7264
	private bool m_allowMultichefMenu = true;

	// Token: 0x04001C61 RID: 7265
	private bool m_autoOpenChefSelectionMenu;

	// Token: 0x04001C63 RID: 7267
	private T17FrontendFlow.CameraState m_PostTransitionState = T17FrontendFlow.CameraState.ePulledBack;

	// Token: 0x04001C64 RID: 7268
	private T17FrontendFlow.CameraState m_CurrentCameraState;

	// Token: 0x04001C65 RID: 7269
	private T17FrontendFlow.CameraState m_PendingCameraState = T17FrontendFlow.CameraState.ePulledBack;

	// Token: 0x04001C66 RID: 7270
	private bool m_bBlockFocusKitchen;

	// Token: 0x04001C67 RID: 7271
	private GameStateMessage.ClientSavePayload m_ClientSavePayload;

	// Token: 0x02000772 RID: 1906
	[Serializable]
	private class DLCSerializedGameSessions : DLCSerializedData<GameSession>
	{
	}

	// Token: 0x02000773 RID: 1907
	public enum CameraState
	{
		// Token: 0x04001C6A RID: 7274
		ePushedForward,
		// Token: 0x04001C6B RID: 7275
		ePulledBack,
		// Token: 0x04001C6C RID: 7276
		eFocusedOnKitchen,
		// Token: 0x04001C6D RID: 7277
		eUnFocusedOnKitchen,
		// Token: 0x04001C6E RID: 7278
		eTransitioning
	}
}
