using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200006F RID: 111
	public class TextureBlenderLegacyDiffuse : TextureBlender
	{
		// Token: 0x060002E8 RID: 744 RVA: 0x0001DA1B File Offset: 0x0001BE1B
		public bool DoesShaderNameMatch(string shaderName)
		{
			return shaderName.Equals("Legacy Shaders/Diffuse") || shaderName.Equals("Diffuse");
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x0001DA42 File Offset: 0x0001BE42
		public void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName)
		{
			if (shaderTexturePropertyName.EndsWith("_MainTex"))
			{
				this.doColor = true;
				this.m_tintColor = sourceMat.GetColor("_Color");
			}
			else
			{
				this.doColor = false;
			}
		}

		// Token: 0x060002EA RID: 746 RVA: 0x0001DA78 File Offset: 0x0001BE78
		public Color OnBlendTexturePixel(string propertyToDoshaderPropertyName, Color pixelColor)
		{
			if (this.doColor)
			{
				return new Color(pixelColor.r * this.m_tintColor.r, pixelColor.g * this.m_tintColor.g, pixelColor.b * this.m_tintColor.b, pixelColor.a * this.m_tintColor.a);
			}
			return pixelColor;
		}

		// Token: 0x060002EB RID: 747 RVA: 0x0001DAE3 File Offset: 0x0001BEE3
		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return TextureBlenderFallback._compareColor(a, b, this.m_defaultTintColor, "_Color");
		}

		// Token: 0x060002EC RID: 748 RVA: 0x0001DAF7 File Offset: 0x0001BEF7
		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
			resultMaterial.SetColor("_Color", Color.white);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x0001DB0C File Offset: 0x0001BF0C
		public Color GetColorIfNoTexture(Material m, ShaderTextureProperty texPropertyName)
		{
			if (texPropertyName.name.Equals("_MainTex") && m != null && m.HasProperty("_Color"))
			{
				try
				{
					return m.GetColor("_Color");
				}
				catch (Exception)
				{
				}
			}
			return new Color(1f, 1f, 1f, 0f);
		}

		// Token: 0x040001CD RID: 461
		private bool doColor;

		// Token: 0x040001CE RID: 462
		private Color m_tintColor;

		// Token: 0x040001CF RID: 463
		private Color m_defaultTintColor = Color.white;
	}
}
