using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000CF RID: 207
	[Serializable]
	public class GrainModel : PostProcessingModel
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x0600040A RID: 1034 RVA: 0x00025719 File Offset: 0x00023B19
		// (set) Token: 0x0600040B RID: 1035 RVA: 0x00025721 File Offset: 0x00023B21
		public GrainModel.Settings settings
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

		// Token: 0x0600040C RID: 1036 RVA: 0x0002572A File Offset: 0x00023B2A
		public override void Reset()
		{
			this.m_Settings = GrainModel.Settings.defaultSettings;
		}

		// Token: 0x04000386 RID: 902
		[SerializeField]
		private GrainModel.Settings m_Settings = GrainModel.Settings.defaultSettings;

		// Token: 0x020000D0 RID: 208
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000081 RID: 129
			// (get) Token: 0x0600040D RID: 1037 RVA: 0x00025738 File Offset: 0x00023B38
			public static GrainModel.Settings defaultSettings
			{
				get
				{
					return new GrainModel.Settings
					{
						colored = true,
						intensity = 0.5f,
						size = 1f,
						luminanceContribution = 0.8f
					};
				}
			}

			// Token: 0x04000387 RID: 903
			[Tooltip("Enable the use of colored grain.")]
			public bool colored;

			// Token: 0x04000388 RID: 904
			[Range(0f, 1f)]
			[Tooltip("Grain strength. Higher means more visible grain.")]
			public float intensity;

			// Token: 0x04000389 RID: 905
			[Range(0.3f, 3f)]
			[Tooltip("Grain particle size.")]
			public float size;

			// Token: 0x0400038A RID: 906
			[Range(0f, 1f)]
			[Tooltip("Controls the noisiness response curve based on scene luminance. Lower values mean less noise in dark areas.")]
			public float luminanceContribution;
		}
	}
}
