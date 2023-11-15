using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x02000A23 RID: 2595
[AddComponentMenu("KMonoBehaviour/Workable/VerticalWindTunnelWorkable")]
public class VerticalWindTunnelWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004DCB RID: 19915 RVA: 0x001B4557 File Offset: 0x001B2757
	private VerticalWindTunnelWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004DCC RID: 19916 RVA: 0x001B4568 File Offset: 0x001B2768
	public override Workable.AnimInfo GetAnim(Worker worker)
	{
		Workable.AnimInfo anim = base.GetAnim(worker);
		anim.smi = new WindTunnelWorkerStateMachine.StatesInstance(worker, this);
		return anim;
	}

	// Token: 0x06004DCD RID: 19917 RVA: 0x001B458C File Offset: 0x001B278C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(90f);
	}

	// Token: 0x06004DCE RID: 19918 RVA: 0x001B45B4 File Offset: 0x001B27B4
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<Effects>().Add("VerticalWindTunnelFlying", false);
	}

	// Token: 0x06004DCF RID: 19919 RVA: 0x001B45CF File Offset: 0x001B27CF
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<Effects>().Remove("VerticalWindTunnelFlying");
	}

	// Token: 0x06004DD0 RID: 19920 RVA: 0x001B45E8 File Offset: 0x001B27E8
	protected override void OnCompleteWork(Worker worker)
	{
		Effects component = worker.GetComponent<Effects>();
		component.Add(this.windTunnel.trackingEffect, true);
		component.Add(this.windTunnel.specificEffect, true);
	}

	// Token: 0x06004DD1 RID: 19921 RVA: 0x001B4618 File Offset: 0x001B2818
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.windTunnel.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (component.HasEffect(this.windTunnel.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (component.HasEffect(this.windTunnel.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040032B1 RID: 12977
	public VerticalWindTunnel windTunnel;

	// Token: 0x040032B2 RID: 12978
	public HashedString overrideAnim;

	// Token: 0x040032B3 RID: 12979
	public string[] preAnims;

	// Token: 0x040032B4 RID: 12980
	public string loopAnim;

	// Token: 0x040032B5 RID: 12981
	public string[] pstAnims;
}
