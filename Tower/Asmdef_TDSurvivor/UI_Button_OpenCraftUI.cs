using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x0200014E RID: 334
public class UI_Button_OpenCraftUI : MonoBehaviour
{
	// Token: 0x060008C8 RID: 2248 RVA: 0x000219D5 File Offset: 0x0001FBD5
	private void OnEnable()
	{
		EventMgr.Register<bool>(eGameEvents.UI_ToggleCraftTowerUI, new Action<bool>(this.OnToggleCraftTowerUI));
		this.button.onClick.AddListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x060008C9 RID: 2249 RVA: 0x00021A0E File Offset: 0x0001FC0E
	private void OnDisable()
	{
		EventMgr.Remove<bool>(eGameEvents.UI_ToggleCraftTowerUI, new Action<bool>(this.OnToggleCraftTowerUI));
		this.button.onClick.RemoveListener(new UnityAction(this.OnClickButton));
	}

	// Token: 0x060008CA RID: 2250 RVA: 0x00021A47 File Offset: 0x0001FC47
	private void OnToggleCraftTowerUI(bool isOn)
	{
		this.canvasGroup.alpha = (isOn ? 0f : 1f);
		this.canvasGroup.blocksRaycasts = !isOn;
	}

	// Token: 0x060008CB RID: 2251 RVA: 0x00021A72 File Offset: 0x0001FC72
	private void OnClickButton()
	{
		EventMgr.SendEvent<bool>(eGameEvents.UI_ToggleCraftTowerUI, true);
	}

	// Token: 0x0400070B RID: 1803
	[SerializeField]
	private Button button;

	// Token: 0x0400070C RID: 1804
	[SerializeField]
	private CanvasGroup canvasGroup;
}
