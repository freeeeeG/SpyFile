using System;
using TUNING;
using UnityEngine;

// Token: 0x0200065B RID: 1627
public class MissionControlWorkable : Workable
{
	// Token: 0x170002BB RID: 699
	// (get) Token: 0x06002AE5 RID: 10981 RVA: 0x000E4D43 File Offset: 0x000E2F43
	// (set) Token: 0x06002AE6 RID: 10982 RVA: 0x000E4D4B File Offset: 0x000E2F4B
	public Spacecraft TargetSpacecraft
	{
		get
		{
			return this.targetSpacecraft;
		}
		set
		{
			base.WorkTimeRemaining = this.GetWorkTime();
			this.targetSpacecraft = value;
		}
	}

	// Token: 0x06002AE7 RID: 10983 RVA: 0x000E4D60 File Offset: 0x000E2F60
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.requiredSkillPerk = Db.Get().SkillPerks.CanMissionControl.Id;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.MissionControlling;
		this.attributeConverter = Db.Get().AttributeConverters.ResearchSpeed;
		this.attributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.PART_DAY_EXPERIENCE;
		this.skillExperienceSkillGroup = Db.Get().SkillGroups.Research.Id;
		this.skillExperienceMultiplier = SKILLS.PART_DAY_EXPERIENCE;
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_mission_control_station_kanim")
		};
		base.SetWorkTime(90f);
		this.showProgressBar = true;
		this.lightEfficiencyBonus = true;
	}

	// Token: 0x06002AE8 RID: 10984 RVA: 0x000E4E1E File Offset: 0x000E301E
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlWorkables.Add(this);
	}

	// Token: 0x06002AE9 RID: 10985 RVA: 0x000E4E31 File Offset: 0x000E3031
	protected override void OnCleanUp()
	{
		Components.MissionControlWorkables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002AEA RID: 10986 RVA: 0x000E4E44 File Offset: 0x000E3044
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, this.TargetSpacecraft);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06002AEB RID: 10987 RVA: 0x000E4E90 File Offset: 0x000E3090
	public override float GetEfficiencyMultiplier(Worker worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	// Token: 0x06002AEC RID: 10988 RVA: 0x000E4EAA File Offset: 0x000E30AA
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (this.TargetSpacecraft == null)
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002AED RID: 10989 RVA: 0x000E4EC4 File Offset: 0x000E30C4
	protected override void OnCompleteWork(Worker worker)
	{
		global::Debug.Assert(this.TargetSpacecraft != null);
		base.gameObject.GetSMI<MissionControl.Instance>().ApplyEffect(this.TargetSpacecraft);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06002AEE RID: 10990 RVA: 0x000E4EF1 File Offset: 0x000E30F1
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem, false);
		this.TargetSpacecraft = null;
		this.operational.SetActive(false, false);
	}

	// Token: 0x04001911 RID: 6417
	private Spacecraft targetSpacecraft;

	// Token: 0x04001912 RID: 6418
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001913 RID: 6419
	private Guid workStatusItem = Guid.Empty;
}
