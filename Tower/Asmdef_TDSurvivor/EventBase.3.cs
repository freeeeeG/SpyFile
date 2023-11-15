using System;
using System.Collections.Generic;

// Token: 0x020000EF RID: 239
public class EventBase<T0, T1> : IEventBase
{
	// Token: 0x17000072 RID: 114
	// (get) Token: 0x060005FE RID: 1534 RVA: 0x0001746F File Offset: 0x0001566F
	public int Count
	{
		get
		{
			return this.handlers.Count;
		}
	}

	// Token: 0x17000073 RID: 115
	// (get) Token: 0x060005FF RID: 1535 RVA: 0x0001747C File Offset: 0x0001567C
	public bool IsEmpty
	{
		get
		{
			return this.Count == 0;
		}
	}

	// Token: 0x17000074 RID: 116
	// (get) Token: 0x06000600 RID: 1536 RVA: 0x00017487 File Offset: 0x00015687
	// (set) Token: 0x06000601 RID: 1537 RVA: 0x0001748F File Offset: 0x0001568F
	public uint SendEventCount { get; protected set; }

	// Token: 0x06000602 RID: 1538 RVA: 0x00017498 File Offset: 0x00015698
	public void Clear()
	{
		this.handlers.Clear();
	}

	// Token: 0x06000603 RID: 1539 RVA: 0x000174A5 File Offset: 0x000156A5
	public void Register(Action<T0, T1> h)
	{
		this.handlers.Add(h);
	}

	// Token: 0x06000604 RID: 1540 RVA: 0x000174B3 File Offset: 0x000156B3
	public bool Remove(Action<T0, T1> h)
	{
		bool flag = this.handlers.Contains(h);
		if (flag)
		{
			this.handlers.Remove(h);
		}
		return flag;
	}

	// Token: 0x06000605 RID: 1541 RVA: 0x000174D4 File Offset: 0x000156D4
	public int SendEvent(T0 p0, T1 p1)
	{
		uint sendEventCount = this.SendEventCount + 1U;
		this.SendEventCount = sendEventCount;
		int num = 0;
		try
		{
			for (int i = this.handlers.Count - 1; i >= 0; i--)
			{
				Action<T0, T1> action = this.handlers[i];
				if (action != null)
				{
					action(p0, p1);
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

	// Token: 0x04000547 RID: 1351
	public readonly List<Action<T0, T1>> handlers = new List<Action<T0, T1>>();
}
