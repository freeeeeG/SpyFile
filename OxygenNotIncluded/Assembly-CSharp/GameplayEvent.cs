using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020003E0 RID: 992
[DebuggerDisplay("{base.Id}")]
public abstract class GameplayEvent : Resource, IComparable<GameplayEvent>
{
	// Token: 0x17000080 RID: 128
	// (get) Token: 0x060014DA RID: 5338 RVA: 0x0006E5A8 File Offset: 0x0006C7A8
	// (set) Token: 0x060014DB RID: 5339 RVA: 0x0006E5B0 File Offset: 0x0006C7B0
	public int importance { get; private set; }

	// Token: 0x060014DC RID: 5340 RVA: 0x0006E5BC File Offset: 0x0006C7BC
	public virtual bool IsAllowed()
	{
		if (this.WillNeverRunAgain())
		{
			return false;
		}
		if (!this.allowMultipleEventInstances && GameplayEventManager.Instance.IsGameplayEventActive(this))
		{
			return false;
		}
		foreach (GameplayEventPrecondition gameplayEventPrecondition in this.preconditions)
		{
			if (gameplayEventPrecondition.required && !gameplayEventPrecondition.condition())
			{
				return false;
			}
		}
		float sleepTimer = GameplayEventManager.Instance.GetSleepTimer(this);
		return GameUtil.GetCurrentTimeInCycles() >= sleepTimer;
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x0006E65C File Offset: 0x0006C85C
	public void SetSleepTimer(float timeToSleepUntil)
	{
		GameplayEventManager.Instance.SetSleepTimerForEvent(this, timeToSleepUntil);
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x0006E66A File Offset: 0x0006C86A
	public virtual bool WillNeverRunAgain()
	{
		return this.numTimesAllowed != -1 && GameplayEventManager.Instance.NumberOfPastEvents(this.Id) >= this.numTimesAllowed;
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x0006E697 File Offset: 0x0006C897
	public int GetCashedPriority()
	{
		return this.calculatedPriority;
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x0006E69F File Offset: 0x0006C89F
	public virtual int CalculatePriority()
	{
		this.calculatedPriority = this.basePriority + this.CalculateBoost();
		return this.calculatedPriority;
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x0006E6BC File Offset: 0x0006C8BC
	public int CalculateBoost()
	{
		int num = 0;
		foreach (GameplayEventPrecondition gameplayEventPrecondition in this.preconditions)
		{
			if (!gameplayEventPrecondition.required && gameplayEventPrecondition.condition())
			{
				num += gameplayEventPrecondition.priorityModifier;
			}
		}
		return num;
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x0006E72C File Offset: 0x0006C92C
	public GameplayEvent AddPrecondition(GameplayEventPrecondition precondition)
	{
		precondition.required = true;
		this.preconditions.Add(precondition);
		return this;
	}

	// Token: 0x060014E3 RID: 5347 RVA: 0x0006E742 File Offset: 0x0006C942
	public GameplayEvent AddPriorityBoost(GameplayEventPrecondition precondition, int priorityBoost)
	{
		precondition.required = false;
		precondition.priorityModifier = priorityBoost;
		this.preconditions.Add(precondition);
		return this;
	}

	// Token: 0x060014E4 RID: 5348 RVA: 0x0006E75F File Offset: 0x0006C95F
	public GameplayEvent AddMinionFilter(GameplayEventMinionFilter filter)
	{
		this.minionFilters.Add(filter);
		return this;
	}

	// Token: 0x060014E5 RID: 5349 RVA: 0x0006E76E File Offset: 0x0006C96E
	public GameplayEvent TrySpawnEventOnSuccess(HashedString evt)
	{
		this.successEvents.Add(evt);
		return this;
	}

	// Token: 0x060014E6 RID: 5350 RVA: 0x0006E77D File Offset: 0x0006C97D
	public GameplayEvent TrySpawnEventOnFailure(HashedString evt)
	{
		this.failureEvents.Add(evt);
		return this;
	}

	// Token: 0x060014E7 RID: 5351 RVA: 0x0006E78C File Offset: 0x0006C98C
	public GameplayEvent SetVisuals(HashedString animFileName)
	{
		this.animFileName = animFileName;
		return this;
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x0006E796 File Offset: 0x0006C996
	public virtual Sprite GetDisplaySprite()
	{
		return null;
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x0006E799 File Offset: 0x0006C999
	public virtual string GetDisplayString()
	{
		return null;
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x0006E79C File Offset: 0x0006C99C
	public MinionIdentity GetRandomFilteredMinion()
	{
		List<MinionIdentity> list = new List<MinionIdentity>(Components.LiveMinionIdentities.Items);
		using (List<GameplayEventMinionFilter>.Enumerator enumerator = this.minionFilters.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameplayEventMinionFilter filter = enumerator.Current;
				list.RemoveAll((MinionIdentity x) => !filter.filter(x));
			}
		}
		if (list.Count != 0)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		return null;
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x0006E834 File Offset: 0x0006CA34
	public MinionIdentity GetRandomMinionPrioritizeFiltered()
	{
		MinionIdentity randomFilteredMinion = this.GetRandomFilteredMinion();
		if (!(randomFilteredMinion == null))
		{
			return randomFilteredMinion;
		}
		return Components.LiveMinionIdentities.Items[UnityEngine.Random.Range(0, Components.LiveMinionIdentities.Items.Count)];
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x0006E878 File Offset: 0x0006CA78
	public int CompareTo(GameplayEvent other)
	{
		return -this.GetCashedPriority().CompareTo(other.GetCashedPriority());
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x0006E89C File Offset: 0x0006CA9C
	public GameplayEvent(string id, int priority, int importance) : base(id, null, null)
	{
		this.tags = new List<Tag>();
		this.basePriority = priority;
		this.preconditions = new List<GameplayEventPrecondition>();
		this.minionFilters = new List<GameplayEventMinionFilter>();
		this.successEvents = new List<HashedString>();
		this.failureEvents = new List<HashedString>();
		this.importance = importance;
		this.animFileName = id;
	}

	// Token: 0x060014EE RID: 5358
	public abstract StateMachine.Instance GetSMI(GameplayEventManager manager, GameplayEventInstance eventInstance);

	// Token: 0x060014EF RID: 5359 RVA: 0x0006E90C File Offset: 0x0006CB0C
	public GameplayEventInstance CreateInstance(int worldId)
	{
		GameplayEventInstance gameplayEventInstance = new GameplayEventInstance(this, worldId);
		if (this.tags != null)
		{
			gameplayEventInstance.tags.AddRange(this.tags);
		}
		return gameplayEventInstance;
	}

	// Token: 0x04000B48 RID: 2888
	public const int INFINITE = -1;

	// Token: 0x04000B49 RID: 2889
	public int numTimesAllowed = -1;

	// Token: 0x04000B4A RID: 2890
	public bool allowMultipleEventInstances;

	// Token: 0x04000B4B RID: 2891
	protected int basePriority;

	// Token: 0x04000B4C RID: 2892
	protected int calculatedPriority;

	// Token: 0x04000B4E RID: 2894
	public List<GameplayEventPrecondition> preconditions;

	// Token: 0x04000B4F RID: 2895
	public List<GameplayEventMinionFilter> minionFilters;

	// Token: 0x04000B50 RID: 2896
	public List<HashedString> successEvents;

	// Token: 0x04000B51 RID: 2897
	public List<HashedString> failureEvents;

	// Token: 0x04000B52 RID: 2898
	public string title;

	// Token: 0x04000B53 RID: 2899
	public string description;

	// Token: 0x04000B54 RID: 2900
	public HashedString animFileName;

	// Token: 0x04000B55 RID: 2901
	public List<Tag> tags;
}
