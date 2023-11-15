using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020008BD RID: 2237
public class NuclearResearchCenterWorkable : Workable
{
	// Token: 0x060040C7 RID: 16583 RVA: 0x00169EAC File Offset: 0x001680AC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Researching;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
		this.radiationStorage = base.GetComponent<HighEnergyParticleStorage>();
		this.nrc = base.GetComponent<NuclearResearchCenter>();
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x060040C8 RID: 16584 RVA: 0x00169F38 File Offset: 0x00168138
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x060040C9 RID: 16585 RVA: 0x00169F4C File Offset: 0x0016814C
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		float num = dt / this.nrc.timePerPoint;
		if (Game.Instance.FastWorkersModeActive)
		{
			num *= 2f;
		}
		this.radiationStorage.ConsumeAndGet(num * this.nrc.materialPerPoint);
		this.pointsProduced += num;
		if (this.pointsProduced >= 1f)
		{
			int num2 = Mathf.FloorToInt(this.pointsProduced);
			this.pointsProduced -= (float)num2;
			PopFXManager.Instance.SpawnFX(PopFXManager.Instance.sprite_Research, Research.Instance.GetResearchType("nuclear").name, base.transform, 1.5f, false);
			Research.Instance.AddResearchPoints("nuclear", (float)num2);
		}
		TechInstance activeResearch = Research.Instance.GetActiveResearch();
		return this.radiationStorage.IsEmpty() || activeResearch == null || activeResearch.PercentageCompleteResearchType("nuclear") >= 1f;
	}

	// Token: 0x060040CA RID: 16586 RVA: 0x0016A042 File Offset: 0x00168242
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.nrc);
	}

	// Token: 0x060040CB RID: 16587 RVA: 0x0016A06C File Offset: 0x0016826C
	protected override void OnAbortWork(Worker worker)
	{
		base.OnAbortWork(worker);
	}

	// Token: 0x060040CC RID: 16588 RVA: 0x0016A075 File Offset: 0x00168275
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.nrc);
	}

	// Token: 0x060040CD RID: 16589 RVA: 0x0016A0A4 File Offset: 0x001682A4
	public override float GetPercentComplete()
	{
		if (Research.Instance.GetActiveResearch() == null)
		{
			return 0f;
		}
		float num = Research.Instance.GetActiveResearch().progressInventory.PointsByTypeID["nuclear"];
		float num2 = 0f;
		if (!Research.Instance.GetActiveResearch().tech.costsByResearchTypeID.TryGetValue("nuclear", out num2))
		{
			return 1f;
		}
		return num / num2;
	}

	// Token: 0x060040CE RID: 16590 RVA: 0x0016A113 File Offset: 0x00168313
	public override bool InstantlyFinish(Worker worker)
	{
		return false;
	}

	// Token: 0x04002A31 RID: 10801
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04002A32 RID: 10802
	[Serialize]
	private float pointsProduced;

	// Token: 0x04002A33 RID: 10803
	private NuclearResearchCenter nrc;

	// Token: 0x04002A34 RID: 10804
	private HighEnergyParticleStorage radiationStorage;
}
