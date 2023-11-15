using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000C5 RID: 197
	[Serializable]
	public class DepthOfFieldModel : PostProcessingModel
	{
		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060003F6 RID: 1014 RVA: 0x00025549 File Offset: 0x00023949
		// (set) Token: 0x060003F7 RID: 1015 RVA: 0x00025551 File Offset: 0x00023951
		public DepthOfFieldModel.Settings settings
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

		// Token: 0x060003F8 RID: 1016 RVA: 0x0002555A File Offset: 0x0002395A
		public override void Reset()
		{
			this.m_Settings = DepthOfFieldModel.Settings.defaultSettings;
		}

		// Token: 0x04000369 RID: 873
		[SerializeField]
		private DepthOfFieldModel.Settings m_Settings = DepthOfFieldModel.Settings.defaultSettings;

		// Token: 0x020000C6 RID: 198
		public enum KernelSize
		{
			// Token: 0x0400036B RID: 875
			Small,
			// Token: 0x0400036C RID: 876
			Medium,
			// Token: 0x0400036D RID: 877
			Large,
			// Token: 0x0400036E RID: 878
			VeryLarge
		}

		// Token: 0x020000C7 RID: 199
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000079 RID: 121
			// (get) Token: 0x060003F9 RID: 1017 RVA: 0x00025568 File Offset: 0x00023968
			public static DepthOfFieldModel.Settings defaultSettings
			{
				get
				{
					return new DepthOfFieldModel.Settings
					{
						focusDistance = 10f,
						aperture = 5.6f,
						focalLength = 50f,
						useCameraFov = false,
						kernelSize = DepthOfFieldModel.KernelSize.Medium
					};
				}
			}

			// Token: 0x0400036F RID: 879
			[Min(0.1f)]
			[Tooltip("Distance to the point of focus.")]
			public float focusDistance;

			// Token: 0x04000370 RID: 880
			[Range(0.05f, 32f)]
			[Tooltip("Ratio of aperture (known as f-stop or f-number). The smaller the value is, the shallower the depth of field is.")]
			public float aperture;

			// Token: 0x04000371 RID: 881
			[Range(1f, 300f)]
			[Tooltip("Distance between the lens and the film. The larger the value is, the shallower the depth of field is.")]
			public float focalLength;

			// Token: 0x04000372 RID: 882
			[Tooltip("Calculate the focal length automatically from the field-of-view value set on the camera. Using this setting isn't recommended.")]
			public bool useCameraFov;

			// Token: 0x04000373 RID: 883
			[Tooltip("Convolution kernel size of the bokeh filter, which determines the maximum radius of bokeh. It also affects the performance (the larger the kernel is, the longer the GPU time is required).")]
			public DepthOfFieldModel.KernelSize kernelSize;
		}
	}
}
