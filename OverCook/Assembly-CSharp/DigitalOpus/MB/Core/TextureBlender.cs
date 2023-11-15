using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200006C RID: 108
	public interface TextureBlender
	{
		// Token: 0x060002D1 RID: 721
		bool DoesShaderNameMatch(string shaderName);

		// Token: 0x060002D2 RID: 722
		void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName);

		// Token: 0x060002D3 RID: 723
		Color OnBlendTexturePixel(string shaderPropertyName, Color pixelColor);

		// Token: 0x060002D4 RID: 724
		bool NonTexturePropertiesAreEqual(Material a, Material b);

		// Token: 0x060002D5 RID: 725
		void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial);

		// Token: 0x060002D6 RID: 726
		Color GetColorIfNoTexture(Material m, ShaderTextureProperty texPropertyName);
	}
}
