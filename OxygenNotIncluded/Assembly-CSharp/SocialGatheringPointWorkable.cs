using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x0200097D RID: 2429
[AddComponentMenu("KMonoBehaviour/Workable/SocialGatheringPointWorkable")]
public class SocialGatheringPointWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x0600478E RID: 18318 RVA: 0x00193D49 File Offset: 0x00191F49
	private SocialGatheringPointWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x0600478F RID: 18319 RVA: 0x00193D5C File Offset: 0x00191F5C
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_generic_convo_kanim")
		};
		this.workAnims = new HashedString[]
		{
			"idle"
		};
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
		this.synchronizeAnims = false;
		this.showProgressBar = false;
		this.resetProgressOnStop = true;
		this.lightEfficiencyBonus = false;
	}

	// Token: 0x06004790 RID: 18320 RVA: 0x00193DE2 File Offset: 0x00191FE2
	public override Vector3 GetFacingTarget()
	{
		if (this.lastTalker != null)
		{
			return this.lastTalker.transform.GetPosition();
		}
		return base.GetFacingTarget();
	}

	// Token: 0x06004791 RID: 18321 RVA: 0x00193E0C File Offset: 0x0019200C
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		if (!worker.GetComponent<Schedulable>().IsAllowed(Db.Get().ScheduleBlockTypes.Recreation))
		{
			Effects component = worker.GetComponent<Effects>();
			if (string.IsNullOrEmpty(this.specificEffect) || component.HasEffect(this.specificEffect))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06004792 RID: 18322 RVA: 0x00193E5C File Offset: 0x0019205C
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		worker.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Subscribe(25860745, new Action<object>(this.OnStoppedTalking));
		this.timesConversed = 0;
	}

	// Token: 0x06004793 RID: 18323 RVA: 0x00193EB8 File Offset: 0x001920B8
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		worker.Unsubscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Unsubscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x06004794 RID: 18324 RVA: 0x00193F0C File Offset: 0x0019210C
	protected override void OnCompleteWork(Worker worker)
	{
		if (this.timesConversed > 0)
		{
			Effects component = worker.GetComponent<Effects>();
			if (!string.IsNullOrEmpty(this.specificEffect))
			{
				component.Add(this.specificEffect, true);
			}
		}
	}

	// Token: 0x06004795 RID: 18325 RVA: 0x00193F44 File Offset: 0x00192144
	private void OnStartedTalking(object data)
	{
		ConversationManager.StartedTalkingEvent startedTalkingEvent = data as ConversationManager.StartedTalkingEvent;
		if (startedTalkingEvent == null)
		{
			return;
		}
		GameObject talker = startedTalkingEvent.talker;
		if (talker == base.worker.gameObject)
		{
			KBatchedAnimController component = base.worker.GetComponent<KBatchedAnimController>();
			string text = startedTalkingEvent.anim;
			text += UnityEngine.Random.Range(1, 9).ToString();
			component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
			component.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
		}
		else
		{
			base.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
			this.lastTalker = talker;
		}
		this.timesConversed++;
	}

	// Token: 0x06004796 RID: 18326 RVA: 0x00194002 File Offset: 0x00192202
	private void OnStoppedTalking(object data)
	{
	}

	// Token: 0x06004797 RID: 18327 RVA: 0x00194004 File Offset: 0x00192204
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04002F68 RID: 12136
	private GameObject lastTalker;

	// Token: 0x04002F69 RID: 12137
	public int basePriority;

	// Token: 0x04002F6A RID: 12138
	public string specificEffect;

	// Token: 0x04002F6B RID: 12139
	public int timesConversed;
}
