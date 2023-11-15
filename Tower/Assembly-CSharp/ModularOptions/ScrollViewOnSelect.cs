using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ModularOptions
{
	// Token: 0x020000B3 RID: 179
	[AddComponentMenu("Modular Options/Scroll View On Select")]
	[RequireComponent(typeof(Selectable))]
	public class ScrollViewOnSelect : MonoBehaviour, ISelectHandler, IEventSystemHandler
	{
		// Token: 0x0600026A RID: 618 RVA: 0x00009624 File Offset: 0x00007824
		public void OnSelect(BaseEventData _eventData)
		{
			Rect rect = this.scrollRect.content.rect;
			RectTransform viewport = this.scrollRect.viewport;
			Scrollbar scrollbar = this.scrollRect.verticalScrollbar;
			if (scrollbar != null && rect.height > viewport.rect.height)
			{
				float num = this.rectToScrollTo.position.y + this.rectToScrollTo.rect.yMax * this.rectToScrollTo.lossyScale.y;
				float num2 = viewport.position.y + viewport.rect.yMax * viewport.lossyScale.y;
				if (num > num2)
				{
					float num3 = num - num2;
					scrollbar.value += num3 * 1f / (1f - scrollbar.size) / viewport.lossyScale.y / rect.height;
				}
				else
				{
					float num4 = this.rectToScrollTo.position.y + this.rectToScrollTo.rect.yMin * this.rectToScrollTo.lossyScale.y;
					float num5 = viewport.position.y + viewport.rect.yMin * viewport.lossyScale.y;
					if (num4 < num5)
					{
						float num6 = num4 - num5;
						scrollbar.value += num6 * 1f / (1f - scrollbar.size) / viewport.lossyScale.y / rect.height;
					}
				}
			}
			scrollbar = this.scrollRect.horizontalScrollbar;
			if (scrollbar != null && rect.width > viewport.rect.width)
			{
				float num7 = this.rectToScrollTo.position.x + this.rectToScrollTo.rect.xMax * this.rectToScrollTo.lossyScale.x;
				float num8 = viewport.position.x + viewport.rect.xMax * viewport.lossyScale.x;
				if (num7 > num8)
				{
					float num9 = num7 - num8;
					scrollbar.value += num9 * 1f / (1f - scrollbar.size) / viewport.lossyScale.x / rect.width;
					return;
				}
				float num10 = this.rectToScrollTo.position.x + this.rectToScrollTo.rect.xMin * this.rectToScrollTo.lossyScale.x;
				float num11 = viewport.position.x + viewport.rect.xMin * viewport.lossyScale.x;
				if (num10 < num11)
				{
					float num12 = num10 - num11;
					scrollbar.value += num12 * 1f / (1f - scrollbar.size) / viewport.lossyScale.x / rect.width;
				}
			}
		}

		// Token: 0x040001FD RID: 509
		[Tooltip("When selected the view will scroll to show this rect. Defaults to itself, but can be changed to include things like labels.")]
		public RectTransform rectToScrollTo;

		// Token: 0x040001FE RID: 510
		[Tooltip("ScrollRect viewport that will be scrolled.")]
		public ScrollRect scrollRect;
	}
}
