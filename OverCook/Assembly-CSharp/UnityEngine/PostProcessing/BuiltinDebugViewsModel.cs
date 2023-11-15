using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000B3 RID: 179
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00024E61 File Offset: 0x00023261
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x00024E69 File Offset: 0x00023269
		public BuiltinDebugViewsModel.Settings settings
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

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x00024E72 File Offset: 0x00023272
		public bool willInterrupt
		{
			get
			{
				return !this.IsModeActive(BuiltinDebugViewsModel.Mode.None) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut);
			}
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00024EB2 File Offset: 0x000232B2
		public override void Reset()
		{
			this.settings = BuiltinDebugViewsModel.Settings.defaultSettings;
		}

		// Token: 0x060003DB RID: 987 RVA: 0x00024EBF File Offset: 0x000232BF
		public bool IsModeActive(BuiltinDebugViewsModel.Mode mode)
		{
			return this.m_Settings.mode == mode;
		}

		// Token: 0x0400031A RID: 794
		[SerializeField]
		private BuiltinDebugViewsModel.Settings m_Settings = BuiltinDebugViewsModel.Settings.defaultSettings;

		// Token: 0x020000B4 RID: 180
		[Serializable]
		public struct DepthSettings
		{
			// Token: 0x17000068 RID: 104
			// (get) Token: 0x060003DC RID: 988 RVA: 0x00024ED0 File Offset: 0x000232D0
			public static BuiltinDebugViewsModel.DepthSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.DepthSettings
					{
						scale = 1f
					};
				}
			}

			// Token: 0x0400031B RID: 795
			[Range(0f, 1f)]
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			public float scale;
		}

		// Token: 0x020000B5 RID: 181
		[Serializable]
		public struct MotionVectorsSettings
		{
			// Token: 0x17000069 RID: 105
			// (get) Token: 0x060003DD RID: 989 RVA: 0x00024EF4 File Offset: 0x000232F4
			public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.MotionVectorsSettings
					{
						sourceOpacity = 1f,
						motionImageOpacity = 0f,
						motionImageAmplitude = 16f,
						motionVectorsOpacity = 1f,
						motionVectorsResolution = 24,
						motionVectorsAmplitude = 64f
					};
				}
			}

			// Token: 0x0400031C RID: 796
			[Range(0f, 1f)]
			[Tooltip("Opacity of the source render.")]
			public float sourceOpacity;

			// Token: 0x0400031D RID: 797
			[Range(0f, 1f)]
			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			public float motionImageOpacity;

			// Token: 0x0400031E RID: 798
			[Min(0f)]
			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			public float motionImageAmplitude;

			// Token: 0x0400031F RID: 799
			[Range(0f, 1f)]
			[Tooltip("Opacity for the motion vector arrows.")]
			public float motionVectorsOpacity;

			// Token: 0x04000320 RID: 800
			[Range(8f, 64f)]
			[Tooltip("The arrow density on screen.")]
			public int motionVectorsResolution;

			// Token: 0x04000321 RID: 801
			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;
		}

		// Token: 0x020000B6 RID: 182
		public enum Mode
		{
			// Token: 0x04000323 RID: 803
			None,
			// Token: 0x04000324 RID: 804
			Depth,
			// Token: 0x04000325 RID: 805
			Normals,
			// Token: 0x04000326 RID: 806
			MotionVectors,
			// Token: 0x04000327 RID: 807
			AmbientOcclusion,
			// Token: 0x04000328 RID: 808
			EyeAdaptation,
			// Token: 0x04000329 RID: 809
			FocusPlane,
			// Token: 0x0400032A RID: 810
			PreGradingLog,
			// Token: 0x0400032B RID: 811
			LogLut,
			// Token: 0x0400032C RID: 812
			UserLut
		}

		// Token: 0x020000B7 RID: 183
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700006A RID: 106
			// (get) Token: 0x060003DE RID: 990 RVA: 0x00024F50 File Offset: 0x00023350
			public static BuiltinDebugViewsModel.Settings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.Settings
					{
						mode = BuiltinDebugViewsModel.Mode.None,
						depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings,
						motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
					};
				}
			}

			// Token: 0x0400032D RID: 813
			public BuiltinDebugViewsModel.Mode mode;

			// Token: 0x0400032E RID: 814
			public BuiltinDebugViewsModel.DepthSettings depth;

			// Token: 0x0400032F RID: 815
			public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
		}
	}
}
