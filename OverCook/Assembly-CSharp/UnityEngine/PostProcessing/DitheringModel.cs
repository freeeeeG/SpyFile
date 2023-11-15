using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x020000C8 RID: 200
	[Serializable]
	public class DitheringModel : PostProcessingModel
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060003FB RID: 1019 RVA: 0x000255C5 File Offset: 0x000239C5
		// (set) Token: 0x060003FC RID: 1020 RVA: 0x000255CD File Offset: 0x000239CD
		public DitheringModel.Settings settings
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

		// Token: 0x060003FD RID: 1021 RVA: 0x000255D6 File Offset: 0x000239D6
		public override void Reset()
		{
			this.m_Settings = DitheringModel.Settings.defaultSettings;
		}

		// Token: 0x04000374 RID: 884
		[SerializeField]
		private DitheringModel.Settings m_Settings = DitheringModel.Settings.defaultSettings;

		// Token: 0x020000C9 RID: 201
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700007B RID: 123
			// (get) Token: 0x060003FE RID: 1022 RVA: 0x000255E4 File Offset: 0x000239E4
			public static DitheringModel.Settings defaultSettings
			{
				get
				{
					return default(DitheringModel.Settings);
				}
			}
		}
	}
}
