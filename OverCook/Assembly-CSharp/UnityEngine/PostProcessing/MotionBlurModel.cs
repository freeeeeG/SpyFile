using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000D1 RID: 209
	[Serializable]
	public class MotionBlurModel : PostProcessingModel
	{
		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x0002578D File Offset: 0x00023B8D
		// (set) Token: 0x06000410 RID: 1040 RVA: 0x00025795 File Offset: 0x00023B95
		public MotionBlurModel.Settings settings
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

		// Token: 0x06000411 RID: 1041 RVA: 0x0002579E File Offset: 0x00023B9E
		public override void Reset()
		{
			this.m_Settings = MotionBlurModel.Settings.defaultSettings;
		}

		// Token: 0x0400038B RID: 907
		[SerializeField]
		private MotionBlurModel.Settings m_Settings = MotionBlurModel.Settings.defaultSettings;

		// Token: 0x020000D2 RID: 210
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000083 RID: 131
			// (get) Token: 0x06000412 RID: 1042 RVA: 0x000257AC File Offset: 0x00023BAC
			public static MotionBlurModel.Settings defaultSettings
			{
				get
				{
					return new MotionBlurModel.Settings
					{
						shutterAngle = 270f,
						sampleCount = 10,
						frameBlending = 0f
					};
				}
			}

			// Token: 0x0400038C RID: 908
			[Range(0f, 360f)]
			[Tooltip("The angle of rotary shutter. Larger values give longer exposure.")]
			public float shutterAngle;

			// Token: 0x0400038D RID: 909
			[Range(4f, 32f)]
			[Tooltip("The amount of sample points, which affects quality and performances.")]
			public int sampleCount;

			// Token: 0x0400038E RID: 910
			[Range(0f, 1f)]
			[Tooltip("The strength of multiple frame blending. The opacity of preceding frames are determined from this coefficient and time differences.")]
			public float frameBlending;
		}
	}
}
