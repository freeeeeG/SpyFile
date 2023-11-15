using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000151 RID: 337
public class UI_Button_ShowPlayerDeck : MonoBehaviour
{
	// Token: 0x060008D5 RID: 2261 RVA: 0x00021B3C File Offset: 0x0001FD3C
	private void OnEnable()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x060008D6 RID: 2262 RVA: 0x00021B5A File Offset: 0x0001FD5A
	private void OnDisable()
	{
		this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x060008D7 RID: 2263 RVA: 0x00021B78 File Offset: 0x0001FD78
	private void OnClickButton()
	{
		APopupWindow.CreateWindow<UI_DeckListPopup>(APopupWindow.ePopupWindowLayer.TOP, Singleton<UIManager>.Instance.PopupUIAnchor_TopLevel, false);
	}

	// Token: 0x04000711 RID: 1809
	[SerializeField]
	private Button button;
}
