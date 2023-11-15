using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using KSerialization;

// Token: 0x020007B3 RID: 1971
[SerializationConfig(MemberSerialization.OptIn)]
public class EventLogger<EventInstanceType, EventType> : KMonoBehaviour, ISaveLoadable where EventInstanceType : EventInstanceBase where EventType : EventBase
{
	// Token: 0x0600369C RID: 13980 RVA: 0x00126F52 File Offset: 0x00125152
	public IEnumerator<EventInstanceType> GetEnumerator()
	{
		return this.EventInstances.GetEnumerator();
	}

	// Token: 0x0600369D RID: 13981 RVA: 0x00126F64 File Offset: 0x00125164
	public EventType AddEvent(EventType ev)
	{
		for (int i = 0; i < this.Events.Count; i++)
		{
			if (this.Events[i].hash == ev.hash)
			{
				this.Events[i] = ev;
				return this.Events[i];
			}
		}
		this.Events.Add(ev);
		return ev;
	}

	// Token: 0x0600369E RID: 13982 RVA: 0x00126FD1 File Offset: 0x001251D1
	public EventInstanceType Add(EventInstanceType ev)
	{
		if (this.EventInstances.Count > 10000)
		{
			this.EventInstances.RemoveAt(0);
		}
		this.EventInstances.Add(ev);
		return ev;
	}

	// Token: 0x0600369F RID: 13983 RVA: 0x00127000 File Offset: 0x00125200
	[OnDeserialized]
	protected internal void OnDeserialized()
	{
		if (this.EventInstances.Count > 10000)
		{
			this.EventInstances.RemoveRange(0, this.EventInstances.Count - 10000);
		}
		for (int i = 0; i < this.EventInstances.Count; i++)
		{
			for (int j = 0; j < this.Events.Count; j++)
			{
				if (this.Events[j].hash == this.EventInstances[i].eventHash)
				{
					this.EventInstances[i].ev = this.Events[j];
					break;
				}
			}
		}
	}

	// Token: 0x060036A0 RID: 13984 RVA: 0x001270BF File Offset: 0x001252BF
	public void Clear()
	{
		this.EventInstances.Clear();
	}

	// Token: 0x0400218A RID: 8586
	private const int MAX_NUM_EVENTS = 10000;

	// Token: 0x0400218B RID: 8587
	private List<EventType> Events = new List<EventType>();

	// Token: 0x0400218C RID: 8588
	[Serialize]
	private List<EventInstanceType> EventInstances = new List<EventInstanceType>();
}
