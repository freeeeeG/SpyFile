using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000ABF RID: 2751
public class FrontendRootMenu : RootMenu
{
	// Token: 0x0600373E RID: 14142 RVA: 0x00104120 File Offset: 0x00102520
	protected override void Awake()
	{
		this.m_RootMenuType = RootMenu.RootMenuType.FrontEnd;
		this.m_CurrentFrontEndMenuType = FrontendRootMenu.FrontendMenuTypeToOpen.MainMenu;
		this.m_menuIsForHeadChef = true;
		base.Awake();
		this.m_FrontEndTabableMenuTypes = new Dictionary<FrontendRootMenu.FrontendMenuTypeToOpen, RootMenu.MenuList_Container>();
		FrontendRootMenu.FrontendMenuTypeToOpen[] array = (FrontendRootMenu.FrontendMenuTypeToOpen[])Enum.GetValues(typeof(FrontendRootMenu.FrontendMenuTypeToOpen));
		for (int i = 0; i < array.Length; i++)
		{
			RootMenu.MenuList_Container menuList_Container = new RootMenu.MenuList_Container();
			menuList_Container.m_Menus = this.m_EditorTabAbleMenuTypes[i].menus;
			for (int j = 0; j < menuList_Container.m_Menus.Count; j++)
			{
				if (menuList_Container.m_Menus[j] != null)
				{
					menuList_Container.m_Menus[j].Hide(true, false);
					BaseMenuBehaviour baseMenuBehaviour = menuList_Container.m_Menus[j];
					baseMenuBehaviour.OnShow = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Remove(baseMenuBehaviour.OnShow, new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnMenuShow));
					BaseMenuBehaviour baseMenuBehaviour2 = menuList_Container.m_Menus[j];
					baseMenuBehaviour2.OnShow = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Combine(baseMenuBehaviour2.OnShow, new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnMenuShow));
					BaseMenuBehaviour baseMenuBehaviour3 = menuList_Container.m_Menus[j];
					baseMenuBehaviour3.OnHide = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Remove(baseMenuBehaviour3.OnHide, new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnMenuHide));
					BaseMenuBehaviour baseMenuBehaviour4 = menuList_Container.m_Menus[j];
					baseMenuBehaviour4.OnHide = (BaseMenuBehaviour.BaseMenuBehaviourEvent)Delegate.Combine(baseMenuBehaviour4.OnHide, new BaseMenuBehaviour.BaseMenuBehaviourEvent(this.OnMenuHide));
				}
			}
			menuList_Container.m_DefaultTab = this.m_EditorTabAbleMenuTypes[i].m_DefaultTab;
			this.m_FrontEndTabableMenuTypes.Add(array[i], menuList_Container);
		}
		if (ConnectionStatus.IsHost())
		{
			ServerUserSystem.RemoveMatchmadeUsers();
		}
		if (ConnectionStatus.IsHost() || !ConnectionStatus.IsInSession())
		{
			ServerUserSystem.ResetTeams();
		}
		InviteMonitor.SwitchHandlerType(InviteMonitor.HandlerType.Frontend);
		this.m_playerManager = GameUtils.RequireManager<PlayerManager>();
		this.m_uiCancelButton = PlayerInputLookup.GetEngagedButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One, PadSide.Both);
	}

	// Token: 0x0600373F RID: 14143 RVA: 0x001042FA File Offset: 0x001026FA
	protected override void OnDestroy()
	{
		base.OnDestroy();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.UserEngagmentChanged));
	}

	// Token: 0x06003740 RID: 14144 RVA: 0x00104324 File Offset: 0x00102724
	protected override void Update()
	{
		base.Update();
		if (this.m_uiCancelButton != null && this.m_uiCancelButton.JustReleased() && (this.m_CachedEventSystem == null || (this.m_CachedEventSystem != null && !this.m_CachedEventSystem.IsDisabled())) && !T17DialogBoxManager.HasDialogsForGamer(this.m_CurrentGamepadUser))
		{
			base.InvokeUICancelWithStack(this.m_FrontendMenuStack);
		}
	}

	// Token: 0x06003741 RID: 14145 RVA: 0x001043A0 File Offset: 0x001027A0
	private void OnMenuHide(BaseMenuBehaviour menu)
	{
		if (this.m_bClosingStack)
		{
			return;
		}
		string text = null;
		if (this.m_FrontendMenuStack.Count > 0)
		{
			if (this.m_FrontendMenuStack[this.m_FrontendMenuStack.Count - 1] != menu)
			{
				this.m_bClosingStack = true;
				int num = this.m_FrontendMenuStack.IndexOf(menu);
				if (num != -1)
				{
					for (int i = this.m_FrontendMenuStack.Count - 1; i >= num; i--)
					{
						this.m_FrontendMenuStack[i].Hide(true, false);
						this.m_FrontendMenuStack.RemoveAt(i);
					}
				}
				if (this.m_FrontendMenuStack.Count > 0)
				{
					text = this.m_FrontendMenuStack[0].GetLegendText();
				}
				this.m_bClosingStack = false;
			}
			else if (this.m_FrontendMenuStack[this.m_FrontendMenuStack.Count - 1].Hide(true, false) || !this.m_FrontendMenuStack[this.m_FrontendMenuStack.Count - 1].gameObject.activeInHierarchy)
			{
				this.m_FrontendMenuStack.Remove(menu);
				if (this.m_FrontendMenuStack.Count > 0)
				{
					text = this.m_FrontendMenuStack[this.m_FrontendMenuStack.Count - 1].GetLegendText();
				}
			}
		}
		if (!string.IsNullOrEmpty(text))
		{
			this.SetLegendText(text);
		}
	}

	// Token: 0x06003742 RID: 14146 RVA: 0x00104510 File Offset: 0x00102910
	private void OnMenuShow(BaseMenuBehaviour menu)
	{
		this.m_FrontendMenuStack.Add(menu);
		this.SetLegendText(menu.GetLegendText());
		this.SetUsernameText();
	}

	// Token: 0x06003743 RID: 14147 RVA: 0x00104530 File Offset: 0x00102930
	public void SetLegendText(string _legendText)
	{
		this.m_t17LegendText.SetLocalisedTextCatchAll(_legendText);
	}

	// Token: 0x06003744 RID: 14148 RVA: 0x00104540 File Offset: 0x00102940
	public void SetUsernameText()
	{
		if (this.m_usernameText == null)
		{
			return;
		}
		this.m_usernameText.text = string.Empty;
		if (this.m_playerManager != null)
		{
			GamepadUser user = this.m_playerManager.GetUser(EngagementSlot.One);
			if (user != null)
			{
				this.m_usernameText.text = user.DisplayName;
			}
		}
	}

	// Token: 0x06003745 RID: 14149 RVA: 0x001045AC File Offset: 0x001029AC
	public override BaseMenuBehaviour GetMenuOfType<T>()
	{
		for (int i = 0; i < this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus.Count; i++)
		{
			if (this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus[i].GetType() == typeof(T))
			{
				return this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus[i];
			}
		}
		return null;
	}

	// Token: 0x06003746 RID: 14150 RVA: 0x00104634 File Offset: 0x00102A34
	public T SearchAllForMenuOfType<T>() where T : BaseMenuBehaviour
	{
		foreach (KeyValuePair<FrontendRootMenu.FrontendMenuTypeToOpen, RootMenu.MenuList_Container> keyValuePair in this.m_FrontEndTabableMenuTypes)
		{
			RootMenu.MenuList_Container value = keyValuePair.Value;
			for (int i = 0; i < value.m_Menus.Count; i++)
			{
				if (value.m_Menus[i] != null && value.m_Menus[i].GetType() == typeof(T))
				{
					return value.m_Menus[i] as T;
				}
			}
		}
		return (T)((object)null);
	}

	// Token: 0x06003747 RID: 14151 RVA: 0x0010470C File Offset: 0x00102B0C
	public override int GetTabNumberOfType<T>()
	{
		for (int i = 0; i < this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus.Count; i++)
		{
			if (this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus[i].GetType() == typeof(T))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06003748 RID: 14152 RVA: 0x00104778 File Offset: 0x00102B78
	public override BaseMenuBehaviour GetCurrentOpenMenu()
	{
		int index = 0;
		if (this.m_MainTabPanel != null)
		{
			index = this.m_MainTabPanel.CurrentTabIndex;
		}
		return this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus[index];
	}

	// Token: 0x06003749 RID: 14153 RVA: 0x001047C0 File Offset: 0x00102BC0
	public override List<BaseMenuBehaviour> GetCurrentMenuSet()
	{
		return this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus;
	}

	// Token: 0x0600374A RID: 14154 RVA: 0x001047D8 File Offset: 0x00102BD8
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		if (this.m_MainTabPanel != null)
		{
			this.m_MainTabPanel.Show(currentGamer, this, null, true);
			this.m_MainTabPanel.SetMenuBodies(this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus);
			int defaultTab = this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_DefaultTab;
			this.ConfigureTabMenuForHeadChef();
			this.m_MainTabPanel.AttemptToSetTabIndex(defaultTab, null);
		}
		else
		{
			this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus[this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_DefaultTab].Show(currentGamer, this, null, true);
		}
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.UserEngagmentChanged));
		return true;
	}

	// Token: 0x0600374B RID: 14155 RVA: 0x001048C8 File Offset: 0x00102CC8
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		for (int i = this.m_FrontendMenuStack.Count - 1; i >= 0; i--)
		{
			if (this.m_FrontendMenuStack[i] != null)
			{
				this.m_FrontendMenuStack[i].Hide(true, false);
			}
		}
		this.m_FrontendMenuStack.Clear();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.UserEngagmentChanged));
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x0600374C RID: 14156 RVA: 0x00104951 File Offset: 0x00102D51
	public void CollapseCurrentTab()
	{
		if (this.m_MainTabPanel != null)
		{
			this.m_MainTabPanel.CollapseCurrentTab();
		}
	}

	// Token: 0x0600374D RID: 14157 RVA: 0x0010496F File Offset: 0x00102D6F
	public void ExpandCurrentTab()
	{
		if (this.m_MainTabPanel != null)
		{
			this.m_MainTabPanel.ExpandCurrentTab();
		}
	}

	// Token: 0x0600374E RID: 14158 RVA: 0x00104990 File Offset: 0x00102D90
	public void OpenFrontendMenu(FrontendMenuBehaviour menu)
	{
		foreach (KeyValuePair<FrontendRootMenu.FrontendMenuTypeToOpen, RootMenu.MenuList_Container> keyValuePair in this.m_FrontEndTabableMenuTypes)
		{
			for (int i = 0; i < keyValuePair.Value.m_Menus.Count; i++)
			{
				if (keyValuePair.Value.m_Menus[i] == menu)
				{
					menu.Show(this.m_CurrentGamepadUser, this, base.gameObject, false);
					return;
				}
			}
		}
	}

	// Token: 0x0600374F RID: 14159 RVA: 0x00104A40 File Offset: 0x00102E40
	public bool OpenFrontendChildOfCurrent(int index)
	{
		if (index > 0 && index < this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus.Count)
		{
			bool flag = this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus[index].Show(this.m_CurrentGamepadUser, this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus[0], null, true);
			if (flag)
			{
				base.RaiseMenuChangedEvent();
			}
		}
		return false;
	}

	// Token: 0x06003750 RID: 14160 RVA: 0x00104AC8 File Offset: 0x00102EC8
	public bool IsChildMenuOpen()
	{
		if (this.m_RootMenuType == RootMenu.RootMenuType.FrontEnd)
		{
			for (int i = this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus.Count - 1; i >= 1; i--)
			{
				if (this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus[i].gameObject.activeInHierarchy)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003751 RID: 14161 RVA: 0x00104B3C File Offset: 0x00102F3C
	public BaseMenuBehaviour ReturnChildMenuOpen()
	{
		if (this.m_RootMenuType == RootMenu.RootMenuType.FrontEnd)
		{
			for (int i = this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus.Count - 1; i >= 1; i--)
			{
				if (this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus[i].gameObject.activeInHierarchy)
				{
					return this.m_FrontEndTabableMenuTypes[this.m_CurrentFrontEndMenuType].m_Menus[i];
				}
			}
		}
		return null;
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x00104BCC File Offset: 0x00102FCC
	public void DoNavigateOnUICancel()
	{
		if (this.m_RootMenuType == RootMenu.RootMenuType.FrontEnd)
		{
			BaseMenuBehaviour x = null;
			foreach (KeyValuePair<FrontendRootMenu.FrontendMenuTypeToOpen, RootMenu.MenuList_Container> keyValuePair in this.m_FrontEndTabableMenuTypes)
			{
				List<BaseMenuBehaviour> menus = keyValuePair.Value.m_Menus;
				for (int i = menus.Count - 1; i >= 0; i--)
				{
					if (menus[i] != null && menus[i].gameObject != null && menus[i].gameObject.activeInHierarchy && menus[i].InvokeNavigateOnUICancel())
					{
						x = menus[i];
						break;
					}
				}
				if (x != null)
				{
					break;
				}
			}
			if (x == null && base.InvokeNavigateOnUICancel())
			{
				x = this;
			}
		}
	}

	// Token: 0x06003753 RID: 14163 RVA: 0x00104CE4 File Offset: 0x001030E4
	private void UserEngagmentChanged()
	{
		this.ConfigureTabMenuForHeadChef();
	}

	// Token: 0x06003754 RID: 14164 RVA: 0x00104CF0 File Offset: 0x001030F0
	private bool ConfigureTabMenuForHeadChef()
	{
		bool flag = !ConnectionStatus.IsInSession() || ConnectionStatus.IsHost();
		if (flag != this.m_menuIsForHeadChef)
		{
			this.m_menuIsForHeadChef = flag;
			if (this.m_MainTabPanel != null)
			{
				this.m_MainTabPanel.SetMenuEnabled(0, flag);
				this.m_MainTabPanel.SetMenuEnabled(1, flag);
				this.m_MainTabPanel.SetMenuEnabled(2, flag);
			}
			if (!flag && T17FrontendFlow.Instance.m_PlayerLobby != null && !T17FrontendFlow.Instance.m_PlayerLobby.IsFocused && this.m_MainTabPanel != null)
			{
				this.m_MainTabPanel.AttemptToSetTabIndex(this.m_MainTabPanel.CurrentTabIndex, null);
			}
			return true;
		}
		return false;
	}

	// Token: 0x06003755 RID: 14165 RVA: 0x00104DB8 File Offset: 0x001031B8
	public void HideMenuStack()
	{
		for (int i = this.m_FrontendMenuStack.Count - 1; i >= 0; i--)
		{
			this.m_FrontendMenuStack[i].Hide(true, false);
		}
		this.m_FrontendMenuStack.Clear();
	}

	// Token: 0x04002C5A RID: 11354
	[HideInInspector]
	public FrontendRootMenu.FrontendMenuTypeToOpen m_CurrentFrontEndMenuType;

	// Token: 0x04002C5B RID: 11355
	public Dictionary<FrontendRootMenu.FrontendMenuTypeToOpen, RootMenu.MenuList_Container> m_FrontEndTabableMenuTypes;

	// Token: 0x04002C5C RID: 11356
	private List<BaseMenuBehaviour> m_FrontendMenuStack = new List<BaseMenuBehaviour>();

	// Token: 0x04002C5D RID: 11357
	public T17Text m_t17LegendText;

	// Token: 0x04002C5E RID: 11358
	public T17Text m_usernameText;

	// Token: 0x04002C5F RID: 11359
	private bool m_bClosingStack;

	// Token: 0x04002C60 RID: 11360
	private bool m_menuIsForHeadChef = true;

	// Token: 0x04002C61 RID: 11361
	private PlayerManager m_playerManager;

	// Token: 0x04002C62 RID: 11362
	private ILogicalButton m_uiCancelButton;

	// Token: 0x02000AC0 RID: 2752
	public enum FrontendMenuTypeToOpen
	{
		// Token: 0x04002C64 RID: 11364
		MainMenu,
		// Token: 0x04002C65 RID: 11365
		Campaign,
		// Token: 0x04002C66 RID: 11366
		Coop,
		// Token: 0x04002C67 RID: 11367
		Versus,
		// Token: 0x04002C68 RID: 11368
		Customization,
		// Token: 0x04002C69 RID: 11369
		Settings,
		// Token: 0x04002C6A RID: 11370
		Switch,
		// Token: 0x04002C6B RID: 11371
		SaveSelect,
		// Token: 0x04002C6C RID: 11372
		DLC,
		// Token: 0x04002C6D RID: 11373
		Popup,
		// Token: 0x04002C6E RID: 11374
		SaveWait
	}
}
