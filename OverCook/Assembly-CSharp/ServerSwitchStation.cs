using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000595 RID: 1429
public class ServerSwitchStation : ServerSynchroniserBase
{
	// Token: 0x06001B18 RID: 6936 RVA: 0x00086B18 File Offset: 0x00084F18
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_switchStation = (SwitchStation)synchronisedObject;
		this.m_interactable = base.gameObject.RequireComponent<ServerInteractable>();
		this.m_attachStation = base.gameObject.RequireComponent<ServerAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_attachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
	}

	// Token: 0x06001B19 RID: 6937 RVA: 0x00086B9F File Offset: 0x00084F9F
	private void OnItemAdded(IAttachment _attachment)
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.enabled = false;
		}
	}

	// Token: 0x06001B1A RID: 6938 RVA: 0x00086BBE File Offset: 0x00084FBE
	private void OnItemRemoved(IAttachment _attachment)
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.enabled = true;
		}
	}

	// Token: 0x06001B1B RID: 6939 RVA: 0x00086BDD File Offset: 0x00084FDD
	private bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		return _context.m_source != PlacementContext.Source.Player;
	}

	// Token: 0x06001B1C RID: 6940 RVA: 0x00086BEC File Offset: 0x00084FEC
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_attachStation != null)
		{
			this.m_attachStation.UnregisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
			this.m_attachStation.UnregisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
			this.m_attachStation.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		}
	}

	// Token: 0x0400155C RID: 5468
	private SwitchStation m_switchStation;

	// Token: 0x0400155D RID: 5469
	private ServerInteractable m_interactable;

	// Token: 0x0400155E RID: 5470
	private ServerAttachStation m_attachStation;
}
