using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000421 RID: 1057
[Serializable]
public class DebugRootMenu : FrontendMenuBehaviour
{
	// Token: 0x0600132D RID: 4909 RVA: 0x0006B826 File Offset: 0x00069C26
	protected override void Awake()
	{
		base.Awake();
		this.InitializeData();
	}

	// Token: 0x0600132E RID: 4910 RVA: 0x0006B834 File Offset: 0x00069C34
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

	// Token: 0x0600132F RID: 4911 RVA: 0x0006B8B2 File Offset: 0x00069CB2
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06001330 RID: 4912 RVA: 0x0006B8BA File Offset: 0x00069CBA
	public virtual BaseMenuBehaviour GetMenuOfType<T>()
	{
		return null;
	}

	// Token: 0x06001331 RID: 4913 RVA: 0x0006B8BD File Offset: 0x00069CBD
	public virtual int GetTabNumberOfType<T>()
	{
		return -1;
	}

	// Token: 0x06001332 RID: 4914 RVA: 0x0006B8C0 File Offset: 0x00069CC0
	public virtual BaseMenuBehaviour GetCurrentOpenMenu()
	{
		return null;
	}

	// Token: 0x06001333 RID: 4915 RVA: 0x0006B8C3 File Offset: 0x00069CC3
	public virtual List<BaseMenuBehaviour> GetCurrentMenuSet()
	{
		return null;
	}

	// Token: 0x06001334 RID: 4916 RVA: 0x0006B8C8 File Offset: 0x00069CC8
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

	// Token: 0x06001335 RID: 4917 RVA: 0x0006B938 File Offset: 0x00069D38
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

	// Token: 0x04000F11 RID: 3857
	public T17TabPanel m_MainTabPanel;

	// Token: 0x04000F12 RID: 3858
	public DebugRootMenu.RootMenuType m_RootMenuType;

	// Token: 0x04000F13 RID: 3859
	[HideInInspector]
	public DebugRootMenu.EditorHack_BaseMenuBehaviour[] m_EditorTabAbleMenuTypes;

	// Token: 0x04000F14 RID: 3860
	private BaseMenuBehaviour[] m_AllBaseMenuBehaviours;

	// Token: 0x04000F15 RID: 3861
	private IT17EventHelper[] m_EventHelperInterfaces;

	// Token: 0x04000F16 RID: 3862
	private int m_CurrentSelectedTab;

	// Token: 0x04000F17 RID: 3863
	protected bool m_bIsDataInitialized;

	// Token: 0x02000422 RID: 1058
	public enum RootMenuType
	{
		// Token: 0x04000F19 RID: 3865
		FrontEnd,
		// Token: 0x04000F1A RID: 3866
		InGame,
		// Token: 0x04000F1B RID: 3867
		HUD,
		// Token: 0x04000F1C RID: 3868
		Results,
		// Token: 0x04000F1D RID: 3869
		LevelEditor,
		// Token: 0x04000F1E RID: 3870
		Lobby
	}

	// Token: 0x02000423 RID: 1059
	[Serializable]
	public class EditorHack_BaseMenuBehaviour
	{
		// Token: 0x04000F1F RID: 3871
		public int m_DefaultTab;

		// Token: 0x04000F20 RID: 3872
		public bool m_bIsExpanded;

		// Token: 0x04000F21 RID: 3873
		public List<BaseMenuBehaviour> menus;
	}

	// Token: 0x02000424 RID: 1060
	[Serializable]
	public class MenuList_Container
	{
		// Token: 0x04000F22 RID: 3874
		public int m_DefaultTab;

		// Token: 0x04000F23 RID: 3875
		public List<BaseMenuBehaviour> m_Menus;
	}
}
