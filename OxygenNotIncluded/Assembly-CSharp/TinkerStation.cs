using System;
using System.Collections.Generic;
using Klei;
using Klei.AI;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x02000A06 RID: 2566
[AddComponentMenu("KMonoBehaviour/Workable/TinkerStation")]
public class TinkerStation : Workable, IGameObjectEffectDescriptor, ISim1000ms
{
	// Token: 0x170005A7 RID: 1447
	// (set) Token: 0x06004CB0 RID: 19632 RVA: 0x001AE113 File Offset: 0x001AC313
	public AttributeConverter AttributeConverter
	{
		set
		{
			this.attributeConverter = value;
		}
	}

	// Token: 0x170005A8 RID: 1448
	// (set) Token: 0x06004CB1 RID: 19633 RVA: 0x001AE11C File Offset: 0x001AC31C
	public float AttributeExperienceMultiplier
	{
		set
		{
			this.attributeExperienceMultiplier = value;
		}
	}

	// Token: 0x170005A9 RID: 1449
	// (set) Token: 0x06004CB2 RID: 19634 RVA: 0x001AE125 File Offset: 0x001AC325
	public string SkillExperienceSkillGroup
	{
		set
		{
			this.skillExperienceSkillGroup = value;
		}
	}

	// Token: 0x170005AA RID: 1450
	// (set) Token: 0x06004CB3 RID: 19635 RVA: 0x001AE12E File Offset: 0x001AC32E
	public float SkillExperienceMultiplier
	{
		set
		{
			this.skillExperienceMultiplier = value;
		}
	}

	// Token: 0x06004CB4 RID: 19636 RVA: 0x001AE138 File Offset: 0x001AC338
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Technicals.Id;
		this.skillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;
		if (this.useFilteredStorage)
		{
			ChoreType byHash = Db.Get().ChoreTypes.GetByHash(this.fetchChoreType);
			this.filteredStorage = new FilteredStorage(this, null, null, false, byHash);
		}
		base.SetWorkTime(15f);
		base.Subscribe<TinkerStation>(-592767678, TinkerStation.OnOperationalChangedDelegate);
	}

	// Token: 0x06004CB5 RID: 19637 RVA: 0x001AE1DA File Offset: 0x001AC3DA
	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (this.useFilteredStorage && this.filteredStorage != null)
		{
			this.filteredStorage.FilterChanged();
		}
	}

	// Token: 0x06004CB6 RID: 19638 RVA: 0x001AE1FD File Offset: 0x001AC3FD
	protected override void OnCleanUp()
	{
		if (this.filteredStorage != null)
		{
			this.filteredStorage.CleanUp();
		}
		base.OnCleanUp();
	}

	// Token: 0x06004CB7 RID: 19639 RVA: 0x001AE218 File Offset: 0x001AC418
	private bool CorrectRolePrecondition(MinionIdentity worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		return component != null && component.HasPerk(this.requiredSkillPerk);
	}

	// Token: 0x06004CB8 RID: 19640 RVA: 0x001AE248 File Offset: 0x001AC448
	private void OnOperationalChanged(object data)
	{
		RoomTracker component = base.GetComponent<RoomTracker>();
		if (component != null && component.room != null)
		{
			component.room.RetriggerBuildings();
		}
	}

	// Token: 0x06004CB9 RID: 19641 RVA: 0x001AE278 File Offset: 0x001AC478
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		if (!this.operational.IsOperational)
		{
			return;
		}
		base.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06004CBA RID: 19642 RVA: 0x001AE2B8 File Offset: 0x001AC4B8
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		base.ShowProgressBar(false);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().BuildingStatusItems.ComplexFabricatorProducing, this);
		this.operational.SetActive(false, false);
	}

	// Token: 0x06004CBB RID: 19643 RVA: 0x001AE2F8 File Offset: 0x001AC4F8
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		float num;
		SimUtil.DiseaseInfo diseaseInfo;
		float num2;
		this.storage.ConsumeAndGetDisease(this.inputMaterial, this.massPerTinker, out num, out diseaseInfo, out num2);
		GameObject gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.outputPrefab), base.transform.GetPosition(), Grid.SceneLayer.Ore, null, 0);
		gameObject.GetComponent<PrimaryElement>().Temperature = this.outputTemperature;
		gameObject.SetActive(true);
		this.chore = null;
	}

	// Token: 0x06004CBC RID: 19644 RVA: 0x001AE366 File Offset: 0x001AC566
	public void Sim1000ms(float dt)
	{
		this.UpdateChore();
	}

	// Token: 0x06004CBD RID: 19645 RVA: 0x001AE370 File Offset: 0x001AC570
	private void UpdateChore()
	{
		if (this.operational.IsOperational && (this.ToolsRequested() || this.alwaysTinker) && this.HasMaterial())
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<TinkerStation>(Db.Get().ChoreTypes.GetByHash(this.choreType), this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
				base.SetWorkTime(this.workTime);
				return;
			}
		}
		else if (this.chore != null)
		{
			this.chore.Cancel("Can't tinker");
			this.chore = null;
		}
	}

	// Token: 0x06004CBE RID: 19646 RVA: 0x001AE423 File Offset: 0x001AC623
	private bool HasMaterial()
	{
		return this.storage.MassStored() > 0f;
	}

	// Token: 0x06004CBF RID: 19647 RVA: 0x001AE438 File Offset: 0x001AC638
	private bool ToolsRequested()
	{
		return MaterialNeeds.GetAmount(this.outputPrefab, base.gameObject.GetMyWorldId(), false) > 0f && this.GetMyWorld().worldInventory.GetAmount(this.outputPrefab, true) <= 0f;
	}

	// Token: 0x06004CC0 RID: 19648 RVA: 0x001AE488 File Offset: 0x001AC688
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		string arg = this.inputMaterial.ProperName();
		List<Descriptor> descriptors = base.GetDescriptors(go);
		descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ELEMENTCONSUMEDPERUSE, arg, GameUtil.GetFormattedMass(this.massPerTinker, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}")), Descriptor.DescriptorType.Requirement, false));
		descriptors.AddRange(GameUtil.GetAllDescriptors(Assets.GetPrefab(this.outputPrefab), false));
		List<Tinkerable> list = new List<Tinkerable>();
		foreach (GameObject gameObject in Assets.GetPrefabsWithComponent<Tinkerable>())
		{
			Tinkerable component = gameObject.GetComponent<Tinkerable>();
			if (component.tinkerMaterialTag == this.outputPrefab)
			{
				list.Add(component);
			}
		}
		if (list.Count > 0)
		{
			Effect effect = Db.Get().effects.Get(list[0].addedEffect);
			descriptors.Add(new Descriptor(string.Format(UI.BUILDINGEFFECTS.ADDED_EFFECT, effect.Name), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.ADDED_EFFECT, effect.Name, Effect.CreateTooltip(effect, true, "\n    • ", true)), Descriptor.DescriptorType.Effect, false));
			descriptors.Add(new Descriptor(UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS, UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS, Descriptor.DescriptorType.Effect, false));
			foreach (Tinkerable cmp in list)
			{
				Descriptor item = new Descriptor(string.Format(UI.BUILDINGEFFECTS.IMPROVED_BUILDINGS_ITEM, cmp.GetProperName()), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.IMPROVED_BUILDINGS_ITEM, cmp.GetProperName()), Descriptor.DescriptorType.Effect, false);
				item.IncreaseIndent();
				descriptors.Add(item);
			}
		}
		return descriptors;
	}

	// Token: 0x06004CC1 RID: 19649 RVA: 0x001AE688 File Offset: 0x001AC888
	public static TinkerStation AddTinkerStation(GameObject go, string required_room_type)
	{
		TinkerStation result = go.AddOrGet<TinkerStation>();
		go.AddOrGet<RoomTracker>().requiredRoomType = required_room_type;
		return result;
	}

	// Token: 0x04003207 RID: 12807
	public HashedString choreType;

	// Token: 0x04003208 RID: 12808
	public HashedString fetchChoreType;

	// Token: 0x04003209 RID: 12809
	private Chore chore;

	// Token: 0x0400320A RID: 12810
	[MyCmpAdd]
	private Operational operational;

	// Token: 0x0400320B RID: 12811
	[MyCmpAdd]
	private Storage storage;

	// Token: 0x0400320C RID: 12812
	public bool useFilteredStorage;

	// Token: 0x0400320D RID: 12813
	protected FilteredStorage filteredStorage;

	// Token: 0x0400320E RID: 12814
	public bool alwaysTinker;

	// Token: 0x0400320F RID: 12815
	public float massPerTinker;

	// Token: 0x04003210 RID: 12816
	public Tag inputMaterial;

	// Token: 0x04003211 RID: 12817
	public Tag outputPrefab;

	// Token: 0x04003212 RID: 12818
	public float outputTemperature;

	// Token: 0x04003213 RID: 12819
	private static readonly EventSystem.IntraObjectHandler<TinkerStation> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<TinkerStation>(delegate(TinkerStation component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
