using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000BA RID: 186
	[Serializable]
	public class ColorGradingModel : PostProcessingModel
	{
		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060003E5 RID: 997 RVA: 0x00024FF5 File Offset: 0x000233F5
		// (set) Token: 0x060003E6 RID: 998 RVA: 0x00024FFD File Offset: 0x000233FD
		public ColorGradingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
				this.OnValidate();
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060003E7 RID: 999 RVA: 0x0002500C File Offset: 0x0002340C
		// (set) Token: 0x060003E8 RID: 1000 RVA: 0x00025014 File Offset: 0x00023414
		public bool isDirty { get; internal set; }

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060003E9 RID: 1001 RVA: 0x0002501D File Offset: 0x0002341D
		// (set) Token: 0x060003EA RID: 1002 RVA: 0x00025025 File Offset: 0x00023425
		public RenderTexture bakedLut { get; internal set; }

		// Token: 0x060003EB RID: 1003 RVA: 0x0002502E File Offset: 0x0002342E
		public override void Reset()
		{
			this.m_Settings = ColorGradingModel.Settings.defaultSettings;
			this.OnValidate();
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00025041 File Offset: 0x00023441
		public override void OnValidate()
		{
			this.isDirty = true;
		}

		// Token: 0x04000333 RID: 819
		[SerializeField]
		private ColorGradingModel.Settings m_Settings = ColorGradingModel.Settings.defaultSettings;

		// Token: 0x020000BB RID: 187
		public enum Tonemapper
		{
			// Token: 0x04000337 RID: 823
			None,
			// Token: 0x04000338 RID: 824
			ACES,
			// Token: 0x04000339 RID: 825
			Neutral
		}

		// Token: 0x020000BC RID: 188
		[Serializable]
		public struct TonemappingSettings
		{
			// Token: 0x17000070 RID: 112
			// (get) Token: 0x060003ED RID: 1005 RVA: 0x0002504C File Offset: 0x0002344C
			public static ColorGradingModel.TonemappingSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.TonemappingSettings
					{
						tonemapper = ColorGradingModel.Tonemapper.Neutral,
						neutralBlackIn = 0.02f,
						neutralWhiteIn = 10f,
						neutralBlackOut = 0f,
						neutralWhiteOut = 10f,
						neutralWhiteLevel = 5.3f,
						neutralWhiteClip = 10f
					};
				}
			}

			// Token: 0x0400033A RID: 826
			[Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
			public ColorGradingModel.Tonemapper tonemapper;

			// Token: 0x0400033B RID: 827
			[Range(-0.1f, 0.1f)]
			public float neutralBlackIn;

			// Token: 0x0400033C RID: 828
			[Range(1f, 20f)]
			public float neutralWhiteIn;

			// Token: 0x0400033D RID: 829
			[Range(-0.09f, 0.1f)]
			public float neutralBlackOut;

			// Token: 0x0400033E RID: 830
			[Range(1f, 19f)]
			public float neutralWhiteOut;

			// Token: 0x0400033F RID: 831
			[Range(0.1f, 20f)]
			public float neutralWhiteLevel;

			// Token: 0x04000340 RID: 832
			[Range(1f, 10f)]
			public float neutralWhiteClip;
		}

		// Token: 0x020000BD RID: 189
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x17000071 RID: 113
			// (get) Token: 0x060003EE RID: 1006 RVA: 0x000250B4 File Offset: 0x000234B4
			public static ColorGradingModel.BasicSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.BasicSettings
					{
						postExposure = 0f,
						temperature = 0f,
						tint = 0f,
						hueShift = 0f,
						saturation = 1f,
						contrast = 1f
					};
				}
			}

			// Token: 0x04000341 RID: 833
			[Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
			public float postExposure;

			// Token: 0x04000342 RID: 834
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to a custom color temperature.")]
			public float temperature;

			// Token: 0x04000343 RID: 835
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
			public float tint;

			// Token: 0x04000344 RID: 836
			[Range(-180f, 180f)]
			[Tooltip("Shift the hue of all colors.")]
			public float hueShift;

			// Token: 0x04000345 RID: 837
			[Range(0f, 2f)]
			[Tooltip("Pushes the intensity of all colors.")]
			public float saturation;

			// Token: 0x04000346 RID: 838
			[Range(0f, 2f)]
			[Tooltip("Expands or shrinks the overall range of tonal values.")]
			public float contrast;
		}

		// Token: 0x020000BE RID: 190
		[Serializable]
		public struct ChannelMixerSettings
		{
			// Token: 0x17000072 RID: 114
			// (get) Token: 0x060003EF RID: 1007 RVA: 0x00025114 File Offset: 0x00023514
			public static ColorGradingModel.ChannelMixerSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ChannelMixerSettings
					{
						red = new Vector3(1f, 0f, 0f),
						green = new Vector3(0f, 1f, 0f),
						blue = new Vector3(0f, 0f, 1f),
						currentEditingChannel = 0
					};
				}
			}

			// Token: 0x04000347 RID: 839
			public Vector3 red;

			// Token: 0x04000348 RID: 840
			public Vector3 green;

			// Token: 0x04000349 RID: 841
			public Vector3 blue;

			// Token: 0x0400034A RID: 842
			[HideInInspector]
			public int currentEditingChannel;
		}

		// Token: 0x020000BF RID: 191
		[Serializable]
		public struct LogWheelsSettings
		{
			// Token: 0x17000073 RID: 115
			// (get) Token: 0x060003F0 RID: 1008 RVA: 0x00025184 File Offset: 0x00023584
			public static ColorGradingModel.LogWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LogWheelsSettings
					{
						slope = Color.clear,
						power = Color.clear,
						offset = Color.clear
					};
				}
			}

			// Token: 0x0400034B RID: 843
			[Trackball("GetSlopeValue")]
			public Color slope;

			// Token: 0x0400034C RID: 844
			[Trackball("GetPowerValue")]
			public Color power;

			// Token: 0x0400034D RID: 845
			[Trackball("GetOffsetValue")]
			public Color offset;
		}

		// Token: 0x020000C0 RID: 192
		[Serializable]
		public struct LinearWheelsSettings
		{
			// Token: 0x17000074 RID: 116
			// (get) Token: 0x060003F1 RID: 1009 RVA: 0x000251C0 File Offset: 0x000235C0
			public static ColorGradingModel.LinearWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LinearWheelsSettings
					{
						lift = Color.clear,
						gamma = Color.clear,
						gain = Color.clear
					};
				}
			}

			// Token: 0x0400034E RID: 846
			[Trackball("GetLiftValue")]
			public Color lift;

			// Token: 0x0400034F RID: 847
			[Trackball("GetGammaValue")]
			public Color gamma;

			// Token: 0x04000350 RID: 848
			[Trackball("GetGainValue")]
			public Color gain;
		}

		// Token: 0x020000C1 RID: 193
		public enum ColorWheelMode
		{
			// Token: 0x04000352 RID: 850
			Linear,
			// Token: 0x04000353 RID: 851
			Log
		}

		// Token: 0x020000C2 RID: 194
		[Serializable]
		public struct ColorWheelsSettings
		{
			// Token: 0x17000075 RID: 117
			// (get) Token: 0x060003F2 RID: 1010 RVA: 0x000251FC File Offset: 0x000235FC
			public static ColorGradingModel.ColorWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ColorWheelsSettings
					{
						mode = ColorGradingModel.ColorWheelMode.Log,
						log = ColorGradingModel.LogWheelsSettings.defaultSettings,
						linear = ColorGradingModel.LinearWheelsSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000354 RID: 852
			public ColorGradingModel.ColorWheelMode mode;

			// Token: 0x04000355 RID: 853
			[TrackballGroup]
			public ColorGradingModel.LogWheelsSettings log;

			// Token: 0x04000356 RID: 854
			[TrackballGroup]
			public ColorGradingModel.LinearWheelsSettings linear;
		}

		// Token: 0x020000C3 RID: 195
		[Serializable]
		public struct CurvesSettings
		{
			// Token: 0x17000076 RID: 118
			// (get) Token: 0x060003F3 RID: 1011 RVA: 0x00025234 File Offset: 0x00023634
			public static ColorGradingModel.CurvesSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.CurvesSettings
					{
						master = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						red = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						green = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						blue = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						e_CurrentEditingCurve = 0,
						e_CurveY = true,
						e_CurveR = false,
						e_CurveG = false,
						e_CurveB = false
					};
				}
			}

			// Token: 0x04000357 RID: 855
			public ColorGradingCurve master;

			// Token: 0x04000358 RID: 856
			public ColorGradingCurve red;

			// Token: 0x04000359 RID: 857
			public ColorGradingCurve green;

			// Token: 0x0400035A RID: 858
			public ColorGradingCurve blue;

			// Token: 0x0400035B RID: 859
			public ColorGradingCurve hueVShue;

			// Token: 0x0400035C RID: 860
			public ColorGradingCurve hueVSsat;

			// Token: 0x0400035D RID: 861
			public ColorGradingCurve satVSsat;

			// Token: 0x0400035E RID: 862
			public ColorGradingCurve lumVSsat;

			// Token: 0x0400035F RID: 863
			[HideInInspector]
			public int e_CurrentEditingCurve;

			// Token: 0x04000360 RID: 864
			[HideInInspector]
			public bool e_CurveY;

			// Token: 0x04000361 RID: 865
			[HideInInspector]
			public bool e_CurveR;

			// Token: 0x04000362 RID: 866
			[HideInInspector]
			public bool e_CurveG;

			// Token: 0x04000363 RID: 867
			[HideInInspector]
			public bool e_CurveB;
		}

		// Token: 0x020000C4 RID: 196
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000077 RID: 119
			// (get) Token: 0x060003F4 RID: 1012 RVA: 0x000254E4 File Offset: 0x000238E4
			public static ColorGradingModel.Settings defaultSettings
			{
				get
				{
					return new ColorGradingModel.Settings
					{
						tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings,
						basic = ColorGradingModel.BasicSettings.defaultSettings,
						channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings,
						colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings,
						curves = ColorGradingModel.CurvesSettings.defaultSettings
					};
				}
			}

			// Token: 0x04000364 RID: 868
			public ColorGradingModel.TonemappingSettings tonemapping;

			// Token: 0x04000365 RID: 869
			public ColorGradingModel.BasicSettings basic;

			// Token: 0x04000366 RID: 870
			public ColorGradingModel.ChannelMixerSettings channelMixer;

			// Token: 0x04000367 RID: 871
			public ColorGradingModel.ColorWheelsSettings colorWheels;

			// Token: 0x04000368 RID: 872
			public ColorGradingModel.CurvesSettings curves;
		}
	}
}
