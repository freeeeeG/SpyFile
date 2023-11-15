using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200015F RID: 351
public abstract class ServerTimedQueue : ServerSynchroniserBase, ITriggerReceiver
{
	// Token: 0x06000630 RID: 1584 RVA: 0x0002C301 File Offset: 0x0002A701
	public override EntityType GetEntityType()
	{
		return EntityType.TimedQueue;
	}

	// Token: 0x06000631 RID: 1585 RVA: 0x0002C308 File Offset: 0x0002A708
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_timedQueue = (TimedQueue)synchronisedObject;
		if (this.m_timedQueue.m_startOnAwake && base.gameObject.activeInHierarchy)
		{
			this.m_iFlowController = GameUtils.GetFlowController();
			if (this.m_iFlowController != null)
			{
				this.m_iFlowController.RoundActivatedCallback += this.OnRoundBegun;
				this.m_intitialised = false;
			}
		}
		else
		{
			this.m_intitialised = true;
		}
	}

	// Token: 0x06000632 RID: 1586 RVA: 0x0002C388 File Offset: 0x0002A788
	private void SendEventForTime(int _index, float _time)
	{
		this.m_data.m_msgType = TimedQueueMessage.MsgType.QueueEvent;
		this.m_data.m_index = _index;
		this.m_data.m_time = _time;
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06000633 RID: 1587 RVA: 0x0002C3BA File Offset: 0x0002A7BA
	private void SendCancelEvents()
	{
		this.m_data.m_msgType = TimedQueueMessage.MsgType.Cancel;
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06000634 RID: 1588 RVA: 0x0002C3D4 File Offset: 0x0002A7D4
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_iFlowController != null)
		{
			this.m_iFlowController.RoundActivatedCallback -= this.OnRoundBegun;
		}
	}

	// Token: 0x06000635 RID: 1589 RVA: 0x0002C3FE File Offset: 0x0002A7FE
	private void OnRoundBegun()
	{
		this.m_intitialised = true;
	}

	// Token: 0x06000636 RID: 1590 RVA: 0x0002C407 File Offset: 0x0002A807
	protected virtual void Start()
	{
	}

	// Token: 0x06000637 RID: 1591 RVA: 0x0002C40C File Offset: 0x0002A80C
	public override void UpdateSynchronising()
	{
		if (this.m_timedQueue == null)
		{
			return;
		}
		if (this.m_intitialised && this.m_timedQueue.m_startOnAwake)
		{
			this.m_nextIndex = 0;
			this.m_delayTimer = this.m_timedQueue.GetDelay(this.m_nextIndex);
			this.m_timedQueue.m_startOnAwake = false;
			this.SendEventForTime(this.m_nextIndex, ClientTime.Time() + this.m_delayTimer);
		}
		if (this.m_delayTimer >= 0f)
		{
			this.m_delayTimer -= TimeManager.GetDeltaTime(base.gameObject);
			if (this.m_delayTimer < 0f)
			{
				this.DoEvent(this.m_nextIndex);
			}
		}
	}

	// Token: 0x06000638 RID: 1592 RVA: 0x0002C4CC File Offset: 0x0002A8CC
	protected virtual void DoEvent(int _index)
	{
	}

	// Token: 0x06000639 RID: 1593 RVA: 0x0002C4D0 File Offset: 0x0002A8D0
	protected void AdvanceQueue()
	{
		this.m_nextIndex++;
		if ((float)this.m_nextIndex < this.m_timedQueue.GetQueueLength())
		{
			this.m_delayTimer = this.m_timedQueue.GetDelay(this.m_nextIndex);
			this.SendEventForTime(this.m_nextIndex, ClientTime.Time() + this.m_delayTimer);
		}
		else
		{
			if (this.m_timedQueue.m_endTrigger != string.Empty)
			{
				GameObject target = base.gameObject;
				if (this.m_timedQueue.m_endTriggerTarget != null)
				{
					target = this.m_timedQueue.m_endTriggerTarget;
				}
				target.SendTrigger(this.m_timedQueue.m_endTrigger);
			}
			if (this.m_timedQueue.m_loopWhenFinished)
			{
				this.m_nextIndex = 0;
				this.m_delayTimer = this.m_timedQueue.m_loopDelay;
				this.SendEventForTime(this.m_nextIndex, ClientTime.Time() + this.m_delayTimer);
			}
			else
			{
				this.m_nextIndex = -1;
				this.m_delayTimer = -1f;
			}
		}
	}

	// Token: 0x0600063A RID: 1594 RVA: 0x0002C5E1 File Offset: 0x0002A9E1
	protected void ResetQueue()
	{
		this.m_nextIndex = -1;
		this.m_delayTimer = -1f;
		this.SendCancelEvents();
	}

	// Token: 0x0600063B RID: 1595 RVA: 0x0002C5FB File Offset: 0x0002A9FB
	protected bool IsActive()
	{
		return this.m_nextIndex >= 0;
	}

	// Token: 0x0600063C RID: 1596 RVA: 0x0002C60C File Offset: 0x0002AA0C
	public void OnTrigger(string _trigger)
	{
		if (this.m_timedQueue.m_startTrigger == _trigger)
		{
			this.m_nextIndex = 0;
			this.m_delayTimer = this.m_timedQueue.GetDelay(this.m_nextIndex);
			this.SendEventForTime(this.m_nextIndex, ClientTime.Time() + this.m_delayTimer);
		}
		if (this.m_timedQueue.m_cancelTrigger == _trigger)
		{
			this.m_nextIndex = -1;
			this.m_delayTimer = -1f;
			this.SendCancelEvents();
		}
	}

	// Token: 0x04000529 RID: 1321
	private TimedQueue m_timedQueue;

	// Token: 0x0400052A RID: 1322
	private TimedQueueMessage m_data = new TimedQueueMessage();

	// Token: 0x0400052B RID: 1323
	private IFlowController m_iFlowController;

	// Token: 0x0400052C RID: 1324
	private bool m_intitialised;

	// Token: 0x0400052D RID: 1325
	private float m_delayTimer = -1f;

	// Token: 0x0400052E RID: 1326
	private int m_nextIndex = -1;
}
