using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200062E RID: 1582
public class ServerWashingStation : ServerSynchroniserBase, IHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x06001DF7 RID: 7671 RVA: 0x00091367 File Offset: 0x0008F767
	public override EntityType GetEntityType()
	{
		return EntityType.WashingStation;
	}

	// Token: 0x06001DF8 RID: 7672 RVA: 0x0009136C File Offset: 0x0008F76C
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_washingStation = (WashingStation)synchronisedObject;
		this.m_serverAttachStation = base.gameObject.RequireComponent<ServerAttachStation>();
		this.m_serverAttachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_serverAttachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_handlePickupReferral = base.gameObject.RequireComponent<ServerHandlePickupReferral>();
		this.m_originalPickupReferee = this.m_handlePickupReferral.GetHandlePickupReferree();
		this.m_interactable = base.gameObject.GetComponent<ServerInteractable>();
		this.m_interactable.RegisterCallbacks(new ServerInteractable.BeginInteractCallback(this.OnInteracterAdded), new ServerInteractable.EndInteractCallback(this.OnInteracterRemoved));
		this.m_interactable.enabled = false;
		this.m_interactable.SetStickyInteractionCallback(() => true);
		this.m_plateReturnStation = this.m_washingStation.m_dryingStation.gameObject.RequireComponent<ServerPlateReturnStation>();
		this.m_dryingAttachStation = this.m_washingStation.m_dryingStation.gameObject.RequireComponent<ServerAttachStation>();
		this.m_dryingAttachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAddedDryingStation));
		this.m_dryingAttachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemovedDryingStation));
	}

	// Token: 0x06001DF9 RID: 7673 RVA: 0x000914B9 File Offset: 0x0008F8B9
	private void SynchroniseInteractionState(bool _interacting)
	{
		this.m_data.m_msgType = WashingStationMessage.MessageType.InteractionState;
		this.m_data.m_interacting = _interacting;
		this.m_data.m_progress = this.m_cleaningTimer;
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001DFA RID: 7674 RVA: 0x000914F0 File Offset: 0x0008F8F0
	private void SendAddPlates()
	{
		this.m_data.m_msgType = WashingStationMessage.MessageType.AddPlates;
		this.m_data.m_plateCount = this.m_plateCount;
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001DFB RID: 7675 RVA: 0x0009151B File Offset: 0x0008F91B
	private void SendCleanedPlate()
	{
		this.m_data.m_msgType = WashingStationMessage.MessageType.CleanedPlate;
		this.m_data.m_plateCount = this.m_plateCount;
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001DFC RID: 7676 RVA: 0x00091546 File Offset: 0x0008F946
	private void OnInteracterAdded(GameObject _interacter, Vector2 _directionXZ)
	{
		this.m_interactor = _interacter;
		this.SynchroniseInteractionState(this.m_interactable.IsBeingInteractedWith());
	}

	// Token: 0x06001DFD RID: 7677 RVA: 0x00091560 File Offset: 0x0008F960
	private void OnInteracterRemoved(GameObject _interacter)
	{
		this.SynchroniseInteractionState(this.m_interactable.IsBeingInteractedWith());
	}

	// Token: 0x06001DFE RID: 7678 RVA: 0x00091574 File Offset: 0x0008F974
	public override void UpdateSynchronising()
	{
		if (this.m_interactable != null && this.m_interactable.IsBeingInteractedWith() && this.m_plateCount > 0)
		{
			this.m_cleaningTimer += TimeManager.GetDeltaTime(base.gameObject) * ((!this.m_interactable.AllowMultipleInteractors()) ? 1f : ((float)this.m_interactable.InteractorCount()));
			if (this.m_cleaningTimer > this.m_washingStation.m_cleanPlateTime)
			{
				this.m_cleaningTimer = 0f;
				this.m_plateCount--;
				ServerMessenger.Achievement(this.m_interactor.gameObject, 2, 1);
				this.SendCleanedPlate();
				this.m_plateReturnStation.ReturnPlate();
				if (this.m_plateCount == 0)
				{
					this.m_interactable.enabled = false;
				}
			}
		}
	}

	// Token: 0x06001DFF RID: 7679 RVA: 0x00091658 File Offset: 0x0008FA58
	private void OnItemAdded(IAttachment _attachment)
	{
		if (this.m_handlePickupReferral != null)
		{
			this.m_handlePickupReferral.SetHandlePickupReferree(null);
		}
		GameObject gameObject = (_attachment as MonoBehaviour).gameObject;
		ServerStack component = gameObject.GetComponent<ServerStack>();
		if (component != null)
		{
			this.m_serverAttachStation.TakeItem();
			NetworkUtils.DestroyObjectsRecursive(gameObject);
			this.AddPlates(component.GetSize());
		}
	}

	// Token: 0x06001E00 RID: 7680 RVA: 0x000916BF File Offset: 0x0008FABF
	private void OnItemRemoved(IAttachment _attachment)
	{
		if (this.m_handlePickupReferral != null)
		{
			this.m_handlePickupReferral.SetHandlePickupReferree(this.m_originalPickupReferee);
		}
	}

	// Token: 0x06001E01 RID: 7681 RVA: 0x000916E4 File Offset: 0x0008FAE4
	private void OnItemAddedDryingStation(IAttachment _attachment)
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.SetInteractionSuppressed(_attachment.AccessGameObject() != null && _attachment.AccessGameObject().RequestComponent<ServerCleanPlateStack>() == null);
		}
	}

	// Token: 0x06001E02 RID: 7682 RVA: 0x00091732 File Offset: 0x0008FB32
	private void OnItemRemovedDryingStation(IAttachment _attachment)
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.SetInteractionSuppressed(false);
		}
	}

	// Token: 0x06001E03 RID: 7683 RVA: 0x00091751 File Offset: 0x0008FB51
	private void AddPlates(int plateCount)
	{
		this.m_plateCount += plateCount;
		if (this.m_plateCount > 0)
		{
			this.m_interactable.enabled = true;
		}
		this.SendAddPlates();
	}

	// Token: 0x06001E04 RID: 7684 RVA: 0x00091780 File Offset: 0x0008FB80
	public void WashAllPlates()
	{
		if (this.m_plateCount > 0)
		{
			for (int i = 0; i < this.m_plateCount; i++)
			{
				this.m_plateReturnStation.ReturnPlate();
			}
			this.m_plateCount = 0;
			this.SendCleanedPlate();
			this.m_cleaningTimer = 0f;
			this.m_interactable.enabled = false;
		}
	}

	// Token: 0x06001E05 RID: 7685 RVA: 0x000917E0 File Offset: 0x0008FBE0
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		ServerStack component = _carrier.InspectCarriedItem().GetComponent<ServerStack>();
		return (component == null && this.m_dryingAttachStation.CanHandlePlacement(_carrier, _directionXZ, _context)) || this.m_washingStation.CanHandlePlacement(_carrier, _directionXZ, this.m_plateCount);
	}

	// Token: 0x06001E06 RID: 7686 RVA: 0x00091830 File Offset: 0x0008FC30
	public void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = _carrier.InspectCarriedItem();
		ServerStack component = gameObject.GetComponent<ServerStack>();
		if (component != null)
		{
			_carrier.DestroyCarriedItem();
			this.AddPlates(component.GetSize());
		}
		else if (this.m_dryingAttachStation.CanHandlePlacement(_carrier, _directionXZ, _context))
		{
			this.m_dryingAttachStation.HandlePlacement(_carrier, _directionXZ, _context);
		}
		else if (this.m_serverAttachStation.CanHandlePlacement(_carrier, _directionXZ, _context))
		{
			this.m_serverAttachStation.HandlePlacement(_carrier, _directionXZ, _context);
		}
	}

	// Token: 0x06001E07 RID: 7687 RVA: 0x000918B5 File Offset: 0x0008FCB5
	public void OnFailedToPlace(GameObject _item)
	{
	}

	// Token: 0x06001E08 RID: 7688 RVA: 0x000918B7 File Offset: 0x0008FCB7
	public int GetPlacementPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x04001722 RID: 5922
	private WashingStation m_washingStation;

	// Token: 0x04001723 RID: 5923
	private ServerAttachStation m_serverAttachStation;

	// Token: 0x04001724 RID: 5924
	private ServerAttachStation m_dryingAttachStation;

	// Token: 0x04001725 RID: 5925
	private WashingStationMessage m_data = new WashingStationMessage();

	// Token: 0x04001726 RID: 5926
	private ServerInteractable m_interactable;

	// Token: 0x04001727 RID: 5927
	private float m_cleaningTimer;

	// Token: 0x04001728 RID: 5928
	private int m_plateCount;

	// Token: 0x04001729 RID: 5929
	private GameObject m_interactor;

	// Token: 0x0400172A RID: 5930
	private ServerHandlePickupReferral m_handlePickupReferral;

	// Token: 0x0400172B RID: 5931
	private IHandlePickup m_originalPickupReferee;

	// Token: 0x0400172C RID: 5932
	private ServerPlateReturnStation m_plateReturnStation;
}
