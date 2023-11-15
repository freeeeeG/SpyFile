using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000155 RID: 341
public class UI_CheckBackToTitlePopup : APopupWindow
{
	// Token: 0x060008EE RID: 2286 RVA: 0x00021E98 File Offset: 0x00020098
	protected override void CloseWindowProc()
	{
		this.button_OK.onClick.RemoveListener(new UnityAction(this.OnButtonOKClick));
		this.button_Cancel.onClick.RemoveListener(new UnityAction(this.OnButtonCancelClick));
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x060008EF RID: 2287 RVA: 0x00021EF0 File Offset: 0x000200F0
	protected override void ShowWindowProc()
	{
		this.button_OK.onClick.AddListener(new UnityAction(this.OnButtonOKClick));
		this.button_Cancel.onClick.AddListener(new UnityAction(this.OnButtonCancelClick));
		this.animator.SetBool("isOn", true);
	}

	// Token: 0x060008F0 RID: 2288 RVA: 0x00021F46 File Offset: 0x00020146
	public void RegisterResultCallback(Action<bool> callback)
	{
		this.OnResultCallback = (Action<bool>)Delegate.Combine(this.OnResultCallback, callback);
	}

	// Token: 0x060008F1 RID: 2289 RVA: 0x00021F5F File Offset: 0x0002015F
	private void OnButtonOKClick()
	{
		this.result = true;
		this.OnResultCallback(this.result);
		base.CloseWindow();
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x00021F7F File Offset: 0x0002017F
	private void OnButtonCancelClick()
	{
		this.result = false;
		this.OnResultCallback(this.result);
		base.CloseWindow();
	}

	// Token: 0x04000716 RID: 1814
	[SerializeField]
	private Button button_OK;

	// Token: 0x04000717 RID: 1815
	[SerializeField]
	private Button button_Cancel;

	// Token: 0x04000718 RID: 1816
	private bool result;

	// Token: 0x04000719 RID: 1817
	public Action<bool> OnResultCallback;
}
