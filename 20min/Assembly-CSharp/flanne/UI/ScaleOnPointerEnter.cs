using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace flanne.UI
{
	// Token: 0x0200022F RID: 559
	public class ScaleOnPointerEnter : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06000C57 RID: 3159 RVA: 0x0002D71B File Offset: 0x0002B91B
		public void Awake()
		{
			this._originalScale = base.transform.localScale;
		}

		// Token: 0x06000C58 RID: 3160 RVA: 0x0002D72E File Offset: 0x0002B92E
		public void OnPointerEnter(PointerEventData pointerEventData)
		{
			base.transform.localScale = this.scaleTo;
		}

		// Token: 0x06000C59 RID: 3161 RVA: 0x0002D741 File Offset: 0x0002B941
		public void OnPointerExit(PointerEventData pointerEventData)
		{
			base.transform.localScale = this._originalScale;
		}

		// Token: 0x040008A2 RID: 2210
		[SerializeField]
		private Vector3 scaleTo;

		// Token: 0x040008A3 RID: 2211
		private Vector3 _originalScale;
	}
}
