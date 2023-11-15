using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000227 RID: 551
	public class MenuEntry : Button, ICancelHandler, IEventSystemHandler
	{
		// Token: 0x06000C2F RID: 3119 RVA: 0x0002CFA3 File Offset: 0x0002B1A3
		public override void OnPointerEnter(PointerEventData eventData)
		{
			base.OnPointerEnter(eventData);
			this.Select();
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x0002CFB2 File Offset: 0x0002B1B2
		public override void OnSelect(BaseEventData eventData)
		{
			base.OnSelect(eventData);
			if (this.onSelect != null)
			{
				this.onSelect.Invoke();
			}
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x0002CFCE File Offset: 0x0002B1CE
		public void OnCancel(BaseEventData eventData)
		{
			if (this.onCancel != null)
			{
				this.onCancel.Invoke();
			}
		}

		// Token: 0x04000885 RID: 2181
		public UnityEvent onSelect;

		// Token: 0x04000886 RID: 2182
		public UnityEvent onCancel;
	}
}
