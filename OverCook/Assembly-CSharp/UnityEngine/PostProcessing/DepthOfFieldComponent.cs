﻿using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000087 RID: 135
	public sealed class DepthOfFieldComponent : PostProcessingComponentRenderTexture<DepthOfFieldModel>
	{
		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00021021 File Offset: 0x0001F421
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00021044 File Offset: 0x0001F444
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00021048 File Offset: 0x0001F448
		private float CalculateFocalLength()
		{
			DepthOfFieldModel.Settings settings = base.model.settings;
			if (!settings.useCameraFov)
			{
				return settings.focalLength / 1000f;
			}
			float num = this.context.camera.fieldOfView * 0.017453292f;
			return 0.012f / Mathf.Tan(0.5f * num);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x000210A4 File Offset: 0x0001F4A4
		private float CalculateMaxCoCRadius(int screenHeight)
		{
			float num = (float)base.model.settings.kernelSize * 4f + 6f;
			return Mathf.Min(0.05f, num / (float)screenHeight);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x000210E0 File Offset: 0x0001F4E0
		private bool CheckHistory(int width, int height)
		{
			return this.m_CoCHistory != null && this.m_CoCHistory.IsCreated() && this.m_CoCHistory.width == width && this.m_CoCHistory.height == height;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00021130 File Offset: 0x0001F530
		private RenderTextureFormat SelectFormat(RenderTextureFormat primary, RenderTextureFormat secondary)
		{
			if (SystemInfo.SupportsRenderTextureFormat(primary))
			{
				return primary;
			}
			if (SystemInfo.SupportsRenderTextureFormat(secondary))
			{
				return secondary;
			}
			return RenderTextureFormat.Default;
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00021150 File Offset: 0x0001F550
		public void Prepare(RenderTexture source, Material uberMaterial, bool antialiasCoC, Vector2 taaJitter, float taaBlending)
		{
			DepthOfFieldModel.Settings settings = base.model.settings;
			RenderTextureFormat format = RenderTextureFormat.DefaultHDR;
			RenderTextureFormat format2 = this.SelectFormat(RenderTextureFormat.R8, RenderTextureFormat.RHalf);
			float num = this.CalculateFocalLength();
			float num2 = Mathf.Max(settings.focusDistance, num);
			float num3 = (float)source.width / (float)source.height;
			float num4 = num * num / (settings.aperture * (num2 - num) * 0.024f * 2f);
			float num5 = this.CalculateMaxCoCRadius(source.height);
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Depth Of Field");
			material.SetFloat(DepthOfFieldComponent.Uniforms._Distance, num2);
			material.SetFloat(DepthOfFieldComponent.Uniforms._LensCoeff, num4);
			material.SetFloat(DepthOfFieldComponent.Uniforms._MaxCoC, num5);
			material.SetFloat(DepthOfFieldComponent.Uniforms._RcpMaxCoC, 1f / num5);
			material.SetFloat(DepthOfFieldComponent.Uniforms._RcpAspect, 1f / num3);
			RenderTexture renderTexture = this.context.renderTextureFactory.Get(this.context.width, this.context.height, 0, format2, RenderTextureReadWrite.Linear, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			Graphics.Blit(null, renderTexture, material, 0);
			if (antialiasCoC)
			{
				material.SetTexture(DepthOfFieldComponent.Uniforms._CoCTex, renderTexture);
				float z = (!this.CheckHistory(this.context.width, this.context.height)) ? 0f : taaBlending;
				material.SetVector(DepthOfFieldComponent.Uniforms._TaaParams, new Vector3(taaJitter.x, taaJitter.y, z));
				RenderTexture temporary = RenderTexture.GetTemporary(this.context.width, this.context.height, 0, format2);
				Graphics.Blit(this.m_CoCHistory, temporary, material, 1);
				this.context.renderTextureFactory.Release(renderTexture);
				if (this.m_CoCHistory != null)
				{
					RenderTexture.ReleaseTemporary(this.m_CoCHistory);
				}
				renderTexture = (this.m_CoCHistory = temporary);
			}
			RenderTexture renderTexture2 = this.context.renderTextureFactory.Get(this.context.width / 2, this.context.height / 2, 0, format, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			material.SetTexture(DepthOfFieldComponent.Uniforms._CoCTex, renderTexture);
			Graphics.Blit(source, renderTexture2, material, 2);
			RenderTexture renderTexture3 = this.context.renderTextureFactory.Get(this.context.width / 2, this.context.height / 2, 0, format, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			Graphics.Blit(renderTexture2, renderTexture3, material, (int)(3 + settings.kernelSize));
			Graphics.Blit(renderTexture3, renderTexture2, material, 7);
			uberMaterial.SetVector(DepthOfFieldComponent.Uniforms._DepthOfFieldParams, new Vector3(num2, num4, num5));
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.FocusPlane))
			{
				uberMaterial.EnableKeyword("DEPTH_OF_FIELD_COC_VIEW");
				this.context.Interrupt();
			}
			else
			{
				uberMaterial.SetTexture(DepthOfFieldComponent.Uniforms._DepthOfFieldTex, renderTexture2);
				uberMaterial.SetTexture(DepthOfFieldComponent.Uniforms._DepthOfFieldCoCTex, renderTexture);
				uberMaterial.EnableKeyword("DEPTH_OF_FIELD");
			}
			this.context.renderTextureFactory.Release(renderTexture3);
		}

		// Token: 0x0600035D RID: 861 RVA: 0x0002146E File Offset: 0x0001F86E
		public override void OnDisable()
		{
			if (this.m_CoCHistory != null)
			{
				RenderTexture.ReleaseTemporary(this.m_CoCHistory);
			}
			this.m_CoCHistory = null;
		}

		// Token: 0x04000236 RID: 566
		private const string k_ShaderString = "Hidden/Post FX/Depth Of Field";

		// Token: 0x04000237 RID: 567
		private RenderTexture m_CoCHistory;

		// Token: 0x04000238 RID: 568
		private const float k_FilmHeight = 0.024f;

		// Token: 0x02000088 RID: 136
		private static class Uniforms
		{
			// Token: 0x04000239 RID: 569
			internal static readonly int _DepthOfFieldTex = Shader.PropertyToID("_DepthOfFieldTex");

			// Token: 0x0400023A RID: 570
			internal static readonly int _DepthOfFieldCoCTex = Shader.PropertyToID("_DepthOfFieldCoCTex");

			// Token: 0x0400023B RID: 571
			internal static readonly int _Distance = Shader.PropertyToID("_Distance");

			// Token: 0x0400023C RID: 572
			internal static readonly int _LensCoeff = Shader.PropertyToID("_LensCoeff");

			// Token: 0x0400023D RID: 573
			internal static readonly int _MaxCoC = Shader.PropertyToID("_MaxCoC");

			// Token: 0x0400023E RID: 574
			internal static readonly int _RcpMaxCoC = Shader.PropertyToID("_RcpMaxCoC");

			// Token: 0x0400023F RID: 575
			internal static readonly int _RcpAspect = Shader.PropertyToID("_RcpAspect");

			// Token: 0x04000240 RID: 576
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x04000241 RID: 577
			internal static readonly int _CoCTex = Shader.PropertyToID("_CoCTex");

			// Token: 0x04000242 RID: 578
			internal static readonly int _TaaParams = Shader.PropertyToID("_TaaParams");

			// Token: 0x04000243 RID: 579
			internal static readonly int _DepthOfFieldParams = Shader.PropertyToID("_DepthOfFieldParams");
		}
	}
}
