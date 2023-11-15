using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace FlatKit
{
	// Token: 0x02000055 RID: 85
	public class FlatKitOutline : ScriptableRendererFeature
	{
		// Token: 0x060000F3 RID: 243 RVA: 0x00004A10 File Offset: 0x00002C10
		public override void Create()
		{
			if (this.settings == null)
			{
				Debug.LogWarning("[FlatKit] Missing Outline Settings");
				return;
			}
			if (this._blitShader == null)
			{
				this._blitShader = Shader.Find(BlitTexturePass.CopyEffectShaderName);
			}
			if (this._blitTexturePass == null)
			{
				this._blitTexturePass = new BlitTexturePass
				{
					renderPassEvent = this.settings.renderEvent
				};
			}
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00004A74 File Offset: 0x00002C74
		public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
		{
			if (this.settings == null)
			{
				Debug.LogWarning("[FlatKit] Missing Outline Settings");
				return;
			}
			if (!this.CreateMaterials())
			{
				return;
			}
			this.SetMaterialProperties();
			this._blitTexturePass.Setup(this._effectMaterial, this.settings.useDepth, this.settings.useNormals, true);
			renderer.EnqueuePass(this._blitTexturePass);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004ADD File Offset: 0x00002CDD
		protected override void Dispose(bool disposing)
		{
			CoreUtils.Destroy(this._effectMaterial);
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004AEC File Offset: 0x00002CEC
		private bool CreateMaterials()
		{
			if (this._effectMaterial == null)
			{
				Shader shader = Shader.Find(FlatKitOutline.OutlineShaderName);
				Shader x = Shader.Find(BlitTexturePass.CopyEffectShaderName);
				if (shader == null || x == null)
				{
					return false;
				}
				this._effectMaterial = CoreUtils.CreateEngineMaterial(shader);
			}
			return true;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004B40 File Offset: 0x00002D40
		private void SetMaterialProperties()
		{
			if (this._effectMaterial == null)
			{
				return;
			}
			if (this.settings.useDepth)
			{
				this._effectMaterial.EnableKeyword("OUTLINE_USE_DEPTH");
			}
			else
			{
				this._effectMaterial.DisableKeyword("OUTLINE_USE_DEPTH");
			}
			if (this.settings.useNormals)
			{
				this._effectMaterial.EnableKeyword("OUTLINE_USE_NORMALS");
			}
			else
			{
				this._effectMaterial.DisableKeyword("OUTLINE_USE_NORMALS");
			}
			if (this.settings.useColor)
			{
				this._effectMaterial.EnableKeyword("OUTLINE_USE_COLOR");
			}
			else
			{
				this._effectMaterial.DisableKeyword("OUTLINE_USE_COLOR");
			}
			if (this.settings.outlineOnly)
			{
				this._effectMaterial.EnableKeyword("OUTLINE_ONLY");
			}
			else
			{
				this._effectMaterial.DisableKeyword("OUTLINE_ONLY");
			}
			if (this.settings.resolutionInvariant)
			{
				this._effectMaterial.EnableKeyword("RESOLUTION_INVARIANT_THICKNESS");
			}
			else
			{
				this._effectMaterial.DisableKeyword("RESOLUTION_INVARIANT_THICKNESS");
			}
			this._effectMaterial.SetColor(FlatKitOutline.EdgeColor, this.settings.edgeColor);
			this._effectMaterial.SetFloat(FlatKitOutline.Thickness, (float)this.settings.thickness);
			this._effectMaterial.SetFloat(FlatKitOutline.DepthThresholdMin, this.settings.minDepthThreshold);
			this._effectMaterial.SetFloat(FlatKitOutline.DepthThresholdMax, this.settings.maxDepthThreshold);
			this._effectMaterial.SetFloat(FlatKitOutline.NormalThresholdMin, this.settings.minNormalsThreshold);
			this._effectMaterial.SetFloat(FlatKitOutline.NormalThresholdMax, this.settings.maxNormalsThreshold);
			this._effectMaterial.SetFloat(FlatKitOutline.ColorThresholdMin, this.settings.minColorThreshold);
			this._effectMaterial.SetFloat(FlatKitOutline.ColorThresholdMax, this.settings.maxColorThreshold);
		}

		// Token: 0x040000C3 RID: 195
		[Tooltip("To create new settings use 'Create > FlatKit > Outline Settings'.")]
		public OutlineSettings settings;

		// Token: 0x040000C4 RID: 196
		[SerializeField]
		[HideInInspector]
		private Material _effectMaterial;

		// Token: 0x040000C5 RID: 197
		private BlitTexturePass _blitTexturePass;

		// Token: 0x040000C6 RID: 198
		[SerializeField]
		[HideInInspector]
		private Shader _blitShader;

		// Token: 0x040000C7 RID: 199
		private static readonly string OutlineShaderName = "Hidden/FlatKit/OutlineFilter";

		// Token: 0x040000C8 RID: 200
		private static readonly int EdgeColor = Shader.PropertyToID("_EdgeColor");

		// Token: 0x040000C9 RID: 201
		private static readonly int Thickness = Shader.PropertyToID("_Thickness");

		// Token: 0x040000CA RID: 202
		private static readonly int DepthThresholdMin = Shader.PropertyToID("_DepthThresholdMin");

		// Token: 0x040000CB RID: 203
		private static readonly int DepthThresholdMax = Shader.PropertyToID("_DepthThresholdMax");

		// Token: 0x040000CC RID: 204
		private static readonly int NormalThresholdMin = Shader.PropertyToID("_NormalThresholdMin");

		// Token: 0x040000CD RID: 205
		private static readonly int NormalThresholdMax = Shader.PropertyToID("_NormalThresholdMax");

		// Token: 0x040000CE RID: 206
		private static readonly int ColorThresholdMin = Shader.PropertyToID("_ColorThresholdMin");

		// Token: 0x040000CF RID: 207
		private static readonly int ColorThresholdMax = Shader.PropertyToID("_ColorThresholdMax");
	}
}
