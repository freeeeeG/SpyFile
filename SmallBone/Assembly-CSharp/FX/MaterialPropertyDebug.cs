using System;
using GameResources;
using UnityEngine;

namespace FX
{
	// Token: 0x02000244 RID: 580
	[ExecuteInEditMode]
	public class MaterialPropertyDebug : MonoBehaviour
	{
		// Token: 0x06000B68 RID: 2920 RVA: 0x0001F76C File Offset: 0x0001D96C
		private void Update()
		{
			MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
			this._renderer.sharedMaterial = MaterialResource.effect;
			this._renderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetInt(MaterialPropertyDebug._huePropertyID, this.hue);
			this._renderer.SetPropertyBlock(materialPropertyBlock);
		}

		// Token: 0x0400097F RID: 2431
		private static readonly int _huePropertyID = Shader.PropertyToID("_Hue");

		// Token: 0x04000980 RID: 2432
		[Range(-180f, 180f)]
		public int hue;

		// Token: 0x04000981 RID: 2433
		[SerializeField]
		[GetComponent]
		private SpriteRenderer _renderer;

		// Token: 0x04000982 RID: 2434
		private MaterialPropertyBlock _propertyBlock;
	}
}
