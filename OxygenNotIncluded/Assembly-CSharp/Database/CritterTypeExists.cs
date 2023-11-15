using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D51 RID: 3409
	public class CritterTypeExists : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006AE4 RID: 27364 RVA: 0x0029A5F8 File Offset: 0x002987F8
		public CritterTypeExists(List<Tag> critterTypes)
		{
			this.critterTypes = critterTypes;
		}

		// Token: 0x06006AE5 RID: 27365 RVA: 0x0029A614 File Offset: 0x00298814
		public override bool Success()
		{
			foreach (Capturable cmp in Components.Capturables.Items)
			{
				if (this.critterTypes.Contains(cmp.PrefabID()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06006AE6 RID: 27366 RVA: 0x0029A680 File Offset: 0x00298880
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.critterTypes = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.critterTypes.Add(new Tag(name));
			}
		}

		// Token: 0x06006AE7 RID: 27367 RVA: 0x0029A6C4 File Offset: 0x002988C4
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.HATCH_A_MORPH;
		}

		// Token: 0x04004DB9 RID: 19897
		private List<Tag> critterTypes = new List<Tag>();
	}
}
