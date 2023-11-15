using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000885 RID: 2181
public class NetworkMessageTracker : DebugDisplay
{
	// Token: 0x06002A4B RID: 10827 RVA: 0x000C5C24 File Offset: 0x000C4024
	public override void OnSetUp()
	{
		this.InitList(this.m_SentGlobalEvents, this.m_SentGlobalEventCounts, 42);
		this.InitList(this.m_ReceivedGlobalEvents, this.m_ReceivedGlobalEventCounts, 42);
		this.InitList(this.m_SentEntityUpdates, this.m_SentEntityUpdateCounts, 65);
		this.InitList(this.m_ReceivedEntityUpdates, this.m_ReceivedEntityUpdateCounts, 65);
		this.InitList(this.m_SentEntityEvents, this.m_SentEntityEventCounts, 65);
		this.InitList(this.m_ReceivedEntityEvents, this.m_ReceivedEntityEventCounts, 65);
		this.InitList(this.m_SentMessageBatches, this.m_SentMessageBatchCounts, 2);
		this.InitList(this.m_ReceivedMessageBatches, this.m_ReceivedMessageBatchCounts, 2);
		this.m_OnlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
	}

	// Token: 0x06002A4C RID: 10828 RVA: 0x000C5CDC File Offset: 0x000C40DC
	private void InitList(FastList<FastList<float>> list, FastList<int> counts, int count)
	{
		for (int i = 0; i < count; i++)
		{
			list.Add(new FastList<float>());
			counts.Add(0);
		}
	}

	// Token: 0x06002A4D RID: 10829 RVA: 0x000C5D10 File Offset: 0x000C4110
	private void InitList(FastList<FastList<NetworkMessageTracker.BatchCount>> list, FastList<int> counts, int count)
	{
		for (int i = 0; i < count; i++)
		{
			list.Add(new FastList<NetworkMessageTracker.BatchCount>());
			counts.Add(0);
		}
	}

	// Token: 0x06002A4E RID: 10830 RVA: 0x000C5D44 File Offset: 0x000C4144
	public override void OnUpdate()
	{
		if (this.m_fNextUpdate < Time.time)
		{
			float time = Time.time;
			this.RemoveOldEntriesFromList(this.m_SentGlobalEvents, this.m_SentGlobalEventCounts, time);
			this.RemoveOldEntriesFromList(this.m_ReceivedGlobalEvents, this.m_ReceivedGlobalEventCounts, time);
			this.RemoveOldEntriesFromList(this.m_SentEntityUpdates, this.m_SentEntityUpdateCounts, time);
			this.RemoveOldEntriesFromList(this.m_ReceivedEntityUpdates, this.m_ReceivedEntityUpdateCounts, time);
			this.RemoveOldEntriesFromList(this.m_SentEntityEvents, this.m_SentEntityEventCounts, time);
			this.RemoveOldEntriesFromList(this.m_ReceivedEntityEvents, this.m_ReceivedEntityEventCounts, time);
			this.RemoveOldEntriesFromList(this.m_SentMessageBatches, this.m_SentMessageBatchCounts, time);
			this.RemoveOldEntriesFromList(this.m_ReceivedMessageBatches, this.m_ReceivedMessageBatchCounts, time);
			this.m_SentGlobalTotals._items[0] = this.GetTotal(this.m_SentMessageBatches._items[0]);
			this.m_ReceivedGlobalTotals._items[0] = this.GetTotal(this.m_ReceivedMessageBatches._items[0]);
			this.m_SentGlobalTotals._items[1] = this.GetTotal(this.m_SentMessageBatches._items[1]);
			this.m_ReceivedGlobalTotals._items[1] = this.GetTotal(this.m_ReceivedMessageBatches._items[1]);
			this.m_fNextUpdate = Time.time + 1f;
		}
	}

	// Token: 0x06002A4F RID: 10831 RVA: 0x000C5E90 File Offset: 0x000C4290
	private int GetTotal(FastList<NetworkMessageTracker.BatchCount> eventList)
	{
		int num = 0;
		int count = eventList.Count;
		for (int i = 0; i < count; i++)
		{
			num += eventList._items[i].iCount;
		}
		return num;
	}

	// Token: 0x06002A50 RID: 10832 RVA: 0x000C5ECC File Offset: 0x000C42CC
	private void RemoveOldEntriesFromList(FastList<FastList<float>> list, FastList<int> counts, float fCurrentTime)
	{
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			FastList<float> fastList = list._items[i];
			fastList.RemoveAll((float x) => x + 1f < fCurrentTime);
			counts._items[i] = fastList.Count;
		}
	}

	// Token: 0x06002A51 RID: 10833 RVA: 0x000C5F2C File Offset: 0x000C432C
	private void RemoveOldEntriesFromList(FastList<FastList<NetworkMessageTracker.BatchCount>> list, FastList<int> counts, float fCurrentTime)
	{
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			FastList<NetworkMessageTracker.BatchCount> fastList = list._items[i];
			fastList.RemoveAll((NetworkMessageTracker.BatchCount x) => x.time + 1f < fCurrentTime);
			counts._items[i] = fastList.Count;
		}
	}

	// Token: 0x06002A52 RID: 10834 RVA: 0x000C5F8C File Offset: 0x000C438C
	public override void OnDraw(ref Rect rect, GUIStyle style)
	{
		IOnlineMultiplayerTransportStats onlineMultiplayerTransportStats = null;
		if (this.m_OnlinePlatformManager != null)
		{
			onlineMultiplayerTransportStats = this.m_OnlinePlatformManager.OnlineMultiplayerTransportStats();
		}
		if (onlineMultiplayerTransportStats != null)
		{
			base.DrawText(ref rect, style, onlineMultiplayerTransportStats.Text);
		}
		this.AppendSentAndReceivedTotalsText(this.m_SentGlobalTotals, this.m_ReceivedGlobalTotals, this.m_SentMessageBatchCounts, this.m_ReceivedMessageBatchCounts, "Network sends/receives per second", delegate(int x)
		{
			NetworkMessageTracker.MessageBatchType messageBatchType = (NetworkMessageTracker.MessageBatchType)x;
			return messageBatchType.ToString();
		}, ref rect, style);
		this.AppendSentAndReceivedText(this.m_SentGlobalEventCounts, this.m_ReceivedGlobalEventCounts, "Game messages per second", delegate(int x)
		{
			MessageType messageType = (MessageType)x;
			return messageType.ToString();
		}, ref rect, style);
		this.AppendSentAndReceivedText(this.m_SentEntityUpdateCounts, this.m_ReceivedEntityUpdateCounts, "EntitySynchronisation breakdown", (int x) => ((EntityType)x).ToString(), ref rect, style);
		this.AppendSentAndReceivedText(this.m_SentEntityEventCounts, this.m_ReceivedEntityEventCounts, "EntityEvent breakdown", (int x) => ((EntityType)x).ToString(), ref rect, style);
	}

	// Token: 0x06002A53 RID: 10835 RVA: 0x000C60AC File Offset: 0x000C44AC
	private void AppendSentAndReceivedTotalsText(FastList<int> sentTotals, FastList<int> receivedTotals, FastList<int> sentList, FastList<int> receivedList, string title, Generic<string, int> toString, ref Rect rect, GUIStyle style)
	{
		int count = sentList.Count;
		base.DrawText(ref rect, style, title);
		for (int i = 0; i < count; i++)
		{
			int num = sentTotals._items[i];
			int num2 = receivedTotals._items[i];
			int num3 = sentList._items[i];
			int num4 = receivedList._items[i];
			base.DrawText(ref rect, style, string.Concat(new string[]
			{
				toString(i),
				": sent ",
				num.ToString("0000"),
				" in ",
				num3.ToString("0000"),
				" batches"
			}));
			base.DrawText(ref rect, style, string.Concat(new string[]
			{
				toString(i),
				": received ",
				num2.ToString("0000"),
				" in ",
				num4.ToString("0000"),
				" batches"
			}));
		}
	}

	// Token: 0x06002A54 RID: 10836 RVA: 0x000C61B4 File Offset: 0x000C45B4
	private void AppendSentAndReceivedText(FastList<int> sentList, FastList<int> receivedList, string title, Generic<string, int> toString, ref Rect rect, GUIStyle style)
	{
		int count = sentList.Count;
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			int num = sentList._items[i];
			int num2 = receivedList._items[i];
			if (num > 0 || num2 > 0)
			{
				if (!flag)
				{
					base.DrawText(ref rect, style, title);
					flag = true;
				}
				base.DrawText(ref rect, style, string.Concat(new string[]
				{
					toString(i),
					" Snt: ",
					num.ToString("0000"),
					" Rcvd: ",
					num2.ToString("0000")
				}));
			}
		}
	}

	// Token: 0x06002A55 RID: 10837 RVA: 0x000C625F File Offset: 0x000C465F
	public void TrackSentGlobalEvent(MessageType type)
	{
		this.m_SentGlobalEvents._items[(int)type].Add(Time.time);
	}

	// Token: 0x06002A56 RID: 10838 RVA: 0x000C6278 File Offset: 0x000C4678
	public void TrackReceivedGlobalEvent(MessageType type)
	{
		this.m_ReceivedGlobalEvents._items[(int)type].Add(Time.time);
	}

	// Token: 0x06002A57 RID: 10839 RVA: 0x000C6291 File Offset: 0x000C4691
	public void TrackSentEntityUpdate(EntityType entity)
	{
		this.m_SentEntityUpdates._items[(int)entity].Add(Time.time);
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x000C62AA File Offset: 0x000C46AA
	public void TrackReceivedEntityUpdate(EntityType entity)
	{
		this.m_ReceivedEntityUpdates._items[(int)entity].Add(Time.time);
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x000C62C3 File Offset: 0x000C46C3
	public void TrackSentEntityEvent(EntityType entity)
	{
		this.m_SentEntityEvents._items[(int)entity].Add(Time.time);
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x000C62DC File Offset: 0x000C46DC
	public void TrackReceivedEntityEvent(EntityType entity)
	{
		this.m_ReceivedEntityEvents._items[(int)entity].Add(Time.time);
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x000C62F5 File Offset: 0x000C46F5
	public void TrackSentMessageBatch(NetworkMessageTracker.MessageBatchType type, int iMessages)
	{
		this.m_SentMessageBatches._items[(int)type].Add(new NetworkMessageTracker.BatchCount(Time.time, iMessages));
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x000C6314 File Offset: 0x000C4714
	public void TrackReceivedMessageBatch(NetworkMessageTracker.MessageBatchType type, int iMessages)
	{
		this.m_ReceivedMessageBatches._items[(int)type].Add(new NetworkMessageTracker.BatchCount(Time.time, iMessages));
	}

	// Token: 0x04002150 RID: 8528
	private FastList<FastList<float>> m_SentGlobalEvents = new FastList<FastList<float>>();

	// Token: 0x04002151 RID: 8529
	private FastList<FastList<float>> m_ReceivedGlobalEvents = new FastList<FastList<float>>();

	// Token: 0x04002152 RID: 8530
	private FastList<FastList<float>> m_SentEntityUpdates = new FastList<FastList<float>>();

	// Token: 0x04002153 RID: 8531
	private FastList<FastList<float>> m_ReceivedEntityUpdates = new FastList<FastList<float>>();

	// Token: 0x04002154 RID: 8532
	private FastList<FastList<float>> m_SentEntityEvents = new FastList<FastList<float>>();

	// Token: 0x04002155 RID: 8533
	private FastList<FastList<float>> m_ReceivedEntityEvents = new FastList<FastList<float>>();

	// Token: 0x04002156 RID: 8534
	private FastList<FastList<NetworkMessageTracker.BatchCount>> m_SentMessageBatches = new FastList<FastList<NetworkMessageTracker.BatchCount>>();

	// Token: 0x04002157 RID: 8535
	private FastList<FastList<NetworkMessageTracker.BatchCount>> m_ReceivedMessageBatches = new FastList<FastList<NetworkMessageTracker.BatchCount>>();

	// Token: 0x04002158 RID: 8536
	private FastList<int> m_SentGlobalEventCounts = new FastList<int>(42);

	// Token: 0x04002159 RID: 8537
	private FastList<int> m_ReceivedGlobalEventCounts = new FastList<int>(42);

	// Token: 0x0400215A RID: 8538
	private FastList<int> m_SentEntityUpdateCounts = new FastList<int>(65);

	// Token: 0x0400215B RID: 8539
	private FastList<int> m_ReceivedEntityUpdateCounts = new FastList<int>(65);

	// Token: 0x0400215C RID: 8540
	private FastList<int> m_SentEntityEventCounts = new FastList<int>(65);

	// Token: 0x0400215D RID: 8541
	private FastList<int> m_ReceivedEntityEventCounts = new FastList<int>(65);

	// Token: 0x0400215E RID: 8542
	private FastList<int> m_SentMessageBatchCounts = new FastList<int>(2);

	// Token: 0x0400215F RID: 8543
	private FastList<int> m_ReceivedMessageBatchCounts = new FastList<int>(2);

	// Token: 0x04002160 RID: 8544
	private FastList<int> m_SentGlobalTotals = new FastList<int>(2);

	// Token: 0x04002161 RID: 8545
	private FastList<int> m_ReceivedGlobalTotals = new FastList<int>(2);

	// Token: 0x04002162 RID: 8546
	private float m_fNextUpdate;

	// Token: 0x04002163 RID: 8547
	private const float kFrameRate = 1f;

	// Token: 0x04002164 RID: 8548
	private const float kFrameDelay = 1f;

	// Token: 0x04002165 RID: 8549
	private IOnlinePlatformManager m_OnlinePlatformManager;

	// Token: 0x02000886 RID: 2182
	public enum MessageBatchType
	{
		// Token: 0x0400216B RID: 8555
		Reliable,
		// Token: 0x0400216C RID: 8556
		Unreliable,
		// Token: 0x0400216D RID: 8557
		COUNT
	}

	// Token: 0x02000887 RID: 2183
	public class BatchCount
	{
		// Token: 0x06002A61 RID: 10849 RVA: 0x000C63A9 File Offset: 0x000C47A9
		public BatchCount(float _time, int _iCount)
		{
			this.time = _time;
			this.iCount = _iCount;
		}

		// Token: 0x0400216E RID: 8558
		public float time;

		// Token: 0x0400216F RID: 8559
		public int iCount;
	}
}
