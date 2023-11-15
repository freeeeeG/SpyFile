using System;
using UnityEngine;

// Token: 0x02000AC8 RID: 2760
public class InGameControllerOptionsMenu : InGameMenuBehaviour
{
	// Token: 0x06003792 RID: 14226 RVA: 0x00105EA2 File Offset: 0x001042A2
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		this.m_KeyboardRebindController = base.GetComponentInChildren<KeyboardRebindController>();
		this.m_KeyboardRebindController.SingleTimeInitialize();
	}

	// Token: 0x06003793 RID: 14227 RVA: 0x00105EC4 File Offset: 0x001042C4
	public override bool Show(GamepadUser currentGamer, BaseMenuBehaviour parent, GameObject invoker, bool hideInvoker = true)
	{
		if (this.m_KeyboardRebindController != null)
		{
			this.m_KeyboardRebindController.OnShow(this);
		}
		SteamPlayerManager.OverlayVisbilityChanged = (GenericVoid<bool>)Delegate.Combine(SteamPlayerManager.OverlayVisbilityChanged, new GenericVoid<bool>(this.OnOverlayVisbilityChanged));
		return base.Show(currentGamer, parent, invoker, hideInvoker);
	}

	// Token: 0x06003794 RID: 14228 RVA: 0x00105F19 File Offset: 0x00104319
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		SteamPlayerManager.OverlayVisbilityChanged = (GenericVoid<bool>)Delegate.Remove(SteamPlayerManager.OverlayVisbilityChanged, new GenericVoid<bool>(this.OnOverlayVisbilityChanged));
		return base.Hide(restoreInvokerState, isTabSwitch);
	}

	// Token: 0x06003795 RID: 14229 RVA: 0x00105F43 File Offset: 0x00104343
	protected override void OnDestroy()
	{
		base.OnDestroy();
		SteamPlayerManager.OverlayVisbilityChanged = (GenericVoid<bool>)Delegate.Remove(SteamPlayerManager.OverlayVisbilityChanged, new GenericVoid<bool>(this.OnOverlayVisbilityChanged));
	}

	// Token: 0x06003796 RID: 14230 RVA: 0x00105F6B File Offset: 0x0010436B
	protected override void Update()
	{
		if (this.m_KeyboardRebindController != null && this.m_KeyboardRebindController.IsRebinding)
		{
			return;
		}
		base.Update();
	}

	// Token: 0x06003797 RID: 14231 RVA: 0x00105F98 File Offset: 0x00104398
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
		base.Close();
	}

	// Token: 0x06003798 RID: 14232 RVA: 0x00105FF8 File Offset: 0x001043F8
	private void OnOverlayVisbilityChanged(bool _visible)
	{
		if (_visible && this.m_KeyboardRebindController != null)
		{
			this.m_KeyboardRebindController.CancelAndCloseAllDialogs();
		}
	}

	// Token: 0x04002C92 RID: 11410
	private KeyboardRebindController m_KeyboardRebindController;
}
