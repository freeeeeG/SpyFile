using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020007A6 RID: 1958
[AddComponentMenu("KMonoBehaviour/Workable/EspressoMachineWorkable")]
public class EspressoMachineWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06003674 RID: 13940 RVA: 0x0012608C File Offset: 0x0012428C
	private EspressoMachineWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06003675 RID: 13941 RVA: 0x001260A8 File Offset: 0x001242A8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_espresso_machine_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		base.SetWorkTime(30f);
	}

	// Token: 0x06003676 RID: 13942 RVA: 0x001260F9 File Offset: 0x001242F9
	protected override void OnStartWork(Worker worker)
	{
		this.operational.SetActive(true, false);
	}

	// Token: 0x06003677 RID: 13943 RVA: 0x00126108 File Offset: 0x00124308
	protected override void OnCompleteWork(Worker worker)
	{
		Storage component = base.GetComponent<Storage>();
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		component.ConsumeAndGetDisease(GameTags.Water, EspressoMachine.WATER_MASS_PER_USE, out num, out diseaseInfo, out num2);
		SimUtil.DiseaseInfo diseaseInfo2;
		component.ConsumeAndGetDisease(EspressoMachine.INGREDIENT_TAG, EspressoMachine.INGREDIENT_MASS_PER_USE, out num, out diseaseInfo2, out num2);
		GermExposureMonitor.Instance smi = worker.GetSMI<GermExposureMonitor.Instance>();
		if (smi != null)
		{
			smi.TryInjectDisease(diseaseInfo.idx, diseaseInfo.count, GameTags.Water, Sickness.InfectionVector.Digestion);
			smi.TryInjectDisease(diseaseInfo2.idx, diseaseInfo2.count, EspressoMachine.INGREDIENT_TAG, Sickness.InfectionVector.Digestion);
		}
		Effects component2 = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.specificEffect))
		{
			component2.Add(EspressoMachineWorkable.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.trackingEffect))
		{
			component2.Add(EspressoMachineWorkable.trackingEffect, true);
		}
	}

	// Token: 0x06003678 RID: 13944 RVA: 0x001261C0 File Offset: 0x001243C0
	protected override void OnStopWork(Worker worker)
	{
		this.operational.SetActive(false, false);
	}

	// Token: 0x06003679 RID: 13945 RVA: 0x001261D0 File Offset: 0x001243D0
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.trackingEffect) && component.HasEffect(EspressoMachineWorkable.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(EspressoMachineWorkable.specificEffect) && component.HasEffect(EspressoMachineWorkable.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400213C RID: 8508
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400213D RID: 8509
	public int basePriority = RELAXATION.PRIORITY.TIER5;

	// Token: 0x0400213E RID: 8510
	private static string specificEffect = "Espresso";

	// Token: 0x0400213F RID: 8511
	private static string trackingEffect = "RecentlyRecDrink";
}
