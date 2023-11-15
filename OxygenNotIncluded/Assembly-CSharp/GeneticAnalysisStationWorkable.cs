using System;
using TUNING;
using UnityEngine;

// Token: 0x0200060A RID: 1546
public class GeneticAnalysisStationWorkable : Workable
{
	// Token: 0x060026DE RID: 9950 RVA: 0x000D2BD8 File Offset: 0x000D0DD8
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanIdentifyMutantSeeds.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.AnalyzingGenes;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_genetic_analysisstation_kanim")
		};
		base.SetWorkTime(150f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x060026DF RID: 9951 RVA: 0x000D2C96 File Offset: 0x000D0E96
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, this.storage.FindFirst(GameTags.UnidentifiedSeed));
	}

	// Token: 0x060026E0 RID: 9952 RVA: 0x000D2CCA File Offset: 0x000D0ECA
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorResearching, false);
	}

	// Token: 0x060026E1 RID: 9953 RVA: 0x000D2CEF File Offset: 0x000D0EEF
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		this.IdentifyMutant();
	}

	// Token: 0x060026E2 RID: 9954 RVA: 0x000D2D00 File Offset: 0x000D0F00
	public void IdentifyMutant()
	{
		GameObject gameObject = this.storage.FindFirst(GameTags.UnidentifiedSeed);
		DebugUtil.DevAssertArgs(gameObject != null, new object[]
		{
			"AAACCCCKKK!! GeneticAnalysisStation finished studying a seed but we don't have one in storage??"
		});
		if (gameObject != null)
		{
			Pickupable component = gameObject.GetComponent<Pickupable>();
			Pickupable pickupable;
			if (component.PrimaryElement.Units > 1f)
			{
				pickupable = component.Take(1f);
			}
			else
			{
				pickupable = this.storage.Drop(gameObject, true).GetComponent<Pickupable>();
			}
			pickupable.transform.SetPosition(base.transform.GetPosition() + this.finishedSeedDropOffset);
			MutantPlant component2 = pickupable.GetComponent<MutantPlant>();
			PlantSubSpeciesCatalog.Instance.IdentifySubSpecies(component2.SubSpeciesID);
			component2.Analyze();
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().LogAnalyzedSeed(component2.SpeciesID);
		}
	}

	// Token: 0x04001646 RID: 5702
	[MyCmpAdd]
	public Notifier notifier;

	// Token: 0x04001647 RID: 5703
	[MyCmpReq]
	public Storage storage;

	// Token: 0x04001648 RID: 5704
	[SerializeField]
	public Vector3 finishedSeedDropOffset;

	// Token: 0x04001649 RID: 5705
	private Notification notification;

	// Token: 0x0400164A RID: 5706
	public GeneticAnalysisStation.StatesInstance statesInstance;
}
