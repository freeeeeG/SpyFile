using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000156 RID: 342
public class UI_CheckStartNewGamePopup : APopupWindow
{
	// Token: 0x060008F4 RID: 2292 RVA: 0x00021FA8 File Offset: 0x000201A8
	protected override void CloseWindowProc()
	{
		this.button_OK.onClick.RemoveListener(new UnityAction(this.OnButtonOKClick));
		this.button_Cancel.onClick.RemoveListener(new UnityAction(this.OnButtonCancelClick));
		this.animator.SetBool("isOn", false);
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x00022000 File Offset: 0x00020200
	protected override void ShowWindowProc()
	{
		this.button_OK.onClick.AddListener(new UnityAction(this.OnButtonOKClick));
		this.button_Cancel.onClick.AddListener(new UnityAction(this.OnButtonCancelClick));
		this.animator.SetBool("isOn", true);
	}

	// Token: 0x060008F6 RID: 2294 RVA: 0x00022056 File Offset: 0x00020256
	public void RegisterResultCallback(Action<bool> callback)
	{
		this.OnResultCallback = (Action<bool>)Delegate.Combine(this.OnResultCallback, callback);
	}

	// Token: 0x060008F7 RID: 2295 RVA: 0x0002206F File Offset: 0x0002026F
	private void OnButtonOKClick()
	{
		this.result = true;
		this.OnResultCallback(this.result);
		base.CloseWindow();
	}

	// Token: 0x060008F8 RID: 2296 RVA: 0x0002208F File Offset: 0x0002028F
	private void OnButtonCancelClick()
	{
		this.result = false;
		this.OnResultCallback(this.result);
		base.CloseWindow();
	}

	// Token: 0x0400071A RID: 1818
	[SerializeField]
	private Button button_OK;

	// Token: 0x0400071B RID: 1819
	[SerializeField]
	private Button button_Cancel;

	// Token: 0x0400071C RID: 1820
	private bool result;

	// Token: 0x0400071D RID: 1821
	public Action<bool> OnResultCallback;
}
