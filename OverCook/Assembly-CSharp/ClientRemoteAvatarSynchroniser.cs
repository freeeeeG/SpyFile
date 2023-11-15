using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000908 RID: 2312
public class ClientRemoteAvatarSynchroniser : ClientWorldObjectSynchroniser
{
	// Token: 0x06002D2D RID: 11565 RVA: 0x000D5D1B File Offset: 0x000D411B
	public override void Awake()
	{
		base.Awake();
		this.m_Rigidbody = base.GetComponent<Rigidbody>();
	}

	// Token: 0x06002D2E RID: 11566 RVA: 0x000D5D2F File Offset: 0x000D412F
	public override EntityType GetEntityType()
	{
		return EntityType.WorldMapVanAvatar;
	}

	// Token: 0x06002D2F RID: 11567 RVA: 0x000D5D34 File Offset: 0x000D4134
	public override void ApplyServerUpdate(Serialisable serialisable)
	{
		AvatarPositionMessage avatarPositionMessage = (AvatarPositionMessage)serialisable;
		this.HandleNetworkMessage(avatarPositionMessage);
		base.ApplyServerUpdate(avatarPositionMessage.WorldObject);
	}

	// Token: 0x06002D30 RID: 11568 RVA: 0x000D5D5C File Offset: 0x000D415C
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		AvatarPositionMessage avatarPositionMessage = (AvatarPositionMessage)serialisable;
		this.HandleNetworkMessage(avatarPositionMessage);
		base.ApplyServerEvent(avatarPositionMessage.WorldObject);
	}

	// Token: 0x06002D31 RID: 11569 RVA: 0x000D5D83 File Offset: 0x000D4183
	protected void HandleNetworkMessage(AvatarPositionMessage message)
	{
		this.m_Velocity = message.Velocity;
		this.m_Rigidbody.velocity = this.m_Velocity;
	}

	// Token: 0x06002D32 RID: 11570 RVA: 0x000D5DA2 File Offset: 0x000D41A2
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
	}

	// Token: 0x0400243A RID: 9274
	private Vector3 m_Velocity = Vector3.zero;

	// Token: 0x0400243B RID: 9275
	private Rigidbody m_Rigidbody;
}
