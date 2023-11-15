using System;
using System.Collections.Generic;
using KSerialization;
using STRINGS;

// Token: 0x020007D5 RID: 2005
public class GameplayEventManager : KMonoBehaviour
{
	// Token: 0x06003889 RID: 14473 RVA: 0x0013A747 File Offset: 0x00138947
	public static void DestroyInstance()
	{
		GameplayEventManager.Instance = null;
	}

	// Token: 0x0600388A RID: 14474 RVA: 0x0013A74F File Offset: 0x0013894F
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		GameplayEventManager.Instance = this;
		this.notifier = base.GetComponent<Notifier>();
	}

	// Token: 0x0600388B RID: 14475 RVA: 0x0013A769 File Offset: 0x00138969
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.RestoreEvents();
	}

	// Token: 0x0600388C RID: 14476 RVA: 0x0013A777 File Offset: 0x00138977
	protected override void OnCleanUp()
	{
		base.OnCleanUp();
		GameplayEventManager.Instance = null;
	}

	// Token: 0x0600388D RID: 14477 RVA: 0x0013A788 File Offset: 0x00138988
	private void RestoreEvents()
	{
		this.activeEvents.RemoveAll((GameplayEventInstance x) => Db.Get().GameplayEvents.TryGet(x.eventID) == null);
		for (int i = this.activeEvents.Count - 1; i >= 0; i--)
		{
			GameplayEventInstance gameplayEventInstance = this.activeEvents[i];
			if (gameplayEventInstance.smi == null)
			{
				this.StartEventInstance(gameplayEventInstance, null);
			}
		}
	}

	// Token: 0x0600388E RID: 14478 RVA: 0x0013A7F5 File Offset: 0x001389F5
	public void SetSleepTimerForEvent(GameplayEvent eventType, float time)
	{
		this.sleepTimers[eventType.IdHash] = time;
	}

	// Token: 0x0600388F RID: 14479 RVA: 0x0013A80C File Offset: 0x00138A0C
	public float GetSleepTimer(GameplayEvent eventType)
	{
		float num = 0f;
		this.sleepTimers.TryGetValue(eventType.IdHash, out num);
		this.sleepTimers[eventType.IdHash] = num;
		return num;
	}

	// Token: 0x06003890 RID: 14480 RVA: 0x0013A848 File Offset: 0x00138A48
	public bool IsGameplayEventActive(GameplayEvent eventType)
	{
		return this.activeEvents.Find((GameplayEventInstance e) => e.eventID == eventType.IdHash) != null;
	}

	// Token: 0x06003891 RID: 14481 RVA: 0x0013A87C File Offset: 0x00138A7C
	public bool IsGameplayEventRunningWithTag(Tag tag)
	{
		using (List<GameplayEventInstance>.Enumerator enumerator = this.activeEvents.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				if (enumerator.Current.tags.Contains(tag))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003892 RID: 14482 RVA: 0x0013A8DC File Offset: 0x00138ADC
	public void GetActiveEventsOfType<T>(int worldID, ref List<GameplayEventInstance> results) where T : GameplayEvent
	{
		foreach (GameplayEventInstance gameplayEventInstance in this.activeEvents)
		{
			if (gameplayEventInstance.worldId == worldID && gameplayEventInstance.gameplayEvent is T)
			{
				results.Add(gameplayEventInstance);
			}
		}
	}

	// Token: 0x06003893 RID: 14483 RVA: 0x0013A950 File Offset: 0x00138B50
	public void GetActiveEventsOfType<T>(ref List<GameplayEventInstance> results) where T : GameplayEvent
	{
		foreach (GameplayEventInstance gameplayEventInstance in this.activeEvents)
		{
			if (gameplayEventInstance.gameplayEvent is T)
			{
				results.Add(gameplayEventInstance);
			}
		}
	}

	// Token: 0x06003894 RID: 14484 RVA: 0x0013A9BC File Offset: 0x00138BBC
	private GameplayEventInstance CreateGameplayEvent(GameplayEvent gameplayEvent, int worldId)
	{
		return gameplayEvent.CreateInstance(worldId);
	}

	// Token: 0x06003895 RID: 14485 RVA: 0x0013A9C8 File Offset: 0x00138BC8
	public GameplayEventInstance GetGameplayEventInstance(HashedString eventID, int worldId = -1)
	{
		return this.activeEvents.Find((GameplayEventInstance e) => e.eventID == eventID && (worldId == -1 || e.worldId == worldId));
	}

	// Token: 0x06003896 RID: 14486 RVA: 0x0013AA00 File Offset: 0x00138C00
	public GameplayEventInstance CreateOrGetEventInstance(GameplayEvent eventType, int worldId = -1)
	{
		GameplayEventInstance gameplayEventInstance = this.GetGameplayEventInstance(eventType.Id, worldId);
		if (gameplayEventInstance == null)
		{
			gameplayEventInstance = this.StartNewEvent(eventType, worldId, null);
		}
		return gameplayEventInstance;
	}

	// Token: 0x06003897 RID: 14487 RVA: 0x0013AA30 File Offset: 0x00138C30
	public void RemoveActiveEvent(GameplayEventInstance eventInstance, string reason = "RemoveActiveEvent() called")
	{
		GameplayEventInstance gameplayEventInstance = this.activeEvents.Find((GameplayEventInstance x) => x == eventInstance);
		if (gameplayEventInstance != null)
		{
			if (gameplayEventInstance.smi != null)
			{
				gameplayEventInstance.smi.StopSM(reason);
				return;
			}
			this.activeEvents.Remove(gameplayEventInstance);
		}
	}

	// Token: 0x06003898 RID: 14488 RVA: 0x0013AA88 File Offset: 0x00138C88
	public GameplayEventInstance StartNewEvent(GameplayEvent eventType, int worldId = -1, Action<StateMachine.Instance> setupActionsBeforeStart = null)
	{
		GameplayEventInstance gameplayEventInstance = this.CreateGameplayEvent(eventType, worldId);
		this.StartEventInstance(gameplayEventInstance, setupActionsBeforeStart);
		this.activeEvents.Add(gameplayEventInstance);
		int num;
		this.pastEvents.TryGetValue(gameplayEventInstance.eventID, out num);
		this.pastEvents[gameplayEventInstance.eventID] = num + 1;
		return gameplayEventInstance;
	}

	// Token: 0x06003899 RID: 14489 RVA: 0x0013AADC File Offset: 0x00138CDC
	private void StartEventInstance(GameplayEventInstance gameplayEventInstance, Action<StateMachine.Instance> setupActionsBeforeStart = null)
	{
		StateMachine.Instance instance = gameplayEventInstance.PrepareEvent(this);
		StateMachine.Instance instance2 = instance;
		instance2.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(instance2.OnStop, new Action<string, StateMachine.Status>(delegate(string reason, StateMachine.Status status)
		{
			this.activeEvents.Remove(gameplayEventInstance);
		}));
		if (setupActionsBeforeStart != null)
		{
			setupActionsBeforeStart(instance);
		}
		gameplayEventInstance.StartEvent();
	}

	// Token: 0x0600389A RID: 14490 RVA: 0x0013AB44 File Offset: 0x00138D44
	public int NumberOfPastEvents(HashedString eventID)
	{
		int result;
		this.pastEvents.TryGetValue(eventID, out result);
		return result;
	}

	// Token: 0x0600389B RID: 14491 RVA: 0x0013AB64 File Offset: 0x00138D64
	public static Notification CreateStandardCancelledNotification(EventInfoData eventInfoData)
	{
		if (eventInfoData == null)
		{
			DebugUtil.LogWarningArgs(new object[]
			{
				"eventPopup is null in CreateStandardCancelledNotification"
			});
			return null;
		}
		eventInfoData.FinalizeText();
		return new Notification(string.Format(GAMEPLAY_EVENTS.CANCELED, eventInfoData.title), NotificationType.Event, (List<Notification> list, object data) => string.Format(GAMEPLAY_EVENTS.CANCELED_TOOLTIP, eventInfoData.title), null, true, 0f, null, null, null, true, false, false);
	}

	// Token: 0x04002585 RID: 9605
	public static GameplayEventManager Instance;

	// Token: 0x04002586 RID: 9606
	public Notifier notifier;

	// Token: 0x04002587 RID: 9607
	[Serialize]
	private List<GameplayEventInstance> activeEvents = new List<GameplayEventInstance>();

	// Token: 0x04002588 RID: 9608
	[Serialize]
	private Dictionary<HashedString, int> pastEvents = new Dictionary<HashedString, int>();

	// Token: 0x04002589 RID: 9609
	[Serialize]
	private Dictionary<HashedString, float> sleepTimers = new Dictionary<HashedString, float>();
}
