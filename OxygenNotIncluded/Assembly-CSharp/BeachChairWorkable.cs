﻿using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200059F RID: 1439
[AddComponentMenu("KMonoBehaviour/Workable/BeachChairWorkable")]
public class BeachChairWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06002317 RID: 8983 RVA: 0x000C0340 File Offset: 0x000BE540
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_beach_chair_kanim")
		};
		this.workAnims = null;
		this.workingPstComplete = null;
		this.workingPstFailed = null;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = false;
		this.lightEfficiencyBonus = false;
		base.SetWorkTime(150f);
		this.beachChair = base.GetComponent<BeachChair>();
	}

	// Token: 0x06002318 RID: 8984 RVA: 0x000C03C1 File Offset: 0x000BE5C1
	protected override void OnStartWork(Worker worker)
	{
		this.timeLit = 0f;
		this.beachChair.SetWorker(worker);
		this.operational.SetActive(true, false);
		worker.GetComponent<Effects>().Add("BeachChairRelaxing", false);
	}

	// Token: 0x06002319 RID: 8985 RVA: 0x000C03FC File Offset: 0x000BE5FC
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		int i = Grid.PosToCell(base.gameObject);
		bool flag = (float)Grid.LightIntensity[i] >= 9999f;
		this.beachChair.SetLit(flag);
		if (flag)
		{
			base.GetComponent<LoopingSounds>().SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 1f);
			this.timeLit += dt;
		}
		else
		{
			base.GetComponent<LoopingSounds>().SetParameter(this.soundPath, this.BEACH_CHAIR_LIT_PARAMETER, 0f);
		}
		return false;
	}

	// Token: 0x0600231A RID: 8986 RVA: 0x000C0484 File Offset: 0x000BE684
	protected override void OnCompleteWork(Worker worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (this.timeLit / this.workTime >= 0.75f)
		{
			component.Add(this.beachChair.specificEffectLit, true);
			component.Remove(this.beachChair.specificEffectUnlit);
		}
		else
		{
			component.Add(this.beachChair.specificEffectUnlit, true);
			component.Remove(this.beachChair.specificEffectLit);
		}
		component.Add(this.beachChair.trackingEffect, true);
	}

	// Token: 0x0600231B RID: 8987 RVA: 0x000C0509 File Offset: 0x000BE709
	protected override void OnStopWork(Worker worker)
	{
		this.operational.SetActive(false, false);
		worker.GetComponent<Effects>().Remove("BeachChairRelaxing");
	}

	// Token: 0x0600231C RID: 8988 RVA: 0x000C0528 File Offset: 0x000BE728
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (component.HasEffect(this.beachChair.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (component.HasEffect(this.beachChair.specificEffectLit) || component.HasEffect(this.beachChair.specificEffectUnlit))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400140A RID: 5130
	[MyCmpReq]
	private Operational operational;

	// Token: 0x0400140B RID: 5131
	private float timeLit;

	// Token: 0x0400140C RID: 5132
	public string soundPath = GlobalAssets.GetSound("BeachChair_music_lp", false);

	// Token: 0x0400140D RID: 5133
	public HashedString BEACH_CHAIR_LIT_PARAMETER = "beachChair_lit";

	// Token: 0x0400140E RID: 5134
	public int basePriority;

	// Token: 0x0400140F RID: 5135
	private BeachChair beachChair;
}
