using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000192 RID: 402
public class UI_Window_YesNo_Popup : APopupWindow
{
	// Token: 0x06000AC1 RID: 2753 RVA: 0x00028654 File Offset: 0x00026854
	public void SetupContent(string contentLocKey, string yesButtonLocKey, string noButtonLocKey, Action<bool> resultCallback)
	{
		this.resultCallback = resultCallback;
		this.text_Content.text = LocalizationManager.Instance.GetString("UI", contentLocKey, Array.Empty<object>());
		this.text_Button_Yes.text = LocalizationManager.Instance.GetString("UI", yesButtonLocKey, Array.Empty<object>());
		this.text_Button_No.text = LocalizationManager.Instance.GetString("UI", noButtonLocKey, Array.Empty<object>());
	}

	// Token: 0x06000AC2 RID: 2754 RVA: 0x000286C9 File Offset: 0x000268C9
	public void RegisterResultCallback(Action<bool> callback)
	{
		this.resultCallback = (Action<bool>)Delegate.Combine(this.resultCallback, callback);
	}

	// Token: 0x06000AC3 RID: 2755 RVA: 0x000286E2 File Offset: 0x000268E2
	private void Awake()
	{
		this.button_Yes.onClick.AddListener(new UnityAction(this.OnClickButton_Yes));
		this.button_No.onClick.AddListener(new UnityAction(this.OnClickButton_No));
	}

	// Token: 0x06000AC4 RID: 2756 RVA: 0x0002871C File Offset: 0x0002691C
	private void OnClickButton_Yes()
	{
		if (this.isButtonClicked)
		{
			return;
		}
		this.resultCallback(true);
		this.isButtonClicked = true;
		base.CloseWindow();
	}

	// Token: 0x06000AC5 RID: 2757 RVA: 0x00028740 File Offset: 0x00026940
	private void OnClickButton_No()
	{
		if (this.isButtonClicked)
		{
			return;
		}
		this.resultCallback(false);
		this.isButtonClicked = true;
		base.CloseWindow();
	}

	// Token: 0x06000AC6 RID: 2758 RVA: 0x00028764 File Offset: 0x00026964
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			this.OnClickButton_No();
		}
	}

	// Token: 0x06000AC7 RID: 2759 RVA: 0x00028778 File Offset: 0x00026978
	public void Toggle(bool isOn)
	{
		this.animator.SetBool("isOn", isOn);
		if (isOn)
		{
			SoundManager.PlaySound("UI", "CommonPopupWindow", -1f, -1f, -1f);
			return;
		}
		SoundManager.PlaySound("UI", "CommonHideWindow", -1f, -1f, -1f);
	}

	// Token: 0x06000AC8 RID: 2760 RVA: 0x000287D8 File Offset: 0x000269D8
	protected override void ShowWindowProc()
	{
		this.Toggle(true);
	}

	// Token: 0x06000AC9 RID: 2761 RVA: 0x000287E1 File Offset: 0x000269E1
	protected override void CloseWindowProc()
	{
		this.Toggle(false);
	}

	// Token: 0x0400083E RID: 2110
	[SerializeField]
	private TMP_Text text_Content;

	// Token: 0x0400083F RID: 2111
	[SerializeField]
	private TMP_Text text_Button_Yes;

	// Token: 0x04000840 RID: 2112
	[SerializeField]
	private TMP_Text text_Button_No;

	// Token: 0x04000841 RID: 2113
	[SerializeField]
	private Button button_Yes;

	// Token: 0x04000842 RID: 2114
	[SerializeField]
	private Button button_No;

	// Token: 0x04000843 RID: 2115
	private bool isButtonClicked;

	// Token: 0x04000844 RID: 2116
	public Action<bool> resultCallback;
}
