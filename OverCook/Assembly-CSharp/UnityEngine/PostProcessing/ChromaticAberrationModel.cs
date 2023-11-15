using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000B8 RID: 184
	[Serializable]
	public class ChromaticAberrationModel : PostProcessingModel
	{
		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060003E0 RID: 992 RVA: 0x00024F99 File Offset: 0x00023399
		// (set) Token: 0x060003E1 RID: 993 RVA: 0x00024FA1 File Offset: 0x000233A1
		public ChromaticAberrationModel.Settings settings
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

		// Token: 0x060003E2 RID: 994 RVA: 0x00024FAA File Offset: 0x000233AA
		public override void Reset()
		{
			this.m_Settings = ChromaticAberrationModel.Settings.defaultSettings;
		}

		// Token: 0x04000330 RID: 816
		[SerializeField]
		private ChromaticAberrationModel.Settings m_Settings = ChromaticAberrationModel.Settings.defaultSettings;

		// Token: 0x020000B9 RID: 185
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700006C RID: 108
			// (get) Token: 0x060003E3 RID: 995 RVA: 0x00024FB8 File Offset: 0x000233B8
			public static ChromaticAberrationModel.Settings defaultSettings
			{
				get
				{
					return new ChromaticAberrationModel.Settings
					{
						spectralTexture = null,
						intensity = 0.1f
					};
				}
			}

			// Token: 0x04000331 RID: 817
			[Tooltip("Shift the hue of chromatic aberrations.")]
			public Texture2D spectralTexture;

			// Token: 0x04000332 RID: 818
			[Range(0f, 1f)]
			[Tooltip("Amount of tangential distortion.")]
			public float intensity;
		}
	}
}
