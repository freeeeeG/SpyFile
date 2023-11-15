using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x0200064F RID: 1615
public class MegaBrainTank : StateMachineComponent<MegaBrainTank.StatesInstance>
{
	// Token: 0x06002A8F RID: 10895 RVA: 0x000E2E5B File Offset: 0x000E105B
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x06002A90 RID: 10896 RVA: 0x000E2E64 File Offset: 0x000E1064
	protected override void OnSpawn()
	{
		base.OnSpawn();
		StoryManager.Instance.ForceCreateStory(Db.Get().Stories.MegaBrainTank, base.gameObject.GetMyWorldId());
		base.smi.StartSM();
		base.Subscribe(-1503271301, new Action<object>(this.OnBuildingSelect));
		base.GetComponent<Activatable>().SetWorkTime(5f);
		base.smi.JournalDelivery.refillMass = 25f;
		base.smi.JournalDelivery.FillToCapacity = true;
	}

	// Token: 0x06002A91 RID: 10897 RVA: 0x000E2EF4 File Offset: 0x000E10F4
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		base.Unsubscribe(-1503271301);
	}

	// Token: 0x06002A92 RID: 10898 RVA: 0x000E2F08 File Offset: 0x000E1108
	private void OnBuildingSelect(object obj)
	{
		if (!(bool)obj)
		{
			return;
		}
		if (!this.introDisplayed)
		{
			this.introDisplayed = true;
			EventInfoScreen.ShowPopup(EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.NAME, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.BEGIN_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.CLOSE_BUTTON, "braintankdiscovered_kanim", EventInfoDataHelper.PopupType.BEGIN, null, null, new System.Action(this.DoInitialUnlock)));
		}
		base.smi.ShowEventCompleteUI(null);
	}

	// Token: 0x06002A93 RID: 10899 RVA: 0x000E2F76 File Offset: 0x000E1176
	private void DoInitialUnlock()
	{
		Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_initial", true);
	}

	// Token: 0x040018E2 RID: 6370
	[Serialize]
	private bool introDisplayed;

	// Token: 0x02001326 RID: 4902
	public class States : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank>
	{
		// Token: 0x06007FD4 RID: 32724 RVA: 0x002EE684 File Offset: 0x002EC884
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			default_state = this.root;
			this.root.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				if (!StoryManager.Instance.CheckState(StoryInstance.State.COMPLETE, Db.Get().Stories.MegaBrainTank))
				{
					smi.GoTo(this.common.dormant);
					return;
				}
				if (smi.IsHungry)
				{
					smi.GoTo(this.common.idle);
					return;
				}
				smi.GoTo(this.common.active);
			});
			this.common.Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.IncrementMeter(dt);
				if (smi.UnitsFromLastStore != 0)
				{
					smi.ShelveJournals(dt);
				}
				bool flag = smi.ElementConverter.HasEnoughMass(GameTags.Oxygen, true);
				smi.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainNotEnoughOxygen, !flag, null);
			}, UpdateRate.SIM_33ms, false);
			this.common.dormant.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.SetBonusActive(false);
				smi.ElementConverter.SetAllConsumedActive(false);
				smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, false);
				smi.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis, false);
				smi.master.GetComponent<Light2D>().enabled = false;
			}).Exit(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.ElementConverter.SetConsumedElementActive(DreamJournalConfig.ID, true);
				smi.ElementConverter.SetConsumedElementActive(GameTags.Oxygen, true);
				RequireInputs component = smi.GetComponent<RequireInputs>();
				component.requireConduitHasMass = true;
				component.visualizeRequirements = RequireInputs.Requirements.All;
			}).Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.ActivateBrains(dt);
			}, UpdateRate.SIM_33ms, false).OnSignal(this.storyTraitCompleted, this.common.active);
			this.common.idle.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.CleanTank(false);
			}).UpdateTransition(this.common.active, (MegaBrainTank.StatesInstance smi, float _) => !smi.IsHungry && smi.gameObject.GetComponent<Operational>().enabled, UpdateRate.SIM_1000ms, false);
			this.common.active.Enter(delegate(MegaBrainTank.StatesInstance smi)
			{
				smi.CleanTank(true);
			}).Update(delegate(MegaBrainTank.StatesInstance smi, float dt)
			{
				smi.Digest(dt);
			}, UpdateRate.SIM_33ms, false).UpdateTransition(this.common.idle, (MegaBrainTank.StatesInstance smi, float _) => smi.IsHungry || !smi.gameObject.GetComponent<Operational>().enabled, UpdateRate.SIM_1000ms, false);
			this.StatBonus = new Effect("MegaBrainTankBonus", DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.TOOLTIP, 0f, true, true, false, null, -1f, 0f, null, "");
			object[,] stat_BONUSES = MegaBrainTankConfig.STAT_BONUSES;
			int length = stat_BONUSES.GetLength(0);
			for (int i = 0; i < length; i++)
			{
				string attribute_id = stat_BONUSES[i, 0] as string;
				float? num = (float?)stat_BONUSES[i, 1];
				Units? units = (Units?)stat_BONUSES[i, 2];
				this.StatBonus.Add(new AttributeModifier(attribute_id, ModifierSet.ConvertValue(num.Value, units.Value), DUPLICANTS.MODIFIERS.MEGABRAINTANKBONUS.NAME, false, false, true));
			}
		}

		// Token: 0x04006189 RID: 24969
		public MegaBrainTank.States.CommonState common;

		// Token: 0x0400618A RID: 24970
		public StateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.Signal storyTraitCompleted;

		// Token: 0x0400618B RID: 24971
		public Effect StatBonus;

		// Token: 0x020020FA RID: 8442
		public class CommonState : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State
		{
			// Token: 0x040093AB RID: 37803
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State dormant;

			// Token: 0x040093AC RID: 37804
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State idle;

			// Token: 0x040093AD RID: 37805
			public GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.State active;
		}
	}

	// Token: 0x02001327 RID: 4903
	public class StatesInstance : GameStateMachine<MegaBrainTank.States, MegaBrainTank.StatesInstance, MegaBrainTank, object>.GameInstance
	{
		// Token: 0x17000870 RID: 2160
		// (get) Token: 0x06007FD7 RID: 32727 RVA: 0x002EE982 File Offset: 0x002ECB82
		public KBatchedAnimController BrainController
		{
			get
			{
				return this.controllers[0];
			}
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06007FD8 RID: 32728 RVA: 0x002EE98C File Offset: 0x002ECB8C
		public KBatchedAnimController ShelfController
		{
			get
			{
				return this.controllers[1];
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06007FD9 RID: 32729 RVA: 0x002EE996 File Offset: 0x002ECB96
		// (set) Token: 0x06007FDA RID: 32730 RVA: 0x002EE99E File Offset: 0x002ECB9E
		public Storage BrainStorage { get; private set; }

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06007FDB RID: 32731 RVA: 0x002EE9A7 File Offset: 0x002ECBA7
		// (set) Token: 0x06007FDC RID: 32732 RVA: 0x002EE9AF File Offset: 0x002ECBAF
		public KSelectable Selectable { get; private set; }

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x06007FDD RID: 32733 RVA: 0x002EE9B8 File Offset: 0x002ECBB8
		// (set) Token: 0x06007FDE RID: 32734 RVA: 0x002EE9C0 File Offset: 0x002ECBC0
		public Operational Operational { get; private set; }

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06007FDF RID: 32735 RVA: 0x002EE9C9 File Offset: 0x002ECBC9
		// (set) Token: 0x06007FE0 RID: 32736 RVA: 0x002EE9D1 File Offset: 0x002ECBD1
		public ElementConverter ElementConverter { get; private set; }

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06007FE1 RID: 32737 RVA: 0x002EE9DA File Offset: 0x002ECBDA
		// (set) Token: 0x06007FE2 RID: 32738 RVA: 0x002EE9E2 File Offset: 0x002ECBE2
		public ManualDeliveryKG JournalDelivery { get; private set; }

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06007FE3 RID: 32739 RVA: 0x002EE9EB File Offset: 0x002ECBEB
		// (set) Token: 0x06007FE4 RID: 32740 RVA: 0x002EE9F3 File Offset: 0x002ECBF3
		public LoopingSounds BrainSounds { get; private set; }

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06007FE5 RID: 32741 RVA: 0x002EE9FC File Offset: 0x002ECBFC
		public bool IsHungry
		{
			get
			{
				return !this.ElementConverter.HasEnoughMassToStartConverting(true);
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06007FE6 RID: 32742 RVA: 0x002EEA0D File Offset: 0x002ECC0D
		public int TimeTilDigested
		{
			get
			{
				return (int)this.timeTilDigested;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06007FE7 RID: 32743 RVA: 0x002EEA16 File Offset: 0x002ECC16
		public int ActivationProgress
		{
			get
			{
				return (int)(25f * this.meterFill);
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06007FE8 RID: 32744 RVA: 0x002EEA25 File Offset: 0x002ECC25
		public HashedString CurrentActivationAnim
		{
			get
			{
				return MegaBrainTankConfig.ACTIVATION_ANIMS[(int)(this.nextActiveBrain - 1)];
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06007FE9 RID: 32745 RVA: 0x002EEA3C File Offset: 0x002ECC3C
		private HashedString currentActivationLoop
		{
			get
			{
				int num = (int)(this.nextActiveBrain - 1 + 5);
				return MegaBrainTankConfig.ACTIVATION_ANIMS[num];
			}
		}

		// Token: 0x06007FEA RID: 32746 RVA: 0x002EEA60 File Offset: 0x002ECC60
		public StatesInstance(MegaBrainTank master) : base(master)
		{
			this.BrainSounds = base.GetComponent<LoopingSounds>();
			this.BrainStorage = base.GetComponent<Storage>();
			this.ElementConverter = base.GetComponent<ElementConverter>();
			this.JournalDelivery = base.GetComponent<ManualDeliveryKG>();
			this.Operational = base.GetComponent<Operational>();
			this.Selectable = base.GetComponent<KSelectable>();
			this.notifier = base.GetComponent<Notifier>();
			this.controllers = base.gameObject.GetComponentsInChildren<KBatchedAnimController>();
			this.meter = new MeterController(this.BrainController, "meter_oxygen_target", "meter", Meter.Offset.Infront, Grid.SceneLayer.NoLayer, MegaBrainTankConfig.METER_SYMBOLS);
			this.fxLink = new KAnimLink(this.BrainController, this.ShelfController);
		}

		// Token: 0x06007FEB RID: 32747 RVA: 0x002EEB28 File Offset: 0x002ECD28
		public override void StartSM()
		{
			this.InitializeEffectsList();
			base.StartSM();
			this.BrainController.onAnimComplete += this.OnAnimComplete;
			this.ShelfController.onAnimComplete += this.OnAnimComplete;
			Storage brainStorage = this.BrainStorage;
			brainStorage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Combine(brainStorage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnJournalDeliveryStateChanged));
			this.brainHum = GlobalAssets.GetSound("MegaBrainTank_brain_wave_LP", false);
			StoryManager.Instance.DiscoverStoryEvent(Db.Get().Stories.MegaBrainTank);
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			if (this.GetCurrentState() == base.sm.common.dormant)
			{
				this.meterFill = (this.targetProgress = unitsAvailable / 25f);
				this.meter.SetPositionPercent(this.meterFill);
				short num = (short)(5f * this.meterFill);
				if (num > 0)
				{
					this.nextActiveBrain = num;
					this.BrainSounds.StartSound(this.brainHum);
					this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)num);
					this.CompleteBrainActivation();
				}
				return;
			}
			this.timeTilDigested = unitsAvailable * 60f;
			this.meterFill = this.timeTilDigested - this.timeTilDigested % 0.04f;
			this.meterFill /= 1500f;
			this.meter.SetPositionPercent(this.meterFill);
			StoryManager.Instance.BeginStoryEvent(Db.Get().Stories.MegaBrainTank);
			this.nextActiveBrain = 5;
			this.CompleteBrainActivation();
		}

		// Token: 0x06007FEC RID: 32748 RVA: 0x002EECD0 File Offset: 0x002ECED0
		public override void StopSM(string reason)
		{
			this.BrainController.onAnimComplete -= this.OnAnimComplete;
			this.ShelfController.onAnimComplete -= this.OnAnimComplete;
			Storage brainStorage = this.BrainStorage;
			brainStorage.OnWorkableEventCB = (Action<Workable, Workable.WorkableEvent>)Delegate.Remove(brainStorage.OnWorkableEventCB, new Action<Workable, Workable.WorkableEvent>(this.OnJournalDeliveryStateChanged));
			base.StopSM(reason);
		}

		// Token: 0x06007FED RID: 32749 RVA: 0x002EED3C File Offset: 0x002ECF3C
		private void InitializeEffectsList()
		{
			Components.Cmps<MinionIdentity> liveMinionIdentities = Components.LiveMinionIdentities;
			liveMinionIdentities.OnAdd += this.OnLiveMinionIdAdded;
			liveMinionIdentities.OnRemove += this.OnLiveMinionIdRemoved;
			MegaBrainTank.StatesInstance.minionEffects = new List<Effects>((liveMinionIdentities.Count > 32) ? liveMinionIdentities.Count : 32);
			for (int i = 0; i < liveMinionIdentities.Count; i++)
			{
				this.OnLiveMinionIdAdded(liveMinionIdentities[i]);
			}
		}

		// Token: 0x06007FEE RID: 32750 RVA: 0x002EEDB0 File Offset: 0x002ECFB0
		private void OnLiveMinionIdAdded(MinionIdentity id)
		{
			Effects component = id.GetComponent<Effects>();
			MegaBrainTank.StatesInstance.minionEffects.Add(component);
			if (this.GetCurrentState() == base.sm.common.active)
			{
				component.Add(base.sm.StatBonus, false);
			}
		}

		// Token: 0x06007FEF RID: 32751 RVA: 0x002EEDFC File Offset: 0x002ECFFC
		private void OnLiveMinionIdRemoved(MinionIdentity id)
		{
			Effects component = id.GetComponent<Effects>();
			MegaBrainTank.StatesInstance.minionEffects.Remove(component);
		}

		// Token: 0x06007FF0 RID: 32752 RVA: 0x002EEE1C File Offset: 0x002ED01C
		public void SetBonusActive(bool active)
		{
			for (int i = 0; i < MegaBrainTank.StatesInstance.minionEffects.Count; i++)
			{
				if (active)
				{
					MegaBrainTank.StatesInstance.minionEffects[i].Add(base.sm.StatBonus, false);
				}
				else
				{
					MegaBrainTank.StatesInstance.minionEffects[i].Remove(base.sm.StatBonus);
				}
			}
		}

		// Token: 0x06007FF1 RID: 32753 RVA: 0x002EEE7C File Offset: 0x002ED07C
		private void OnAnimComplete(HashedString anim)
		{
			if (anim == MegaBrainTankConfig.KACHUNK)
			{
				this.StoreJournals();
				return;
			}
			if ((anim == base.smi.CurrentActivationAnim || anim == MegaBrainTankConfig.ACTIVATE_ALL) && this.GetCurrentState() != base.sm.common.idle)
			{
				this.CompleteBrainActivation();
			}
		}

		// Token: 0x06007FF2 RID: 32754 RVA: 0x002EEEDC File Offset: 0x002ED0DC
		private void OnJournalDeliveryStateChanged(Workable w, Workable.WorkableEvent state)
		{
			if (state == Workable.WorkableEvent.WorkStopped)
			{
				return;
			}
			if (state != Workable.WorkableEvent.WorkStarted)
			{
				this.ShelfController.Play(MegaBrainTankConfig.KACHUNK, KAnim.PlayMode.Once, 1f, 0f);
				return;
			}
			FetchAreaChore.StatesInstance smi = w.worker.GetSMI<FetchAreaChore.StatesInstance>();
			if (smi.IsNullOrStopped())
			{
				return;
			}
			GameObject gameObject = smi.sm.deliveryObject.Get(smi);
			if (gameObject == null)
			{
				return;
			}
			Pickupable component = gameObject.GetComponent<Pickupable>();
			this.UnitsFromLastStore = (short)component.PrimaryElement.Units;
			float num = Mathf.Clamp01(component.PrimaryElement.Units / 5f);
			this.BrainStorage.SetWorkTime(num * this.BrainStorage.storageWorkTime);
		}

		// Token: 0x06007FF3 RID: 32755 RVA: 0x002EEF88 File Offset: 0x002ED188
		public void ShelveJournals(float dt)
		{
			float num = this.lastRemainingTime - this.BrainStorage.WorkTimeRemaining;
			if (num <= 0f)
			{
				num = this.BrainStorage.storageWorkTime - this.BrainStorage.WorkTimeRemaining;
			}
			this.lastRemainingTime = this.BrainStorage.WorkTimeRemaining;
			if (this.BrainStorage.storageWorkTime / 5f - this.journalActivationTimer > 0.001f)
			{
				this.journalActivationTimer += num;
				return;
			}
			int num2 = -1;
			this.journalActivationTimer = 0f;
			for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
			{
				byte b = (byte)(1 << i);
				bool flag = (this.activatedJournals & b) == 0;
				if (flag && num2 == -1)
				{
					num2 = i;
				}
				if (flag & UnityEngine.Random.Range(0f, 1f) >= 0.5f)
				{
					num2 = -1;
					this.activatedJournals |= b;
					this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], true);
					break;
				}
			}
			if (num2 != -1)
			{
				this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[num2], true);
			}
			this.UnitsFromLastStore -= 1;
		}

		// Token: 0x06007FF4 RID: 32756 RVA: 0x002EF0BC File Offset: 0x002ED2BC
		public void StoreJournals()
		{
			this.lastRemainingTime = 0f;
			this.activatedJournals = 0;
			for (int i = 0; i < MegaBrainTankConfig.JOURNAL_SYMBOLS.Length; i++)
			{
				this.ShelfController.SetSymbolVisiblity(MegaBrainTankConfig.JOURNAL_SYMBOLS[i], false);
			}
			this.ShelfController.PlayMode = KAnim.PlayMode.Paused;
			this.ShelfController.SetPositionPercent(0f);
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.targetProgress = Mathf.Clamp01(unitsAvailable / 25f);
		}

		// Token: 0x06007FF5 RID: 32757 RVA: 0x002EF148 File Offset: 0x002ED348
		public void ActivateBrains(float dt)
		{
			if (this.currentlyActivating)
			{
				return;
			}
			this.currentlyActivating = ((float)this.nextActiveBrain / 5f - this.meterFill <= 0.001f);
			if (!this.currentlyActivating)
			{
				return;
			}
			this.BrainController.QueueAndSyncTransition(this.CurrentActivationAnim, KAnim.PlayMode.Once, 1f, 0f);
			if (this.nextActiveBrain > 0)
			{
				this.BrainSounds.StartSound(this.brainHum);
				this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)this.nextActiveBrain);
			}
		}

		// Token: 0x06007FF6 RID: 32758 RVA: 0x002EF1E4 File Offset: 0x002ED3E4
		public void CompleteBrainActivation()
		{
			this.BrainController.Play(this.currentActivationLoop, KAnim.PlayMode.Loop, 1f, 0f);
			this.nextActiveBrain += 1;
			this.currentlyActivating = false;
			if (this.nextActiveBrain > 5)
			{
				float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
				this.timeTilDigested = unitsAvailable * 60f;
				this.CompleteEvent();
			}
		}

		// Token: 0x06007FF7 RID: 32759 RVA: 0x002EF250 File Offset: 0x002ED450
		public void Digest(float dt)
		{
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.timeTilDigested = unitsAvailable * 60f;
			if (this.targetProgress - this.meterFill > Mathf.Epsilon)
			{
				return;
			}
			this.targetProgress = 0f;
			float num = this.meterFill - this.timeTilDigested / 1500f;
			if (num >= 0.04f)
			{
				this.meterFill -= num - num % 0.04f;
				this.meter.SetPositionPercent(this.meterFill);
			}
		}

		// Token: 0x06007FF8 RID: 32760 RVA: 0x002EF2E0 File Offset: 0x002ED4E0
		public void CleanTank(bool active)
		{
			this.SetBonusActive(active);
			base.GetComponent<Light2D>().enabled = active;
			this.Selectable.ToggleStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankDreamAnalysis, active, this);
			this.ElementConverter.SetAllConsumedActive(active);
			this.BrainController.ClearQueue();
			float unitsAvailable = this.BrainStorage.GetUnitsAvailable(DreamJournalConfig.ID);
			this.timeTilDigested = unitsAvailable * 60f;
			if (active)
			{
				this.nextActiveBrain = 5;
				this.BrainController.QueueAndSyncTransition(MegaBrainTankConfig.ACTIVATE_ALL, KAnim.PlayMode.Once, 1f, 0f);
				this.BrainSounds.StartSound(this.brainHum);
				this.BrainSounds.SetParameter(this.brainHum, "BrainTankProgress", (float)this.nextActiveBrain);
				return;
			}
			if (this.timeTilDigested < 0.016666668f)
			{
				this.BrainStorage.ConsumeAllIgnoringDisease(DreamJournalConfig.ID);
				this.timeTilDigested = 0f;
				this.meterFill = 0f;
				this.meter.SetPositionPercent(this.meterFill);
			}
			this.BrainController.QueueAndSyncTransition(MegaBrainTankConfig.DEACTIVATE_ALL, KAnim.PlayMode.Once, 1f, 0f);
			this.BrainSounds.StopSound(this.brainHum);
		}

		// Token: 0x06007FF9 RID: 32761 RVA: 0x002EF41C File Offset: 0x002ED61C
		public bool IncrementMeter(float dt)
		{
			if (this.targetProgress - this.meterFill <= Mathf.Epsilon)
			{
				return false;
			}
			this.meterFill += Mathf.Lerp(0f, 1f, 0.04f * dt);
			if (1f - this.meterFill <= 0.001f)
			{
				this.meterFill = 1f;
			}
			this.meter.SetPositionPercent(this.meterFill);
			return this.targetProgress - this.meterFill > 0.001f;
		}

		// Token: 0x06007FFA RID: 32762 RVA: 0x002EF4A8 File Offset: 0x002ED6A8
		public void CompleteEvent()
		{
			this.Selectable.RemoveStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankActivationProgress, false);
			this.Selectable.AddStatusItem(Db.Get().BuildingStatusItems.MegaBrainTankComplete, base.smi);
			StoryInstance storyInstance = StoryManager.Instance.GetStoryInstance(Db.Get().Stories.MegaBrainTank.HashId);
			if (storyInstance == null || storyInstance.CurrentState == StoryInstance.State.COMPLETE)
			{
				return;
			}
			this.eventInfo = EventInfoDataHelper.GenerateStoryTraitData(CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.NAME, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.DESCRIPTION, CODEX.STORY_TRAITS.MEGA_BRAIN_TANK.END_POPUP.BUTTON, "braintankcomplete_kanim", EventInfoDataHelper.PopupType.COMPLETE, null, null, null);
			base.smi.Selectable.AddStatusItem(Db.Get().MiscStatusItems.AttentionRequired, base.smi);
			this.eventComplete = EventInfoScreen.CreateNotification(this.eventInfo, new Notification.ClickCallback(this.ShowEventCompleteUI));
			this.notifier.Add(this.eventComplete, "");
		}

		// Token: 0x06007FFB RID: 32763 RVA: 0x002EF5AC File Offset: 0x002ED7AC
		public void ShowEventCompleteUI(object _ = null)
		{
			if (this.eventComplete == null)
			{
				return;
			}
			base.smi.Selectable.RemoveStatusItem(Db.Get().MiscStatusItems.AttentionRequired, false);
			this.notifier.Remove(this.eventComplete);
			this.eventComplete = null;
			Game.Instance.unlocks.Unlock("story_trait_mega_brain_tank_competed", true);
			Vector3 target = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), new CellOffset(0, 3)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, base.master, new FocusTargetSequence.Data
			{
				WorldId = base.master.GetMyWorldId(),
				OrthographicSize = 6f,
				TargetSize = 6f,
				Target = target,
				PopupData = this.eventInfo,
				CompleteCB = new System.Action(this.OnCompleteStorySequence),
				CanCompleteCB = null
			});
		}

		// Token: 0x06007FFC RID: 32764 RVA: 0x002EF6B4 File Offset: 0x002ED8B4
		private void OnCompleteStorySequence()
		{
			Vector3 keepsakeSpawnPosition = Grid.CellToPosCCC(Grid.OffsetCell(Grid.PosToCell(base.master), new CellOffset(0, 2)), Grid.SceneLayer.Ore);
			StoryManager.Instance.CompleteStoryEvent(Db.Get().Stories.MegaBrainTank, keepsakeSpawnPosition);
			this.eventInfo = null;
			base.sm.storyTraitCompleted.Trigger(this);
		}

		// Token: 0x0400618C RID: 24972
		private static List<Effects> minionEffects;

		// Token: 0x04006193 RID: 24979
		public short UnitsFromLastStore;

		// Token: 0x04006194 RID: 24980
		private float meterFill = 0.04f;

		// Token: 0x04006195 RID: 24981
		private float targetProgress;

		// Token: 0x04006196 RID: 24982
		private float timeTilDigested;

		// Token: 0x04006197 RID: 24983
		private float journalActivationTimer;

		// Token: 0x04006198 RID: 24984
		private float lastRemainingTime;

		// Token: 0x04006199 RID: 24985
		private byte activatedJournals;

		// Token: 0x0400619A RID: 24986
		private bool currentlyActivating;

		// Token: 0x0400619B RID: 24987
		private short nextActiveBrain = 1;

		// Token: 0x0400619C RID: 24988
		private string brainHum;

		// Token: 0x0400619D RID: 24989
		private KBatchedAnimController[] controllers;

		// Token: 0x0400619E RID: 24990
		private KAnimLink fxLink;

		// Token: 0x0400619F RID: 24991
		private MeterController meter;

		// Token: 0x040061A0 RID: 24992
		private EventInfoData eventInfo;

		// Token: 0x040061A1 RID: 24993
		private Notification eventComplete;

		// Token: 0x040061A2 RID: 24994
		private Notifier notifier;
	}
}
