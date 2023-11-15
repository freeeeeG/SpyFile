using System;
using UnityEngine;

// Token: 0x02000AB1 RID: 2737
public class FrontendControllerOptionsMenu : FrontendMenuBehaviour
{
	// Token: 0x06003652 RID: 13906 RVA: 0x000FEBC8 File Offset: 0x000FCFC8
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		this.m_gamepadEngagementManager = GameUtils.RequireManager<GamepadEngagementManager>();
		this.m_KeyboardRebindController = base.GetComponentInChildren<KeyboardRebindController>();
		this.m_KeyboardRebindController.SingleTimeInitialize();
	}

	// Token: 0x06003653 RID: 13907 RVA: 0x000FEBF4 File Offset: 0x000FCFF4
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (this.m_KeyboardRebindController != null)
		{
			this.m_KeyboardRebindController.OnShow(this);
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = true;
		}
		if (this.m_gamepadEngagementManager != null)
		{
			this.m_engagementSuppressor = this.m_gamepadEngagementManager.Suppressor.AddSuppressor(this);
		}
		InviteMonitor.InviteAccepted = (GenericVoid)Delegate.Combine(InviteMonitor.InviteAccepted, new GenericVoid(this.OnInviteAccepted));
		SteamPlayerManager.OverlayVisbilityChanged = (GenericVoid<bool>)Delegate.Combine(SteamPlayerManager.OverlayVisbilityChanged, new GenericVoid<bool>(this.OnOverlayVisbilityChanged));
		return base.Show(currentGamer, parent, invoker, hideInvoker);
	}

	// Token: 0x06003654 RID: 13908 RVA: 0x000FECAC File Offset: 0x000FD0AC
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = false;
		}
		if (this.m_engagementSuppressor != null)
		{
			this.m_engagementSuppressor.Release();
			this.m_engagementSuppressor = null;
		}
		InviteMonitor.InviteAccepted = (GenericVoid)Delegate.Remove(InviteMonitor.InviteAccepted, new GenericVoid(this.OnInviteAccepted));
		SteamPlayerManager.OverlayVisbilityChanged = (GenericVoid<bool>)Delegate.Remove(SteamPlayerManager.OverlayVisbilityChanged, new GenericVoid<bool>(this.OnOverlayVisbilityChanged));
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x06003655 RID: 13909 RVA: 0x000FED39 File Offset: 0x000FD139
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06003656 RID: 13910 RVA: 0x000FED41 File Offset: 0x000FD141
	protected override void Update()
	{
		if (this.m_KeyboardRebindController != null && this.m_KeyboardRebindController.IsRebinding)
		{
			return;
		}
		base.Update();
	}

	// Token: 0x06003657 RID: 13911 RVA: 0x000FED6B File Offset: 0x000FD16B
	public void CancelAndCloseAllDialogs()
	{
		if (this.m_KeyboardRebindController != null)
		{
			this.m_KeyboardRebindController.CancelAndCloseAllDialogs();
		}
	}

	// Token: 0x06003658 RID: 13912 RVA: 0x000FED8C File Offset: 0x000FD18C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		InviteMonitor.InviteAccepted = (GenericVoid)Delegate.Remove(InviteMonitor.InviteAccepted, new GenericVoid(this.OnInviteAccepted));
		SteamPlayerManager.OverlayVisbilityChanged = (GenericVoid<bool>)Delegate.Remove(SteamPlayerManager.OverlayVisbilityChanged, new GenericVoid<bool>(this.OnOverlayVisbilityChanged));
	}

	// Token: 0x06003659 RID: 13913 RVA: 0x000FEDE0 File Offset: 0x000FD1E0
	public override void Close()
	{
		if (this.m_KeyboardRebindController != null)
		{
			if (this.m_KeyboardRebindController.IsRebinding)
			{
				this.m_KeyboardRebindController.CancelRebind();
			}
			if (this.m_KeyboardRebindController.UnsavedChanges && this.m_KeyboardRebindController.ShowUnsavedChangesDialog())
			{
				return;
			}
		}
		this.Hide(true, false);
	}

	// Token: 0x0600365A RID: 13914 RVA: 0x000FEE43 File Offset: 0x000FD243
	private void OnInviteAccepted()
	{
		this.CancelAndCloseAllDialogs();
		this.Close();
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x000FEE51 File Offset: 0x000FD251
	private void OnOverlayVisbilityChanged(bool _visible)
	{
		if (_visible)
		{
			this.CancelAndCloseAllDialogs();
		}
	}

	// Token: 0x04002BB7 RID: 11191
	private GamepadEngagementManager m_gamepadEngagementManager;

	// Token: 0x04002BB8 RID: 11192
	private Suppressor m_engagementSuppressor;

	// Token: 0x04002BB9 RID: 11193
	private KeyboardRebindController m_KeyboardRebindController;
}
