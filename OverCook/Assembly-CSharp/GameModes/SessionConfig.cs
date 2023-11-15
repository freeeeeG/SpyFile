using System;

namespace GameModes
{
	// Token: 0x02000694 RID: 1684
	[Serializable]
	public class SessionConfig
	{
		// Token: 0x0600202B RID: 8235 RVA: 0x0009CDDC File Offset: 0x0009B1DC
		public void Copy(SessionConfig config)
		{
			this.m_kind = config.m_kind;
			for (int i = 0; i < this.m_settings.Length; i++)
			{
				this.m_settings[i] = config.m_settings[i];
			}
		}

		// Token: 0x0600202C RID: 8236 RVA: 0x0009CE20 File Offset: 0x0009B220
		public void Save(GlobalSave save)
		{
			save.Set("GameModeKind", (int)this.m_kind);
			for (int i = 0; i < 3; i++)
			{
				string str = "GameModeSetting ";
				SettingKind settingKind = (SettingKind)i;
				save.Set(str + settingKind.ToString(), this.m_settings[i]);
			}
		}

		// Token: 0x0600202D RID: 8237 RVA: 0x0009CE78 File Offset: 0x0009B278
		public void Load(GlobalSave save)
		{
			int kind = 0;
			save.Get("GameModeKind", out kind, 0);
			this.m_kind = (Kind)kind;
			for (int i = 0; i < 3; i++)
			{
				string str = "GameModeSetting ";
				SettingKind settingKind = (SettingKind)i;
				save.Get(str + settingKind.ToString(), out this.m_settings[i], false);
			}
		}

		// Token: 0x040018AB RID: 6315
		public Kind m_kind;

		// Token: 0x040018AC RID: 6316
		public bool[] m_settings = new bool[3];
	}
}
