using System;
using UnityEngine;

// Token: 0x02000B2D RID: 2861
public class KModalButtonMenu : KButtonMenu
{
	// Token: 0x06005831 RID: 22577 RVA: 0x00205671 File Offset: 0x00203871
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.modalBackground = KModalScreen.MakeScreenModal(this);
	}

	// Token: 0x06005832 RID: 22578 RVA: 0x00205685 File Offset: 0x00203885
	protected override void OnCmpEnable()
	{
		KModalScreen.ResizeBackground(this.modalBackground);
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Combine(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x06005833 RID: 22579 RVA: 0x002056B8 File Offset: 0x002038B8
	protected override void OnCmpDisable()
	{
		base.OnCmpDisable();
		if (this.childDialog == null)
		{
			base.Trigger(476357528, null);
		}
		ScreenResize instance = ScreenResize.Instance;
		instance.OnResize = (System.Action)Delegate.Remove(instance.OnResize, new System.Action(this.OnResize));
	}

	// Token: 0x06005834 RID: 22580 RVA: 0x0020570B File Offset: 0x0020390B
	private void OnResize()
	{
		KModalScreen.ResizeBackground(this.modalBackground);
	}

	// Token: 0x06005835 RID: 22581 RVA: 0x00205718 File Offset: 0x00203918
	public override bool IsModal()
	{
		return true;
	}

	// Token: 0x06005836 RID: 22582 RVA: 0x0020571B File Offset: 0x0020391B
	public override float GetSortKey()
	{
		return 100f;
	}

	// Token: 0x06005837 RID: 22583 RVA: 0x00205724 File Offset: 0x00203924
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		if (SpeedControlScreen.Instance != null)
		{
			if (show && !this.shown)
			{
				SpeedControlScreen.Instance.Pause(false, false);
			}
			else if (!show && this.shown)
			{
				SpeedControlScreen.Instance.Unpause(false);
			}
			this.shown = show;
		}
		if (CameraController.Instance != null)
		{
			CameraController.Instance.DisableUserCameraControl = show;
		}
	}

	// Token: 0x06005838 RID: 22584 RVA: 0x00205793 File Offset: 0x00203993
	public override void OnKeyDown(KButtonEvent e)
	{
		base.OnKeyDown(e);
		e.Consumed = true;
	}

	// Token: 0x06005839 RID: 22585 RVA: 0x002057A3 File Offset: 0x002039A3
	public override void OnKeyUp(KButtonEvent e)
	{
		base.OnKeyUp(e);
		e.Consumed = true;
	}

	// Token: 0x0600583A RID: 22586 RVA: 0x002057B3 File Offset: 0x002039B3
	public void SetBackgroundActive(bool active)
	{
	}

	// Token: 0x0600583B RID: 22587 RVA: 0x002057B8 File Offset: 0x002039B8
	protected GameObject ActivateChildScreen(GameObject screenPrefab)
	{
		GameObject gameObject = Util.KInstantiateUI(screenPrefab, base.transform.parent.gameObject, false);
		this.childDialog = gameObject;
		gameObject.Subscribe(476357528, new Action<object>(this.Unhide));
		this.Hide();
		return gameObject;
	}

	// Token: 0x0600583C RID: 22588 RVA: 0x00205803 File Offset: 0x00203A03
	private void Hide()
	{
		this.panelRoot.rectTransform().localScale = Vector3.zero;
	}

	// Token: 0x0600583D RID: 22589 RVA: 0x0020581A File Offset: 0x00203A1A
	private void Unhide(object data = null)
	{
		this.panelRoot.rectTransform().localScale = Vector3.one;
		this.childDialog.Unsubscribe(476357528, new Action<object>(this.Unhide));
		this.childDialog = null;
	}

	// Token: 0x04003BAF RID: 15279
	private bool shown;

	// Token: 0x04003BB0 RID: 15280
	[SerializeField]
	private GameObject panelRoot;

	// Token: 0x04003BB1 RID: 15281
	private GameObject childDialog;

	// Token: 0x04003BB2 RID: 15282
	private RectTransform modalBackground;
}
