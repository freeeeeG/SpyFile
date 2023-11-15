using System;
using KSerialization;
using TUNING;
using UnityEngine;

// Token: 0x020007F3 RID: 2035
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/Workable/Harvestable")]
public class Harvestable : Workable
{
	// Token: 0x17000432 RID: 1074
	// (get) Token: 0x060039F1 RID: 14833 RVA: 0x00142ECD File Offset: 0x001410CD
	// (set) Token: 0x060039F2 RID: 14834 RVA: 0x00142ED5 File Offset: 0x001410D5
	public Worker completed_by { get; protected set; }

	// Token: 0x17000433 RID: 1075
	// (get) Token: 0x060039F3 RID: 14835 RVA: 0x00142EDE File Offset: 0x001410DE
	public bool CanBeHarvested
	{
		get
		{
			return this.canBeHarvested;
		}
	}

	// Token: 0x060039F4 RID: 14836 RVA: 0x00142EE6 File Offset: 0x001410E6
	protected Harvestable()
	{
		base.SetOffsetTable(OffsetGroups.InvertedStandardTable);
	}

	// Token: 0x060039F5 RID: 14837 RVA: 0x00142EF9 File Offset: 0x001410F9
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Harvesting;
		this.multitoolContext = "harvest";
		this.multitoolHitEffectTag = "fx_harvest_splash";
	}

	// Token: 0x060039F6 RID: 14838 RVA: 0x00142F38 File Offset: 0x00141138
	protected override void OnSpawn()
	{
		this.harvestDesignatable = base.GetComponent<HarvestDesignatable>();
		base.Subscribe<Harvestable>(2127324410, Harvestable.ForceCancelHarvestDelegate);
		base.SetWorkTime(10f);
		base.Subscribe<Harvestable>(2127324410, Harvestable.OnCancelDelegate);
		this.faceTargetWhenWorking = true;
		Components.Harvestables.Add(this);
		this.attributeConverter = Db.Get().AttributeConverters.HarvestSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Farming.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
	}

	// Token: 0x060039F7 RID: 14839 RVA: 0x00142FD5 File Offset: 0x001411D5
	public void OnUprooted(object data)
	{
		if (this.canBeHarvested)
		{
			this.Harvest();
		}
	}

	// Token: 0x060039F8 RID: 14840 RVA: 0x00142FE8 File Offset: 0x001411E8
	public void Harvest()
	{
		this.harvestDesignatable.MarkedForHarvest = false;
		this.chore = null;
		base.Trigger(1272413801, this);
		KSelectable component = base.GetComponent<KSelectable>();
		component.RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
		component.RemoveStatusItem(Db.Get().MiscStatusItems.Operating, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x060039F9 RID: 14841 RVA: 0x0014305C File Offset: 0x0014125C
	public void OnMarkedForHarvest()
	{
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.chore == null)
		{
			this.chore = new WorkChore<Harvestable>(Db.Get().ChoreTypes.Harvest, this, null, true, null, null, null, true, null, false, true, null, true, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			component.AddStatusItem(Db.Get().MiscStatusItems.PendingHarvest, this);
		}
		component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
	}

	// Token: 0x060039FA RID: 14842 RVA: 0x001430D4 File Offset: 0x001412D4
	public void SetCanBeHarvested(bool state)
	{
		this.canBeHarvested = state;
		KSelectable component = base.GetComponent<KSelectable>();
		if (this.canBeHarvested)
		{
			component.AddStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest, null);
			if (this.harvestDesignatable.HarvestWhenReady)
			{
				this.harvestDesignatable.MarkForHarvest();
			}
			else if (this.harvestDesignatable.InPlanterBox)
			{
				component.AddStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, this);
			}
		}
		else
		{
			component.RemoveStatusItem(Db.Get().CreatureStatusItems.ReadyForHarvest, false);
			component.RemoveStatusItem(Db.Get().MiscStatusItems.NotMarkedForHarvest, false);
		}
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x060039FB RID: 14843 RVA: 0x00143191 File Offset: 0x00141391
	protected override void OnCompleteWork(Worker worker)
	{
		this.completed_by = worker;
		this.Harvest();
	}

	// Token: 0x060039FC RID: 14844 RVA: 0x001431A0 File Offset: 0x001413A0
	protected virtual void OnCancel(object data)
	{
		if (this.chore != null)
		{
			this.chore.Cancel("Cancel harvest");
			this.chore = null;
			base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
			this.harvestDesignatable.SetHarvestWhenReady(false);
		}
		this.harvestDesignatable.MarkedForHarvest = false;
	}

	// Token: 0x060039FD RID: 14845 RVA: 0x00143200 File Offset: 0x00141400
	public bool HasChore()
	{
		return this.chore != null;
	}

	// Token: 0x060039FE RID: 14846 RVA: 0x0014320D File Offset: 0x0014140D
	public virtual void ForceCancelHarvest(object data = null)
	{
		this.OnCancel(null);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
		Game.Instance.userMenu.Refresh(base.gameObject);
	}

	// Token: 0x060039FF RID: 14847 RVA: 0x00143247 File Offset: 0x00141447
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Harvestables.Remove(this);
	}

	// Token: 0x06003A00 RID: 14848 RVA: 0x0014325A File Offset: 0x0014145A
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		base.GetComponent<KSelectable>().RemoveStatusItem(Db.Get().MiscStatusItems.PendingHarvest, false);
	}

	// Token: 0x04002694 RID: 9876
	public HarvestDesignatable harvestDesignatable;

	// Token: 0x04002695 RID: 9877
	[Serialize]
	protected bool canBeHarvested;

	// Token: 0x04002697 RID: 9879
	protected Chore chore;

	// Token: 0x04002698 RID: 9880
	private static readonly EventSystem.IntraObjectHandler<Harvestable> ForceCancelHarvestDelegate = new EventSystem.IntraObjectHandler<Harvestable>(delegate(Harvestable component, object data)
	{
		component.ForceCancelHarvest(data);
	});

	// Token: 0x04002699 RID: 9881
	private static readonly EventSystem.IntraObjectHandler<Harvestable> OnCancelDelegate = new EventSystem.IntraObjectHandler<Harvestable>(delegate(Harvestable component, object data)
	{
		component.OnCancel(data);
	});
}
