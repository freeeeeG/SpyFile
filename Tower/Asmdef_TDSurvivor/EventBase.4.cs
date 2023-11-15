using System;
using System.Collections.Generic;

// Token: 0x020000F0 RID: 240
public class EventBase<T0, T1, T2> : IEventBase
{
	// Token: 0x17000075 RID: 117
	// (get) Token: 0x06000607 RID: 1543 RVA: 0x00017557 File Offset: 0x00015757
	public int Count
	{
		get
		{
			return this.handlers.Count;
		}
	}

	// Token: 0x17000076 RID: 118
	// (get) Token: 0x06000608 RID: 1544 RVA: 0x00017564 File Offset: 0x00015764
	public bool IsEmpty
	{
		get
		{
			return this.Count == 0;
		}
	}

	// Token: 0x17000077 RID: 119
	// (get) Token: 0x06000609 RID: 1545 RVA: 0x0001756F File Offset: 0x0001576F
	// (set) Token: 0x0600060A RID: 1546 RVA: 0x00017577 File Offset: 0x00015777
	public uint SendEventCount { get; protected set; }

	// Token: 0x0600060B RID: 1547 RVA: 0x00017580 File Offset: 0x00015780
	public void Clear()
	{
		this.handlers.Clear();
	}

	// Token: 0x0600060C RID: 1548 RVA: 0x0001758D File Offset: 0x0001578D
	public void Register(Action<T0, T1, T2> h)
	{
		this.handlers.Add(h);
	}

	// Token: 0x0600060D RID: 1549 RVA: 0x0001759B File Offset: 0x0001579B
	public bool Remove(Action<T0, T1, T2> h)
	{
		bool flag = this.handlers.Contains(h);
		if (flag)
		{
			this.handlers.Remove(h);
		}
		return flag;
	}

	// Token: 0x0600060E RID: 1550 RVA: 0x000175BC File Offset: 0x000157BC
	public int SendEvent(T0 p0, T1 p1, T2 p2)
	{
		uint sendEventCount = this.SendEventCount + 1U;
		this.SendEventCount = sendEventCount;
		int num = 0;
		try
		{
			for (int i = this.handlers.Count - 1; i >= 0; i--)
			{
				Action<T0, T1, T2> action = this.handlers[i];
				if (action != null)
				{
					action(p0, p1, p2);
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

	// Token: 0x04000549 RID: 1353
	public readonly List<Action<T0, T1, T2>> handlers = new List<Action<T0, T1, T2>>();
}
