using System;

// Token: 0x02000106 RID: 262
public class SingleEventCapturer<T0, T1, T2>
{
	// Token: 0x1700008B RID: 139
	// (get) Token: 0x060006A8 RID: 1704 RVA: 0x00018619 File Offset: 0x00016819
	public bool IsEventReceived
	{
		get
		{
			return this.isEventReceived;
		}
	}

	// Token: 0x060006A9 RID: 1705 RVA: 0x00018621 File Offset: 0x00016821
	public SingleEventCapturer(Enum e, Action callback = null)
	{
		this.isEventRegistered = false;
		this.isEventReceived = false;
		this.eventEnum = e;
		this.callback = callback;
		this.RegisterEvent(e);
	}

	// Token: 0x060006AA RID: 1706 RVA: 0x0001864C File Offset: 0x0001684C
	~SingleEventCapturer()
	{
		if (this.isEventRegistered)
		{
			this.UnregisterEvent();
		}
	}

	// Token: 0x060006AB RID: 1707 RVA: 0x00018680 File Offset: 0x00016880
	private void RegisterEvent(Enum e)
	{
		if (this.isEventRegistered)
		{
			return;
		}
		EventMgr.Register<T0, T1, T2>(e, new Action<T0, T1, T2>(this.OnReceiveEvent));
		this.isEventRegistered = true;
	}

	// Token: 0x060006AC RID: 1708 RVA: 0x000186A4 File Offset: 0x000168A4
	private void OnReceiveEvent(T0 obj0, T1 obj1, T2 obj2)
	{
		this.data0 = obj0;
		this.data1 = obj1;
		this.data2 = obj2;
		this.isEventReceived = true;
		Action action = this.callback;
		if (action != null)
		{
			action();
		}
		this.UnregisterEvent();
	}

	// Token: 0x060006AD RID: 1709 RVA: 0x000186D9 File Offset: 0x000168D9
	public void UnregisterEvent()
	{
		if (!this.isEventRegistered)
		{
			return;
		}
		EventMgr.Remove<T0, T1, T2>(this.eventEnum, new Action<T0, T1, T2>(this.OnReceiveEvent));
		this.isEventRegistered = false;
	}

	// Token: 0x060006AE RID: 1710 RVA: 0x00018702 File Offset: 0x00016902
	public void GetData(out T0 out0, out T1 out1, out T2 out2)
	{
		out0 = this.data0;
		out1 = this.data1;
		out2 = this.data2;
	}

	// Token: 0x04000563 RID: 1379
	private bool isEventReceived;

	// Token: 0x04000564 RID: 1380
	private bool isEventRegistered;

	// Token: 0x04000565 RID: 1381
	private Action callback;

	// Token: 0x04000566 RID: 1382
	private T0 data0;

	// Token: 0x04000567 RID: 1383
	private T1 data1;

	// Token: 0x04000568 RID: 1384
	private T2 data2;

	// Token: 0x04000569 RID: 1385
	private Enum eventEnum;
}
