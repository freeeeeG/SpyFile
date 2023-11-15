using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000906 RID: 2310
public class ClientLocalAvatarSynchroniser : ClientWorldObjectSynchroniser
{
	// Token: 0x06002D20 RID: 11552 RVA: 0x000D5632 File Offset: 0x000D3A32
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_bHasEverReceived = true;
	}

	// Token: 0x06002D21 RID: 11553 RVA: 0x000D563B File Offset: 0x000D3A3B
	public override EntityType GetEntityType()
	{
		return EntityType.WorldMapVanAvatar;
	}

	// Token: 0x06002D22 RID: 11554 RVA: 0x000D563F File Offset: 0x000D3A3F
	public override void ApplyServerEvent(Serialisable serialisable)
	{
	}

	// Token: 0x06002D23 RID: 11555 RVA: 0x000D5641 File Offset: 0x000D3A41
	public override void ApplyServerUpdate(Serialisable serialisable)
	{
	}

	// Token: 0x06002D24 RID: 11556 RVA: 0x000D5643 File Offset: 0x000D3A43
	public override void UpdateSynchronising()
	{
	}

	// Token: 0x06002D25 RID: 11557 RVA: 0x000D5645 File Offset: 0x000D3A45
	public override void Pause()
	{
	}

	// Token: 0x06002D26 RID: 11558 RVA: 0x000D5647 File Offset: 0x000D3A47
	public override void Resume()
	{
	}
}
