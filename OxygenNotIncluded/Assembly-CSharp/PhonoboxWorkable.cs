using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020008D9 RID: 2265
[AddComponentMenu("KMonoBehaviour/Workable/PhonoboxWorkable")]
public class PhonoboxWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004181 RID: 16769 RVA: 0x0016EB1C File Offset: 0x0016CD1C
	private PhonoboxWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004182 RID: 16770 RVA: 0x0016EBB5 File Offset: 0x0016CDB5
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.synchronizeAnims = false;
		this.showProgressBar = true;
		this.resetProgressOnStop = true;
		base.SetWorkTime(15f);
	}

	// Token: 0x06004183 RID: 16771 RVA: 0x0016EBE0 File Offset: 0x0016CDE0
	protected override void OnCompleteWork(Worker worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect))
		{
			component.Add(this.trackingEffect, true);
		}
		if (!string.IsNullOrEmpty(this.specificEffect))
		{
			component.Add(this.specificEffect, true);
		}
	}

	// Token: 0x06004184 RID: 16772 RVA: 0x0016EC2C File Offset: 0x0016CE2C
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.trackingEffect) && component.HasEffect(this.trackingEffect))
		{
			priority = 0;
			return false;
		}
		if (!string.IsNullOrEmpty(this.specificEffect) && component.HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x06004185 RID: 16773 RVA: 0x0016EC8B File Offset: 0x0016CE8B
	protected override void OnStartWork(Worker worker)
	{
		this.owner.AddWorker(worker);
		worker.GetComponent<Effects>().Add("Dancing", false);
	}

	// Token: 0x06004186 RID: 16774 RVA: 0x0016ECAB File Offset: 0x0016CEAB
	protected override void OnStopWork(Worker worker)
	{
		this.owner.RemoveWorker(worker);
		worker.GetComponent<Effects>().Remove("Dancing");
	}

	// Token: 0x06004187 RID: 16775 RVA: 0x0016ECCC File Offset: 0x0016CECC
	public override Workable.AnimInfo GetAnim(Worker worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x04002AA3 RID: 10915
	public Phonobox owner;

	// Token: 0x04002AA4 RID: 10916
	public int basePriority = RELAXATION.PRIORITY.TIER3;

	// Token: 0x04002AA5 RID: 10917
	public string specificEffect = "Danced";

	// Token: 0x04002AA6 RID: 10918
	public string trackingEffect = "RecentlyDanced";

	// Token: 0x04002AA7 RID: 10919
	public KAnimFile[][] workerOverrideAnims = new KAnimFile[][]
	{
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_danceone_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_dancetwo_kanim")
		},
		new KAnimFile[]
		{
			Assets.GetAnim("anim_interacts_phonobox_dancethree_kanim")
		}
	};
}
