using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering.Universal.PostProcessing
{
	// Token: 0x020000BB RID: 187
	public class CompoundPass : ScriptableRenderPass
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000293 RID: 659 RVA: 0x0000A695 File Offset: 0x00008895
		public bool HasPostProcessRenderers
		{
			get
			{
				return this.m_PostProcessRenderers.Count != 0;
			}
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0000A6A8 File Offset: 0x000088A8
		public CompoundPass(InjectionPoint injectionPoint, List<CompoundRenderer> renderers)
		{
			this._injectionPoint = injectionPoint;
			this.m_ProfilingSamplers = new List<ProfilingSampler>(renderers.Count);
			this.m_PostProcessRenderers = renderers;
			foreach (CompoundRenderer compoundRenderer in renderers)
			{
				CompoundRendererFeatureAttribute attribute = CompoundRendererFeatureAttribute.GetAttribute(compoundRenderer.GetType());
				this.m_ProfilingSamplers.Add(new ProfilingSampler((attribute != null) ? attribute.Name : null));
			}
			this.m_ActivePostProcessRenderers = new List<int>(renderers.Count);
			switch (injectionPoint)
			{
			case InjectionPoint.AfterOpaqueAndSky:
				base.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
				this.m_PassName = "[Dustyroom] PostProcess after Opaque & Sky";
				break;
			case InjectionPoint.BeforePostProcess:
				base.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
				this.m_PassName = "[Dustyroom] PostProcess before PostProcess";
				break;
			case InjectionPoint.AfterPostProcess:
				base.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
				this.m_PassName = "[Dustyroom] PostProcess after PostProcess";
				break;
			}
			this.m_Intermediate = new RenderTargetHandle[2];
			this.m_Intermediate[0].Init("_IntermediateRT0");
			this.m_Intermediate[1].Init("_IntermediateRT1");
			this.m_IntermediateAllocated = new bool[2];
			this.m_IntermediateAllocated[0] = false;
			this.m_IntermediateAllocated[1] = false;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x0000A804 File Offset: 0x00008A04
		private RenderTargetIdentifier GetIntermediate(CommandBuffer cmd, int index)
		{
			if (!this.m_IntermediateAllocated[index])
			{
				cmd.GetTemporaryRT(this.m_Intermediate[index].id, this._intermediateDescriptor);
				this.m_IntermediateAllocated[index] = true;
			}
			return this.m_Intermediate[index].Identifier();
		}

		// Token: 0x06000296 RID: 662 RVA: 0x0000A854 File Offset: 0x00008A54
		private void CleanupIntermediate(CommandBuffer cmd)
		{
			for (int i = 0; i < 2; i++)
			{
				if (this.m_IntermediateAllocated[i])
				{
					cmd.ReleaseTemporaryRT(this.m_Intermediate[i].id);
					this.m_IntermediateAllocated[i] = false;
				}
			}
		}

		// Token: 0x06000297 RID: 663 RVA: 0x0000A897 File Offset: 0x00008A97
		public void Setup(RenderTargetIdentifier source, RenderTargetIdentifier destination)
		{
			this.m_Source = source;
			this.m_Destination = destination;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000A8A8 File Offset: 0x00008AA8
		public bool PrepareRenderers(in RenderingData renderingData)
		{
			if (renderingData.cameraData.cameraType == CameraType.Preview)
			{
				return false;
			}
			bool flag = renderingData.cameraData.cameraType == CameraType.SceneView;
			ScriptableRenderPassInput scriptableRenderPassInput = ScriptableRenderPassInput.None;
			this.m_ActivePostProcessRenderers.Clear();
			for (int i = 0; i < this.m_PostProcessRenderers.Count; i++)
			{
				CompoundRenderer compoundRenderer = this.m_PostProcessRenderers[i];
				if ((!flag || compoundRenderer.visibleInSceneView) && compoundRenderer.Setup(renderingData, this._injectionPoint))
				{
					this.m_ActivePostProcessRenderers.Add(i);
					scriptableRenderPassInput |= compoundRenderer.input;
				}
			}
			base.ConfigureInput(scriptableRenderPassInput);
			return this.m_ActivePostProcessRenderers.Count != 0;
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000A948 File Offset: 0x00008B48
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			this._intermediateDescriptor = renderingData.cameraData.cameraTargetDescriptor;
			this._intermediateDescriptor.msaaSamples = 1;
			this._intermediateDescriptor.depthBufferBits = 0;
			CommandBuffer commandBuffer = CommandBufferPool.Get(this.m_PassName);
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			int width = this._intermediateDescriptor.width;
			int height = this._intermediateDescriptor.height;
			commandBuffer.SetGlobalVector("_ScreenSize", new Vector4((float)width, (float)height, 1f / (float)width, 1f / (float)height));
			bool flag = false;
			int num = 0;
			for (int i = 0; i < this.m_ActivePostProcessRenderers.Count; i++)
			{
				int index = this.m_ActivePostProcessRenderers[i];
				CompoundRenderer compoundRenderer = this.m_PostProcessRenderers[index];
				RenderTargetIdentifier source;
				RenderTargetIdentifier destination;
				if (i == 0)
				{
					source = this.m_Source;
					if (this.m_ActivePostProcessRenderers.Count == 1)
					{
						if (this.m_Source == this.m_Destination)
						{
							destination = this.GetIntermediate(commandBuffer, 0);
							flag = true;
						}
						else
						{
							destination = this.m_Destination;
						}
					}
					else
					{
						destination = this.GetIntermediate(commandBuffer, num);
					}
				}
				else
				{
					source = this.GetIntermediate(commandBuffer, num);
					if (i == this.m_ActivePostProcessRenderers.Count - 1)
					{
						destination = this.m_Destination;
					}
					else
					{
						num = 1 - num;
						destination = this.GetIntermediate(commandBuffer, num);
					}
				}
				using (new ProfilingScope(commandBuffer, this.m_ProfilingSamplers[index]))
				{
					if (!compoundRenderer.Initialized)
					{
						compoundRenderer.InitializeInternal();
					}
					compoundRenderer.Render(commandBuffer, source, destination, ref renderingData, this._injectionPoint);
				}
			}
			if (flag)
			{
				base.Blit(commandBuffer, this.m_Intermediate[0].Identifier(), this.m_Destination, null, 0);
			}
			this.CleanupIntermediate(commandBuffer);
			context.ExecuteCommandBuffer(commandBuffer);
			CommandBufferPool.Release(commandBuffer);
		}

		// Token: 0x04000223 RID: 547
		private readonly InjectionPoint _injectionPoint;

		// Token: 0x04000224 RID: 548
		private string m_PassName;

		// Token: 0x04000225 RID: 549
		private List<CompoundRenderer> m_PostProcessRenderers;

		// Token: 0x04000226 RID: 550
		private List<int> m_ActivePostProcessRenderers;

		// Token: 0x04000227 RID: 551
		private RenderTargetHandle[] m_Intermediate;

		// Token: 0x04000228 RID: 552
		private bool[] m_IntermediateAllocated;

		// Token: 0x04000229 RID: 553
		private RenderTextureDescriptor _intermediateDescriptor;

		// Token: 0x0400022A RID: 554
		private RenderTargetIdentifier m_Source;

		// Token: 0x0400022B RID: 555
		private RenderTargetIdentifier m_Destination;

		// Token: 0x0400022C RID: 556
		private List<ProfilingSampler> m_ProfilingSamplers;
	}
}
