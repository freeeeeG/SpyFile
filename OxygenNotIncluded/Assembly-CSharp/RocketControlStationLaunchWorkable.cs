using System;
using TUNING;
using UnityEngine;

// Token: 0x02000685 RID: 1669
[AddComponentMenu("KMonoBehaviour/Workable/RocketControlStationLaunchWorkable")]
public class RocketControlStationLaunchWorkable : Workable
{
	// Token: 0x06002C9E RID: 11422 RVA: 0x000ED3FC File Offset: 0x000EB5FC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_rocket_control_station_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		this.attributeConverter = Db.Get().AttributeConverters.PilotingSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Rocketry.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
		base.SetWorkTime(30f);
	}

	// Token: 0x06002C9F RID: 11423 RVA: 0x000ED494 File Offset: 0x000EB694
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		RocketControlStation.StatesInstance smi = this.GetSMI<RocketControlStation.StatesInstance>();
		if (smi != null)
		{
			smi.SetPilotSpeedMult(worker);
			smi.LaunchRocket();
		}
	}

	// Token: 0x04001A49 RID: 6729
	[MyCmpReq]
	private Operational operational;
}
