using System;
using TUNING;
using UnityEngine;

// Token: 0x02000771 RID: 1905
[AddComponentMenu("KMonoBehaviour/Workable/DoctorStationDoctorWorkable")]
public class DoctorStationDoctorWorkable : Workable
{
	// Token: 0x060034AC RID: 13484 RVA: 0x0011AF67 File Offset: 0x00119167
	private DoctorStationDoctorWorkable()
	{
		this.synchronizeAnims = false;
	}

	// Token: 0x060034AD RID: 13485 RVA: 0x0011AF78 File Offset: 0x00119178
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.DoctorSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.BARELY_EVER_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.MedicalAid.Id;
		this.skillExperienceMultiplier = SKILLS.BARELY_EVER_EXPERIENCE;
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x0011AFD0 File Offset: 0x001191D0
	protected override void OnSpawn()
	{
		base.OnSpawn();
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x0011AFD8 File Offset: 0x001191D8
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.station.SetHasDoctor(true);
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x0011AFED File Offset: 0x001191ED
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		this.station.SetHasDoctor(false);
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x0011B002 File Offset: 0x00119202
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		this.station.CompleteDoctoring();
	}

	// Token: 0x04001FCC RID: 8140
	[MyCmpReq]
	private DoctorStation station;
}
