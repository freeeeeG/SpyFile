using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace flanne.UI
{
	// Token: 0x02000234 RID: 564
	public class SpriteSwapOnPointerEnter : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		// Token: 0x06000C65 RID: 3173 RVA: 0x0002D7E3 File Offset: 0x0002B9E3
		public void Awake()
		{
			this._originalSprite = this.targetImage.sprite;
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0002D7F6 File Offset: 0x0002B9F6
		public void OnPointerEnter(PointerEventData pointerEventData)
		{
			this.targetImage.sprite = this.mouseOverSprite;
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0002D809 File Offset: 0x0002BA09
		public void OnPointerExit(PointerEventData pointerEventData)
		{
			this.targetImage.sprite = this._originalSprite;
		}

		// Token: 0x040008AA RID: 2218
		[SerializeField]
		private Image targetImage;

		// Token: 0x040008AB RID: 2219
		[SerializeField]
		private Sprite mouseOverSprite;

		// Token: 0x040008AC RID: 2220
		private Sprite _originalSprite;
	}
}
