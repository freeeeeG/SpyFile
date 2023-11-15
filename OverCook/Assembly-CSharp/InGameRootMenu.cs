using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ACC RID: 2764
public class InGameRootMenu : RootMenu
{
	// Token: 0x060037C0 RID: 14272 RVA: 0x00106A28 File Offset: 0x00104E28
	protected override void Awake()
	{
		this.m_RootMenuType = RootMenu.RootMenuType.InGame;
		this.m_CurrentInGameMenuType = InGameRootMenu.IngameMenuTypeToOpen.WorldMapPause;
		base.Awake();
		this.m_InGameMenuTypes = new Dictionary<InGameRootMenu.IngameMenuTypeToOpen, RootMenu.MenuList_Container>(default(InGameRootMenu.IngameMenuTypeToOpenComparer));
		InGameRootMenu.IngameMenuTypeToOpen[] array = (InGameRootMenu.IngameMenuTypeToOpen[])Enum.GetValues(typeof(InGameRootMenu.IngameMenuTypeToOpen));
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
			this.m_InGameMenuTypes.Add(array[i], menuList_Container);
		}
		this.m_uiCancelButton = PlayerInputLookup.GetEngagedButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One, PadSide.Both);
		this.m_IPlayerManager = GameUtils.RequireManager<PlayerManager>();
		this.m_IPlayerManager.EngagementChangeCallback += this.OnEngagementChanged;
	}

	// Token: 0x060037C1 RID: 14273 RVA: 0x00106BFC File Offset: 0x00104FFC
	protected override void OnDestroy()
	{
		this.m_IPlayerManager.EngagementChangeCallback -= this.OnEngagementChanged;
	}

	// Token: 0x060037C2 RID: 14274 RVA: 0x00106C15 File Offset: 0x00105015
	private void OnEngagementChanged(EngagementSlot _slot, GamepadUser _old, GamepadUser _new)
	{
		this.m_CurrentGamepadUser = _new;
	}

	// Token: 0x060037C3 RID: 14275 RVA: 0x00106C20 File Offset: 0x00105020
	private void OnMenuHide(BaseMenuBehaviour menu)
	{
		if (this.m_bClosingStack)
		{
			return;
		}
		if (this.m_InGameMenuStack.Count > 0)
		{
			if (this.m_InGameMenuStack[this.m_InGameMenuStack.Count - 1] != menu)
			{
				this.m_bClosingStack = true;
				int num = this.m_InGameMenuStack.IndexOf(menu);
				for (int i = this.m_InGameMenuStack.Count - 1; i >= num; i--)
				{
					this.m_InGameMenuStack[i].Hide(true, false);
					this.m_InGameMenuStack.RemoveAt(i);
				}
				this.m_bClosingStack = false;
			}
			else
			{
				this.m_InGameMenuStack.Remove(menu);
			}
		}
	}

	// Token: 0x060037C4 RID: 14276 RVA: 0x00106CD8 File Offset: 0x001050D8
	private void OnMenuShow(BaseMenuBehaviour menu)
	{
		this.m_InGameMenuStack.Add(menu);
	}

	// Token: 0x060037C5 RID: 14277 RVA: 0x00106CE8 File Offset: 0x001050E8
	public override BaseMenuBehaviour GetMenuOfType<T>()
	{
		for (int i = 0; i < this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus.Count; i++)
		{
			if (this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus[i].GetType() == typeof(T))
			{
				return this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus[i];
			}
		}
		return null;
	}

	// Token: 0x060037C6 RID: 14278 RVA: 0x00106D70 File Offset: 0x00105170
	public override int GetTabNumberOfType<T>()
	{
		for (int i = 0; i < this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus.Count; i++)
		{
			if (this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus[i].GetType() == typeof(T))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x060037C7 RID: 14279 RVA: 0x00106DDC File Offset: 0x001051DC
	public override BaseMenuBehaviour GetCurrentOpenMenu()
	{
		if (this.m_InGameMenuStack.Count > 0)
		{
			return this.m_InGameMenuStack[this.m_InGameMenuStack.Count - 1];
		}
		return null;
	}

	// Token: 0x060037C8 RID: 14280 RVA: 0x00106E0C File Offset: 0x0010520C
	public bool IsCurrentOpenMenuOfType(InGameRootMenu.IngameMenuTypeToOpen _type)
	{
		RootMenu.MenuList_Container menuList_Container;
		if (this.m_InGameMenuTypes != null && this.m_InGameMenuTypes.TryGetValue(_type, out menuList_Container))
		{
			BaseMenuBehaviour currentOpenMenu = this.GetCurrentOpenMenu();
			for (int i = 0; i < menuList_Container.m_Menus.Count; i++)
			{
				if (currentOpenMenu == menuList_Container.m_Menus[i])
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060037C9 RID: 14281 RVA: 0x00106E74 File Offset: 0x00105274
	public override List<BaseMenuBehaviour> GetCurrentMenuSet()
	{
		return this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus;
	}

	// Token: 0x060037CA RID: 14282 RVA: 0x00106E8C File Offset: 0x0010528C
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		if (this.m_MainTabPanel != null)
		{
			this.m_MainTabPanel.Show(currentGamer, this, null, true);
			this.m_MainTabPanel.SetMenuBodies(this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus);
			int defaultTab = this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_DefaultTab;
			this.m_MainTabPanel.SetTabIndex(defaultTab, null);
		}
		else
		{
			this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus[this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_DefaultTab].Show(currentGamer, this, null, true);
		}
		return true;
	}

	// Token: 0x060037CB RID: 14283 RVA: 0x00106F54 File Offset: 0x00105354
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		for (int i = this.m_InGameMenuStack.Count - 1; i >= 0; i--)
		{
			if (this.m_InGameMenuStack[i] != null)
			{
				this.m_InGameMenuStack[i].Hide(true, false);
			}
		}
		this.m_InGameMenuStack.Clear();
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x060037CC RID: 14284 RVA: 0x00106FC0 File Offset: 0x001053C0
	public void OpenInGameMenu(InGameMenuBehaviour menu)
	{
		foreach (KeyValuePair<InGameRootMenu.IngameMenuTypeToOpen, RootMenu.MenuList_Container> keyValuePair in this.m_InGameMenuTypes)
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

	// Token: 0x060037CD RID: 14285 RVA: 0x00107070 File Offset: 0x00105470
	public bool OpenInGameMenu(InGameRootMenu.IngameMenuTypeToOpen menuType, int index = -1)
	{
		index = ((index != -1) ? index : this.m_InGameMenuTypes[menuType].m_DefaultTab);
		this.m_CurrentInGameMenuType = menuType;
		return this.m_InGameMenuTypes[menuType].m_Menus[index].Show(this.m_CurrentGamepadUser, this, null, true);
	}

	// Token: 0x060037CE RID: 14286 RVA: 0x001070CC File Offset: 0x001054CC
	public bool OpenInGameChildOfCurrent(int index)
	{
		if (index > 0 && index < this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus.Count)
		{
			bool flag = this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus[index].Show(this.m_CurrentGamepadUser, this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus[0], null, true);
			if (flag)
			{
				base.RaiseMenuChangedEvent();
			}
		}
		return false;
	}

	// Token: 0x060037CF RID: 14287 RVA: 0x00107154 File Offset: 0x00105554
	public bool IsChildMenuOpen()
	{
		if (this.m_RootMenuType == RootMenu.RootMenuType.InGame)
		{
			for (int i = this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus.Count - 1; i >= 1; i--)
			{
				if (this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus[i].gameObject.activeInHierarchy)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x060037D0 RID: 14288 RVA: 0x001071CC File Offset: 0x001055CC
	public BaseMenuBehaviour ReturnChildMenuOpen()
	{
		if (this.m_RootMenuType == RootMenu.RootMenuType.InGame)
		{
			for (int i = this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus.Count - 1; i >= 1; i--)
			{
				if (this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus[i].gameObject.activeInHierarchy)
				{
					return this.m_InGameMenuTypes[this.m_CurrentInGameMenuType].m_Menus[i];
				}
			}
		}
		return null;
	}

	// Token: 0x060037D1 RID: 14289 RVA: 0x0010725C File Offset: 0x0010565C
	public void DoNavigateOnUICancel()
	{
		InGameMenuBehaviour inGameMenuBehaviour = (InGameMenuBehaviour)this.ReturnChildMenuOpen();
		if (inGameMenuBehaviour == null)
		{
			inGameMenuBehaviour = (InGameMenuBehaviour)this.GetCurrentOpenMenu();
		}
		if (inGameMenuBehaviour != null)
		{
			inGameMenuBehaviour.InvokeNavigateOnUICancel();
		}
	}

	// Token: 0x060037D2 RID: 14290 RVA: 0x001072A0 File Offset: 0x001056A0
	public void PromptGameExit()
	{
		T17DialogBox dialog = T17DialogBoxManager.GetDialog(true);
		if (dialog != null)
		{
			dialog.Initialize("Text.UI.Quit", "Text.Menu.OkToExit", "Text.Dialog.Prompt.Yes", "Text.Dialog.Prompt.No", string.Empty, T17DialogBox.Symbols.Warning, true, true, false);
			T17DialogBox t17DialogBox = dialog;
			t17DialogBox.OnConfirm = (T17DialogBox.DialogEvent)Delegate.Combine(t17DialogBox.OnConfirm, new T17DialogBox.DialogEvent(this.DoExit));
			dialog.Show();
		}
	}

	// Token: 0x060037D3 RID: 14291 RVA: 0x0010730B File Offset: 0x0010570B
	private void DoExit()
	{
		Application.Quit();
	}

	// Token: 0x060037D4 RID: 14292 RVA: 0x00107314 File Offset: 0x00105714
	protected override void Update()
	{
		base.Update();
		if (this.m_uiCancelButton != null && this.m_uiCancelButton.JustReleased() && (this.m_CachedEventSystem == null || (this.m_CachedEventSystem != null && !this.m_CachedEventSystem.IsDisabled())) && (this.m_CurrentGamepadUser == null || !T17DialogBoxManager.HasDialogsForGamer(this.m_CurrentGamepadUser)))
		{
			base.InvokeUICancelWithStack(this.m_InGameMenuStack);
		}
	}

	// Token: 0x04002CA8 RID: 11432
	[HideInInspector]
	public InGameRootMenu.IngameMenuTypeToOpen m_CurrentInGameMenuType;

	// Token: 0x04002CA9 RID: 11433
	public Dictionary<InGameRootMenu.IngameMenuTypeToOpen, RootMenu.MenuList_Container> m_InGameMenuTypes;

	// Token: 0x04002CAA RID: 11434
	private List<BaseMenuBehaviour> m_InGameMenuStack = new List<BaseMenuBehaviour>();

	// Token: 0x04002CAB RID: 11435
	private bool m_bClosingStack;

	// Token: 0x04002CAC RID: 11436
	private ILogicalButton m_uiCancelButton;

	// Token: 0x04002CAD RID: 11437
	private PlayerManager m_IPlayerManager;

	// Token: 0x02000ACD RID: 2765
	public enum IngameMenuTypeToOpen
	{
		// Token: 0x04002CAF RID: 11439
		WorldMapPause,
		// Token: 0x04002CB0 RID: 11440
		InLevelPause,
		// Token: 0x04002CB1 RID: 11441
		Controls,
		// Token: 0x04002CB2 RID: 11442
		Customisation,
		// Token: 0x04002CB3 RID: 11443
		GameMode
	}

	// Token: 0x02000ACE RID: 2766
	public struct IngameMenuTypeToOpenComparer : IEqualityComparer<InGameRootMenu.IngameMenuTypeToOpen>
	{
		// Token: 0x060037D5 RID: 14293 RVA: 0x001073A1 File Offset: 0x001057A1
		public bool Equals(InGameRootMenu.IngameMenuTypeToOpen x, InGameRootMenu.IngameMenuTypeToOpen y)
		{
			return x == y;
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x001073A7 File Offset: 0x001057A7
		public int GetHashCode(InGameRootMenu.IngameMenuTypeToOpen obj)
		{
			return (int)obj;
		}
	}
}
