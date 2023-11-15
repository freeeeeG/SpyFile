using System;
using System.Collections.Generic;

// Token: 0x020000F1 RID: 241
public class EventBase<T0, T1, T2, T3> : IEventBase
{
	// Token: 0x17000078 RID: 120
	// (get) Token: 0x06000610 RID: 1552 RVA: 0x0001763F File Offset: 0x0001583F
	public int Count
	{
		get
		{
			return this.handlers.Count;
		}
	}

	// Token: 0x17000079 RID: 121
	// (get) Token: 0x06000611 RID: 1553 RVA: 0x0001764C File Offset: 0x0001584C
	public bool IsEmpty
	{
		get
		{
			return this.Count == 0;
		}
	}

	// Token: 0x1700007A RID: 122
	// (get) Token: 0x06000612 RID: 1554 RVA: 0x00017657 File Offset: 0x00015857
	// (set) Token: 0x06000613 RID: 1555 RVA: 0x0001765F File Offset: 0x0001585F
	public uint SendEventCount { get; protected set; }

	// Token: 0x06000614 RID: 1556 RVA: 0x00017668 File Offset: 0x00015868
	public void Clear()
	{
		this.handlers.Clear();
	}

	// Token: 0x06000615 RID: 1557 RVA: 0x00017675 File Offset: 0x00015875
	public void Register(Action<T0, T1, T2, T3> h)
	{
		this.handlers.Add(h);
	}

	// Token: 0x06000616 RID: 1558 RVA: 0x00017683 File Offset: 0x00015883
	public bool Remove(Action<T0, T1, T2, T3> h)
	{
		bool flag = this.handlers.Contains(h);
		if (flag)
		{
			this.handlers.Remove(h);
		}
		return flag;
	}

	// Token: 0x06000617 RID: 1559 RVA: 0x000176A4 File Offset: 0x000158A4
	public int SendEvent(T0 p0, T1 p1, T2 p2, T3 p3)
	{
		uint sendEventCount = this.SendEventCount + 1U;
		this.SendEventCount = sendEventCount;
		int num = 0;
		try
		{
			for (int i = this.handlers.Count - 1; i >= 0; i--)
			{
				Action<T0, T1, T2, T3> action = this.handlers[i];
				if (action != null)
				{
					action(p0, p1, p2, p3);
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

	// Token: 0x0400054B RID: 1355
	public readonly List<Action<T0, T1, T2, T3>> handlers = new List<Action<T0, T1, T2, T3>>();
}
