using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Database;

// Token: 0x020006DD RID: 1757
public class ColonyAchievementStatus
{
	// Token: 0x17000353 RID: 851
	// (get) Token: 0x06003034 RID: 12340 RVA: 0x000FEADE File Offset: 0x000FCCDE
	public List<ColonyAchievementRequirement> Requirements
	{
		get
		{
			return this.m_achievement.requirementChecklist;
		}
	}

	// Token: 0x06003035 RID: 12341 RVA: 0x000FEAEB File Offset: 0x000FCCEB
	public ColonyAchievementStatus(string achievementId)
	{
		this.m_achievement = Db.Get().ColonyAchievements.TryGet(achievementId);
	}

	// Token: 0x06003036 RID: 12342 RVA: 0x000FEB0C File Offset: 0x000FCD0C
	public void UpdateAchievement()
	{
		if (this.Requirements.Count <= 0)
		{
			return;
		}
		if (this.m_achievement.Disabled)
		{
			return;
		}
		this.success = true;
		foreach (ColonyAchievementRequirement colonyAchievementRequirement in this.Requirements)
		{
			this.success &= colonyAchievementRequirement.Success();
			this.failed |= colonyAchievementRequirement.Fail();
		}
	}

	// Token: 0x06003037 RID: 12343 RVA: 0x000FEBA4 File Offset: 0x000FCDA4
	public static ColonyAchievementStatus Deserialize(IReader reader, string achievementId)
	{
		bool flag = reader.ReadByte() > 0;
		bool flag2 = reader.ReadByte() > 0;
		if (SaveLoader.Instance.GameInfo.IsVersionOlderThan(7, 22))
		{
			int num = reader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				Type type = Type.GetType(reader.ReadKleiString());
				if (type != null)
				{
					AchievementRequirementSerialization_Deprecated achievementRequirementSerialization_Deprecated = FormatterServices.GetUninitializedObject(type) as AchievementRequirementSerialization_Deprecated;
					Debug.Assert(achievementRequirementSerialization_Deprecated != null, string.Format("Cannot deserialize old data for type {0}", type));
					achievementRequirementSerialization_Deprecated.Deserialize(reader);
				}
			}
		}
		return new ColonyAchievementStatus(achievementId)
		{
			success = flag,
			failed = flag2
		};
	}

	// Token: 0x06003038 RID: 12344 RVA: 0x000FEC45 File Offset: 0x000FCE45
	public void Serialize(BinaryWriter writer)
	{
		writer.Write(this.success ? 1 : 0);
		writer.Write(this.failed ? 1 : 0);
	}

	// Token: 0x04001C67 RID: 7271
	public bool success;

	// Token: 0x04001C68 RID: 7272
	public bool failed;

	// Token: 0x04001C69 RID: 7273
	private ColonyAchievement m_achievement;
}
