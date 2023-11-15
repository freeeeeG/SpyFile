using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000B83 RID: 2947
[AddComponentMenu("KMonoBehaviour/scripts/Messenger")]
public class Messenger : KMonoBehaviour
{
	// Token: 0x17000687 RID: 1671
	// (get) Token: 0x06005B85 RID: 23429 RVA: 0x00219285 File Offset: 0x00217485
	public int Count
	{
		get
		{
			return this.messages.Count;
		}
	}

	// Token: 0x06005B86 RID: 23430 RVA: 0x00219292 File Offset: 0x00217492
	public IEnumerator<Message> GetEnumerator()
	{
		return this.messages.GetEnumerator();
	}

	// Token: 0x06005B87 RID: 23431 RVA: 0x0021929F File Offset: 0x0021749F
	public static void DestroyInstance()
	{
		Messenger.Instance = null;
	}

	// Token: 0x17000688 RID: 1672
	// (get) Token: 0x06005B88 RID: 23432 RVA: 0x002192A7 File Offset: 0x002174A7
	public SerializedList<Message> Messages
	{
		get
		{
			return this.messages;
		}
	}

	// Token: 0x06005B89 RID: 23433 RVA: 0x002192AF File Offset: 0x002174AF
	protected override void OnPrefabInit()
	{
		Messenger.Instance = this;
	}

	// Token: 0x06005B8A RID: 23434 RVA: 0x002192B8 File Offset: 0x002174B8
	protected override void OnSpawn()
	{
		int i = 0;
		while (i < this.messages.Count)
		{
			if (this.messages[i].IsValid())
			{
				i++;
			}
			else
			{
				this.messages.RemoveAt(i);
			}
		}
		base.Trigger(-599791736, null);
	}

	// Token: 0x06005B8B RID: 23435 RVA: 0x00219308 File Offset: 0x00217508
	public void QueueMessage(Message message)
	{
		this.messages.Add(message);
		base.Trigger(1558809273, message);
	}

	// Token: 0x06005B8C RID: 23436 RVA: 0x00219324 File Offset: 0x00217524
	public Message DequeueMessage()
	{
		Message result = null;
		if (this.messages.Count > 0)
		{
			result = this.messages[0];
			this.messages.RemoveAt(0);
		}
		return result;
	}

	// Token: 0x06005B8D RID: 23437 RVA: 0x0021935C File Offset: 0x0021755C
	public void ClearAllMessages()
	{
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			this.messages.RemoveAt(i);
		}
	}

	// Token: 0x06005B8E RID: 23438 RVA: 0x0021938D File Offset: 0x0021758D
	public void RemoveMessage(Message m)
	{
		this.messages.Remove(m);
		base.Trigger(-599791736, null);
	}

	// Token: 0x04003DB2 RID: 15794
	[Serialize]
	private SerializedList<Message> messages = new SerializedList<Message>();

	// Token: 0x04003DB3 RID: 15795
	public static Messenger Instance;
}
