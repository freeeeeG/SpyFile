using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000A4 RID: 164
	[Serializable]
	public class AmbientOcclusionModel : PostProcessingModel
	{
		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x00024972 File Offset: 0x00022D72
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x0002497A File Offset: 0x00022D7A
		public AmbientOcclusionModel.Settings settings
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

		// Token: 0x060003C2 RID: 962 RVA: 0x00024983 File Offset: 0x00022D83
		public override void Reset()
		{
			this.m_Settings = AmbientOcclusionModel.Settings.defaultSettings;
		}

		// Token: 0x040002E8 RID: 744
		[SerializeField]
		private AmbientOcclusionModel.Settings m_Settings = AmbientOcclusionModel.Settings.defaultSettings;

		// Token: 0x020000A5 RID: 165
		public enum SampleCount
		{
			// Token: 0x040002EA RID: 746
			Lowest = 3,
			// Token: 0x040002EB RID: 747
			Low = 6,
			// Token: 0x040002EC RID: 748
			Medium = 10,
			// Token: 0x040002ED RID: 749
			High = 16
		}

		// Token: 0x020000A6 RID: 166
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700005C RID: 92
			// (get) Token: 0x060003C3 RID: 963 RVA: 0x00024990 File Offset: 0x00022D90
			public static AmbientOcclusionModel.Settings defaultSettings
			{
				get
				{
					return new AmbientOcclusionModel.Settings
					{
						intensity = 1f,
						radius = 0.3f,
						sampleCount = AmbientOcclusionModel.SampleCount.Medium,
						downsampling = true,
						distanceFalloff = 4f
					};
				}
			}

			// Token: 0x040002EE RID: 750
			[Range(0f, 4f)]
			[Tooltip("Degree of darkness produced by the effect.")]
			public float intensity;

			// Token: 0x040002EF RID: 751
			[Range(0.005f, 0.05f)]
			[Tooltip("Radius of sample points, which affects extent of darkened areas.")]
			public float radius;

			// Token: 0x040002F0 RID: 752
			[Tooltip("Number of sample points, which affects quality and performance.")]
			public AmbientOcclusionModel.SampleCount sampleCount;

			// Token: 0x040002F1 RID: 753
			[Tooltip("Halves the resolution of the effect to increase performance at the cost of visual quality.")]
			public bool downsampling;

			// Token: 0x040002F2 RID: 754
			[Tooltip("Rotation Texture.")]
			public Texture2D RotationTexture;

			// Token: 0x040002F3 RID: 755
			public float blurSize;

			// Token: 0x040002F4 RID: 756
			public float distanceFalloff;
		}
	}
}
