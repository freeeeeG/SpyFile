using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000CD RID: 205
	[Serializable]
	public class FogModel : PostProcessingModel
	{
		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x000256CA File Offset: 0x00023ACA
		// (set) Token: 0x06000406 RID: 1030 RVA: 0x000256D2 File Offset: 0x00023AD2
		public FogModel.Settings settings
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

		// Token: 0x06000407 RID: 1031 RVA: 0x000256DB File Offset: 0x00023ADB
		public override void Reset()
		{
			this.m_Settings = FogModel.Settings.defaultSettings;
		}

		// Token: 0x04000384 RID: 900
		[SerializeField]
		private FogModel.Settings m_Settings = FogModel.Settings.defaultSettings;

		// Token: 0x020000CE RID: 206
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700007F RID: 127
			// (get) Token: 0x06000408 RID: 1032 RVA: 0x000256E8 File Offset: 0x00023AE8
			public static FogModel.Settings defaultSettings
			{
				get
				{
					return new FogModel.Settings
					{
						excludeSkybox = true
					};
				}
			}

			// Token: 0x04000385 RID: 901
			[Tooltip("Should the fog affect the skybox?")]
			public bool excludeSkybox;
		}
	}
}
