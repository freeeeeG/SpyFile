using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FlatKit
{
	// Token: 0x02000053 RID: 83
	public class FlatKitFog : ScriptableRendererFeature
	{
		// Token: 0x060000E8 RID: 232 RVA: 0x00004434 File Offset: 0x00002634
		public override void Create()
		{
			if (this.settings == null)
			{
				Debug.LogWarning("[FlatKit] Missing Fog Settings");
				return;
			}
			if (this._blitShader == null)
			{
				this._blitShader = Shader.Find(BlitTexturePass.CopyEffectShaderName);
			}
			this._blitTexturePass = new BlitTexturePass
			{
				renderPassEvent = this.settings.renderEvent
			};
			this._fogTexture.Init("_EffectTexture");
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x000044A0 File Offset: 0x000026A0
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (this.settings == null)
			{
				Debug.LogWarning("[FlatKit] Missing Fog Settings");
				return;
			}
			if (!this.CreateMaterials())
			{
				return;
			}
			this.SetMaterialProperties();
			this._blitTexturePass.Setup(this._effectMaterial, true, false, false);
			renderer.EnqueuePass(this._blitTexturePass);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x000044F5 File Offset: 0x000026F5
		protected override void Dispose(bool disposing)
		{
			CoreUtils.Destroy(this._effectMaterial);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00004504 File Offset: 0x00002704
		private bool CreateMaterials()
		{
			if (this._effectMaterial == null)
			{
				Shader shader = Shader.Find(FlatKitFog.FogShaderName);
				Shader x = Shader.Find(BlitTexturePass.CopyEffectShaderName);
				if (shader == null || x == null)
				{
					return false;
				}
				this._effectMaterial = CoreUtils.CreateEngineMaterial(shader);
			}
			return true;
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00004558 File Offset: 0x00002758
		private void SetMaterialProperties()
		{
			if (this._effectMaterial == null)
			{
				return;
			}
			this.UpdateDistanceLut();
			this._effectMaterial.SetTexture(FlatKitFog.DistanceLut, this._lutDepth);
			this._effectMaterial.SetFloat(FlatKitFog.Near, this.settings.near);
			this._effectMaterial.SetFloat(FlatKitFog.Far, this.settings.far);
			this._effectMaterial.SetFloat(FlatKitFog.UseDistanceFog, this.settings.useDistance ? 1f : 0f);
			this._effectMaterial.SetFloat(FlatKitFog.UseDistanceFogOnSky, this.settings.useDistanceFogOnSky ? 1f : 0f);
			this._effectMaterial.SetFloat(FlatKitFog.DistanceFogIntensity, this.settings.distanceFogIntensity);
			this.UpdateHeightLut();
			this._effectMaterial.SetTexture(FlatKitFog.HeightLut, this._lutHeight);
			this._effectMaterial.SetFloat(FlatKitFog.LowWorldY, this.settings.low);
			this._effectMaterial.SetFloat(FlatKitFog.HighWorldY, this.settings.high);
			this._effectMaterial.SetFloat(FlatKitFog.UseHeightFog, this.settings.useHeight ? 1f : 0f);
			this._effectMaterial.SetFloat(FlatKitFog.UseHeightFogOnSky, this.settings.useHeightFogOnSky ? 1f : 0f);
			this._effectMaterial.SetFloat(FlatKitFog.HeightFogIntensity, this.settings.heightFogIntensity);
			this._effectMaterial.SetFloat(FlatKitFog.DistanceHeightBlend, this.settings.distanceHeightBlend);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00004710 File Offset: 0x00002910
		private void UpdateDistanceLut()
		{
			if (this.settings.distanceGradient == null)
			{
				return;
			}
			if (this._lutDepth != null)
			{
				Object.DestroyImmediate(this._lutDepth);
			}
			this._lutDepth = new Texture2D(256, 1, TextureFormat.RGBA32, false)
			{
				wrapMode = TextureWrapMode.Clamp,
				hideFlags = HideFlags.HideAndDontSave,
				filterMode = FilterMode.Bilinear
			};
			for (float num = 0f; num < 256f; num += 1f)
			{
				Color color = this.settings.distanceGradient.Evaluate(num / 255f);
				for (float num2 = 0f; num2 < 1f; num2 += 1f)
				{
					this._lutDepth.SetPixel(Mathf.CeilToInt(num), Mathf.CeilToInt(num2), color);
				}
			}
			this._lutDepth.Apply();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x000047D8 File Offset: 0x000029D8
		private void UpdateHeightLut()
		{
			if (this.settings.heightGradient == null)
			{
				return;
			}
			if (this._lutHeight != null)
			{
				Object.DestroyImmediate(this._lutHeight);
			}
			this._lutHeight = new Texture2D(256, 1, TextureFormat.RGBA32, false)
			{
				wrapMode = TextureWrapMode.Clamp,
				hideFlags = HideFlags.HideAndDontSave,
				filterMode = FilterMode.Bilinear
			};
			for (float num = 0f; num < 256f; num += 1f)
			{
				Color color = this.settings.heightGradient.Evaluate(num / 255f);
				for (float num2 = 0f; num2 < 1f; num2 += 1f)
				{
					this._lutHeight.SetPixel(Mathf.CeilToInt(num), Mathf.CeilToInt(num2), color);
				}
			}
			this._lutHeight.Apply();
		}

		// Token: 0x040000A0 RID: 160
		[Tooltip("To create new settings use 'Create > FlatKit > Fog Settings'.")]
		public FogSettings settings;

		// Token: 0x040000A1 RID: 161
		[SerializeField]
		[HideInInspector]
		private Material _effectMaterial;

		// Token: 0x040000A2 RID: 162
		private BlitTexturePass _blitTexturePass;

		// Token: 0x040000A3 RID: 163
		[SerializeField]
		[HideInInspector]
		private Shader _blitShader;

		// Token: 0x040000A4 RID: 164
		private RenderTargetHandle _fogTexture;

		// Token: 0x040000A5 RID: 165
		private Texture2D _lutDepth;

		// Token: 0x040000A6 RID: 166
		private Texture2D _lutHeight;

		// Token: 0x040000A7 RID: 167
		private static readonly string FogShaderName = "Hidden/FlatKit/FogFilter";

		// Token: 0x040000A8 RID: 168
		private static readonly int DistanceLut = Shader.PropertyToID("_DistanceLUT");

		// Token: 0x040000A9 RID: 169
		private static readonly int Near = Shader.PropertyToID("_Near");

		// Token: 0x040000AA RID: 170
		private static readonly int Far = Shader.PropertyToID("_Far");

		// Token: 0x040000AB RID: 171
		private static readonly int UseDistanceFog = Shader.PropertyToID("_UseDistanceFog");

		// Token: 0x040000AC RID: 172
		private static readonly int UseDistanceFogOnSky = Shader.PropertyToID("_UseDistanceFogOnSky");

		// Token: 0x040000AD RID: 173
		private static readonly int DistanceFogIntensity = Shader.PropertyToID("_DistanceFogIntensity");

		// Token: 0x040000AE RID: 174
		private static readonly int HeightLut = Shader.PropertyToID("_HeightLUT");

		// Token: 0x040000AF RID: 175
		private static readonly int LowWorldY = Shader.PropertyToID("_LowWorldY");

		// Token: 0x040000B0 RID: 176
		private static readonly int HighWorldY = Shader.PropertyToID("_HighWorldY");

		// Token: 0x040000B1 RID: 177
		private static readonly int UseHeightFog = Shader.PropertyToID("_UseHeightFog");

		// Token: 0x040000B2 RID: 178
		private static readonly int UseHeightFogOnSky = Shader.PropertyToID("_UseHeightFogOnSky");

		// Token: 0x040000B3 RID: 179
		private static readonly int HeightFogIntensity = Shader.PropertyToID("_HeightFogIntensity");

		// Token: 0x040000B4 RID: 180
		private static readonly int DistanceHeightBlend = Shader.PropertyToID("_DistanceHeightBlend");
	}
}
