using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020008E2 RID: 2274
public class TeleportalMessage : Serialisable
{
	// Token: 0x06002C28 RID: 11304 RVA: 0x000CDACC File Offset: 0x000CBECC
	public void Initialise_State(bool _canTeleport, bool _isTeleporting)
	{
		this.m_msgType = TeleportalMessage.MsgType.PortalState;
		this.m_canTeleport = _canTeleport;
		this.m_isTeleporting = _isTeleporting;
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x000CDAE3 File Offset: 0x000CBEE3
	public void Initialise_StartTeleport(ITeleportalSender _sender, ITeleportalReceiver _receiver, ITeleportable _object)
	{
		this.m_msgType = TeleportalMessage.MsgType.TeleportStart;
		this.m_sender = _sender;
		this.m_receiver = _receiver;
		this.m_object = ((MonoBehaviour)_object).gameObject;
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x000CDB0B File Offset: 0x000CBF0B
	public void Initialise_EndTeleport(ITeleportalReceiver _receiver, ITeleportalSender _sender, ITeleportable _object)
	{
		this.m_msgType = TeleportalMessage.MsgType.TeleportEnd;
		this.m_sender = _sender;
		this.m_receiver = _receiver;
		this.m_object = ((MonoBehaviour)_object).gameObject;
	}

	// Token: 0x06002C2B RID: 11307 RVA: 0x000CDB34 File Offset: 0x000CBF34
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_msgType, 2);
		TeleportalMessage.MsgType msgType = this.m_msgType;
		if (msgType != TeleportalMessage.MsgType.PortalState)
		{
			if (msgType == TeleportalMessage.MsgType.TeleportStart || msgType == TeleportalMessage.MsgType.TeleportEnd)
			{
				this.SerialiseComponentByIndex<ITeleportalSender>(writer, this.m_sender as MonoBehaviour);
				this.SerialiseComponentByIndex<ITeleportalReceiver>(writer, this.m_receiver as MonoBehaviour);
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_object);
				entry.m_Header.Serialise(writer);
			}
		}
		else
		{
			writer.Write(this.m_canTeleport);
			writer.Write(this.m_isTeleporting);
		}
	}

	// Token: 0x06002C2C RID: 11308 RVA: 0x000CDBD0 File Offset: 0x000CBFD0
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_msgType = (TeleportalMessage.MsgType)reader.ReadUInt32(2);
		TeleportalMessage.MsgType msgType = this.m_msgType;
		if (msgType != TeleportalMessage.MsgType.PortalState)
		{
			if (msgType == TeleportalMessage.MsgType.TeleportStart || msgType == TeleportalMessage.MsgType.TeleportEnd)
			{
				this.m_clientSender = this.DeserialiseComponentByIndex<IClientTeleportalSender>(reader);
				this.m_clientReceiver = this.DeserialiseComponentByIndex<IClientTeleportalReceiver>(reader);
				this.m_entityHeader.Deserialise(reader);
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_entityHeader.m_uEntityID);
				this.m_object = entry.m_GameObject;
			}
		}
		else
		{
			this.m_canTeleport = reader.ReadBit();
			this.m_isTeleporting = reader.ReadBit();
		}
		return true;
	}

	// Token: 0x06002C2D RID: 11309 RVA: 0x000CDC74 File Offset: 0x000CC074
	private bool SerialiseComponentByIndex<T>(BitStreamWriter _writer, MonoBehaviour _component) where T : class
	{
		GameObject gameObject = _component.gameObject;
		int bits = gameObject.GetComponents<T>().FindIndex_Predicate((T x) => x == _component);
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(gameObject);
		if (entry != null)
		{
			entry.m_Header.Serialise(_writer);
			_writer.Write((uint)bits, 8);
			return true;
		}
		return false;
	}

	// Token: 0x06002C2E RID: 11310 RVA: 0x000CDCD8 File Offset: 0x000CC0D8
	private T DeserialiseComponentByIndex<T>(BitStreamReader reader) where T : class
	{
		this.m_entityHeader.Deserialise(reader);
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_entityHeader.m_uEntityID);
		GameObject gameObject = entry.m_GameObject;
		int index = (int)reader.ReadUInt32(8);
		return gameObject.GetComponents<T>().TryAtIndex(index);
	}

	// Token: 0x04002376 RID: 9078
	public const int kMsgTypeBits = 2;

	// Token: 0x04002377 RID: 9079
	public const int kComponentIndexBits = 8;

	// Token: 0x04002378 RID: 9080
	public TeleportalMessage.MsgType m_msgType;

	// Token: 0x04002379 RID: 9081
	public bool m_canTeleport;

	// Token: 0x0400237A RID: 9082
	public bool m_isTeleporting;

	// Token: 0x0400237B RID: 9083
	public ITeleportalSender m_sender;

	// Token: 0x0400237C RID: 9084
	public ITeleportalReceiver m_receiver;

	// Token: 0x0400237D RID: 9085
	public GameObject m_object;

	// Token: 0x0400237E RID: 9086
	public IClientTeleportalSender m_clientSender;

	// Token: 0x0400237F RID: 9087
	public IClientTeleportalReceiver m_clientReceiver;

	// Token: 0x04002380 RID: 9088
	private EntityMessageHeader m_entityHeader = new EntityMessageHeader();

	// Token: 0x020008E3 RID: 2275
	public enum MsgType
	{
		// Token: 0x04002382 RID: 9090
		PortalState,
		// Token: 0x04002383 RID: 9091
		TeleportStart,
		// Token: 0x04002384 RID: 9092
		TeleportEnd
	}
}
