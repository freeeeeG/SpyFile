using System;
using TUNING;

// Token: 0x020009E8 RID: 2536
public class SplatWorkable : Workable
{
	// Token: 0x06004BBB RID: 19387 RVA: 0x001A9344 File Offset: 0x001A7544
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetOffsetTable(OffsetGroups.InvertedStandardTableWithCorners);
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Mopping;
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.multitoolContext = "disinfect";
		this.multitoolHitEffectTag = "fx_disinfect_splash";
		this.synchronizeAnims = false;
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06004BBC RID: 19388 RVA: 0x001A93EE File Offset: 0x001A75EE
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(5f);
	}
}
