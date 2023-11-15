using System;
using Klei;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200085A RID: 2138
[AddComponentMenu("KMonoBehaviour/Workable/MechanicalSurfboardWorkable")]
public class MechanicalSurfboardWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06003E88 RID: 16008 RVA: 0x0015B119 File Offset: 0x00159319
	private MechanicalSurfboardWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06003E89 RID: 16009 RVA: 0x0015B129 File Offset: 0x00159329
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(30f);
		this.surfboard = base.GetComponent<MechanicalSurfboard>();
	}

	// Token: 0x06003E8A RID: 16010 RVA: 0x0015B15D File Offset: 0x0015935D
	protected override void OnStartWork(Worker worker)
	{
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("MechanicalSurfing", false);
	}

	// Token: 0x06003E8B RID: 16011 RVA: 0x0015B180 File Offset: 0x00159380
	public override Workable.AnimInfo GetAnim(Worker worker)
	{
		Workable.AnimInfo result = default(Workable.AnimInfo);
		AttributeInstance attributeInstance = worker.GetAttributes().Get(Db.Get().Attributes.Athletics);
		if (attributeInstance.GetTotalValue() <= 7f)
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[0])
			};
		}
		else if (attributeInstance.GetTotalValue() <= 15f)
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[1])
			};
		}
		else
		{
			result.overrideAnims = new KAnimFile[]
			{
				Assets.GetAnim(this.surfboard.interactAnims[2])
			};
		}
		return result;
	}

	// Token: 0x06003E8C RID: 16012 RVA: 0x0015B244 File Offset: 0x00159444
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		Building component = base.GetComponent<Building>();
		MechanicalSurfboard component2 = base.GetComponent<MechanicalSurfboard>();
		int widthInCells = component.Def.WidthInCells;
		int minInclusive = -(widthInCells - 1) / 2;
		int maxExclusive = widthInCells / 2;
		int x = UnityEngine.Random.Range(minInclusive, maxExclusive);
		float amount = component2.waterSpillRateKG * dt;
		float base_mass;
		SimUtil.DiseaseInfo diseaseInfo;
		float temperature;
		base.GetComponent<Storage>().ConsumeAndGetDisease(SimHashes.Water.CreateTag(), amount, out base_mass, out diseaseInfo, out temperature);
		int cell = Grid.OffsetCell(Grid.PosToCell(base.gameObject), new CellOffset(x, 0));
		ushort elementIndex = ElementLoader.GetElementIndex(SimHashes.Water);
		FallingWater.instance.AddParticle(cell, elementIndex, base_mass, temperature, diseaseInfo.idx, diseaseInfo.count, true, false, false, false);
		return false;
	}

	// Token: 0x06003E8D RID: 16013 RVA: 0x0015B2EC File Offset: 0x001594EC
	protected override void OnCompleteWork(Worker worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.surfboard.specificEffect))
		{
			component.Add(this.surfboard.specificEffect, true);
		}
		if (!string.IsNullOrEmpty(this.surfboard.trackingEffect))
		{
			component.Add(this.surfboard.trackingEffect, true);
		}
	}

	// Token: 0x06003E8E RID: 16014 RVA: 0x0015B34A File Offset: 0x0015954A
	protected override void OnStopWork(Worker worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("MechanicalSurfing");
	}

	// Token: 0x06003E8F RID: 16015 RVA: 0x0015B36C File Offset: 0x0015956C
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.surfboard.trackingEffect) && component.HasEffect(this.surfboard.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.surfboard.specificEffect) && component.HasEffect(this.surfboard.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400288A RID: 10378
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400288B RID: 10379
	public int basePriority;

	// Token: 0x0400288C RID: 10380
	private MechanicalSurfboard surfboard;
}
