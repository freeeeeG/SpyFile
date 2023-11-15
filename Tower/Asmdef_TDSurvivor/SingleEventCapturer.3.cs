using System;

// Token: 0x02000105 RID: 261
public class SingleEventCapturer<T0, T1>
{
	// Token: 0x1700008A RID: 138
	// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001851C File Offset: 0x0001671C
	public bool IsEventReceived
	{
		get
		{
			return this.isEventReceived;
		}
	}

	// Token: 0x060006A2 RID: 1698 RVA: 0x00018524 File Offset: 0x00016724
	public SingleEventCapturer(Enum e, Action callback = null)
	{
		this.isEventRegistered = false;
		this.isEventReceived = false;
		this.eventEnum = e;
		this.callback = callback;
		this.RegisterEvent(e);
	}

	// Token: 0x060006A3 RID: 1699 RVA: 0x00018550 File Offset: 0x00016750
	~SingleEventCapturer()
	{
		if (this.isEventRegistered)
		{
			this.UnregisterEvent();
		}
	}

	// Token: 0x060006A4 RID: 1700 RVA: 0x00018584 File Offset: 0x00016784
	private void RegisterEvent(Enum e)
	{
		if (this.isEventRegistered)
		{
			return;
		}
		EventMgr.Register<T0, T1>(e, new Action<T0, T1>(this.OnReceiveEvent));
		this.isEventRegistered = true;
	}

	// Token: 0x060006A5 RID: 1701 RVA: 0x000185A8 File Offset: 0x000167A8
	private void OnReceiveEvent(T0 obj1, T1 obj2)
	{
		this.data0 = obj1;
		this.data1 = obj2;
		this.isEventReceived = true;
		Action action = this.callback;
		if (action != null)
		{
			action();
		}
		this.UnregisterEvent();
	}

	// Token: 0x060006A6 RID: 1702 RVA: 0x000185D6 File Offset: 0x000167D6
	public void UnregisterEvent()
	{
		if (!this.isEventRegistered)
		{
			return;
		}
		EventMgr.Remove<T0, T1>(this.eventEnum, new Action<T0, T1>(this.OnReceiveEvent));
		this.isEventRegistered = false;
	}

	// Token: 0x060006A7 RID: 1703 RVA: 0x000185FF File Offset: 0x000167FF
	public void GetData(out T0 out0, out T1 out1)
	{
		out0 = this.data0;
		out1 = this.data1;
	}

	// Token: 0x0400055D RID: 1373
	private bool isEventReceived;

	// Token: 0x0400055E RID: 1374
	private bool isEventRegistered;

	// Token: 0x0400055F RID: 1375
	private Action callback;

	// Token: 0x04000560 RID: 1376
	private T0 data0;

	// Token: 0x04000561 RID: 1377
	private T1 data1;

	// Token: 0x04000562 RID: 1378
	private Enum eventEnum;
}
