using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	// Token: 0x020003CD RID: 973
	[RequireComponent(typeof(RectTransform), typeof(ILayoutElement))]
	public class SizeSetter : MonoBehaviour
	{
		// Token: 0x0600121E RID: 4638 RVA: 0x000356A8 File Offset: 0x000338A8
		private void OnEnable()
		{
			this._rectTransform = base.GetComponent<RectTransform>();
			this._layoutElement = base.GetComponent<ILayoutElement>();
		}

		// Token: 0x0600121F RID: 4639 RVA: 0x000356C4 File Offset: 0x000338C4
		private void Update()
		{
			Vector2 sizeDelta = this._rectTransform.sizeDelta;
			switch (this._widthSizing)
			{
			case SizeSetter.SizingMethod.Min:
				sizeDelta.x = this._layoutElement.minWidth;
				break;
			case SizeSetter.SizingMethod.Preferred:
				sizeDelta.x = this._layoutElement.preferredWidth;
				break;
			case SizeSetter.SizingMethod.Flexible:
				sizeDelta.x = this._layoutElement.flexibleWidth;
				break;
			}
			switch (this._heightSizing)
			{
			case SizeSetter.SizingMethod.Min:
				sizeDelta.y = this._layoutElement.minHeight;
				break;
			case SizeSetter.SizingMethod.Preferred:
				sizeDelta.y = this._layoutElement.preferredHeight;
				break;
			case SizeSetter.SizingMethod.Flexible:
				sizeDelta.y = this._layoutElement.flexibleHeight;
				break;
			}
			this._rectTransform.sizeDelta = sizeDelta;
		}

		// Token: 0x04000F00 RID: 3840
		[SerializeField]
		private SizeSetter.SizingMethod _widthSizing;

		// Token: 0x04000F01 RID: 3841
		[SerializeField]
		private SizeSetter.SizingMethod _heightSizing;

		// Token: 0x04000F02 RID: 3842
		private RectTransform _rectTransform;

		// Token: 0x04000F03 RID: 3843
		private ILayoutElement _layoutElement;

		// Token: 0x020003CE RID: 974
		private enum SizingMethod
		{
			// Token: 0x04000F05 RID: 3845
			None,
			// Token: 0x04000F06 RID: 3846
			Min,
			// Token: 0x04000F07 RID: 3847
			Preferred,
			// Token: 0x04000F08 RID: 3848
			Flexible
		}
	}
}
