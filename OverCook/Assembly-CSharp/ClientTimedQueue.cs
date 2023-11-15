using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000160 RID: 352
public abstract class ClientTimedQueue : ClientSynchroniserBase
{
	// Token: 0x0600063E RID: 1598 RVA: 0x0002C6A6 File Offset: 0x0002AAA6
	public override EntityType GetEntityType()
	{
		return EntityType.TimedQueue;
	}

	// Token: 0x0600063F RID: 1599 RVA: 0x0002C6AA File Offset: 0x0002AAAA
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_timedQueue = (TimedQueue)synchronisedObject;
	}

	// Token: 0x06000640 RID: 1600 RVA: 0x0002C6C0 File Offset: 0x0002AAC0
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		TimedQueueMessage timedQueueMessage = (TimedQueueMessage)serialisable;
		TimedQueueMessage.MsgType msgType = timedQueueMessage.m_msgType;
		if (msgType != TimedQueueMessage.MsgType.QueueEvent)
		{
			if (msgType == TimedQueueMessage.MsgType.Cancel)
			{
				this.m_pendingEvents.Clear();
			}
		}
		else
		{
			this.m_pendingEvents.Add(new ClientTimedQueue.EventInfo(timedQueueMessage.m_index, timedQueueMessage.m_time));
		}
	}

	// Token: 0x06000641 RID: 1601 RVA: 0x0002C720 File Offset: 0x0002AB20
	public override void UpdateSynchronising()
	{
		float num = ClientTime.Time();
		if (this.m_pendingEvents.Count > 0)
		{
			for (int i = 0; i < this.m_pendingEvents.Count; i++)
			{
				if (this.m_pendingEvents[i].m_time <= num)
				{
					this.DoEvent(this.m_pendingEvents[i].m_index);
				}
			}
			for (int j = this.m_pendingEvents.Count - 1; j >= 0; j--)
			{
				if (this.m_pendingEvents[j].m_time <= num)
				{
					this.m_pendingEvents.RemoveAt(j);
				}
			}
		}
		if (TimeManager.IsPaused(base.gameObject))
		{
			for (int k = 0; k < this.m_pendingEvents.Count; k++)
			{
				ClientTimedQueue.EventInfo value = this.m_pendingEvents[k];
				value.m_time += ClientTime.DeltaTime();
				this.m_pendingEvents[k] = value;
			}
		}
	}

	// Token: 0x06000642 RID: 1602 RVA: 0x0002C840 File Offset: 0x0002AC40
	protected virtual void DoEvent(int _index)
	{
	}

	// Token: 0x0400052F RID: 1327
	protected TimedQueue m_timedQueue;

	// Token: 0x04000530 RID: 1328
	private List<ClientTimedQueue.EventInfo> m_pendingEvents = new List<ClientTimedQueue.EventInfo>();

	// Token: 0x02000161 RID: 353
	private struct EventInfo
	{
		// Token: 0x06000643 RID: 1603 RVA: 0x0002C842 File Offset: 0x0002AC42
		public EventInfo(int _index, float _time)
		{
			this.m_index = _index;
			this.m_time = _time;
		}

		// Token: 0x04000531 RID: 1329
		public int m_index;

		// Token: 0x04000532 RID: 1330
		public float m_time;
	}
}
