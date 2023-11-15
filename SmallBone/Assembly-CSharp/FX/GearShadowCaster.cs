using System;
using UnityEngine;

namespace FX
{
	// Token: 0x0200023D RID: 573
	public class GearShadowCaster : MonoBehaviour
	{
		// Token: 0x06000B4A RID: 2890 RVA: 0x0001F46C File Offset: 0x0001D66C
		private void Awake()
		{
			this._shadowRenderer = new FootShadowRenderer(0, base.transform);
			this._shadowRenderer.spriteRenderer.sortingLayerID = this._renderer.sortingLayerID;
			this._shadowRenderer.spriteRenderer.sortingOrder = this._renderer.sortingOrder - 10000;
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0001F4C8 File Offset: 0x0001D6C8
		private void LateUpdate()
		{
			Bounds bounds = this._collider.bounds;
			bounds.size = new Vector2(0.75f, 0.5f);
			this._shadowRenderer.SetBounds(bounds);
			this._shadowRenderer.Update();
		}

		// Token: 0x0400096D RID: 2413
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x0400096E RID: 2414
		[SerializeField]
		private Collider2D _collider;

		// Token: 0x0400096F RID: 2415
		private FootShadowRenderer _shadowRenderer;
	}
}
