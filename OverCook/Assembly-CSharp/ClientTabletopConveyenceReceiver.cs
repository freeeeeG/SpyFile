using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000598 RID: 1432
public class ClientTabletopConveyenceReceiver : ClientSynchroniserBase, IClientConveyenceReceiver
{
	// Token: 0x06001B36 RID: 6966 RVA: 0x000873A0 File Offset: 0x000857A0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_attachStation = base.gameObject.RequireComponent<ClientAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_attachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowPlacement));
		AttachStation attachStation = base.gameObject.RequireComponent<AttachStation>();
		attachStation.SetClientSidePredictionEnabled(true);
	}

	// Token: 0x06001B37 RID: 6967 RVA: 0x0008741D File Offset: 0x0008581D
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.m_attachStation)
		{
			this.m_attachStation.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowPlacement));
		}
	}

	// Token: 0x06001B38 RID: 6968 RVA: 0x0008744D File Offset: 0x0008584D
	private bool AllowPlacement(GameObject _object, PlacementContext _context)
	{
		return !this.m_receiving;
	}

	// Token: 0x06001B39 RID: 6969 RVA: 0x00087458 File Offset: 0x00085858
	private void OnItemAdded(IClientAttachment _attachment)
	{
		this.m_refreshedConveyToCallback();
	}

	// Token: 0x06001B3A RID: 6970 RVA: 0x00087465 File Offset: 0x00085865
	private void OnItemRemoved(IClientAttachment _attachment)
	{
		this.m_refreshedConveyToCallback();
	}

	// Token: 0x06001B3B RID: 6971 RVA: 0x00087472 File Offset: 0x00085872
	public void InformStartingConveyToMe()
	{
		this.m_receiving = true;
		this.m_refreshedConveyToCallback();
	}

	// Token: 0x06001B3C RID: 6972 RVA: 0x00087486 File Offset: 0x00085886
	public void InformEndingConveyToMe()
	{
		this.m_receiving = false;
		this.m_refreshedConveyToCallback();
	}

	// Token: 0x06001B3D RID: 6973 RVA: 0x0008749A File Offset: 0x0008589A
	public bool IsReceiving()
	{
		return this.m_receiving;
	}

	// Token: 0x06001B3E RID: 6974 RVA: 0x000874A2 File Offset: 0x000858A2
	public void RegisterRefreshedConveyToCallback(CallbackVoid _callback)
	{
		this.m_refreshedConveyToCallback = (CallbackVoid)Delegate.Combine(this.m_refreshedConveyToCallback, _callback);
	}

	// Token: 0x06001B3F RID: 6975 RVA: 0x000874BB File Offset: 0x000858BB
	public void UnregisterRefreshedConveyToCallback(CallbackVoid _callback)
	{
		this.m_refreshedConveyToCallback = (CallbackVoid)Delegate.Remove(this.m_refreshedConveyToCallback, _callback);
	}

	// Token: 0x04001569 RID: 5481
	private ClientAttachStation m_attachStation;

	// Token: 0x0400156A RID: 5482
	private bool m_receiving;

	// Token: 0x0400156B RID: 5483
	private CallbackVoid m_refreshedConveyToCallback = delegate()
	{
	};
}
