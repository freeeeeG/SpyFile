using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000596 RID: 1430
public class ClientSwitchStation : ClientSynchroniserBase
{
	// Token: 0x06001B1E RID: 6942 RVA: 0x00086C60 File Offset: 0x00085060
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_switchStation = (SwitchStation)synchronisedObject;
		this.m_interactable = base.gameObject.RequireComponent<ClientInteractable>();
		this.m_attachStation = base.gameObject.RequireComponent<ClientAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_attachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
	}

	// Token: 0x06001B1F RID: 6943 RVA: 0x00086CE7 File Offset: 0x000850E7
	private void OnItemAdded(IClientAttachment _attachment)
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.enabled = false;
		}
	}

	// Token: 0x06001B20 RID: 6944 RVA: 0x00086D06 File Offset: 0x00085106
	private void OnItemRemoved(IClientAttachment _attachment)
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.enabled = true;
		}
	}

	// Token: 0x06001B21 RID: 6945 RVA: 0x00086D25 File Offset: 0x00085125
	private bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		return _context.m_source != PlacementContext.Source.Player;
	}

	// Token: 0x06001B22 RID: 6946 RVA: 0x00086D34 File Offset: 0x00085134
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_attachStation != null)
		{
			this.m_attachStation.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
			this.m_attachStation.UnregisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
			this.m_attachStation.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		}
	}

	// Token: 0x0400155F RID: 5471
	private SwitchStation m_switchStation;

	// Token: 0x04001560 RID: 5472
	private ClientInteractable m_interactable;

	// Token: 0x04001561 RID: 5473
	private ClientAttachStation m_attachStation;
}
