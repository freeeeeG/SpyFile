using System;
using Unity.Mathematics;
using UnityEngine;

namespace UI.GearPopup
{
	// Token: 0x0200044D RID: 1101
	[RequireComponent(typeof(RectTransform))]
	public class GearPopupCanvas : MonoBehaviour
	{
		// Token: 0x1700041A RID: 1050
		// (get) Token: 0x060014FE RID: 5374 RVA: 0x00041D76 File Offset: 0x0003FF76
		public GearPopup gearPopup
		{
			get
			{
				return this._gearPopup;
			}
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00041D7E File Offset: 0x0003FF7E
		private void Awake()
		{
			this.Close();
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x00041D88 File Offset: 0x0003FF88
		private void Update()
		{
			Vector2 vector = this._content.sizeDelta / 2f;
			vector.x = 474f;
			vector.x *= this._container.lossyScale.x;
			vector.y *= this._container.lossyScale.y;
			vector.x += 5f;
			vector.y += 5f;
			float num = this._canvas.sizeDelta.x * this._canvas.localScale.x;
			float num2 = this._canvas.sizeDelta.y * this._canvas.localScale.y;
			Vector3 position = this._container.position;
			position.x = math.clamp(position.x, vector.x, num - vector.x);
			position.y = math.clamp(position.y, vector.y, num2 - vector.y);
			this._container.position = position;
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00041EAC File Offset: 0x000400AC
		public void Open(Vector3 position)
		{
			this._container.gameObject.SetActive(true);
			Vector3 vector = Camera.main.WorldToViewportPoint(position);
			vector.y = Mathf.Clamp(vector.y, 0.4f, 0.6f);
			Vector2 sizeDelta = this._canvas.sizeDelta;
			sizeDelta.x *= this._canvas.localScale.x;
			sizeDelta.y *= this._canvas.localScale.y;
			Vector2 v = new Vector2(vector.x * sizeDelta.x, vector.y * sizeDelta.y);
			this._container.position = v;
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00041F66 File Offset: 0x00040166
		public void Close()
		{
			this._container.gameObject.SetActive(false);
		}

		// Token: 0x04001245 RID: 4677
		private const float _width = 474f;

		// Token: 0x04001246 RID: 4678
		private const float _minViewportY = 0.4f;

		// Token: 0x04001247 RID: 4679
		private const float _maxViewportY = 0.6f;

		// Token: 0x04001248 RID: 4680
		private const float _padding = 5f;

		// Token: 0x04001249 RID: 4681
		[SerializeField]
		private GearPopup _gearPopup;

		// Token: 0x0400124A RID: 4682
		[SerializeField]
		private RectTransform _container;

		// Token: 0x0400124B RID: 4683
		[SerializeField]
		private RectTransform _content;

		// Token: 0x0400124C RID: 4684
		[SerializeField]
		private RectTransform _canvas;
	}
}
