using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020006A9 RID: 1705
[AddComponentMenu("KMonoBehaviour/Workable/ToiletWorkableClean")]
public class ToiletWorkableClean : Workable
{
	// Token: 0x06002E31 RID: 11825 RVA: 0x000F41C8 File Offset: 0x000F23C8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Cleaning;
		this.workingStatusItem = Db.Get().MiscStatusItems.Cleaning;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.workAnims = ToiletWorkableClean.CLEAN_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			ToiletWorkableClean.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			ToiletWorkableClean.PST_ANIM
		};
	}

	// Token: 0x06002E32 RID: 11826 RVA: 0x000F4285 File Offset: 0x000F2485
	protected override void OnCompleteWork(Worker worker)
	{
		this.timesCleaned++;
		base.OnCompleteWork(worker);
	}

	// Token: 0x04001B3A RID: 6970
	[Serialize]
	public int timesCleaned;

	// Token: 0x04001B3B RID: 6971
	private static readonly HashedString[] CLEAN_ANIMS = new HashedString[]
	{
		"unclog_pre",
		"unclog_loop"
	};

	// Token: 0x04001B3C RID: 6972
	private static readonly HashedString PST_ANIM = new HashedString("unclog_pst");
}
