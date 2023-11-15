using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200014F RID: 335
public class UI_Button_OpenSettingPage : MonoBehaviour
{
	// Token: 0x060008CD RID: 2253 RVA: 0x00021A8D File Offset: 0x0001FC8D
	private void OnEnable()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
	}

	// Token: 0x060008CE RID: 2254 RVA: 0x00021AAB File Offset: 0x0001FCAB
	private void OnDisable()
	{
		this.button.onClick.RemoveListener(new UnityAction(this.OnButtonClick));
	}

	// Token: 0x060008CF RID: 2255 RVA: 0x00021AC9 File Offset: 0x0001FCC9
	private void OnButtonClick()
	{
		DebugManager.Log(eDebugKey.UI, "按鈕: 開啟設定頁", null);
		APopupWindow.CreateWindow<UI_SettingPage_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false);
	}

	// Token: 0x0400070D RID: 1805
	[SerializeField]
	private Button button;
}
