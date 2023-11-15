using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020003E2 RID: 994
[SerializationConfig(MemberSerialization.OptIn)]
public class GameplayEventInstance : ISaveLoadable
{
	// Token: 0x17000081 RID: 129
	// (get) Token: 0x060014F1 RID: 5361 RVA: 0x0006E946 File Offset: 0x0006CB46
	// (set) Token: 0x060014F2 RID: 5362 RVA: 0x0006E94E File Offset: 0x0006CB4E
	public StateMachine.Instance smi { get; private set; }

	// Token: 0x17000082 RID: 130
	// (get) Token: 0x060014F3 RID: 5363 RVA: 0x0006E957 File Offset: 0x0006CB57
	// (set) Token: 0x060014F4 RID: 5364 RVA: 0x0006E95F File Offset: 0x0006CB5F
	public bool seenNotification
	{
		get
		{
			return this._seenNotification;
		}
		set
		{
			this._seenNotification = value;
			this.monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(-1122598290, this);
			});
		}
	}

	// Token: 0x17000083 RID: 131
	// (get) Token: 0x060014F5 RID: 5365 RVA: 0x0006E97F File Offset: 0x0006CB7F
	public GameplayEvent gameplayEvent
	{
		get
		{
			if (this._gameplayEvent == null)
			{
				this._gameplayEvent = Db.Get().GameplayEvents.TryGet(this.eventID);
			}
			return this._gameplayEvent;
		}
	}

	// Token: 0x060014F6 RID: 5366 RVA: 0x0006E9AA File Offset: 0x0006CBAA
	public GameplayEventInstance(GameplayEvent gameplayEvent, int worldId)
	{
		this.eventID = gameplayEvent.Id;
		this.tags = new List<Tag>();
		this.eventStartTime = GameUtil.GetCurrentTimeInCycles();
		this.worldId = worldId;
	}

	// Token: 0x060014F7 RID: 5367 RVA: 0x0006E9E0 File Offset: 0x0006CBE0
	public StateMachine.Instance PrepareEvent(GameplayEventManager manager)
	{
		this.smi = this.gameplayEvent.GetSMI(manager, this);
		return this.smi;
	}

	// Token: 0x060014F8 RID: 5368 RVA: 0x0006E9FC File Offset: 0x0006CBFC
	public void StartEvent()
	{
		GameplayEventManager.Instance.Trigger(1491341646, this);
		StateMachine.Instance smi = this.smi;
		smi.OnStop = (Action<string, StateMachine.Status>)Delegate.Combine(smi.OnStop, new Action<string, StateMachine.Status>(this.OnStop));
		this.smi.StartSM();
	}

	// Token: 0x060014F9 RID: 5369 RVA: 0x0006EA4B File Offset: 0x0006CC4B
	public void RegisterMonitorCallback(GameObject go)
	{
		if (this.monitorCallbackObjects == null)
		{
			this.monitorCallbackObjects = new List<GameObject>();
		}
		if (!this.monitorCallbackObjects.Contains(go))
		{
			this.monitorCallbackObjects.Add(go);
		}
	}

	// Token: 0x060014FA RID: 5370 RVA: 0x0006EA7A File Offset: 0x0006CC7A
	public void UnregisterMonitorCallback(GameObject go)
	{
		if (this.monitorCallbackObjects == null)
		{
			this.monitorCallbackObjects = new List<GameObject>();
		}
		this.monitorCallbackObjects.Remove(go);
	}

	// Token: 0x060014FB RID: 5371 RVA: 0x0006EA9C File Offset: 0x0006CC9C
	public void OnStop(string reason, StateMachine.Status status)
	{
		GameplayEventManager.Instance.Trigger(1287635015, this);
		if (this.monitorCallbackObjects != null)
		{
			this.monitorCallbackObjects.ForEach(delegate(GameObject x)
			{
				x.Trigger(1287635015, this);
			});
		}
		if (status == StateMachine.Status.Success)
		{
			using (List<HashedString>.Enumerator enumerator = this.gameplayEvent.successEvents.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HashedString hashedString = enumerator.Current;
					GameplayEvent gameplayEvent = Db.Get().GameplayEvents.TryGet(hashedString);
					DebugUtil.DevAssert(gameplayEvent != null, string.Format("GameplayEvent {0} is null", hashedString), null);
					if (gameplayEvent != null && gameplayEvent.IsAllowed())
					{
						GameplayEventManager.Instance.StartNewEvent(gameplayEvent, -1, null);
					}
				}
				return;
			}
		}
		if (status == StateMachine.Status.Failed)
		{
			foreach (HashedString hashedString2 in this.gameplayEvent.failureEvents)
			{
				GameplayEvent gameplayEvent2 = Db.Get().GameplayEvents.TryGet(hashedString2);
				DebugUtil.DevAssert(gameplayEvent2 != null, string.Format("GameplayEvent {0} is null", hashedString2), null);
				if (gameplayEvent2 != null && gameplayEvent2.IsAllowed())
				{
					GameplayEventManager.Instance.StartNewEvent(gameplayEvent2, -1, null);
				}
			}
		}
	}

	// Token: 0x060014FC RID: 5372 RVA: 0x0006EBF4 File Offset: 0x0006CDF4
	public float AgeInCycles()
	{
		return GameUtil.GetCurrentTimeInCycles() - this.eventStartTime;
	}

	// Token: 0x04000B57 RID: 2903
	[Serialize]
	public readonly HashedString eventID;

	// Token: 0x04000B58 RID: 2904
	[Serialize]
	public List<Tag> tags;

	// Token: 0x04000B59 RID: 2905
	[Serialize]
	public float eventStartTime;

	// Token: 0x04000B5A RID: 2906
	[Serialize]
	public readonly int worldId;

	// Token: 0x04000B5B RID: 2907
	[Serialize]
	private bool _seenNotification;

	// Token: 0x04000B5C RID: 2908
	public List<GameObject> monitorCallbackObjects;

	// Token: 0x04000B5D RID: 2909
	public GameplayEventInstance.GameplayEventPopupDataCallback GetEventPopupData;

	// Token: 0x04000B5E RID: 2910
	private GameplayEvent _gameplayEvent;

	// Token: 0x02001053 RID: 4179
	// (Invoke) Token: 0x06007556 RID: 30038
	public delegate EventInfoData GameplayEventPopupDataCallback();
}
