using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using Klei.AI;

// Token: 0x020003E6 RID: 998
public class GameplayEventPreconditions
{
	// Token: 0x17000085 RID: 133
	// (get) Token: 0x0600150A RID: 5386 RVA: 0x0006EE5B File Offset: 0x0006D05B
	public static GameplayEventPreconditions Instance
	{
		get
		{
			if (GameplayEventPreconditions._instance == null)
			{
				GameplayEventPreconditions._instance = new GameplayEventPreconditions();
			}
			return GameplayEventPreconditions._instance;
		}
	}

	// Token: 0x0600150B RID: 5387 RVA: 0x0006EE74 File Offset: 0x0006D074
	public GameplayEventPrecondition LiveMinions(int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => Components.LiveMinionIdentities.Count >= count),
			description = string.Format("At least {0} dupes alive", count)
		};
	}

	// Token: 0x0600150C RID: 5388 RVA: 0x0006EEC0 File Offset: 0x0006D0C0
	public GameplayEventPrecondition BuildingExists(string buildingId, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => BuildingInventory.Instance.BuildingCount(new Tag(buildingId)) >= count),
			description = string.Format("{0} {1} has been built", count, buildingId)
		};
	}

	// Token: 0x0600150D RID: 5389 RVA: 0x0006EF1C File Offset: 0x0006D11C
	public GameplayEventPrecondition ResearchCompleted(string techName)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => Research.Instance.Get(Db.Get().Techs.Get(techName)).IsComplete()),
			description = "Has researched " + techName + "."
		};
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x0006EF68 File Offset: 0x0006D168
	public GameplayEventPrecondition AchievementUnlocked(ColonyAchievement achievement)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => SaveGame.Instance.GetComponent<ColonyAchievementTracker>().IsAchievementUnlocked(achievement)),
			description = "Unlocked the " + achievement.Id + " achievement"
		};
	}

	// Token: 0x0600150F RID: 5391 RVA: 0x0006EFBC File Offset: 0x0006D1BC
	public GameplayEventPrecondition RoomBuilt(RoomType roomType)
	{
		Predicate<Room> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				List<Room> rooms = Game.Instance.roomProber.rooms;
				Predicate<Room> match2;
				if ((match2 = <>9__1) == null)
				{
					match2 = (<>9__1 = ((Room match) => match.roomType == roomType));
				}
				return rooms.Exists(match2);
			},
			description = "Built a " + roomType.Id + " room"
		};
	}

	// Token: 0x06001510 RID: 5392 RVA: 0x0006F010 File Offset: 0x0006D210
	public GameplayEventPrecondition CycleRestriction(float min = 0f, float max = float.PositiveInfinity)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameUtil.GetCurrentTimeInCycles() >= min && GameUtil.GetCurrentTimeInCycles() <= max),
			description = string.Format("After cycle {0} and before cycle {1}", min, max)
		};
	}

	// Token: 0x06001511 RID: 5393 RVA: 0x0006F070 File Offset: 0x0006D270
	public GameplayEventPrecondition MinionsWithEffect(string effectId, int count = 1)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MinionIdentity minion) => minion.GetComponent<Effects>().Get(effectId) != null));
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have the {1} effect applied", count, effectId)
		};
	}

	// Token: 0x06001512 RID: 5394 RVA: 0x0006F0CC File Offset: 0x0006D2CC
	public GameplayEventPrecondition MinionsWithStatusItem(StatusItem statusItem, int count = 1)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = ((MinionIdentity minion) => minion.GetComponent<KSelectable>().HasStatusItem(statusItem)));
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have the {1} status item", count, statusItem)
		};
	}

	// Token: 0x06001513 RID: 5395 RVA: 0x0006F128 File Offset: 0x0006D328
	public GameplayEventPrecondition MinionsWithChoreGroupPriorityOrGreater(ChoreGroup choreGroup, int count, int priority)
	{
		Func<MinionIdentity, bool> <>9__1;
		return new GameplayEventPrecondition
		{
			condition = delegate()
			{
				IEnumerable<MinionIdentity> items = Components.LiveMinionIdentities.Items;
				Func<MinionIdentity, bool> predicate;
				if ((predicate = <>9__1) == null)
				{
					predicate = (<>9__1 = delegate(MinionIdentity minion)
					{
						ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
						return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
					});
				}
				return items.Count(predicate) >= count;
			},
			description = string.Format("At least {0} dupes have their {1} set to {2} or higher.", count, choreGroup.Name, priority)
		};
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x0006F198 File Offset: 0x0006D398
	public GameplayEventPrecondition PastEventCount(string evtId, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameplayEventManager.Instance.NumberOfPastEvents(evtId) >= count),
			description = string.Format("The {0} event has triggered {1} times.", evtId, count)
		};
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x0006F1F4 File Offset: 0x0006D3F4
	public GameplayEventPrecondition PastEventCountAndNotActive(GameplayEvent evt, int count = 1)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => GameplayEventManager.Instance.NumberOfPastEvents(evt.IdHash) >= count && !GameplayEventManager.Instance.IsGameplayEventActive(evt)),
			description = string.Format("The {0} event has triggered {1} times and is not active.", evt.Id, count)
		};
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x0006F254 File Offset: 0x0006D454
	public GameplayEventPrecondition Not(GameplayEventPrecondition precondition)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => !precondition.condition()),
			description = "Not[" + precondition.description + "]"
		};
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x0006F2A8 File Offset: 0x0006D4A8
	public GameplayEventPrecondition Or(GameplayEventPrecondition precondition1, GameplayEventPrecondition precondition2)
	{
		return new GameplayEventPrecondition
		{
			condition = (() => precondition1.condition() || precondition2.condition()),
			description = string.Concat(new string[]
			{
				"[",
				precondition1.description,
				"]-OR-[",
				precondition2.description,
				"]"
			})
		};
	}

	// Token: 0x04000B66 RID: 2918
	private static GameplayEventPreconditions _instance;
}
