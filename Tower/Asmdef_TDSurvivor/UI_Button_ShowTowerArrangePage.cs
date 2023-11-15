using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000153 RID: 339
public class UI_Button_ShowTowerArrangePage : MonoBehaviour
{
	// Token: 0x060008DD RID: 2269 RVA: 0x00021BE3 File Offset: 0x0001FDE3
	private void OnEnable()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnClickButton_ShowTowerArrangePage));
	}

	// Token: 0x060008DE RID: 2270 RVA: 0x00021C01 File Offset: 0x0001FE01
	private void OnDisable()
	{
		this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton_ShowTowerArrangePage));
	}

	// Token: 0x060008DF RID: 2271 RVA: 0x00021C1F File Offset: 0x0001FE1F
	private void OnClickButton_ShowTowerArrangePage()
	{
		APopupWindow.CreateWindow<UI_TowerArrange_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false);
	}

	// Token: 0x04000713 RID: 1811
	[SerializeField]
	private Button button;
}
