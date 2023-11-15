using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200007B RID: 123
	public sealed class AmbientOcclusionComponent : PostProcessingComponentCommandBuffer<AmbientOcclusionModel>
	{
		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0001EF00 File Offset: 0x0001D300
		public override bool active
		{
			get
			{
				return base.model.enabled && base.model.settings.intensity > 0f && !this.context.interrupted;
			}
		}

		// Token: 0x0600031A RID: 794 RVA: 0x0001EF4B File Offset: 0x0001D34B
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x0600031B RID: 795 RVA: 0x0001EF4E File Offset: 0x0001D34E
		public override string GetName()
		{
			return "Ambient Occlusion";
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0001EF55 File Offset: 0x0001D355
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.BeforeImageEffectsOpaque;
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0001EF5C File Offset: 0x0001D35C
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			AmbientOcclusionModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Ambient Occlusion");
			material.SetFloat(AmbientOcclusionComponent.Uniforms._Intensity, settings.intensity);
			material.SetFloat(AmbientOcclusionComponent.Uniforms._Radius, settings.radius);
			material.SetTexture(AmbientOcclusionComponent.Uniforms._RotationTex, settings.RotationTexture);
			material.SetFloat(AmbientOcclusionComponent.Uniforms._BlurSize, settings.blurSize);
			material.SetFloat(AmbientOcclusionComponent.Uniforms._Downsample, (!settings.downsampling) ? 1f : 2f);
			material.SetFloat(AmbientOcclusionComponent.Uniforms._DistanceFalloff, settings.distanceFalloff);
			material.shaderKeywords = null;
			AmbientOcclusionModel.SampleCount sampleCount = settings.sampleCount;
			switch (sampleCount)
			{
			case AmbientOcclusionModel.SampleCount.Lowest:
				material.EnableKeyword("FOUR_SAMPLES");
				break;
			default:
				if (sampleCount != AmbientOcclusionModel.SampleCount.Medium)
				{
					if (sampleCount == AmbientOcclusionModel.SampleCount.High)
					{
						material.EnableKeyword("TWELVE_SAMPLES");
					}
				}
				else
				{
					material.EnableKeyword("TEN_SAMPLES");
				}
				break;
			case AmbientOcclusionModel.SampleCount.Low:
				material.EnableKeyword("FIVE_SAMPLES");
				break;
			}
			int num = this.context.width;
			int num2 = this.context.height;
			if (settings.downsampling)
			{
				num /= 2;
				num2 /= 2;
			}
			cb.Clear();
			cb.GetTemporaryRT(AmbientOcclusionComponent.Uniforms._DownsampledDepth, num, num2, 0, FilterMode.Point, RenderTextureFormat.RFloat, RenderTextureReadWrite.Linear);
			cb.Blit(null, AmbientOcclusionComponent.Uniforms._DownsampledDepth, material, 4);
			cb.GetTemporaryRT(AmbientOcclusionComponent.Uniforms._AOTex, num, num2, 0, FilterMode.Bilinear, RenderTextureFormat.R8, RenderTextureReadWrite.Linear);
			cb.Blit(null, AmbientOcclusionComponent.Uniforms._AOTex, material, 0);
			cb.GetTemporaryRT(AmbientOcclusionComponent.Uniforms._TempBlur, num, num2, 0, FilterMode.Bilinear, RenderTextureFormat.R8);
			cb.Blit(null, AmbientOcclusionComponent.Uniforms._TempBlur, material, 1);
			cb.Blit(null, AmbientOcclusionComponent.Uniforms._AOTex, material, 2);
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, material, 3);
			cb.ReleaseTemporaryRT(AmbientOcclusionComponent.Uniforms._AOTex);
			cb.ReleaseTemporaryRT(AmbientOcclusionComponent.Uniforms._TempBlur);
		}

		// Token: 0x040001EF RID: 495
		private const string k_BlitShaderString = "Hidden/Post FX/Blit";

		// Token: 0x040001F0 RID: 496
		private const string k_ShaderString = "Hidden/Post FX/Ambient Occlusion";

		// Token: 0x0200007C RID: 124
		private static class Uniforms
		{
			// Token: 0x040001F1 RID: 497
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x040001F2 RID: 498
			internal static readonly int _AOTex = Shader.PropertyToID("_AOTex");

			// Token: 0x040001F3 RID: 499
			internal static readonly int _Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x040001F4 RID: 500
			internal static readonly int _Radius = Shader.PropertyToID("_Radius");

			// Token: 0x040001F5 RID: 501
			internal static readonly int _RotationTex = Shader.PropertyToID("_RotationTex");

			// Token: 0x040001F6 RID: 502
			internal static readonly int _TempBlur = Shader.PropertyToID("_TempBlur");

			// Token: 0x040001F7 RID: 503
			internal static readonly int _BlurSize = Shader.PropertyToID("_BlurSize");

			// Token: 0x040001F8 RID: 504
			internal static readonly int _Downsample = Shader.PropertyToID("_Downsample");

			// Token: 0x040001F9 RID: 505
			internal static readonly int _DownsampledDepth = Shader.PropertyToID("_DownsampledDepth");

			// Token: 0x040001FA RID: 506
			internal static readonly int _DistanceFalloff = Shader.PropertyToID("_DistanceFalloff");
		}
	}
}
