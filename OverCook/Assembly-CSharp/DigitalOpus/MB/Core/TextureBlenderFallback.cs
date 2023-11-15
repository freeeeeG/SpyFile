using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	// Token: 0x0200006D RID: 109
	public class TextureBlenderFallback : TextureBlender
	{
		// Token: 0x060002D8 RID: 728 RVA: 0x0001D325 File Offset: 0x0001B725
		public bool DoesShaderNameMatch(string shaderName)
		{
			return true;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0001D328 File Offset: 0x0001B728
		public void OnBeforeTintTexture(Material sourceMat, string shaderTexturePropertyName)
		{
			if (shaderTexturePropertyName.Equals("_MainTex"))
			{
				this.m_doTintColor = true;
				this.m_tintColor = Color.white;
				if (sourceMat.HasProperty("_Color"))
				{
					this.m_tintColor = sourceMat.GetColor("_Color");
				}
			}
			else
			{
				this.m_doTintColor = false;
			}
		}

		// Token: 0x060002DA RID: 730 RVA: 0x0001D384 File Offset: 0x0001B784
		public Color OnBlendTexturePixel(string shaderPropertyName, Color pixelColor)
		{
			if (this.m_doTintColor)
			{
				return new Color(pixelColor.r * this.m_tintColor.r, pixelColor.g * this.m_tintColor.g, pixelColor.b * this.m_tintColor.b, pixelColor.a * this.m_tintColor.a);
			}
			return pixelColor;
		}

		// Token: 0x060002DB RID: 731 RVA: 0x0001D3EF File Offset: 0x0001B7EF
		public bool NonTexturePropertiesAreEqual(Material a, Material b)
		{
			return TextureBlenderFallback._compareColor(a, b, this.m_defaultColor, "_Color");
		}

		// Token: 0x060002DC RID: 732 RVA: 0x0001D40B File Offset: 0x0001B80B
		public void SetNonTexturePropertyValuesOnResultMaterial(Material resultMaterial)
		{
			if (resultMaterial.HasProperty("_Color"))
			{
				resultMaterial.SetColor("_Color", this.m_defaultColor);
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0001D430 File Offset: 0x0001B830
		public Color GetColorIfNoTexture(Material mat, ShaderTextureProperty texProperty)
		{
			if (texProperty.isNormalMap)
			{
				return new Color(0.5f, 0.5f, 1f);
			}
			if (texProperty.name.Equals("_MainTex"))
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
			else if (texProperty.name.Equals("_SpecGlossMap"))
			{
				if (mat != null && mat.HasProperty("_SpecColor"))
				{
					try
					{
						Color color = mat.GetColor("_SpecColor");
						if (mat.HasProperty("_Glossiness"))
						{
							try
							{
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
			}
			else if (texProperty.name.Equals("_MetallicGlossMap"))
			{
				if (mat != null && mat.HasProperty("_Metallic"))
				{
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
			}
			else
			{
				if (texProperty.name.Equals("_ParallaxMap"))
				{
					return new Color(0f, 0f, 0f, 0f);
				}
				if (texProperty.name.Equals("_OcclusionMap"))
				{
					return new Color(1f, 1f, 1f, 1f);
				}
				if (texProperty.name.Equals("_EmissionMap"))
				{
					if (mat != null && mat.HasProperty("_EmissionScaleUI"))
					{
						if (mat.HasProperty("_EmissionColor") && mat.HasProperty("_EmissionColorUI"))
						{
							try
							{
								Color color2 = mat.GetColor("_EmissionColor");
								Color color3 = mat.GetColor("_EmissionColorUI");
								float float2 = mat.GetFloat("_EmissionScaleUI");
								if (color2 == new Color(0f, 0f, 0f, 0f) && color3 == new Color(1f, 1f, 1f, 1f))
								{
									return new Color(float2, float2, float2, float2);
								}
								return color3;
							}
							catch (Exception)
							{
							}
						}
						else
						{
							try
							{
								float float3 = mat.GetFloat("_EmissionScaleUI");
								return new Color(float3, float3, float3, float3);
							}
							catch (Exception)
							{
							}
						}
					}
				}
				else if (texProperty.name.Equals("_DetailMask"))
				{
					return new Color(0f, 0f, 0f, 0f);
				}
			}
			return new Color(1f, 1f, 1f, 0f);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0001D7CC File Offset: 0x0001BBCC
		public static bool _compareColor(Material a, Material b, Color defaultVal, string propertyName)
		{
			Color lhs = defaultVal;
			Color rhs = defaultVal;
			if (a.HasProperty(propertyName))
			{
				lhs = a.GetColor(propertyName);
			}
			if (b.HasProperty(propertyName))
			{
				rhs = b.GetColor(propertyName);
			}
			return !(lhs != rhs);
		}

		// Token: 0x060002DF RID: 735 RVA: 0x0001D814 File Offset: 0x0001BC14
		public static bool _compareFloat(Material a, Material b, float defaultVal, string propertyName)
		{
			float num = defaultVal;
			float num2 = defaultVal;
			if (a.HasProperty(propertyName))
			{
				num = a.GetFloat(propertyName);
			}
			if (b.HasProperty(propertyName))
			{
				num2 = b.GetFloat(propertyName);
			}
			return num == num2;
		}

		// Token: 0x040001C7 RID: 455
		private bool m_doTintColor;

		// Token: 0x040001C8 RID: 456
		private Color m_tintColor;

		// Token: 0x040001C9 RID: 457
		private Color m_defaultColor = Color.white;
	}
}
