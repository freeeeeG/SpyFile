using System;
using UnityEngine;

namespace FX
{
	// Token: 0x02000226 RID: 550
	public class ApplyEffectFX : MonoBehaviour
	{
		// Token: 0x06000AD3 RID: 2771 RVA: 0x0001D90C File Offset: 0x0001BB0C
		private void Awake()
		{
			this._propertyBlock = new MaterialPropertyBlock();
			this._renderer.GetPropertyBlock(this._propertyBlock);
			this._propertyBlock.SetInt(EffectInfo.huePropertyID, this._hue);
			this._renderer.SetPropertyBlock(this._propertyBlock);
		}

		// Token: 0x040008EE RID: 2286
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x040008EF RID: 2287
		[SerializeField]
		private int _hue;

		// Token: 0x040008F0 RID: 2288
		private MaterialPropertyBlock _propertyBlock;
	}
}
