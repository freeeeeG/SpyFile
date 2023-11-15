using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000A01 RID: 2561
[AddComponentMenu("KMonoBehaviour/Workable/TelephoneWorkable")]
public class TelephoneCallerWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004C9A RID: 19610 RVA: 0x001AD438 File Offset: 0x001AB638
	private TelephoneCallerWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.workingPstComplete = new HashedString[]
		{
			"on_pst"
		};
		this.workAnims = new HashedString[]
		{
			"on_pre",
			"on",
			"on_receiving",
			"on_pre_loop_receiving",
			"on_loop",
			"on_loop_pre"
		};
	}

	// Token: 0x06004C9B RID: 19611 RVA: 0x001AD4E4 File Offset: 0x001AB6E4
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_telephone_kanim")
		};
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.synchronizeAnims = true;
		base.SetWorkTime(40f);
		this.telephone = base.GetComponent<Telephone>();
	}

	// Token: 0x06004C9C RID: 19612 RVA: 0x001AD541 File Offset: 0x001AB741
	protected override void OnStartWork(Worker worker)
	{
		this.operational.SetActive(true, false);
		this.telephone.isInUse = true;
	}

	// Token: 0x06004C9D RID: 19613 RVA: 0x001AD55C File Offset: 0x001AB75C
	protected override void OnCompleteWork(Worker worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (this.telephone.HasTag(GameTags.LongDistanceCall))
		{
			if (!string.IsNullOrEmpty(this.telephone.longDistanceEffect))
			{
				component.Add(this.telephone.longDistanceEffect, true);
			}
		}
		else if (this.telephone.wasAnswered)
		{
			if (!string.IsNullOrEmpty(this.telephone.chatEffect))
			{
				component.Add(this.telephone.chatEffect, true);
			}
		}
		else if (!string.IsNullOrEmpty(this.telephone.babbleEffect))
		{
			component.Add(this.telephone.babbleEffect, true);
		}
		if (!string.IsNullOrEmpty(this.telephone.trackingEffect))
		{
			component.Add(this.telephone.trackingEffect, true);
		}
	}

	// Token: 0x06004C9E RID: 19614 RVA: 0x001AD627 File Offset: 0x001AB827
	protected override void OnStopWork(Worker worker)
	{
		this.operational.SetActive(false, false);
		this.telephone.HangUp();
	}

	// Token: 0x06004C9F RID: 19615 RVA: 0x001AD644 File Offset: 0x001AB844
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.telephone.trackingEffect) && component.HasEffect(this.telephone.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.telephone.chatEffect) && component.HasEffect(this.telephone.chatEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		if (!string.IsNullOrEmpty(this.telephone.babbleEffect) && component.HasEffect(this.telephone.babbleEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040031ED RID: 12781
	[MyCmpReq]
	private Operational operational;

	// Token: 0x040031EE RID: 12782
	public int basePriority;

	// Token: 0x040031EF RID: 12783
	private Telephone telephone;
}
