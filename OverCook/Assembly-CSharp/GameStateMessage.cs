using System;
using BitStream;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x020008B9 RID: 2233
public class GameStateMessage : Serialisable
{
	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06002B74 RID: 11124 RVA: 0x000CB62E File Offset: 0x000C9A2E
	// (set) Token: 0x06002B75 RID: 11125 RVA: 0x000CB636 File Offset: 0x000C9A36
	public GameStateMessage.GameStatePayload Payload { get; private set; }

	// Token: 0x06002B76 RID: 11126 RVA: 0x000CB63F File Offset: 0x000C9A3F
	public void Initialise(GameState state, User.MachineID machine)
	{
		this.Initialise(state, machine, null);
	}

	// Token: 0x06002B77 RID: 11127 RVA: 0x000CB64A File Offset: 0x000C9A4A
	public void Initialise(GameState state, User.MachineID machine, GameStateMessage.GameStatePayload payload)
	{
		this.m_State = state;
		this.m_Machine = machine;
		this.Payload = payload;
	}

	// Token: 0x06002B78 RID: 11128 RVA: 0x000CB664 File Offset: 0x000C9A64
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_State, 6);
		writer.Write((uint)this.m_Machine, 3);
		writer.Write(this.Payload != null);
		if (this.Payload != null)
		{
			writer.Write((uint)this.Payload.GetPayLoadType(), this.kBitsPayloadType);
			this.Payload.Serialise(writer);
		}
	}

	// Token: 0x06002B79 RID: 11129 RVA: 0x000CB6CC File Offset: 0x000C9ACC
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_State = (GameState)reader.ReadUInt32(6);
		this.m_Machine = (User.MachineID)reader.ReadUInt32(3);
		if (reader.ReadBit())
		{
			GameStateMessage.GameStatePayload payloadForType = this.GetPayloadForType((GameStateMessage.PayLoadType)reader.ReadUInt32(this.kBitsPayloadType));
			payloadForType.Deserialise(reader);
			this.Payload = payloadForType;
		}
		else
		{
			this.Payload = null;
		}
		return true;
	}

	// Token: 0x06002B7A RID: 11130 RVA: 0x000CB72D File Offset: 0x000C9B2D
	private GameStateMessage.GameStatePayload GetPayloadForType(GameStateMessage.PayLoadType type)
	{
		if (type != GameStateMessage.PayLoadType.ClientSave)
		{
			return null;
		}
		return new GameStateMessage.ClientSavePayload();
	}

	// Token: 0x040022A6 RID: 8870
	public const int kGameStateBits = 6;

	// Token: 0x040022A7 RID: 8871
	public GameState m_State;

	// Token: 0x040022A8 RID: 8872
	public User.MachineID m_Machine = User.MachineID.Count;

	// Token: 0x040022A9 RID: 8873
	private int kBitsPayloadType = GameUtils.GetRequiredBitCount(1);

	// Token: 0x020008BA RID: 2234
	public enum PayLoadType
	{
		// Token: 0x040022AC RID: 8876
		ClientSave,
		// Token: 0x040022AD RID: 8877
		COUNT
	}

	// Token: 0x020008BB RID: 2235
	public abstract class GameStatePayload : Serialisable
	{
		// Token: 0x06002B7C RID: 11132
		public abstract GameStateMessage.PayLoadType GetPayLoadType();

		// Token: 0x06002B7D RID: 11133
		public abstract void Serialise(BitStreamWriter writer);

		// Token: 0x06002B7E RID: 11134
		public abstract bool Deserialise(BitStreamReader reader);
	}

	// Token: 0x020008BC RID: 2236
	public class ClientSavePayload : GameStateMessage.GameStatePayload
	{
		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06002B80 RID: 11136 RVA: 0x000CB751 File Offset: 0x000C9B51
		// (set) Token: 0x06002B81 RID: 11137 RVA: 0x000CB759 File Offset: 0x000C9B59
		public int DLCID { get; private set; }

		// Token: 0x06002B82 RID: 11138 RVA: 0x000CB762 File Offset: 0x000C9B62
		public override GameStateMessage.PayLoadType GetPayLoadType()
		{
			return GameStateMessage.PayLoadType.ClientSave;
		}

		// Token: 0x06002B83 RID: 11139 RVA: 0x000CB765 File Offset: 0x000C9B65
		public void Initialise(int dlcID)
		{
			this.DLCID = dlcID;
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x000CB770 File Offset: 0x000C9B70
		public override void Serialise(BitStreamWriter writer)
		{
			bool flag = this.DLCID != -1;
			writer.Write(flag);
			if (flag)
			{
				writer.Write((uint)this.DLCID, 4);
			}
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x000CB7A4 File Offset: 0x000C9BA4
		public override bool Deserialise(BitStreamReader reader)
		{
			bool flag = reader.ReadBit();
			if (flag)
			{
				this.DLCID = (int)reader.ReadUInt32(4);
			}
			else
			{
				this.DLCID = -1;
			}
			return true;
		}
	}
}
