using System;

// Token: 0x02000103 RID: 259
public class SingleEventCapturer
{
	// Token: 0x17000088 RID: 136
	// (get) Token: 0x06000694 RID: 1684 RVA: 0x00018363 File Offset: 0x00016563
	public bool IsEventReceived
	{
		get
		{
			return this.isEventReceived;
		}
	}

	// Token: 0x06000695 RID: 1685 RVA: 0x0001836B File Offset: 0x0001656B
	public SingleEventCapturer(Enum e, Action callback = null)
	{
		this.isEventRegistered = false;
		this.isEventReceived = false;
		this.eventEnum = e;
		this.callback = callback;
		this.RegisterEvent(e);
	}

	// Token: 0x06000696 RID: 1686 RVA: 0x00018398 File Offset: 0x00016598
	~SingleEventCapturer()
	{
		if (this.isEventRegistered)
		{
			this.UnregisterEvent();
		}
	}

	// Token: 0x06000697 RID: 1687 RVA: 0x000183CC File Offset: 0x000165CC
	private void RegisterEvent(Enum e)
	{
		if (this.isEventRegistered)
		{
			return;
		}
		EventMgr.Register(e, new Action(this.OnReceiveEvent));
		this.isEventRegistered = true;
	}

	// Token: 0x06000698 RID: 1688 RVA: 0x000183F0 File Offset: 0x000165F0
	private void OnReceiveEvent()
	{
		this.isEventReceived = true;
		Action action = this.callback;
		if (action != null)
		{
			action();
		}
		this.UnregisterEvent();
	}

	// Token: 0x06000699 RID: 1689 RVA: 0x00018410 File Offset: 0x00016610
	public void UnregisterEvent()
	{
		if (!this.isEventRegistered)
		{
			return;
		}
		EventMgr.Remove(this.eventEnum, new Action(this.OnReceiveEvent));
		this.isEventRegistered = false;
	}

	// Token: 0x04000554 RID: 1364
	private bool isEventReceived;

	// Token: 0x04000555 RID: 1365
	private bool isEventRegistered;

	// Token: 0x04000556 RID: 1366
	private Action callback;

	// Token: 0x04000557 RID: 1367
	private Enum eventEnum;
}
