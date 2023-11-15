using System;
using TMPro;
using UnityEngine;

// Token: 0x0200014C RID: 332
public class UI_Button_AddTower : MonoBehaviour
{
	// Token: 0x060008BB RID: 2235 RVA: 0x000217F8 File Offset: 0x0001F9F8
	private void OnEnable()
	{
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnButtonDown = (Action)Delegate.Combine(ui_HoldableButton.OnButtonDown, new Action(this.OnButtonDown));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnHoldButton = (Action)Delegate.Combine(ui_HoldableButton2.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonUp = (Action)Delegate.Combine(ui_HoldableButton3.OnButtonUp, new Action(this.OnButtonUp));
		EventMgr.Register<int>(eGameEvents.OnCoinChanged, new Action<int>(this.OnCoinChanged));
	}

	// Token: 0x060008BC RID: 2236 RVA: 0x00021894 File Offset: 0x0001FA94
	private void OnDisable()
	{
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnButtonDown = (Action)Delegate.Remove(ui_HoldableButton.OnButtonDown, new Action(this.OnButtonDown));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnHoldButton = (Action)Delegate.Remove(ui_HoldableButton2.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonUp = (Action)Delegate.Remove(ui_HoldableButton3.OnButtonUp, new Action(this.OnButtonUp));
		EventMgr.Remove<int>(eGameEvents.OnCoinChanged, new Action<int>(this.OnCoinChanged));
	}

	// Token: 0x060008BD RID: 2237 RVA: 0x0002192E File Offset: 0x0001FB2E
	private void OnCoinChanged(int coin)
	{
	}

	// Token: 0x060008BE RID: 2238 RVA: 0x00021930 File Offset: 0x0001FB30
	private void OnButtonDown()
	{
	}

	// Token: 0x060008BF RID: 2239 RVA: 0x00021932 File Offset: 0x0001FB32
	private void OnHoldButton()
	{
		Debug.Log("OnHoldButton");
		this.InitiateTowerPlacement();
	}

	// Token: 0x060008C0 RID: 2240 RVA: 0x00021944 File Offset: 0x0001FB44
	private void OnButtonUp()
	{
		Debug.Log("OnButtonUp");
		this.InitiateTowerPlacement();
	}

	// Token: 0x060008C1 RID: 2241 RVA: 0x00021956 File Offset: 0x0001FB56
	private void InitiateTowerPlacement()
	{
		EventMgr.SendEvent<eGameState>(eGameEvents.RequestChangeGameState, eGameState.EDIT_MODE);
	}

	// Token: 0x04000707 RID: 1799
	[SerializeField]
	private eItemType towerType;

	// Token: 0x04000708 RID: 1800
	[SerializeField]
	private UI_HoldableButton button;

	// Token: 0x04000709 RID: 1801
	[SerializeField]
	private TMP_Text text_Cost;
}
