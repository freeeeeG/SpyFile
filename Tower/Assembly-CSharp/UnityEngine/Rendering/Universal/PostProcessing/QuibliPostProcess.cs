using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal.PostProcessing
{
	// Token: 0x020000BF RID: 191
	public class QuibliPostProcess : ScriptableRendererFeature
	{
		// Token: 0x060002AC RID: 684 RVA: 0x0000ACDC File Offset: 0x00008EDC
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (renderingData.cameraData.postProcessEnabled)
			{
				if (this._afterOpaqueAndSky.HasPostProcessRenderers && this._afterOpaqueAndSky.PrepareRenderers(renderingData))
				{
					renderer.EnqueuePass(this._afterOpaqueAndSky);
				}
				if (this._beforePostProcess.HasPostProcessRenderers && this._beforePostProcess.PrepareRenderers(renderingData))
				{
					renderer.EnqueuePass(this._beforePostProcess);
				}
				if (this._afterPostProcess.HasPostProcessRenderers && this._afterPostProcess.PrepareRenderers(renderingData))
				{
					renderer.EnqueuePass(this._afterPostProcess);
				}
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x0000AD6B File Offset: 0x00008F6B
		public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
		{
			this.SetupRenderPassesCore(renderer, renderingData);
		}

		// Token: 0x060002AE RID: 686 RVA: 0x0000AD78 File Offset: 0x00008F78
		private void SetupRenderPassesCore(ScriptableRenderer renderer, in RenderingData renderingData)
		{
			RTHandle cameraColorTargetHandle = renderer.cameraColorTargetHandle;
			if (this._afterOpaqueAndSky.HasPostProcessRenderers && this._afterOpaqueAndSky.PrepareRenderers(renderingData))
			{
				this._afterOpaqueAndSky.Setup(cameraColorTargetHandle, cameraColorTargetHandle);
			}
			if (this._beforePostProcess.HasPostProcessRenderers && this._beforePostProcess.PrepareRenderers(renderingData))
			{
				this._beforePostProcess.Setup(cameraColorTargetHandle, cameraColorTargetHandle);
			}
			if (this._afterPostProcess.HasPostProcessRenderers && this._afterPostProcess.PrepareRenderers(renderingData))
			{
				this._afterPostProcess.Setup(cameraColorTargetHandle, cameraColorTargetHandle);
			}
		}

		// Token: 0x060002AF RID: 687 RVA: 0x0000AE24 File Offset: 0x00009024
		public override void Create()
		{
			this._afterPostProcessColor.Init("_AfterPostProcessTexture");
			Dictionary<string, CompoundRenderer> shared = new Dictionary<string, CompoundRenderer>();
			this._afterOpaqueAndSky = new CompoundPass(InjectionPoint.AfterOpaqueAndSky, this.InstantiateRenderers(this.settings.renderersAfterOpaqueAndSky, shared));
			this._beforePostProcess = new CompoundPass(InjectionPoint.BeforePostProcess, this.InstantiateRenderers(this.settings.renderersBeforePostProcess, shared));
			this._afterPostProcess = new CompoundPass(InjectionPoint.AfterPostProcess, this.InstantiateRenderers(this.settings.renderersAfterPostProcess, shared));
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000AEA4 File Offset: 0x000090A4
		private List<CompoundRenderer> InstantiateRenderers(List<string> names, Dictionary<string, CompoundRenderer> shared)
		{
			List<CompoundRenderer> list = new List<CompoundRenderer>(names.Count);
			foreach (string text in names)
			{
				CompoundRenderer compoundRenderer;
				if (shared.TryGetValue(text, out compoundRenderer))
				{
					list.Add(compoundRenderer);
				}
				else
				{
					Type type = Type.GetType(text);
					if (!(type == null) && type.IsSubclassOf(typeof(CompoundRenderer)))
					{
						CompoundRendererFeatureAttribute attribute = CompoundRendererFeatureAttribute.GetAttribute(type);
						if (attribute != null)
						{
							compoundRenderer = (Activator.CreateInstance(type) as CompoundRenderer);
							list.Add(compoundRenderer);
							if (attribute.ShareInstance)
							{
								shared.Add(text, compoundRenderer);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x04000237 RID: 567
		[SerializeField]
		public QuibliPostProcess.Settings settings = new QuibliPostProcess.Settings();

		// Token: 0x04000238 RID: 568
		private CompoundPass _afterOpaqueAndSky;

		// Token: 0x04000239 RID: 569
		private CompoundPass _beforePostProcess;

		// Token: 0x0400023A RID: 570
		private CompoundPass _afterPostProcess;

		// Token: 0x0400023B RID: 571
		private RenderTargetHandle _afterPostProcessColor;

		// Token: 0x0200010C RID: 268
		[Serializable]
		public class Settings
		{
			// Token: 0x060003EC RID: 1004 RVA: 0x00010AAB File Offset: 0x0000ECAB
			public Settings()
			{
				this.renderersAfterOpaqueAndSky = new List<string>();
				this.renderersBeforePostProcess = new List<string>();
				this.renderersAfterPostProcess = new List<string>();
			}

			// Token: 0x040003E3 RID: 995
			[SerializeField]
			public List<string> renderersAfterOpaqueAndSky;

			// Token: 0x040003E4 RID: 996
			[SerializeField]
			public List<string> renderersBeforePostProcess;

			// Token: 0x040003E5 RID: 997
			[SerializeField]
			public List<string> renderersAfterPostProcess;
		}
	}
}
