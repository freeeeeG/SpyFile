using System;
using System.Collections.Generic;
using Klei.AI;
using UnityEngine;

// Token: 0x02000977 RID: 2423
[AddComponentMenu("KMonoBehaviour/Workable/Sleepable")]
public class Sleepable : Workable
{
	// Token: 0x06004759 RID: 18265 RVA: 0x001931DC File Offset: 0x001913DC
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.SetReportType(ReportManager.ReportType.PersonalTime);
		this.showProgressBar = false;
		this.workerStatusItem = null;
		this.synchronizeAnims = false;
		this.triggerWorkReactions = false;
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x0600475A RID: 18266 RVA: 0x0019320F File Offset: 0x0019140F
	protected override void OnSpawn()
	{
		Components.Sleepables.Add(this);
		base.SetWorkTime(float.PositiveInfinity);
	}

	// Token: 0x0600475B RID: 18267 RVA: 0x00193228 File Offset: 0x00191428
	public override HashedString[] GetWorkAnims(Worker worker)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return Sleepable.hatWorkAnims;
		}
		return Sleepable.normalWorkAnims;
	}

	// Token: 0x0600475C RID: 18268 RVA: 0x00193268 File Offset: 0x00191468
	public override HashedString[] GetWorkPstAnims(Worker worker, bool successfully_completed)
	{
		MinionResume component = worker.GetComponent<MinionResume>();
		if (base.GetComponent<Building>() != null && component != null && component.CurrentHat != null)
		{
			return Sleepable.hatWorkPstAnim;
		}
		return Sleepable.normalWorkPstAnim;
	}

	// Token: 0x0600475D RID: 18269 RVA: 0x001932A8 File Offset: 0x001914A8
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (component != null)
		{
			component.Play("working_pre", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("working_loop", KAnim.PlayMode.Loop, 1f, 0f);
		}
		base.Subscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
		if (this.operational != null)
		{
			this.operational.SetActive(true, false);
		}
		worker.Trigger(-1283701846, this);
		worker.GetComponent<Effects>().Add(this.effectName, false);
		this.isDoneSleeping = false;
	}

	// Token: 0x0600475E RID: 18270 RVA: 0x00193364 File Offset: 0x00191564
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (this.isDoneSleeping)
		{
			return Time.time > this.wakeTime;
		}
		if (this.Dreamable != null && !this.Dreamable.DreamIsDisturbed)
		{
			this.Dreamable.WorkTick(worker, dt);
		}
		if (worker.GetSMI<StaminaMonitor.Instance>().ShouldExitSleep())
		{
			this.isDoneSleeping = true;
			this.wakeTime = Time.time + UnityEngine.Random.value * 3f;
		}
		return false;
	}

	// Token: 0x0600475F RID: 18271 RVA: 0x001933DC File Offset: 0x001915DC
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		if (this.operational != null)
		{
			this.operational.SetActive(false, false);
		}
		base.Unsubscribe(worker.gameObject, -1142962013, new Action<object>(this.PlayPstAnim));
		if (worker != null)
		{
			Effects component = worker.GetComponent<Effects>();
			component.Remove(this.effectName);
			if (this.wakeEffects != null)
			{
				foreach (string effect_id in this.wakeEffects)
				{
					component.Add(effect_id, true);
				}
			}
			if (this.stretchOnWake && UnityEngine.Random.value < 0.33f)
			{
				new EmoteChore(worker.GetComponent<ChoreProvider>(), Db.Get().ChoreTypes.EmoteHighPriority, Db.Get().Emotes.Minion.MorningStretch, 1, null);
			}
			if (worker.GetAmounts().Get(Db.Get().Amounts.Stamina).value < worker.GetAmounts().Get(Db.Get().Amounts.Stamina).GetMax())
			{
				worker.Trigger(1338475637, this);
			}
		}
	}

	// Token: 0x06004760 RID: 18272 RVA: 0x00193528 File Offset: 0x00191728
	public override bool InstantlyFinish(Worker worker)
	{
		return false;
	}

	// Token: 0x06004761 RID: 18273 RVA: 0x0019352B File Offset: 0x0019172B
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		Components.Sleepables.Remove(this);
	}

	// Token: 0x06004762 RID: 18274 RVA: 0x00193540 File Offset: 0x00191740
	private void PlayPstAnim(object data)
	{
		Worker worker = (Worker)data;
		if (worker != null && worker.workable != null)
		{
			KAnimControllerBase component = worker.workable.gameObject.GetComponent<KAnimControllerBase>();
			if (component != null)
			{
				component.Play("working_pst", KAnim.PlayMode.Once, 1f, 0f);
			}
		}
	}

	// Token: 0x04002F40 RID: 12096
	private const float STRECH_CHANCE = 0.33f;

	// Token: 0x04002F41 RID: 12097
	[MyCmpGet]
	private Operational operational;

	// Token: 0x04002F42 RID: 12098
	public string effectName = "Sleep";

	// Token: 0x04002F43 RID: 12099
	public List<string> wakeEffects;

	// Token: 0x04002F44 RID: 12100
	public bool stretchOnWake = true;

	// Token: 0x04002F45 RID: 12101
	private float wakeTime;

	// Token: 0x04002F46 RID: 12102
	private bool isDoneSleeping;

	// Token: 0x04002F47 RID: 12103
	public ClinicDreamable Dreamable;

	// Token: 0x04002F48 RID: 12104
	private static readonly HashedString[] normalWorkAnims = new HashedString[]
	{
		"working_pre",
		"working_loop"
	};

	// Token: 0x04002F49 RID: 12105
	private static readonly HashedString[] hatWorkAnims = new HashedString[]
	{
		"hat_pre",
		"working_loop"
	};

	// Token: 0x04002F4A RID: 12106
	private static readonly HashedString[] normalWorkPstAnim = new HashedString[]
	{
		"working_pst"
	};

	// Token: 0x04002F4B RID: 12107
	private static readonly HashedString[] hatWorkPstAnim = new HashedString[]
	{
		"hat_pst"
	};
}
