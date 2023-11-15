using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200080A RID: 2058
public class ClientLimitedQuantityItem : ClientSynchroniserBase
{
	// Token: 0x0600275D RID: 10077 RVA: 0x000B94E4 File Offset: 0x000B78E4
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_baseObject = (LimitedQuantityItem)synchronisedObject;
		this.m_Manager = GameUtils.RequireManager<LimitedQuantityItemManager>();
		base.StartSynchronising(synchronisedObject);
	}

	// Token: 0x0600275E RID: 10078 RVA: 0x000B9504 File Offset: 0x000B7904
	public void PlayDestructionPFX()
	{
		if (null != this.m_Manager.m_DestroyPFXPrefab)
		{
			this.m_Manager.m_DestroyPFXPrefab.InstantiatePFX(base.transform.position);
		}
	}

	// Token: 0x0600275F RID: 10079 RVA: 0x000B9538 File Offset: 0x000B7938
	public void RegisterImpendingDestructionNotification(ImpendingDestructionCallback func)
	{
		this.m_baseObject.RegisterImpendingDestructionNotification(func);
	}

	// Token: 0x06002760 RID: 10080 RVA: 0x000B9546 File Offset: 0x000B7946
	public void UnregisterImpendingDestructionNotification(ImpendingDestructionCallback func)
	{
		this.m_baseObject.UnregisterImpendingDestructionNotification(func);
	}

	// Token: 0x06002761 RID: 10081 RVA: 0x000B9554 File Offset: 0x000B7954
	public void NotifyOfImpendingDestruction()
	{
		this.m_baseObject.NotifyOfImpendingDestruction();
	}

	// Token: 0x04001EE9 RID: 7913
	private LimitedQuantityItem m_baseObject;

	// Token: 0x04001EEA RID: 7914
	private LimitedQuantityItemManager m_Manager;
}
