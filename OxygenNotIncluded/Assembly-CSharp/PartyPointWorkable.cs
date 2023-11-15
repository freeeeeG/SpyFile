using System;
using Klei.AI;
using TUNING;
using UnityEngine;

// Token: 0x020008D5 RID: 2261
public class PartyPointWorkable : Workable, IWorkerPrioritizable
{
	// Token: 0x06004168 RID: 16744 RVA: 0x0016E2FE File Offset: 0x0016C4FE
	private PartyPointWorkable()
	{
		base.SetReportType(ReportManager.ReportType.PersonalTime);
	}

	// Token: 0x06004169 RID: 16745 RVA: 0x0016E310 File Offset: 0x0016C510
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.overrideAnims = new KAnimFile[]
		{
			Assets.GetAnim("anim_generic_convo_kanim")
		};
		this.workAnimPlayMode = KAnim.PlayMode.Loop;
		this.faceTargetWhenWorking = true;
		this.workerStatusItem = Db.Get().DuplicantStatusItems.Socializing;
		this.synchronizeAnims = false;
		this.showProgressBar = false;
		this.resetProgressOnStop = true;
		this.lightEfficiencyBonus = false;
		if (UnityEngine.Random.Range(0f, 100f) > 80f)
		{
			this.activity = PartyPointWorkable.ActivityType.Dance;
		}
		else
		{
			this.activity = PartyPointWorkable.ActivityType.Talk;
		}
		PartyPointWorkable.ActivityType activityType = this.activity;
		if (activityType == PartyPointWorkable.ActivityType.Talk)
		{
			this.workAnims = new HashedString[]
			{
				"idle"
			};
			this.workerOverrideAnims = new KAnimFile[][]
			{
				new KAnimFile[]
				{
					Assets.GetAnim("anim_generic_convo_kanim")
				}
			};
			return;
		}
		if (activityType != PartyPointWorkable.ActivityType.Dance)
		{
			return;
		}
		this.workAnims = new HashedString[]
		{
			"working_loop"
		};
		this.workerOverrideAnims = new KAnimFile[][]
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

	// Token: 0x0600416A RID: 16746 RVA: 0x0016E474 File Offset: 0x0016C674
	public override Workable.AnimInfo GetAnim(Worker worker)
	{
		int num = UnityEngine.Random.Range(0, this.workerOverrideAnims.Length);
		this.overrideAnims = this.workerOverrideAnims[num];
		return base.GetAnim(worker);
	}

	// Token: 0x0600416B RID: 16747 RVA: 0x0016E4A5 File Offset: 0x0016C6A5
	public override Vector3 GetFacingTarget()
	{
		if (this.lastTalker != null)
		{
			return this.lastTalker.transform.GetPosition();
		}
		return base.GetFacingTarget();
	}

	// Token: 0x0600416C RID: 16748 RVA: 0x0016E4CC File Offset: 0x0016C6CC
	protected override bool OnWorkTick(Worker worker, float dt)
	{
		return false;
	}

	// Token: 0x0600416D RID: 16749 RVA: 0x0016E4D0 File Offset: 0x0016C6D0
	protected override void OnStartWork(Worker worker)
	{
		base.OnStartWork(worker);
		worker.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		worker.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Subscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x0600416E RID: 16750 RVA: 0x0016E528 File Offset: 0x0016C728
	protected override void OnStopWork(Worker worker)
	{
		base.OnStopWork(worker);
		worker.GetComponent<KPrefabID>().RemoveTag(GameTags.AlwaysConverse);
		worker.Unsubscribe(-594200555, new Action<object>(this.OnStartedTalking));
		worker.Unsubscribe(25860745, new Action<object>(this.OnStoppedTalking));
	}

	// Token: 0x0600416F RID: 16751 RVA: 0x0016E57C File Offset: 0x0016C77C
	protected override void OnCompleteWork(Worker worker)
	{
		Effects component = worker.GetComponent<Effects>();
		if (!string.IsNullOrEmpty(this.specificEffect))
		{
			component.Add(this.specificEffect, true);
		}
	}

	// Token: 0x06004170 RID: 16752 RVA: 0x0016E5AC File Offset: 0x0016C7AC
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
			if (this.activity == PartyPointWorkable.ActivityType.Talk)
			{
				KBatchedAnimController component = base.worker.GetComponent<KBatchedAnimController>();
				string text = startedTalkingEvent.anim;
				text += UnityEngine.Random.Range(1, 9).ToString();
				component.Play(text, KAnim.PlayMode.Once, 1f, 0f);
				component.Queue("idle", KAnim.PlayMode.Loop, 1f, 0f);
				return;
			}
		}
		else
		{
			if (this.activity == PartyPointWorkable.ActivityType.Talk)
			{
				base.worker.GetComponent<Facing>().Face(talker.transform.GetPosition());
			}
			this.lastTalker = talker;
		}
	}

	// Token: 0x06004171 RID: 16753 RVA: 0x0016E66E File Offset: 0x0016C86E
	private void OnStoppedTalking(object data)
	{
	}

	// Token: 0x06004172 RID: 16754 RVA: 0x0016E670 File Offset: 0x0016C870
	public bool GetWorkerPriority(Worker worker, out int priority)
	{
		priority = this.basePriority;
		if (!string.IsNullOrEmpty(this.specificEffect) && worker.GetComponent<Effects>().HasEffect(this.specificEffect))
		{
			priority = RELAXATION.PRIORITY.RECENTLY_USED;
		}
		return true;
	}

	// Token: 0x04002A94 RID: 10900
	private GameObject lastTalker;

	// Token: 0x04002A95 RID: 10901
	public int basePriority;

	// Token: 0x04002A96 RID: 10902
	public string specificEffect;

	// Token: 0x04002A97 RID: 10903
	public KAnimFile[][] workerOverrideAnims;

	// Token: 0x04002A98 RID: 10904
	private PartyPointWorkable.ActivityType activity;

	// Token: 0x02001711 RID: 5905
	private enum ActivityType
	{
		// Token: 0x04006D9F RID: 28063
		Talk,
		// Token: 0x04006DA0 RID: 28064
		Dance,
		// Token: 0x04006DA1 RID: 28065
		LENGTH
	}
}
