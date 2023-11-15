using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020000EA RID: 234
public static class EventMgr
{
	// Token: 0x060005CC RID: 1484 RVA: 0x00016E82 File Offset: 0x00015082
	public static void Register(Enum key, Action act)
	{
		EventMgr.Ensure(key).Register(act);
	}

	// Token: 0x060005CD RID: 1485 RVA: 0x00016E90 File Offset: 0x00015090
	public static void Register<T0>(Enum key, Action<T0> act)
	{
		EventMgr.Ensure<T0>(key).Register(act);
	}

	// Token: 0x060005CE RID: 1486 RVA: 0x00016E9E File Offset: 0x0001509E
	public static void Register<T0, T1>(Enum key, Action<T0, T1> act)
	{
		EventMgr.Ensure<T0, T1>(key).Register(act);
	}

	// Token: 0x060005CF RID: 1487 RVA: 0x00016EAC File Offset: 0x000150AC
	public static void Register<T0, T1, T2>(Enum key, Action<T0, T1, T2> act)
	{
		EventMgr.Ensure<T0, T1, T2>(key).Register(act);
	}

	// Token: 0x060005D0 RID: 1488 RVA: 0x00016EBA File Offset: 0x000150BA
	public static void Register<T0, T1, T2, T3>(Enum key, Action<T0, T1, T2, T3> act)
	{
		EventMgr.Ensure<T0, T1, T2, T3>(key).Register(act);
	}

	// Token: 0x060005D1 RID: 1489 RVA: 0x00016EC8 File Offset: 0x000150C8
	public static void Remove(Enum key, Action act)
	{
		EventMgr.Ensure(key).Remove(act);
	}

	// Token: 0x060005D2 RID: 1490 RVA: 0x00016ED7 File Offset: 0x000150D7
	public static void Remove<T0>(Enum key, Action<T0> act)
	{
		EventMgr.Ensure<T0>(key).Remove(act);
	}

	// Token: 0x060005D3 RID: 1491 RVA: 0x00016EE6 File Offset: 0x000150E6
	public static void Remove<T0, T1>(Enum key, Action<T0, T1> act)
	{
		EventMgr.Ensure<T0, T1>(key).Remove(act);
	}

	// Token: 0x060005D4 RID: 1492 RVA: 0x00016EF5 File Offset: 0x000150F5
	public static void Remove<T0, T1, T2>(Enum key, Action<T0, T1, T2> act)
	{
		EventMgr.Ensure<T0, T1, T2>(key).Remove(act);
	}

	// Token: 0x060005D5 RID: 1493 RVA: 0x00016F04 File Offset: 0x00015104
	public static void Remove<T0, T1, T2, T3>(Enum key, Action<T0, T1, T2, T3> act)
	{
		EventMgr.Ensure<T0, T1, T2, T3>(key).Remove(act);
	}

	// Token: 0x060005D6 RID: 1494 RVA: 0x00016F14 File Offset: 0x00015114
	public static void Clear(Enum key)
	{
		IEventBase eventBase;
		if (EventMgr.s_events.TryGetValue(key, out eventBase))
		{
			eventBase.Clear();
			EventMgr.s_events.Remove(key);
			return;
		}
		EventMgrHelper.Log(string.Format("Clear: 找不到對應 {0}.{1} 的事件", key.GetType(), key));
	}

	// Token: 0x060005D7 RID: 1495 RVA: 0x00016F5C File Offset: 0x0001515C
	public static void PrintEvents()
	{
		Debug.Log("列印目前的事件-----");
		foreach (KeyValuePair<Enum, IEventBase> keyValuePair in EventMgr.s_events)
		{
			Debug.Log(string.Format("{0}", keyValuePair.Key));
		}
	}

	// Token: 0x060005D8 RID: 1496 RVA: 0x00016FC8 File Offset: 0x000151C8
	public static int SendEvent(Enum key)
	{
		EventBase eventBase = EventMgr.TryGet<EventBase>(key);
		if (eventBase == null)
		{
			return 0;
		}
		return eventBase.SendEvent();
	}

	// Token: 0x060005D9 RID: 1497 RVA: 0x00016FE8 File Offset: 0x000151E8
	public static int SendEvent<T0>(Enum key, T0 arg0)
	{
		EventBase<T0> eventBase = EventMgr.TryGet<EventBase<T0>>(key);
		if (eventBase == null)
		{
			return 0;
		}
		return eventBase.SendEvent(arg0);
	}

	// Token: 0x060005DA RID: 1498 RVA: 0x00017008 File Offset: 0x00015208
	public static int SendEvent<T0, T1>(Enum key, T0 arg0, T1 arg1)
	{
		EventBase<T0, T1> eventBase = EventMgr.TryGet<EventBase<T0, T1>>(key);
		if (eventBase == null)
		{
			return 0;
		}
		return eventBase.SendEvent(arg0, arg1);
	}

	// Token: 0x060005DB RID: 1499 RVA: 0x0001702C File Offset: 0x0001522C
	public static int SendEvent<T0, T1, T2>(Enum key, T0 arg0, T1 arg1, T2 arg2)
	{
		EventBase<T0, T1, T2> eventBase = EventMgr.TryGet<EventBase<T0, T1, T2>>(key);
		if (eventBase == null)
		{
			return 0;
		}
		return eventBase.SendEvent(arg0, arg1, arg2);
	}

	// Token: 0x060005DC RID: 1500 RVA: 0x00017050 File Offset: 0x00015250
	public static int SendEvent<T0, T1, T2, T3>(Enum key, T0 arg0, T1 arg1, T2 arg2, T3 arg3)
	{
		EventBase<T0, T1, T2, T3> eventBase = EventMgr.TryGet<EventBase<T0, T1, T2, T3>>(key);
		if (eventBase == null)
		{
			return 0;
		}
		return eventBase.SendEvent(arg0, arg1, arg2, arg3);
	}

	// Token: 0x060005DD RID: 1501 RVA: 0x00017074 File Offset: 0x00015274
	private static T TryGet<T>(Enum key) where T : IEventBase
	{
		IEventBase obj;
		if (EventMgr.s_events.TryGetValue(key, out obj))
		{
			return EventMgrHelper.SafeCast<T>(key, obj);
		}
		EventMgrHelper.Log(string.Format("TryGet: 找不到對應 {0}.{1} 的事件", key.GetType(), key));
		return default(T);
	}

	// Token: 0x060005DE RID: 1502 RVA: 0x000170B8 File Offset: 0x000152B8
	private static EventBase Ensure(Enum key)
	{
		IEventBase eventBase = null;
		if (!EventMgr.s_events.TryGetValue(key, out eventBase))
		{
			eventBase = new EventBase();
			EventMgr.s_events.Add(key, eventBase);
		}
		return EventMgrHelper.SafeCast<EventBase>(key, eventBase);
	}

	// Token: 0x060005DF RID: 1503 RVA: 0x000170F0 File Offset: 0x000152F0
	private static EventBase<T0> Ensure<T0>(Enum key)
	{
		IEventBase eventBase = null;
		if (!EventMgr.s_events.TryGetValue(key, out eventBase))
		{
			eventBase = new EventBase<T0>();
			EventMgr.s_events.Add(key, eventBase);
		}
		return EventMgrHelper.SafeCast<EventBase<T0>>(key, eventBase);
	}

	// Token: 0x060005E0 RID: 1504 RVA: 0x00017128 File Offset: 0x00015328
	private static EventBase<T0, T1> Ensure<T0, T1>(Enum key)
	{
		IEventBase eventBase = null;
		if (!EventMgr.s_events.TryGetValue(key, out eventBase))
		{
			eventBase = new EventBase<T0, T1>();
			EventMgr.s_events.Add(key, eventBase);
		}
		return EventMgrHelper.SafeCast<EventBase<T0, T1>>(key, eventBase);
	}

	// Token: 0x060005E1 RID: 1505 RVA: 0x00017160 File Offset: 0x00015360
	private static EventBase<T0, T1, T2> Ensure<T0, T1, T2>(Enum key)
	{
		IEventBase eventBase = null;
		if (!EventMgr.s_events.TryGetValue(key, out eventBase))
		{
			eventBase = new EventBase<T0, T1, T2>();
			EventMgr.s_events.Add(key, eventBase);
		}
		return EventMgrHelper.SafeCast<EventBase<T0, T1, T2>>(key, eventBase);
	}

	// Token: 0x060005E2 RID: 1506 RVA: 0x00017198 File Offset: 0x00015398
	private static EventBase<T0, T1, T2, T3> Ensure<T0, T1, T2, T3>(Enum key)
	{
		IEventBase eventBase = null;
		if (!EventMgr.s_events.TryGetValue(key, out eventBase))
		{
			eventBase = new EventBase<T0, T1, T2, T3>();
			EventMgr.s_events.Add(key, eventBase);
		}
		return EventMgrHelper.SafeCast<EventBase<T0, T1, T2, T3>>(key, eventBase);
	}

	// Token: 0x04000542 RID: 1346
	private static Dictionary<Enum, IEventBase> s_events = new Dictionary<Enum, IEventBase>();
}
