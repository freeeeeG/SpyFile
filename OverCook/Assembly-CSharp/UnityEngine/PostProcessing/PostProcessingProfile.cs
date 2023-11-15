using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000E8 RID: 232
	public class PostProcessingProfile : ScriptableObject
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x06000459 RID: 1113 RVA: 0x00026916 File Offset: 0x00024D16
		[SerializeField]
		public FogModel fog
		{
			get
			{
				return this.fogModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600045A RID: 1114 RVA: 0x00026924 File Offset: 0x00024D24
		public AntialiasingModel antialiasing
		{
			get
			{
				return this.antialiasingModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x0600045B RID: 1115 RVA: 0x00026932 File Offset: 0x00024D32
		public AmbientOcclusionModel ambientOcclusion
		{
			get
			{
				return this.ambientOcclusionModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x0600045C RID: 1116 RVA: 0x00026940 File Offset: 0x00024D40
		public ScreenSpaceReflectionModel screenSpaceReflection
		{
			get
			{
				return this.screenSpaceReflectionModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x0600045D RID: 1117 RVA: 0x0002694E File Offset: 0x00024D4E
		public DepthOfFieldModel depthOfField
		{
			get
			{
				return this.depthOfFieldModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x0600045E RID: 1118 RVA: 0x0002695C File Offset: 0x00024D5C
		public MotionBlurModel motionBlur
		{
			get
			{
				return this.motionBlurModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600045F RID: 1119 RVA: 0x0002696A File Offset: 0x00024D6A
		public EyeAdaptationModel eyeAdaptation
		{
			get
			{
				return this.eyeAdaptationModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000460 RID: 1120 RVA: 0x00026978 File Offset: 0x00024D78
		public BloomModel bloom
		{
			get
			{
				return this.bloomModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000461 RID: 1121 RVA: 0x00026986 File Offset: 0x00024D86
		public ColorGradingModel colorGrading
		{
			get
			{
				return this.colorGradingModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x00026994 File Offset: 0x00024D94
		public UserLutModel userLut
		{
			get
			{
				return this.userLutModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000463 RID: 1123 RVA: 0x000269A2 File Offset: 0x00024DA2
		public ChromaticAberrationModel chromaticAberration
		{
			get
			{
				return this.chromaticAberrationModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x000269B0 File Offset: 0x00024DB0
		public GrainModel grain
		{
			get
			{
				return this.grainModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000465 RID: 1125 RVA: 0x000269BE File Offset: 0x00024DBE
		public VignetteModel vignette
		{
			get
			{
				return this.vignetteModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x000269CC File Offset: 0x00024DCC
		public DitheringModel dithering
		{
			get
			{
				return this.ditheringModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000467 RID: 1127 RVA: 0x000269DA File Offset: 0x00024DDA
		public TiltShiftModel tiltshift
		{
			get
			{
				return this.tiltshiftModels[(int)PlatformUtils.GetCurrentPlatform()];
			}
		}

		// Token: 0x040003E0 RID: 992
		public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();

		// Token: 0x040003E1 RID: 993
		[SerializeField]
		private FogModel[] fogModels = new FogModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003E2 RID: 994
		[SerializeField]
		private AntialiasingModel[] antialiasingModels = new AntialiasingModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003E3 RID: 995
		[SerializeField]
		private AmbientOcclusionModel[] ambientOcclusionModels = new AmbientOcclusionModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003E4 RID: 996
		[SerializeField]
		private ScreenSpaceReflectionModel[] screenSpaceReflectionModels = new ScreenSpaceReflectionModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003E5 RID: 997
		[SerializeField]
		private DepthOfFieldModel[] depthOfFieldModels = new DepthOfFieldModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003E6 RID: 998
		[SerializeField]
		private MotionBlurModel[] motionBlurModels = new MotionBlurModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003E7 RID: 999
		[SerializeField]
		private EyeAdaptationModel[] eyeAdaptationModels = new EyeAdaptationModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003E8 RID: 1000
		[SerializeField]
		private BloomModel[] bloomModels = new BloomModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003E9 RID: 1001
		[SerializeField]
		private ColorGradingModel[] colorGradingModels = new ColorGradingModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003EA RID: 1002
		[SerializeField]
		private UserLutModel[] userLutModels = new UserLutModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003EB RID: 1003
		[SerializeField]
		private ChromaticAberrationModel[] chromaticAberrationModels = new ChromaticAberrationModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003EC RID: 1004
		[SerializeField]
		private GrainModel[] grainModels = new GrainModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003ED RID: 1005
		[SerializeField]
		private VignetteModel[] vignetteModels = new VignetteModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003EE RID: 1006
		[SerializeField]
		private DitheringModel[] ditheringModels = new DitheringModel[PlatformUtils.s_PlatformCount];

		// Token: 0x040003EF RID: 1007
		[SerializeField]
		private TiltShiftModel[] tiltshiftModels = new TiltShiftModel[PlatformUtils.s_PlatformCount];
	}
}
