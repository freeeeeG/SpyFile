using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02000DCB RID: 3531
	public class ListSettingConfig : SettingConfig
	{
		// Token: 0x170007A9 RID: 1961
		// (get) Token: 0x06006CB2 RID: 27826 RVA: 0x002AE576 File Offset: 0x002AC776
		// (set) Token: 0x06006CB3 RID: 27827 RVA: 0x002AE57E File Offset: 0x002AC77E
		public List<SettingLevel> levels { get; private set; }

		// Token: 0x06006CB4 RID: 27828 RVA: 0x002AE588 File Offset: 0x002AC788
		public ListSettingConfig(string id, string label, string tooltip, List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id, long coordinate_dimension = -1L, long coordinate_dimension_width = -1L, bool debug_only = false, bool triggers_custom_game = true, string required_content = "", string missing_content_default = "", bool editor_only = false) : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_dimension, coordinate_dimension_width, debug_only, triggers_custom_game, required_content, missing_content_default, editor_only)
		{
			this.levels = levels;
		}

		// Token: 0x06006CB5 RID: 27829 RVA: 0x002AE5B8 File Offset: 0x002AC7B8
		public void StompLevels(List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id)
		{
			this.levels = levels;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
		}

		// Token: 0x06006CB6 RID: 27830 RVA: 0x002AE5D0 File Offset: 0x002AC7D0
		public override SettingLevel GetLevel(string level_id)
		{
			for (int i = 0; i < this.levels.Count; i++)
			{
				if (this.levels[i].id == level_id)
				{
					return this.levels[i];
				}
			}
			for (int j = 0; j < this.levels.Count; j++)
			{
				if (this.levels[j].id == this.default_level_id)
				{
					return this.levels[j];
				}
			}
			global::Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
			return null;
		}

		// Token: 0x06006CB7 RID: 27831 RVA: 0x002AE676 File Offset: 0x002AC876
		public override List<SettingLevel> GetLevels()
		{
			return this.levels;
		}

		// Token: 0x06006CB8 RID: 27832 RVA: 0x002AE680 File Offset: 0x002AC880
		public string CycleSettingLevelID(string current_id, int direction)
		{
			string result = "";
			if (current_id == "")
			{
				current_id = this.levels[0].id;
			}
			for (int i = 0; i < this.levels.Count; i++)
			{
				if (this.levels[i].id == current_id)
				{
					int index = Mathf.Clamp(i + direction, 0, this.levels.Count - 1);
					result = this.levels[index].id;
					break;
				}
			}
			return result;
		}

		// Token: 0x06006CB9 RID: 27833 RVA: 0x002AE710 File Offset: 0x002AC910
		public bool IsFirstLevel(string level_id)
		{
			return this.levels.FindIndex((SettingLevel l) => l.id == level_id) == 0;
		}

		// Token: 0x06006CBA RID: 27834 RVA: 0x002AE744 File Offset: 0x002AC944
		public bool IsLastLevel(string level_id)
		{
			return this.levels.FindIndex((SettingLevel l) => l.id == level_id) == this.levels.Count - 1;
		}
	}
}
