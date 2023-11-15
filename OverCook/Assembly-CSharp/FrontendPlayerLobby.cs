using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000ABB RID: 2747
public class FrontendPlayerLobby : FrontendMenuBehaviour
{
	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x060036B3 RID: 14003 RVA: 0x00100585 File Offset: 0x000FE985
	public bool IsFocused
	{
		get
		{
			return this.m_bIsFocussed;
		}
	}

	// Token: 0x170003C4 RID: 964
	// (get) Token: 0x060036B4 RID: 14004 RVA: 0x0010058D File Offset: 0x000FE98D
	public bool IsPlayerSlotMenuOpen
	{
		get
		{
			return this.m_isPlayerSlotMenuOpen;
		}
	}

	// Token: 0x060036B5 RID: 14005 RVA: 0x00100595 File Offset: 0x000FE995
	public bool IsKitchenConnectionTaskRunning()
	{
		return (this.m_switchConnectionModeTask != null && this.m_switchConnectionModeTask.isRunning) || (null != this.m_switchPlayWithFriendsMenu && this.m_switchPlayWithFriendsMenu.IsBusy());
	}

	// Token: 0x060036B6 RID: 14006 RVA: 0x001005D4 File Offset: 0x000FE9D4
	public void SetupNetworking()
	{
		if (this.m_NetworkErrorDialog == null)
		{
			this.m_NetworkErrorDialog = new NetworkErrorDialog();
			this.m_NetworkErrorDialog.Enable(new T17DialogBox.DialogEvent(this.OnNetworkErrorDismissed));
			InviteMonitor.InviteJoinComplete = (GenericVoid)Delegate.Combine(InviteMonitor.InviteJoinComplete, new GenericVoid(this.OnInviteJoinComplete));
		}
	}

	// Token: 0x060036B7 RID: 14007 RVA: 0x0010062E File Offset: 0x000FEA2E
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
	}

	// Token: 0x060036B8 RID: 14008 RVA: 0x00100638 File Offset: 0x000FEA38
	protected override void Start()
	{
		base.Start();
		this.m_IPlayerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
		if (this.m_PlayerSlots == null || this.m_PlayerSlots.Count == 0)
		{
			this.m_PlayerSlots = new List<FrontendPlayerSlot>(base.GetComponentsInChildren<FrontendPlayerSlot>(true));
		}
		for (int i = 0; i < this.m_PlayerSlots.Count; i++)
		{
			if (this.m_PlayerSlots[i] != null && this.m_PlayerSlots[i].m_SlotButton != null)
			{
				int index = i;
				FrontendPlayerSlot slot = this.m_PlayerSlots[i];
				slot.m_SlotButton.onClick.AddListener(delegate()
				{
					this.OpenOptionsMenuForPlayerslot(index);
				});
				T17Button slotButton = slot.m_SlotButton;
				slotButton.OnButtonSelect = (T17Button.T17ButtonDelegate)Delegate.Combine(slotButton.OnButtonSelect, new T17Button.T17ButtonDelegate(delegate(T17Button A_1)
				{
					this.CloseOptionsMenu();
				}));
				T17Button slotButton2 = slot.m_SlotButton;
				slotButton2.OnButtonMove = (T17Button.T17ButtonMoveDelegate)Delegate.Combine(slotButton2.OnButtonMove, new T17Button.T17ButtonMoveDelegate(delegate(Selectable _from, Selectable _to, MoveDirection _direction)
				{
					if (_to != null && _to.gameObject.IsInHierarchyOf(this.m_OptionsParent))
					{
						this.m_CurrentExpandedSlot = slot;
						this.UpdateButtonNavigation();
					}
				}));
			}
		}
		if (this.m_GamertagButton != null)
		{
			this.m_GamertagButtonText = this.m_GamertagButton.GetComponentInChildren<T17Text>();
		}
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChangedEvent));
		ClientUserSystem.userAdded = (GenericVoid<uint, UserData>)Delegate.Combine(ClientUserSystem.userAdded, new GenericVoid<uint, UserData>(this.OnUserAdded));
		this.m_IPlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
		this.m_sendInviteTask = new KitchenSendInviteTask();
		this.m_sendInviteTask.onComplete += this.OnSendInviteTaskComplete;
		this.m_switchConnectionModeTask = new KitchenSwitchConnectionModeTask();
		this.m_switchConnectionModeTask.onComplete += this.OnSwitchConnectionModeComplete;
		KitchenSwitchConnectionModeTask switchConnectionModeTask = this.m_switchConnectionModeTask;
		switchConnectionModeTask.onResults = (KitchenSwitchConnectionModeTask.OnResults)Delegate.Combine(switchConnectionModeTask.onResults, new KitchenSwitchConnectionModeTask.OnResults(this.OnResults));
		ServerUserSystem.OnEngagementPrivilegeCheckStarted = (GenericVoid<IConnectionModeSwitchStatus>)Delegate.Combine(ServerUserSystem.OnEngagementPrivilegeCheckStarted, new GenericVoid<IConnectionModeSwitchStatus>(this.EngagementPrivilegeCheckStarted));
		ServerUserSystem.OnEngagementPrivilegeCheckCompleted = (GenericVoid<IConnectionModeSwitchStatus>)Delegate.Combine(ServerUserSystem.OnEngagementPrivilegeCheckCompleted, new GenericVoid<IConnectionModeSwitchStatus>(this.EngagementPrivilegeCheckComplete));
		this.ShowIdleMenu();
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			for (int j = 0; j < ServerUserSystem.m_Users.Count; j++)
			{
				User user = ServerUserSystem.m_Users._items[j];
				user.Colour = (uint)j;
			}
		}
		this.m_animSets.ShuffleContents<FrontendChef.AnimationSet>();
		NetworkUtils.SelectRandomAvatar();
		this.RecalculateChefAvatars(true);
		this.UpdatePromptPlayerCount();
		this.SetMouseBlockActive(false);
		DisconnectionHandler.SessionConnectionLostEvent = (GenericVoid)Delegate.Combine(DisconnectionHandler.SessionConnectionLostEvent, new GenericVoid(this.OnSessionConnectionLost));
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Combine(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Combine(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnKickedFromSession));
		DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Combine(DisconnectionHandler.LocalDisconnectionEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(this.OnLocalDisconnection));
	}

	// Token: 0x060036B9 RID: 14009 RVA: 0x00100984 File Offset: 0x000FED84
	protected override void OnDestroy()
	{
		base.OnDestroy();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.OnUsersChangedEvent));
		ClientUserSystem.userAdded = (GenericVoid<uint, UserData>)Delegate.Remove(ClientUserSystem.userAdded, new GenericVoid<uint, UserData>(this.OnUserAdded));
		if (this.m_IPlayerManager != null)
		{
			this.m_IPlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
		}
		if (this.m_sendInviteTask != null)
		{
			this.m_sendInviteTask.CleanUp();
		}
		if (this.m_switchConnectionModeTask != null)
		{
			this.m_switchConnectionModeTask.CleanUp();
			this.m_switchConnectionModeTask.onComplete -= this.OnSwitchConnectionModeComplete;
			KitchenSwitchConnectionModeTask switchConnectionModeTask = this.m_switchConnectionModeTask;
			switchConnectionModeTask.onResults = (KitchenSwitchConnectionModeTask.OnResults)Delegate.Remove(switchConnectionModeTask.onResults, new KitchenSwitchConnectionModeTask.OnResults(this.OnResults));
		}
		ServerUserSystem.OnEngagementPrivilegeCheckStarted = (GenericVoid<IConnectionModeSwitchStatus>)Delegate.Remove(ServerUserSystem.OnEngagementPrivilegeCheckStarted, new GenericVoid<IConnectionModeSwitchStatus>(this.EngagementPrivilegeCheckStarted));
		ServerUserSystem.OnEngagementPrivilegeCheckCompleted = (GenericVoid<IConnectionModeSwitchStatus>)Delegate.Remove(ServerUserSystem.OnEngagementPrivilegeCheckCompleted, new GenericVoid<IConnectionModeSwitchStatus>(this.EngagementPrivilegeCheckComplete));
		ConnectionModeSwitcher.InvalidateCallback(new GenericVoid<IConnectionModeSwitchStatus>(this.OnLeaveKitchenComplete));
		ConnectionModeSwitcher.InvalidateCallback(new GenericVoid<IConnectionModeSwitchStatus>(this.OnLeaveOnlineModeForPadSplitComplete));
		if (this.m_NetworkErrorDialog != null)
		{
			InviteMonitor.InviteJoinComplete = (GenericVoid)Delegate.Remove(InviteMonitor.InviteJoinComplete, new GenericVoid(this.OnInviteJoinComplete));
			this.m_NetworkErrorDialog.Disable();
			this.m_NetworkErrorDialog = null;
		}
		DisconnectionHandler.SessionConnectionLostEvent = (GenericVoid)Delegate.Remove(DisconnectionHandler.SessionConnectionLostEvent, new GenericVoid(this.OnSessionConnectionLost));
		DisconnectionHandler.ConnectionModeErrorEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>)Delegate.Remove(DisconnectionHandler.ConnectionModeErrorEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult>>(this.OnConnectionModeError));
		DisconnectionHandler.KickedFromSessionEvent = (GenericVoid)Delegate.Remove(DisconnectionHandler.KickedFromSessionEvent, new GenericVoid(this.OnKickedFromSession));
		DisconnectionHandler.LocalDisconnectionEvent = (GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>)Delegate.Remove(DisconnectionHandler.LocalDisconnectionEvent, new GenericVoid<OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult>>(this.OnLocalDisconnection));
	}

	// Token: 0x060036BA RID: 14010 RVA: 0x00100B82 File Offset: 0x000FEF82
	private void OnSessionConnectionLost()
	{
		this.DisableLeaveKitchenButton();
	}

	// Token: 0x060036BB RID: 14011 RVA: 0x00100B8A File Offset: 0x000FEF8A
	private void OnConnectionModeError(OnlineMultiplayerReturnCode<OnlineMultiplayerConnectionModeErrorResult> _error)
	{
		this.DisableLeaveKitchenButton();
	}

	// Token: 0x060036BC RID: 14012 RVA: 0x00100B92 File Offset: 0x000FEF92
	private void OnKickedFromSession()
	{
		this.DisableLeaveKitchenButton();
	}

	// Token: 0x060036BD RID: 14013 RVA: 0x00100B9A File Offset: 0x000FEF9A
	private void OnLocalDisconnection(OnlineMultiplayerReturnCode<OnlineMultiplayerSessionDisconnectionResult> _error)
	{
		this.DisableLeaveKitchenButton();
	}

	// Token: 0x060036BE RID: 14014 RVA: 0x00100BA4 File Offset: 0x000FEFA4
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		bool result = base.Show(currentGamer, parent, invoker, hideInvoker);
		PlayerInputLookup.ResetToDefaultInputConfig();
		this.UpdateTooltipDefaults(true);
		this.ShowIdleMenu();
		return result;
	}

	// Token: 0x060036BF RID: 14015 RVA: 0x00100BD0 File Offset: 0x000FEFD0
	public void RefreshSearch()
	{
		this.AdhocSearch();
	}

	// Token: 0x060036C0 RID: 14016 RVA: 0x00100BD8 File Offset: 0x000FEFD8
	private void OnUserAdded(uint _idx, UserData _userData)
	{
		this.TriggerUserJoinEffect((int)_idx);
	}

	// Token: 0x060036C1 RID: 14017 RVA: 0x00100BE4 File Offset: 0x000FEFE4
	private void TriggerUserJoinEffect(int _userIdx)
	{
		FrontendChef frontendChef = this.m_PlayerChefs[_userIdx];
		if (frontendChef != null)
		{
			if (this.m_playerJoinPFXPrefab != null)
			{
				this.m_playerJoinPFXPrefab.InstantiatePFX(frontendChef.transform);
			}
			if (this.m_playerJoinAudioTag != GameOneShotAudioTag.COUNT)
			{
				GameUtils.TriggerAudio(this.m_playerJoinAudioTag, frontendChef.gameObject.layer);
			}
		}
	}

	// Token: 0x060036C2 RID: 14018 RVA: 0x00100C50 File Offset: 0x000FF050
	private void OnUsersChangedEvent()
	{
		this.RecalculateChefAvatars(false);
		if (this.m_CurrentExpandedSlot != null)
		{
			int engagementSlot = (int)this.m_CurrentExpandedSlot.m_EngagementSlot;
			bool flag = this.GetClientUserForCurrentSlot() != null;
			if (this.m_currentSlotHasUser != flag || engagementSlot > ClientUserSystem.m_Users.Count)
			{
				this.CloseOptionsMenu();
			}
		}
		if (this.m_CachedEventSystem != null && this.m_CachedEventSystem.currentSelectedGameObject == null)
		{
			GameObject lastRequestedSelectedGameobject = this.m_CachedEventSystem.GetLastRequestedSelectedGameobject();
			for (int i = ClientUserSystem.m_Users.Count; i < this.m_PlayerSlots.Count; i++)
			{
				FrontendPlayerSlot frontendPlayerSlot = this.m_PlayerSlots[i];
				if (frontendPlayerSlot.m_SlotButton.gameObject == lastRequestedSelectedGameobject && ClientUserSystem.m_Users.Count < this.m_PlayerSlots.Count)
				{
					GameObject gameObject = this.m_PlayerSlots[ClientUserSystem.m_Users.Count].m_SlotButton.gameObject;
					this.m_CachedEventSystem.SetSelectedGameObject(gameObject);
				}
			}
		}
		bool bIsHeadChef = this.m_bIsHeadChef;
		bool flag2 = false;
		if (this.m_chalkboardInactivePrompt != null)
		{
			flag2 = this.m_chalkboardInactivePrompt.gameObject.activeInHierarchy;
		}
		this.m_bIsHeadChef = (!ConnectionStatus.IsInSession() || ConnectionStatus.IsHost());
		if (ClientUserSystem.m_Users.Count != 4)
		{
			if (this.m_bIsHeadChef && !bIsHeadChef && flag2)
			{
				if (this.m_chalkboardInactivePrompt != null)
				{
					this.m_chalkboardInactivePrompt.gameObject.SetActive(true);
				}
			}
			else if (!this.m_bIsHeadChef && bIsHeadChef && flag2 && this.m_chalkboardInactivePrompt != null)
			{
				this.m_chalkboardInactivePrompt.gameObject.SetActive(false);
			}
		}
		this.UpdatePromptPlayerCount();
		this.UpdateCurrentConnectionMode();
		this.UpdateTooltipDefaults(true);
	}

	// Token: 0x060036C3 RID: 14019 RVA: 0x00100E5C File Offset: 0x000FF25C
	private void RecalculateChefAvatars(bool _force = false)
	{
		if (this.m_PlayerChefs != null)
		{
			for (int i = 0; i < this.m_PlayerChefs.Length; i++)
			{
				FrontendChef y = this.m_PlayerChefs[i];
				if (null != y)
				{
					if (i >= 0 && i < ClientUserSystem.m_Users.Count)
					{
						User user = ClientUserSystem.m_Users._items[i];
						GameSession.SelectedChefData selectedChefData = user.SelectedChefData;
						if (selectedChefData != null)
						{
							this.m_PlayerChefs[i].SetChefData(selectedChefData, _force);
						}
						this.m_PlayerChefs[i].SetChefHat(this.m_ChefHat);
						this.m_PlayerChefs[i].SetAnimationSet(this.m_animSets[i]);
						this.m_PlayerChefs[i].gameObject.SetActive(true);
					}
					else
					{
						this.m_PlayerChefs[i].gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x060036C4 RID: 14020 RVA: 0x00100F35 File Offset: 0x000FF335
	private void OnEngagementChanged(EngagementSlot _slot, GamepadUser _prevUser, GamepadUser _newUser)
	{
	}

	// Token: 0x060036C5 RID: 14021 RVA: 0x00100F37 File Offset: 0x000FF337
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		this.HideAllOptionsButtons();
		return true;
	}

	// Token: 0x060036C6 RID: 14022 RVA: 0x00100F50 File Offset: 0x000FF350
	protected override void Update()
	{
		base.Update();
		if (this.m_sendInviteTask.isRunning)
		{
			this.m_sendInviteTask.Update();
		}
		if (this.m_switchConnectionModeTask.isRunning)
		{
			this.m_switchConnectionModeTask.Update();
		}
		if (this.m_progressBox != null)
		{
			string strMessage = string.Empty;
			FrontendPlayerLobby.DisplayProgressInfo displayProgressInformation = this.m_displayProgressInformation;
			if (displayProgressInformation != FrontendPlayerLobby.DisplayProgressInfo.ConnectionModeSwitch)
			{
				if (displayProgressInformation != FrontendPlayerLobby.DisplayProgressInfo.DropInPlayer)
				{
					if (displayProgressInformation != FrontendPlayerLobby.DisplayProgressInfo.None)
					{
					}
				}
				else
				{
					strMessage = ServerUserSystem.GetEngagementPrivilegeCheckStatus().GetLocalisedProgressDescription();
				}
			}
			else
			{
				strMessage = ConnectionModeSwitcher.GetStatus().GetLocalisedProgressDescription();
			}
			this.m_progressBox.SetMessage(strMessage, false);
		}
	}

	// Token: 0x060036C7 RID: 14023 RVA: 0x00101002 File Offset: 0x000FF402
	private void LateUpdate()
	{
		if (this.m_slotMenuClosedThisFrame)
		{
			this.m_isPlayerSlotMenuOpen = false;
			this.m_slotMenuClosedThisFrame = false;
		}
	}

	// Token: 0x060036C8 RID: 14024 RVA: 0x0010101D File Offset: 0x000FF41D
	private void ShowIdleMenu()
	{
		this.HideAllOptionsButtons();
		if (ConnectionStatus.IsInSession() && !ConnectionStatus.IsHost())
		{
			this.m_OptionsParent.SetActive(true);
			this.EnableLeaveKitchenButton();
		}
		this.UpdateButtonNavigation();
		this.UpdateTooltipDefaults(true);
	}

	// Token: 0x060036C9 RID: 14025 RVA: 0x00101058 File Offset: 0x000FF458
	private void OpenOptionsMenuForPlayerslot(int playerslotIndex)
	{
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.FocusOnMultiplayerKitchen(false);
		}
		FrontendPlayerSlot frontendPlayerSlot = this.m_PlayerSlots[playerslotIndex];
		if (this.m_OptionsParent != null)
		{
			if (this.m_OptionsParent.activeSelf && this.m_CurrentExpandedSlot == frontendPlayerSlot)
			{
				this.HideAllOptionsButtons();
				this.ShowIdleMenu();
			}
			else
			{
				this.HideAllOptionsButtons();
				this.HidePrompts();
				this.m_CurrentExpandedSlot = frontendPlayerSlot;
				if (!ConnectionStatus.IsInSession() || ConnectionStatus.IsHost())
				{
					this.ConfigureMenuForHeadChef();
				}
				else
				{
					this.ConfigureMenuForSecondaryPlayer();
				}
				this.UpdateButtonNavigation();
				if (this.m_CurrentExpandedSlot != null)
				{
					this.m_CurrentExpandedSlot.SelectSlot();
				}
				this.m_currentSlotHasUser = (this.GetClientUserForCurrentSlot() != null);
				this.m_isPlayerSlotMenuOpen = true;
				this.m_slotMenuClosedThisFrame = false;
			}
		}
	}

	// Token: 0x060036CA RID: 14026 RVA: 0x0010114C File Offset: 0x000FF54C
	private FrontendPlayerSlot GetSlotToReturnTo()
	{
		if (this.m_CurrentExpandedSlot != null && this.m_CurrentExpandedSlot.m_SlotButton != null)
		{
			int engagementSlot = (int)this.m_CurrentExpandedSlot.m_EngagementSlot;
			if (engagementSlot <= ClientUserSystem.m_Users.Count)
			{
				return this.m_CurrentExpandedSlot;
			}
			if (ClientUserSystem.m_Users.Count <= this.m_PlayerSlots.Count)
			{
				return this.m_PlayerSlots[ClientUserSystem.m_Users.Count];
			}
		}
		return null;
	}

	// Token: 0x060036CB RID: 14027 RVA: 0x001011DC File Offset: 0x000FF5DC
	public void CloseOptionsMenu()
	{
		FrontendPlayerSlot slotToReturnTo = this.GetSlotToReturnTo();
		if (slotToReturnTo != null)
		{
			this.m_CachedEventSystem.SetSelectedGameObject(slotToReturnTo.m_SlotButton.gameObject);
		}
		this.ShowIdleMenu();
		this.ShowCorrectPrompt();
	}

	// Token: 0x060036CC RID: 14028 RVA: 0x00101220 File Offset: 0x000FF620
	private void ConfigureMenuForHeadChef()
	{
		User clientUserForCurrentSlot = this.GetClientUserForCurrentSlot();
		GamepadUser user = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		this.m_OptionsParent.SetActive(true);
		if (clientUserForCurrentSlot == null)
		{
			this.EnableInviteButton();
			if (this.CanAddSplitPadGuest())
			{
				this.EnableSplitPadButton();
			}
			this.FocusKitchenMenuButton(this.m_InviteButton);
		}
		else if (clientUserForCurrentSlot.GamepadUser != null && clientUserForCurrentSlot.GamepadUser == user && clientUserForCurrentSlot.Split != User.SplitStatus.SplitPadGuest)
		{
			this.EnableGamertagButton(clientUserForCurrentSlot.DisplayName);
			this.EnableControllerOptionsButton(clientUserForCurrentSlot);
			this.EnableChangeProfileButton();
			this.FocusKitchenMenuButton(this.m_GamertagButton);
		}
		else if (!clientUserForCurrentSlot.IsLocal)
		{
			this.EnableGamertagButton(clientUserForCurrentSlot.DisplayName);
			this.EnableKickButton();
			this.FocusKitchenMenuButton(this.m_GamertagButton);
		}
		else if (clientUserForCurrentSlot.Split == User.SplitStatus.SplitPadGuest)
		{
			this.EnableRemoveButton();
			this.FocusKitchenMenuButton(this.m_RemoveButton);
		}
		else
		{
			this.EnableControllerOptionsButton(clientUserForCurrentSlot);
			this.EnableRemoveButton();
			this.FocusKitchenMenuButton(this.m_ControllerLeftSideButton.gameObject);
		}
	}

	// Token: 0x060036CD RID: 14029 RVA: 0x00101344 File Offset: 0x000FF744
	private void ConfigureMenuForSecondaryPlayer()
	{
		User clientUserForCurrentSlot = this.GetClientUserForCurrentSlot();
		this.m_OptionsParent.SetActive(true);
		if (clientUserForCurrentSlot != null)
		{
			if (clientUserForCurrentSlot.IsLocal)
			{
				this.EnableGamertagButton(clientUserForCurrentSlot.DisplayName);
				this.EnableControllerOptionsButton(clientUserForCurrentSlot);
				this.EnableLeaveKitchenButton();
			}
			else
			{
				this.EnableGamertagButton(clientUserForCurrentSlot.DisplayName);
			}
			this.UpdateButtonNavigation();
			this.FocusKitchenMenuButton(this.m_GamertagButton);
		}
	}

	// Token: 0x060036CE RID: 14030 RVA: 0x001013B4 File Offset: 0x000FF7B4
	private void ConfigureMenuForSwitch()
	{
		this.m_OptionsParent.SetActive(true);
		this.UpdateCurrentConnectionMode();
		this.EnableModeButton();
		if (this.m_currentOnlineMode == OnlineMultiplayerConnectionMode.eAdhoc)
		{
			this.EnableFriendsButton(false);
		}
		else
		{
			this.EnableFriendsButton(true);
		}
		if (this.m_HostingIndicator != null)
		{
			bool active = this.m_currentOnlineMode == OnlineMultiplayerConnectionMode.eAdhoc && ConnectionStatus.IsHost();
			this.m_HostingIndicator.SetActive(active);
		}
	}

	// Token: 0x060036CF RID: 14031 RVA: 0x0010142C File Offset: 0x000FF82C
	private void EnableControllerOptionsButton(User slotUser)
	{
		if (this.m_ControllerOptions == null || this.m_ControllerLeftSideButton == null || this.m_ControllerRightSideButton == null || this.m_ControllerFullButton == null || slotUser == null)
		{
			return;
		}
		this.m_ControllerOptions.SetActive(true);
		this.m_ControllerLeftSideButton.gameObject.SetActive(true);
		if (this.m_ControllerLeftSideImage != null)
		{
			this.m_ControllerLeftSideImage.sprite = this.m_ControllerSprites.GetEngagementImage(PadSide.Left, slotUser.GamepadUser.ControlType);
		}
		this.m_ControllerRightSideButton.gameObject.SetActive(true);
		if (this.m_ControllerRightSideImage != null)
		{
			this.m_ControllerRightSideImage.sprite = this.m_ControllerSprites.GetEngagementImage(PadSide.Right, slotUser.GamepadUser.ControlType);
		}
		this.m_ControllerFullButton.gameObject.SetActive(slotUser.Split == User.SplitStatus.NotSplit);
		if (this.m_ControllerFullImage != null)
		{
			this.m_ControllerFullImage.sprite = this.m_ControllerSprites.GetEngagementImage(PadSide.Both, slotUser.GamepadUser.ControlType);
		}
	}

	// Token: 0x060036D0 RID: 14032 RVA: 0x00101564 File Offset: 0x000FF964
	private void EnableGamertagButton(string gamerTag)
	{
		if (this.m_GamertagButton != null)
		{
			this.m_GamertagButton.SetActive(true);
			if (this.m_GamertagButtonText != null)
			{
				this.m_GamertagButtonText.text = gamerTag;
			}
		}
	}

	// Token: 0x060036D1 RID: 14033 RVA: 0x001015A0 File Offset: 0x000FF9A0
	private void EnableInviteButton()
	{
		if (this.m_InviteButton != null)
		{
			this.m_InviteButton.SetActive(true);
		}
	}

	// Token: 0x060036D2 RID: 14034 RVA: 0x001015BF File Offset: 0x000FF9BF
	private void EnableSplitPadButton()
	{
		if (this.m_SplitPadButton != null)
		{
			this.m_SplitPadButton.SetActive(true);
		}
	}

	// Token: 0x060036D3 RID: 14035 RVA: 0x001015DE File Offset: 0x000FF9DE
	private void EnableChangeProfileButton()
	{
	}

	// Token: 0x060036D4 RID: 14036 RVA: 0x001015E0 File Offset: 0x000FF9E0
	private void EnableModeButton()
	{
		this.m_selectedOnlineMode = this.m_currentOnlineMode;
		if (this.m_ModeButtonGroup != null)
		{
			this.m_ModeButtonGroup.SetActive(true);
			this.UpdateSwitchOnlineModeText();
		}
	}

	// Token: 0x060036D5 RID: 14037 RVA: 0x00101614 File Offset: 0x000FFA14
	private void EnableFriendsButton(bool bEnabled)
	{
		if (this.m_FriendsButton != null)
		{
			this.m_FriendsButton.SetActive(bEnabled);
			RectTransform rectTransform = (RectTransform)this.m_OptionsParent.transform;
			if (rectTransform != null)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
			}
		}
	}

	// Token: 0x060036D6 RID: 14038 RVA: 0x00101661 File Offset: 0x000FFA61
	private void EnableSearchButton(bool bEnabled)
	{
		if (this.m_SearchButton != null)
		{
			this.m_SearchButton.SetActive(bEnabled);
		}
	}

	// Token: 0x060036D7 RID: 14039 RVA: 0x00101680 File Offset: 0x000FFA80
	private void EnableKickButton()
	{
		if (this.m_KickButton != null)
		{
			this.m_KickButton.SetActive(true);
		}
	}

	// Token: 0x060036D8 RID: 14040 RVA: 0x0010169F File Offset: 0x000FFA9F
	private void EnableRemoveButton()
	{
		if (this.m_RemoveButton != null)
		{
			this.m_RemoveButton.SetActive(true);
		}
	}

	// Token: 0x060036D9 RID: 14041 RVA: 0x001016BE File Offset: 0x000FFABE
	private void EnableLeaveKitchenButton()
	{
		if (this.m_LeaveKitchenButton != null)
		{
			this.m_LeaveKitchenButton.SetActive(true);
		}
	}

	// Token: 0x060036DA RID: 14042 RVA: 0x001016DD File Offset: 0x000FFADD
	private void DisableLeaveKitchenButton()
	{
		if (this.m_LeaveKitchenButton != null)
		{
			this.m_LeaveKitchenButton.SetActive(false);
		}
	}

	// Token: 0x060036DB RID: 14043 RVA: 0x001016FC File Offset: 0x000FFAFC
	private void HideAllOptionsButtons()
	{
		if (this.m_CurrentExpandedSlot != null)
		{
			this.m_CurrentExpandedSlot.DeselectSlot();
		}
		this.m_CurrentExpandedSlot = null;
		this.m_slotMenuClosedThisFrame = true;
		this.DisableLeaveKitchenButton();
		if (this.m_ControllerOptions != null)
		{
			this.m_ControllerOptions.SetActive(false);
		}
		if (this.m_ControllerLeftSideButton != null)
		{
			this.m_ControllerLeftSideButton.gameObject.SetActive(false);
		}
		if (this.m_ControllerFullButton != null)
		{
			this.m_ControllerFullButton.gameObject.SetActive(false);
		}
		if (this.m_ControllerRightSideButton != null)
		{
			this.m_ControllerRightSideButton.gameObject.SetActive(false);
		}
		if (this.m_InviteButton != null)
		{
			this.m_InviteButton.SetActive(false);
		}
		if (this.m_SplitPadButton != null)
		{
			this.m_SplitPadButton.SetActive(false);
		}
		if (this.m_GamertagButton != null)
		{
			this.m_GamertagButton.SetActive(false);
		}
		if (this.m_KickButton != null)
		{
			this.m_KickButton.SetActive(false);
		}
		if (this.m_RemoveButton != null)
		{
			this.m_RemoveButton.SetActive(false);
		}
		if (this.m_ChangeProfileButton != null)
		{
			this.m_ChangeProfileButton.SetActive(false);
		}
		if (this.m_ModeButtonGroup != null)
		{
			this.m_ModeButtonGroup.SetActive(false);
		}
		if (this.m_FriendsButton != null)
		{
			this.m_FriendsButton.SetActive(false);
		}
		if (this.m_SearchButton != null)
		{
			this.m_SearchButton.SetActive(false);
		}
		this.ShowCorrectPrompt();
	}

	// Token: 0x060036DC RID: 14044 RVA: 0x001018C8 File Offset: 0x000FFCC8
	public void FocusMenu()
	{
		this.m_bIsFocussed = true;
		this.SetMouseBlockActive(false);
		if (this.m_CachedEventSystem != null)
		{
			FastList<User> users = ClientUserSystem.m_Users;
			if (!ConnectionStatus.IsHost() && ConnectionStatus.IsInSession())
			{
				for (int i = 0; i < users.Count; i++)
				{
					if (users._items[i].IsLocal && users._items[i].Engagement == EngagementSlot.One)
					{
						this.m_CachedEventSystem.SetSelectedGameObject(this.m_PlayerSlots[i].m_SlotButton.gameObject);
						break;
					}
				}
			}
			else if (users.Count != 4)
			{
				this.m_CachedEventSystem.SetSelectedGameObject(this.m_PlayerSlots[users.Count].m_SlotButton.gameObject);
			}
			else
			{
				this.m_CachedEventSystem.SetSelectedGameObject(this.m_BorderSelectables.selectOnUp.gameObject);
			}
		}
	}

	// Token: 0x060036DD RID: 14045 RVA: 0x001019C6 File Offset: 0x000FFDC6
	public void ShowPlayWithFriendsMenu()
	{
		if (this.m_switchPlayWithFriendsMenu != null)
		{
			this.m_rootMenu.CollapseCurrentTab();
			this.m_rootMenu.OpenFrontendMenu(this.m_switchPlayWithFriendsMenu);
		}
	}

	// Token: 0x060036DE RID: 14046 RVA: 0x001019F8 File Offset: 0x000FFDF8
	private void UpdateTooltipDefaults(bool _setToDefault = true)
	{
		T17TooltipManager t17TooltipManager = T17TooltipManager.Instance;
		if (t17TooltipManager == null)
		{
			t17TooltipManager = UnityEngine.Object.FindObjectOfType<T17TooltipManager>();
		}
		if (t17TooltipManager != null)
		{
			if (!ConnectionStatus.IsInSession() || ConnectionStatus.IsHost())
			{
				t17TooltipManager.SetDefaultTooltip(this.m_hostTooltip, null);
			}
			else
			{
				t17TooltipManager.SetDefaultTooltip(this.m_clientTooltip, new LocToken[]
				{
					new LocToken("[Player]", ClientUserSystem.m_Users._items[0].DisplayName)
				});
			}
			if (_setToDefault)
			{
				t17TooltipManager.Show(string.Empty, true);
			}
		}
	}

	// Token: 0x060036DF RID: 14047 RVA: 0x00101A9A File Offset: 0x000FFE9A
	public void UnFocusMenu()
	{
		this.m_bIsFocussed = false;
		this.SetMouseBlockActive(false);
		this.ShowIdleMenu();
	}

	// Token: 0x060036E0 RID: 14048 RVA: 0x00101AB0 File Offset: 0x000FFEB0
	public void SetMouseBlockActive(bool _active)
	{
		if (this.m_mouseBlock != null)
		{
			this.m_mouseBlock.SetActive(_active);
		}
	}

	// Token: 0x060036E1 RID: 14049 RVA: 0x00101ACF File Offset: 0x000FFECF
	private void FocusKitchenMenuButton(GameObject button)
	{
		if (button != null)
		{
			this.m_CachedEventSystem.SetSelectedGameObject(button);
		}
	}

	// Token: 0x060036E2 RID: 14050 RVA: 0x00101AE9 File Offset: 0x000FFEE9
	public void OnLeaveKitchen()
	{
		if (ConnectionStatus.IsInSession())
		{
			this.ShowProgressSpinnerDialog();
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnLeaveKitchenComplete));
		}
	}

	// Token: 0x060036E3 RID: 14051 RVA: 0x00101B0F File Offset: 0x000FFF0F
	private void OnLeaveKitchenComplete(IConnectionModeSwitchStatus status)
	{
		this.HideProgressSpinnerDialog();
		this.CloseOptionsMenu();
		T17FrontendFlow.Instance.OnSessionLeft();
	}

	// Token: 0x060036E4 RID: 14052 RVA: 0x00101B28 File Offset: 0x000FFF28
	public void OnUseLeftHalfOnly()
	{
		User clientUserForCurrentSlot = this.GetClientUserForCurrentSlot();
		if (clientUserForCurrentSlot != null && clientUserForCurrentSlot.PadSide != PadSide.Left)
		{
			ClientMessenger.ControllerSettings(PadSide.Left, clientUserForCurrentSlot);
			if (clientUserForCurrentSlot.GamepadUser != null)
			{
				clientUserForCurrentSlot.GamepadUser.Side = PadSide.Left;
			}
			User splitPadPartner = this.GetSplitPadPartner(clientUserForCurrentSlot);
			if (splitPadPartner != null && splitPadPartner.Split == User.SplitStatus.SplitPadGuest)
			{
				ClientMessenger.ControllerSettings(PadSide.Right, splitPadPartner);
			}
		}
	}

	// Token: 0x060036E5 RID: 14053 RVA: 0x00101B94 File Offset: 0x000FFF94
	public void OnUseFullPad()
	{
		User clientUserForCurrentSlot = this.GetClientUserForCurrentSlot();
		if (clientUserForCurrentSlot != null && clientUserForCurrentSlot.PadSide != PadSide.Both && clientUserForCurrentSlot.Split == User.SplitStatus.NotSplit)
		{
			ClientMessenger.ControllerSettings(PadSide.Both, clientUserForCurrentSlot);
			if (clientUserForCurrentSlot.GamepadUser != null)
			{
				clientUserForCurrentSlot.GamepadUser.Side = PadSide.Both;
			}
		}
	}

	// Token: 0x060036E6 RID: 14054 RVA: 0x00101BEC File Offset: 0x000FFFEC
	public void OnUseRightHalfOnly()
	{
		User clientUserForCurrentSlot = this.GetClientUserForCurrentSlot();
		if (clientUserForCurrentSlot != null && clientUserForCurrentSlot.PadSide != PadSide.Right)
		{
			ClientMessenger.ControllerSettings(PadSide.Right, clientUserForCurrentSlot);
			if (clientUserForCurrentSlot.GamepadUser != null)
			{
				clientUserForCurrentSlot.GamepadUser.Side = PadSide.Right;
			}
			User splitPadPartner = this.GetSplitPadPartner(clientUserForCurrentSlot);
			if (splitPadPartner != null && splitPadPartner.Split == User.SplitStatus.SplitPadGuest)
			{
				ClientMessenger.ControllerSettings(PadSide.Left, splitPadPartner);
			}
		}
	}

	// Token: 0x060036E7 RID: 14055 RVA: 0x00101C58 File Offset: 0x00100058
	public void OnSplitPad()
	{
		User firstUnsplitUser = this.GetFirstUnsplitUser();
		if (firstUnsplitUser != null)
		{
			if (ConnectionStatus.IsInSession())
			{
				if (UserSystemUtils.AnyRemoteUsers())
				{
					NetworkDialogHelper.ShowGoingOfflineDialog(new T17DialogBox.DialogEvent(this.LeaveOnlineModeThenSplitPad));
				}
				else
				{
					this.LeaveOnlineModeThenSplitPad();
				}
				return;
			}
			ServerUserSystem.SplitUser(firstUnsplitUser.Engagement);
			Analytics.LogEvent("Split Pad User", "Players", (long)ServerUserSystem.m_Users.Count, (Analytics.Flags)0);
			if (this.m_CurrentExpandedSlot != null)
			{
				this.m_CachedEventSystem.SetSelectedGameObject(this.m_CurrentExpandedSlot.m_SlotButton.gameObject);
			}
			this.CloseOptionsMenu();
		}
	}

	// Token: 0x060036E8 RID: 14056 RVA: 0x00101CFC File Offset: 0x001000FC
	private void LeaveOnlineModeThenSplitPad()
	{
		this.ShowProgressSpinnerDialog();
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, new GenericVoid<IConnectionModeSwitchStatus>(this.OnLeaveOnlineModeForPadSplitComplete));
	}

	// Token: 0x060036E9 RID: 14057 RVA: 0x00101D18 File Offset: 0x00100118
	private void OnLeaveOnlineModeForPadSplitComplete(IConnectionModeSwitchStatus status)
	{
		this.HideProgressSpinnerDialog();
		if (status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			this.OnSplitPad();
		}
	}

	// Token: 0x060036EA RID: 14058 RVA: 0x00101D32 File Offset: 0x00100132
	public void OnInvite()
	{
		if (!this.m_sendInviteTask.isRunning)
		{
			this.m_sendInviteTask.Start();
		}
	}

	// Token: 0x060036EB RID: 14059 RVA: 0x00101D50 File Offset: 0x00100150
	private void OnSendInviteTaskComplete(KitchenTaskResult result)
	{
		if (result != KitchenTaskResult.Success)
		{
			IConnectionModeSwitchStatus status = ConnectionModeSwitcher.GetStatus();
			if (!status.DisplayPlatformDialog())
			{
				T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
				if (dialog != null)
				{
					string text = status.GetLocalisedResultDescription();
					if (string.IsNullOrEmpty(text))
					{
						text = Localization.Get("Text.Kitchen.Invite.Failed", new LocToken[0]);
					}
					dialog.Initialize("Text.Warning", text, "Text.Button.Continue", null, null, T17DialogBox.Symbols.Warning, true, false, false);
					dialog.Show();
				}
			}
			ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, null, null);
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.FocusOnMainMenu();
		}
	}

	// Token: 0x060036EC RID: 14060 RVA: 0x00101DEA File Offset: 0x001001EA
	public void OnResults(SearchTask.SearchResultData results)
	{
		if (this.m_switchSearchMenu != null)
		{
			this.m_switchSearchMenu.SetResults(results);
		}
	}

	// Token: 0x060036ED RID: 14061 RVA: 0x00101E0C File Offset: 0x0010020C
	public void SwitchConnectionMode(OnlineMultiplayerConnectionMode mode)
	{
		if (!this.m_switchConnectionModeTask.isRunning)
		{
			this.m_selectedOnlineMode = mode;
			if (mode != OnlineMultiplayerConnectionMode.eNone)
			{
				if (mode != OnlineMultiplayerConnectionMode.eInternet)
				{
					if (mode == OnlineMultiplayerConnectionMode.eAdhoc)
					{
						this.m_switchConnectionModeTask.connectionMode = KitchenSwitchConnectionModeTask.Mode.Wireless;
					}
				}
				else
				{
					this.m_switchConnectionModeTask.connectionMode = KitchenSwitchConnectionModeTask.Mode.Internet;
				}
			}
			else
			{
				this.m_switchConnectionModeTask.connectionMode = KitchenSwitchConnectionModeTask.Mode.Offline;
			}
			this.m_switchConnectionModeTask.Start();
		}
	}

	// Token: 0x060036EE RID: 14062 RVA: 0x00101E88 File Offset: 0x00100288
	private void OnSwitchConnectionModeComplete(KitchenTaskResult result)
	{
		if (this.m_CachedEventSystem != null && this.m_ModeButton != null)
		{
			this.m_CachedEventSystem.SetSelectedGameObject(this.m_ModeButton.gameObject);
		}
		if (result != KitchenTaskResult.Success)
		{
			if (result != KitchenTaskResult.Failure)
			{
				if (result == KitchenTaskResult.Cancelled)
				{
					this.m_selectedOnlineMode = this.m_currentOnlineMode;
					this.UpdateSwitchOnlineModeText();
				}
			}
			else
			{
				IConnectionModeSwitchStatus status = ConnectionModeSwitcher.GetStatus();
				bool flag = status.DisplayPlatformDialog();
				if (!flag && this.m_switchConnectionModeTask.connectionMode == KitchenSwitchConnectionModeTask.Mode.JoinRoom)
				{
					CompositeStatus compositeStatus = status as CompositeStatus;
					if (compositeStatus != null)
					{
						JoinSessionStatus joinSessionStatus = compositeStatus.m_TaskSubStatus as JoinSessionStatus;
						if (joinSessionStatus != null && joinSessionStatus.sessionJoinResult != null)
						{
							OnlineMultiplayerSessionJoinResult returnCode = joinSessionStatus.sessionJoinResult.m_returnCode;
							flag = (returnCode != OnlineMultiplayerSessionJoinResult.eFull && returnCode != OnlineMultiplayerSessionJoinResult.eNotEnoughRoomForAllLocalUsers);
						}
						else
						{
							flag = true;
						}
					}
					else
					{
						flag = true;
					}
				}
				if (!flag)
				{
					T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
					if (dialog != null)
					{
						string text = status.GetLocalisedResultDescription();
						if (string.IsNullOrEmpty(text))
						{
							text = Localization.Get("Online.ConnectionMode.ConnectionMode.Result.eGeneric", new LocToken[0]);
						}
						dialog.Initialize("Text.Warning", text, "Text.Button.Continue", null, null, T17DialogBox.Symbols.Warning, true, false, false);
						dialog.Show();
					}
				}
				if (this.m_switchConnectionModeTask.connectionMode == KitchenSwitchConnectionModeTask.Mode.JoinRoom)
				{
					IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
					OfflineOptions offlineOptions = new OfflineOptions
					{
						hostUser = playerManager.GetUser(EngagementSlot.One),
						eAdditionalAction = OfflineOptions.AdditionalAction.None,
						connectionMode = new OnlineMultiplayerConnectionMode?(OnlineMultiplayerConnectionMode.eNone)
					};
					ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, offlineOptions, new GenericVoid<IConnectionModeSwitchStatus>(this.OnAdhocJoinFailToOfflineComplete));
				}
				this.m_selectedOnlineMode = this.m_currentOnlineMode;
				this.UpdateSwitchOnlineModeText();
			}
		}
		else
		{
			this.m_currentOnlineMode = this.m_selectedOnlineMode;
			this.ConfigureMenuForSwitch();
			if (this.m_switchConnectionModeTask.connectionMode == KitchenSwitchConnectionModeTask.Mode.Wireless && !this.m_switchConnectionModeTask.m_hosting)
			{
				this.m_rootMenu.OpenFrontendMenu(this.m_switchSearchMenu);
			}
			else if (this.m_switchConnectionModeTask.connectionMode == KitchenSwitchConnectionModeTask.Mode.Offline)
			{
				T17FrontendFlow.Instance.OnSessionLeft();
			}
		}
	}

	// Token: 0x060036EF RID: 14063 RVA: 0x001020CC File Offset: 0x001004CC
	public void OnOpenGamertag()
	{
		User clientUserForCurrentSlot = this.GetClientUserForCurrentSlot();
		if (clientUserForCurrentSlot != null)
		{
			if (clientUserForCurrentSlot.IsLocal)
			{
				this.m_IPlayerManager.ShowGamerCard(clientUserForCurrentSlot.GamepadUser);
			}
			else
			{
				this.m_IPlayerManager.ShowGamerCard(clientUserForCurrentSlot.PlatformID);
			}
		}
	}

	// Token: 0x060036F0 RID: 14064 RVA: 0x00102120 File Offset: 0x00100520
	public void OnRemovePlayer()
	{
		if (this.m_CurrentExpandedSlot != null)
		{
			User serverUserForCurrentSlot = this.GetServerUserForCurrentSlot();
			if (serverUserForCurrentSlot != null)
			{
				if (serverUserForCurrentSlot.Split == User.SplitStatus.SplitPadGuest)
				{
					ServerUserSystem.RemoveUser(serverUserForCurrentSlot, true);
				}
				else if (!serverUserForCurrentSlot.IsLocal)
				{
					FastList<User> users = ServerUserSystem.m_Users;
					User.MachineID machine = serverUserForCurrentSlot.Machine;
					User[] array = UserSystemUtils.FindUsers(users, null, machine, EngagementSlot.Count, TeamID.Count, User.SplitStatus.Count);
					if (array != null && array.Length > 0)
					{
						for (int i = 0; i < array.Length; i++)
						{
							ServerUserSystem.RemoveUser(array[i], i == array.Length - 1);
						}
					}
				}
				else
				{
					this.m_IPlayerManager.DisengagePad(serverUserForCurrentSlot.Engagement);
				}
				this.CloseOptionsMenu();
			}
		}
	}

	// Token: 0x060036F1 RID: 14065 RVA: 0x001021DC File Offset: 0x001005DC
	public void OnChangeProfile()
	{
	}

	// Token: 0x060036F2 RID: 14066 RVA: 0x001021E0 File Offset: 0x001005E0
	public void OnMode()
	{
		if (this.m_isSelectingMode)
		{
			if (this.m_selectedOnlineMode != this.m_currentOnlineMode || this.m_currentOnlineMode == OnlineMultiplayerConnectionMode.eAdhoc)
			{
				this.SwitchConnectionMode(this.m_selectedOnlineMode);
			}
			this.m_isSelectingMode = false;
		}
		else
		{
			this.m_selectedOnlineMode = this.m_currentOnlineMode;
			this.m_isSelectingMode = true;
		}
		this.EnableSwitchOnlineModeSelectionArrows(this.m_isSelectingMode);
		this.UpdateModeSelectionTooltip();
	}

	// Token: 0x060036F3 RID: 14067 RVA: 0x00102252 File Offset: 0x00100652
	public void OnCancelOnlineModeSelection()
	{
		if (this.m_isSelectingMode)
		{
			this.m_selectedOnlineMode = this.m_currentOnlineMode;
			this.UpdateSwitchOnlineModeText();
			this.EnableSwitchOnlineModeSelectionArrows(false);
			this.m_isSelectingMode = false;
			this.UpdateModeSelectionTooltip();
		}
	}

	// Token: 0x060036F4 RID: 14068 RVA: 0x00102288 File Offset: 0x00100688
	public void OnFriends()
	{
		if (this.m_currentOnlineMode != OnlineMultiplayerConnectionMode.eInternet)
		{
			this.m_switchConnectionModeTask.onComplete += this.OpenFriendsAfterConnectionModeComplete;
			this.SwitchConnectionMode(OnlineMultiplayerConnectionMode.eInternet);
		}
		else if (this.m_switchFriendsMenu != null && this.m_rootMenu != null)
		{
			this.m_switchFriendsMenu.Hide(true, false);
			this.m_rootMenu.OpenFrontendMenu(this.m_switchFriendsMenu);
		}
	}

	// Token: 0x060036F5 RID: 14069 RVA: 0x00102308 File Offset: 0x00100708
	private void OpenFriendsAfterConnectionModeComplete(KitchenTaskResult result)
	{
		if (result == KitchenTaskResult.Success && this.m_switchFriendsMenu != null && this.m_rootMenu != null)
		{
			this.m_switchFriendsMenu.Hide(true, false);
			this.m_rootMenu.OpenFrontendMenu(this.m_switchFriendsMenu);
		}
		this.m_switchConnectionModeTask.onComplete -= this.OpenFriendsAfterConnectionModeComplete;
	}

	// Token: 0x060036F6 RID: 14070 RVA: 0x00102373 File Offset: 0x00100773
	public void OnSearch()
	{
	}

	// Token: 0x060036F7 RID: 14071 RVA: 0x00102375 File Offset: 0x00100775
	public void OnSearchCancelled()
	{
		if (this.m_currentOnlineMode != OnlineMultiplayerConnectionMode.eNone)
		{
			this.m_switchConnectionModeTask.onComplete += this.OpenPlayWithFriendsAfterConnectionModeComplete;
			this.SwitchConnectionMode(OnlineMultiplayerConnectionMode.eNone);
		}
		else
		{
			this.ShowPlayWithFriendsMenu();
		}
	}

	// Token: 0x060036F8 RID: 14072 RVA: 0x001023AB File Offset: 0x001007AB
	private void OpenPlayWithFriendsAfterConnectionModeComplete(KitchenTaskResult result)
	{
		if (result == KitchenTaskResult.Success)
		{
			this.ShowPlayWithFriendsMenu();
		}
		this.m_switchConnectionModeTask.onComplete -= this.OpenPlayWithFriendsAfterConnectionModeComplete;
	}

	// Token: 0x060036F9 RID: 14073 RVA: 0x001023D0 File Offset: 0x001007D0
	private bool CanAddSplitPadGuest()
	{
		if (ServerUserSystem.m_Users.Count >= 4)
		{
			return false;
		}
		User firstUnsplitUser = this.GetFirstUnsplitUser();
		return firstUnsplitUser != null;
	}

	// Token: 0x060036FA RID: 14074 RVA: 0x00102400 File Offset: 0x00100800
	private GameInputConfig BuildInputConfig()
	{
		List<PlayerGameInput> list = new List<PlayerGameInput>();
		for (int i = 0; i < 4; i++)
		{
			PlayerGameInput engagedPlayerGameInput = LobbyUIController.GetEngagedPlayerGameInput(this.m_IPlayerManager, i);
			if (engagedPlayerGameInput != null)
			{
				list.Add(engagedPlayerGameInput);
			}
		}
		GameInputConfig.ConfigEntry[] array = new GameInputConfig.ConfigEntry[list.Count];
		for (int j = 0; j < list.Count; j++)
		{
			PlayerInputLookup.Player player = (PlayerInputLookup.Player)j;
			array[j] = new GameInputConfig.ConfigEntry(player, list[j].Pad, list[j].Side, ClientUserSystem.s_LocalMachineId, list[j].AmbiControlsMapping);
		}
		return new GameInputConfig(array);
	}

	// Token: 0x060036FB RID: 14075 RVA: 0x001024A8 File Offset: 0x001008A8
	private User GetClientUserForCurrentSlot()
	{
		User result = null;
		if (this.m_CurrentExpandedSlot != null)
		{
			int engagementSlot = (int)this.m_CurrentExpandedSlot.m_EngagementSlot;
			FastList<User> users = ClientUserSystem.m_Users;
			if (engagementSlot < users.Count)
			{
				result = users._items[engagementSlot];
			}
		}
		return result;
	}

	// Token: 0x060036FC RID: 14076 RVA: 0x001024F0 File Offset: 0x001008F0
	private User GetServerUserForCurrentSlot()
	{
		User result = null;
		if (this.m_CurrentExpandedSlot != null)
		{
			int engagementSlot = (int)this.m_CurrentExpandedSlot.m_EngagementSlot;
			FastList<User> users = ServerUserSystem.m_Users;
			if (engagementSlot < users.Count)
			{
				result = users._items[engagementSlot];
			}
		}
		return result;
	}

	// Token: 0x060036FD RID: 14077 RVA: 0x00102538 File Offset: 0x00100938
	private User GetFirstUnsplitUser()
	{
		for (int i = 0; i < ServerUserSystem.m_Users.Count; i++)
		{
			if (ServerUserSystem.m_Users._items[i].Split == User.SplitStatus.NotSplit)
			{
				return ServerUserSystem.m_Users._items[i];
			}
		}
		return null;
	}

	// Token: 0x060036FE RID: 14078 RVA: 0x00102588 File Offset: 0x00100988
	private User GetSplitPadPartner(User me)
	{
		User result = null;
		if (me != null && (me.Split == User.SplitStatus.SplitPadHost || me.Split == User.SplitStatus.SplitPadGuest))
		{
			FastList<User> users = ServerUserSystem.m_Users;
			User.MachineID machine = me.Machine;
			User[] array = UserSystemUtils.FindUsers(users, null, machine, me.Engagement, TeamID.Count, User.SplitStatus.Count);
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Split != me.Split)
				{
					result = array[i];
					break;
				}
			}
		}
		return result;
	}

	// Token: 0x060036FF RID: 14079 RVA: 0x0010260C File Offset: 0x00100A0C
	private void UpdateButtonNavigation()
	{
		T17Button[] array = this.m_OptionsParent.RequestComponentsRecursive<T17Button>();
		array = array.AllRemoved_Predicate((T17Button x) => !x.isActiveAndEnabled || !x.IsInteractable() || (this.m_ControllerFullButton != null && this.m_ControllerFullButton.gameObject.activeInHierarchy && x == this.m_ControllerLeftSideButton) || x == this.m_ControllerRightSideButton);
		for (int i = 0; i < this.m_PlayerSlots.Count; i++)
		{
			T17Button slotButton = this.m_PlayerSlots[i].m_SlotButton;
			Navigation navigation = slotButton.navigation;
			navigation.selectOnDown = ((array.Length <= 0) ? null : array[0]);
			slotButton.navigation = navigation;
		}
		if (array.Length > 0)
		{
			for (int j = 0; j < array.Length; j++)
			{
				T17Button t17Button = array[j];
				Navigation navigation2 = t17Button.navigation;
				navigation2.selectOnUp = ((j <= 0) ? null : array[j - 1]);
				navigation2.selectOnDown = ((j >= array.Length - 1) ? null : array[j + 1]);
				t17Button.navigation = navigation2;
			}
			T17Button t17Button2 = array[0];
			Navigation navigation3 = t17Button2.navigation;
			FrontendPlayerSlot slotToReturnTo = this.GetSlotToReturnTo();
			if (slotToReturnTo != null)
			{
				navigation3.selectOnUp = slotToReturnTo.m_SlotButton;
			}
			t17Button2.navigation = navigation3;
			if (this.m_ControllerFullButton != null)
			{
				if (this.m_ControllerLeftSideButton != null)
				{
					Navigation navigation4 = this.m_ControllerLeftSideButton.navigation;
					navigation4.selectOnUp = this.m_ControllerFullButton.navigation.selectOnUp;
					navigation4.selectOnDown = this.m_ControllerFullButton.navigation.selectOnDown;
					this.m_ControllerLeftSideButton.navigation = navigation4;
				}
				if (this.m_ControllerRightSideButton != null)
				{
					Navigation navigation5 = this.m_ControllerRightSideButton.navigation;
					navigation5.selectOnUp = this.m_ControllerFullButton.navigation.selectOnUp;
					navigation5.selectOnDown = this.m_ControllerFullButton.navigation.selectOnDown;
					this.m_ControllerRightSideButton.navigation = navigation5;
				}
			}
			else if (this.m_ControllerLeftSideButton != null && this.m_ControllerRightSideButton != null)
			{
				Navigation navigation6 = this.m_ControllerRightSideButton.navigation;
				navigation6.selectOnUp = this.m_ControllerLeftSideButton.navigation.selectOnUp;
				navigation6.selectOnDown = this.m_ControllerLeftSideButton.navigation.selectOnDown;
				this.m_ControllerRightSideButton.navigation = navigation6;
			}
		}
	}

	// Token: 0x06003700 RID: 14080 RVA: 0x00102884 File Offset: 0x00100C84
	private void ShowProgressSpinnerDialog()
	{
		if (this.m_progressBox == null)
		{
			this.m_progressBox = T17DialogBoxManager.GetDialog(false);
			if (this.m_progressBox != null)
			{
				this.m_progressBox.Initialize("Text.PleaseWait", string.Empty, null, null, null, T17DialogBox.Symbols.Spinner, true, true, false);
				this.m_progressBox.Show();
			}
		}
	}

	// Token: 0x06003701 RID: 14081 RVA: 0x001028E6 File Offset: 0x00100CE6
	private void HideProgressSpinnerDialog()
	{
		if (this.m_progressBox != null)
		{
			this.m_progressBox.Hide();
			this.m_progressBox = null;
		}
		this.m_displayProgressInformation = FrontendPlayerLobby.DisplayProgressInfo.ConnectionModeSwitch;
	}

	// Token: 0x06003702 RID: 14082 RVA: 0x00102914 File Offset: 0x00100D14
	private void SelectHeadChefSlot(bool bImmediate = false)
	{
		if (this.m_PlayerSlots[0] != null && this.m_PlayerSlots[0].m_SlotButton != null)
		{
			if (bImmediate)
			{
				EventSystem cachedEventSystem = this.m_CachedEventSystem;
				if (cachedEventSystem != null)
				{
					cachedEventSystem.SetSelectedGameObject(this.m_PlayerSlots[0].m_SlotButton.gameObject);
				}
			}
			else
			{
				this.m_CachedEventSystem.SetSelectedGameObject(this.m_PlayerSlots[0].m_SlotButton.gameObject);
			}
		}
	}

	// Token: 0x06003703 RID: 14083 RVA: 0x001029AF File Offset: 0x00100DAF
	private void OnNetworkErrorDismissed()
	{
		if (this.m_bIsFocussed)
		{
			this.SelectHeadChefSlot(false);
		}
	}

	// Token: 0x06003704 RID: 14084 RVA: 0x001029C4 File Offset: 0x00100DC4
	private void OnInviteJoinComplete()
	{
		PadSide currentPadSide = this.m_PlayerSlots[0].CurrentPadSide;
		FastList<User> users = ClientUserSystem.m_Users;
		User.MachineID s_LocalMachineId = ClientUserSystem.s_LocalMachineId;
		User user = UserSystemUtils.FindUser(users, null, s_LocalMachineId, EngagementSlot.One, TeamID.Count, User.SplitStatus.Count);
		ClientMessenger.ControllerSettings(currentPadSide, user);
		this.ShowIdleMenu();
		if (this.m_bIsFocussed)
		{
			this.FocusOnInviteAcceptedItem();
		}
		for (int i = 0; i < ClientUserSystem.m_Users.Count; i++)
		{
			this.TriggerUserJoinEffect(i);
		}
		SelectSaveDialog[] array = T17FrontendFlow.Instance.gameObject.RequestComponentsRecursive<SelectSaveDialog>();
		if (array != null && array.Length > 0)
		{
			for (int j = 0; j < array.Length; j++)
			{
				if (array[j].isActiveAndEnabled)
				{
					array[j].Close();
				}
			}
		}
	}

	// Token: 0x06003705 RID: 14085 RVA: 0x00102A94 File Offset: 0x00100E94
	private void EngagementPrivilegeCheckStarted(IConnectionModeSwitchStatus status)
	{
		this.m_displayProgressInformation = FrontendPlayerLobby.DisplayProgressInfo.DropInPlayer;
		this.ShowProgressSpinnerDialog();
	}

	// Token: 0x06003706 RID: 14086 RVA: 0x00102AA4 File Offset: 0x00100EA4
	private void EngagementPrivilegeCheckComplete(IConnectionModeSwitchStatus status)
	{
		this.HideProgressSpinnerDialog();
		if (status.GetResult() != eConnectionModeSwitchResult.Success && !status.DisplayPlatformDialog())
		{
			T17DialogBox dialog = T17DialogBoxManager.GetDialog(false);
			if (dialog != null)
			{
				dialog.Initialize("Text.Warning", status.GetLocalisedResultDescription(), "Text.Button.Continue", null, null, T17DialogBox.Symbols.Warning, true, false, false);
				dialog.Show();
			}
		}
	}

	// Token: 0x06003707 RID: 14087 RVA: 0x00102B04 File Offset: 0x00100F04
	public void UpdateCurrentConnectionMode()
	{
		this.m_currentOnlineMode = ConnectionStatus.CurrentConnectionMode();
		this.m_selectedOnlineMode = this.m_currentOnlineMode;
		this.UpdateSwitchOnlineModeText();
		for (int i = 0; i < this.m_PlayerSlots.Count; i++)
		{
			if (this.m_PlayerSlots[i] != null)
			{
				this.m_PlayerSlots[i].OnConnectionModeUpdated();
			}
		}
	}

	// Token: 0x06003708 RID: 14088 RVA: 0x00102B74 File Offset: 0x00100F74
	private void UpdateSwitchOnlineModeSelection()
	{
		OnlineMultiplayerConnectionMode onlineMultiplayerConnectionMode = this.m_selectedOnlineMode;
		if (this.m_cycleModeLeftButton != null && this.m_cycleModeLeftButton.JustPressed())
		{
			if (this.m_selectedOnlineMode == OnlineMultiplayerConnectionMode.eNone)
			{
				onlineMultiplayerConnectionMode = OnlineMultiplayerConnectionMode.eAdhoc;
			}
			else
			{
				onlineMultiplayerConnectionMode = (OnlineMultiplayerConnectionMode)(this.m_selectedOnlineMode - OnlineMultiplayerConnectionMode.eInternet);
			}
		}
		if (this.m_cycleModeRightButton != null && this.m_cycleModeRightButton.JustPressed())
		{
			if (this.m_selectedOnlineMode == OnlineMultiplayerConnectionMode.eAdhoc)
			{
				onlineMultiplayerConnectionMode = OnlineMultiplayerConnectionMode.eNone;
			}
			else
			{
				onlineMultiplayerConnectionMode = this.m_selectedOnlineMode + 1;
			}
		}
		if (this.m_selectedOnlineMode != onlineMultiplayerConnectionMode)
		{
			this.m_selectedOnlineMode = onlineMultiplayerConnectionMode;
			this.UpdateSwitchOnlineModeText();
			this.UpdateModeSelectionTooltip();
		}
	}

	// Token: 0x06003709 RID: 14089 RVA: 0x00102C18 File Offset: 0x00101018
	private void UpdateSwitchOnlineModeText()
	{
		if (this.m_ModeText != null)
		{
			this.m_ModeText.SetNonLocalizedText(Localization.Get("MainMenu.Kitchen.Mode." + this.m_selectedOnlineMode.ToString(), new LocToken[0]));
		}
	}

	// Token: 0x0600370A RID: 14090 RVA: 0x00102C68 File Offset: 0x00101068
	public void LeaveSession()
	{
		if (ConnectionStatus.IsInSession())
		{
			this.m_leaveSessionDialogBox = T17DialogBoxManager.GetDialog(false);
			if (this.m_leaveSessionDialogBox != null)
			{
				this.m_leaveSessionDialogBox.Initialize("Text.LeaveSession.Title", "Text.LeaveSession.Body", "Text.Button.Confirm", null, "Text.Button.Cancel", T17DialogBox.Symbols.Warning, true, true, false);
				T17DialogBox leaveSessionDialogBox = this.m_leaveSessionDialogBox;
				leaveSessionDialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(leaveSessionDialogBox.OnConfirm, new T17DialogBox.DialogEvent(this.LeaveSessionConfirmed));
				this.m_leaveSessionDialogBox.Show();
			}
		}
	}

	// Token: 0x0600370B RID: 14091 RVA: 0x00102CF2 File Offset: 0x001010F2
	private void LeaveSessionConfirmed()
	{
		if (!this.m_switchConnectionModeTask.isRunning)
		{
			this.m_switchConnectionModeTask.connectionMode = KitchenSwitchConnectionModeTask.Mode.Offline;
			this.m_switchConnectionModeTask.Start();
		}
	}

	// Token: 0x0600370C RID: 14092 RVA: 0x00102D1C File Offset: 0x0010111C
	private void EnableSwitchOnlineModeSelectionArrows(bool bEnabled)
	{
		if (this.m_LeftModeArrow != null)
		{
			this.m_LeftModeArrow.gameObject.SetActive(bEnabled);
		}
		if (this.m_RightModeArrow != null)
		{
			this.m_RightModeArrow.gameObject.SetActive(bEnabled);
		}
	}

	// Token: 0x0600370D RID: 14093 RVA: 0x00102D70 File Offset: 0x00101170
	private void AdhocSearch()
	{
		GamepadUser user = this.m_IPlayerManager.GetUser(EngagementSlot.One);
		ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, new OfflineOptions
		{
			hostUser = user,
			searchGameMode = GameMode.OnlineKitchen,
			eAdditionalAction = OfflineOptions.AdditionalAction.PrivilegeCheckAllUsersAndSearchForGames,
			connectionMode = new OnlineMultiplayerConnectionMode?(OnlineMultiplayerConnectionMode.eAdhoc)
		}, new GenericVoid<IConnectionModeSwitchStatus>(this.OnSearchComplete));
	}

	// Token: 0x0600370E RID: 14094 RVA: 0x00102DD0 File Offset: 0x001011D0
	private void OnAdhocJoinFailToOfflineComplete(IConnectionModeSwitchStatus status)
	{
		this.UpdateCurrentConnectionMode();
		this.UpdateTooltipDefaults(true);
	}

	// Token: 0x0600370F RID: 14095 RVA: 0x00102DE0 File Offset: 0x001011E0
	private void OnSearchComplete(IConnectionModeSwitchStatus status)
	{
		if (status != null && status.GetResult() == eConnectionModeSwitchResult.Success)
		{
			SearchTask.SearchResultData searchResultData = ConnectionModeSwitcher.GetAgentData() as SearchTask.SearchResultData;
			if (searchResultData != null && searchResultData.m_AvailableSessions != null)
			{
				this.OnResults(searchResultData);
				this.m_switchSearchMenu.Hide(true, false);
				this.m_rootMenu.OpenFrontendMenu(this.m_switchSearchMenu);
			}
		}
		else if (status != null && status.GetResult() == eConnectionModeSwitchResult.Failure)
		{
			this.m_switchSearchMenu.Hide(true, false);
			if (T17FrontendFlow.Instance != null)
			{
				T17FrontendFlow.Instance.FocusOnMainMenu();
			}
		}
		if (this.m_progressBox != null)
		{
			this.m_progressBox.Hide();
			this.m_progressBox = null;
		}
	}

	// Token: 0x06003710 RID: 14096 RVA: 0x00102EA8 File Offset: 0x001012A8
	private void ShowCorrectPrompt()
	{
		if (ClientUserSystem.m_Users.Count != 4)
		{
			if (this.m_bIsHeadChef && (!ConnectionStatus.IsInSession() || ConnectionStatus.IsHost()))
			{
				if (this.m_chalkboardInactivePrompt != null)
				{
					this.m_chalkboardInactivePrompt.gameObject.SetActive(true);
				}
			}
			else if (this.m_chalkboardInactivePrompt != null)
			{
				this.m_chalkboardInactivePrompt.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x06003711 RID: 14097 RVA: 0x00102F2D File Offset: 0x0010132D
	private void HidePrompts()
	{
		if (this.m_chalkboardInactivePrompt != null)
		{
			this.m_chalkboardInactivePrompt.gameObject.SetActive(false);
		}
	}

	// Token: 0x06003712 RID: 14098 RVA: 0x00102F54 File Offset: 0x00101354
	private void UpdatePromptPlayerCount()
	{
		if (!string.IsNullOrEmpty(this.m_HeadChefPrompt) && this.m_chalkboardInactivePrompt != null)
		{
			if (!this.IsPlayerSlotMenuOpen)
			{
				bool active = ClientUserSystem.m_Users.Count != 4 && (!ConnectionStatus.IsInSession() || ConnectionStatus.IsHost());
				this.m_chalkboardInactivePrompt.gameObject.SetActive(active);
			}
			int num = ClientUserSystem.m_Users.Count + 1;
			if (this.m_lastPlayerCountForPrompt != num)
			{
				string nonLocalizedText = Localization.Get(this.m_HeadChefPrompt, new LocToken[]
				{
					new LocToken("PlayerCount", num.ToString())
				});
				this.m_chalkboardInactivePrompt.SetNonLocalizedText(nonLocalizedText);
				this.m_lastPlayerCountForPrompt = num;
			}
		}
	}

	// Token: 0x06003713 RID: 14099 RVA: 0x00103028 File Offset: 0x00101428
	private void UpdateModeSelectionTooltip()
	{
		if (this.m_isSelectingMode)
		{
			switch (this.m_selectedOnlineMode)
			{
			case OnlineMultiplayerConnectionMode.eNone:
				this.m_ModeButton.m_TooltipTag = FrontendPlayerLobby.s_OfflineModeTooltip;
				break;
			case OnlineMultiplayerConnectionMode.eInternet:
				this.m_ModeButton.m_TooltipTag = FrontendPlayerLobby.s_OnlineModeTooltip;
				break;
			case OnlineMultiplayerConnectionMode.eAdhoc:
				this.m_ModeButton.m_TooltipTag = FrontendPlayerLobby.s_WirelessModeTooltip;
				break;
			}
		}
		else
		{
			this.m_ModeButton.m_TooltipTag = FrontendPlayerLobby.s_ModeTooltip;
		}
		if (T17TooltipManager.Instance != null)
		{
			T17TooltipManager.Instance.Show(this.m_ModeButton.m_TooltipTag, false);
		}
	}

	// Token: 0x06003714 RID: 14100 RVA: 0x001030E0 File Offset: 0x001014E0
	private void FocusOnInviteAcceptedItem()
	{
		EventSystem cachedEventSystem = this.m_CachedEventSystem;
		if (cachedEventSystem != null)
		{
			if (this.m_InviteFocusItem == null || !this.m_InviteFocusItem.IsInHierarchyOf(base.gameObject))
			{
				T17FrontendFlow.Instance.FocusOnMainMenu();
			}
			cachedEventSystem.SetSelectedGameObject(this.m_InviteFocusItem);
		}
	}

	// Token: 0x06003715 RID: 14101 RVA: 0x00103140 File Offset: 0x00101540
	public void JoinAdhocGame(JoinEnumeratedRoomOptions joinOptions, string progressText)
	{
		if (!this.m_switchConnectionModeTask.isRunning)
		{
			this.m_switchConnectionModeTask.connectionMode = KitchenSwitchConnectionModeTask.Mode.JoinRoom;
			this.m_switchConnectionModeTask.joinOptions = joinOptions;
			this.m_switchConnectionModeTask.joinProgressText = progressText;
			this.m_switchConnectionModeTask.Start();
		}
	}

	// Token: 0x04002BEF RID: 11247
	public List<FrontendPlayerSlot> m_PlayerSlots = new List<FrontendPlayerSlot>();

	// Token: 0x04002BF0 RID: 11248
	public FrontendChef[] m_PlayerChefs;

	// Token: 0x04002BF1 RID: 11249
	[SerializeField]
	public ParticleSystem m_playerJoinPFXPrefab;

	// Token: 0x04002BF2 RID: 11250
	[SerializeField]
	public GameOneShotAudioTag m_playerJoinAudioTag = GameOneShotAudioTag.COUNT;

	// Token: 0x04002BF3 RID: 11251
	public GameObject m_OptionsParent;

	// Token: 0x04002BF4 RID: 11252
	public T17Text m_chalkboardInactivePrompt;

	// Token: 0x04002BF5 RID: 11253
	public GameObject m_chalkboardGuestPrompt;

	// Token: 0x04002BF6 RID: 11254
	public GameObject m_LeaveKitchenButton;

	// Token: 0x04002BF7 RID: 11255
	public GameObject m_ControllerOptions;

	// Token: 0x04002BF8 RID: 11256
	public T17Button m_ControllerLeftSideButton;

	// Token: 0x04002BF9 RID: 11257
	public T17Image m_ControllerLeftSideImage;

	// Token: 0x04002BFA RID: 11258
	public T17Button m_ControllerFullButton;

	// Token: 0x04002BFB RID: 11259
	public T17Image m_ControllerFullImage;

	// Token: 0x04002BFC RID: 11260
	public T17Button m_ControllerRightSideButton;

	// Token: 0x04002BFD RID: 11261
	public T17Image m_ControllerRightSideImage;

	// Token: 0x04002BFE RID: 11262
	public GameObject m_InviteButton;

	// Token: 0x04002BFF RID: 11263
	public GameObject m_SplitPadButton;

	// Token: 0x04002C00 RID: 11264
	public GameObject m_GamertagButton;

	// Token: 0x04002C01 RID: 11265
	public GameObject m_KickButton;

	// Token: 0x04002C02 RID: 11266
	public GameObject m_RemoveButton;

	// Token: 0x04002C03 RID: 11267
	public GameObject m_ChangeProfileButton;

	// Token: 0x04002C04 RID: 11268
	public GameObject m_HostingIndicator;

	// Token: 0x04002C05 RID: 11269
	[SerializeField]
	private GameObject m_ModeButtonGroup;

	// Token: 0x04002C06 RID: 11270
	[SerializeField]
	private T17Button m_ModeButton;

	// Token: 0x04002C07 RID: 11271
	[SerializeField]
	private GameObject m_FriendsButton;

	// Token: 0x04002C08 RID: 11272
	[SerializeField]
	private GameObject m_SearchButton;

	// Token: 0x04002C09 RID: 11273
	[SerializeField]
	private GameObject m_LeftModeArrow;

	// Token: 0x04002C0A RID: 11274
	[SerializeField]
	private GameObject m_RightModeArrow;

	// Token: 0x04002C0B RID: 11275
	[SerializeField]
	private T17Text m_ModeText;

	// Token: 0x04002C0C RID: 11276
	[SerializeField]
	private FrontendSwitchSearch m_switchSearchMenu;

	// Token: 0x04002C0D RID: 11277
	[SerializeField]
	private FrontendSwitchFriends m_switchFriendsMenu;

	// Token: 0x04002C0E RID: 11278
	[SerializeField]
	private FrontendPlayWithFriendsMenu m_switchPlayWithFriendsMenu;

	// Token: 0x04002C0F RID: 11279
	[SerializeField]
	private GameObject m_mouseBlock;

	// Token: 0x04002C10 RID: 11280
	private static readonly string s_ModeTooltip = "Text.Menu.ModeTooltip";

	// Token: 0x04002C11 RID: 11281
	private static readonly string s_OnlineModeTooltip = "Text.Menu.ModeOnlineTooltip";

	// Token: 0x04002C12 RID: 11282
	private static readonly string s_OfflineModeTooltip = "Text.Menu.ModeOfflineTooltip";

	// Token: 0x04002C13 RID: 11283
	private static readonly string s_WirelessModeTooltip = "Text.Menu.ModeWirelessTooltip";

	// Token: 0x04002C14 RID: 11284
	private static readonly string s_offlineTooltip = "Text.Menu.OfflineTooltip";

	// Token: 0x04002C15 RID: 11285
	private bool m_isSelectingMode;

	// Token: 0x04002C16 RID: 11286
	private ILogicalButton m_cycleModeLeftButton;

	// Token: 0x04002C17 RID: 11287
	private ILogicalButton m_cycleModeRightButton;

	// Token: 0x04002C18 RID: 11288
	private OnlineMultiplayerConnectionMode m_currentOnlineMode;

	// Token: 0x04002C19 RID: 11289
	private OnlineMultiplayerConnectionMode m_selectedOnlineMode;

	// Token: 0x04002C1A RID: 11290
	[SerializeField]
	private string m_hostTooltip = "Text.Menu.DefaultTooltip";

	// Token: 0x04002C1B RID: 11291
	[SerializeField]
	private string m_clientTooltip = "Text.Menu.DefaultTooltipClient";

	// Token: 0x04002C1C RID: 11292
	[SerializeField]
	private GameObject m_InviteFocusItem;

	// Token: 0x04002C1D RID: 11293
	[SerializeField]
	private FrontendChefMenu m_chefMenu;

	// Token: 0x04002C1E RID: 11294
	private FrontendPlayerSlot m_CurrentExpandedSlot;

	// Token: 0x04002C1F RID: 11295
	private bool m_currentSlotHasUser;

	// Token: 0x04002C20 RID: 11296
	private T17Text m_GamertagButtonText;

	// Token: 0x04002C21 RID: 11297
	private IPlayerManager m_IPlayerManager;

	// Token: 0x04002C22 RID: 11298
	private T17DialogBox m_progressBox;

	// Token: 0x04002C23 RID: 11299
	private FrontendPlayerLobby.DisplayProgressInfo m_displayProgressInformation = FrontendPlayerLobby.DisplayProgressInfo.ConnectionModeSwitch;

	// Token: 0x04002C24 RID: 11300
	[AssignResource("Frontend_ControllerTypeSprites", Editorbility.NonEditable)]
	public ControllerTypeSprites m_ControllerSprites;

	// Token: 0x04002C25 RID: 11301
	private KitchenSendInviteTask m_sendInviteTask;

	// Token: 0x04002C26 RID: 11302
	private KitchenSwitchConnectionModeTask m_switchConnectionModeTask;

	// Token: 0x04002C27 RID: 11303
	private NetworkErrorDialog m_NetworkErrorDialog;

	// Token: 0x04002C28 RID: 11304
	private bool m_bIsHeadChef = true;

	// Token: 0x04002C29 RID: 11305
	[SerializeField]
	private FrontendRootMenu m_rootMenu;

	// Token: 0x04002C2A RID: 11306
	[SerializeField]
	private string m_HeadChefPrompt = "Text.Menu.AddLocalPlayer";

	// Token: 0x04002C2B RID: 11307
	private int m_lastPlayerCountForPrompt = -1;

	// Token: 0x04002C2C RID: 11308
	private FrontendChef.AnimationSet[] m_animSets = new FrontendChef.AnimationSet[]
	{
		FrontendChef.AnimationSet.One,
		FrontendChef.AnimationSet.Two,
		FrontendChef.AnimationSet.Three,
		FrontendChef.AnimationSet.Four
	};

	// Token: 0x04002C2D RID: 11309
	public HatMeshVisibility.VisState m_ChefHat = HatMeshVisibility.VisState.Fancy;

	// Token: 0x04002C2E RID: 11310
	private bool m_bIsFocussed;

	// Token: 0x04002C2F RID: 11311
	private bool m_isPlayerSlotMenuOpen;

	// Token: 0x04002C30 RID: 11312
	public bool m_slotMenuClosedThisFrame;

	// Token: 0x04002C31 RID: 11313
	private T17DialogBox m_leaveSessionDialogBox;

	// Token: 0x02000ABC RID: 2748
	private enum DisplayProgressInfo
	{
		// Token: 0x04002C33 RID: 11315
		None,
		// Token: 0x04002C34 RID: 11316
		ConnectionModeSwitch,
		// Token: 0x04002C35 RID: 11317
		DropInPlayer
	}
}
