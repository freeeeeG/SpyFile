using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000191 RID: 401
public class UI_WelcomeToDemo : AUISituational
{
	// Token: 0x06000ABC RID: 2748 RVA: 0x0002858A File Offset: 0x0002678A
	private void OnEnable()
	{
		EventMgr.Register(eGameEvents.UI_ShowWelcomeToDemoUI, new Action(this.OnShowWelcomeToDemoUI));
		this.button_OK.onClick.AddListener(new UnityAction(this.OnClickButton_OK));
	}

	// Token: 0x06000ABD RID: 2749 RVA: 0x000285C3 File Offset: 0x000267C3
	private void OnDisable()
	{
		EventMgr.Remove(eGameEvents.UI_ShowWelcomeToDemoUI, new Action(this.OnShowWelcomeToDemoUI));
		this.button_OK.onClick.RemoveListener(new UnityAction(this.OnClickButton_OK));
	}

	// Token: 0x06000ABE RID: 2750 RVA: 0x000285FC File Offset: 0x000267FC
	private void OnShowWelcomeToDemoUI()
	{
		SoundManager.PlaySound("UI", "InfoPopupWindow", -1f, -1f, -1f);
		base.Toggle(true);
	}

	// Token: 0x06000ABF RID: 2751 RVA: 0x00028624 File Offset: 0x00026824
	private void OnClickButton_OK()
	{
		SoundManager.PlaySound("UI", "CommonButton_Click", -1f, -1f, -1f);
		base.Toggle(false);
	}

	// Token: 0x0400083D RID: 2109
	[SerializeField]
	private Button button_OK;
}
