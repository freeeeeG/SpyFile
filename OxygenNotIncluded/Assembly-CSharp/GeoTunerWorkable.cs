using System;
using TUNING;

// Token: 0x0200060D RID: 1549
public class GeoTunerWorkable : Workable
{
	// Token: 0x060026FE RID: 9982 RVA: 0x000D3CD8 File Offset: 0x000D1ED8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetWorkTime(30f);
		this.requiredSkillPerk = Db.Get().SkillPerks.AllowGeyserTuning.Id;
		base.SetWorkerStatusItem(Db.Get().DuplicantStatusItems.Studying);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_geotuner_kanim")
		};
		this.attributeConverter = Db.Get().AttributeConverters.GeotuningSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.lightEfficiencyBonus = true;
	}
}
