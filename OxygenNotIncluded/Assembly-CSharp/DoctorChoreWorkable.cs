using System;
using TUNING;
using UnityEngine;

// Token: 0x020005C8 RID: 1480
[AddComponentMenu("KMonoBehaviour/Workable/DoctorChoreWorkable")]
public class DoctorChoreWorkable : Workable
{
	// Token: 0x06002481 RID: 9345 RVA: 0x000C704E File Offset: 0x000C524E
	private DoctorChoreWorkable()
	{
		this.synchronizeAnims = false;
	}

	// Token: 0x06002482 RID: 9346 RVA: 0x000C7060 File Offset: 0x000C5260
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}
}
