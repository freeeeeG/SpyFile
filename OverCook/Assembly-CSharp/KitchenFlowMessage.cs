using System;
using BitStream;
using OrderController;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020008C2 RID: 2242
public class KitchenFlowMessage : Serialisable
{
	// Token: 0x06002B97 RID: 11159 RVA: 0x000CBC56 File Offset: 0x000CA056
	public void SetScoreData(TeamMonitor.TeamScoreStats _scoreData)
	{
		this.m_teamScore.Copy(_scoreData);
	}

	// Token: 0x06002B98 RID: 11160 RVA: 0x000CBC65 File Offset: 0x000CA065
	public void Initialise_DeliverySuccess(TeamID _teamID, GameObject _station, OrderID _orderID, float _timePropRemainingPercentage, int _tip, bool _wasCombo)
	{
		this.m_msgType = KitchenFlowMessage.MsgType.Delivery;
		this.m_teamID = _teamID;
		this.m_success = true;
		this.m_plateStation = _station;
		this.m_orderID = _orderID;
		this.m_wasCombo = _wasCombo;
		this.m_timePropRemainingPercentage = _timePropRemainingPercentage;
		this.m_tip = _tip;
	}

	// Token: 0x06002B99 RID: 11161 RVA: 0x000CBCA2 File Offset: 0x000CA0A2
	public void Initialise_DeliveryFailed(TeamID _teamID, GameObject _station)
	{
		this.m_msgType = KitchenFlowMessage.MsgType.Delivery;
		this.m_teamID = _teamID;
		this.m_success = false;
		this.m_plateStation = _station;
	}

	// Token: 0x06002B9A RID: 11162 RVA: 0x000CBCC0 File Offset: 0x000CA0C0
	public void Initialise_OrderAdded(TeamID _teamID, Serialisable _orderData)
	{
		this.m_msgType = KitchenFlowMessage.MsgType.OrderAdded;
		this.m_teamID = _teamID;
		this.m_orderData = (ServerOrderData)_orderData;
	}

	// Token: 0x06002B9B RID: 11163 RVA: 0x000CBCDC File Offset: 0x000CA0DC
	public void Initialise_OrderExpired(TeamID _teamID, OrderID _orderID)
	{
		this.m_msgType = KitchenFlowMessage.MsgType.OrderExpired;
		this.m_teamID = _teamID;
		this.m_orderID = _orderID;
	}

	// Token: 0x06002B9C RID: 11164 RVA: 0x000CBCF3 File Offset: 0x000CA0F3
	public void Initialise_ScoreOnly(TeamID _teamID)
	{
		this.m_msgType = KitchenFlowMessage.MsgType.ScoreOnly;
		this.m_teamID = _teamID;
	}

	// Token: 0x06002B9D RID: 11165 RVA: 0x000CBD04 File Offset: 0x000CA104
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_msgType, 2);
		writer.Write((uint)this.m_teamID, 2);
		switch (this.m_msgType)
		{
		case KitchenFlowMessage.MsgType.Delivery:
			writer.Write(this.m_success);
			this.m_teamScore.Serialise(writer);
			if (this.m_success)
			{
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_plateStation);
				entry.m_Header.Serialise(writer);
				this.m_orderID.Serialise(writer);
				writer.Write(this.m_wasCombo);
				writer.Write(this.m_timePropRemainingPercentage);
				writer.Write((uint)this.m_tip, 6);
			}
			break;
		case KitchenFlowMessage.MsgType.OrderAdded:
			this.m_orderData.Serialise(writer);
			break;
		case KitchenFlowMessage.MsgType.OrderExpired:
			this.m_orderID.Serialise(writer);
			this.m_teamScore.Serialise(writer);
			break;
		case KitchenFlowMessage.MsgType.ScoreOnly:
			this.m_teamScore.Serialise(writer);
			break;
		}
	}

	// Token: 0x06002B9E RID: 11166 RVA: 0x000CBE00 File Offset: 0x000CA200
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_msgType = (KitchenFlowMessage.MsgType)reader.ReadUInt32(2);
		this.m_teamID = (TeamID)reader.ReadUInt32(2);
		switch (this.m_msgType)
		{
		case KitchenFlowMessage.MsgType.Delivery:
			this.m_success = reader.ReadBit();
			this.m_teamScore.Deserialise(reader);
			if (this.m_success)
			{
				this.m_plateStationHeader.Deserialise(reader);
				EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(this.m_plateStationHeader.m_uEntityID);
				if (entry == null)
				{
					return false;
				}
				this.m_plateStation = entry.m_GameObject;
				this.m_orderID.Deserialise(reader);
				this.m_wasCombo = reader.ReadBit();
				this.m_timePropRemainingPercentage = reader.ReadFloat32();
				this.m_tip = (int)reader.ReadUInt32(6);
			}
			break;
		case KitchenFlowMessage.MsgType.OrderAdded:
			this.m_orderData.Deserialise(reader);
			break;
		case KitchenFlowMessage.MsgType.OrderExpired:
			this.m_orderID.Deserialise(reader);
			this.m_teamScore.Deserialise(reader);
			break;
		case KitchenFlowMessage.MsgType.ScoreOnly:
			this.m_teamScore.Deserialise(reader);
			break;
		}
		return true;
	}

	// Token: 0x040022CA RID: 8906
	public const int kMsgTypeBits = 2;

	// Token: 0x040022CB RID: 8907
	public const int kTeamIDBits = 2;

	// Token: 0x040022CC RID: 8908
	public const int kTipBits = 6;

	// Token: 0x040022CD RID: 8909
	public KitchenFlowMessage.MsgType m_msgType;

	// Token: 0x040022CE RID: 8910
	public TeamID m_teamID;

	// Token: 0x040022CF RID: 8911
	public TeamMonitor.TeamScoreStats m_teamScore = new TeamMonitor.TeamScoreStats();

	// Token: 0x040022D0 RID: 8912
	public OrderID m_orderID;

	// Token: 0x040022D1 RID: 8913
	public ServerOrderData m_orderData = new ServerOrderData();

	// Token: 0x040022D2 RID: 8914
	public GameObject m_plateStation;

	// Token: 0x040022D3 RID: 8915
	public bool m_success;

	// Token: 0x040022D4 RID: 8916
	public bool m_wasCombo;

	// Token: 0x040022D5 RID: 8917
	public int m_tip;

	// Token: 0x040022D6 RID: 8918
	public float m_timePropRemainingPercentage;

	// Token: 0x040022D7 RID: 8919
	private EntityMessageHeader m_plateStationHeader = new EntityMessageHeader();

	// Token: 0x020008C3 RID: 2243
	public enum MsgType
	{
		// Token: 0x040022D9 RID: 8921
		Delivery,
		// Token: 0x040022DA RID: 8922
		OrderAdded,
		// Token: 0x040022DB RID: 8923
		OrderExpired,
		// Token: 0x040022DC RID: 8924
		ScoreOnly
	}
}
