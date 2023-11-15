using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200054A RID: 1354
public class ClientPlateReturnStation : ClientSynchroniserBase
{
	// Token: 0x0600197F RID: 6527 RVA: 0x0008046C File Offset: 0x0007E86C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_returnStation = (PlateReturnStation)synchronisedObject;
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_returnStation.m_stackPrefab);
		this.m_attachStation = base.GetComponent<ClientAttachStation>();
		this.m_attachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
		this.m_attachStation.RegisterAllowItemPickup(new Generic<bool>(this.CanRemoveItem));
		this.m_attachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_handlePlacementreferral = base.gameObject.RequestComponent<ClientHandlePlacementReferral>();
		Mailbox.Client.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001980 RID: 6528 RVA: 0x00080534 File Offset: 0x0007E934
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.m_attachStation)
		{
			this.m_attachStation.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
			this.m_attachStation.UnregisterAllowItemPickup(new Generic<bool>(this.CanRemoveItem));
			this.m_attachStation.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
			this.m_attachStation.UnregisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
			if (this.m_stack != null)
			{
				this.m_stack.UnregisterOnPlateAdded(new GenericVoid<GameObject>(this.OnPlateAdded));
				this.m_stack.UnregisterOnPlateRemoved(new GenericVoid<GameObject>(this.OnPlatesUpdated));
			}
		}
		Mailbox.Client.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001981 RID: 6529 RVA: 0x0008060C File Offset: 0x0007EA0C
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities && this.m_handlePlacementreferral != null)
		{
			this.m_placementReferree = this.m_handlePlacementreferral.GetHandlePlacementReferree();
		}
	}

	// Token: 0x06001982 RID: 6530 RVA: 0x0008064F File Offset: 0x0007EA4F
	public bool HasReturnedPlates()
	{
		return this.m_stack != null && this.m_stack.GetCount() > 0;
	}

	// Token: 0x06001983 RID: 6531 RVA: 0x00080674 File Offset: 0x0007EA74
	public void OnItemAdded(IClientAttachment _iHoldable)
	{
		this.m_stack = _iHoldable.AccessGameObject().GetComponent<ClientPlateStackBase>();
		if (this.m_stack != null)
		{
			this.m_stack.RegisterOnPlateAdded(new GenericVoid<GameObject>(this.OnPlateAdded));
			this.m_stack.RegisterOnPlateRemoved(new GenericVoid<GameObject>(this.OnPlatesUpdated));
			GameUtils.TriggerAudio(GameOneShotAudioTag.WashedPlate, base.gameObject.layer);
		}
	}

	// Token: 0x06001984 RID: 6532 RVA: 0x000806E4 File Offset: 0x0007EAE4
	public void OnItemRemoved(IClientAttachment _iHoldable)
	{
		if (this.m_stack != null)
		{
			this.m_stack.UnregisterOnPlateAdded(new GenericVoid<GameObject>(this.OnPlateAdded));
			this.m_stack.UnregisterOnPlateRemoved(new GenericVoid<GameObject>(this.OnPlatesUpdated));
			this.m_stack = null;
		}
	}

	// Token: 0x06001985 RID: 6533 RVA: 0x00080737 File Offset: 0x0007EB37
	public void OnPlateAdded(GameObject _plate)
	{
		GameUtils.TriggerAudio(GameOneShotAudioTag.WashedPlate, base.gameObject.layer);
		this.OnPlatesUpdated(_plate);
	}

	// Token: 0x06001986 RID: 6534 RVA: 0x00080753 File Offset: 0x0007EB53
	public void OnPlatesUpdated(GameObject _plate)
	{
		if (this.m_handlePlacementreferral != null)
		{
			this.m_handlePlacementreferral.SetHandlePlacementReferree((!this.HasReturnedPlates()) ? this.m_placementReferree : null);
		}
	}

	// Token: 0x06001987 RID: 6535 RVA: 0x00080788 File Offset: 0x0007EB88
	private bool CanAddItem(GameObject _object, PlacementContext _context)
	{
		return _context.m_source != PlacementContext.Source.Player && !(this.m_stack != null) && !this.m_attachStation.HasItem();
	}

	// Token: 0x06001988 RID: 6536 RVA: 0x000807BF File Offset: 0x0007EBBF
	private bool CanRemoveItem()
	{
		return this.m_stack == null || this.HasReturnedPlates();
	}

	// Token: 0x04001444 RID: 5188
	private PlateReturnStation m_returnStation;

	// Token: 0x04001445 RID: 5189
	private ClientAttachStation m_attachStation;

	// Token: 0x04001446 RID: 5190
	private ClientPlateStackBase m_stack;

	// Token: 0x04001447 RID: 5191
	private ClientHandlePlacementReferral m_handlePlacementreferral;

	// Token: 0x04001448 RID: 5192
	private IClientHandlePlacement m_placementReferree;
}
