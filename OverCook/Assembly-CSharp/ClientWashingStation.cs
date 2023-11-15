using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200062F RID: 1583
public class ClientWashingStation : ClientSynchroniserBase, IClientHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x06001E0B RID: 7691 RVA: 0x000918C9 File Offset: 0x0008FCC9
	public override EntityType GetEntityType()
	{
		return EntityType.WashingStation;
	}

	// Token: 0x06001E0C RID: 7692 RVA: 0x000918D0 File Offset: 0x0008FCD0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		if (this.m_washingStation == null)
		{
			this.m_washingStation = (WashingStation)synchronisedObject;
		}
		this.m_interactable = base.gameObject.GetComponent<ClientInteractable>();
		this.m_interactable.enabled = false;
		this.m_interactable.SetStickyInteractionCallback(() => true);
		this.m_clientAttachStation = base.gameObject.RequireComponent<ClientAttachStation>();
		this.m_handlePickupreferral = base.gameObject.RequireComponent<ClientHandlePickupReferral>();
		this.m_clientAttachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAddedOntoSink));
		this.m_clientAttachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemovedFromSink));
		this.m_pickupReferree = this.m_handlePickupreferral.GetHandlePickupReferree();
		this.m_dryingAttachStation = this.m_washingStation.m_dryingStation.gameObject.RequireComponent<ClientAttachStation>();
		this.m_dryingAttachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAddedDryingStation));
		this.m_dryingAttachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemovedDryingStation));
	}

	// Token: 0x06001E0D RID: 7693 RVA: 0x000919F0 File Offset: 0x0008FDF0
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		WashingStationMessage washingStationMessage = (WashingStationMessage)serialisable;
		WashingStationMessage.MessageType msgType = washingStationMessage.m_msgType;
		if (msgType != WashingStationMessage.MessageType.InteractionState)
		{
			if (msgType != WashingStationMessage.MessageType.AddPlates)
			{
				if (msgType == WashingStationMessage.MessageType.CleanedPlate)
				{
					this.m_plateCount = washingStationMessage.m_plateCount;
					if (this.m_plateCount == 0)
					{
						this.m_interactable.enabled = false;
					}
					this.OnPlateCleaned();
				}
			}
			else
			{
				this.m_plateCount = washingStationMessage.m_plateCount;
				this.m_interactable.enabled = true;
				this.OnPlatesAdded();
			}
		}
		else
		{
			this.m_isWashing = washingStationMessage.m_interacting;
			this.m_cleaningTimer = washingStationMessage.m_progress;
			if (this.m_isWashing)
			{
				this.m_progressUI.SetVisibility(true);
			}
			else
			{
				this.m_progressUI.SetProgress(Mathf.Clamp01(this.m_cleaningTimer / this.m_washingStation.m_cleanPlateTime));
			}
		}
	}

	// Token: 0x06001E0E RID: 7694 RVA: 0x00091AD0 File Offset: 0x0008FED0
	private void Awake()
	{
		if (this.m_washingStation == null)
		{
			this.m_washingStation = base.gameObject.RequireComponent<WashingStation>();
		}
		for (int i = 0; i < this.m_washingStation.m_dirtyPlates.Length; i++)
		{
			this.m_washingStation.m_dirtyPlates[i].SetActive(false);
		}
		GameObject gameObject = GameUtils.InstantiateUIController(this.m_washingStation.m_progressUIPrefab.gameObject, "HoverIconCanvas");
		this.m_progressUI = gameObject.GetComponent<ProgressUIController>();
		this.m_progressUI.SetFollowTransform(base.transform, Vector3.zero);
	}

	// Token: 0x06001E0F RID: 7695 RVA: 0x00091B6D File Offset: 0x0008FF6D
	protected override void OnDestroy()
	{
		if (this.m_progressUI != null && this.m_progressUI.gameObject != null)
		{
			UnityEngine.Object.Destroy(this.m_progressUI);
		}
		base.OnDestroy();
	}

	// Token: 0x06001E10 RID: 7696 RVA: 0x00091BA8 File Offset: 0x0008FFA8
	public override void UpdateSynchronising()
	{
		if (this.m_isWashing && this.m_plateCount > 0)
		{
			this.m_cleaningTimer += TimeManager.GetDeltaTime(base.gameObject) * (float)this.m_interactable.InteractorCount() * ((!this.m_interactable.AllowMultipleInteractors()) ? 0f : 1f);
			this.m_progressUI.SetProgress(Mathf.Clamp01(this.m_cleaningTimer / this.m_washingStation.m_cleanPlateTime));
		}
	}

	// Token: 0x06001E11 RID: 7697 RVA: 0x00091C33 File Offset: 0x00090033
	private void OnItemAddedOntoSink(IClientAttachment _iHoldable)
	{
		this.m_handlePickupreferral.SetHandlePickupReferree(null);
		this.m_interactable.SetInteractionSuppressed(true);
	}

	// Token: 0x06001E12 RID: 7698 RVA: 0x00091C4D File Offset: 0x0009004D
	private void OnItemRemovedFromSink(IClientAttachment _iHoldable)
	{
		this.m_handlePickupreferral.SetHandlePickupReferree(this.m_pickupReferree);
		this.m_interactable.SetInteractionSuppressed(false);
	}

	// Token: 0x06001E13 RID: 7699 RVA: 0x00091C6C File Offset: 0x0009006C
	private void OnItemAddedDryingStation(IClientAttachment _iHoldable)
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.SetInteractionSuppressed(_iHoldable.AccessGameObject() != null && _iHoldable.AccessGameObject().RequestComponent<ClientCleanPlateStack>() == null);
		}
	}

	// Token: 0x06001E14 RID: 7700 RVA: 0x00091CBA File Offset: 0x000900BA
	private void OnItemRemovedDryingStation(IClientAttachment _iHoldable)
	{
		if (this.m_interactable != null)
		{
			this.m_interactable.SetInteractionSuppressed(false);
		}
	}

	// Token: 0x06001E15 RID: 7701 RVA: 0x00091CD9 File Offset: 0x000900D9
	private void OnPlatesAdded()
	{
		this.UpdateCosmetics();
	}

	// Token: 0x06001E16 RID: 7702 RVA: 0x00091CE4 File Offset: 0x000900E4
	private void OnPlateCleaned()
	{
		if (this.m_plateCount > 0)
		{
			this.m_cleaningTimer = 0f;
		}
		else
		{
			this.m_progressUI.SetVisibility(false);
		}
		GameUtils.TriggerAudio(GameOneShotAudioTag.WashedPlate, base.gameObject.layer);
		this.UpdateCosmetics();
	}

	// Token: 0x06001E17 RID: 7703 RVA: 0x00091D34 File Offset: 0x00090134
	private void UpdateCosmetics()
	{
		for (int i = 0; i < this.m_washingStation.m_dirtyPlates.Length; i++)
		{
			this.m_washingStation.m_dirtyPlates[i].SetActive(false);
		}
		for (int j = 0; j < Mathf.Min(this.m_plateCount, this.m_washingStation.m_dirtyPlates.Length); j++)
		{
			this.m_washingStation.m_dirtyPlates[j].SetActive(true);
		}
	}

	// Token: 0x06001E18 RID: 7704 RVA: 0x00091DB0 File Offset: 0x000901B0
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		ClientStack component = _carrier.InspectCarriedItem().GetComponent<ClientStack>();
		return (component == null && this.m_dryingAttachStation.CanHandlePlacement(_carrier, _directionXZ, _context)) || this.m_washingStation.CanHandlePlacement(_carrier, _directionXZ, this.m_plateCount);
	}

	// Token: 0x06001E19 RID: 7705 RVA: 0x00091DFE File Offset: 0x000901FE
	public int GetPlacementPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x0400172E RID: 5934
	private WashingStation m_washingStation;

	// Token: 0x0400172F RID: 5935
	private ClientAttachStation m_clientAttachStation;

	// Token: 0x04001730 RID: 5936
	private ClientAttachStation m_dryingAttachStation;

	// Token: 0x04001731 RID: 5937
	private ClientHandlePickupReferral m_handlePickupreferral;

	// Token: 0x04001732 RID: 5938
	private ClientInteractable m_interactable;

	// Token: 0x04001733 RID: 5939
	private IClientHandlePickup m_pickupReferree;

	// Token: 0x04001734 RID: 5940
	private int m_plateCount;

	// Token: 0x04001735 RID: 5941
	private bool m_isWashing;

	// Token: 0x04001736 RID: 5942
	private ProgressUIController m_progressUI;

	// Token: 0x04001737 RID: 5943
	private float m_cleaningTimer;
}
