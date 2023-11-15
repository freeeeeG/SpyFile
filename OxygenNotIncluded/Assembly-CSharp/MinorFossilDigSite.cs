using System;
using Klei.AI;
using STRINGS;
using UnityEngine;

// Token: 0x02000259 RID: 601
public class MinorFossilDigSite : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>
{
	// Token: 0x06000C13 RID: 3091 RVA: 0x000444E4 File Offset: 0x000426E4
	public override void InitializeStates(out StateMachine.BaseState default_state)
	{
		default_state = this.Idle;
		base.serializable = StateMachine.SerializeType.ParamsOnly;
		this.Idle.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(true);
		}).PlayAnim("object_dirty").ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.IsRevealed, this.WaitingForQuestCompletion, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue).ParamTransition<bool>(this.MarkedForDig, this.Workable, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue);
		this.Workable.PlayAnim("object_dirty").Toggle("Activate Entombed Status Item If Required", delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}, delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, false);
		}).DefaultState(this.Workable.NonOperational).ParamTransition<bool>(this.MarkedForDig, this.Idle, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsFalse);
		this.Workable.NonOperational.TagTransition(GameTags.Operational, this.Workable.Operational, false);
		this.Workable.Operational.Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.StartWorkChore)).Exit(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.CancelWorkChore)).TagTransition(GameTags.Operational, this.Workable.NonOperational, true).WorkableCompleteTransition((MinorFossilDigSite.Instance smi) => smi.GetWorkable(), this.WaitingForQuestCompletion);
		this.WaitingForQuestCompletion.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.Reveal)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.ProgressStoryTrait)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.MakeItDemolishable)).PlayAnim("object").ParamTransition<bool>(this.IsQuestCompleted, this.Completed, GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.IsTrue);
		this.Completed.Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			smi.SetDecorState(false);
		}).Enter(delegate(MinorFossilDigSite.Instance smi)
		{
			MinorFossilDigSite.SetEntombedStatusItemVisibility(smi, true);
		}).PlayAnim("object").Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.DestroyUIExcavateButton)).Enter(new StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State.Callback(MinorFossilDigSite.MakeItDemolishable));
	}

	// Token: 0x06000C14 RID: 3092 RVA: 0x000447D8 File Offset: 0x000429D8
	public static void MakeItDemolishable(MinorFossilDigSite.Instance smi)
	{
		smi.gameObject.GetComponent<Demolishable>().allowDemolition = true;
	}

	// Token: 0x06000C15 RID: 3093 RVA: 0x000447EB File Offset: 0x000429EB
	public static void DestroyUIExcavateButton(MinorFossilDigSite.Instance smi)
	{
		smi.DestroyExcavateButton();
	}

	// Token: 0x06000C16 RID: 3094 RVA: 0x000447F3 File Offset: 0x000429F3
	public static void SetEntombedStatusItemVisibility(MinorFossilDigSite.Instance smi, bool val)
	{
		smi.SetEntombStatusItemVisibility(val);
	}

	// Token: 0x06000C17 RID: 3095 RVA: 0x000447FC File Offset: 0x000429FC
	public static void UnregisterFromComponents(MinorFossilDigSite.Instance smi)
	{
		Components.MinorFossilDigSites.Remove(smi);
	}

	// Token: 0x06000C18 RID: 3096 RVA: 0x00044809 File Offset: 0x00042A09
	public static void SelfDestroy(MinorFossilDigSite.Instance smi)
	{
		Util.KDestroyGameObject(smi.gameObject);
	}

	// Token: 0x06000C19 RID: 3097 RVA: 0x00044816 File Offset: 0x00042A16
	public static void StartWorkChore(MinorFossilDigSite.Instance smi)
	{
		smi.CreateWorkableChore();
	}

	// Token: 0x06000C1A RID: 3098 RVA: 0x0004481E File Offset: 0x00042A1E
	public static void CancelWorkChore(MinorFossilDigSite.Instance smi)
	{
		smi.CancelWorkChore();
	}

	// Token: 0x06000C1B RID: 3099 RVA: 0x00044826 File Offset: 0x00042A26
	public static void Reveal(MinorFossilDigSite.Instance smi)
	{
		bool flag = !smi.sm.IsRevealed.Get(smi);
		smi.sm.IsRevealed.Set(true, smi, false);
		if (flag)
		{
			smi.ShowCompletionNotification();
			MinorFossilDigSite.DropLoot(smi);
		}
	}

	// Token: 0x06000C1C RID: 3100 RVA: 0x00044860 File Offset: 0x00042A60
	public static void DropLoot(MinorFossilDigSite.Instance smi)
	{
		PrimaryElement component = smi.GetComponent<PrimaryElement>();
		int cell = Grid.PosToCell(smi.transform.GetPosition());
		Element element = ElementLoader.GetElement(component.Element.tag);
		if (element != null)
		{
			float num = component.Mass;
			int num2 = 0;
			while ((float)num2 < component.Mass / 400f)
			{
				float num3 = num;
				if (num > 400f)
				{
					num3 = 400f;
					num -= 400f;
				}
				int disease_count = (int)((float)component.DiseaseCount * (num3 / component.Mass));
				element.substance.SpawnResource(Grid.CellToPosCBC(cell, Grid.SceneLayer.Ore), num3, component.Temperature, component.DiseaseIdx, disease_count, false, false, false);
				num2++;
			}
		}
	}

	// Token: 0x06000C1D RID: 3101 RVA: 0x00044911 File Offset: 0x00042B11
	public static void ProgressStoryTrait(MinorFossilDigSite.Instance smi)
	{
		MinorFossilDigSite.ProgressQuest(smi);
	}

	// Token: 0x06000C1E RID: 3102 RVA: 0x0004491C File Offset: 0x00042B1C
	public static QuestInstance ProgressQuest(MinorFossilDigSite.Instance smi)
	{
		QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
		if (instance != null)
		{
			Quest.ItemData data = new Quest.ItemData
			{
				CriteriaId = smi.def.fossilQuestCriteriaID,
				CurrentValue = 1f
			};
			bool flag;
			bool flag2;
			instance.TrackProgress(data, out flag, out flag2);
			return instance;
		}
		return null;
	}

	// Token: 0x0400072B RID: 1835
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Idle;

	// Token: 0x0400072C RID: 1836
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Completed;

	// Token: 0x0400072D RID: 1837
	public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State WaitingForQuestCompletion;

	// Token: 0x0400072E RID: 1838
	public MinorFossilDigSite.ReadyToBeWorked Workable;

	// Token: 0x0400072F RID: 1839
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter MarkedForDig;

	// Token: 0x04000730 RID: 1840
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter IsRevealed;

	// Token: 0x04000731 RID: 1841
	public StateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.BoolParameter IsQuestCompleted;

	// Token: 0x04000732 RID: 1842
	private const string UNEXCAVATED_ANIM_NAME = "object_dirty";

	// Token: 0x04000733 RID: 1843
	private const string EXCAVATED_ANIM_NAME = "object";

	// Token: 0x02000F62 RID: 3938
	public class Def : StateMachine.BaseDef
	{
		// Token: 0x040055C2 RID: 21954
		public HashedString fossilQuestCriteriaID;
	}

	// Token: 0x02000F63 RID: 3939
	public class ReadyToBeWorked : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State
	{
		// Token: 0x040055C3 RID: 21955
		public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State Operational;

		// Token: 0x040055C4 RID: 21956
		public GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.State NonOperational;
	}

	// Token: 0x02000F64 RID: 3940
	public new class Instance : GameStateMachine<MinorFossilDigSite, MinorFossilDigSite.Instance, IStateMachineTarget, MinorFossilDigSite.Def>.GameInstance, ICheckboxListGroupControl
	{
		// Token: 0x060071DB RID: 29147 RVA: 0x002BE45C File Offset: 0x002BC65C
		public Instance(IStateMachineTarget master, MinorFossilDigSite.Def def) : base(master, def)
		{
			Components.MinorFossilDigSites.Add(this);
			this.negativeDecorModifier = new AttributeModifier(Db.Get().Attributes.Decor.Id, -1f, CODEX.STORY_TRAITS.FOSSILHUNT.MISC.DECREASE_DECOR_ATTRIBUTE, true, false, true);
		}

		// Token: 0x060071DC RID: 29148 RVA: 0x002BE4AD File Offset: 0x002BC6AD
		public void SetDecorState(bool isDusty)
		{
			if (isDusty)
			{
				base.gameObject.GetComponent<DecorProvider>().decor.Add(this.negativeDecorModifier);
				return;
			}
			base.gameObject.GetComponent<DecorProvider>().decor.Remove(this.negativeDecorModifier);
		}

		// Token: 0x060071DD RID: 29149 RVA: 0x002BE4EC File Offset: 0x002BC6EC
		public override void StartSM()
		{
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Combine(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStorytraitProgressChanged));
			}
			if (!base.sm.IsRevealed.Get(this))
			{
				this.CreateExcavateButton();
			}
			QuestInstance questInstance = QuestManager.InitializeQuest(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Combine(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			this.workable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
			base.StartSM();
			this.RefreshUI();
		}

		// Token: 0x060071DE RID: 29150 RVA: 0x002BE5B9 File Offset: 0x002BC7B9
		private void OnQuestProgressChanged(QuestInstance quest, Quest.State previousState, float progressIncreased)
		{
			if (quest.CurrentState == Quest.State.Completed && base.sm.IsRevealed.Get(this))
			{
				this.OnQuestCompleted(quest);
			}
			this.RefreshUI();
		}

		// Token: 0x060071DF RID: 29151 RVA: 0x002BE5E4 File Offset: 0x002BC7E4
		public void OnQuestCompleted(QuestInstance quest)
		{
			base.sm.IsQuestCompleted.Set(true, this, false);
			quest.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(quest.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
		}

		// Token: 0x060071E0 RID: 29152 RVA: 0x002BE61C File Offset: 0x002BC81C
		protected override void OnCleanUp()
		{
			MinorFossilDigSite.ProgressQuest(base.smi);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.FossilHunt.HashId);
			if (storyInstance != null)
			{
				StoryInstance storyInstance2 = storyInstance;
				storyInstance2.StoryStateChanged = (Action<StoryInstance.State>)Delegate.Remove(storyInstance2.StoryStateChanged, new Action<StoryInstance.State>(this.OnStorytraitProgressChanged));
			}
			QuestInstance instance = QuestManager.GetInstance(FossilDigSiteConfig.hashID, Db.Get().Quests.FossilHuntQuest);
			if (instance != null)
			{
				QuestInstance questInstance = instance;
				questInstance.QuestProgressChanged = (Action<QuestInstance, Quest.State, float>)Delegate.Remove(questInstance.QuestProgressChanged, new Action<QuestInstance, Quest.State, float>(this.OnQuestProgressChanged));
			}
			Components.MinorFossilDigSites.Remove(this);
			base.OnCleanUp();
		}

		// Token: 0x060071E1 RID: 29153 RVA: 0x002BE6C9 File Offset: 0x002BC8C9
		public void OnStorytraitProgressChanged(StoryInstance.State state)
		{
			if (state != StoryInstance.State.IN_PROGRESS)
			{
				return;
			}
			this.RefreshUI();
		}

		// Token: 0x060071E2 RID: 29154 RVA: 0x002BE6DA File Offset: 0x002BC8DA
		public void RefreshUI()
		{
			base.Trigger(1980521255, null);
		}

		// Token: 0x060071E3 RID: 29155 RVA: 0x002BE6E8 File Offset: 0x002BC8E8
		public Workable GetWorkable()
		{
			return this.workable;
		}

		// Token: 0x060071E4 RID: 29156 RVA: 0x002BE6F0 File Offset: 0x002BC8F0
		public void CreateWorkableChore()
		{
			if (this.chore == null)
			{
				this.chore = new WorkChore<MinorDigSiteWorkable>(Db.Get().ChoreTypes.ExcavateFossil, this.workable, null, true, null, null, null, true, null, false, false, null, false, true, true, PriorityScreen.PriorityClass.basic, 5, false, true);
			}
		}

		// Token: 0x060071E5 RID: 29157 RVA: 0x002BE736 File Offset: 0x002BC936
		public void CancelWorkChore()
		{
			if (this.chore != null)
			{
				this.chore.Cancel("MinorFossilDigsite.CancelChore");
				this.chore = null;
			}
		}

		// Token: 0x060071E6 RID: 29158 RVA: 0x002BE757 File Offset: 0x002BC957
		public void SetEntombStatusItemVisibility(bool visible)
		{
			this.entombComponent.SetShowStatusItemOnEntombed(visible);
		}

		// Token: 0x060071E7 RID: 29159 RVA: 0x002BE768 File Offset: 0x002BC968
		public void ShowCompletionNotification()
		{
			FossilHuntInitializer.Instance smi = base.gameObject.GetSMI<FossilHuntInitializer.Instance>();
			if (smi != null)
			{
				smi.ShowObjectiveCompletedNotification();
			}
		}

		// Token: 0x060071E8 RID: 29160 RVA: 0x002BE78C File Offset: 0x002BC98C
		public void OnExcavateButtonPressed()
		{
			base.sm.MarkedForDig.Set(!base.sm.MarkedForDig.Get(this), this, false);
			this.workable.SetShouldShowSkillPerkStatusItem(base.sm.MarkedForDig.Get(this));
		}

		// Token: 0x060071E9 RID: 29161 RVA: 0x002BE7DC File Offset: 0x002BC9DC
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

		// Token: 0x060071EA RID: 29162 RVA: 0x002BE84C File Offset: 0x002BCA4C
		public void DestroyExcavateButton()
		{
			this.workable.SetShouldShowSkillPerkStatusItem(false);
			if (this.excavateButton != null)
			{
				UnityEngine.Object.DestroyImmediate(this.excavateButton);
				this.excavateButton = null;
			}
		}

		// Token: 0x170007E5 RID: 2021
		// (get) Token: 0x060071EB RID: 29163 RVA: 0x002BE87A File Offset: 0x002BCA7A
		public string Title
		{
			get
			{
				return CODEX.STORY_TRAITS.FOSSILHUNT.NAME;
			}
		}

		// Token: 0x170007E6 RID: 2022
		// (get) Token: 0x060071EC RID: 29164 RVA: 0x002BE886 File Offset: 0x002BCA86
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

		// Token: 0x060071ED RID: 29165 RVA: 0x002BE8B0 File Offset: 0x002BCAB0
		public bool SidescreenEnabled()
		{
			return !base.sm.IsQuestCompleted.Get(this);
		}

		// Token: 0x060071EE RID: 29166 RVA: 0x002BE8C6 File Offset: 0x002BCAC6
		public ICheckboxListGroupControl.ListGroup[] GetData()
		{
			return FossilHuntInitializer.GetFossilHuntQuestData();
		}

		// Token: 0x060071EF RID: 29167 RVA: 0x002BE8CD File Offset: 0x002BCACD
		public int CheckboxSideScreenSortOrder()
		{
			return 20;
		}

		// Token: 0x040055C5 RID: 21957
		[MyCmpGet]
		private MinorDigSiteWorkable workable;

		// Token: 0x040055C6 RID: 21958
		[MyCmpGet]
		private EntombVulnerable entombComponent;

		// Token: 0x040055C7 RID: 21959
		private ExcavateButton excavateButton;

		// Token: 0x040055C8 RID: 21960
		private Chore chore;

		// Token: 0x040055C9 RID: 21961
		private AttributeModifier negativeDecorModifier;
	}
}
