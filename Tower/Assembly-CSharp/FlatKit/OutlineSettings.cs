using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FlatKit
{
	// Token: 0x02000056 RID: 86
	[CreateAssetMenu(fileName = "OutlineSettings", menuName = "FlatKit/Outline Settings")]
	public class OutlineSettings : ScriptableObject
	{
		// Token: 0x060000FA RID: 250 RVA: 0x00004DB8 File Offset: 0x00002FB8
		private void OnValidate()
		{
			if (this.minDepthThreshold > this.maxDepthThreshold)
			{
				Debug.LogWarning("[FlatKit] Outline configuration error: 'Min Depth Threshold' must not be greater than 'Max Depth Threshold'");
			}
			if (this.minNormalsThreshold > this.maxNormalsThreshold)
			{
				Debug.LogWarning("[FlatKit] Outline configuration error: 'Min Normals Threshold' must not be greater than 'Max Normals Threshold'");
			}
			if (this.minColorThreshold > this.maxColorThreshold)
			{
				Debug.LogWarning("[FlatKit] Outline configuration error: 'Min Color Threshold' must not be greater than 'Max Color Threshold'");
			}
		}

		// Token: 0x040000D0 RID: 208
		public Color edgeColor = Color.white;

		// Token: 0x040000D1 RID: 209
		[Range(0f, 5f)]
		public int thickness = 1;

		// Token: 0x040000D2 RID: 210
		[Tooltip("If enabled, the line width will stay constant regardless of the rendering resolution. However, some of the lines may appear blurry.")]
		public bool resolutionInvariant;

		// Token: 0x040000D3 RID: 211
		[Space]
		public bool useDepth = true;

		// Token: 0x040000D4 RID: 212
		public bool useNormals;

		// Token: 0x040000D5 RID: 213
		public bool useColor;

		// Token: 0x040000D6 RID: 214
		[Header("Advanced settings")]
		public float minDepthThreshold;

		// Token: 0x040000D7 RID: 215
		public float maxDepthThreshold = 0.25f;

		// Token: 0x040000D8 RID: 216
		[Space]
		public float minNormalsThreshold;

		// Token: 0x040000D9 RID: 217
		public float maxNormalsThreshold = 0.25f;

		// Token: 0x040000DA RID: 218
		[Space]
		public float minColorThreshold;

		// Token: 0x040000DB RID: 219
		public float maxColorThreshold = 0.25f;

		// Token: 0x040000DC RID: 220
		[Space]
		[Tooltip("The render stage at which the effect is applied. To exclude transparent objects, like water or UI elements, set this to \"Before Transparent\".")]
		public RenderPassEvent renderEvent = RenderPassEvent.BeforeRenderingPostProcessing;

		// Token: 0x040000DD RID: 221
		[Space]
		public bool outlineOnly;
	}
}
