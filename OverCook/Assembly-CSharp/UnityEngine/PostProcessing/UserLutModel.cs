using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000DC RID: 220
	[Serializable]
	public class UserLutModel : PostProcessingModel
	{
		// Token: 0x17000088 RID: 136
		// (get) Token: 0x0600041E RID: 1054 RVA: 0x0002595D File Offset: 0x00023D5D
		// (set) Token: 0x0600041F RID: 1055 RVA: 0x00025965 File Offset: 0x00023D65
		public UserLutModel.Settings settings
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

		// Token: 0x06000420 RID: 1056 RVA: 0x0002596E File Offset: 0x00023D6E
		public override void Reset()
		{
			this.m_Settings = UserLutModel.Settings.defaultSettings;
		}

		// Token: 0x040003AA RID: 938
		[SerializeField]
		private UserLutModel.Settings m_Settings = UserLutModel.Settings.defaultSettings;

		// Token: 0x020000DD RID: 221
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000089 RID: 137
			// (get) Token: 0x06000421 RID: 1057 RVA: 0x0002597C File Offset: 0x00023D7C
			public static UserLutModel.Settings defaultSettings
			{
				get
				{
					return new UserLutModel.Settings
					{
						lut = null,
						contribution = 1f
					};
				}
			}

			// Token: 0x040003AB RID: 939
			[Tooltip("Custom lookup texture (strip format, e.g. 256x16).")]
			public Texture2D lut;

			// Token: 0x040003AC RID: 940
			[Range(0f, 1f)]
			[Tooltip("Blending factor.")]
			public float contribution;
		}
	}
}
