using System;
using UnityEngine;

// Token: 0x0200014A RID: 330
public class UI_Button_AddRandomTetris : MonoBehaviour
{
	// Token: 0x060008AD RID: 2221 RVA: 0x000215B0 File Offset: 0x0001F7B0
	private void OnEnable()
	{
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnButtonDown = (Action)Delegate.Combine(ui_HoldableButton.OnButtonDown, new Action(this.OnButtonDown));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnHoldButton = (Action)Delegate.Combine(ui_HoldableButton2.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonUp = (Action)Delegate.Combine(ui_HoldableButton3.OnButtonUp, new Action(this.OnButtonUp));
	}

	// Token: 0x060008AE RID: 2222 RVA: 0x00021634 File Offset: 0x0001F834
	private void OnDisable()
	{
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnButtonDown = (Action)Delegate.Remove(ui_HoldableButton.OnButtonDown, new Action(this.OnButtonDown));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnHoldButton = (Action)Delegate.Remove(ui_HoldableButton2.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonUp = (Action)Delegate.Remove(ui_HoldableButton3.OnButtonUp, new Action(this.OnButtonUp));
	}

	// Token: 0x060008AF RID: 2223 RVA: 0x000216B6 File Offset: 0x0001F8B6
	private void OnButtonDown()
	{
	}

	// Token: 0x060008B0 RID: 2224 RVA: 0x000216B8 File Offset: 0x0001F8B8
	private void OnHoldButton()
	{
		this.InitiatePlacement();
	}

	// Token: 0x060008B1 RID: 2225 RVA: 0x000216C0 File Offset: 0x0001F8C0
	private void OnButtonUp()
	{
		this.InitiatePlacement();
	}

	// Token: 0x060008B2 RID: 2226 RVA: 0x000216C8 File Offset: 0x0001F8C8
	private void InitiatePlacement()
	{
	}

	// Token: 0x04000704 RID: 1796
	[SerializeField]
	private UI_HoldableButton button;
}
