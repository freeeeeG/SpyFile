using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000907 RID: 2311
public class ServerAvatarSynchroniser : ServerWorldObjectSynchroniser
{
	// Token: 0x06002D28 RID: 11560 RVA: 0x000D5C4A File Offset: 0x000D404A
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
		base.StartSynchronising(synchronisedObject);
	}

	// Token: 0x06002D29 RID: 11561 RVA: 0x000D5C5F File Offset: 0x000D405F
	public override EntityType GetEntityType()
	{
		return EntityType.WorldMapVanAvatar;
	}

	// Token: 0x06002D2A RID: 11562 RVA: 0x000D5C64 File Offset: 0x000D4064
	public override Serialisable GetServerUpdate()
	{
		this.m_AvatarData.WorldObject = (WorldObjectMessage)base.GetServerUpdate();
		if (this.m_AvatarData.WorldObject != null)
		{
			this.m_AvatarData.Velocity = this.m_Rigidbody.velocity;
			return this.m_AvatarData;
		}
		return null;
	}

	// Token: 0x06002D2B RID: 11563 RVA: 0x000D5CB8 File Offset: 0x000D40B8
	public override void SendServerEvent(Serialisable message)
	{
		this.m_AvatarData.WorldObject = (WorldObjectMessage)message;
		if (this.m_AvatarData.WorldObject != null)
		{
			this.m_AvatarData.Velocity = this.m_Rigidbody.velocity;
		}
		base.SendServerEvent(this.m_AvatarData);
	}

	// Token: 0x04002438 RID: 9272
	private Rigidbody m_Rigidbody;

	// Token: 0x04002439 RID: 9273
	private AvatarPositionMessage m_AvatarData = new AvatarPositionMessage();
}
