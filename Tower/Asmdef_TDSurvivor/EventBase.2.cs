using System;
using System.Collections.Generic;

// Token: 0x020000EE RID: 238
public class EventBase<T0> : IEventBase
{
	// Token: 0x1700006F RID: 111
	// (get) Token: 0x060005F5 RID: 1525 RVA: 0x00017387 File Offset: 0x00015587
	public int Count
	{
		get
		{
			return this.handlers.Count;
		}
	}

	// Token: 0x17000070 RID: 112
	// (get) Token: 0x060005F6 RID: 1526 RVA: 0x00017394 File Offset: 0x00015594
	public bool IsEmpty
	{
		get
		{
			return this.Count == 0;
		}
	}

	// Token: 0x17000071 RID: 113
	// (get) Token: 0x060005F7 RID: 1527 RVA: 0x0001739F File Offset: 0x0001559F
	// (set) Token: 0x060005F8 RID: 1528 RVA: 0x000173A7 File Offset: 0x000155A7
	public uint SendEventCount { get; protected set; }

	// Token: 0x060005F9 RID: 1529 RVA: 0x000173B0 File Offset: 0x000155B0
	public void Clear()
	{
		this.handlers.Clear();
	}

	// Token: 0x060005FA RID: 1530 RVA: 0x000173BD File Offset: 0x000155BD
	public void Register(Action<T0> h)
	{
		this.handlers.Add(h);
	}

	// Token: 0x060005FB RID: 1531 RVA: 0x000173CB File Offset: 0x000155CB
	public bool Remove(Action<T0> h)
	{
		bool flag = this.handlers.Contains(h);
		if (flag)
		{
			this.handlers.Remove(h);
		}
		return flag;
	}

	// Token: 0x060005FC RID: 1532 RVA: 0x000173EC File Offset: 0x000155EC
	public int SendEvent(T0 p0)
	{
		uint sendEventCount = this.SendEventCount + 1U;
		this.SendEventCount = sendEventCount;
		int num = 0;
		try
		{
			for (int i = this.handlers.Count - 1; i >= 0; i--)
			{
				Action<T0> action = this.handlers[i];
				if (action != null)
				{
					action(p0);
				}
				num++;
			}
		}
		catch (Exception e)
		{
			EventMgrHelper.LogError(e);
		}
		return num;
	}

	// Token: 0x04000545 RID: 1349
	public readonly List<Action<T0>> handlers = new List<Action<T0>>();
}
