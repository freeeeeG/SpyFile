using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x0200014D RID: 333
public class UI_Button_BackToTitle : MonoBehaviour
{
	// Token: 0x060008C3 RID: 2243 RVA: 0x0002196D File Offset: 0x0001FB6D
	private void OnEnable()
	{
		this.button.onClick.AddListener(new UnityAction(this.OnButtonClick));
	}

	// Token: 0x060008C4 RID: 2244 RVA: 0x0002198B File Offset: 0x0001FB8B
	private void OnDisable()
	{
		this.button.onClick.RemoveListener(new UnityAction(this.OnButtonClick));
	}

	// Token: 0x060008C5 RID: 2245 RVA: 0x000219A9 File Offset: 0x0001FBA9
	private void OnButtonClick()
	{
		DebugManager.Log(eDebugKey.UI, "按鈕: 回到標題頁", null);
		base.StartCoroutine(this.CR_BackToTitle());
	}

	// Token: 0x060008C6 RID: 2246 RVA: 0x000219C5 File Offset: 0x0001FBC5
	private IEnumerator CR_BackToTitle()
	{
		bool doBackToTitle = false;
		UI_Window_YesNo_Popup window = APopupWindow.CreateWindow<UI_Window_YesNo_Popup>(APopupWindow.ePopupWindowLayer.TOP, null, false);
		window.SetupContent("BACK_TO_TITLE_CONFIRM", "BACK_TO_TITLE_YES", "BACK_TO_TITLE_NO", delegate(bool result)
		{
			doBackToTitle = result;
		});
		while (!window.IsWindowFinished)
		{
			yield return null;
		}
		if (doBackToTitle)
		{
			EventMgr.SendEvent(eGameEvents.UI_TriggerTransition_Show);
			yield return new WaitForSeconds(1f);
			SceneManager.LoadScene("CoinPage");
		}
		yield break;
	}

	// Token: 0x0400070A RID: 1802
	[SerializeField]
	private Button button;
}
