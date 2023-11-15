using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020007FB RID: 2043
[AddComponentMenu("KMonoBehaviour/Workable/HotTubWorkable")]
public class HotTubWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06003A59 RID: 14937 RVA: 0x00144C11 File Offset: 0x00142E11
	private HotTubWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06003A5A RID: 14938 RVA: 0x00144C21 File Offset: 0x00142E21
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		this.faceTargetWhenWorking = true;
		base.SetWorkTime(90f);
	}

	// Token: 0x06003A5B RID: 14939 RVA: 0x00144C50 File Offset: 0x00142E50
	public override Workable.AnimInfo GetAnim(Worker worker)
	{
		Workable.AnimInfo anim = base.GetAnim(worker);
		anim.smi = new HotTubWorkerStateMachine.StatesInstance(worker);
		return anim;
	}

	// Token: 0x06003A5C RID: 14940 RVA: 0x00144C73 File Offset: 0x00142E73
	protected override void OnStartWork(Worker worker)
	{
		this.faceLeft = (UnityEngine.Random.value > 0.5f);
		worker.GetComponent<Effects>().Add("HotTubRelaxing", false);
	}

	// Token: 0x06003A5D RID: 14941 RVA: 0x00144C9D File Offset: 0x00142E9D
	protected override void OnStopWork(Worker worker)
	{
		worker.GetComponent<Effects>().Remove("HotTubRelaxing");
	}

	// Token: 0x06003A5E RID: 14942 RVA: 0x00144CAF File Offset: 0x00142EAF
	public override Vector3 GetFacingTarget()
	{
		return base.transform.GetPosition() + (this.faceLeft ? Vector3.left : Vector3.right);
	}

	// Token: 0x06003A5F RID: 14943 RVA: 0x00144CD8 File Offset: 0x00142ED8
	protected override void OnCompleteWork(Worker worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.hotTub.trackingEffect))
		{
			component.Add(this.hotTub.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(this.hotTub.specificEffect))
		{
			component.Add(this.hotTub.specificEffect, true);
		}
	}

	// Token: 0x06003A60 RID: 14944 RVA: 0x00144D38 File Offset: 0x00142F38
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.hotTub.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.hotTub.trackingEffect) && component.HasEffect(this.hotTub.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.hotTub.specificEffect) && component.HasEffect(this.hotTub.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x040026DB RID: 9947
	public HotTub hotTub;

	// Token: 0x040026DC RID: 9948
	private bool faceLeft;
}
