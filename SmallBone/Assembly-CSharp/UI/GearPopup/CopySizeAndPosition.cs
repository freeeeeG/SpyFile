using System;
using UnityEngine;

namespace UI.GearPopup
{
	// Token: 0x0200044B RID: 1099
	public class CopySizeAndPosition : MonoBehaviour
	{
		// Token: 0x060014E0 RID: 5344 RVA: 0x000412C0 File Offset: 0x0003F4C0
		private void Update()
		{
			this._rectTransform.sizeDelta = this._targetTransform.sizeDelta;
			this._rectTransform.position = this._targetTransform.position;
		}

		// Token: 0x04001221 RID: 4641
		[SerializeField]
		private RectTransform _rectTransform;

		// Token: 0x04001222 RID: 4642
		[SerializeField]
		private RectTransform _targetTransform;
	}
}
