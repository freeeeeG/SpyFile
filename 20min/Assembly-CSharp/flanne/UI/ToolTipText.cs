using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace flanne.UI
{
	// Token: 0x02000238 RID: 568
	public class ToolTipText : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06000C7F RID: 3199 RVA: 0x0002DCE4 File Offset: 0x0002BEE4
		private void Start()
		{
			this.TP = Tooltip.Instance;
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0002DCF1 File Offset: 0x0002BEF1
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.setToolTip)
			{
				this.TP.ShowTooltip(LocalizationSystem.GetLocalizedValue(this.tooltipString.key));
				return;
			}
			this.TP.ShowTooltip(this.tooltip);
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0002DD28 File Offset: 0x0002BF28
		public void OnPointerExit(PointerEventData eventData)
		{
			this.TP.HideTooltip();
		}

		// Token: 0x040008BF RID: 2239
		public bool setToolTip;

		// Token: 0x040008C0 RID: 2240
		[SerializeField]
		private LocalizedString tooltipString;

		// Token: 0x040008C1 RID: 2241
		[NonSerialized]
		public string tooltip;

		// Token: 0x040008C2 RID: 2242
		private Tooltip TP;
	}
}
