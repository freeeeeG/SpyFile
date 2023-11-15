using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000DE RID: 222
	[Serializable]
	public class VignetteModel : PostProcessingModel
	{
		// Token: 0x1700008A RID: 138
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x000259B9 File Offset: 0x00023DB9
		// (set) Token: 0x06000424 RID: 1060 RVA: 0x000259C1 File Offset: 0x00023DC1
		public VignetteModel.Settings settings
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

		// Token: 0x06000425 RID: 1061 RVA: 0x000259CA File Offset: 0x00023DCA
		public override void Reset()
		{
			this.m_Settings = VignetteModel.Settings.defaultSettings;
		}

		// Token: 0x040003AD RID: 941
		[SerializeField]
		private VignetteModel.Settings m_Settings = VignetteModel.Settings.defaultSettings;

		// Token: 0x020000DF RID: 223
		public enum Mode
		{
			// Token: 0x040003AF RID: 943
			Classic,
			// Token: 0x040003B0 RID: 944
			Masked
		}

		// Token: 0x020000E0 RID: 224
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700008B RID: 139
			// (get) Token: 0x06000426 RID: 1062 RVA: 0x000259D8 File Offset: 0x00023DD8
			public static VignetteModel.Settings defaultSettings
			{
				get
				{
					return new VignetteModel.Settings
					{
						mode = VignetteModel.Mode.Classic,
						color = new Color(0f, 0f, 0f, 1f),
						center = new Vector2(0.5f, 0.5f),
						intensity = 0.45f,
						smoothness = 0.2f,
						roundness = 1f,
						mask = null,
						opacity = 1f,
						rounded = false
					};
				}
			}

			// Token: 0x040003B1 RID: 945
			[Tooltip("Use the \"Classic\" mode for parametric controls. Use the \"Masked\" mode to use your own texture mask.")]
			public VignetteModel.Mode mode;

			// Token: 0x040003B2 RID: 946
			[ColorUsage(false)]
			[Tooltip("Vignette color. Use the alpha channel for transparency.")]
			public Color color;

			// Token: 0x040003B3 RID: 947
			[Tooltip("Sets the vignette center point (screen center is [0.5,0.5]).")]
			public Vector2 center;

			// Token: 0x040003B4 RID: 948
			[Range(0f, 1f)]
			[Tooltip("Amount of vignetting on screen.")]
			public float intensity;

			// Token: 0x040003B5 RID: 949
			[Range(0.01f, 1f)]
			[Tooltip("Smoothness of the vignette borders.")]
			public float smoothness;

			// Token: 0x040003B6 RID: 950
			[Range(0f, 1f)]
			[Tooltip("Lower values will make a square-ish vignette.")]
			public float roundness;

			// Token: 0x040003B7 RID: 951
			[Tooltip("A black and white mask to use as a vignette.")]
			public Texture mask;

			// Token: 0x040003B8 RID: 952
			[Range(0f, 1f)]
			[Tooltip("Mask opacity.")]
			public float opacity;

			// Token: 0x040003B9 RID: 953
			[Tooltip("Should the vignette be perfectly round or be dependent on the current aspect ratio?")]
			public bool rounded;
		}
	}
}
