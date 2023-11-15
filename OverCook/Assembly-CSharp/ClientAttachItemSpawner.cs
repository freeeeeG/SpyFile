using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000429 RID: 1065
public class ClientAttachItemSpawner : ClientSynchroniserBase
{
	// Token: 0x06001355 RID: 4949 RVA: 0x0006C0A4 File Offset: 0x0006A4A4
	public override EntityType GetEntityType()
	{
		return EntityType.PlacementItemSpawner;
	}

	// Token: 0x06001356 RID: 4950 RVA: 0x0006C0A8 File Offset: 0x0006A4A8
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_attachItemSpawner = (AttachItemSpawner)synchronisedObject;
		this.m_orderDefinition = base.gameObject.RequireInterface<IOrderDefinition>();
		this.m_attachStation = base.gameObject.RequestComponent<ClientAttachStation>();
	}

	// Token: 0x06001357 RID: 4951 RVA: 0x0006C0DF File Offset: 0x0006A4DF
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		this.OnItemSpawned();
	}

	// Token: 0x06001358 RID: 4952 RVA: 0x0006C0E7 File Offset: 0x0006A4E7
	private void OnItemSpawned()
	{
		base.gameObject.SendMessage("OnPickupItem", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x04000F39 RID: 3897
	private AttachItemSpawner m_attachItemSpawner;

	// Token: 0x04000F3A RID: 3898
	private IOrderDefinition m_orderDefinition;

	// Token: 0x04000F3B RID: 3899
	private ClientAttachStation m_attachStation;
}
