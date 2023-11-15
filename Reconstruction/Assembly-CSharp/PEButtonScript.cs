using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000007 RID: 7
public class PEButtonScript : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
{
	// Token: 0x0600004B RID: 75 RVA: 0x0000388E File Offset: 0x00001A8E
	private void Start()
	{
		this.myButton = base.gameObject.GetComponent<Button>();
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000038A1 File Offset: 0x00001AA1
	public void OnPointerEnter(PointerEventData eventData)
	{
		UICanvasManager.GlobalAccess.MouseOverButton = true;
		UICanvasManager.GlobalAccess.UpdateToolTip(this.ButtonType);
	}

	// Token: 0x0600004D RID: 77 RVA: 0x000038BE File Offset: 0x00001ABE
	public void OnPointerExit(PointerEventData eventData)
	{
		UICanvasManager.GlobalAccess.MouseOverButton = false;
		UICanvasManager.GlobalAccess.ClearToolTip();
	}

	// Token: 0x0600004E RID: 78 RVA: 0x000038D5 File Offset: 0x00001AD5
	public void OnButtonClicked()
	{
		UICanvasManager.GlobalAccess.UIButtonClick(this.ButtonType);
	}

	// Token: 0x04000028 RID: 40
	private Button myButton;

	// Token: 0x04000029 RID: 41
	public ButtonTypes ButtonType;
}
