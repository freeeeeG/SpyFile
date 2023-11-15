﻿using System;
using System.Collections.Generic;
using STRINGS;
using TUNING;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02000DFB RID: 3579
	public class PartyEvent : GameplayEvent<PartyEvent.StatesInstance>
	{
		// Token: 0x06006DEB RID: 28139 RVA: 0x002B576B File Offset: 0x002B396B
		public PartyEvent() : base("Party", 0, 0)
		{
			this.animFileName = "event_pop_up_assets_kanim";
			this.title = GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.NAME;
			this.description = GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.DESCRIPTION;
		}

		// Token: 0x06006DEC RID: 28140 RVA: 0x002B57AA File Offset: 0x002B39AA
		public override StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance)
		{
			return new PartyEvent.StatesInstance(manager, eventInstance, this);
		}

		// Token: 0x04005273 RID: 21107
		public const string cancelEffect = "NoFunAllowed";

		// Token: 0x04005274 RID: 21108
		public const float FUTURE_TIME = 60f;

		// Token: 0x04005275 RID: 21109
		public const float DURATION = 60f;

		// Token: 0x02001F7F RID: 8063
		public class StatesInstance : GameplayEventStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, PartyEvent>.GameplayEventStateMachineInstance
		{
			// Token: 0x0600A2B0 RID: 41648 RVA: 0x00365E4C File Offset: 0x0036404C
			public StatesInstance(GameplayEventManager master, GameplayEventInstance eventInstance, PartyEvent partyEvent) : base(master, eventInstance, partyEvent)
			{
			}

			// Token: 0x0600A2B1 RID: 41649 RVA: 0x00365E58 File Offset: 0x00364058
			public void AddNewChore(Room room)
			{
				List<KPrefabID> list = room.buildings.FindAll((KPrefabID match) => match.HasTag(RoomConstraints.ConstraintTags.RecBuilding));
				if (list.Count == 0)
				{
					DebugUtil.LogWarningArgs(new object[]
					{
						"Tried adding a party chore but the room wasn't valid! This probably happened on load? It's because rooms aren't built yet!"
					});
					return;
				}
				int num = 0;
				bool flag = false;
				int locator_cell = Grid.InvalidCell;
				Predicate<Chore> <>9__2;
				while (num < 20 && !flag)
				{
					num++;
					KPrefabID cmp = list[UnityEngine.Random.Range(0, list.Count)];
					CellOffset offset = new CellOffset(UnityEngine.Random.Range(-2, 3), 0);
					locator_cell = Grid.OffsetCell(Grid.PosToCell(cmp), offset);
					if (!Grid.HasDoor[locator_cell] && Game.Instance.roomProber.GetCavityForCell(locator_cell) == room.cavity)
					{
						List<Chore> list2 = this.chores;
						Predicate<Chore> match2;
						if ((match2 = <>9__2) == null)
						{
							match2 = (<>9__2 = ((Chore match) => Grid.PosToCell(match.target.gameObject) == locator_cell));
						}
						if (list2.Find(match2) == null)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					return;
				}
				Vector3 pos = Grid.CellToPosCBC(locator_cell, Grid.SceneLayer.Move);
				GameObject locator = ChoreHelpers.CreateLocator("PartyWorkable", pos);
				PartyPointWorkable partyPointWorkable = locator.AddOrGet<PartyPointWorkable>();
				partyPointWorkable.SetWorkTime((float)UnityEngine.Random.Range(10, 30));
				partyPointWorkable.basePriority = RELAXATION.PRIORITY.SPECIAL_EVENT;
				partyPointWorkable.faceTargetWhenWorking = true;
				PartyChore item = new PartyChore(locator.GetComponent<IStateMachineTarget>(), partyPointWorkable, null, null, delegate(Chore data)
				{
					if (this.chores != null)
					{
						this.chores.Remove(data);
						Util.KDestroyGameObject(locator);
					}
				});
				this.chores.Add(item);
			}

			// Token: 0x0600A2B2 RID: 41650 RVA: 0x00365FFC File Offset: 0x003641FC
			public void ClearChores()
			{
				if (this.chores != null)
				{
					for (int i = this.chores.Count - 1; i >= 0; i--)
					{
						if (this.chores[i] != null)
						{
							Util.KDestroyGameObject(this.chores[i].gameObject);
						}
					}
				}
				this.chores = null;
			}

			// Token: 0x0600A2B3 RID: 41651 RVA: 0x00366054 File Offset: 0x00364254
			public void UpdateChores(Room room)
			{
				if (room == null)
				{
					return;
				}
				if (this.chores == null)
				{
					this.chores = new List<Chore>();
				}
				for (int i = this.chores.Count; i < Components.LiveMinionIdentities.Count * 2; i++)
				{
					this.AddNewChore(room);
				}
			}

			// Token: 0x04008E52 RID: 36434
			private List<Chore> chores;

			// Token: 0x04008E53 RID: 36435
			public Notification mainNotification;
		}

		// Token: 0x02001F80 RID: 8064
		public class States : GameplayEventStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, PartyEvent>
		{
			// Token: 0x0600A2B4 RID: 41652 RVA: 0x003660A0 File Offset: 0x003642A0
			public override void InitializeStates(out StateMachine.BaseState default_state)
			{
				base.InitializeStates(out default_state);
				default_state = this.planning.prepare_entities;
				base.serializable = StateMachine.SerializeType.Both_DEPRECATED;
				this.root.Enter(new StateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State.Callback(this.PopulateTargetsAndText));
				this.planning.DefaultState(this.planning.prepare_entities);
				this.planning.prepare_entities.Enter(delegate(PartyEvent.StatesInstance smi)
				{
					if (this.planner.Get(smi) != null && this.guest.Get(smi) != null)
					{
						smi.GoTo(this.planning.wait_for_input);
						return;
					}
					smi.GoTo(this.ending);
				});
				this.planning.wait_for_input.ToggleNotification((PartyEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.warmup.ToggleNotification((PartyEvent.StatesInstance smi) => EventInfoScreen.CreateNotification(this.GenerateEventPopupData(smi), null));
				this.warmup.wait.ScheduleGoTo(60f, this.warmup.start);
				this.warmup.start.Enter(new StateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State.Callback(this.PopulateTargetsAndText)).Enter(delegate(PartyEvent.StatesInstance smi)
				{
					if (this.GetChosenRoom(smi) == null)
					{
						smi.GoTo(this.canceled);
						return;
					}
					smi.GoTo(this.partying);
				});
				this.partying.ToggleNotification((PartyEvent.StatesInstance smi) => new Notification(GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.UNDERWAY, NotificationType.Good, (List<Notification> a, object b) => GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.UNDERWAY_TOOLTIP, null, false, 0f, null, null, this.roomObject.Get(smi).transform, true, false, false)).Update(delegate(PartyEvent.StatesInstance smi, float dt)
				{
					smi.UpdateChores(this.GetChosenRoom(smi));
				}, UpdateRate.SIM_4000ms, false).ScheduleGoTo(60f, this.ending);
				this.ending.ReturnSuccess();
				this.canceled.DoNotification((PartyEvent.StatesInstance smi) => GameplayEventManager.CreateStandardCancelledNotification(this.GenerateEventPopupData(smi))).Enter(delegate(PartyEvent.StatesInstance smi)
				{
					if (this.planner.Get(smi) != null)
					{
						this.planner.Get(smi).GetComponent<Effects>().Add("NoFunAllowed", true);
					}
					if (this.guest.Get(smi) != null)
					{
						this.guest.Get(smi).GetComponent<Effects>().Add("NoFunAllowed", true);
					}
				}).ReturnFailure();
			}

			// Token: 0x0600A2B5 RID: 41653 RVA: 0x0036620D File Offset: 0x0036440D
			public Room GetChosenRoom(PartyEvent.StatesInstance smi)
			{
				return Game.Instance.roomProber.GetRoomOfGameObject(this.roomObject.Get(smi));
			}

			// Token: 0x0600A2B6 RID: 41654 RVA: 0x0036622C File Offset: 0x0036442C
			public override EventInfoData GenerateEventPopupData(PartyEvent.StatesInstance smi)
			{
				EventInfoData eventInfoData = new EventInfoData(smi.gameplayEvent.title, smi.gameplayEvent.description, smi.gameplayEvent.animFileName);
				Room chosenRoom = this.GetChosenRoom(smi);
				string location = (chosenRoom != null) ? chosenRoom.GetProperName() : GAMEPLAY_EVENTS.LOCATIONS.NONE_AVAILABLE.ToString();
				Effect effect = Db.Get().effects.Get("Socialized");
				Effect effect2 = Db.Get().effects.Get("NoFunAllowed");
				eventInfoData.location = location;
				eventInfoData.whenDescription = string.Format(GAMEPLAY_EVENTS.TIMES.IN_CYCLES, 0.1f);
				eventInfoData.minions = new GameObject[]
				{
					smi.sm.guest.Get(smi),
					smi.sm.planner.Get(smi)
				};
				bool flag = true;
				EventInfoData.Option option = eventInfoData.AddOption(GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.ACCEPT_OPTION_NAME, GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.ACCEPT_OPTION_DESC);
				option.callback = delegate()
				{
					smi.GoTo(smi.sm.warmup.wait);
				};
				option.AddPositiveIcon(Assets.GetSprite("overlay_materials"), Effect.CreateFullTooltip(effect, true), 1f);
				option.tooltip = GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.ACCEPT_OPTION_DESC;
				if (!flag)
				{
					option.AddInformationIcon("Cake must be built", 1f);
					option.allowed = false;
					option.tooltip = GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.ACCEPT_OPTION_INVALID_TOOLTIP;
				}
				EventInfoData.Option option2 = eventInfoData.AddOption(GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.REJECT_OPTION_NAME, GAMEPLAY_EVENTS.EVENT_TYPES.PARTY.REJECT_OPTION_DESC);
				option2.callback = delegate()
				{
					smi.GoTo(smi.sm.canceled);
				};
				option2.AddNegativeIcon(Assets.GetSprite("overlay_decor"), Effect.CreateFullTooltip(effect2, true), 1f);
				eventInfoData.AddDefaultConsiderLaterOption(null);
				eventInfoData.SetTextParameter("host", this.planner.Get(smi).GetProperName());
				eventInfoData.SetTextParameter("dupe", this.guest.Get(smi).GetProperName());
				eventInfoData.SetTextParameter("goodEffect", effect.Name);
				eventInfoData.SetTextParameter("badEffect", effect2.Name);
				return eventInfoData;
			}

			// Token: 0x0600A2B7 RID: 41655 RVA: 0x00366488 File Offset: 0x00364688
			public void PopulateTargetsAndText(PartyEvent.StatesInstance smi)
			{
				if (this.roomObject.Get(smi) == null)
				{
					Room room = Game.Instance.roomProber.rooms.Find((Room match) => match.roomType == Db.Get().RoomTypes.RecRoom);
					this.roomObject.Set((room != null) ? room.GetPrimaryEntities()[0] : null, smi);
				}
				if (Components.LiveMinionIdentities.Count > 0)
				{
					if (this.planner.Get(smi) == null)
					{
						MinionIdentity value = Components.LiveMinionIdentities[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count)];
						this.planner.Set(value, smi);
					}
					if (this.guest.Get(smi) == null)
					{
						MinionIdentity value2 = Components.LiveMinionIdentities[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Count)];
						this.guest.Set(value2, smi);
					}
				}
			}

			// Token: 0x04008E54 RID: 36436
			public StateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.TargetParameter roomObject;

			// Token: 0x04008E55 RID: 36437
			public StateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.TargetParameter planner;

			// Token: 0x04008E56 RID: 36438
			public StateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.TargetParameter guest;

			// Token: 0x04008E57 RID: 36439
			public PartyEvent.States.PlanningStates planning;

			// Token: 0x04008E58 RID: 36440
			public PartyEvent.States.WarmupStates warmup;

			// Token: 0x04008E59 RID: 36441
			public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State partying;

			// Token: 0x04008E5A RID: 36442
			public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State ending;

			// Token: 0x04008E5B RID: 36443
			public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State canceled;

			// Token: 0x02002F4D RID: 12109
			public class PlanningStates : GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400C182 RID: 49538
				public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State prepare_entities;

				// Token: 0x0400C183 RID: 49539
				public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State wait_for_input;
			}

			// Token: 0x02002F4E RID: 12110
			public class WarmupStates : GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State
			{
				// Token: 0x0400C184 RID: 49540
				public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State wait;

				// Token: 0x0400C185 RID: 49541
				public GameStateMachine<PartyEvent.States, PartyEvent.StatesInstance, GameplayEventManager, object>.State start;
			}
		}
	}
}
