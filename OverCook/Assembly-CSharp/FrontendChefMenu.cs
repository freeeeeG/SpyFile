using System;
using System.Collections.Generic;
using Team17.Online;
using UnityEngine;

// Token: 0x02000AAF RID: 2735
public class FrontendChefMenu : FrontendMenuBehaviour
{
	// Token: 0x06003646 RID: 13894 RVA: 0x000FE762 File Offset: 0x000FCB62
	private void OnInviteJoinComplete()
	{
		if (base.CurrentGamepadUser != null)
		{
			this.Close();
		}
	}

	// Token: 0x06003647 RID: 13895 RVA: 0x000FE77B File Offset: 0x000FCB7B
	protected override void Start()
	{
		base.Start();
		this.m_customiser.SetChefs(this.m_PlayerChefs);
		this.m_selectButton = PlayerInputLookup.GetEngagedButton(PlayerInputLookup.LogicalButtonID.UISelect, PlayerInputLookup.Player.One, PadSide.Both);
		this.m_cancelButton = PlayerInputLookup.GetEngagedButton(PlayerInputLookup.LogicalButtonID.UICancel, PlayerInputLookup.Player.One, PadSide.Both);
	}

	// Token: 0x06003648 RID: 13896 RVA: 0x000FE7B4 File Offset: 0x000FCBB4
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if ((!base.gameObject.activeSelf && T17FrontendFlow.Instance != null && T17FrontendFlow.Instance.IsCameraTransitioning()) || GameUtils.RequireManager<PlayerManager>().IsWarningActive(PlayerWarning.Disengaged))
		{
			return false;
		}
		this.m_currentGamer = currentGamer;
		this.m_parent = parent;
		this.m_invoker = invoker;
		InviteMonitor.InviteJoinComplete = (GenericVoid)Delegate.Combine(InviteMonitor.InviteJoinComplete, new GenericVoid(this.OnInviteJoinComplete));
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Combine(ClientUserSystem.usersChanged, new GenericVoid(this.EngagementChangedEvent));
		this.ActivateChefCustomisation(true);
		this.PlayIntroSelectingAnimations();
		FrontendRootMenu frontendRootMenu = parent as FrontendRootMenu;
		if (frontendRootMenu != null)
		{
			this.m_tabPanel = frontendRootMenu.m_MainTabPanel;
			this.m_tabPanel.Hide(true, false);
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.PushForwardCamera();
		}
		bool flag = base.Show(currentGamer, parent, invoker, hideInvoker);
		if (flag)
		{
			if (this.m_selectButton != null)
			{
				this.m_selectButton.ClaimPressEvent();
				this.m_selectButton.ClaimReleaseEvent();
			}
			if (this.m_cancelButton != null)
			{
				this.m_cancelButton.ClaimPressEvent();
				this.m_cancelButton.ClaimReleaseEvent();
			}
		}
		this.m_customiser.CacheCurrentAvatars();
		return flag;
	}

	// Token: 0x06003649 RID: 13897 RVA: 0x000FE90C File Offset: 0x000FCD0C
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (T17FrontendFlow.Instance == null || T17FrontendFlow.Instance.IsCameraTransitioning())
		{
			return false;
		}
		InviteMonitor.InviteJoinComplete = (GenericVoid)Delegate.Remove(InviteMonitor.InviteJoinComplete, new GenericVoid(this.OnInviteJoinComplete));
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.EngagementChangedEvent));
		this.ActivateChefCustomisation(false);
		this.m_customiser.PlaySelectedAnimations();
		if (base.gameObject != null && base.gameObject.activeInHierarchy && T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.PullBackCamera();
		}
		if (this.m_tabPanel != null)
		{
			this.m_tabPanel.Show(this.m_currentGamer, this.m_parent, this.m_invoker, true);
			this.m_tabPanel.ExpandCurrentTab();
		}
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x0600364A RID: 13898 RVA: 0x000FEA0C File Offset: 0x000FCE0C
	protected override void OnDestroy()
	{
		this.Hide(true, false);
		base.OnDestroy();
		ClientUserSystem.usersChanged = (GenericVoid)Delegate.Remove(ClientUserSystem.usersChanged, new GenericVoid(this.EngagementChangedEvent));
		InviteMonitor.InviteJoinComplete = (GenericVoid)Delegate.Remove(InviteMonitor.InviteJoinComplete, new GenericVoid(this.OnInviteJoinComplete));
	}

	// Token: 0x0600364B RID: 13899 RVA: 0x000FEA68 File Offset: 0x000FCE68
	protected override void Update()
	{
		base.Update();
		if (this.m_selectButton.JustPressed() && !T17FrontendFlow.Instance.IsCameraTransitioning())
		{
			this.m_selectButton.ClaimPressEvent();
			this.m_selectButton.ClaimReleaseEvent();
			this.Hide(true, false);
		}
		if (this.m_selectButton.JustPressed() && !T17FrontendFlow.Instance.IsCameraTransitioning())
		{
			this.m_cancelButton.ClaimPressEvent();
			this.m_cancelButton.ClaimReleaseEvent();
			this.Close();
		}
	}

	// Token: 0x0600364C RID: 13900 RVA: 0x000FEAF4 File Offset: 0x000FCEF4
	private void ActivateChefCustomisation(bool activate)
	{
		this.m_customiser.SetChefs(this.m_PlayerChefs);
		this.m_customiser.ActivateChefCustomisation(activate);
		T17FrontendFlow instance = T17FrontendFlow.Instance;
		if (instance != null)
		{
			if (instance.m_PlayerLobby != null)
			{
				instance.m_PlayerLobby.SetMouseBlockActive(activate);
			}
			instance.allowMultichefMenu = !activate;
		}
	}

	// Token: 0x0600364D RID: 13901 RVA: 0x000FEB57 File Offset: 0x000FCF57
	private void EngagementChangedEvent()
	{
		this.ActivateChefCustomisation(base.enabled);
	}

	// Token: 0x0600364E RID: 13902 RVA: 0x000FEB68 File Offset: 0x000FCF68
	private void PlayIntroSelectingAnimations()
	{
		int num = this.m_PlayerChefs.Length;
		for (int i = 0; i < num; i++)
		{
			this.m_PlayerChefs[i].PlaySelectingAnimation();
		}
	}

	// Token: 0x0600364F RID: 13903 RVA: 0x000FEB9D File Offset: 0x000FCF9D
	public override void Close()
	{
		this.m_customiser.RevertAvatars();
		base.Close();
	}

	// Token: 0x04002BAC RID: 11180
	public FrontendChefCustomisation[] m_PlayerChefs;

	// Token: 0x04002BAD RID: 11181
	private T17TabPanel m_tabPanel;

	// Token: 0x04002BAE RID: 11182
	private GamepadUser m_currentGamer;

	// Token: 0x04002BAF RID: 11183
	private BaseMenuBehaviour m_parent;

	// Token: 0x04002BB0 RID: 11184
	private GameObject m_invoker;

	// Token: 0x04002BB1 RID: 11185
	private ILogicalButton m_selectButton;

	// Token: 0x04002BB2 RID: 11186
	private ILogicalButton m_cancelButton;

	// Token: 0x04002BB3 RID: 11187
	private Dictionary<FrontendChefMenu.CachedKey, uint> m_cachedAvatars = new Dictionary<FrontendChefMenu.CachedKey, uint>();

	// Token: 0x04002BB4 RID: 11188
	private ChefCustomiser m_customiser = new ChefCustomiser();

	// Token: 0x02000AB0 RID: 2736
	private struct CachedKey
	{
		// Token: 0x06003650 RID: 13904 RVA: 0x000FEBB0 File Offset: 0x000FCFB0
		public CachedKey(EngagementSlot _slot, User.SplitStatus _status)
		{
			this.slot = _slot;
			this.splitStatus = _status;
		}

		// Token: 0x04002BB5 RID: 11189
		public EngagementSlot slot;

		// Token: 0x04002BB6 RID: 11190
		public User.SplitStatus splitStatus;
	}
}
