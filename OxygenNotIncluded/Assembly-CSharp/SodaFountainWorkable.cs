using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200097F RID: 2431
[AddComponentMenu("KMonoBehaviour/Workable/SodaFountainWorkable")]
public class SodaFountainWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x0600479D RID: 18333 RVA: 0x0019417B File Offset: 0x0019237B
	private SodaFountainWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x0600479E RID: 18334 RVA: 0x0019418C File Offset: 0x0019238C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_sodamaker_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		base.SetWorkTime(30f);
		this.sodaFountain = base.GetComponent<SodaFountain>();
	}

	// Token: 0x0600479F RID: 18335 RVA: 0x001941E9 File Offset: 0x001923E9
	protected override void OnStartWork(Worker worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x060047A0 RID: 18336 RVA: 0x001941F8 File Offset: 0x001923F8
	protected override void OnCompleteWork(Worker worker)
	{
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(GameTags.Water, this.sodaFountain.waterMassPerUse, out num, out diseaseInfo, out num2);
		SimUtil.DiseaseInfo diseaseInfo2;
		component.ConsumeAndGetDisease(this.sodaFountain.ingredientTag, this.sodaFountain.ingredientMassPerUse, out num, out diseaseInfo2, out num2);
		GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
		if (smi != null)
		{
			smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, GameTags.Water, Sickness.InfectionVector.Digestion);
			smi.TryInjectDisease(diseaseInfo2.idx, diseaseInfo2.count, this.sodaFountain.ingredientTag, Sickness.InfectionVector.Digestion);
		}
		Effects component2 = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sodaFountain.specificEffect))
		{
			component2.Add(this.sodaFountain.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.sodaFountain.trackingEffect))
		{
			component2.Add(this.sodaFountain.trackingEffect, true);
		}
	}

	// Token: 0x060047A1 RID: 18337 RVA: 0x001942E0 File Offset: 0x001924E0
	protected override void OnStopWork(Worker worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x060047A2 RID: 18338 RVA: 0x001942F0 File Offset: 0x001924F0
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.sodaFountain.trackingEffect) && component.HasEffect(this.sodaFountain.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.sodaFountain.specificEffect) && component.HasEffect(this.sodaFountain.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04002F71 RID: 12145
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002F72 RID: 12146
	public int basePriority;

	// Token: 0x04002F73 RID: 12147
	private SodaFountain sodaFountain;
}
