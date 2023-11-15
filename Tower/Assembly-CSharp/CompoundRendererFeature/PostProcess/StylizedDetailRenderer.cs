using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.PostProcessing;

namespace CompoundRendererFeature.PostProcess
{
	// Token: 0x02000050 RID: 80
	[CompoundRendererFeature("Stylized Detail", InjectionPoint.BeforePostProcess, false)]
	public class StylizedDetailRenderer : CompoundRenderer
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x00003E10 File Offset: 0x00002010
		public override bool visibleInSceneView
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D8 RID: 216 RVA: 0x00003E13 File Offset: 0x00002013
		public override ScriptableRenderPassInput input
		{
			get
			{
				return ScriptableRenderPassInput.Depth | ScriptableRenderPassInput.Color;
			}
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00003E16 File Offset: 0x00002016
		public override void Initialize()
		{
			base.Initialize();
			this._effectMaterial = CoreUtils.CreateEngineMaterial("Hidden/CompoundRendererFeature/StylizedDetail");
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00003E30 File Offset: 0x00002030
		public override bool Setup(in RenderingData renderingData, InjectionPoint injectionPoint)
		{
			base.Setup(renderingData, injectionPoint);
			VolumeStack stack = VolumeManager.instance.stack;
			this._volumeComponent = stack.GetComponent<StylizedDetail>();
			return this._volumeComponent.intensity.value > 0f;
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00003E74 File Offset: 0x00002074
		public override void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, ref RenderingData renderingData, InjectionPoint injectionPoint)
		{
			RenderTextureDescriptor tempRTDescriptor = CompoundRenderer.GetTempRTDescriptor(renderingData);
			int num = tempRTDescriptor.width / 1;
			int height = tempRTDescriptor.height / 1;
			float num2 = this._volumeComponent.blur.value * ((float)num / 1080f);
			num2 = Mathf.Min(num2, 2f);
			float num3 = this._volumeComponent.edgePreserve.value * ((float)num / 1080f);
			num3 = Mathf.Min(num3, 2f);
			float x = this._volumeComponent.rangeStart.overrideState ? this._volumeComponent.rangeStart.value : 0f;
			float y = this._volumeComponent.rangeEnd.overrideState ? this._volumeComponent.rangeEnd.value : -1f;
			this._effectMaterial.SetVector(StylizedDetailRenderer.PropertyIDs.CoCParams, new Vector2(x, y));
			this._effectMaterial.SetFloat(StylizedDetailRenderer.PropertyIDs.Intensity, this._volumeComponent.intensity.value);
			CompoundRenderer.SetSourceSize(cmd, tempRTDescriptor);
			RenderTextureDescriptor tempRTDescriptor2 = CompoundRenderer.GetTempRTDescriptor(renderingData, num, height, this._defaultHDRFormat);
			cmd.GetTemporaryRT(StylizedDetailRenderer.PropertyIDs.PingTexture, tempRTDescriptor2, FilterMode.Bilinear);
			cmd.GetTemporaryRT(StylizedDetailRenderer.PropertyIDs.Blur1, tempRTDescriptor2, FilterMode.Bilinear);
			cmd.GetTemporaryRT(StylizedDetailRenderer.PropertyIDs.Blur2, tempRTDescriptor2, FilterMode.Bilinear);
			cmd.SetGlobalVector(StylizedDetailRenderer.PropertyIDs.DownSampleScaleFactor, new Vector4(1f, 1f, 1f, 1f));
			cmd.SetGlobalFloat(StylizedDetailRenderer.PropertyIDs.BlurStrength, num3);
			cmd.SetGlobalTexture(StylizedDetailRenderer.PropertyIDs.Input, source);
			CoreUtils.DrawFullScreen(cmd, this._effectMaterial, StylizedDetailRenderer.PropertyIDs.PingTexture, null, 1);
			cmd.SetGlobalTexture(StylizedDetailRenderer.PropertyIDs.Input, StylizedDetailRenderer.PropertyIDs.PingTexture);
			CoreUtils.DrawFullScreen(cmd, this._effectMaterial, StylizedDetailRenderer.PropertyIDs.Blur1, null, 2);
			cmd.SetGlobalFloat(StylizedDetailRenderer.PropertyIDs.BlurStrength, num2);
			cmd.SetGlobalTexture(StylizedDetailRenderer.PropertyIDs.Input, StylizedDetailRenderer.PropertyIDs.Blur1);
			CoreUtils.DrawFullScreen(cmd, this._effectMaterial, StylizedDetailRenderer.PropertyIDs.PingTexture, null, 1);
			cmd.SetGlobalTexture(StylizedDetailRenderer.PropertyIDs.Input, StylizedDetailRenderer.PropertyIDs.PingTexture);
			CoreUtils.DrawFullScreen(cmd, this._effectMaterial, StylizedDetailRenderer.PropertyIDs.Blur2, null, 2);
			cmd.SetGlobalTexture(StylizedDetailRenderer.PropertyIDs.Input, source);
			CoreUtils.DrawFullScreen(cmd, this._effectMaterial, destination, null, 0);
			cmd.ReleaseTemporaryRT(StylizedDetailRenderer.PropertyIDs.PingTexture);
			cmd.ReleaseTemporaryRT(StylizedDetailRenderer.PropertyIDs.Blur1);
			cmd.ReleaseTemporaryRT(StylizedDetailRenderer.PropertyIDs.Blur2);
		}

		// Token: 0x060000DC RID: 220 RVA: 0x000040E6 File Offset: 0x000022E6
		public override void Dispose(bool disposing)
		{
		}

		// Token: 0x04000094 RID: 148
		private StylizedDetail _volumeComponent;

		// Token: 0x04000095 RID: 149
		private Material _effectMaterial;

		// Token: 0x020000F5 RID: 245
		private static class PropertyIDs
		{
			// Token: 0x04000386 RID: 902
			internal static readonly int Input = Shader.PropertyToID("_MainTex");

			// Token: 0x04000387 RID: 903
			internal static readonly int PingTexture = Shader.PropertyToID("_PingTexture");

			// Token: 0x04000388 RID: 904
			internal static readonly int BlurStrength = Shader.PropertyToID("_BlurStrength");

			// Token: 0x04000389 RID: 905
			internal static readonly int Blur1 = Shader.PropertyToID("_BlurTex1");

			// Token: 0x0400038A RID: 906
			internal static readonly int Blur2 = Shader.PropertyToID("_BlurTex2");

			// Token: 0x0400038B RID: 907
			internal static readonly int Intensity = Shader.PropertyToID("_Intensity");

			// Token: 0x0400038C RID: 908
			internal static readonly int DownSampleScaleFactor = Shader.PropertyToID("_DownSampleScaleFactor");

			// Token: 0x0400038D RID: 909
			public static readonly int CoCParams = Shader.PropertyToID("_CoCParams");
		}
	}
}
