using System;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering.Universal.PostProcessing
{
	// Token: 0x020000BD RID: 189
	public abstract class CompoundRenderer : IDisposable
	{
		// Token: 0x1700003A RID: 58
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000AB30 File Offset: 0x00008D30
		public virtual bool visibleInSceneView
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x0600029B RID: 667 RVA: 0x0000AB33 File Offset: 0x00008D33
		public virtual ScriptableRenderPassInput input
		{
			get
			{
				return ScriptableRenderPassInput.Color;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000AB36 File Offset: 0x00008D36
		public bool Initialized
		{
			get
			{
				return this._initialized;
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000AB3E File Offset: 0x00008D3E
		internal void InitializeInternal()
		{
			this.Initialize();
			this._initialized = true;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000AB4D File Offset: 0x00008D4D
		public virtual void Initialize()
		{
			if (SystemInfo.IsFormatSupported(GraphicsFormat.B10G11R11_UFloatPack32, FormatUsage.Blend))
			{
				this._defaultHDRFormat = GraphicsFormat.B10G11R11_UFloatPack32;
				this._useRGBM = false;
				return;
			}
			this._defaultHDRFormat = ((QualitySettings.activeColorSpace == ColorSpace.Linear) ? GraphicsFormat.R8G8B8A8_SRGB : GraphicsFormat.R8G8B8A8_UNorm);
			this._useRGBM = true;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000AB82 File Offset: 0x00008D82
		public virtual bool Setup(in RenderingData renderingData, InjectionPoint injectionPoint)
		{
			return true;
		}

		// Token: 0x060002A0 RID: 672
		public abstract void Render(CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier destination, ref RenderingData renderingData, InjectionPoint injectionPoint);

		// Token: 0x060002A1 RID: 673 RVA: 0x0000AB85 File Offset: 0x00008D85
		public virtual void Dispose(bool disposing)
		{
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000AB87 File Offset: 0x00008D87
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000AB98 File Offset: 0x00008D98
		public static RenderTextureDescriptor GetTempRTDescriptor(in RenderingData renderingData)
		{
			RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			cameraTargetDescriptor.depthBufferBits = 0;
			cameraTargetDescriptor.msaaSamples = 1;
			return cameraTargetDescriptor;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x0000ABC4 File Offset: 0x00008DC4
		public static RenderTextureDescriptor GetTempRTDescriptor(in RenderingData renderingData, int width, int height, GraphicsFormat format)
		{
			if (width <= 0 || height <= 0)
			{
				Debug.LogError(string.Format("Invalid parameters for GetTempRTDescriptor: {0}, {1}.", width, height));
			}
			RenderTextureDescriptor tempRTDescriptor = CompoundRenderer.GetTempRTDescriptor(renderingData);
			tempRTDescriptor.width = width;
			tempRTDescriptor.height = height;
			return tempRTDescriptor;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0000AC0C File Offset: 0x00008E0C
		public static void SetSourceSize(CommandBuffer cmd, RenderTextureDescriptor desc)
		{
			float num = (float)desc.width;
			float num2 = (float)desc.height;
			if (desc.useDynamicScale)
			{
				num *= ScalableBufferManager.widthScaleFactor;
				num2 *= ScalableBufferManager.heightScaleFactor;
			}
			cmd.SetGlobalVector(CompoundRenderer.ShaderConstants._SourceSize, new Vector4(num, num2, 1f / num, 1f / num2));
		}

		// Token: 0x04000231 RID: 561
		private bool _initialized;

		// Token: 0x04000232 RID: 562
		protected GraphicsFormat _defaultHDRFormat;

		// Token: 0x04000233 RID: 563
		protected bool _useRGBM;

		// Token: 0x0200010B RID: 267
		private static class ShaderConstants
		{
			// Token: 0x040003E2 RID: 994
			public static readonly int _SourceSize = Shader.PropertyToID("_SourceSize");
		}
	}
}
