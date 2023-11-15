using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ETFXPEL
{
	// Token: 0x0200005C RID: 92
	public class PEButtonScript : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		// Token: 0x06000114 RID: 276 RVA: 0x00005CCA File Offset: 0x00003ECA
		private void Start()
		{
			this.myButton = base.gameObject.GetComponent<Button>();
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00005CDD File Offset: 0x00003EDD
		public void OnPointerEnter(PointerEventData eventData)
		{
			UICanvasManager.GlobalAccess.MouseOverButton = true;
			UICanvasManager.GlobalAccess.UpdateToolTip(this.ButtonType);
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00005CFA File Offset: 0x00003EFA
		public void OnPointerExit(PointerEventData eventData)
		{
			UICanvasManager.GlobalAccess.MouseOverButton = false;
			UICanvasManager.GlobalAccess.ClearToolTip();
		}

		// Token: 0x06000117 RID: 279 RVA: 0x00005D11 File Offset: 0x00003F11
		public void OnButtonClicked()
		{
			UICanvasManager.GlobalAccess.UIButtonClick(this.ButtonType);
		}

		// Token: 0x04000118 RID: 280
		private Button myButton;

		// Token: 0x04000119 RID: 281
		public ButtonTypes ButtonType;
	}
}
