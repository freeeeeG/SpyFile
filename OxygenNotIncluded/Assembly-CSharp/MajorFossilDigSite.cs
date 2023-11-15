using System;
using STRINGS;
using UnityEngine;

// Token: 0x0200023A RID: 570
public class MajorFossilDigSite : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>
{
	// Token: 0x06000B6E RID: 2926 RVA: 0x000405B8 File Offset: 0x0003E7B8
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Idle.PlayAnim("covered").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOffLight)).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.MarkedForDig, this.Workable, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsTrue);
		this.Workable.PlayAnim("covered").Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).DefaultState(this.Workable.NonOperational).ParamTransition<bool>(this.MarkedForDig, this.Idle, GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.IsFalse);
		this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational, false);
		this.Workable.Operational.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((MajorFossilDigSite.Instance smi) => smi.GetWorkable(), this.WaitingForQuestCompletion);
		this.WaitingForQuestCompletion.OnSignal(this.CompleteStorySignal, this.Completed).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).PlayAnim("reveal").Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.Reveal)).ScheduleActionNextFrame("Refresh UI", new Action<MajorFossilDigSite.Instance>(MajorFossilDigSite.RefreshUI)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CheckForQuestCompletion)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.ProgressStoryTrait));
		this.Completed.Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.TurnOnLight)).Enter(delegate(MajorFossilDigSite.Instance smi)
		{
			MajorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.CompleteStory)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.UnlockFossilMine)).Enter(new StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State.Callback(MajorFossilDigSite.MakeItDemolishable));
	}

	// Token: 0x06000B6F RID: 2927 RVA: 0x00040867 File Offset: 0x0003EA67
	public static void MakeItDemolishable(MajorFossilDigSite.Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	// Token: 0x06000B70 RID: 2928 RVA: 0x0004087C File Offset: 0x0003EA7C
	public static void ProgressStoryTrait(MajorFossilDigSite.Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData data = new Quest.ItemData
			{
				CriteriaId = smi.def.questCriteria,
				CurrentValue = 1f
			};
			bool flag;
			bool flag2;
			instance.TrackProgress(data, out flag, out flag2);
		}
	}

	// Token: 0x06000B71 RID: 2929 RVA: 0x000408DB File Offset: 0x0003EADB
	public static void TurnOnLight(MajorFossilDigSite.Instance smi)
	{
		smi.SetLightOnState(true);
	}

	// Token: 0x06000B72 RID: 2930 RVA: 0x000408E4 File Offset: 0x0003EAE4
	public static void TurnOffLight(MajorFossilDigSite.Instance smi)
	{
		smi.SetLightOnState(false);
	}

	// Token: 0x06000B73 RID: 2931 RVA: 0x000408F0 File Offset: 0x0003EAF0
	public static void CheckForQuestCompletion(MajorFossilDigSite.Instance smi)
	{
		QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (questInstance != null && questInstance.CurrentState == Quest.State.Completed)
		{
			smi.OnQuestCompleted(questInstance);
		}
	}

	// Token: 0x06000B74 RID: 2932 RVA: 0x0004092A File Offset: 0x0003EB2A
	public static void SetEntombedStatusItemVisibility(MajorFossilDigSite.Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	// Token: 0x06000B75 RID: 2933 RVA: 0x00040933 File Offset: 0x0003EB33
	public static void UnlockFossilMine(MajorFossilDigSite.Instance smi)
	{
		smi.UnlockFossilMine();
	}

	// Token: 0x06000B76 RID: 2934 RVA: 0x0004093B File Offset: 0x0003EB3B
	public static void DestroyUIExcavateButton(MajorFossilDigSite.Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	// Token: 0x06000B77 RID: 2935 RVA: 0x00040943 File Offset: 0x0003EB43
	public static void CompleteStory(MajorFossilDigSite.Instance smi)
	{
		if (smi.sm.IsQuestCompleted.Get(smi))
		{
			return;
		}
		smi.sm.IsQuestCompleted.Set(true, smi, false);
		smi.CompleteStoryTrait();
	}

	// Token: 0x06000B78 RID: 2936 RVA: 0x00040974 File Offset: 0x0003EB74
	public static void Reveal(MajorFossilDigSite.Instance smi)
	{
		bool flag = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(true, smi, false);
		if (flag)
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null && !instance.IsComplete)
			{
				smi.ShowCompletionNotification();
			}
		}
	}

	// Token: 0x06000B79 RID: 2937 RVA: 0x000409D6 File Offset: 0x0003EBD6
	public static void RevealMinorDigSites(MajorFossilDigSite.Instance smi)
	{
		smi.RevealMinorDigSites();
	}

	// Token: 0x06000B7A RID: 2938 RVA: 0x000409DE File Offset: 0x0003EBDE
	public static void RefreshUI(MajorFossilDigSite.Instance smi)
	{
		smi.RefreshUI();
	}

	// Token: 0x06000B7B RID: 2939 RVA: 0x000409E6 File Offset: 0x0003EBE6
	public static void StartWorkChore(MajorFossilDigSite.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06000B7C RID: 2940 RVA: 0x000409EE File Offset: 0x0003EBEE
	public static void CancelWorkChore(MajorFossilDigSite.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x040006AF RID: 1711
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Idle;

	// Token: 0x040006B0 RID: 1712
	public MajorFossilDigSite.ReadyToBeWorked Workable;

	// Token: 0x040006B1 RID: 1713
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State WaitingForQuestCompletion;

	// Token: 0x040006B2 RID: 1714
	public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Completed;

	// Token: 0x040006B3 RID: 1715
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter MarkedForDig;

	// Token: 0x040006B4 RID: 1716
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsRevealed;

	// Token: 0x040006B5 RID: 1717
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.BoolParameter IsQuestCompleted;

	// Token: 0x040006B6 RID: 1718
	public StateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.Signal CompleteStorySignal;

	// Token: 0x040006B7 RID: 1719
	public const string ANIM_COVERED_NAME = "covered";

	// Token: 0x040006B8 RID: 1720
	public const string ANIM_REVEALED_NAME = "reveal";

	// Token: 0x02000F5A RID: 3930
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040055A8 RID: 21928
		public HashedString questCriteria;
	}

	// Token: 0x02000F5B RID: 3931
	public class ReadyToBeWorked : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State
	{
		// Token: 0x040055A9 RID: 21929
		public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State Operational;

		// Token: 0x040055AA RID: 21930
		public GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.State NonOperational;
	}

	// Token: 0x02000F5C RID: 3932
	public new class Instance : GameStateMachine<MajorFossilDigSite, MajorFossilDigSite.Instance, IStateMachineTarget, MajorFossilDigSite.Def>.GameInstance, ICheckboxListGroupControl
	{
		// Token: 0x060071A9 RID: 29097 RVA: 0x002BDC4C File Offset: 0x002BBE4C
		public Instance(IStateMachineTarget master, MajorFossilDigSite.Def def) : base(master, def)
		{
			Components.MajorFossilDigSites.Add(this);
		}

		// Token: 0x060071AA RID: 29098 RVA: 0x002BDC64 File Offset: 0x002BBE64
		public override void StartSM()
		{
			this.entombComponent.SetStatusItem(Db.Get().BuildingStatusItems.FossilEntombed);
			this.storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			base.GetComponent<KPrefabID>();
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			if (!base.sm.IsRevealed.Get(this))
			{
				this.CreateExcavateButton();
			}
			this.fossilMine.SetActiveState(base.sm.IsQuestCompleted.Get(this));
			if (base.sm.IsQuestCompleted.Get(this))
			{
				this.UnlockStandarBuildingButtons();
				this.ScheduleNextFrame(delegate(object obj)
				{
					this.ChangeUIDescriptionToCompleted();
				}, null);
			}
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			this.RefreshUI();
		}

		// Token: 0x060071AB RID: 29099 RVA: 0x002BDD68 File Offset: 0x002BBF68
		public void SetLightOnState(bool isOn)
		{
			FossilDigsiteLampLight component = base.gameObject.GetComponent<FossilDigsiteLampLight>();
			component.SetIndependentState(isOn, true);
			if (!isOn)
			{
				component.enabled = false;
			}
		}

		// Token: 0x060071AC RID: 29100 RVA: 0x002BDD93 File Offset: 0x002BBF93
		public Workable GetWorkable()
		{
			return this.excavateWorkable;
		}

		// Token: 0x060071AD RID: 29101 RVA: 0x002BDD9C File Offset: 0x002BBF9C
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<MajorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, this.excavateWorkable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x060071AE RID: 29102 RVA: 0x002BDDE2 File Offset: 0x002BBFE2
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("MajorFossilDigsite.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x060071AF RID: 29103 RVA: 0x002BDE03 File Offset: 0x002BC003
		public void SetEntombStatusItemVisibility(bool visible)
		{
			this.entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		// Token: 0x060071B0 RID: 29104 RVA: 0x002BDE14 File Offset: 0x002BC014
		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this, false);
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
		}

		// Token: 0x060071B1 RID: 29105 RVA: 0x002BDE64 File Offset: 0x002BC064
		public ExcavateButton CreateExcavateButton()
		{
			if (this.excavateButton == null)
			{
				this.excavateButton = base.gameObject.AddComponent<ExcavateButton>();
				ExcavateButton excavateButton = this.excavateButton;
				excavateButton.OnButtonPressed = (System.Action)Delegate.Combine(excavateButton.OnButtonPressed, new System.Action(this.OnExcavateButtonPressed));
				this.excavateButton.isMarkedForDig = (() => base.sm.MarkedForDig.Get(this));
			}
			return this.excavateButton;
		}

		// Token: 0x060071B2 RID: 29106 RVA: 0x002BDED4 File Offset: 0x002BC0D4
		public void DestroyExcavateButton()
		{
			this.excavateWorkable.SetShouldShowSkillPerkStatusItem(false);
			if (this.excavateButton != null)
			{
				UnityEngine.Object.DestroyImmediate(this.excavateButton);
				this.excavateButton = null;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x060071B3 RID: 29107 RVA: 0x002BDF02 File Offset: 0x002BC102
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x170007E4 RID: 2020
		// (get) Token: 0x060071B4 RID: 29108 RVA: 0x002BDF0E File Offset: 0x002BC10E
		public string Description
		{
			get
			{
				if (base.sm.IsRevealed.Get(this))
				{
					return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_REVEALED;
				}
				return CODEX.STORY_TRAITS.FOSSILHUNT.DESCRIPTION_BUILDINGMENU_COVERED;
			}
		}

		// Token: 0x060071B5 RID: 29109 RVA: 0x002BDF38 File Offset: 0x002BC138
		public bool SidescreenEnabled()
		{
			return !base.sm.IsQuestCompleted.Get(this);
		}

		// Token: 0x060071B6 RID: 29110 RVA: 0x002BDF4E File Offset: 0x002BC14E
		public void RevealMinorDigSites()
		{
			if (this.storyInitializer == null)
			{
				this.storyInitializer = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			}
			if (this.storyInitializer != null)
			{
				this.storyInitializer.RevealMinorFossilDigSites();
			}
		}

		// Token: 0x060071B7 RID: 29111 RVA: 0x002BDF7C File Offset: 0x002BC17C
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State previousState, float progressIncreased)
		{
			if (quest.CurrentState == Quest.State.Completed && base.sm.IsRevealed.Get(this))
			{
				this.OnQuestCompleted(quest);
			}
			this.RefreshUI();
		}

		// Token: 0x060071B8 RID: 29112 RVA: 0x002BDFA7 File Offset: 0x002BC1A7
		public void OnQuestCompleted(QuestInstance quest)
		{
			base.sm.CompleteStorySignal.Trigger(this);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
		}

		// Token: 0x060071B9 RID: 29113 RVA: 0x002BDFDC File Offset: 0x002BC1DC
		public void CompleteStoryTrait()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			smi.sm.CompleteStory.Trigger(smi);
		}

		// Token: 0x060071BA RID: 29114 RVA: 0x002BE006 File Offset: 0x002BC206
		public void UnlockFossilMine()
		{
			this.fossilMine.SetActiveState(true);
			this.UnlockStandarBuildingButtons();
			this.ChangeUIDescriptionToCompleted();
		}

		// Token: 0x060071BB RID: 29115 RVA: 0x002BE020 File Offset: 0x002BC220
		private void ChangeUIDescriptionToCompleted()
		{
			BuildingComplete component = base.gameObject.GetComponent<BuildingComplete>();
			base.gameObject.GetComponent<KSelectable>().SetName(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.NAME);
			component.SetDescriptionFlavour(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.EFFECT);
			component.SetDescription(BUILDINGS.PREFABS.FOSSILDIG_COMPLETED.DESC);
		}

		// Token: 0x060071BC RID: 29116 RVA: 0x002BE071 File Offset: 0x002BC271
		private void UnlockStandarBuildingButtons()
		{
			base.gameObject.AddOrGet<BuildingEnabledButton>();
		}

		// Token: 0x060071BD RID: 29117 RVA: 0x002BE07F File Offset: 0x002BC27F
		public void RefreshUI()
		{
			base.gameObject.Trigger(1980521255, null);
		}

		// Token: 0x060071BE RID: 29118 RVA: 0x002BE094 File Offset: 0x002BC294
		protected override void OnCleanUp()
		{
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				QuestInstance questInstance = instance;
				questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			}
			Components.MajorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x060071BF RID: 29119 RVA: 0x002BE0F1 File Offset: 0x002BC2F1
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x060071C0 RID: 29120 RVA: 0x002BE0F5 File Offset: 0x002BC2F5
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		// Token: 0x060071C1 RID: 29121 RVA: 0x002BE0FC File Offset: 0x002BC2FC
		public void ShowCompletionNotification()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			if (smi != null)
			{
				smi.ShowObjectiveCompletedNotification();
			}
		}

		// Token: 0x040055AB RID: 21931
		[MyCmpGet]
		private Operational operational;

		// Token: 0x040055AC RID: 21932
		[MyCmpGet]
		private MajorDigSiteWorkable excavateWorkable;

		// Token: 0x040055AD RID: 21933
		[MyCmpGet]
		private FossilMine fossilMine;

		// Token: 0x040055AE RID: 21934
		[MyCmpGet]
		private EntombVulnerable entombComponent;

		// Token: 0x040055AF RID: 21935
		private Chore chore;

		// Token: 0x040055B0 RID: 21936
		private FossilHuntInitializer.Instance storyInitializer;

		// Token: 0x040055B1 RID: 21937
		private ExcavateButton excavateButton;
	}
}
