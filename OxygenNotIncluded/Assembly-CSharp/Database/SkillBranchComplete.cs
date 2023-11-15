using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000D3F RID: 3391
	public class SkillBranchComplete : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06006A99 RID: 27289 RVA: 0x00298F00 File Offset: 0x00297100
		public SkillBranchComplete(List<Skill> skillsToMaster)
		{
			this.skillsToMaster = skillsToMaster;
		}

		// Token: 0x06006A9A RID: 27290 RVA: 0x00298F10 File Offset: 0x00297110
		public override bool Success()
		{
			foreach (MinionResume minionResume in Components.MinionResumes.Items)
			{
				foreach (Skill skill in this.skillsToMaster)
				{
					if (minionResume.HasMasteredSkill(skill.Id))
					{
						if (!minionResume.HasBeenGrantedSkill(skill))
						{
							return true;
						}
						List<Skill> allPriorSkills = Db.Get().Skills.GetAllPriorSkills(skill);
						bool flag = true;
						foreach (Skill skill2 in allPriorSkills)
						{
							flag = (flag && minionResume.HasMasteredSkill(skill2.Id));
						}
						if (flag)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06006A9B RID: 27291 RVA: 0x00299030 File Offset: 0x00297230
		public void Deserialize(IReader reader)
		{
			this.skillsToMaster = new List<Skill>();
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				string id = reader.ReadKleiString();
				this.skillsToMaster.Add(Db.Get().Skills.Get(id));
			}
		}

		// Token: 0x06006A9C RID: 27292 RVA: 0x0029907D File Offset: 0x0029727D
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.SKILL_BRANCH;
		}

		// Token: 0x04004DA1 RID: 19873
		private List<Skill> skillsToMaster;
	}
}
