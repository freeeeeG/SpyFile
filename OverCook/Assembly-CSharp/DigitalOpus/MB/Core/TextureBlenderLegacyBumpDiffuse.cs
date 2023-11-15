using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200006E RID: 110
	public class TextureBlenderLegacyBumpDiffuse : TextureBlender
	{
		// Token: 0x060002E1 RID: 737 RVA: 0x0001D86A File Offset: 0x0001BC6A
		public bool DoesShaderNameMatch(string shaderName)
		{
			return shaderName.Equals("Legacy Shaders/Bumped Diffuse") || shaderName.Equals("Bumped Diffuse");
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0001D891 File Offset: 0x0001BC91
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

		// Token: 0x060002E3 RID: 739 RVA: 0x0001D8C8 File Offset: 0x0001BCC8
		public Color OnBlendTexturePixel(string propertyToDoshaderPropertyName, Color pixelColor)
		{
			if (this.doColor)
			{
				return new Color(pixelColor.r * this.m_tintColor.r, pixelColor.g * this.m_tintColor.g, pixelColor.b * this.m_tintColor.b, pixelColor.a * this.m_tintColor.a);
			}
			return pixelColor;
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0001D933 File Offset: 0x0001BD33
		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return TextureBlenderFallback._compareColor(a, b, this.m_defaultTintColor, "_Color");
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0001D947 File Offset: 0x0001BD47
		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
			resultMaterial.SetColor("_Color", Color.white);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0001D95C File Offset: 0x0001BD5C
		public Color GetColorIfNoTexture(Material m, ShaderTextureProperty texPropertyName)
		{
			if (texPropertyName.name.Equals("_BumpMap"))
			{
				return new Color(0.5f, 0.5f, 1f);
			}
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

		// Token: 0x040001CA RID: 458
		private bool doColor;

		// Token: 0x040001CB RID: 459
		private Color m_tintColor;

		// Token: 0x040001CC RID: 460
		private Color m_defaultTintColor = Color.white;
	}
}
