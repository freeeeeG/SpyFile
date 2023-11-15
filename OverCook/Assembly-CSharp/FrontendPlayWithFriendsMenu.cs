using System;
using Team17.Online;
using UnityEngine;

// Token: 0x02000ABE RID: 2750
public class FrontendPlayWithFriendsMenu : FrontendMenuBehaviour
{
	// Token: 0x0600372D RID: 14125 RVA: 0x00103CC5 File Offset: 0x001020C5
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x0600372E RID: 14126 RVA: 0x00103CD0 File Offset: 0x001020D0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.m_switchConnectionModeTask.CleanUp();
		this.m_switchConnectionModeTask.onComplete -= this.OnSwitchConnectionModeComplete;
		KitchenSwitchConnectionModeTask switchConnectionModeTask = this.m_switchConnectionModeTask;
		switchConnectionModeTask.onResults = (KitchenSwitchConnectionModeTask.OnResults)Delegate.Remove(switchConnectionModeTask.onResults, new KitchenSwitchConnectionModeTask.OnResults(this.OnSearchResults));
	}

	// Token: 0x0600372F RID: 14127 RVA: 0x00103D2C File Offset: 0x0010212C
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		this.m_switchConnectionModeTask = new KitchenSwitchConnectionModeTask();
		this.m_switchConnectionModeTask.onComplete += this.OnSwitchConnectionModeComplete;
		KitchenSwitchConnectionModeTask switchConnectionModeTask = this.m_switchConnectionModeTask;
		switchConnectionModeTask.onResults = (KitchenSwitchConnectionModeTask.OnResults)Delegate.Combine(switchConnectionModeTask.onResults, new KitchenSwitchConnectionModeTask.OnResults(this.OnSearchResults));
	}

	// Token: 0x06003730 RID: 14128 RVA: 0x00103D88 File Offset: 0x00102188
	public bool IsBusy()
	{
		return this.m_switchConnectionModeTask != null && this.m_switchConnectionModeTask.isRunning;
	}

	// Token: 0x06003731 RID: 14129 RVA: 0x00103DA4 File Offset: 0x001021A4
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = true;
			if (this.m_playerLobby == null)
			{
				this.m_playerLobby = T17FrontendFlow.Instance.m_PlayerLobby;
			}
		}
		return true;
	}

	// Token: 0x06003732 RID: 14130 RVA: 0x00103E00 File Offset: 0x00102200
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = false;
		}
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x06003733 RID: 14131 RVA: 0x00103E25 File Offset: 0x00102225
	public override void Close()
	{
		base.Close();
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.FocusOnMainMenu();
		}
	}

	// Token: 0x06003734 RID: 14132 RVA: 0x00103E47 File Offset: 0x00102247
	protected override void Update()
	{
		base.Update();
		if (this.m_switchConnectionModeTask.isRunning)
		{
			this.m_switchConnectionModeTask.Update();
		}
	}

	// Token: 0x06003735 RID: 14133 RVA: 0x00103E6A File Offset: 0x0010226A
	public void OnHostFriendsPressed()
	{
		this.m_internetSubmode = KitchenSwitchConnectionModeTask.SubMode.Host;
		this.SwitchConnectionMode(KitchenSwitchConnectionModeTask.Mode.Internet);
	}

	// Token: 0x06003736 RID: 14134 RVA: 0x00103E7A File Offset: 0x0010227A
	public void OnJoinFriendsPressed()
	{
		this.m_switchConnectionModeTask.onComplete += this.OpenFriendsAfterConnectionModeComplete;
		this.m_internetSubmode = KitchenSwitchConnectionModeTask.SubMode.Search;
		this.SwitchConnectionMode(KitchenSwitchConnectionModeTask.Mode.Internet);
	}

	// Token: 0x06003737 RID: 14135 RVA: 0x00103EA1 File Offset: 0x001022A1
	public void OnHostWirelessPressed()
	{
		this.m_wirelessSubmode = KitchenSwitchConnectionModeTask.SubMode.Host;
		this.SwitchConnectionMode(KitchenSwitchConnectionModeTask.Mode.Wireless);
	}

	// Token: 0x06003738 RID: 14136 RVA: 0x00103EB1 File Offset: 0x001022B1
	public void OnJoinWirelessPressed()
	{
		this.m_wirelessSubmode = KitchenSwitchConnectionModeTask.SubMode.Search;
		this.SwitchConnectionMode(KitchenSwitchConnectionModeTask.Mode.Wireless);
	}

	// Token: 0x06003739 RID: 14137 RVA: 0x00103EC4 File Offset: 0x001022C4
	private void SwitchConnectionMode(KitchenSwitchConnectionModeTask.Mode mode)
	{
		if (!this.m_switchConnectionModeTask.isRunning)
		{
			this.m_switchConnectionModeTask.connectionMode = mode;
			this.m_switchConnectionModeTask.adhocSubmode = this.m_wirelessSubmode;
			this.m_switchConnectionModeTask.internetSubmode = this.m_internetSubmode;
			this.m_switchConnectionModeTask.Start();
		}
	}

	// Token: 0x0600373A RID: 14138 RVA: 0x00103F1C File Offset: 0x0010231C
	private void OnSwitchConnectionModeComplete(KitchenTaskResult result)
	{
		this.Close();
		if (result != KitchenTaskResult.Success)
		{
			if (result != KitchenTaskResult.Failure)
			{
				if (result == KitchenTaskResult.Cancelled)
				{
					Debug.Log("Connection mode change cancelled");
				}
			}
			else
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
							text = Localization.Get("Online.ConnectionMode.ConnectionMode.Result.eGeneric", new LocToken[0]);
						}
						dialog.Initialize("Text.Warning", text, "Text.Button.Continue", null, null, T17DialogBox.Symbols.Warning, true, false, false);
						dialog.Show();
					}
				}
				IPlayerManager playerManager = GameUtils.RequireManagerInterface<IPlayerManager>();
				OfflineOptions offlineOptions = new OfflineOptions
				{
					hostUser = playerManager.GetUser(EngagementSlot.One),
					eAdditionalAction = OfflineOptions.AdditionalAction.None,
					connectionMode = new OnlineMultiplayerConnectionMode?(OnlineMultiplayerConnectionMode.eNone)
				};
				ConnectionModeSwitcher.RequestConnectionState(NetConnectionState.Offline, offlineOptions, null);
			}
		}
		else
		{
			if (this.m_switchConnectionModeTask.connectionMode == KitchenSwitchConnectionModeTask.Mode.Wireless && !this.m_switchConnectionModeTask.m_hosting && this.m_rootMenu != null && this.m_switchSearchMenu != null)
			{
				this.m_rootMenu.OpenFrontendMenu(this.m_switchSearchMenu);
			}
			if (this.m_playerLobby != null)
			{
				this.m_playerLobby.UpdateCurrentConnectionMode();
			}
		}
	}

	// Token: 0x0600373B RID: 14139 RVA: 0x0010407C File Offset: 0x0010247C
	private void OpenFriendsAfterConnectionModeComplete(KitchenTaskResult result)
	{
		if (result == KitchenTaskResult.Success && this.m_switchFriendsMenu != null && this.m_rootMenu != null)
		{
			this.m_switchFriendsMenu.Hide(true, false);
			this.m_rootMenu.OpenFrontendMenu(this.m_switchFriendsMenu);
		}
		this.m_switchConnectionModeTask.onComplete -= this.OpenFriendsAfterConnectionModeComplete;
	}

	// Token: 0x0600373C RID: 14140 RVA: 0x001040E7 File Offset: 0x001024E7
	public void OnSearchResults(SearchTask.SearchResultData results)
	{
		if (this.m_switchSearchMenu != null)
		{
			this.m_switchSearchMenu.SetResults(results);
		}
	}

	// Token: 0x04002C53 RID: 11347
	[SerializeField]
	private FrontendRootMenu m_rootMenu;

	// Token: 0x04002C54 RID: 11348
	[SerializeField]
	private FrontendSwitchSearch m_switchSearchMenu;

	// Token: 0x04002C55 RID: 11349
	[SerializeField]
	private FrontendSwitchFriends m_switchFriendsMenu;

	// Token: 0x04002C56 RID: 11350
	private FrontendPlayerLobby m_playerLobby;

	// Token: 0x04002C57 RID: 11351
	private KitchenSwitchConnectionModeTask m_switchConnectionModeTask;

	// Token: 0x04002C58 RID: 11352
	private KitchenSwitchConnectionModeTask.SubMode m_wirelessSubmode;

	// Token: 0x04002C59 RID: 11353
	private KitchenSwitchConnectionModeTask.SubMode m_internetSubmode;
}
