using System;
using TUNING;
using UnityEngine;

// Token: 0x020005D1 RID: 1489
[AddComponentMenu("KMonoBehaviour/Workable/CompostWorkable")]
public class CompostWorkable : Workable
{
	// Token: 0x060024FC RID: 9468 RVA: 0x000CA5EC File Offset: 0x000C87EC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.TidyingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Basekeeping.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x060024FD RID: 9469 RVA: 0x000CA644 File Offset: 0x000C8844
	protected override void OnStartWork(Worker worker)
	{
	}

	// Token: 0x060024FE RID: 9470 RVA: 0x000CA646 File Offset: 0x000C8846
	protected override void OnStopWork(Worker worker)
	{
	}
}
