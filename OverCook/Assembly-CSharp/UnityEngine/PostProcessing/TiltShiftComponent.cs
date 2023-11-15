using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200009E RID: 158
	public sealed class TiltShiftComponent : PostProcessingComponentCommandBuffer<TiltShiftModel>
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060003B0 RID: 944 RVA: 0x0002411B File Offset: 0x0002251B
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x0002413E File Offset: 0x0002253E
		public override string GetName()
		{
			return "Tilt Shift";
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00024145 File Offset: 0x00022545
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.BeforeImageEffects;
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x0002414C File Offset: 0x0002254C
		public override void Init(PostProcessingContext context, TiltShiftModel model)
		{
			base.Init(context, model);
			if (model.settings.BlurArea != this.m_LastArea)
			{
				List<Vector3> list = new List<Vector3>();
				float blurArea = model.settings.BlurArea;
				this.m_LastArea = blurArea;
				list.Add(new Vector3(-1f, -1f));
				list.Add(new Vector3(1f, -1f + blurArea));
				list.Add(new Vector3(-1f, -1f + blurArea));
				list.Add(new Vector3(1f, -1f));
				List<Vector2> list2 = new List<Vector2>();
				list2.Add(new Vector2(0f, 1f));
				list2.Add(new Vector2(1f, 1f - blurArea * 0.5f));
				list2.Add(new Vector2(0f, 1f - blurArea * 0.5f));
				list2.Add(new Vector2(1f, 1f));
				int[] indices = new int[]
				{
					0,
					1,
					2,
					0,
					3,
					1
				};
				this.m_TopMesh = new Mesh();
				this.m_TopMesh.SetVertices(list);
				this.m_TopMesh.SetUVs(0, list2);
				this.m_TopMesh.SetIndices(indices, MeshTopology.Triangles, 0);
				list.Clear();
				list2.Clear();
				list.Add(new Vector3(-1f, 1f - blurArea));
				list.Add(new Vector3(1f, 1f));
				list.Add(new Vector3(-1f, 1f));
				list.Add(new Vector3(1f, 1f - blurArea));
				list2.Add(new Vector2(0f, blurArea * 0.5f));
				list2.Add(new Vector2(1f, 0f));
				list2.Add(new Vector2(0f, 0f));
				list2.Add(new Vector2(1f, blurArea * 0.5f));
				this.m_BottomMesh = new Mesh();
				this.m_BottomMesh.SetVertices(list);
				this.m_BottomMesh.SetUVs(0, list2);
				this.m_BottomMesh.SetIndices(indices, MeshTopology.Triangles, 0);
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0002439C File Offset: 0x0002279C
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			TiltShiftModel.Settings settings = base.model.settings;
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Blit");
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Tilt Shift");
			material.SetFloat(TiltShiftComponent.Uniforms._BlurArea, settings.BlurArea);
			cb.Clear();
			int width = this.context.width;
			int height = this.context.height;
			cb.SetGlobalTexture(TiltShiftComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
			if (settings.Downsample)
			{
				cb.GetTemporaryRT(TiltShiftComponent.Uniforms._MainDownsampledHalf, width / 2, height / 2, 0, FilterMode.Bilinear, RenderTextureFormat.Default);
				cb.GetTemporaryRT(TiltShiftComponent.Uniforms._MainDownsampled, width / 4, height / 4, 0, FilterMode.Bilinear, RenderTextureFormat.Default);
				cb.GetTemporaryRT(TiltShiftComponent.Uniforms._Blurred, width / 4, height / 4, 0, FilterMode.Bilinear, RenderTextureFormat.Default);
				cb.Blit(BuiltinRenderTextureType.CameraTarget, TiltShiftComponent.Uniforms._MainDownsampledHalf, mat, 0);
				cb.Blit(TiltShiftComponent.Uniforms._MainDownsampledHalf, TiltShiftComponent.Uniforms._MainDownsampled, mat, 0);
				cb.ReleaseTemporaryRT(TiltShiftComponent.Uniforms._MainDownsampledHalf);
			}
			else
			{
				cb.GetTemporaryRT(TiltShiftComponent.Uniforms._MainDownsampled, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.Default);
				cb.GetTemporaryRT(TiltShiftComponent.Uniforms._Blurred, width, height, 0, FilterMode.Bilinear, RenderTextureFormat.Default);
				cb.Blit(BuiltinRenderTextureType.CameraTarget, TiltShiftComponent.Uniforms._MainDownsampled, mat, 0);
			}
			for (int i = 0; i < settings.Iterations; i++)
			{
				cb.Blit(TiltShiftComponent.Uniforms._MainDownsampled, TiltShiftComponent.Uniforms._Blurred, material, 0);
				cb.Blit(TiltShiftComponent.Uniforms._Blurred, TiltShiftComponent.Uniforms._MainDownsampled, material, 1);
			}
			cb.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
			cb.DrawMesh(this.m_TopMesh, Matrix4x4.identity, material, 0, 2);
			cb.DrawMesh(this.m_BottomMesh, Matrix4x4.identity, material, 0, 2);
			cb.ReleaseTemporaryRT(TiltShiftComponent.Uniforms._Blurred);
			cb.ReleaseTemporaryRT(TiltShiftComponent.Uniforms._MainDownsampled);
		}

		// Token: 0x040002D6 RID: 726
		private Mesh m_TopMesh;

		// Token: 0x040002D7 RID: 727
		private Mesh m_BottomMesh;

		// Token: 0x040002D8 RID: 728
		private float m_LastArea = -1f;

		// Token: 0x040002D9 RID: 729
		private const string k_BlitShaderString = "Hidden/Post FX/Blit";

		// Token: 0x040002DA RID: 730
		private const string k_ShaderString = "Hidden/Post FX/Tilt Shift";

		// Token: 0x0200009F RID: 159
		private static class Uniforms
		{
			// Token: 0x040002DB RID: 731
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x040002DC RID: 732
			internal static readonly int _MainDownsampled = Shader.PropertyToID("_MainDownsampled");

			// Token: 0x040002DD RID: 733
			internal static readonly int _MainDownsampledHalf = Shader.PropertyToID("_MainDownsampledHalf");

			// Token: 0x040002DE RID: 734
			internal static readonly int _Blurred = Shader.PropertyToID("_Blurred");

			// Token: 0x040002DF RID: 735
			internal static readonly int _BlurStrength = Shader.PropertyToID("_BlurStrength");

			// Token: 0x040002E0 RID: 736
			internal static readonly int _BlurArea = Shader.PropertyToID("_BlurArea");
		}
	}
}
