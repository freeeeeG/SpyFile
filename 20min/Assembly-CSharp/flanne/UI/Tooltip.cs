using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace flanne.UI
{
	// Token: 0x02000239 RID: 569
	public class Tooltip : MonoBehaviour
	{
		// Token: 0x06000C83 RID: 3203 RVA: 0x0002DD38 File Offset: 0x0002BF38
		private void Awake()
		{
			if (Tooltip.Instance == null)
			{
				Tooltip.Instance = this;
			}
			else if (Tooltip.Instance != this)
			{
				Object.Destroy(base.gameObject);
			}
			this.HideTooltip();
			this.rectTransform = base.GetComponent<RectTransform>();
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0002DD84 File Offset: 0x0002BF84
		private void Update()
		{
			Vector2 vector = Mouse.current.position.ReadValue();
			Vector2 v;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvas.transform as RectTransform, vector, this.canvas.worldCamera, out v);
			this.rectTransform.position = this.canvas.transform.TransformPoint(v);
			if (vector.x > (float)(Screen.width / 2))
			{
				this.backgroundRectTransform.pivot = new Vector2(1f, 0f);
				return;
			}
			this.backgroundRectTransform.pivot = new Vector2(0f, 0f);
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0002DE2C File Offset: 0x0002C02C
		public void ShowTooltip(string tooltipString)
		{
			this.cg.alpha = 1f;
			this.tooltipText.text = tooltipString;
			float num = 8f;
			float num2;
			if (this.tooltipText.preferredWidth < ((RectTransform)this.tooltipText.transform).rect.width)
			{
				num2 = this.tooltipText.preferredWidth;
			}
			else
			{
				num2 = ((RectTransform)this.tooltipText.transform).rect.width;
			}
			Vector2 sizeDelta = new Vector2(num2 + num * 2f, this.tooltipText.preferredHeight + num * 2f);
			this.backgroundRectTransform.sizeDelta = sizeDelta;
			this.Update();
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0002DEEC File Offset: 0x0002C0EC
		public void HideTooltip()
		{
			this.cg.alpha = 0f;
		}

		// Token: 0x040008C3 RID: 2243
		public static Tooltip Instance;

		// Token: 0x040008C4 RID: 2244
		[SerializeField]
		private Camera uiCamera;

		// Token: 0x040008C5 RID: 2245
		[SerializeField]
		private TMP_Text tooltipText;

		// Token: 0x040008C6 RID: 2246
		[SerializeField]
		private RectTransform backgroundRectTransform;

		// Token: 0x040008C7 RID: 2247
		[SerializeField]
		private CanvasGroup cg;

		// Token: 0x040008C8 RID: 2248
		[SerializeField]
		private Canvas canvas;

		// Token: 0x040008C9 RID: 2249
		private RectTransform rectTransform;
	}
}
