using System;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200007F RID: 127
	public sealed class BuiltinDebugViewsComponent : PostProcessingComponentCommandBuffer<BuiltinDebugViewsModel>
	{
		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000324 RID: 804 RVA: 0x0001F71B File Offset: 0x0001DB1B
		public override bool active
		{
			get
			{
				return base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Depth) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.Normals) || base.model.IsModeActive(BuiltinDebugViewsModel.Mode.MotionVectors);
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0001F750 File Offset: 0x0001DB50
		public override DepthTextureMode GetCameraFlags()
		{
			BuiltinDebugViewsModel.Mode mode = base.model.settings.mode;
			DepthTextureMode depthTextureMode = DepthTextureMode.None;
			if (mode != BuiltinDebugViewsModel.Mode.Normals)
			{
				if (mode != BuiltinDebugViewsModel.Mode.MotionVectors)
				{
					if (mode == BuiltinDebugViewsModel.Mode.Depth)
					{
						depthTextureMode |= DepthTextureMode.Depth;
					}
				}
				else
				{
					depthTextureMode |= (DepthTextureMode.Depth | DepthTextureMode.MotionVectors);
				}
			}
			else
			{
				depthTextureMode |= DepthTextureMode.DepthNormals;
			}
			return depthTextureMode;
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0001F7AC File Offset: 0x0001DBAC
		public override CameraEvent GetCameraEvent()
		{
			return (base.model.settings.mode != BuiltinDebugViewsModel.Mode.MotionVectors) ? CameraEvent.BeforeImageEffectsOpaque : CameraEvent.BeforeImageEffects;
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0001F7DB File Offset: 0x0001DBDB
		public override string GetName()
		{
			return "Builtin Debug Views";
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0001F7E4 File Offset: 0x0001DBE4
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			BuiltinDebugViewsModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			material.shaderKeywords = null;
			if (this.context.isGBufferAvailable)
			{
				material.EnableKeyword("SOURCE_GBUFFER");
			}
			BuiltinDebugViewsModel.Mode mode = settings.mode;
			if (mode != BuiltinDebugViewsModel.Mode.Depth)
			{
				if (mode != BuiltinDebugViewsModel.Mode.Normals)
				{
					if (mode == BuiltinDebugViewsModel.Mode.MotionVectors)
					{
						this.MotionVectorsPass(cb);
					}
				}
				else
				{
					this.DepthNormalsPass(cb);
				}
			}
			else
			{
				this.DepthPass(cb);
			}
			this.context.Interrupt();
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0001F888 File Offset: 0x0001DC88
		private void DepthPass(CommandBuffer cb)
		{
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.DepthSettings depth = base.model.settings.depth;
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._DepthScale, 1f / depth.scale);
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 0);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0001F8E8 File Offset: 0x0001DCE8
		private void DepthNormalsPass(CommandBuffer cb)
		{
			Material mat = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			cb.Blit(null, BuiltinRenderTextureType.CameraTarget, mat, 1);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0001F91C File Offset: 0x0001DD1C
		private void MotionVectorsPass(CommandBuffer cb)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Builtin Debug Views");
			BuiltinDebugViewsModel.MotionVectorsSettings motionVectors = base.model.settings.motionVectors;
			int nameID = BuiltinDebugViewsComponent.Uniforms._TempRT;
			cb.GetTemporaryRT(nameID, this.context.width, this.context.height, 0, FilterMode.Bilinear);
			cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.sourceOpacity);
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, BuiltinRenderTextureType.CameraTarget);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, nameID, material, 2);
			if (motionVectors.motionImageOpacity > 0f && motionVectors.motionImageAmplitude > 0f)
			{
				int tempRT = BuiltinDebugViewsComponent.Uniforms._TempRT2;
				cb.GetTemporaryRT(tempRT, this.context.width, this.context.height, 0, FilterMode.Bilinear);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionImageOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionImageAmplitude);
				cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, nameID);
				cb.Blit(nameID, tempRT, material, 3);
				cb.ReleaseTemporaryRT(nameID);
				nameID = tempRT;
			}
			if (motionVectors.motionVectorsOpacity > 0f && motionVectors.motionVectorsAmplitude > 0f)
			{
				this.PrepareArrows();
				float num = 1f / (float)motionVectors.motionVectorsResolution;
				float x = num * (float)this.context.height / (float)this.context.width;
				cb.SetGlobalVector(BuiltinDebugViewsComponent.Uniforms._Scale, new Vector2(x, num));
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Opacity, motionVectors.motionVectorsOpacity);
				cb.SetGlobalFloat(BuiltinDebugViewsComponent.Uniforms._Amplitude, motionVectors.motionVectorsAmplitude);
				cb.DrawMesh(this.m_Arrows.mesh, Matrix4x4.identity, material, 0, 4);
			}
			cb.SetGlobalTexture(BuiltinDebugViewsComponent.Uniforms._MainTex, nameID);
			cb.Blit(nameID, BuiltinRenderTextureType.CameraTarget);
			cb.ReleaseTemporaryRT(nameID);
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0001FB24 File Offset: 0x0001DF24
		private void PrepareArrows()
		{
			int motionVectorsResolution = base.model.settings.motionVectors.motionVectorsResolution;
			int num = motionVectorsResolution * Screen.width / Screen.height;
			if (this.m_Arrows == null)
			{
				this.m_Arrows = new BuiltinDebugViewsComponent.ArrowArray();
			}
			if (this.m_Arrows.columnCount != num || this.m_Arrows.rowCount != motionVectorsResolution)
			{
				this.m_Arrows.Release();
				this.m_Arrows.BuildMesh(num, motionVectorsResolution);
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0001FBA8 File Offset: 0x0001DFA8
		public override void OnDisable()
		{
			if (this.m_Arrows != null)
			{
				this.m_Arrows.Release();
			}
			this.m_Arrows = null;
		}

		// Token: 0x04000208 RID: 520
		private const string k_ShaderString = "Hidden/Post FX/Builtin Debug Views";

		// Token: 0x04000209 RID: 521
		private BuiltinDebugViewsComponent.ArrowArray m_Arrows;

		// Token: 0x02000080 RID: 128
		private static class Uniforms
		{
			// Token: 0x0400020A RID: 522
			internal static readonly int _DepthScale = Shader.PropertyToID("_DepthScale");

			// Token: 0x0400020B RID: 523
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");

			// Token: 0x0400020C RID: 524
			internal static readonly int _Opacity = Shader.PropertyToID("_Opacity");

			// Token: 0x0400020D RID: 525
			internal static readonly int _MainTex = Shader.PropertyToID("_MainTex");

			// Token: 0x0400020E RID: 526
			internal static readonly int _TempRT2 = Shader.PropertyToID("_TempRT2");

			// Token: 0x0400020F RID: 527
			internal static readonly int _Amplitude = Shader.PropertyToID("_Amplitude");

			// Token: 0x04000210 RID: 528
			internal static readonly int _Scale = Shader.PropertyToID("_Scale");
		}

		// Token: 0x02000081 RID: 129
		private enum Pass
		{
			// Token: 0x04000212 RID: 530
			Depth,
			// Token: 0x04000213 RID: 531
			Normals,
			// Token: 0x04000214 RID: 532
			MovecOpacity,
			// Token: 0x04000215 RID: 533
			MovecImaging,
			// Token: 0x04000216 RID: 534
			MovecArrows
		}

		// Token: 0x02000082 RID: 130
		private class ArrowArray
		{
			// Token: 0x17000047 RID: 71
			// (get) Token: 0x06000330 RID: 816 RVA: 0x0001FC46 File Offset: 0x0001E046
			// (set) Token: 0x06000331 RID: 817 RVA: 0x0001FC4E File Offset: 0x0001E04E
			public Mesh mesh { get; private set; }

			// Token: 0x17000048 RID: 72
			// (get) Token: 0x06000332 RID: 818 RVA: 0x0001FC57 File Offset: 0x0001E057
			// (set) Token: 0x06000333 RID: 819 RVA: 0x0001FC5F File Offset: 0x0001E05F
			public int columnCount { get; private set; }

			// Token: 0x17000049 RID: 73
			// (get) Token: 0x06000334 RID: 820 RVA: 0x0001FC68 File Offset: 0x0001E068
			// (set) Token: 0x06000335 RID: 821 RVA: 0x0001FC70 File Offset: 0x0001E070
			public int rowCount { get; private set; }

			// Token: 0x06000336 RID: 822 RVA: 0x0001FC7C File Offset: 0x0001E07C
			public void BuildMesh(int columns, int rows)
			{
				Vector3[] array = new Vector3[]
				{
					new Vector3(0f, 0f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(-1f, 1f, 0f),
					new Vector3(0f, 1f, 0f),
					new Vector3(1f, 1f, 0f)
				};
				int num = 6 * columns * rows;
				List<Vector3> list = new List<Vector3>(num);
				List<Vector2> list2 = new List<Vector2>(num);
				for (int i = 0; i < rows; i++)
				{
					for (int j = 0; j < columns; j++)
					{
						Vector2 item = new Vector2((0.5f + (float)j) / (float)columns, (0.5f + (float)i) / (float)rows);
						for (int k = 0; k < 6; k++)
						{
							list.Add(array[k]);
							list2.Add(item);
						}
					}
				}
				int[] array2 = new int[num];
				for (int l = 0; l < num; l++)
				{
					array2[l] = l;
				}
				this.mesh = new Mesh
				{
					hideFlags = HideFlags.DontSave
				};
				this.mesh.SetVertices(list);
				this.mesh.SetUVs(0, list2);
				this.mesh.SetIndices(array2, MeshTopology.Lines, 0);
				this.mesh.UploadMeshData(true);
				this.columnCount = columns;
				this.rowCount = rows;
			}

			// Token: 0x06000337 RID: 823 RVA: 0x0001FE5F File Offset: 0x0001E25F
			public void Release()
			{
				GraphicsUtils.Destroy(this.mesh);
				this.mesh = null;
			}
		}
	}
}
