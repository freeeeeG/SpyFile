using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DF3 RID: 3571
	public class BonusEvent : GameplayEvent<BonusEvent.StatesInstance>
	{
		// Token: 0x06006DC5 RID: 28101 RVA: 0x002B4E04 File Offset: 0x002B3004
		public BonusEvent(string id, string overrideEffect = null, int numTimesAllowed = 1, bool preSelectMinion = false, int priority = 0) : base(id, priority, 0)
		{
			this.title = Strings.Get("STRINGS.GAMEPLAY_EVENTS.BONUS." + id.ToUpper() + ".NAME");
			this.description = Strings.Get("STRINGS.GAMEPLAY_EVENTS.BONUS." + id.ToUpper() + ".DESCRIPTION");
			this.effect = ((overrideEffect != null) ? overrideEffect : id);
			this.numTimesAllowed = numTimesAllowed;
			this.preSelectMinion = preSelectMinion;
			this.animFileName = id.ToLower() + "_kanim";
			base.AddPrecondition(GameplayEventPreconditions.Instance.LiveMinions(1));
		}

		// Token: 0x06006DC6 RID: 28102 RVA: 0x002B4EAE File Offset: 0x002B30AE
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new BonusEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x06006DC7 RID: 28103 RVA: 0x002B4EB8 File Offset: 0x002B30B8
		public BonusEvent TriggerOnNewBuilding(int triggerCount, params string[] buildings)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.NewBuilding;
			this.buildingTrigger = new HashSet<Tag>(buildings.ToTagList());
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x06006DC8 RID: 28104 RVA: 0x002B4EEE File Offset: 0x002B30EE
		public BonusEvent TriggerOnUseBuilding(int triggerCount, params string[] buildings)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.UseBuilding;
			this.buildingTrigger = new HashSet<Tag>(buildings.ToTagList());
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x06006DC9 RID: 28105 RVA: 0x002B4F24 File Offset: 0x002B3124
		public BonusEvent TriggerOnWorkableComplete(int triggerCount, params Type[] types)
		{
			DebugUtil.DevAssert(this.triggerType == BonusEvent.TriggerType.None, "Only one trigger per event", null);
			this.triggerType = BonusEvent.TriggerType.WorkableComplete;
			this.workableType = new HashSet<Type>(types);
			this.numTimesToTrigger = triggerCount;
			return this;
		}

		// Token: 0x06006DCA RID: 28106 RVA: 0x002B4F55 File Offset: 0x002B3155
		public BonusEvent SetExtraCondition(BonusEvent.ConditionFn extraCondition)
		{
			this.extraCondition = extraCondition;
			return this;
		}

		// Token: 0x06006DCB RID: 28107 RVA: 0x002B4F5F File Offset: 0x002B315F
		public BonusEvent SetRoomConstraints(bool hasOwnableInRoom, params RoomType[] types)
		{
			this.roomHasOwnable = hasOwnableInRoom;
			this.roomRestrictions = ((types == null) ? null : new HashSet<RoomType>(types));
			return this;
		}

		// Token: 0x06006DCC RID: 28108 RVA: 0x002B4F7B File Offset: 0x002B317B
		public string GetEffectTooltip(Effect effect)
		{
			return effect.Name + "\n\n" + Effect.CreateTooltip(effect, true, "\n    • ", true);
		}

		// Token: 0x06006DCD RID: 28109 RVA: 0x002B4F9C File Offset: 0x002B319C
		public override Sprite GetDisplaySprite()
		{
			Effect effect = Db.Get().effects.Get(this.effect);
			if (effect.SelfModifiers.Count > 0)
			{
				return Assets.GetSprite(Db.Get().Attributes.TryGet(effect.SelfModifiers[0].AttributeId).uiFullColourSprite);
			}
			return null;
		}

		// Token: 0x06006DCE RID: 28110 RVA: 0x002B5000 File Offset: 0x002B3200
		public override string GetDisplayString()
		{
			Effect effect = Db.Get().effects.Get(this.effect);
			if (effect.SelfModifiers.Count > 0)
			{
				return Db.Get().Attributes.TryGet(effect.SelfModifiers[0].AttributeId).Name;
			}
			return null;
		}

		// Token: 0x04005240 RID: 21056
		public const int PRE_SELECT_MINION_TIMEOUT = 5;

		// Token: 0x04005241 RID: 21057
		public string effect;

		// Token: 0x04005242 RID: 21058
		public bool preSelectMinion;

		// Token: 0x04005243 RID: 21059
		public int numTimesToTrigger;

		// Token: 0x04005244 RID: 21060
		public BonusEvent.TriggerType triggerType;

		// Token: 0x04005245 RID: 21061
		public HashSet<Tag> buildingTrigger;

		// Token: 0x04005246 RID: 21062
		public HashSet<Type> workableType;

		// Token: 0x04005247 RID: 21063
		public HashSet<RoomType> roomRestrictions;

		// Token: 0x04005248 RID: 21064
		public BonusEvent.ConditionFn extraCondition;

		// Token: 0x04005249 RID: 21065
		public bool roomHasOwnable;

		// Token: 0x02001F6F RID: 8047
		public enum TriggerType
		{
			// Token: 0x04008E18 RID: 36376
			None,
			// Token: 0x04008E19 RID: 36377
			NewBuilding,
			// Token: 0x04008E1A RID: 36378
			UseBuilding,
			// Token: 0x04008E1B RID: 36379
			WorkableComplete,
			// Token: 0x04008E1C RID: 36380
			AchievementUnlocked
		}

		// Token: 0x02001F70 RID: 8048
		// (Invoke) Token: 0x0600A263 RID: 41571
		public delegate bool ConditionFn(BonusEvent.GameplayEventData data);

		// Token: 0x02001F71 RID: 8049
		public class GameplayEventData
		{
			// Token: 0x04008E1D RID: 36381
			public GameHashes eventTrigger;

			// Token: 0x04008E1E RID: 36382
			public BuildingComplete building;

			// Token: 0x04008E1F RID: 36383
			public Workable workable;

			// Token: 0x04008E20 RID: 36384
			public Worker worker;
		}

		// Token: 0x02001F72 RID: 8050
		public class States : GameplayEventStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, BonusEvent>
		{
			// Token: 0x0600A267 RID: 41575 RVA: 0x003641DC File Offset: 0x003623DC
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				default_state = this.load;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.load.Enter(new StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State.Callback(this.AssignPreSelectedMinionIfNeeded)).Transition(this.waitNewBuilding, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.NewBuilding, UpdateRate.SIM_200ms).Transition(this.waitUseBuilding, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.UseBuilding, UpdateRate.SIM_200ms).Transition(this.waitforWorkables, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.WorkableComplete, UpdateRate.SIM_200ms).Transition(this.waitForAchievement, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.AchievementUnlocked, UpdateRate.SIM_200ms).Transition(this.immediate, (BonusEvent.StatesInstance smi) => smi.gameplayEvent.triggerType == BonusEvent.TriggerType.None, UpdateRate.SIM_200ms);
				this.waitNewBuilding.EventHandlerTransition(GameHashes.NewBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.BuildingEventTrigger));
				this.waitUseBuilding.EventHandlerTransition(GameHashes.UseBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.BuildingEventTrigger));
				this.waitforWorkables.EventHandlerTransition(GameHashes.UseBuilding, this.active, new Func<BonusEvent.StatesInstance, object, bool>(this.WorkableEventTrigger));
				this.immediate.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					GameObject gameObject = smi.sm.chosen.Get(smi);
					if (gameObject == null)
					{
						gameObject = smi.gameplayEvent.GetRandomMinionPrioritizeFiltered().gameObject;
						smi.sm.chosen.Set(gameObject, smi, false);
					}
				}).GoTo(this.active);
				this.active.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					smi.sm.chosen.Get(smi).GetComponent<Effects>().Add(smi.gameplayEvent.effect, true);
				}).Enter(delegate(BonusEvent.StatesInstance smi)
				{
					base.MonitorStart(this.chosen, smi);
				}).Exit(delegate(BonusEvent.StatesInstance smi)
				{
					base.MonitorStop(this.chosen, smi);
				}).ScheduleGoTo(delegate(BonusEvent.StatesInstance smi)
				{
					Effect effect = this.GetEffect(smi);
					if (effect != null)
					{
						return effect.duration;
					}
					return 0f;
				}, this.ending).DefaultState(this.active.notify).OnTargetLost(this.chosen, this.ending).Target(this.chosen).TagTransition(GameTags.Dead, this.ending, false);
				this.active.notify.ToggleNotification((BonusEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.active.seenNotification.Enter(delegate(BonusEvent.StatesInstance smi)
				{
					smi.eventInstance.seenNotification = true;
				});
				this.ending.ReturnSuccess();
			}

			// Token: 0x0600A268 RID: 41576 RVA: 0x00364480 File Offset: 0x00362680
			public override EventInfoData GenerateEventPopupData(BonusEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				GameObject gameObject = smi.sm.chosen.Get(smi);
				if (gameObject == null)
				{
					DebugUtil.LogErrorArgs(new object[]
					{
						"Minion not set for " + smi.gameplayEvent.Id
					});
					return null;
				}
				Effect effect = this.GetEffect(smi);
				if (effect == null)
				{
					return null;
				}
				eventInfoData.clickFocus = gameObject.transform;
				eventInfoData.minions = new GameObject[]
				{
					gameObject
				};
				eventInfoData.SetTextParameter("dupe", gameObject.GetProperName());
				if (smi.building != null)
				{
					eventInfoData.SetTextParameter("building", UI.FormatAsLink(smi.building.GetProperName(), smi.building.GetProperName().ToUpper()));
				}
				EventInfoData.Option option = eventInfoData.AddDefaultOption(delegate
				{
					smi.GoTo(smi.sm.active.seenNotification);
				});
				GAMEPLAY_EVENTS.BONUS_EVENT_DESCRIPTION.Replace("{effects}", Effect.CreateTooltip(effect, false, " ", false)).Replace("{durration}", GameUtil.GetFormattedCycles(effect.duration, "F1", false));
				foreach (AttributeModifier attributeModifier in effect.SelfModifiers)
				{
					Attribute attribute = Db.Get().Attributes.TryGet(attributeModifier.AttributeId);
					string text = string.Format(DUPLICANTS.MODIFIERS.MODIFIER_FORMAT, attribute.Name, attributeModifier.GetFormattedString());
					text = text + "\n" + string.Format(DUPLICANTS.MODIFIERS.TIME_TOTAL, GameUtil.GetFormattedCycles(effect.duration, "F1", false));
					Sprite sprite = Assets.GetSprite(attribute.uiFullColourSprite);
					option.AddPositiveIcon(sprite, text, 1.75f);
				}
				return eventInfoData;
			}

			// Token: 0x0600A269 RID: 41577 RVA: 0x003646C8 File Offset: 0x003628C8
			private void AssignPreSelectedMinionIfNeeded(BonusEvent.StatesInstance smi)
			{
				if (smi.gameplayEvent.preSelectMinion && smi.sm.chosen.Get(smi) == null)
				{
					smi.sm.chosen.Set(smi.gameplayEvent.GetRandomMinionPrioritizeFiltered().gameObject, smi, false);
					smi.timesTriggered = 0;
				}
			}

			// Token: 0x0600A26A RID: 41578 RVA: 0x00364728 File Offset: 0x00362928
			private bool IsCorrectMinion(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				if (!smi.gameplayEvent.preSelectMinion || !(smi.sm.chosen.Get(smi) != gameplayEventData.worker.gameObject))
				{
					return true;
				}
				if (GameUtil.GetCurrentTimeInCycles() - smi.lastTriggered > 5f && smi.PercentageUntilTriggered() < 0.5f)
				{
					smi.sm.chosen.Set(gameplayEventData.worker.gameObject, smi, false);
					smi.timesTriggered = 0;
					return true;
				}
				return false;
			}

			// Token: 0x0600A26B RID: 41579 RVA: 0x003647B0 File Offset: 0x003629B0
			private bool OtherConditionsAreSatisfied(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				if (smi.gameplayEvent.roomRestrictions != null)
				{
					Room roomOfGameObject = Game.Instance.roomProber.GetRoomOfGameObject(gameplayEventData.worker.gameObject);
					if (roomOfGameObject == null)
					{
						return false;
					}
					if (!smi.gameplayEvent.roomRestrictions.Contains(roomOfGameObject.roomType))
					{
						return false;
					}
					if (smi.gameplayEvent.roomHasOwnable)
					{
						bool flag = false;
						using (List<Ownables>.Enumerator enumerator = roomOfGameObject.GetOwners().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.gameObject == gameplayEventData.worker.gameObject)
								{
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							return false;
						}
					}
				}
				return smi.gameplayEvent.extraCondition == null || smi.gameplayEvent.extraCondition(gameplayEventData);
			}

			// Token: 0x0600A26C RID: 41580 RVA: 0x00364898 File Offset: 0x00362A98
			private bool IncrementAndTrigger(BonusEvent.StatesInstance smi, BonusEvent.GameplayEventData gameplayEventData)
			{
				smi.timesTriggered++;
				smi.lastTriggered = GameUtil.GetCurrentTimeInCycles();
				if (smi.timesTriggered < smi.gameplayEvent.numTimesToTrigger)
				{
					return false;
				}
				smi.building = gameplayEventData.building;
				smi.sm.chosen.Set(gameplayEventData.worker.gameObject, smi, false);
				return true;
			}

			// Token: 0x0600A26D RID: 41581 RVA: 0x00364900 File Offset: 0x00362B00
			private bool BuildingEventTrigger(BonusEvent.StatesInstance smi, object data)
			{
				BonusEvent.GameplayEventData gameplayEventData = data as BonusEvent.GameplayEventData;
				if (gameplayEventData == null)
				{
					return false;
				}
				this.AssignPreSelectedMinionIfNeeded(smi);
				return !(gameplayEventData.building == null) && (smi.gameplayEvent.buildingTrigger.Count <= 0 || smi.gameplayEvent.buildingTrigger.Contains(gameplayEventData.building.PrefabID())) && this.OtherConditionsAreSatisfied(smi, gameplayEventData) && this.IsCorrectMinion(smi, gameplayEventData) && this.IncrementAndTrigger(smi, gameplayEventData);
			}

			// Token: 0x0600A26E RID: 41582 RVA: 0x00364984 File Offset: 0x00362B84
			private bool WorkableEventTrigger(BonusEvent.StatesInstance smi, object data)
			{
				BonusEvent.GameplayEventData gameplayEventData = data as BonusEvent.GameplayEventData;
				if (gameplayEventData == null)
				{
					return false;
				}
				this.AssignPreSelectedMinionIfNeeded(smi);
				return (smi.gameplayEvent.workableType.Count <= 0 || smi.gameplayEvent.workableType.Contains(gameplayEventData.workable.GetType())) && this.OtherConditionsAreSatisfied(smi, gameplayEventData) && this.IsCorrectMinion(smi, gameplayEventData) && this.IncrementAndTrigger(smi, gameplayEventData);
			}

			// Token: 0x0600A26F RID: 41583 RVA: 0x003649F6 File Offset: 0x00362BF6
			private bool ChosenMinionDied(BonusEvent.StatesInstance smi, object data)
			{
				return smi.sm.chosen.Get(smi) == data as GameObject;
			}

			// Token: 0x0600A270 RID: 41584 RVA: 0x00364A14 File Offset: 0x00362C14
			private Effect GetEffect(BonusEvent.StatesInstance smi)
			{
				GameObject gameObject = smi.sm.chosen.Get(smi);
				if (gameObject == null)
				{
					return null;
				}
				EffectInstance effectInstance = gameObject.GetComponent<Effects>().Get(smi.gameplayEvent.effect);
				if (effectInstance == null)
				{
					global::Debug.LogWarning(string.Format("Effect {0} not found on {1} in BonusEvent", smi.gameplayEvent.effect, gameObject));
					return null;
				}
				return effectInstance.effect;
			}

			// Token: 0x04008E21 RID: 36385
			public StateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.TargetParameter chosen;

			// Token: 0x04008E22 RID: 36386
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State load;

			// Token: 0x04008E23 RID: 36387
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitNewBuilding;

			// Token: 0x04008E24 RID: 36388
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitUseBuilding;

			// Token: 0x04008E25 RID: 36389
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitForAchievement;

			// Token: 0x04008E26 RID: 36390
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State waitforWorkables;

			// Token: 0x04008E27 RID: 36391
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State immediate;

			// Token: 0x04008E28 RID: 36392
			public BonusEvent.States.ActiveStates active;

			// Token: 0x04008E29 RID: 36393
			public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State ending;

			// Token: 0x02002F3E RID: 12094
			public class ActiveStates : GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400C154 RID: 49492
				public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State notify;

				// Token: 0x0400C155 RID: 49493
				public GameStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, object>.State seenNotification;
			}
		}

		// Token: 0x02001F73 RID: 8051
		public class StatesInstance : GameplayEventStateMachine<BonusEvent.States, BonusEvent.StatesInstance, GameplayEventManager, BonusEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600A276 RID: 41590 RVA: 0x00364AD7 File Offset: 0x00362CD7
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, BonusEvent bonusEvent) : base(master, eventInstance, bonusEvent)
			{
				this.lastTriggered = GameUtil.GetCurrentTimeInCycles();
			}

			// Token: 0x0600A277 RID: 41591 RVA: 0x00364AED File Offset: 0x00362CED
			public float PercentageUntilTriggered()
			{
				return (float)this.timesTriggered / (float)base.smi.gameplayEvent.numTimesToTrigger;
			}

			// Token: 0x04008E2A RID: 36394
			[Serialize]
			public int timesTriggered;

			// Token: 0x04008E2B RID: 36395
			[Serialize]
			public float lastTriggered;

			// Token: 0x04008E2C RID: 36396
			public BuildingComplete building;
		}
	}
}
