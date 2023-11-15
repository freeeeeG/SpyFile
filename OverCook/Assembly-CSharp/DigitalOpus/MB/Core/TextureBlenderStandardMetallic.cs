using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000070 RID: 112
	public class TextureBlenderStandardMetallic : TextureBlender
	{
		// Token: 0x060002EF RID: 751 RVA: 0x0001DBBC File Offset: 0x0001BFBC
		public bool DoesShaderNameMatch(string shaderName)
		{
			return shaderName.Equals("Standard");
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x0001DBCC File Offset: 0x0001BFCC
		public void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName)
		{
			if (shaderTexturePropertyName.Equals("_MainTex"))
			{
				this.propertyToDo = TextureBlenderStandardMetallic.Prop.doColor;
				if (sourceMat.HasProperty(shaderTexturePropertyName))
				{
					this.m_tintColor = sourceMat.GetColor("_Color");
				}
				else
				{
					this.m_tintColor = this.m_defaultColor;
				}
			}
			else if (shaderTexturePropertyName.Equals("_MetallicGlossMap"))
			{
				this.propertyToDo = TextureBlenderStandardMetallic.Prop.doMetallic;
			}
			else if (shaderTexturePropertyName.Equals("_EmissionMap"))
			{
				this.propertyToDo = TextureBlenderStandardMetallic.Prop.doEmission;
				if (sourceMat.HasProperty(shaderTexturePropertyName))
				{
					this.m_emission = sourceMat.GetColor("_EmissionColor");
				}
				else
				{
					this.m_emission = this.m_defaultEmission;
				}
			}
			else
			{
				this.propertyToDo = TextureBlenderStandardMetallic.Prop.doNone;
			}
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x0001DC90 File Offset: 0x0001C090
		public Color OnBlendTexturePixel(string propertyToDoshaderPropertyName, Color pixelColor)
		{
			if (this.propertyToDo == TextureBlenderStandardMetallic.Prop.doColor)
			{
				return new Color(pixelColor.r * this.m_tintColor.r, pixelColor.g * this.m_tintColor.g, pixelColor.b * this.m_tintColor.b, pixelColor.a * this.m_tintColor.a);
			}
			if (this.propertyToDo == TextureBlenderStandardMetallic.Prop.doMetallic)
			{
				return pixelColor;
			}
			if (this.propertyToDo == TextureBlenderStandardMetallic.Prop.doEmission)
			{
				return new Color(pixelColor.r * this.m_emission.r, pixelColor.g * this.m_emission.g, pixelColor.b * this.m_emission.b, pixelColor.a * this.m_emission.a);
			}
			return pixelColor;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x0001DD68 File Offset: 0x0001C168
		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return TextureBlenderFallback._compareColor(a, b, this.m_defaultColor, "_Color") && TextureBlenderFallback._compareFloat(a, b, this.m_defaultMetallic, "_Metallic") && TextureBlenderFallback._compareFloat(a, b, this.m_defaultGlossiness, "_Glossiness") && TextureBlenderFallback._compareColor(a, b, this.m_defaultEmission, "_EmissionColor");
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x0001DDDC File Offset: 0x0001C1DC
		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
			resultMaterial.SetColor("_Color", this.m_defaultColor);
			resultMaterial.SetFloat("_Metallic", this.m_defaultMetallic);
			resultMaterial.SetFloat("_Glossiness", this.m_defaultGlossiness);
			if (resultMaterial.GetTexture("_EmissionMap") == null)
			{
				resultMaterial.SetColor("_EmissionColor", Color.black);
			}
			else
			{
				resultMaterial.SetColor("_EmissionColor", Color.white);
			}
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x0001DE58 File Offset: 0x0001C258
		public Color GetColorIfNoTexture(Material mat, ShaderTextureProperty texPropertyName)
		{
			if (texPropertyName.name.Equals("_BumpMap"))
			{
				return new Color(0.5f, 0.5f, 1f);
			}
			if (texPropertyName.Equals("_MainTex"))
			{
				if (mat != null && mat.HasProperty("_Color"))
				{
					try
					{
						return mat.GetColor("_Color");
					}
					catch (Exception)
					{
					}
				}
			}
			else if (texPropertyName.name.Equals("_MetallicGlossMap"))
			{
				if (!(mat != null) || !mat.HasProperty("_Metallic"))
				{
					return new Color(0f, 0f, 0f, 0.5f);
				}
				try
				{
					float @float = mat.GetFloat("_Metallic");
					Color result = new Color(@float, @float, @float);
					if (mat.HasProperty("_Glossiness"))
					{
						try
						{
							result.a = mat.GetFloat("_Glossiness");
						}
						catch (Exception)
						{
						}
					}
					return result;
				}
				catch (Exception)
				{
				}
			}
			else
			{
				if (texPropertyName.name.Equals("_ParallaxMap"))
				{
					return new Color(0f, 0f, 0f, 0f);
				}
				if (texPropertyName.name.Equals("_OcclusionMap"))
				{
					return new Color(1f, 1f, 1f, 1f);
				}
				if (texPropertyName.name.Equals("_EmissionMap"))
				{
					if (mat != null)
					{
						if (!mat.HasProperty("_EmissionColor"))
						{
							return Color.black;
						}
						try
						{
							return mat.GetColor("_EmissionColor");
						}
						catch (Exception)
						{
						}
					}
				}
				else if (texPropertyName.name.Equals("_DetailMask"))
				{
					return new Color(0f, 0f, 0f, 0f);
				}
			}
			return new Color(1f, 1f, 1f, 0f);
		}

		// Token: 0x040001D0 RID: 464
		private Color m_tintColor;

		// Token: 0x040001D1 RID: 465
		private Color m_emission;

		// Token: 0x040001D2 RID: 466
		private TextureBlenderStandardMetallic.Prop propertyToDo = TextureBlenderStandardMetallic.Prop.doNone;

		// Token: 0x040001D3 RID: 467
		private Color m_defaultColor = Color.white;

		// Token: 0x040001D4 RID: 468
		private float m_defaultMetallic;

		// Token: 0x040001D5 RID: 469
		private float m_defaultGlossiness = 0.5f;

		// Token: 0x040001D6 RID: 470
		private Color m_defaultEmission = Color.black;

		// Token: 0x02000071 RID: 113
		private enum Prop
		{
			// Token: 0x040001D8 RID: 472
			doColor,
			// Token: 0x040001D9 RID: 473
			doMetallic,
			// Token: 0x040001DA RID: 474
			doEmission,
			// Token: 0x040001DB RID: 475
			doNone
		}
	}
}
