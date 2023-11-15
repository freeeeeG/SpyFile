using System;
using System.Collections.Generic;
using Team17.Online;
using Team17.Online.Multiplayer;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008CA RID: 2250
internal class Mailbox
{
	// Token: 0x06002BB3 RID: 11187 RVA: 0x000CC16C File Offset: 0x000CA56C
	public void Initialise(NetworkPeer peer)
	{
		IOnlinePlatformManager onlinePlatformManager = GameUtils.RequireManagerInterface<IOnlinePlatformManager>();
		this.m_SessionCoordinator = onlinePlatformManager.OnlineMultiplayerSessionCoordinator();
		for (int i = 0; i < 42; i++)
		{
			this.m_OrderedEvents.Add(new FastList<OrderedMessageReceivedCallback>());
		}
		this.ResolveSessionMembers();
		for (int j = 0; j < 42; j++)
		{
			this.m_UnorderedEvents.Add(new FastList<UnorderedMessageReceivedCallback>());
		}
		this.m_Peer = peer;
		this.m_Peer.OnMessageReceived += this.OnMessageReceived;
		ServerUserSystem.OnUserRemoved = (GenericVoid<User>)Delegate.Combine(ServerUserSystem.OnUserRemoved, new GenericVoid<User>(this.OnUserRemoved));
	}

	// Token: 0x06002BB4 RID: 11188 RVA: 0x000CC215 File Offset: 0x000CA615
	public void Shutdown()
	{
		this.m_Peer.OnMessageReceived -= this.OnMessageReceived;
		this.m_Peer = null;
		ServerUserSystem.OnUserRemoved = (GenericVoid<User>)Delegate.Remove(ServerUserSystem.OnUserRemoved, new GenericVoid<User>(this.OnUserRemoved));
	}

	// Token: 0x06002BB5 RID: 11189 RVA: 0x000CC258 File Offset: 0x000CA658
	public void Clear()
	{
		for (int i = 0; i < this.m_OrderedEvents.Count; i++)
		{
			this.m_OrderedEvents._items[i].Clear();
		}
		this.m_OrderedEventSequenceNumbers.Clear();
		for (int j = 0; j < 42; j++)
		{
			this.m_UnorderedEvents._items[j].Clear();
		}
	}

	// Token: 0x06002BB6 RID: 11190 RVA: 0x000CC2C4 File Offset: 0x000CA6C4
	private void OnUserRemoved(User user)
	{
		Mailbox.SequenceNumberInformation sequenceNumberInformation = this.FindSequenceInfo(user.SessionId);
		if (sequenceNumberInformation == null)
		{
			this.m_OrderedEventSequenceNumbers.Remove(sequenceNumberInformation);
		}
	}

	// Token: 0x06002BB7 RID: 11191 RVA: 0x000CC2F4 File Offset: 0x000CA6F4
	public void ResolveSessionMembers()
	{
		if (this.m_SessionCoordinator != null && this.m_SessionCoordinator.Members() != null)
		{
			foreach (IOnlineMultiplayerSessionUserId sessionUserId in this.m_SessionCoordinator.Members())
			{
				if (this.FindSequenceInfo(sessionUserId) == null)
				{
					Mailbox.SequenceNumberInformation sequenceNumberInformation = this.AddSequenceInfoForSessionUser(sessionUserId);
				}
			}
			this.toRemove.Clear();
			for (int j = 0; j < this.m_OrderedEventSequenceNumbers.Count; j++)
			{
				Mailbox.SequenceNumberInformation sequenceNumberInformation2 = this.m_OrderedEventSequenceNumbers._items[j];
				IOnlineMultiplayerSessionUserId[] array;
				if (!array.Contains(sequenceNumberInformation2.sessionUserId))
				{
					this.toRemove.Add(sequenceNumberInformation2);
				}
			}
			for (int k = 0; k < this.toRemove.Count; k++)
			{
				this.m_OrderedEventSequenceNumbers.Remove(this.toRemove._items[k]);
			}
		}
		else
		{
			this.m_OrderedEventSequenceNumbers.RemoveAll(this.HasValidSessionUser);
		}
	}

	// Token: 0x06002BB8 RID: 11192 RVA: 0x000CC404 File Offset: 0x000CA804
	private Mailbox.SequenceNumberInformation AddSequenceInfoForSessionUser(IOnlineMultiplayerSessionUserId sessionUserId)
	{
		Mailbox.SequenceNumberInformation sequenceNumberInformation = new Mailbox.SequenceNumberInformation();
		sequenceNumberInformation.sessionUserId = sessionUserId;
		for (int i = 0; i < 42; i++)
		{
			sequenceNumberInformation.sequenceNumbers._items[i] = -1;
		}
		this.m_OrderedEventSequenceNumbers.Add(sequenceNumberInformation);
		return sequenceNumberInformation;
	}

	// Token: 0x06002BB9 RID: 11193 RVA: 0x000CC44C File Offset: 0x000CA84C
	public void ResetSequenceNumbers()
	{
		this.m_OrderedEventSequenceNumbers.Clear();
	}

	// Token: 0x06002BBA RID: 11194 RVA: 0x000CC459 File Offset: 0x000CA859
	public void RegisterForMessageType(MessageType type, OrderedMessageReceivedCallback func)
	{
		this.m_OrderedEvents._items[(int)type].Add(func);
	}

	// Token: 0x06002BBB RID: 11195 RVA: 0x000CC470 File Offset: 0x000CA870
	public void UnregisterForMessageType(MessageType type, OrderedMessageReceivedCallback func)
	{
		FastList<OrderedMessageReceivedCallback> fastList = this.m_OrderedEvents._items[(int)type];
		if (fastList != null && fastList.Contains(func))
		{
			fastList.Remove(func);
		}
	}

	// Token: 0x06002BBC RID: 11196 RVA: 0x000CC4A5 File Offset: 0x000CA8A5
	public void RegisterForMessageType(MessageType type, UnorderedMessageReceivedCallback func)
	{
		this.m_UnorderedEvents._items[(int)type].Add(func);
	}

	// Token: 0x06002BBD RID: 11197 RVA: 0x000CC4BC File Offset: 0x000CA8BC
	public void UnregisterForMessageType(MessageType type, UnorderedMessageReceivedCallback func)
	{
		FastList<UnorderedMessageReceivedCallback> fastList = this.m_UnorderedEvents._items[(int)type];
		if (fastList != null && fastList.Contains(func))
		{
			fastList.Remove(func);
		}
	}

	// Token: 0x06002BBE RID: 11198 RVA: 0x000CC4F4 File Offset: 0x000CA8F4
	private Mailbox.SequenceNumberInformation FindSequenceInfo(IOnlineMultiplayerSessionUserId sessionUserId)
	{
		if (sessionUserId == null)
		{
			return null;
		}
		Mailbox.SequenceNumberInformation result = null;
		for (int i = 0; i < this.m_OrderedEventSequenceNumbers.Count; i++)
		{
			if (sessionUserId == this.m_OrderedEventSequenceNumbers._items[i].sessionUserId)
			{
				result = this.m_OrderedEventSequenceNumbers._items[i];
			}
		}
		return result;
	}

	// Token: 0x06002BBF RID: 11199 RVA: 0x000CC550 File Offset: 0x000CA950
	private void OnMessageReceived(IOnlineMultiplayerSessionUserId sessionUserId, MessageType type, Serialisable message, uint sequence, bool bReliable)
	{
		int iLastSequence = -1;
		Mailbox.SequenceNumberInformation sequenceNumberInformation = null;
		if (!bReliable && sessionUserId != null)
		{
			sequenceNumberInformation = this.FindSequenceInfo(sessionUserId);
			if (sequenceNumberInformation == null)
			{
				sequenceNumberInformation = this.AddSequenceInfoForSessionUser(sessionUserId);
			}
			iLastSequence = sequenceNumberInformation.sequenceNumbers._items[(int)type];
		}
		if (bReliable || sequenceNumberInformation == null || Mailbox.CheckSequenced(iLastSequence, (int)sequence))
		{
			if (sequenceNumberInformation != null)
			{
				sequenceNumberInformation.sequenceNumbers._items[(int)type] = (int)sequence;
			}
			FastList<OrderedMessageReceivedCallback> fastList = this.m_OrderedEvents._items[(int)type];
			for (int i = 0; i < fastList.Count; i++)
			{
				fastList._items[i](sessionUserId, message);
			}
		}
		FastList<UnorderedMessageReceivedCallback> fastList2 = this.m_UnorderedEvents._items[(int)type];
		for (int j = 0; j < fastList2.Count; j++)
		{
			fastList2._items[j](sessionUserId, message, sequence);
		}
	}

	// Token: 0x06002BC0 RID: 11200 RVA: 0x000CC638 File Offset: 0x000CAA38
	public static bool CheckSequenced(int iLastSequence, int iCurrentSequence)
	{
		if (iLastSequence == -1)
		{
			return true;
		}
		if (iLastSequence == iCurrentSequence)
		{
			return true;
		}
		if (iCurrentSequence >= iLastSequence)
		{
			if (iCurrentSequence < iLastSequence + 32767 && iCurrentSequence <= 65535)
			{
				return true;
			}
		}
		else if (iLastSequence > 32768 && iCurrentSequence < 32767 - (65535 - iLastSequence))
		{
			return true;
		}
		return false;
	}

	// Token: 0x040022ED RID: 8941
	public static Mailbox Client = new Mailbox();

	// Token: 0x040022EE RID: 8942
	public static Mailbox Server = new Mailbox();

	// Token: 0x040022EF RID: 8943
	private IOnlineMultiplayerSessionCoordinator m_SessionCoordinator;

	// Token: 0x040022F0 RID: 8944
	private Predicate<Mailbox.SequenceNumberInformation> HasValidSessionUser = (Mailbox.SequenceNumberInformation info) => info.sessionUserId != null;

	// Token: 0x040022F1 RID: 8945
	private FastList<FastList<OrderedMessageReceivedCallback>> m_OrderedEvents = new FastList<FastList<OrderedMessageReceivedCallback>>(42);

	// Token: 0x040022F2 RID: 8946
	private FastList<Mailbox.SequenceNumberInformation> m_OrderedEventSequenceNumbers = new FastList<Mailbox.SequenceNumberInformation>(4);

	// Token: 0x040022F3 RID: 8947
	private FastList<FastList<UnorderedMessageReceivedCallback>> m_UnorderedEvents = new FastList<FastList<UnorderedMessageReceivedCallback>>(42);

	// Token: 0x040022F4 RID: 8948
	private FastList<Mailbox.SequenceNumberInformation> toRemove = new FastList<Mailbox.SequenceNumberInformation>();

	// Token: 0x040022F5 RID: 8949
	private NetworkPeer m_Peer;

	// Token: 0x020008CB RID: 2251
	public class SequenceNumberInformation
	{
		// Token: 0x040022F7 RID: 8951
		public FastList<int> sequenceNumbers = new FastList<int>(42);

		// Token: 0x040022F8 RID: 8952
		public IOnlineMultiplayerSessionUserId sessionUserId;
	}
}
