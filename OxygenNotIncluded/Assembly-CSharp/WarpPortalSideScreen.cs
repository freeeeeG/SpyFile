using System;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C5D RID: 3165
public class WarpPortalSideScreen : SideScreenContent
{
	// Token: 0x060064A0 RID: 25760 RVA: 0x00252708 File Offset: 0x00250908
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.buttonLabel.SetText(UI.UISIDESCREENS.WARPPORTALSIDESCREEN.BUTTON);
		this.cancelButtonLabel.SetText(UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CANCELBUTTON);
		this.button.onClick += this.OnButtonClick;
		this.cancelButton.onClick += this.OnCancelClick;
		this.Refresh(null);
	}

	// Token: 0x060064A1 RID: 25761 RVA: 0x0025277A File Offset: 0x0025097A
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<WarpPortal>() != null;
	}

	// Token: 0x060064A2 RID: 25762 RVA: 0x00252788 File Offset: 0x00250988
	public override void SetTarget(GameObject target)
	{
		WarpPortal component = target.GetComponent<WarpPortal>();
		if (component == null)
		{
			global::Debug.LogError("Target doesn't have a WarpPortal associated with it.");
			return;
		}
		this.target = component;
		target.GetComponent<Assignable>().OnAssign += new Action<IAssignableIdentity>(this.Refresh);
		this.Refresh(null);
	}

	// Token: 0x060064A3 RID: 25763 RVA: 0x002527D8 File Offset: 0x002509D8
	private void Update()
	{
		if (this.progressBar.activeSelf)
		{
			RectTransform rectTransform = this.progressBar.GetComponentsInChildren<Image>()[1].rectTransform;
			float num = this.target.rechargeProgress / 3000f;
			rectTransform.sizeDelta = new Vector2(rectTransform.transform.parent.GetComponent<LayoutElement>().minWidth * num, 24f);
			this.progressLabel.text = GameUtil.GetFormattedPercent(num * 100f, GameUtil.TimeSlice.None);
		}
	}

	// Token: 0x060064A4 RID: 25764 RVA: 0x00252854 File Offset: 0x00250A54
	private void OnButtonClick()
	{
		if (this.target.ReadyToWarp)
		{
			this.target.StartWarpSequence();
			this.Refresh(null);
		}
	}

	// Token: 0x060064A5 RID: 25765 RVA: 0x00252875 File Offset: 0x00250A75
	private void OnCancelClick()
	{
		this.target.CancelAssignment();
		this.Refresh(null);
	}

	// Token: 0x060064A6 RID: 25766 RVA: 0x0025288C File Offset: 0x00250A8C
	private void Refresh(object data = null)
	{
		this.progressBar.SetActive(false);
		this.cancelButton.gameObject.SetActive(false);
		if (!(this.target != null))
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
			this.button.gameObject.SetActive(false);
			return;
		}
		if (this.target.ReadyToWarp)
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.WAITING;
			this.button.gameObject.SetActive(true);
			this.cancelButton.gameObject.SetActive(true);
			return;
		}
		if (this.target.IsConsumed)
		{
			this.button.gameObject.SetActive(false);
			this.progressBar.SetActive(true);
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.CONSUMED;
			return;
		}
		if (this.target.IsWorking)
		{
			this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.UNDERWAY;
			this.button.gameObject.SetActive(false);
			this.cancelButton.gameObject.SetActive(true);
			return;
		}
		this.label.text = UI.UISIDESCREENS.WARPPORTALSIDESCREEN.IDLE;
		this.button.gameObject.SetActive(false);
	}

	// Token: 0x040044AA RID: 17578
	[SerializeField]
	private LocText label;

	// Token: 0x040044AB RID: 17579
	[SerializeField]
	private KButton button;

	// Token: 0x040044AC RID: 17580
	[SerializeField]
	private LocText buttonLabel;

	// Token: 0x040044AD RID: 17581
	[SerializeField]
	private KButton cancelButton;

	// Token: 0x040044AE RID: 17582
	[SerializeField]
	private LocText cancelButtonLabel;

	// Token: 0x040044AF RID: 17583
	[SerializeField]
	private WarpPortal target;

	// Token: 0x040044B0 RID: 17584
	[SerializeField]
	private GameObject contents;

	// Token: 0x040044B1 RID: 17585
	[SerializeField]
	private GameObject progressBar;

	// Token: 0x040044B2 RID: 17586
	[SerializeField]
	private LocText progressLabel;
}
