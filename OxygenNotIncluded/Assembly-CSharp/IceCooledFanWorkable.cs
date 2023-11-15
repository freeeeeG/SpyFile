using System;
using TUNING;
using UnityEngine;

// Token: 0x02000619 RID: 1561
[AddComponentMenu("KMonoBehaviour/Workable/IceCooledFanWorkable")]
public class IceCooledFanWorkable : Workable
{
	// Token: 0x06002763 RID: 10083 RVA: 0x000D5B77 File Offset: 0x000D3D77
	private IceCooledFanWorkable()
	{
		this.showProgressBar = false;
	}

	// Token: 0x06002764 RID: 10084 RVA: 0x000D5B88 File Offset: 0x000D3D88
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		this.workerStatusItem = null;
	}

	// Token: 0x06002765 RID: 10085 RVA: 0x000D5BE7 File Offset: 0x000D3DE7
	protected override void OnSpawn()
	{
		GameScheduler.Instance.Schedule("InsulationTutorial", 2f, delegate(object obj)
		{
			Tutorial.Instance.TutorialMessage(Tutorial.TutorialMessages.TM_Insulation, true);
		}, null, null);
		base.OnSpawn();
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x000D5C25 File Offset: 0x000D3E25
	protected override void OnStartWork(Worker worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06002767 RID: 10087 RVA: 0x000D5C34 File Offset: 0x000D3E34
	protected override void OnStopWork(Worker worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x06002768 RID: 10088 RVA: 0x000D5C43 File Offset: 0x000D3E43
	protected override void OnCompleteWork(Worker worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x040016B3 RID: 5811
	[MyCmpGet]
	private Operational operational;
}
