using System;
using GameResources;
using UnityEngine;

namespace FX
{
	// Token: 0x02000243 RID: 579
	public class MaterialProperty : MonoBehaviour
	{
		// Token: 0x06000B65 RID: 2917 RVA: 0x0001F70C File Offset: 0x0001D90C
		private void Start()
		{
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			this._renderer.sharedMaterial = MaterialResource.effect;
			this._renderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetInt(MaterialProperty._huePropertyID, this.hue);
			this._renderer.SetPropertyBlock(materialPropertyBlock);
		}

		// Token: 0x0400097B RID: 2427
		private static readonly int _huePropertyID = Shader.PropertyToID("_Hue");

		// Token: 0x0400097C RID: 2428
		[Range(-180f, 180f)]
		public int hue;

		// Token: 0x0400097D RID: 2429
		[GetComponent]
		[SerializeField]
		private SpriteRenderer _renderer;

		// Token: 0x0400097E RID: 2430
		private MaterialPropertyBlock _propertyBlock;
	}
}
