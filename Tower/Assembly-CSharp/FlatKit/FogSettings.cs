using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FlatKit
{
	// Token: 0x02000054 RID: 84
	[CreateAssetMenu(fileName = "FogSettings", menuName = "FlatKit/Fog Settings")]
	public class FogSettings : ScriptableObject
	{
		// Token: 0x060000F1 RID: 241 RVA: 0x00004982 File Offset: 0x00002B82
		private void OnValidate()
		{
			if (this.low > this.high)
			{
				Debug.LogWarning("[FlatKit] Fog Height configuration error: 'Low' must not be greater than 'High'");
			}
			if (this.near > this.far)
			{
				Debug.LogWarning("[FlatKit] Fog Distance configuration error: 'Near' must not be greater than 'Far'");
			}
		}

		// Token: 0x040000B5 RID: 181
		[Header("Distance Fog")]
		public bool useDistance = true;

		// Token: 0x040000B6 RID: 182
		public Gradient distanceGradient;

		// Token: 0x040000B7 RID: 183
		public float near;

		// Token: 0x040000B8 RID: 184
		public float far = 100f;

		// Token: 0x040000B9 RID: 185
		[Range(0f, 1f)]
		public float distanceFogIntensity = 1f;

		// Token: 0x040000BA RID: 186
		public bool useDistanceFogOnSky;

		// Token: 0x040000BB RID: 187
		[Header("Height Fog")]
		[Space]
		public bool useHeight;

		// Token: 0x040000BC RID: 188
		public Gradient heightGradient;

		// Token: 0x040000BD RID: 189
		public float low;

		// Token: 0x040000BE RID: 190
		public float high = 10f;

		// Token: 0x040000BF RID: 191
		[Range(0f, 1f)]
		public float heightFogIntensity = 1f;

		// Token: 0x040000C0 RID: 192
		public bool useHeightFogOnSky;

		// Token: 0x040000C1 RID: 193
		[Header("Blending")]
		[Space]
		[Range(0f, 1f)]
		public float distanceHeightBlend = 0.5f;

		// Token: 0x040000C2 RID: 194
		[Header("Advanced settings")]
		[Space]
		[Tooltip("The render stage at which the effect is applied. To exclude transparent objects, like water or UI elements, set this to \"Before Transparent\".")]
		public RenderPassEvent renderEvent = RenderPassEvent.BeforeRenderingPostProcessing;
	}
}
