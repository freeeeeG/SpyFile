using System;
using UnityEngine;

namespace UI.Inventory
{
	// Token: 0x02000444 RID: 1092
	public class SkillFrameSizeFitter : MonoBehaviour
	{
		// Token: 0x060014C3 RID: 5315 RVA: 0x00040854 File Offset: 0x0003EA54
		private void Awake()
		{
			Vector2 sizeDelta = this._rectTransform.sizeDelta;
			this._defaultHeight = sizeDelta.y;
			this._scale = base.transform.localScale.y;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00040890 File Offset: 0x0003EA90
		private void Update()
		{
			float y = this._targetRectTransform.sizeDelta.y;
			float num = 0f;
			if (y > this._startHeight)
			{
				num = (y - this._startHeight) / this._scale;
			}
			Vector2 sizeDelta = this._rectTransform.sizeDelta;
			sizeDelta.y = this._defaultHeight + num;
			this._rectTransform.sizeDelta = sizeDelta;
		}

		// Token: 0x040011DB RID: 4571
		[SerializeField]
		private RectTransform _rectTransform;

		// Token: 0x040011DC RID: 4572
		[SerializeField]
		private RectTransform _targetRectTransform;

		// Token: 0x040011DD RID: 4573
		[SerializeField]
		private float _startHeight;

		// Token: 0x040011DE RID: 4574
		private float _scale;

		// Token: 0x040011DF RID: 4575
		private float _defaultHeight;
	}
}
