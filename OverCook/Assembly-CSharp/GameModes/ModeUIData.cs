using System;
using UnityEngine;

namespace GameModes
{
	// Token: 0x0200069C RID: 1692
	[Serializable]
	public struct ModeUIData
	{
		// Token: 0x040018C5 RID: 6341
		public string m_nameLocalisationKey;

		// Token: 0x040018C6 RID: 6342
		public string m_descriptionLocalisationKey;

		// Token: 0x040018C7 RID: 6343
		public Sprite m_previewImage;

		// Token: 0x040018C8 RID: 6344
		public SettingKind[] m_supportedSettings;

		// Token: 0x040018C9 RID: 6345
		public WorldMapLevelIconUI m_levelPreview;
	}
}
