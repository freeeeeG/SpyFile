using System;
using Klei.AI;
using UnityEngine;

// Token: 0x0200053E RID: 1342
[AddComponentMenu("KMonoBehaviour/scripts/Worker")]
public class Worker : KMonoBehaviour
{
	// Token: 0x17000161 RID: 353
	// (get) Token: 0x0600206A RID: 8298 RVA: 0x000ADD4C File Offset: 0x000ABF4C
	// (set) Token: 0x0600206B RID: 8299 RVA: 0x000ADD54 File Offset: 0x000ABF54
	public Worker.State state { get; private set; }

	// Token: 0x17000162 RID: 354
	// (get) Token: 0x0600206C RID: 8300 RVA: 0x000ADD5D File Offset: 0x000ABF5D
	// (set) Token: 0x0600206D RID: 8301 RVA: 0x000ADD65 File Offset: 0x000ABF65
	public Worker.StartWorkInfo startWorkInfo { get; private set; }

	// Token: 0x17000163 RID: 355
	// (get) Token: 0x0600206E RID: 8302 RVA: 0x000ADD6E File Offset: 0x000ABF6E
	public Workable workable
	{
		get
		{
			if (this.startWorkInfo != null)
			{
				return this.startWorkInfo.workable;
			}
			return null;
		}
	}

	// Token: 0x0600206F RID: 8303 RVA: 0x000ADD85 File Offset: 0x000ABF85
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		this.state = Worker.State.Idle;
		base.Subscribe<Worker>(1485595942, Worker.OnChoreInterruptDelegate);
	}

	// Token: 0x06002070 RID: 8304 RVA: 0x000ADDA5 File Offset: 0x000ABFA5
	private string GetWorkableDebugString()
	{
		if (this.workable == null)
		{
			return "Null";
		}
		return this.workable.name;
	}

	// Token: 0x06002071 RID: 8305 RVA: 0x000ADDC8 File Offset: 0x000ABFC8
	public void CompleteWork()
	{
		this.successFullyCompleted = false;
		this.state = Worker.State.Idle;
		if (this.workable != null)
		{
			if (this.workable.triggerWorkReactions && this.workable.GetWorkTime() > 30f)
			{
				string conversationTopic = this.workable.GetConversationTopic();
				if (!conversationTopic.IsNullOrWhiteSpace())
				{
					this.CreateCompletionReactable(conversationTopic);
				}
			}
			this.DetachAnimOverrides();
			this.workable.CompleteWork(this);
			if (this.workable.worker != null && !(this.workable is Constructable) && !(this.workable is Deconstructable) && !(this.workable is Repairable) && !(this.workable is Disinfectable))
			{
				BonusEvent.GameplayEventData gameplayEventData = new BonusEvent.GameplayEventData();
				gameplayEventData.workable = this.workable;
				gameplayEventData.worker = this.workable.worker;
				gameplayEventData.building = this.workable.GetComponent<BuildingComplete>();
				gameplayEventData.eventTrigger = GameHashes.UseBuilding;
				GameplayEventManager.Instance.Trigger(1175726587, gameplayEventData);
			}
		}
		this.InternalStopWork(this.workable, false);
	}

	// Token: 0x06002072 RID: 8306 RVA: 0x000ADEE8 File Offset: 0x000AC0E8
	public Worker.WorkResult Work(float dt)
	{
		if (this.state == Worker.State.PendingCompletion)
		{
			bool flag = Time.time - this.workPendingCompletionTime > 10f;
			if (!base.GetComponent<KAnimControllerBase>().IsStopped() && !flag)
			{
				return Worker.WorkResult.InProgress;
			}
			Navigator component = base.GetComponent<Navigator>();
			if (component != null)
			{
				NavGrid.NavTypeData navTypeData = component.NavGrid.GetNavTypeData(component.CurrentNavType);
				if (navTypeData.idleAnim.IsValid)
				{
					base.GetComponent<KAnimControllerBase>().Play(navTypeData.idleAnim, KAnim.PlayMode.Once, 1f, 0f);
				}
			}
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				return Worker.WorkResult.Success;
			}
			this.StopWork();
			return Worker.WorkResult.Failed;
		}
		else
		{
			if (this.state != Worker.State.Completing)
			{
				if (this.workable != null)
				{
					if (this.facing)
					{
						if (this.workable.ShouldFaceTargetWhenWorking())
						{
							this.facing.Face(this.workable.GetFacingTarget());
						}
						else
						{
							Rotatable component2 = this.workable.GetComponent<Rotatable>();
							bool flag2 = component2 != null && component2.GetOrientation() == Orientation.FlipH;
							Vector3 vector = this.facing.transform.GetPosition();
							vector += (flag2 ? Vector3.left : Vector3.right);
							this.facing.Face(vector);
						}
					}
					if (dt > 0f && Game.Instance.FastWorkersModeActive)
					{
						dt = Mathf.Min(this.workable.WorkTimeRemaining + 0.01f, 5f);
					}
					Klei.AI.Attribute workAttribute = this.workable.GetWorkAttribute();
					AttributeLevels component3 = base.GetComponent<AttributeLevels>();
					if (workAttribute != null && workAttribute.IsTrainable && component3 != null)
					{
						float attributeExperienceMultiplier = this.workable.GetAttributeExperienceMultiplier();
						component3.AddExperience(workAttribute.Id, dt, attributeExperienceMultiplier);
					}
					string skillExperienceSkillGroup = this.workable.GetSkillExperienceSkillGroup();
					if (this.resume != null && skillExperienceSkillGroup != null)
					{
						float skillExperienceMultiplier = this.workable.GetSkillExperienceMultiplier();
						this.resume.AddExperienceWithAptitude(skillExperienceSkillGroup, dt, skillExperienceMultiplier);
					}
					float efficiencyMultiplier = this.workable.GetEfficiencyMultiplier(this);
					float dt2 = dt * efficiencyMultiplier * 1f;
					if (this.workable.WorkTick(this, dt2) && this.state == Worker.State.Working)
					{
						this.successFullyCompleted = true;
						this.StartPlayingPostAnim();
						this.workable.OnPendingCompleteWork(this);
					}
				}
				return Worker.WorkResult.InProgress;
			}
			if (this.successFullyCompleted)
			{
				this.CompleteWork();
				return Worker.WorkResult.Success;
			}
			this.StopWork();
			return Worker.WorkResult.Failed;
		}
	}

	// Token: 0x06002073 RID: 8307 RVA: 0x000AE150 File Offset: 0x000AC350
	private void StartPlayingPostAnim()
	{
		if (this.workable != null && !this.workable.alwaysShowProgressBar)
		{
			this.workable.ShowProgressBar(false);
		}
		base.GetComponent<KPrefabID>().AddTag(GameTags.PreventChoreInterruption, false);
		this.state = Worker.State.PendingCompletion;
		this.workPendingCompletionTime = Time.time;
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		HashedString[] workPstAnims = this.workable.GetWorkPstAnims(this, this.successFullyCompleted);
		if (this.smi == null)
		{
			if (workPstAnims != null && workPstAnims.Length != 0)
			{
				if (this.workable != null && this.workable.synchronizeAnims)
				{
					KAnimControllerBase component2 = this.workable.GetComponent<KAnimControllerBase>();
					if (component2 != null)
					{
						component2.Play(workPstAnims, KAnim.PlayMode.Once);
					}
				}
				else
				{
					component.Play(workPstAnims, KAnim.PlayMode.Once);
				}
			}
			else
			{
				this.state = Worker.State.Completing;
			}
		}
		base.Trigger(-1142962013, this);
	}

	// Token: 0x06002074 RID: 8308 RVA: 0x000AE228 File Offset: 0x000AC428
	private void InternalStopWork(Workable target_workable, bool is_aborted)
	{
		this.state = Worker.State.Idle;
		base.gameObject.RemoveTag(GameTags.PerformingWorkRequest);
		base.GetComponent<KAnimControllerBase>().Offset -= this.workAnimOffset;
		this.workAnimOffset = Vector3.zero;
		base.GetComponent<KPrefabID>().RemoveTag(GameTags.PreventChoreInterruption);
		this.DetachAnimOverrides();
		this.ClearPasserbyReactable();
		AnimEventHandler component = base.GetComponent<AnimEventHandler>();
		if (component)
		{
			component.ClearContext();
		}
		if (this.previousStatusItem.item != null)
		{
			base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, this.previousStatusItem.item, this.previousStatusItem.data);
		}
		if (target_workable != null)
		{
			target_workable.Unsubscribe(this.onWorkChoreDisabledHandle);
			target_workable.StopWork(this, is_aborted);
		}
		if (this.smi != null)
		{
			this.smi.StopSM("stopping work");
			this.smi = null;
		}
		Vector3 position = base.transform.GetPosition();
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Move);
		base.transform.SetPosition(position);
		this.startWorkInfo = null;
	}

	// Token: 0x06002075 RID: 8309 RVA: 0x000AE34C File Offset: 0x000AC54C
	private void OnChoreInterrupt(object data)
	{
		if (this.state == Worker.State.Working)
		{
			this.successFullyCompleted = false;
			this.StartPlayingPostAnim();
		}
	}

	// Token: 0x06002076 RID: 8310 RVA: 0x000AE364 File Offset: 0x000AC564
	private void OnWorkChoreDisabled(object data)
	{
		string text = data as string;
		ChoreConsumer component = base.GetComponent<ChoreConsumer>();
		if (component != null && component.choreDriver != null)
		{
			component.choreDriver.GetCurrentChore().Fail((text != null) ? text : "WorkChoreDisabled");
		}
	}

	// Token: 0x06002077 RID: 8311 RVA: 0x000AE3B4 File Offset: 0x000AC5B4
	public void StopWork()
	{
		if (this.state != Worker.State.PendingCompletion && this.state != Worker.State.Completing)
		{
			if (this.state == Worker.State.Working)
			{
				if (this.workable != null && this.workable.synchronizeAnims)
				{
					KBatchedAnimController component = this.workable.GetComponent<KBatchedAnimController>();
					if (component != null)
					{
						HashedString[] workPstAnims = this.workable.GetWorkPstAnims(this, false);
						if (workPstAnims != null && workPstAnims.Length != 0)
						{
							component.Play(workPstAnims, KAnim.PlayMode.Once);
							component.SetPositionPercent(1f);
						}
					}
				}
				this.InternalStopWork(this.workable, true);
			}
			return;
		}
		this.state = Worker.State.Idle;
		if (this.successFullyCompleted)
		{
			this.CompleteWork();
			return;
		}
		this.InternalStopWork(this.workable, true);
	}

	// Token: 0x06002078 RID: 8312 RVA: 0x000AE468 File Offset: 0x000AC668
	public void StartWork(Worker.StartWorkInfo start_work_info)
	{
		this.startWorkInfo = start_work_info;
		Game.Instance.StartedWork();
		if (this.state != Worker.State.Idle)
		{
			string text = "";
			if (this.workable != null)
			{
				text = this.workable.name;
			}
			global::Debug.LogError(string.Concat(new string[]
			{
				base.name,
				".",
				text,
				".state should be idle but instead it's:",
				this.state.ToString()
			}));
		}
		string name = this.workable.GetType().Name;
		try
		{
			base.gameObject.AddTag(GameTags.PerformingWorkRequest);
			this.state = Worker.State.Working;
			base.gameObject.Trigger(1568504979, this);
			if (this.workable != null)
			{
				this.animInfo = this.workable.GetAnim(this);
				if (this.animInfo.smi != null)
				{
					this.smi = this.animInfo.smi;
					this.smi.StartSM();
				}
				Vector3 position = base.transform.GetPosition();
				position.z = Grid.GetLayerZ(this.workable.workLayer);
				base.transform.SetPosition(position);
				KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
				if (this.animInfo.smi == null)
				{
					this.AttachOverrideAnims(component);
				}
				HashedString[] workAnims = this.workable.GetWorkAnims(this);
				KAnim.PlayMode workAnimPlayMode = this.workable.GetWorkAnimPlayMode();
				Vector3 workOffset = this.workable.GetWorkOffset();
				this.workAnimOffset = workOffset;
				component.Offset += workOffset;
				if (this.usesMultiTool && this.animInfo.smi == null && workAnims != null && workAnims.Length != 0 && this.resume != null)
				{
					if (this.workable.synchronizeAnims)
					{
						KAnimControllerBase component2 = this.workable.GetComponent<KAnimControllerBase>();
						if (component2 != null)
						{
							this.kanimSynchronizer = component2.GetSynchronizer();
							if (this.kanimSynchronizer != null)
							{
								this.kanimSynchronizer.Add(component);
							}
						}
						component2.Play(workAnims, workAnimPlayMode);
					}
					else
					{
						component.Play(workAnims, workAnimPlayMode);
					}
				}
			}
			this.workable.StartWork(this);
			if (this.workable == null)
			{
				global::Debug.LogWarning("Stopped work as soon as I started. This is usually a sign that a chore is open when it shouldn't be or that it's preconditions are wrong.");
			}
			else
			{
				this.onWorkChoreDisabledHandle = this.workable.Subscribe(2108245096, new Action<object>(this.OnWorkChoreDisabled));
				if (this.workable.triggerWorkReactions && this.workable.WorkTimeRemaining > 10f)
				{
					this.CreatePasserbyReactable();
				}
				KSelectable component3 = base.GetComponent<KSelectable>();
				this.previousStatusItem = component3.GetStatusItem(Db.Get().StatusItemCategories.Main);
				component3.SetStatusItem(Db.Get().StatusItemCategories.Main, this.workable.GetWorkerStatusItem(), this.workable);
			}
		}
		catch (Exception ex)
		{
			string str = "Exception in: Worker.StartWork(" + name + ")";
			DebugUtil.LogErrorArgs(this, new object[]
			{
				str + "\n" + ex.ToString()
			});
			throw;
		}
	}

	// Token: 0x06002079 RID: 8313 RVA: 0x000AE7A8 File Offset: 0x000AC9A8
	private void Update()
	{
		if (this.state == Worker.State.Working)
		{
			this.ForceSyncAnims();
		}
	}

	// Token: 0x0600207A RID: 8314 RVA: 0x000AE7B9 File Offset: 0x000AC9B9
	private void ForceSyncAnims()
	{
		if (Time.deltaTime > 0f && this.kanimSynchronizer != null)
		{
			this.kanimSynchronizer.SyncTime();
		}
	}

	// Token: 0x0600207B RID: 8315 RVA: 0x000AE7DA File Offset: 0x000AC9DA
	public bool InstantlyFinish()
	{
		return this.workable != null && this.workable.InstantlyFinish(this);
	}

	// Token: 0x0600207C RID: 8316 RVA: 0x000AE7F8 File Offset: 0x000AC9F8
	private void AttachOverrideAnims(KAnimControllerBase worker_controller)
	{
		if (this.animInfo.overrideAnims != null && this.animInfo.overrideAnims.Length != 0)
		{
			for (int i = 0; i < this.animInfo.overrideAnims.Length; i++)
			{
				worker_controller.AddAnimOverrides(this.animInfo.overrideAnims[i], 0f);
			}
		}
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x000AE850 File Offset: 0x000ACA50
	private void DetachAnimOverrides()
	{
		KAnimControllerBase component = base.GetComponent<KAnimControllerBase>();
		if (this.kanimSynchronizer != null)
		{
			this.kanimSynchronizer.Remove(component);
			this.kanimSynchronizer = null;
		}
		if (this.animInfo.overrideAnims != null)
		{
			for (int i = 0; i < this.animInfo.overrideAnims.Length; i++)
			{
				component.RemoveAnimOverrides(this.animInfo.overrideAnims[i]);
			}
			this.animInfo.overrideAnims = null;
		}
	}

	// Token: 0x0600207E RID: 8318 RVA: 0x000AE8C4 File Offset: 0x000ACAC4
	private void CreateCompletionReactable(string topic)
	{
		if (GameClock.Instance.GetTime() / 600f < 1f)
		{
			return;
		}
		EmoteReactable emoteReactable = OneshotReactableLocator.CreateOneshotReactable(base.gameObject, 3f, "WorkCompleteAcknowledgement", Db.Get().ChoreTypes.Emote, 9, 5, 100f);
		Emote clapCheer = Db.Get().Emotes.Minion.ClapCheer;
		emoteReactable.SetEmote(clapCheer);
		emoteReactable.RegisterEmoteStepCallbacks("clapcheer_pre", new Action<GameObject>(this.GetReactionEffect), null).RegisterEmoteStepCallbacks("clapcheer_pst", null, delegate(GameObject r)
		{
			r.Trigger(937885943, topic);
		});
		global::Tuple<Sprite, Color> uisprite = Def.GetUISprite(topic, "ui", true);
		if (uisprite != null)
		{
			Thought thought = new Thought("Completion_" + topic, null, uisprite.first, "mode_satisfaction", "conversation_short", "bubble_conversation", SpeechMonitor.PREFIX_HAPPY, "", true, 4f);
			emoteReactable.SetThought(thought);
		}
	}

	// Token: 0x0600207F RID: 8319 RVA: 0x000AE9DC File Offset: 0x000ACBDC
	public void CreatePasserbyReactable()
	{
		if (GameClock.Instance.GetTime() / 600f < 1f)
		{
			return;
		}
		if (this.passerbyReactable == null)
		{
			EmoteReactable emoteReactable = new EmoteReactable(base.gameObject, "WorkPasserbyAcknowledgement", Db.Get().ChoreTypes.Emote, 5, 5, 30f, 720f * TuningData<DupeGreetingManager.Tuning>.Get().greetingDelayMultiplier, float.PositiveInfinity, 0f);
			Emote thumbsUp = Db.Get().Emotes.Minion.ThumbsUp;
			emoteReactable.SetEmote(thumbsUp).SetThought(Db.Get().Thoughts.Encourage).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsOnFloor)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsFacingMe)).AddPrecondition(new Reactable.ReactablePrecondition(this.ReactorIsntPartying));
			emoteReactable.RegisterEmoteStepCallbacks("react", new Action<GameObject>(this.GetReactionEffect), null);
			this.passerbyReactable = emoteReactable;
		}
	}

	// Token: 0x06002080 RID: 8320 RVA: 0x000AEADB File Offset: 0x000ACCDB
	private void GetReactionEffect(GameObject reactor)
	{
		base.GetComponent<Effects>().Add("WorkEncouraged", true);
	}

	// Token: 0x06002081 RID: 8321 RVA: 0x000AEAEF File Offset: 0x000ACCEF
	private bool ReactorIsOnFloor(GameObject reactor, Navigator.ActiveTransition transition)
	{
		return transition.end == NavType.Floor;
	}

	// Token: 0x06002082 RID: 8322 RVA: 0x000AEAFC File Offset: 0x000ACCFC
	private bool ReactorIsFacingMe(GameObject reactor, Navigator.ActiveTransition transition)
	{
		Facing component = reactor.GetComponent<Facing>();
		return base.transform.GetPosition().x < reactor.transform.GetPosition().x == component.GetFacing();
	}

	// Token: 0x06002083 RID: 8323 RVA: 0x000AEB3C File Offset: 0x000ACD3C
	private bool ReactorIsntPartying(GameObject reactor, Navigator.ActiveTransition transition)
	{
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		return component.choreDriver.HasChore() && component.choreDriver.GetCurrentChore().choreType != Db.Get().ChoreTypes.Party;
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x000AEB83 File Offset: 0x000ACD83
	public void ClearPasserbyReactable()
	{
		if (this.passerbyReactable != null)
		{
			this.passerbyReactable.Cleanup();
			this.passerbyReactable = null;
		}
	}

	// Token: 0x0400121F RID: 4639
	private const float EARLIEST_REACT_TIME = 1f;

	// Token: 0x04001220 RID: 4640
	[MyCmpGet]
	private Facing facing;

	// Token: 0x04001221 RID: 4641
	[MyCmpGet]
	private MinionResume resume;

	// Token: 0x04001224 RID: 4644
	private float workPendingCompletionTime;

	// Token: 0x04001225 RID: 4645
	private int onWorkChoreDisabledHandle;

	// Token: 0x04001226 RID: 4646
	public object workCompleteData;

	// Token: 0x04001227 RID: 4647
	private Workable.AnimInfo animInfo;

	// Token: 0x04001228 RID: 4648
	private KAnimSynchronizer kanimSynchronizer;

	// Token: 0x04001229 RID: 4649
	private StatusItemGroup.Entry previousStatusItem;

	// Token: 0x0400122A RID: 4650
	private StateMachine.Instance smi;

	// Token: 0x0400122B RID: 4651
	private bool successFullyCompleted;

	// Token: 0x0400122C RID: 4652
	private Vector3 workAnimOffset = Vector3.zero;

	// Token: 0x0400122D RID: 4653
	public bool usesMultiTool = true;

	// Token: 0x0400122E RID: 4654
	private static readonly EventSystem.IntraObjectHandler<Worker> OnChoreInterruptDelegate = new EventSystem.IntraObjectHandler<Worker>(delegate(Worker component, object data)
	{
		component.OnChoreInterrupt(data);
	});

	// Token: 0x0400122F RID: 4655
	private Reactable passerbyReactable;

	// Token: 0x020011E0 RID: 4576
	public enum State
	{
		// Token: 0x04005DE4 RID: 24036
		Idle,
		// Token: 0x04005DE5 RID: 24037
		Working,
		// Token: 0x04005DE6 RID: 24038
		PendingCompletion,
		// Token: 0x04005DE7 RID: 24039
		Completing
	}

	// Token: 0x020011E1 RID: 4577
	public class StartWorkInfo
	{
		// Token: 0x17000845 RID: 2117
		// (get) Token: 0x06007B15 RID: 31509 RVA: 0x002DD5D0 File Offset: 0x002DB7D0
		// (set) Token: 0x06007B16 RID: 31510 RVA: 0x002DD5D8 File Offset: 0x002DB7D8
		public Workable workable { get; set; }

		// Token: 0x06007B17 RID: 31511 RVA: 0x002DD5E1 File Offset: 0x002DB7E1
		public StartWorkInfo(Workable workable)
		{
			this.workable = workable;
		}
	}

	// Token: 0x020011E2 RID: 4578
	public enum WorkResult
	{
		// Token: 0x04005DEA RID: 24042
		Success,
		// Token: 0x04005DEB RID: 24043
		InProgress,
		// Token: 0x04005DEC RID: 24044
		Failed
	}
}
