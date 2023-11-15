using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000154 RID: 340
public class UI_Button_StartWave : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x060008E1 RID: 2273 RVA: 0x00021C34 File Offset: 0x0001FE34
	private void OnEnable()
	{
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnButtonDown = (Action)Delegate.Combine(ui_HoldableButton.OnButtonDown, new Action(this.OnButtonDown));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnHoldButton = (Action)Delegate.Combine(ui_HoldableButton2.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonUp = (Action)Delegate.Combine(ui_HoldableButton3.OnButtonUp, new Action(this.OnButtonUp));
		EventMgr.Register<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Register(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
		EventMgr.Register(eGameEvents.OnBattleEnd, new Action(this.OnBattleEnd));
		EventMgr.Register(eGameEvents.OnPlayerVictory, new Action(this.OnPlayerVictory));
	}

	// Token: 0x060008E2 RID: 2274 RVA: 0x00021D18 File Offset: 0x0001FF18
	private void OnDisable()
	{
		UI_HoldableButton ui_HoldableButton = this.button;
		ui_HoldableButton.OnButtonDown = (Action)Delegate.Remove(ui_HoldableButton.OnButtonDown, new Action(this.OnButtonDown));
		UI_HoldableButton ui_HoldableButton2 = this.button;
		ui_HoldableButton2.OnHoldButton = (Action)Delegate.Remove(ui_HoldableButton2.OnHoldButton, new Action(this.OnHoldButton));
		UI_HoldableButton ui_HoldableButton3 = this.button;
		ui_HoldableButton3.OnButtonUp = (Action)Delegate.Remove(ui_HoldableButton3.OnButtonUp, new Action(this.OnButtonUp));
		EventMgr.Remove<int, int>(eGameEvents.OnRoundStart, new Action<int, int>(this.OnRoundStart));
		EventMgr.Remove(eGameEvents.OnBattleStart, new Action(this.OnBattleStart));
		EventMgr.Remove(eGameEvents.OnBattleEnd, new Action(this.OnBattleEnd));
		EventMgr.Remove(eGameEvents.OnPlayerVictory, new Action(this.OnPlayerVictory));
	}

	// Token: 0x060008E3 RID: 2275 RVA: 0x00021DFA File Offset: 0x0001FFFA
	private void OnRoundStart(int index, int totalRound)
	{
		if (index == 1)
		{
			this.animator.SetBool("isOn", true);
		}
	}

	// Token: 0x060008E4 RID: 2276 RVA: 0x00021E11 File Offset: 0x00020011
	private void OnBattleStart()
	{
		this.Toggle(false);
	}

	// Token: 0x060008E5 RID: 2277 RVA: 0x00021E1A File Offset: 0x0002001A
	private void OnBattleEnd()
	{
		this.Toggle(true);
	}

	// Token: 0x060008E6 RID: 2278 RVA: 0x00021E23 File Offset: 0x00020023
	private void OnPlayerVictory()
	{
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x060008E7 RID: 2279 RVA: 0x00021E36 File Offset: 0x00020036
	private void OnButtonDown()
	{
		EventMgr.SendEvent(eGameEvents.CancelPlacement);
		EventMgr.SendEvent(eGameEvents.RequestStartNextWave);
	}

	// Token: 0x060008E8 RID: 2280 RVA: 0x00021E52 File Offset: 0x00020052
	private void OnHoldButton()
	{
	}

	// Token: 0x060008E9 RID: 2281 RVA: 0x00021E54 File Offset: 0x00020054
	private void OnButtonUp()
	{
	}

	// Token: 0x060008EA RID: 2282 RVA: 0x00021E56 File Offset: 0x00020056
	private void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x060008EB RID: 2283 RVA: 0x00021E69 File Offset: 0x00020069
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.animator.SetBool("shake", true);
	}

	// Token: 0x060008EC RID: 2284 RVA: 0x00021E7C File Offset: 0x0002007C
	public void OnPointerExit(PointerEventData eventData)
	{
		this.animator.SetBool("shake", false);
	}

	// Token: 0x04000714 RID: 1812
	[SerializeField]
	private Animator animator;

	// Token: 0x04000715 RID: 1813
	[SerializeField]
	private UI_HoldableButton button;
}
