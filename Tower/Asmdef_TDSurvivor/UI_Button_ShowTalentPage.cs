using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000152 RID: 338
public class UI_Button_ShowTalentPage : MonoBehaviour
{
	// Token: 0x060008D9 RID: 2265 RVA: 0x00021B94 File Offset: 0x0001FD94
	private void OnEnable()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnClickButton_ShowTalentPage));
	}

	// Token: 0x060008DA RID: 2266 RVA: 0x00021BB2 File Offset: 0x0001FDB2
	private void OnDisable()
	{
		this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton_ShowTalentPage));
	}

	// Token: 0x060008DB RID: 2267 RVA: 0x00021BD0 File Offset: 0x0001FDD0
	private void OnClickButton_ShowTalentPage()
	{
		APopupWindow.CreateWindow<UI_TalentPage_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false);
	}

	// Token: 0x04000712 RID: 1810
	[SerializeField]
	private Button button;
}
