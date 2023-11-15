using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200015F RID: 351
public class UI_Defeat_Popup : APopupWindow
{
	// Token: 0x06000931 RID: 2353 RVA: 0x00022EBF File Offset: 0x000210BF
	private void OnEnable()
	{
		this.button_BackToTitle.onClick.AddListener(new UnityAction(this.OnClick_BackToTitle));
	}

	// Token: 0x06000932 RID: 2354 RVA: 0x00022EDD File Offset: 0x000210DD
	private void OnDisable()
	{
		this.button_BackToTitle.onClick.RemoveListener(new UnityAction(this.OnClick_BackToTitle));
	}

	// Token: 0x06000933 RID: 2355 RVA: 0x00022EFB File Offset: 0x000210FB
	private void OnClick_BackToTitle()
	{
		this.isButtonPressed = true;
	}

	// Token: 0x06000934 RID: 2356 RVA: 0x00022F04 File Offset: 0x00021104
	private void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
	}

	// Token: 0x06000935 RID: 2357 RVA: 0x00022F17 File Offset: 0x00021117
	protected override void ShowWindowProc()
	{
		this.Toggle(true);
		SoundManager.PlaySound("UI", "Defeat", -1f, -1f, -1f);
	}

	// Token: 0x06000936 RID: 2358 RVA: 0x00022F3F File Offset: 0x0002113F
	protected override void CloseWindowProc()
	{
	}

	// Token: 0x06000937 RID: 2359 RVA: 0x00022F41 File Offset: 0x00021141
	private IEnumerator CR_Proc()
	{
		this.Toggle(true);
		yield return new WaitForSeconds(1.5f);
		this.animator.SetTrigger("showDetail");
		yield return new WaitForSeconds(0.66f);
		yield break;
	}

	// Token: 0x0400074D RID: 1869
	[SerializeField]
	private Button button_BackToTitle;

	// Token: 0x0400074E RID: 1870
	public bool isButtonPressed;
}
