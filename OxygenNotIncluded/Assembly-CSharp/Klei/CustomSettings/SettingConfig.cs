using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02000DCA RID: 3530
	public abstract class SettingConfig
	{
		// Token: 0x06006C98 RID: 27800 RVA: 0x002AE3E4 File Offset: 0x002AC5E4
		public SettingConfig(string id, string label, string tooltip, string default_level_id, string nosweat_default_level_id, long coordinate_dimension = -1L, long coordinate_dimension_width = -1L, bool debug_only = false, bool triggers_custom_game = true, string required_content = "", string missing_content_default = "", bool editor_only = false)
		{
			this.id = id;
			this.label = label;
			this.tooltip = tooltip;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
			this.coordinate_dimension = coordinate_dimension;
			this.coordinate_dimension_width = coordinate_dimension_width;
			this.debug_only = debug_only;
			this.triggers_custom_game = triggers_custom_game;
			this.required_content = required_content;
			this.missing_content_default = missing_content_default;
			this.editor_only = editor_only;
			DebugUtil.DevAssert(coordinate_dimension <= 1162261467L, "CustomGameSetting's coordinate_dimension is too large, if you're seeing this message it means too many settings have been added", null);
		}

		// Token: 0x1700079F RID: 1951
		// (get) Token: 0x06006C99 RID: 27801 RVA: 0x002AE46C File Offset: 0x002AC66C
		// (set) Token: 0x06006C9A RID: 27802 RVA: 0x002AE474 File Offset: 0x002AC674
		public string id { get; private set; }

		// Token: 0x170007A0 RID: 1952
		// (get) Token: 0x06006C9B RID: 27803 RVA: 0x002AE47D File Offset: 0x002AC67D
		// (set) Token: 0x06006C9C RID: 27804 RVA: 0x002AE485 File Offset: 0x002AC685
		public string label { get; private set; }

		// Token: 0x170007A1 RID: 1953
		// (get) Token: 0x06006C9D RID: 27805 RVA: 0x002AE48E File Offset: 0x002AC68E
		// (set) Token: 0x06006C9E RID: 27806 RVA: 0x002AE496 File Offset: 0x002AC696
		public string tooltip { get; private set; }

		// Token: 0x170007A2 RID: 1954
		// (get) Token: 0x06006C9F RID: 27807 RVA: 0x002AE49F File Offset: 0x002AC69F
		// (set) Token: 0x06006CA0 RID: 27808 RVA: 0x002AE4A7 File Offset: 0x002AC6A7
		public long coordinate_dimension { get; protected set; }

		// Token: 0x170007A3 RID: 1955
		// (get) Token: 0x06006CA1 RID: 27809 RVA: 0x002AE4B0 File Offset: 0x002AC6B0
		// (set) Token: 0x06006CA2 RID: 27810 RVA: 0x002AE4B8 File Offset: 0x002AC6B8
		public long coordinate_dimension_width { get; protected set; }

		// Token: 0x170007A4 RID: 1956
		// (get) Token: 0x06006CA3 RID: 27811 RVA: 0x002AE4C1 File Offset: 0x002AC6C1
		// (set) Token: 0x06006CA4 RID: 27812 RVA: 0x002AE4C9 File Offset: 0x002AC6C9
		public string required_content { get; private set; }

		// Token: 0x170007A5 RID: 1957
		// (get) Token: 0x06006CA5 RID: 27813 RVA: 0x002AE4D2 File Offset: 0x002AC6D2
		// (set) Token: 0x06006CA6 RID: 27814 RVA: 0x002AE4DA File Offset: 0x002AC6DA
		public string missing_content_default { get; private set; }

		// Token: 0x170007A6 RID: 1958
		// (get) Token: 0x06006CA7 RID: 27815 RVA: 0x002AE4E3 File Offset: 0x002AC6E3
		// (set) Token: 0x06006CA8 RID: 27816 RVA: 0x002AE4EB File Offset: 0x002AC6EB
		public bool triggers_custom_game { get; protected set; }

		// Token: 0x170007A7 RID: 1959
		// (get) Token: 0x06006CA9 RID: 27817 RVA: 0x002AE4F4 File Offset: 0x002AC6F4
		// (set) Token: 0x06006CAA RID: 27818 RVA: 0x002AE4FC File Offset: 0x002AC6FC
		public bool debug_only { get; protected set; }

		// Token: 0x170007A8 RID: 1960
		// (get) Token: 0x06006CAB RID: 27819 RVA: 0x002AE505 File Offset: 0x002AC705
		// (set) Token: 0x06006CAC RID: 27820 RVA: 0x002AE50D File Offset: 0x002AC70D
		public bool editor_only { get; protected set; }

		// Token: 0x06006CAD RID: 27821
		public abstract SettingLevel GetLevel(string level_id);

		// Token: 0x06006CAE RID: 27822
		public abstract List<SettingLevel> GetLevels();

		// Token: 0x06006CAF RID: 27823 RVA: 0x002AE516 File Offset: 0x002AC716
		public bool IsDefaultLevel(string level_id)
		{
			return level_id == this.default_level_id;
		}

		// Token: 0x06006CB0 RID: 27824 RVA: 0x002AE524 File Offset: 0x002AC724
		public string GetDefaultLevelId()
		{
			if (!DlcManager.IsContentActive(this.required_content) && !string.IsNullOrEmpty(this.missing_content_default))
			{
				return this.missing_content_default;
			}
			return this.default_level_id;
		}

		// Token: 0x06006CB1 RID: 27825 RVA: 0x002AE54D File Offset: 0x002AC74D
		public string GetNoSweatDefaultLevelId()
		{
			if (!DlcManager.IsContentActive(this.required_content) && !string.IsNullOrEmpty(this.missing_content_default))
			{
				return this.missing_content_default;
			}
			return this.nosweat_default_level_id;
		}

		// Token: 0x0400519B RID: 20891
		protected string default_level_id;

		// Token: 0x0400519C RID: 20892
		protected string nosweat_default_level_id;
	}
}
