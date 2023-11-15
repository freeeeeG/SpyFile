using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000AF RID: 175
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060003CE RID: 974 RVA: 0x00024D69 File Offset: 0x00023169
		// (set) Token: 0x060003CF RID: 975 RVA: 0x00024D71 File Offset: 0x00023171
		public BloomModel.Settings settings
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

		// Token: 0x060003D0 RID: 976 RVA: 0x00024D7A File Offset: 0x0002317A
		public override void Reset()
		{
			this.m_Settings = BloomModel.Settings.defaultSettings;
		}

		// Token: 0x04000310 RID: 784
		[SerializeField]
		private BloomModel.Settings m_Settings = BloomModel.Settings.defaultSettings;

		// Token: 0x020000B0 RID: 176
		[Serializable]
		public struct BloomSettings
		{
			// Token: 0x17000062 RID: 98
			// (get) Token: 0x060003D2 RID: 978 RVA: 0x00024D95 File Offset: 0x00023195
			// (set) Token: 0x060003D1 RID: 977 RVA: 0x00024D87 File Offset: 0x00023187
			public float thresholdLinear
			{
				get
				{
					return Mathf.GammaToLinearSpace(this.threshold);
				}
				set
				{
					this.threshold = Mathf.LinearToGammaSpace(value);
				}
			}

			// Token: 0x17000063 RID: 99
			// (get) Token: 0x060003D3 RID: 979 RVA: 0x00024DA4 File Offset: 0x000231A4
			public static BloomModel.BloomSettings defaultSettings
			{
				get
				{
					return new BloomModel.BloomSettings
					{
						intensity = 0.5f,
						threshold = 1.1f,
						softKnee = 0.5f,
						radius = 4f,
						antiFlicker = false
					};
				}
			}

			// Token: 0x04000311 RID: 785
			[Min(0f)]
			[Tooltip("Strength of the bloom filter.")]
			public float intensity;

			// Token: 0x04000312 RID: 786
			[Min(0f)]
			[Tooltip("Filters out pixels under this level of brightness.")]
			public float threshold;

			// Token: 0x04000313 RID: 787
			[Range(0f, 1f)]
			[Tooltip("Makes transition between under/over-threshold gradual (0 = hard threshold, 1 = soft threshold).")]
			public float softKnee;

			// Token: 0x04000314 RID: 788
			[Range(1f, 7f)]
			[Tooltip("Changes extent of veiling effects in a screen resolution-independent fashion.")]
			public float radius;

			// Token: 0x04000315 RID: 789
			[Tooltip("Reduces flashing noise with an additional filter.")]
			public bool antiFlicker;
		}

		// Token: 0x020000B1 RID: 177
		[Serializable]
		public struct LensDirtSettings
		{
			// Token: 0x17000064 RID: 100
			// (get) Token: 0x060003D4 RID: 980 RVA: 0x00024DF4 File Offset: 0x000231F4
			public static BloomModel.LensDirtSettings defaultSettings
			{
				get
				{
					return new BloomModel.LensDirtSettings
					{
						texture = null,
						intensity = 3f
					};
				}
			}

			// Token: 0x04000316 RID: 790
			[Tooltip("Dirtiness texture to add smudges or dust to the lens.")]
			public Texture texture;

			// Token: 0x04000317 RID: 791
			[Min(0f)]
			[Tooltip("Amount of lens dirtiness.")]
			public float intensity;
		}

		// Token: 0x020000B2 RID: 178
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000065 RID: 101
			// (get) Token: 0x060003D5 RID: 981 RVA: 0x00024E20 File Offset: 0x00023220
			public static BloomModel.Settings defaultSettings
			{
				get
				{
					return new BloomModel.Settings
					{
						bloom = BloomModel.BloomSettings.defaultSettings,
						lensDirt = BloomModel.LensDirtSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000318 RID: 792
			public BloomModel.BloomSettings bloom;

			// Token: 0x04000319 RID: 793
			public BloomModel.LensDirtSettings lensDirt;
		}
	}
}
