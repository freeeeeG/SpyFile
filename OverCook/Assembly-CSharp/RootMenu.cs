using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A7B RID: 2683
[Serializable]
public class RootMenu : BaseMenuBehaviour
{
	// Token: 0x0600350E RID: 13582 RVA: 0x000F7B77 File Offset: 0x000F5F77
	protected override void Awake()
	{
		base.Awake();
		this.InitializeData();
	}

	// Token: 0x0600350F RID: 13583 RVA: 0x000F7B88 File Offset: 0x000F5F88
	public virtual void InitializeData()
	{
		if (this.m_bIsDataInitialized)
		{
			return;
		}
		this.m_bIsDataInitialized = true;
		this.m_AllBaseMenuBehaviours = base.GetComponentsInChildren<BaseMenuBehaviour>(true);
		this.m_EventHelperInterfaces = base.GetComponentsInChildren<IT17EventHelper>(true);
		if (this.m_AllBaseMenuBehaviours != null)
		{
			for (int i = 0; i < this.m_AllBaseMenuBehaviours.Length; i++)
			{
				if (this.m_AllBaseMenuBehaviours[i] != this)
				{
					this.m_AllBaseMenuBehaviours[i].DoSingleTimeInitialize();
				}
			}
		}
	}

	// Token: 0x06003510 RID: 13584 RVA: 0x000F7C06 File Offset: 0x000F6006
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06003511 RID: 13585 RVA: 0x000F7C0E File Offset: 0x000F600E
	public virtual BaseMenuBehaviour GetMenuOfType<T>() where T : BaseMenuBehaviour
	{
		return null;
	}

	// Token: 0x06003512 RID: 13586 RVA: 0x000F7C11 File Offset: 0x000F6011
	public virtual int GetTabNumberOfType<T>()
	{
		return -1;
	}

	// Token: 0x06003513 RID: 13587 RVA: 0x000F7C14 File Offset: 0x000F6014
	public virtual BaseMenuBehaviour GetCurrentOpenMenu()
	{
		return null;
	}

	// Token: 0x06003514 RID: 13588 RVA: 0x000F7C17 File Offset: 0x000F6017
	public virtual List<BaseMenuBehaviour> GetCurrentMenuSet()
	{
		return null;
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x000F7C1C File Offset: 0x000F601C
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (!base.Show(currentGamer, parent, invoker, hideInvoker))
		{
			return false;
		}
		if (this.m_EventHelperInterfaces != null)
		{
			for (int i = 0; i < this.m_EventHelperInterfaces.Length; i++)
			{
				if (this.m_EventHelperInterfaces[i] != null && currentGamer != null)
				{
					this.m_EventHelperInterfaces[i].SetEventSystem(this.m_CachedEventSystem);
				}
			}
		}
		return true;
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x000F7C8C File Offset: 0x000F608C
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		if (this.m_MainTabPanel != null)
		{
			this.m_CurrentSelectedTab = this.m_MainTabPanel.CurrentTabIndex;
		}
		return true;
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x000F7CC0 File Offset: 0x000F60C0
	protected void InvokeUICancelWithStack(List<BaseMenuBehaviour> stack)
	{
		for (int i = stack.Count - 1; i >= 0; i--)
		{
			BaseMenuBehaviour baseMenuBehaviour = stack[i];
			if (baseMenuBehaviour.m_bShouldBlockParentCancelHandler && baseMenuBehaviour.NavigateOnUICancel != null)
			{
				stack[i].NavigateOnUICancel.m_DoThisOnUICancel.Invoke();
				return;
			}
		}
		if (this.m_bShouldBlockParentCancelHandler && base.NavigateOnUICancel != null)
		{
			base.NavigateOnUICancel.m_DoThisOnUICancel.Invoke();
		}
	}

	// Token: 0x04002A83 RID: 10883
	public T17TabPanel m_MainTabPanel;

	// Token: 0x04002A84 RID: 10884
	public RootMenu.RootMenuType m_RootMenuType;

	// Token: 0x04002A85 RID: 10885
	[HideInInspector]
	public RootMenu.EditorHack_BaseMenuBehaviour[] m_EditorTabAbleMenuTypes;

	// Token: 0x04002A86 RID: 10886
	private BaseMenuBehaviour[] m_AllBaseMenuBehaviours;

	// Token: 0x04002A87 RID: 10887
	private IT17EventHelper[] m_EventHelperInterfaces;

	// Token: 0x04002A88 RID: 10888
	private int m_CurrentSelectedTab;

	// Token: 0x04002A89 RID: 10889
	protected bool m_bIsDataInitialized;

	// Token: 0x02000A7C RID: 2684
	public enum RootMenuType
	{
		// Token: 0x04002A8B RID: 10891
		FrontEnd,
		// Token: 0x04002A8C RID: 10892
		InGame,
		// Token: 0x04002A8D RID: 10893
		HUD,
		// Token: 0x04002A8E RID: 10894
		Results,
		// Token: 0x04002A8F RID: 10895
		LevelEditor,
		// Token: 0x04002A90 RID: 10896
		Lobby
	}

	// Token: 0x02000A7D RID: 2685
	[Serializable]
	public class EditorHack_BaseMenuBehaviour
	{
		// Token: 0x04002A91 RID: 10897
		public int m_DefaultTab;

		// Token: 0x04002A92 RID: 10898
		public bool m_bIsExpanded;

		// Token: 0x04002A93 RID: 10899
		public List<BaseMenuBehaviour> menus;
	}

	// Token: 0x02000A7E RID: 2686
	[Serializable]
	public class MenuList_Container
	{
		// Token: 0x04002A94 RID: 10900
		public int m_DefaultTab;

		// Token: 0x04002A95 RID: 10901
		public List<BaseMenuBehaviour> m_Menus;
	}
}
