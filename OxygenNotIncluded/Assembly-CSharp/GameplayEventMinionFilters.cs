using System;
using Database;

// Token: 0x020003E4 RID: 996
public class GameplayEventMinionFilters
{
	// Token: 0x17000084 RID: 132
	// (get) Token: 0x06001500 RID: 5376 RVA: 0x0006EC26 File Offset: 0x0006CE26
	public static GameplayEventMinionFilters Instance
	{
		get
		{
			if (GameplayEventMinionFilters._instance == null)
			{
				GameplayEventMinionFilters._instance = new GameplayEventMinionFilters();
			}
			return GameplayEventMinionFilters._instance;
		}
	}

	// Token: 0x06001501 RID: 5377 RVA: 0x0006EC40 File Offset: 0x0006CE40
	public GameplayEventMinionFilter HasMasteredSkill(Skill skill)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.GetComponent<MinionResume>().HasMasteredSkill(skill.Id)),
			id = "HasMasteredSkill"
		};
	}

	// Token: 0x06001502 RID: 5378 RVA: 0x0006EC7C File Offset: 0x0006CE7C
	public GameplayEventMinionFilter HasSkillAptitude(Skill skill)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.GetComponent<MinionResume>().HasSkillAptitude(skill)),
			id = "HasSkillAptitude"
		};
	}

	// Token: 0x06001503 RID: 5379 RVA: 0x0006ECB8 File Offset: 0x0006CEB8
	public GameplayEventMinionFilter HasChoreGroupPriorityOrHigher(ChoreGroup choreGroup, int priority)
	{
		return new GameplayEventMinionFilter
		{
			filter = delegate(MinionIdentity minion)
			{
				ChoreConsumer component = minion.GetComponent<ChoreConsumer>();
				return !component.IsChoreGroupDisabled(choreGroup) && component.GetPersonalPriority(choreGroup) >= priority;
			},
			id = "HasChoreGroupPriorityOrHigher"
		};
	}

	// Token: 0x06001504 RID: 5380 RVA: 0x0006ECFC File Offset: 0x0006CEFC
	public GameplayEventMinionFilter AgeRange(float min = 0f, float max = float.PositiveInfinity)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => minion.arrivalTime >= min && minion.arrivalTime <= max),
			id = "AgeRange"
		};
	}

	// Token: 0x06001505 RID: 5381 RVA: 0x0006ED3F File Offset: 0x0006CF3F
	public GameplayEventMinionFilter PriorityIn()
	{
		GameplayEventMinionFilter gameplayEventMinionFilter = new GameplayEventMinionFilter();
		gameplayEventMinionFilter.filter = ((MinionIdentity minion) => true);
		gameplayEventMinionFilter.id = "PriorityIn";
		return gameplayEventMinionFilter;
	}

	// Token: 0x06001506 RID: 5382 RVA: 0x0006ED78 File Offset: 0x0006CF78
	public GameplayEventMinionFilter Not(GameplayEventMinionFilter filter)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => !filter.filter(minion)),
			id = "Not[" + filter.id + "]"
		};
	}

	// Token: 0x06001507 RID: 5383 RVA: 0x0006EDCC File Offset: 0x0006CFCC
	public GameplayEventMinionFilter Or(GameplayEventMinionFilter precondition1, GameplayEventMinionFilter precondition2)
	{
		return new GameplayEventMinionFilter
		{
			filter = ((MinionIdentity minion) => precondition1.filter(minion) || precondition2.filter(minion)),
			id = string.Concat(new string[]
			{
				"[",
				precondition1.id,
				"]-OR-[",
				precondition2.id,
				"]"
			})
		};
	}

	// Token: 0x04000B61 RID: 2913
	private static GameplayEventMinionFilters _instance;
}
