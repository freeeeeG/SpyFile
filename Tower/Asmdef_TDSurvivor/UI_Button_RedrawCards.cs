using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000150 RID: 336
public class UI_Button_RedrawCards : MonoBehaviour
{
	// Token: 0x060008D1 RID: 2257 RVA: 0x00021AE9 File Offset: 0x0001FCE9
	private void OnEnable()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x060008D2 RID: 2258 RVA: 0x00021B07 File Offset: 0x0001FD07
	private void OnDisable()
	{
		this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x060008D3 RID: 2259 RVA: 0x00021B25 File Offset: 0x0001FD25
	private void OnClickButton()
	{
		EventMgr.SendEvent(eGameEvents.RequestRedrawCards);
	}

	// Token: 0x0400070E RID: 1806
	[SerializeField]
	private Button button;

	// Token: 0x0400070F RID: 1807
	[SerializeField]
	private TMP_Text text_Cost;

	// Token: 0x04000710 RID: 1808
	[SerializeField]
	private CanvasGroup canvasGroup;
}
