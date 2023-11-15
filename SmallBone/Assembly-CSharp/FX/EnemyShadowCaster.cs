using System;
using UnityEngine;

namespace FX
{
	// Token: 0x02000236 RID: 566
	public class EnemyShadowCaster : MonoBehaviour
	{
		// Token: 0x06000B22 RID: 2850 RVA: 0x0001ED7C File Offset: 0x0001CF7C
		private void Awake()
		{
			if (this._collider == null)
			{
				this._collider = base.GetComponent<Collider2D>();
			}
			this._shadowRenderer = new FootShadowRenderer(0, base.transform);
			this._shadowRenderer.spriteRenderer.sortingLayerID = this._renderer.sortingLayerID;
			this._shadowRenderer.spriteRenderer.sortingOrder = this._renderer.sortingOrder - 10000;
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x0001EDF4 File Offset: 0x0001CFF4
		private void LateUpdate()
		{
			Bounds bounds = this._collider.bounds;
			if (this._customWidth > 0f)
			{
				Vector3 size = bounds.size;
				size.x = this._customWidth;
				bounds.size = size;
			}
			this._shadowRenderer.SetBounds(bounds);
			this._shadowRenderer.Update();
			Vector3 position = this._shadowRenderer.spriteRenderer.transform.position;
			position.x = bounds.center.x;
			this._shadowRenderer.spriteRenderer.transform.position = position;
		}

		// Token: 0x04000951 RID: 2385
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x04000952 RID: 2386
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x04000953 RID: 2387
		[SerializeField]
		[Information("0이면 콜라이더 크기 따라감", InformationAttribute.InformationType.Info, false)]
		private float _customWidth;

		// Token: 0x04000954 RID: 2388
		private FootShadowRenderer _shadowRenderer;
	}
}
