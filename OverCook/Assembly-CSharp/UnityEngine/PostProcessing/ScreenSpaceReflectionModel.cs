using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D3 RID: 211
	[Serializable]
	public class ScreenSpaceReflectionModel : PostProcessingModel
	{
		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x000257F6 File Offset: 0x00023BF6
		// (set) Token: 0x06000415 RID: 1045 RVA: 0x000257FE File Offset: 0x00023BFE
		public ScreenSpaceReflectionModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x00025807 File Offset: 0x00023C07
		public override void Reset()
		{
			this.m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;
		}

		// Token: 0x0400038F RID: 911
		[SerializeField]
		private ScreenSpaceReflectionModel.Settings m_Settings = ScreenSpaceReflectionModel.Settings.defaultSettings;

		// Token: 0x020000D4 RID: 212
		public enum SSRResolution
		{
			// Token: 0x04000391 RID: 913
			High,
			// Token: 0x04000392 RID: 914
			Low = 2
		}

		// Token: 0x020000D5 RID: 213
		public enum SSRReflectionBlendType
		{
			// Token: 0x04000394 RID: 916
			PhysicallyBased,
			// Token: 0x04000395 RID: 917
			Additive
		}

		// Token: 0x020000D6 RID: 214
		[Serializable]
		public struct IntensitySettings
		{
			// Token: 0x04000396 RID: 918
			[Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
			[Range(0f, 2f)]
			public float reflectionMultiplier;

			// Token: 0x04000397 RID: 919
			[Tooltip("How far away from the maxDistance to begin fading SSR.")]
			[Range(0f, 1000f)]
			public float fadeDistance;

			// Token: 0x04000398 RID: 920
			[Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
			[Range(0f, 1f)]
			public float fresnelFade;

			// Token: 0x04000399 RID: 921
			[Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
			[Range(0.1f, 10f)]
			public float fresnelFadePower;
		}

		// Token: 0x020000D7 RID: 215
		[Serializable]
		public struct ReflectionSettings
		{
			// Token: 0x0400039A RID: 922
			[Tooltip("How the reflections are blended into the render.")]
			public ScreenSpaceReflectionModel.SSRReflectionBlendType blendType;

			// Token: 0x0400039B RID: 923
			[Tooltip("Half resolution SSRR is much faster, but less accurate.")]
			public ScreenSpaceReflectionModel.SSRResolution reflectionQuality;

			// Token: 0x0400039C RID: 924
			[Tooltip("Maximum reflection distance in world units.")]
			[Range(0.1f, 300f)]
			public float maxDistance;

			// Token: 0x0400039D RID: 925
			[Tooltip("Max raytracing length.")]
			[Range(16f, 1024f)]
			public int iterationCount;

			// Token: 0x0400039E RID: 926
			[Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
			[Range(1f, 16f)]
			public int stepSize;

			// Token: 0x0400039F RID: 927
			[Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
			[Range(0.01f, 10f)]
			public float widthModifier;

			// Token: 0x040003A0 RID: 928
			[Tooltip("Blurriness of reflections.")]
			[Range(0.1f, 8f)]
			public float reflectionBlur;

			// Token: 0x040003A1 RID: 929
			[Tooltip("Disable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
			public bool reflectBackfaces;
		}

		// Token: 0x020000D8 RID: 216
		[Serializable]
		public struct ScreenEdgeMask
		{
			// Token: 0x040003A2 RID: 930
			[Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
			[Range(0f, 1f)]
			public float intensity;
		}

		// Token: 0x020000D9 RID: 217
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000085 RID: 133
			// (get) Token: 0x06000417 RID: 1047 RVA: 0x00025814 File Offset: 0x00023C14
			public static ScreenSpaceReflectionModel.Settings defaultSettings
			{
				get
				{
					return new ScreenSpaceReflectionModel.Settings
					{
						reflection = new ScreenSpaceReflectionModel.ReflectionSettings
						{
							blendType = ScreenSpaceReflectionModel.SSRReflectionBlendType.PhysicallyBased,
							reflectionQuality = ScreenSpaceReflectionModel.SSRResolution.Low,
							maxDistance = 100f,
							iterationCount = 256,
							stepSize = 3,
							widthModifier = 0.5f,
							reflectionBlur = 1f,
							reflectBackfaces = false
						},
						intensity = new ScreenSpaceReflectionModel.IntensitySettings
						{
							reflectionMultiplier = 1f,
							fadeDistance = 100f,
							fresnelFade = 1f,
							fresnelFadePower = 1f
						},
						screenEdgeMask = new ScreenSpaceReflectionModel.ScreenEdgeMask
						{
							intensity = 0.03f
						}
					};
				}
			}

			// Token: 0x040003A3 RID: 931
			public ScreenSpaceReflectionModel.ReflectionSettings reflection;

			// Token: 0x040003A4 RID: 932
			public ScreenSpaceReflectionModel.IntensitySettings intensity;

			// Token: 0x040003A5 RID: 933
			public ScreenSpaceReflectionModel.ScreenEdgeMask screenEdgeMask;
		}
	}
}
