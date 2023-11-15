using System;
using TUNING;
using UnityEngine;

// Token: 0x0200065A RID: 1626
public class MissionControlClusterWorkable : Workable
{
	// Token: 0x170002BA RID: 698
	// (get) Token: 0x06002AD9 RID: 10969 RVA: 0x000E4B15 File Offset: 0x000E2D15
	// (set) Token: 0x06002ADA RID: 10970 RVA: 0x000E4B1D File Offset: 0x000E2D1D
	public Clustercraft TargetClustercraft
	{
		get
		{
			return this.targetClustercraft;
		}
		set
		{
			base.WorkTimeRemaining = this.GetWorkTime();
			this.targetClustercraft = value;
		}
	}

	// Token: 0x06002ADB RID: 10971 RVA: 0x000E4B34 File Offset: 0x000E2D34
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

	// Token: 0x06002ADC RID: 10972 RVA: 0x000E4BF2 File Offset: 0x000E2DF2
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Components.MissionControlClusterWorkables.Add(this);
	}

	// Token: 0x06002ADD RID: 10973 RVA: 0x000E4C05 File Offset: 0x000E2E05
	protected override void OnCleanUp()
	{
		Components.MissionControlClusterWorkables.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x06002ADE RID: 10974 RVA: 0x000E4C18 File Offset: 0x000E2E18
	public static bool IsRocketInRange(AxialI worldLocation, AxialI rocketLocation)
	{
		return AxialUtil.GetDistance(worldLocation, rocketLocation) <= 2;
	}

	// Token: 0x06002ADF RID: 10975 RVA: 0x000E4C28 File Offset: 0x000E2E28
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		this.workStatusItem = base.gameObject.GetComponent<KSelectable>().AddStatusItem(Db.Get().BuildingStatusItems.MissionControlAssistingRocket, this.TargetClustercraft);
		this.operational.SetActive(true, false);
	}

	// Token: 0x06002AE0 RID: 10976 RVA: 0x000E4C74 File Offset: 0x000E2E74
	public override float GetEfficiencyMultiplier(Worker worker)
	{
		return base.GetEfficiencyMultiplier(worker) * Mathf.Clamp01(this.GetSMI<SkyVisibilityMonitor.Instance>().PercentClearSky);
	}

	// Token: 0x06002AE1 RID: 10977 RVA: 0x000E4C8E File Offset: 0x000E2E8E
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (this.TargetClustercraft == null || !MissionControlClusterWorkable.IsRocketInRange(base.gameObject.GetMyWorldLocation(), this.TargetClustercraft.Location))
		{
			worker.StopWork();
			return true;
		}
		return base.OnWorkTick(worker, dt);
	}

	// Token: 0x06002AE2 RID: 10978 RVA: 0x000E4CCB File Offset: 0x000E2ECB
	protected override void OnCompleteWork(Worker worker)
	{
		global::Debug.Assert(this.TargetClustercraft != null);
		base.gameObject.GetSMI<MissionControlCluster.Instance>().ApplyEffect(this.TargetClustercraft);
		base.OnCompleteWork(worker);
	}

	// Token: 0x06002AE3 RID: 10979 RVA: 0x000E4CFB File Offset: 0x000E2EFB
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		base.gameObject.GetComponent<KSelectable>().RemoveStatusItem(this.workStatusItem, false);
		this.TargetClustercraft = null;
		this.operational.SetActive(false, false);
	}

	// Token: 0x0400190E RID: 6414
	private Clustercraft targetClustercraft;

	// Token: 0x0400190F RID: 6415
	[MyCmpReq]
	private Operational operational;

	// Token: 0x04001910 RID: 6416
	private Guid workStatusItem = Guid.Empty;
}
