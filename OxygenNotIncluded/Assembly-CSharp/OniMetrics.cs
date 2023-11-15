using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A3A RID: 2618
public class OniMetrics : MonoBehaviour
{
	// Token: 0x06004ED2 RID: 20178 RVA: 0x001BC934 File Offset: 0x001BAB34
	private static void EnsureMetrics()
	{
		if (OniMetrics.Metrics != null)
		{
			return;
		}
		OniMetrics.Metrics = new List<Dictionary<string, object>>(2);
		for (int i = 0; i < 2; i++)
		{
			OniMetrics.Metrics.Add(null);
		}
	}

	// Token: 0x06004ED3 RID: 20179 RVA: 0x001BC96B File Offset: 0x001BAB6B
	public static void LogEvent(OniMetrics.Event eventType, string key, object data)
	{
		OniMetrics.EnsureMetrics();
		if (OniMetrics.Metrics[(int)eventType] == null)
		{
			OniMetrics.Metrics[(int)eventType] = new Dictionary<string, object>();
		}
		OniMetrics.Metrics[(int)eventType][key] = data;
	}

	// Token: 0x06004ED4 RID: 20180 RVA: 0x001BC9A4 File Offset: 0x001BABA4
	public static void SendEvent(OniMetrics.Event eventType, string debugName)
	{
		if (OniMetrics.Metrics[(int)eventType] == null || OniMetrics.Metrics[(int)eventType].Count == 0)
		{
			return;
		}
		ThreadedHttps<KleiMetrics>.Instance.SendEvent(OniMetrics.Metrics[(int)eventType], debugName);
		OniMetrics.Metrics[(int)eventType].Clear();
	}

	// Token: 0x04003346 RID: 13126
	private static List<Dictionary<string, object>> Metrics;

	// Token: 0x020018C7 RID: 6343
	public enum Event : short
	{
		// Token: 0x040072F4 RID: 29428
		NewSave,
		// Token: 0x040072F5 RID: 29429
		EndOfCycle,
		// Token: 0x040072F6 RID: 29430
		NumEvents
	}
}
