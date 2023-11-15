using System;
using UnityEngine;

// Token: 0x0200014B RID: 331
public class UI_Button_AddTetris : MonoBehaviour
{
	// Token: 0x060008B4 RID: 2228 RVA: 0x000216D4 File Offset: 0x0001F8D4
	private void OnEnable()
	{
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnButtonDown = (Action)Delegate.Combine(ui_HoldableButton.OnButtonDown, new Action(this.OnButtonDown));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnHoldButton = (Action)Delegate.Combine(ui_HoldableButton2.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonUp = (Action)Delegate.Combine(ui_HoldableButton3.OnButtonUp, new Action(this.OnButtonUp));
	}

	// Token: 0x060008B5 RID: 2229 RVA: 0x00021758 File Offset: 0x0001F958
	private void OnDisable()
	{
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnButtonDown = (Action)Delegate.Remove(ui_HoldableButton.OnButtonDown, new Action(this.OnButtonDown));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnHoldButton = (Action)Delegate.Remove(ui_HoldableButton2.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonUp = (Action)Delegate.Remove(ui_HoldableButton3.OnButtonUp, new Action(this.OnButtonUp));
	}

	// Token: 0x060008B6 RID: 2230 RVA: 0x000217DA File Offset: 0x0001F9DA
	private void OnButtonDown()
	{
	}

	// Token: 0x060008B7 RID: 2231 RVA: 0x000217DC File Offset: 0x0001F9DC
	private void OnHoldButton()
	{
		this.InitiatePlacement();
	}

	// Token: 0x060008B8 RID: 2232 RVA: 0x000217E4 File Offset: 0x0001F9E4
	private void OnButtonUp()
	{
		this.InitiatePlacement();
	}

	// Token: 0x060008B9 RID: 2233 RVA: 0x000217EC File Offset: 0x0001F9EC
	private void InitiatePlacement()
	{
	}

	// Token: 0x04000705 RID: 1797
	[SerializeField]
	private eTetrisType tetrisType;

	// Token: 0x04000706 RID: 1798
	[SerializeField]
	private UI_HoldableButton button;
}
