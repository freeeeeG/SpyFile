using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000DA RID: 218
	[Serializable]
	public class TiltShiftModel : PostProcessingModel
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000419 RID: 1049 RVA: 0x000258F9 File Offset: 0x00023CF9
		// (set) Token: 0x0600041A RID: 1050 RVA: 0x00025901 File Offset: 0x00023D01
		public TiltShiftModel.Settings settings
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

		// Token: 0x0600041B RID: 1051 RVA: 0x0002590A File Offset: 0x00023D0A
		public override void Reset()
		{
			this.m_Settings = TiltShiftModel.Settings.defaultSettings;
		}

		// Token: 0x040003A6 RID: 934
		[SerializeField]
		private TiltShiftModel.Settings m_Settings = TiltShiftModel.Settings.defaultSettings;

		// Token: 0x020000DB RID: 219
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000087 RID: 135
			// (get) Token: 0x0600041C RID: 1052 RVA: 0x00025918 File Offset: 0x00023D18
			public static TiltShiftModel.Settings defaultSettings
			{
				get
				{
					return new TiltShiftModel.Settings
					{
						BlurArea = 0.4f,
						Downsample = true,
						Iterations = 1
					};
				}
			}

			// Token: 0x040003A7 RID: 935
			[Range(0f, 1f)]
			[Tooltip("Range of the blur. *Keep low for performance. ~0.4 is good.*")]
			public float BlurArea;

			// Token: 0x040003A8 RID: 936
			public bool Downsample;

			// Token: 0x040003A9 RID: 937
			[Range(1f, 4f)]
			[Tooltip("Number of iterations. *Performance Warning*")]
			public int Iterations;
		}
	}
}
