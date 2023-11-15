using System;
using UnityEngine;

// Token: 0x02000AC1 RID: 2753
public class FrontendSaveWaitingDialog : FrontendMenuBehaviour
{
	// Token: 0x06003757 RID: 14167 RVA: 0x00104E0A File Offset: 0x0010320A
	protected override void SingleTimeInitialize()
	{
		base.SingleTimeInitialize();
		this.m_gamepadEngagementManager = GameUtils.RequireManager<GamepadEngagementManager>();
	}

	// Token: 0x06003758 RID: 14168 RVA: 0x00104E20 File Offset: 0x00103220
	public override bool Show(GamepadUser _currentGamer, BaseMenuBehaviour _parent, GameObject _invoker, bool _hideInvoker = true)
	{
		if (!base.Show(_currentGamer, _parent, _invoker, _hideInvoker))
		{
			return false;
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = true;
		}
		if (this.m_gamepadEngagementManager != null)
		{
			this.m_engagementSuppressor = this.m_gamepadEngagementManager.Suppressor.AddSuppressor(this);
		}
		if (this.m_legend)
		{
			this.m_legend.SetActive(false);
		}
		return true;
	}

	// Token: 0x06003759 RID: 14169 RVA: 0x00104EA0 File Offset: 0x001032A0
	public override bool Hide(bool restoreInvokerState = true, bool isTabSwitch = false)
	{
		if (!base.Hide(restoreInvokerState, isTabSwitch))
		{
			return false;
		}
		if (this.m_engagementSuppressor != null)
		{
			this.m_engagementSuppressor.Release();
			this.m_engagementSuppressor = null;
		}
		if (T17FrontendFlow.Instance != null)
		{
			T17FrontendFlow.Instance.BlockFocusKitchen = false;
		}
		if (this.m_legend != null)
		{
			this.m_legend.SetActive(true);
		}
		return true;
	}

	// Token: 0x0600375A RID: 14170 RVA: 0x00104F14 File Offset: 0x00103314
	protected override void Update()
	{
		base.Update();
		if (this.m_Timer != null && T17FrontendFlow.Instance != null && ConnectionStatus.IsInSession())
		{
			float num = T17FrontendFlow.Instance.ClientCountdown;
			num = Mathf.Max((float)Mathf.FloorToInt(num + 0.99f), 0f);
			this.m_Timer.text = num.ToString();
		}
	}

	// Token: 0x04002C6F RID: 11375
	[SerializeField]
	private T17Text m_Timer;

	// Token: 0x04002C70 RID: 11376
	[SerializeField]
	private GameObject m_legend;

	// Token: 0x04002C71 RID: 11377
	private GamepadEngagementManager m_gamepadEngagementManager;

	// Token: 0x04002C72 RID: 11378
	private Suppressor m_engagementSuppressor;
}
