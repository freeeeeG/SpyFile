using System;
using System.Collections.Generic;
using TUNING;

namespace Database
{
	// Token: 0x02000D6D RID: 3437
	public class Skill : Resource
	{
		// Token: 0x06006B3F RID: 27455 RVA: 0x0029E478 File Offset: 0x0029C678
		public Skill(string id, string name, string description, string dlcId, int tier, string hat, string badge, string skillGroup, List<SkillPerk> perks = null, List<string> priorSkills = null) : base(id, name)
		{
			this.description = description;
			this.dlcId = dlcId;
			this.tier = tier;
			this.hat = hat;
			this.badge = badge;
			this.skillGroup = skillGroup;
			this.perks = perks;
			if (this.perks == null)
			{
				this.perks = new List<SkillPerk>();
			}
			this.priorSkills = priorSkills;
			if (this.priorSkills == null)
			{
				this.priorSkills = new List<string>();
			}
		}

		// Token: 0x06006B40 RID: 27456 RVA: 0x0029E4F2 File Offset: 0x0029C6F2
		public int GetMoraleExpectation()
		{
			return SKILLS.SKILL_TIER_MORALE_COST[this.tier];
		}

		// Token: 0x06006B41 RID: 27457 RVA: 0x0029E500 File Offset: 0x0029C700
		public bool GivesPerk(SkillPerk perk)
		{
			return this.perks.Contains(perk);
		}

		// Token: 0x06006B42 RID: 27458 RVA: 0x0029E510 File Offset: 0x0029C710
		public bool GivesPerk(HashedString perkId)
		{
			using (List<SkillPerk>.Enumerator enumerator = this.perks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.IdHash == perkId)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x04004E6B RID: 20075
		public string description;

		// Token: 0x04004E6C RID: 20076
		public string dlcId;

		// Token: 0x04004E6D RID: 20077
		public string skillGroup;

		// Token: 0x04004E6E RID: 20078
		public string hat;

		// Token: 0x04004E6F RID: 20079
		public string badge;

		// Token: 0x04004E70 RID: 20080
		public int tier;

		// Token: 0x04004E71 RID: 20081
		public bool deprecated;

		// Token: 0x04004E72 RID: 20082
		public List<SkillPerk> perks;

		// Token: 0x04004E73 RID: 20083
		public List<string> priorSkills;
	}
}
