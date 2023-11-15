using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.PostProcessing;

namespace CompoundRendererFeature.PostProcess
{
	// Token: 0x0200004E RID: 78
	[CompoundRendererFeature("Stylized Color Grading", InjectionPoint.BeforePostProcess, false)]
	public class ColorGradingRenderer : CompoundRenderer
	{
		// Token: 0x1700002A RID: 42
		// (get) Token: 0x060000CF RID: 207 RVA: 0x00003BE4 File Offset: 0x00001DE4
		public override bool visibleInSceneView
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x00003BE7 File Offset: 0x00001DE7
		public override ScriptableRenderPassInput input
		{
			get
			{
				return ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Color;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00003BEA File Offset: 0x00001DEA
		public override void Initialize()
		{
			base.Initialize();
			this._effectMaterial = CoreUtils.CreateEngineMaterial("Hidden/CompoundRendererFeature/ColorGrading");
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00003C04 File Offset: 0x00001E04
		public override bool Setup(in RenderingData renderingData, InjectionPoint injectionPoint)
		{
			base.Setup(renderingData, injectionPoint);
			VolumeStack stack = VolumeManager.instance.stack;
			this._volumeComponent = stack.GetComponent<ColorGrading>();
			return this._volumeComponent.intensity.value > 0f;
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00003C48 File Offset: 0x00001E48
		public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, ref RenderingData renderingData, InjectionPoint injectionPoint)
		{
			RenderTextureDescriptor tempRTDescriptor = CompoundRenderer.GetTempRTDescriptor(renderingData);
			this._effectMaterial.SetFloat(ColorGradingRenderer.PropertyIDs.Intensity, this._volumeComponent.intensity.value);
			this._effectMaterial.SetVector(ColorGradingRenderer.PropertyIDs.ShadowBezierPoints, new Vector4(this._volumeComponent.blueShadows.value, this._volumeComponent.greenShadows.value));
			this._effectMaterial.SetVector(ColorGradingRenderer.PropertyIDs.HighlightBezierPoints, new Vector4(this._volumeComponent.redHighlights.value, 0f, 0f, 0f));
			this._effectMaterial.SetFloat(ColorGradingRenderer.PropertyIDs.Contrast, this._volumeComponent.contrast.value);
			this._effectMaterial.SetFloat(ColorGradingRenderer.PropertyIDs.Vibrance, this._volumeComponent.vibrance.value * 0.5f);
			this._effectMaterial.SetFloat(ColorGradingRenderer.PropertyIDs.Saturation, this._volumeComponent.saturation.value * 0.5f);
			CompoundRenderer.SetSourceSize(cmd, tempRTDescriptor);
			cmd.SetGlobalTexture(ColorGradingRenderer.PropertyIDs.Input, source);
			CoreUtils.DrawFullScreen(cmd, this._effectMaterial, destination, null, 0);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00003D74 File Offset: 0x00001F74
		public override void Dispose(bool disposing)
		{
		}

		// Token: 0x0400008D RID: 141
		private ColorGrading _volumeComponent;

		// Token: 0x0400008E RID: 142
		private Material _effectMaterial;

		// Token: 0x020000F4 RID: 244
		private static class PropertyIDs
		{
			// Token: 0x0400037F RID: 895
			internal static readonly int Input = Shader.PropertyToID("_MainTex");

			// Token: 0x04000380 RID: 896
			internal static readonly int Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x04000381 RID: 897
			internal static readonly int ShadowBezierPoints = Shader.PropertyToID("_ShadowBezierPoints");

			// Token: 0x04000382 RID: 898
			internal static readonly int HighlightBezierPoints = Shader.PropertyToID("_HighlightBezierPoints");

			// Token: 0x04000383 RID: 899
			internal static readonly int Contrast = Shader.PropertyToID("_Contrast");

			// Token: 0x04000384 RID: 900
			internal static readonly int Vibrance = Shader.PropertyToID("_Vibrance");

			// Token: 0x04000385 RID: 901
			internal static readonly int Saturation = Shader.PropertyToID("_Saturation");
		}
	}
}
