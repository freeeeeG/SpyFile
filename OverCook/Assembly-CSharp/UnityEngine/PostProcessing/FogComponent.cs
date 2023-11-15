using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200008D RID: 141
	public sealed class FogComponent : PostProcessingComponentCommandBuffer<FogModel>
	{
		// Token: 0x1700004F RID: 79
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00021CC7 File Offset: 0x000200C7
		public override bool active
		{
			get
			{
				return base.model.enabled && this.context.isGBufferAvailable && RenderSettings.fog && !this.context.interrupted;
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x00021D04 File Offset: 0x00020104
		public override string GetName()
		{
			return "Fog";
		}

		// Token: 0x06000371 RID: 881 RVA: 0x00021D0B File Offset: 0x0002010B
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00021D0E File Offset: 0x0002010E
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.AfterImageEffectsOpaque;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x00021D14 File Offset: 0x00020114
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			FogModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Fog");
			material.shaderKeywords = null;
			Color value = (!GraphicsUtils.isLinearColorSpace) ? RenderSettings.fogColor : RenderSettings.fogColor.linear;
			material.SetColor(FogComponent.Uniforms._FogColor, value);
			material.SetFloat(FogComponent.Uniforms._Density, RenderSettings.fogDensity);
			material.SetFloat(FogComponent.Uniforms._Start, RenderSettings.fogStartDistance);
			material.SetFloat(FogComponent.Uniforms._End, RenderSettings.fogEndDistance);
			FogMode fogMode = RenderSettings.fogMode;
			if (fogMode != FogMode.Linear)
			{
				if (fogMode != FogMode.Exponential)
				{
					if (fogMode == FogMode.ExponentialSquared)
					{
						material.EnableKeyword("FOG_EXP2");
					}
				}
				else
				{
					material.EnableKeyword("FOG_EXP");
				}
			}
			else
			{
				material.EnableKeyword("FOG_LINEAR");
			}
			RenderTextureFormat format = (!this.context.isHdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
			cb.GetTemporaryRT(FogComponent.Uniforms._TempRT, this.context.width, this.context.height, 24, FilterMode.Bilinear, format);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, FogComponent.Uniforms._TempRT);
			cb.Blit(FogComponent.Uniforms._TempRT, BuiltinRenderTextureType.CameraTarget, material, (!settings.excludeSkybox) ? 0 : 1);
			cb.ReleaseTemporaryRT(FogComponent.Uniforms._TempRT);
		}

		// Token: 0x0400025A RID: 602
		private const string k_ShaderString = "Hidden/Post FX/Fog";

		// Token: 0x0200008E RID: 142
		private static class Uniforms
		{
			// Token: 0x0400025B RID: 603
			internal static readonly int _FogColor = Shader.PropertyToID("_FogColor");

			// Token: 0x0400025C RID: 604
			internal static readonly int _Density = Shader.PropertyToID("_Density");

			// Token: 0x0400025D RID: 605
			internal static readonly int _Start = Shader.PropertyToID("_Start");

			// Token: 0x0400025E RID: 606
			internal static readonly int _End = Shader.PropertyToID("_End");

			// Token: 0x0400025F RID: 607
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}
	}
}
