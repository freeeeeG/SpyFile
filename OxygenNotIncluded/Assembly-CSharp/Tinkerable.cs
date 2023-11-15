using System;
using System.Collections.Generic;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000A07 RID: 2567
[AddComponentMenu("KMonoBehaviour/Workable/Tinkerable")]
public class Tinkerable : Workable
{
	// Token: 0x06004CC4 RID: 19652 RVA: 0x001AE6C0 File Offset: 0x001AC8C0
	public static Tinkerable MakePowerTinkerable(GameObject prefab)
	{
		RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.PowerPlant.Id;
		roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
		Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
		tinkerable.tinkerMaterialTag = PowerControlStationConfig.TINKER_TOOLS;
		tinkerable.tinkerMaterialAmount = 1f;
		tinkerable.requiredSkillPerk = PowerControlStationConfig.ROLE_PERK;
		tinkerable.SetWorkTime(180f);
		tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		tinkerable.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		tinkerable.choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;
		tinkerable.choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;
		tinkerable.addedEffect = "PowerTinker";
		tinkerable.effectAttributeId = Db.Get().Attributes.Machinery.Id;
		tinkerable.effectMultiplier = 0.025f;
		tinkerable.multitoolContext = "powertinker";
		tinkerable.multitoolHitEffectTag = "fx_powertinker_splash";
		tinkerable.shouldShowSkillPerkStatusItem = false;
		prefab.AddOrGet<Storage>();
		prefab.AddOrGet<Effects>();
		prefab.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable);
		};
		return tinkerable;
	}

	// Token: 0x06004CC5 RID: 19653 RVA: 0x001AE820 File Offset: 0x001ACA20
	public static Tinkerable MakeFarmTinkerable(GameObject prefab)
	{
		RoomTracker roomTracker = prefab.AddOrGet<RoomTracker>();
		roomTracker.requiredRoomType = Db.Get().RoomTypes.Farm.Id;
		roomTracker.requirement = RoomTracker.Requirement.TrackingOnly;
		Tinkerable tinkerable = prefab.AddOrGet<Tinkerable>();
		tinkerable.tinkerMaterialTag = FarmStationConfig.TINKER_TOOLS;
		tinkerable.tinkerMaterialAmount = 1f;
		tinkerable.requiredSkillPerk = Db.Get().SkillPerks.CanFarmTinker.Id;
		tinkerable.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		tinkerable.addedEffect = "FarmTinker";
		tinkerable.effectAttributeId = Db.Get().Attributes.Botanist.Id;
		tinkerable.effectMultiplier = 0.1f;
		tinkerable.SetWorkTime(15f);
		tinkerable.attributeConverter = Db.Get().AttributeConverters.PlantTendSpeed;
		tinkerable.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		tinkerable.choreTypeTinker = Db.Get().ChoreTypes.CropTend.IdHash;
		tinkerable.choreTypeFetch = Db.Get().ChoreTypes.FarmFetch.IdHash;
		tinkerable.multitoolContext = "tend";
		tinkerable.multitoolHitEffectTag = "fx_tend_splash";
		tinkerable.shouldShowSkillPerkStatusItem = false;
		prefab.AddOrGet<Storage>();
		prefab.AddOrGet<Effects>();
		prefab.GetComponent<KPrefabID>().prefabInitFn += delegate(GameObject inst)
		{
			inst.GetComponent<Tinkerable>().SetOffsetTable(OffsetGroups.InvertedStandardTable);
		};
		return tinkerable;
	}

	// Token: 0x06004CC6 RID: 19654 RVA: 0x001AE98C File Offset: 0x001ACB8C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_use_machine_kanim")
		};
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Tinkering;
		this.attributeConverter = Db.Get().AttributeConverters.MachinerySpeed;
		this.faceTargetWhenWorking = true;
		this.synchronizeAnims = false;
		base.Subscribe<Tinkerable>(-1157678353, Tinkerable.OnEffectRemovedDelegate);
		base.Subscribe<Tinkerable>(-1697596308, Tinkerable.OnStorageChangeDelegate);
		base.Subscribe<Tinkerable>(144050788, Tinkerable.OnUpdateRoomDelegate);
		base.Subscribe<Tinkerable>(-592767678, Tinkerable.OnOperationalChangedDelegate);
	}

	// Token: 0x06004CC7 RID: 19655 RVA: 0x001AEA39 File Offset: 0x001ACC39
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Prioritizable.AddRef(base.gameObject);
	}

	// Token: 0x06004CC8 RID: 19656 RVA: 0x001AEA4C File Offset: 0x001ACC4C
	protected override void OnCleanUp()
	{
		this.UpdateMaterialReservation(false);
		if (this.updateHandle.IsValid)
		{
			this.updateHandle.ClearScheduler();
		}
		Prioritizable.RemoveRef(base.gameObject);
		base.OnCleanUp();
	}

	// Token: 0x06004CC9 RID: 19657 RVA: 0x001AEA7E File Offset: 0x001ACC7E
	private void OnOperationalChanged(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06004CCA RID: 19658 RVA: 0x001AEA86 File Offset: 0x001ACC86
	private void OnEffectRemoved(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06004CCB RID: 19659 RVA: 0x001AEA8E File Offset: 0x001ACC8E
	private void OnUpdateRoom(object data)
	{
		this.QueueUpdateChore();
	}

	// Token: 0x06004CCC RID: 19660 RVA: 0x001AEA96 File Offset: 0x001ACC96
	private void OnStorageChange(object data)
	{
		if (((GameObject)data).IsPrefabID(this.tinkerMaterialTag))
		{
			this.QueueUpdateChore();
		}
	}

	// Token: 0x06004CCD RID: 19661 RVA: 0x001AEAB4 File Offset: 0x001ACCB4
	private void QueueUpdateChore()
	{
		if (this.updateHandle.IsValid)
		{
			this.updateHandle.ClearScheduler();
		}
		this.updateHandle = GameScheduler.Instance.Schedule("UpdateTinkerChore", 1.2f, new Action<object>(this.UpdateChoreCallback), null, null);
	}

	// Token: 0x06004CCE RID: 19662 RVA: 0x001AEB01 File Offset: 0x001ACD01
	private void UpdateChoreCallback(object obj)
	{
		this.UpdateChore();
	}

	// Token: 0x06004CCF RID: 19663 RVA: 0x001AEB0C File Offset: 0x001ACD0C
	private void UpdateChore()
	{
		Operational component = base.GetComponent<Operational>();
		bool flag = component == null || component.IsFunctional;
		bool flag2 = this.HasEffect();
		bool flag3 = this.RoomHasActiveTinkerstation();
		bool flag4 = !flag2 && flag3 && flag;
		bool flag5 = flag2 || !flag3;
		if (this.chore == null && flag4)
		{
			this.UpdateMaterialReservation(true);
			if (this.HasMaterial())
			{
				this.chore = new WorkChore<Tinkerable>(Db.Get().ChoreTypes.GetByHash(this.choreTypeTinker), this, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
				if (component != null)
				{
					this.chore.AddPrecondition(ChorePreconditions.instance.IsFunctional, component);
				}
			}
			else
			{
				this.chore = new FetchChore(Db.Get().ChoreTypes.GetByHash(this.choreTypeFetch), this.storage, this.tinkerMaterialAmount, new HashSet<Tag>
				{
					this.tinkerMaterialTag
				}, FetchChore.MatchCriteria.MatchID, Tag.Invalid, null, null, true, new Action<Chore>(this.OnFetchComplete), null, null, Operational.State.Functional, 0);
			}
			this.chore.AddPrecondition(ChorePreconditions.instance.HasSkillPerk, this.requiredSkillPerk);
			if (!string.IsNullOrEmpty(base.GetComponent<RoomTracker>().requiredRoomType))
			{
				this.chore.AddPrecondition(ChorePreconditions.instance.IsInMyRoom, Grid.PosToCell(base.transform.GetPosition()));
				return;
			}
		}
		else if (this.chore != null && flag5)
		{
			this.UpdateMaterialReservation(false);
			this.chore.Cancel("No longer needed");
			this.chore = null;
		}
	}

	// Token: 0x06004CD0 RID: 19664 RVA: 0x001AECA4 File Offset: 0x001ACEA4
	private bool RoomHasActiveTinkerstation()
	{
		if (!this.roomTracker.IsInCorrectRoom())
		{
			return false;
		}
		if (this.roomTracker.room == null)
		{
			return false;
		}
		foreach (KPrefabID kprefabID in this.roomTracker.room.buildings)
		{
			if (!(kprefabID == null))
			{
				TinkerStation component = kprefabID.GetComponent<TinkerStation>();
				if (component != null && component.outputPrefab == this.tinkerMaterialTag && kprefabID.GetComponent<Operational>().IsOperational)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06004CD1 RID: 19665 RVA: 0x001AED58 File Offset: 0x001ACF58
	private void UpdateMaterialReservation(bool shouldReserve)
	{
		if (shouldReserve && !this.hasReservedMaterial)
		{
			MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, this.tinkerMaterialAmount, base.gameObject.GetMyWorldId());
			this.hasReservedMaterial = shouldReserve;
			return;
		}
		if (!shouldReserve && this.hasReservedMaterial)
		{
			MaterialNeeds.UpdateNeed(this.tinkerMaterialTag, -this.tinkerMaterialAmount, base.gameObject.GetMyWorldId());
			this.hasReservedMaterial = shouldReserve;
		}
	}

	// Token: 0x06004CD2 RID: 19666 RVA: 0x001AEDC3 File Offset: 0x001ACFC3
	private void OnFetchComplete(Chore data)
	{
		this.UpdateMaterialReservation(false);
		this.chore = null;
		this.UpdateChore();
	}

	// Token: 0x06004CD3 RID: 19667 RVA: 0x001AEDDC File Offset: 0x001ACFDC
	protected override void OnCompleteWork(Worker worker)
	{
		base.OnCompleteWork(worker);
		this.storage.ConsumeIgnoringDisease(this.tinkerMaterialTag, this.tinkerMaterialAmount);
		float totalValue = worker.GetAttributes().Get(Db.Get().Attributes.Get(this.effectAttributeId)).GetTotalValue();
		this.effects.Add(this.addedEffect, true).timeRemaining *= 1f + totalValue * this.effectMultiplier;
		this.UpdateMaterialReservation(false);
		this.chore = null;
		this.UpdateChore();
	}

	// Token: 0x06004CD4 RID: 19668 RVA: 0x001AEE6D File Offset: 0x001AD06D
	private bool HasMaterial()
	{
		return this.storage.GetAmountAvailable(this.tinkerMaterialTag) >= this.tinkerMaterialAmount;
	}

	// Token: 0x06004CD5 RID: 19669 RVA: 0x001AEE8B File Offset: 0x001AD08B
	private bool HasEffect()
	{
		return this.effects.HasEffect(this.addedEffect);
	}

	// Token: 0x04003214 RID: 12820
	private Chore chore;

	// Token: 0x04003215 RID: 12821
	[MyCmpGet]
	private Storage storage;

	// Token: 0x04003216 RID: 12822
	[MyCmpGet]
	private Effects effects;

	// Token: 0x04003217 RID: 12823
	[MyCmpGet]
	private RoomTracker roomTracker;

	// Token: 0x04003218 RID: 12824
	public Tag tinkerMaterialTag;

	// Token: 0x04003219 RID: 12825
	public float tinkerMaterialAmount;

	// Token: 0x0400321A RID: 12826
	public string addedEffect;

	// Token: 0x0400321B RID: 12827
	public string effectAttributeId;

	// Token: 0x0400321C RID: 12828
	public float effectMultiplier;

	// Token: 0x0400321D RID: 12829
	public HashedString choreTypeTinker = Db.Get().ChoreTypes.PowerTinker.IdHash;

	// Token: 0x0400321E RID: 12830
	public HashedString choreTypeFetch = Db.Get().ChoreTypes.PowerFetch.IdHash;

	// Token: 0x0400321F RID: 12831
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnEffectRemovedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnEffectRemoved(data);
	});

	// Token: 0x04003220 RID: 12832
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnStorageChangeDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnStorageChange(data);
	});

	// Token: 0x04003221 RID: 12833
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnUpdateRoomDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnUpdateRoom(data);
	});

	// Token: 0x04003222 RID: 12834
	private static readonly EventSystem.IntraObjectHandler<Tinkerable> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<Tinkerable>(delegate(Tinkerable component, object data)
	{
		component.OnOperationalChanged(data);
	});

	// Token: 0x04003223 RID: 12835
	private SchedulerHandle updateHandle;

	// Token: 0x04003224 RID: 12836
	private bool hasReservedMaterial;
}
