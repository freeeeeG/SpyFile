using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02000DCC RID: 3532
	public class ToggleSettingConfig : SettingConfig
	{
		// Token: 0x170007AA RID: 1962
		// (get) Token: 0x06006CBB RID: 27835 RVA: 0x002AE784 File Offset: 0x002AC984
		// (set) Token: 0x06006CBC RID: 27836 RVA: 0x002AE78C File Offset: 0x002AC98C
		public SettingLevel on_level { get; private set; }

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06006CBD RID: 27837 RVA: 0x002AE795 File Offset: 0x002AC995
		// (set) Token: 0x06006CBE RID: 27838 RVA: 0x002AE79D File Offset: 0x002AC99D
		public SettingLevel off_level { get; private set; }

		// Token: 0x06006CBF RID: 27839 RVA: 0x002AE7A8 File Offset: 0x002AC9A8
		public ToggleSettingConfig(string id, string label, string tooltip, SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id, long coordinate_dimension = -1L, long coordinate_dimension_width = -1L, bool debug_only = false, bool triggers_custom_game = true, string required_content = "", string missing_content_default = "") : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_dimension, coordinate_dimension_width, debug_only, triggers_custom_game, required_content, missing_content_default, false)
		{
			this.off_level = off_level;
			this.on_level = on_level;
		}

		// Token: 0x06006CC0 RID: 27840 RVA: 0x002AE7E0 File Offset: 0x002AC9E0
		public override SettingLevel GetLevel(string level_id)
		{
			if (this.on_level.id == level_id)
			{
				return this.on_level;
			}
			if (this.off_level.id == level_id)
			{
				return this.off_level;
			}
			if (this.default_level_id == this.on_level.id)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find level for setting:",
					base.id,
					"(",
					level_id,
					") Using default level."
				}));
				return this.on_level;
			}
			if (this.default_level_id == this.off_level.id)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find level for setting:",
					base.id,
					"(",
					level_id,
					") Using default level."
				}));
				return this.off_level;
			}
			Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
			return null;
		}

		// Token: 0x06006CC1 RID: 27841 RVA: 0x002AE8E5 File Offset: 0x002ACAE5
		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>
			{
				this.off_level,
				this.on_level
			};
		}

		// Token: 0x06006CC2 RID: 27842 RVA: 0x002AE904 File Offset: 0x002ACB04
		public string ToggleSettingLevelID(string current_id)
		{
			if (this.on_level.id == current_id)
			{
				return this.off_level.id;
			}
			return this.on_level.id;
		}

		// Token: 0x06006CC3 RID: 27843 RVA: 0x002AE930 File Offset: 0x002ACB30
		public bool IsOnLevel(string level_id)
		{
			return level_id == this.on_level.id;
		}
	}
}
