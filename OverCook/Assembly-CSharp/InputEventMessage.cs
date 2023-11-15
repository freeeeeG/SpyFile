using System;
using BitStream;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020008C0 RID: 2240
public class InputEventMessage : Serialisable
{
	// Token: 0x06002B91 RID: 11153 RVA: 0x000CBB3C File Offset: 0x000C9F3C
	public InputEventMessage()
	{
	}

	// Token: 0x06002B92 RID: 11154 RVA: 0x000CBB44 File Offset: 0x000C9F44
	public InputEventMessage(InputEventMessage.InputEventType inputEventType)
	{
		this.m_inputEventType = inputEventType;
	}

	// Token: 0x17000339 RID: 825
	// (get) Token: 0x06002B93 RID: 11155 RVA: 0x000CBB53 File Offset: 0x000C9F53
	public InputEventMessage.InputEventType inputEventType
	{
		get
		{
			return this.m_inputEventType;
		}
	}

	// Token: 0x06002B94 RID: 11156 RVA: 0x000CBB5C File Offset: 0x000C9F5C
	public void Serialise(BitStreamWriter writer)
	{
		writer.Write((uint)this.m_inputEventType, 10);
		writer.Write(this.entityId, 10);
		if (this.m_inputEventType == InputEventMessage.InputEventType.DashCollision)
		{
			writer.Write(this.collisionContactPoint.x);
			writer.Write(this.collisionContactPoint.y);
			writer.Write(this.collisionContactPoint.z);
		}
	}

	// Token: 0x06002B95 RID: 11157 RVA: 0x000CBBC4 File Offset: 0x000C9FC4
	public bool Deserialise(BitStreamReader reader)
	{
		this.m_inputEventType = (InputEventMessage.InputEventType)reader.ReadUInt32(10);
		this.entityId = reader.ReadUInt32(10);
		if (this.m_inputEventType == InputEventMessage.InputEventType.DashCollision)
		{
			this.collisionContactPoint.x = reader.ReadFloat32();
			this.collisionContactPoint.y = reader.ReadFloat32();
			this.collisionContactPoint.z = reader.ReadFloat32();
		}
		return true;
	}

	// Token: 0x040022BD RID: 8893
	private InputEventMessage.InputEventType m_inputEventType;

	// Token: 0x040022BE RID: 8894
	public uint entityId;

	// Token: 0x040022BF RID: 8895
	public Vector3 collisionContactPoint;

	// Token: 0x020008C1 RID: 2241
	public enum InputEventType
	{
		// Token: 0x040022C1 RID: 8897
		Dash,
		// Token: 0x040022C2 RID: 8898
		DashCollision,
		// Token: 0x040022C3 RID: 8899
		Catch,
		// Token: 0x040022C4 RID: 8900
		Curse,
		// Token: 0x040022C5 RID: 8901
		BeginInteraction,
		// Token: 0x040022C6 RID: 8902
		EndInteraction,
		// Token: 0x040022C7 RID: 8903
		TriggerInteraction,
		// Token: 0x040022C8 RID: 8904
		StartThrow,
		// Token: 0x040022C9 RID: 8905
		EndThrow
	}
}
