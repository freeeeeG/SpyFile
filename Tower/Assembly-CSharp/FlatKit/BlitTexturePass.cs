using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FlatKit
{
	// Token: 0x02000052 RID: 82
	internal class BlitTexturePass : ScriptableRenderPass
	{
		// Token: 0x060000E2 RID: 226 RVA: 0x00004184 File Offset: 0x00002384
		public void Setup(Material effectMaterial, bool useDepth, bool useNormals, bool useColor)
		{
			this._effectMaterial = effectMaterial;
			string str = effectMaterial.name.Substring(effectMaterial.name.LastIndexOf('/') + 1);
			this._profilingSampler = new ProfilingSampler("Blit " + str);
			if (this._copyMaterial == null)
			{
				this._copyMaterial = CoreUtils.CreateEngineMaterial(BlitTexturePass.CopyEffectShaderName);
			}
			base.ConfigureInput((useColor ? ScriptableRenderPassInput.Color : ScriptableRenderPassInput.None) | (useDepth ? ScriptableRenderPassInput.Depth : ScriptableRenderPassInput.None) | (useNormals ? ScriptableRenderPassInput.Normal : ScriptableRenderPassInput.None));
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00004205 File Offset: 0x00002405
		public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
		{
			base.ConfigureTarget(new RenderTargetIdentifier(renderingData.cameraData.renderer.cameraColorTarget, 0, CubemapFace.Unknown, -1));
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00004228 File Offset: 0x00002428
		public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
		{
			if (this._effectMaterial == null)
			{
				return;
			}
			if (renderingData.cameraData.camera.cameraType != CameraType.Game)
			{
				return;
			}
			this._temporaryColorTexture = default(RenderTargetHandle);
			CommandBuffer commandBuffer = CommandBufferPool.Get();
			using (new ProfilingScope(commandBuffer, this._profilingSampler))
			{
				RenderTextureDescriptor cameraTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
				cameraTargetDescriptor.depthBufferBits = 0;
				BlitTexturePass.SetSourceSize(commandBuffer, cameraTargetDescriptor);
				RTHandle cameraColorTargetHandle = renderingData.cameraData.renderer.cameraColorTargetHandle;
				commandBuffer.GetTemporaryRT(this._temporaryColorTexture.id, cameraTargetDescriptor);
				if (renderingData.cameraData.xrRendering)
				{
					this._effectMaterial.EnableKeyword("_USE_DRAW_PROCEDURAL");
					commandBuffer.SetRenderTarget(this._temporaryColorTexture.Identifier());
					commandBuffer.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, this._effectMaterial, 0, 0);
					commandBuffer.SetGlobalTexture("_EffectTexture", this._temporaryColorTexture.Identifier());
					commandBuffer.SetRenderTarget(new RenderTargetIdentifier(cameraColorTargetHandle, 0, CubemapFace.Unknown, -1));
					commandBuffer.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, this._copyMaterial, 0, 0);
				}
				else
				{
					this._effectMaterial.DisableKeyword("_USE_DRAW_PROCEDURAL");
					commandBuffer.Blit(cameraColorTargetHandle, this._temporaryColorTexture.Identifier(), this._effectMaterial, 0);
					commandBuffer.Blit(this._temporaryColorTexture.Identifier(), cameraColorTargetHandle);
				}
			}
			context.ExecuteCommandBuffer(commandBuffer);
			commandBuffer.Clear();
			CommandBufferPool.Release(commandBuffer);
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x000043C8 File Offset: 0x000025C8
		private static void SetSourceSize(CommandBuffer cmd, RenderTextureDescriptor desc)
		{
			float num = (float)desc.width;
			float num2 = (float)desc.height;
			if (desc.useDynamicScale)
			{
				num *= ScalableBufferManager.widthScaleFactor;
				num2 *= ScalableBufferManager.heightScaleFactor;
			}
			cmd.SetGlobalVector("_SourceSize", new Vector4(num, num2, 1f / num, 1f / num2));
		}

		// Token: 0x0400009B RID: 155
		public static readonly string CopyEffectShaderName = "Hidden/FlatKit/CopyTexture";

		// Token: 0x0400009C RID: 156
		private ProfilingSampler _profilingSampler;

		// Token: 0x0400009D RID: 157
		private Material _effectMaterial;

		// Token: 0x0400009E RID: 158
		private Material _copyMaterial;

		// Token: 0x0400009F RID: 159
		private RenderTargetHandle _temporaryColorTexture;
	}
}
