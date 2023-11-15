using System;
using System.Collections.Generic;
using Database;
using STRINGS;
using TUNING;
using UnityEngine;

// Token: 0x020006A4 RID: 1700
[AddComponentMenu("KMonoBehaviour/Workable/Telescope")]
public class Telescope : Workable, OxygenBreather.IGasProvider, IGameObjectEffectDescriptor, ISim200ms, BuildingStatusItems.ISkyVisInfo
{
	// Token: 0x06002DEF RID: 11759 RVA: 0x000F2F5D File Offset: 0x000F115D
	float BuildingStatusItems.ISkyVisInfo.GetPercentVisible01()
	{
		return this.percentClear;
	}

	// Token: 0x06002DF0 RID: 11760 RVA: 0x000F2F68 File Offset: 0x000F1168
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.ALL_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.ALL_DAY_EXPERIENCE;
	}

	// Token: 0x06002DF1 RID: 11761 RVA: 0x000F2FC0 File Offset: 0x000F11C0
	protected override void OnSpawn()
	{
		base.OnSpawn();
		SpacecraftManager.instance.Subscribe(532901469, new Action<object>(this.UpdateWorkingState));
		Components.Telescopes.Add(this);
		this.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(this.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnWorkableEvent));
		this.operational = base.GetComponent<Operational>();
		this.storage = base.GetComponent<Storage>();
		this.UpdateWorkingState(null);
	}

	// Token: 0x06002DF2 RID: 11762 RVA: 0x000F303B File Offset: 0x000F123B
	protected override void OnCleanUp()
	{
		Components.Telescopes.Remove(this);
		SpacecraftManager.instance.Unsubscribe(532901469, new Action<object>(this.UpdateWorkingState));
		base.OnCleanUp();
	}

	// Token: 0x06002DF3 RID: 11763 RVA: 0x000F306C File Offset: 0x000F126C
	public void Sim200ms(float dt)
	{
		base.GetComponent<Building>().GetExtents();
		ValueTuple<bool, float> visibilityOf = TelescopeConfig.SKY_VISIBILITY_INFO.GetVisibilityOf(base.gameObject);
		bool item = visibilityOf.Item1;
		float item2 = visibilityOf.Item2;
		this.percentClear = item2;
		KSelectable component = base.GetComponent<KSelectable>();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisNone, !item, this);
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.SkyVisLimited, item && item2 < 1f, this);
		Operational component2 = base.GetComponent<Operational>();
		component2.SetFlag(Telescope.visibleSkyFlag, item);
		if (!component2.IsActive && component2.IsOperational && this.chore == null)
		{
			this.chore = this.CreateChore();
			base.SetWorkTime(float.PositiveInfinity);
		}
	}

	// Token: 0x06002DF4 RID: 11764 RVA: 0x000F3130 File Offset: 0x000F1330
	private void OnWorkableEvent(Workable workable, Workable.WorkableEvent ev)
	{
		Worker worker = base.worker;
		if (worker == null)
		{
			return;
		}
		OxygenBreather component = worker.GetComponent<OxygenBreather>();
		KPrefabID component2 = worker.GetComponent<KPrefabID>();
		KSelectable component3 = base.GetComponent<KSelectable>();
		if (ev == Workable.WorkableEvent.WorkStarted)
		{
			base.ShowProgressBar(true);
			this.progressBar.SetUpdateFunc(delegate
			{
				if (SpacecraftManager.instance.HasAnalysisTarget())
				{
					return SpacecraftManager.instance.GetDestinationAnalysisScore(SpacecraftManager.instance.GetStarmapAnalysisDestinationID()) / (float)ROCKETRY.DESTINATION_ANALYSIS.COMPLETE;
				}
				return 0f;
			});
			this.workerGasProvider = component.GetGasProvider();
			component.SetGasProvider(this);
			component.GetComponent<CreatureSimTemperatureTransfer>().enabled = false;
			component2.AddTag(GameTags.Shaded, false);
			component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
			return;
		}
		if (ev != Workable.WorkableEvent.WorkStopped)
		{
			return;
		}
		component.SetGasProvider(this.workerGasProvider);
		component.GetComponent<CreatureSimTemperatureTransfer>().enabled = true;
		base.ShowProgressBar(false);
		component2.RemoveTag(GameTags.Shaded);
		component3.AddStatusItem(Db.Get().BuildingStatusItems.TelescopeWorking, this);
	}

	// Token: 0x06002DF5 RID: 11765 RVA: 0x000F3220 File Offset: 0x000F1420
	public override float GetEfficiencyMultiplier(Worker worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.percentClear);
	}

	// Token: 0x06002DF6 RID: 11766 RVA: 0x000F3238 File Offset: 0x000F1438
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (SpacecraftManager.instance.HasAnalysisTarget())
		{
			int starmapAnalysisDestinationID = SpacecraftManager.instance.GetStarmapAnalysisDestinationID();
			SpaceDestination destination = SpacecraftManager.instance.GetDestination(starmapAnalysisDestinationID);
			float num = 1f / (float)destination.OneBasedDistance;
			float num2 = (float)ROCKETRY.DESTINATION_ANALYSIS.DISCOVERED;
			float default_CYCLES_PER_DISCOVERY = ROCKETRY.DESTINATION_ANALYSIS.DEFAULT_CYCLES_PER_DISCOVERY;
			float num3 = num2 / default_CYCLES_PER_DISCOVERY / 600f;
			float points = dt * num * num3;
			SpacecraftManager.instance.EarnDestinationAnalysisPoints(starmapAnalysisDestinationID, points);
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002DF7 RID: 11767 RVA: 0x000F32AC File Offset: 0x000F14AC
	public override List<Descriptor> GetDescriptors(GameObject go)
	{
		List<Descriptor> descriptors = base.GetDescriptors(go);
		Element element = ElementLoader.FindElementByHash(SimHashes.Oxygen);
		Descriptor item = default(Descriptor);
		item.SetupDescriptor(element.tag.ProperName(), string.Format(STRINGS.BUILDINGS.PREFABS.TELESCOPE.REQUIREMENT_TOOLTIP, element.tag.ProperName()), Descriptor.DescriptorType.Requirement);
		descriptors.Add(item);
		return descriptors;
	}

	// Token: 0x06002DF8 RID: 11768 RVA: 0x000F3308 File Offset: 0x000F1508
	protected Chore CreateChore()
	{
		WorkChore<Telescope> workChore = new WorkChore<Telescope>(Db.Get().ChoreTypes.Research, this, null, true, null, null, null, true, null, false, true, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
		workChore.AddPrecondition(Telescope.ContainsOxygen, null);
		return workChore;
	}

	// Token: 0x06002DF9 RID: 11769 RVA: 0x000F3348 File Offset: 0x000F1548
	protected void UpdateWorkingState(object data)
	{
		bool flag = false;
		if (SpacecraftManager.instance.HasAnalysisTarget() && SpacecraftManager.instance.GetDestinationAnalysisState(SpacecraftManager.instance.GetDestination(SpacecraftManager.instance.GetStarmapAnalysisDestinationID())) != SpacecraftManager.DestinationAnalysisState.Complete)
		{
			flag = true;
		}
		KSelectable component = base.GetComponent<KSelectable>();
		bool on = !flag && !SpacecraftManager.instance.AreAllDestinationsAnalyzed();
		component.ToggleStatusItem(Db.Get().BuildingStatusItems.NoApplicableAnalysisSelected, on, null);
		this.operational.SetFlag(Telescope.flag, flag);
		if (!flag && base.worker)
		{
			base.StopWork(base.worker, true);
		}
	}

	// Token: 0x06002DFA RID: 11770 RVA: 0x000F33E5 File Offset: 0x000F15E5
	public void OnSetOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06002DFB RID: 11771 RVA: 0x000F33E7 File Offset: 0x000F15E7
	public void OnClearOxygenBreather(OxygenBreather oxygen_breather)
	{
	}

	// Token: 0x06002DFC RID: 11772 RVA: 0x000F33E9 File Offset: 0x000F15E9
	public bool ShouldEmitCO2()
	{
		return false;
	}

	// Token: 0x06002DFD RID: 11773 RVA: 0x000F33EC File Offset: 0x000F15EC
	public bool ShouldStoreCO2()
	{
		return false;
	}

	// Token: 0x06002DFE RID: 11774 RVA: 0x000F33F0 File Offset: 0x000F15F0
	public bool ConsumeGas(OxygenBreather oxygen_breather, float amount)
	{
		if (this.storage.items.Count <= 0)
		{
			return false;
		}
		GameObject gameObject = this.storage.items[0];
		if (gameObject == null)
		{
			return false;
		}
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		bool result = component.Mass >= amount;
		component.Mass = Mathf.Max(0f, component.Mass - amount);
		return result;
	}

	// Token: 0x06002DFF RID: 11775 RVA: 0x000F345C File Offset: 0x000F165C
	public bool IsLowOxygen()
	{
		if (this.storage.items.Count <= 0)
		{
			return true;
		}
		PrimaryElement primaryElement = this.storage.FindFirstWithMass(GameTags.Breathable, 0f);
		return primaryElement == null || primaryElement.Mass == 0f;
	}

	// Token: 0x04001B10 RID: 6928
	public int clearScanCellRadius = 15;

	// Token: 0x04001B11 RID: 6929
	private OxygenBreather.IGasProvider workerGasProvider;

	// Token: 0x04001B12 RID: 6930
	private Operational operational;

	// Token: 0x04001B13 RID: 6931
	private float percentClear;

	// Token: 0x04001B14 RID: 6932
	private static readonly Operational.Flag visibleSkyFlag = new Operational.Flag("VisibleSky", Operational.Flag.Type.Requirement);

	// Token: 0x04001B15 RID: 6933
	private Storage storage;

	// Token: 0x04001B16 RID: 6934
	public static readonly Chore.Precondition ContainsOxygen = new Chore.Precondition
	{
		id = "ContainsOxygen",
		sortOrder = 1,
		description = DUPLICANTS.CHORES.PRECONDITIONS.CONTAINS_OXYGEN,
		fn = delegate(ref Chore.Precondition.Context context, object data)
		{
			return context.chore.target.GetComponent<Storage>().FindFirstWithMass(GameTags.Oxygen, 0f) != null;
		}
	};

	// Token: 0x04001B17 RID: 6935
	private Chore chore;

	// Token: 0x04001B18 RID: 6936
	private static readonly Operational.Flag flag = new Operational.Flag("ValidTarget", Operational.Flag.Type.Requirement);
}
