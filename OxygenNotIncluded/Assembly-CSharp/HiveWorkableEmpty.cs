using System;
using TUNING;
using UnityEngine;

// Token: 0x02000618 RID: 1560
[AddComponentMenu("KMonoBehaviour/Workable/HiveWorkableEmpty")]
public class HiveWorkableEmpty : Workable
{
	// Token: 0x0600275F RID: 10079 RVA: 0x000D5A68 File Offset: 0x000D3C68
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Emptying;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.workAnims = HiveWorkableEmpty.WORK_ANIMS;
		this.workingPstComplete = new HashedString[]
		{
			HiveWorkableEmpty.PST_ANIM
		};
		this.workingPstFailed = new HashedString[]
		{
			HiveWorkableEmpty.PST_ANIM
		};
	}

	// Token: 0x06002760 RID: 10080 RVA: 0x000D5B10 File Offset: 0x000D3D10
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		if (!this.wasStung)
		{
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().harvestAHiveWithoutGettingStung = true;
		}
	}

	// Token: 0x040016B0 RID: 5808
	private static readonly HashedString[] WORK_ANIMS = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x040016B1 RID: 5809
	private static readonly HashedString PST_ANIM = new HashedString("working_pst");

	// Token: 0x040016B2 RID: 5810
	public bool wasStung;
}
