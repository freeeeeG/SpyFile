using System;

// Token: 0x02000104 RID: 260
public class SingleEventCapturer<T0>
{
	// Token: 0x17000089 RID: 137
	// (get) Token: 0x0600069A RID: 1690 RVA: 0x00018439 File Offset: 0x00016639
	public bool IsEventReceived
	{
		get
		{
			return this.isEventReceived;
		}
	}

	// Token: 0x0600069B RID: 1691 RVA: 0x00018441 File Offset: 0x00016641
	public SingleEventCapturer(Enum e, Action callback = null)
	{
		this.isEventRegistered = false;
		this.isEventReceived = false;
		this.eventEnum = e;
		this.callback = callback;
		this.RegisterEvent(e);
	}

	// Token: 0x0600069C RID: 1692 RVA: 0x0001846C File Offset: 0x0001666C
	~SingleEventCapturer()
	{
		if (this.isEventRegistered)
		{
			this.UnregisterEvent();
		}
	}

	// Token: 0x0600069D RID: 1693 RVA: 0x000184A0 File Offset: 0x000166A0
	private void RegisterEvent(Enum e)
	{
		if (this.isEventRegistered)
		{
			return;
		}
		EventMgr.Register<T0>(e, new Action<T0>(this.OnReceiveEvent));
		this.isEventRegistered = true;
	}

	// Token: 0x0600069E RID: 1694 RVA: 0x000184C4 File Offset: 0x000166C4
	private void OnReceiveEvent(T0 obj)
	{
		this.data = obj;
		this.isEventReceived = true;
		Action action = this.callback;
		if (action != null)
		{
			action();
		}
		this.UnregisterEvent();
	}

	// Token: 0x0600069F RID: 1695 RVA: 0x000184EB File Offset: 0x000166EB
	public void UnregisterEvent()
	{
		if (!this.isEventRegistered)
		{
			return;
		}
		EventMgr.Remove<T0>(this.eventEnum, new Action<T0>(this.OnReceiveEvent));
		this.isEventRegistered = false;
	}

	// Token: 0x060006A0 RID: 1696 RVA: 0x00018514 File Offset: 0x00016714
	public T0 GetData()
	{
		return this.data;
	}

	// Token: 0x04000558 RID: 1368
	private bool isEventReceived;

	// Token: 0x04000559 RID: 1369
	private bool isEventRegistered;

	// Token: 0x0400055A RID: 1370
	private Action callback;

	// Token: 0x0400055B RID: 1371
	private T0 data;

	// Token: 0x0400055C RID: 1372
	private Enum eventEnum;
}
