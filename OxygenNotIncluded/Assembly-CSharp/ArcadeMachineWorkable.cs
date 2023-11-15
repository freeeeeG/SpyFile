using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200058C RID: 1420
[AddComponentMenu("KMonoBehaviour/Workable/ArcadeMachineWorkable")]
public class ArcadeMachineWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06002256 RID: 8790 RVA: 0x000BCC86 File Offset: 0x000BAE86
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x000BCCB6 File Offset: 0x000BAEB6
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("ArcadePlaying", false);
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x000BCCD1 File Offset: 0x000BAED1
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<Effects>().Remove("ArcadePlaying");
	}

	// Token: 0x06002259 RID: 8793 RVA: 0x000BCCEC File Offset: 0x000BAEEC
	protected override void OnCompleteWork(Worker worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect))
		{
			component.Add(ArcadeMachineWorkable.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect))
		{
			component.Add(ArcadeMachineWorkable.specificEffect, true);
		}
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x000BCD34 File Offset: 0x000BAF34
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.trackingEffect) && component.HasEffect(ArcadeMachineWorkable.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(ArcadeMachineWorkable.specificEffect) && component.HasEffect(ArcadeMachineWorkable.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x0400139D RID: 5021
	public ArcadeMachine owner;

	// Token: 0x0400139E RID: 5022
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x0400139F RID: 5023
	private static string specificEffect = "PlayedArcade";

	// Token: 0x040013A0 RID: 5024
	private static string trackingEffect = "RecentlyPlayedArcade";
}
