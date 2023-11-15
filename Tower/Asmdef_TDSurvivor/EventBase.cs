using System;
using System.Collections.Generic;

// Token: 0x020000ED RID: 237
public class EventBase : IEventBase
{
	// Token: 0x1700006C RID: 108
	// (get) Token: 0x060005EC RID: 1516 RVA: 0x000172A6 File Offset: 0x000154A6
	public int Count
	{
		get
		{
			return this.handlers.Count;
		}
	}

	// Token: 0x1700006D RID: 109
	// (get) Token: 0x060005ED RID: 1517 RVA: 0x000172B3 File Offset: 0x000154B3
	public bool IsEmpty
	{
		get
		{
			return this.Count == 0;
		}
	}

	// Token: 0x1700006E RID: 110
	// (get) Token: 0x060005EE RID: 1518 RVA: 0x000172BE File Offset: 0x000154BE
	// (set) Token: 0x060005EF RID: 1519 RVA: 0x000172C6 File Offset: 0x000154C6
	public uint SendEventCount { get; protected set; }

	// Token: 0x060005F0 RID: 1520 RVA: 0x000172CF File Offset: 0x000154CF
	public void Clear()
	{
		this.handlers.Clear();
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x000172DC File Offset: 0x000154DC
	public void Register(Action h)
	{
		this.handlers.Add(h);
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x000172EA File Offset: 0x000154EA
	public bool Remove(Action h)
	{
		bool flag = this.handlers.Contains(h);
		if (flag)
		{
			this.handlers.Remove(h);
		}
		return flag;
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x00017308 File Offset: 0x00015508
	public int SendEvent()
	{
		uint sendEventCount = this.SendEventCount + 1U;
		this.SendEventCount = sendEventCount;
		int num = 0;
		try
		{
			for (int i = this.handlers.Count - 1; i >= 0; i--)
			{
				Action action = this.handlers[i];
				if (action != null)
				{
					action();
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

	// Token: 0x04000543 RID: 1347
	public readonly List<Action> handlers = new List<Action>();
}
