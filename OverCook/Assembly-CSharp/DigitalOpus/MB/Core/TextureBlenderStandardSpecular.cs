using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x02000072 RID: 114
	public class TextureBlenderStandardSpecular : TextureBlender
	{
		// Token: 0x060002F6 RID: 758 RVA: 0x0001E0EB File Offset: 0x0001C4EB
		public bool DoesShaderNameMatch(string shaderName)
		{
			return shaderName.Equals("Standard (Specular setup)");
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0001E0F8 File Offset: 0x0001C4F8
		public void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName)
		{
			if (shaderTexturePropertyName.Equals("_MainTex"))
			{
				this.propertyToDo = TextureBlenderStandardSpecular.Prop.doColor;
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
				this.propertyToDo = TextureBlenderStandardSpecular.Prop.doSpecular;
			}
			else if (shaderTexturePropertyName.Equals("_EmissionMap"))
			{
				this.propertyToDo = TextureBlenderStandardSpecular.Prop.doEmission;
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
				this.propertyToDo = TextureBlenderStandardSpecular.Prop.doNone;
			}
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x0001E1BC File Offset: 0x0001C5BC
		public Color OnBlendTexturePixel(string propertyToDoshaderPropertyName, Color pixelColor)
		{
			if (this.propertyToDo == TextureBlenderStandardSpecular.Prop.doColor)
			{
				return new Color(pixelColor.r * this.m_tintColor.r, pixelColor.g * this.m_tintColor.g, pixelColor.b * this.m_tintColor.b, pixelColor.a * this.m_tintColor.a);
			}
			if (this.propertyToDo == TextureBlenderStandardSpecular.Prop.doSpecular)
			{
				return pixelColor;
			}
			if (this.propertyToDo == TextureBlenderStandardSpecular.Prop.doEmission)
			{
				return new Color(pixelColor.r * this.m_emission.r, pixelColor.g * this.m_emission.g, pixelColor.b * this.m_emission.b, pixelColor.a * this.m_emission.a);
			}
			return pixelColor;
		}

		// Token: 0x060002F9 RID: 761 RVA: 0x0001E294 File Offset: 0x0001C694
		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return TextureBlenderFallback._compareColor(a, b, this.m_defaultColor, "_Color") && TextureBlenderFallback._compareColor(a, b, this.m_defaultSpecular, "_SpecColor") && TextureBlenderFallback._compareFloat(a, b, this.m_defaultGlossiness, "_Glossiness") && TextureBlenderFallback._compareColor(a, b, this.m_defaultEmission, "_EmissionColor");
		}

		// Token: 0x060002FA RID: 762 RVA: 0x0001E308 File Offset: 0x0001C708
		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
			resultMaterial.SetColor("_Color", this.m_defaultColor);
			resultMaterial.SetColor("_SpecColor", this.m_defaultSpecular);
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

		// Token: 0x060002FB RID: 763 RVA: 0x0001E384 File Offset: 0x0001C784
		public Color GetColorIfNoTexture(Material mat, ShaderTextureProperty texPropertyName)
		{
			if (texPropertyName.name.Equals("_BumpMap"))
			{
				return new Color(0.5f, 0.5f, 1f);
			}
			if (texPropertyName.name.Equals("_MainTex"))
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
			else if (texPropertyName.name.Equals("_SpecGlossMap"))
			{
				bool flag = false;
				if (mat != null && mat.HasProperty("_SpecColor"))
				{
					try
					{
						Color color = mat.GetColor("_SpecColor");
						if (mat.HasProperty("_Glossiness"))
						{
							try
							{
								flag = true;
								color.a = mat.GetFloat("_Glossiness");
							}
							catch (Exception)
							{
							}
						}
						Debug.LogWarning(color);
						return color;
					}
					catch (Exception)
					{
					}
				}
				if (!flag)
				{
					return this.m_defaultSpecular;
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

		// Token: 0x040001DC RID: 476
		private Color m_tintColor;

		// Token: 0x040001DD RID: 477
		private Color m_emission;

		// Token: 0x040001DE RID: 478
		private TextureBlenderStandardSpecular.Prop propertyToDo = TextureBlenderStandardSpecular.Prop.doNone;

		// Token: 0x040001DF RID: 479
		private Color m_defaultColor = Color.white;

		// Token: 0x040001E0 RID: 480
		private Color m_defaultSpecular = Color.black;

		// Token: 0x040001E1 RID: 481
		private float m_defaultGlossiness = 0.5f;

		// Token: 0x040001E2 RID: 482
		private Color m_defaultEmission = Color.black;

		// Token: 0x02000073 RID: 115
		private enum Prop
		{
			// Token: 0x040001E4 RID: 484
			doColor,
			// Token: 0x040001E5 RID: 485
			doSpecular,
			// Token: 0x040001E6 RID: 486
			doEmission,
			// Token: 0x040001E7 RID: 487
			doNone
		}
	}
}
