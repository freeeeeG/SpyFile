using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02000DCD RID: 3533
	public class SeedSettingConfig : SettingConfig
	{
		// Token: 0x06006CC4 RID: 27844 RVA: 0x002AE944 File Offset: 0x002ACB44
		public SeedSettingConfig(string id, string label, string tooltip, bool debug_only = false, bool triggers_custom_game = true) : base(id, label, tooltip, "", "", -1L, -1L, debug_only, triggers_custom_game, "", "", false)
		{
		}

		// Token: 0x06006CC5 RID: 27845 RVA: 0x002AE977 File Offset: 0x002ACB77
		public override SettingLevel GetLevel(string level_id)
		{
			return new SettingLevel(level_id, level_id, level_id, 0L, null);
		}

		// Token: 0x06006CC6 RID: 27846 RVA: 0x002AE984 File Offset: 0x002ACB84
		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>();
		}
	}
}
