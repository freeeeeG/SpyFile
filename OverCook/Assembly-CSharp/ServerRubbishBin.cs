using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000564 RID: 1380
public class ServerRubbishBin : ServerSynchroniserBase, IDisposer, IHandlePickup, IHandlePlacement, IBaseHandlePickup, IBaseHandlePlacement
{
	// Token: 0x060019F6 RID: 6646 RVA: 0x000822A4 File Offset: 0x000806A4
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_rubbishBin = (RubbishBin)synchronisedObject;
		this.m_serverAttachStation = this.m_rubbishBin.gameObject.RequireComponent<ServerAttachStation>();
		this.m_serverReceiver = base.gameObject.RequireComponent<ServerTabletopConveyenceReceiver>();
	}

	// Token: 0x060019F7 RID: 6647 RVA: 0x000822D9 File Offset: 0x000806D9
	public override EntityType GetEntityType()
	{
		return EntityType.RubbishBin;
	}

	// Token: 0x060019F8 RID: 6648 RVA: 0x000822DD File Offset: 0x000806DD
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return _carrier != null && this.m_serverAttachStation.CanHandlePickup(_carrier) && this.m_serverReceiver.IsReceiving();
	}

	// Token: 0x060019F9 RID: 6649 RVA: 0x00082304 File Offset: 0x00080704
	public int GetPickupPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x060019FA RID: 6650 RVA: 0x0008230B File Offset: 0x0008070B
	public void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
	{
		if (_carrier != null)
		{
			this.m_serverAttachStation.HandlePickup(_carrier, _directionXZ);
		}
	}

	// Token: 0x060019FB RID: 6651 RVA: 0x00082320 File Offset: 0x00080720
	public void PassToDestroy(IAttachment _attachment)
	{
		ServerLimitedQuantityItem serverLimitedQuantityItem = _attachment.AccessGameObject().RequestComponent<ServerLimitedQuantityItem>();
		if (null != serverLimitedQuantityItem)
		{
			serverLimitedQuantityItem.AddInvincibilityCondition(this.m_true);
		}
		_attachment.AccessGameObject().GetComponent<Collider>().enabled = false;
		this.m_Data.BinnedItemEntityID = ((ServerSynchroniserBase)_attachment).GetEntityId();
		this.m_Data.m_alive = true;
		this.SendServerEvent(this.m_Data);
		ServerRubbishBin.ItemData itemData = new ServerRubbishBin.ItemData();
		itemData.m_object = _attachment;
		itemData.m_deathProp = 0f;
		this.m_itemData.Add(itemData);
		ServerHandlePickupReferral serverHandlePickupReferral = _attachment.AccessGameObject().RequestComponent<ServerHandlePickupReferral>();
		if (serverHandlePickupReferral != null)
		{
			serverHandlePickupReferral.SetHandlePickupReferree(this);
		}
	}

	// Token: 0x060019FC RID: 6652 RVA: 0x000823D4 File Offset: 0x000807D4
	public override void UpdateSynchronising()
	{
		GameObject gameObject = this.m_serverAttachStation.InspectItem();
		if (gameObject != null && !this.m_serverReceiver.IsReceiving() && this.m_itemData.Count <= 0)
		{
			IAttachment attachment = gameObject.RequireInterface<IAttachment>();
			IThrowable component = gameObject.GetComponent<IThrowable>();
			if (component as MonoBehaviour != null)
			{
				MonoBehaviour monoBehaviour = component.GetPreviousThrower() as MonoBehaviour;
				if (monoBehaviour != null)
				{
					GameObject gameObject2 = (component as MonoBehaviour).gameObject;
					if (monoBehaviour.gameObject != null)
					{
						ServerMessenger.Achievement(monoBehaviour.gameObject, 14, 1);
					}
					GameObject gameObject3 = attachment.AccessGameObject();
					if (gameObject3 != null)
					{
						IDisposalBehaviour disposalBehaviour = gameObject3.RequestInterface<IDisposalBehaviour>();
						if (disposalBehaviour != null)
						{
							disposalBehaviour.Destroying(this);
						}
					}
				}
			}
			this.PassToDestroy(attachment);
		}
		for (int i = this.m_itemData.Count - 1; i >= 0; i--)
		{
			ServerRubbishBin.ItemData itemData = this.m_itemData[i];
			if (itemData.m_object as MonoBehaviour == null)
			{
				this.m_itemData.RemoveAt(i);
			}
		}
		for (int j = 0; j < this.m_itemData.Count; j++)
		{
			ServerRubbishBin.ItemData itemData2 = this.m_itemData[j];
			float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
			itemData2.m_deathProp = Mathf.Clamp01(itemData2.m_deathProp + deltaTime / this.m_rubbishBin.m_fallTime);
			if (itemData2.m_deathProp >= 1f && this.m_serverAttachStation.InspectItem() != null)
			{
				this.m_serverAttachStation.TakeItem();
				GameObject gameObject4 = itemData2.m_object.AccessGameObject();
				ServerHandlePickupReferral serverHandlePickupReferral = gameObject4.RequestComponent<ServerHandlePickupReferral>();
				if (serverHandlePickupReferral != null && serverHandlePickupReferral.GetHandlePickupReferree() == this)
				{
					serverHandlePickupReferral.SetHandlePickupReferree(null);
				}
				IAttachment attachment2 = gameObject4.RequireInterface<IAttachment>();
				this.m_Data.BinnedItemEntityID = ((ServerSynchroniserBase)attachment2).GetEntityId();
				this.m_Data.m_alive = false;
				this.SendServerEvent(this.m_Data);
				ServerPlayerRespawnManager.KillOrRespawn(gameObject4, null);
				itemData2.m_object = null;
			}
		}
	}

	// Token: 0x060019FD RID: 6653 RVA: 0x0008261C File Offset: 0x00080A1C
	public void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		if (_carrier != null && _carrier.AccessGameObject() != null)
		{
			IDisposalBehaviour disposalBehaviour = _carrier.InspectCarriedItem().RequestInterface<IDisposalBehaviour>();
			if (disposalBehaviour != null && !disposalBehaviour.WillBeDestroyed())
			{
				disposalBehaviour.AddToDisposer(_carrier, this);
			}
			else
			{
				if (disposalBehaviour != null)
				{
					disposalBehaviour.Destroying(this);
				}
				this.m_serverAttachStation.HandlePlacement(_carrier, _directionXZ, _context);
				ServerMessenger.Achievement(_carrier.AccessGameObject(), 14, 1);
			}
		}
	}

	// Token: 0x060019FE RID: 6654 RVA: 0x00082694 File Offset: 0x00080A94
	public void OnFailedToPlace(GameObject _item)
	{
	}

	// Token: 0x060019FF RID: 6655 RVA: 0x00082698 File Offset: 0x00080A98
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = _carrier.InspectCarriedItem();
		return !(gameObject == null) && gameObject.RequestInterface<IDisposalBehaviour>() != null && this.m_serverAttachStation.CanHandlePlacement(_carrier, _directionXZ, _context);
	}

	// Token: 0x06001A00 RID: 6656 RVA: 0x000826D3 File Offset: 0x00080AD3
	public int GetPlacementPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x040014A7 RID: 5287
	private RubbishBin m_rubbishBin;

	// Token: 0x040014A8 RID: 5288
	private List<ServerRubbishBin.ItemData> m_itemData = new List<ServerRubbishBin.ItemData>();

	// Token: 0x040014A9 RID: 5289
	private Generic<bool> m_true = () => true;

	// Token: 0x040014AA RID: 5290
	private RubbishBinMessage m_Data = new RubbishBinMessage();

	// Token: 0x040014AB RID: 5291
	private ServerAttachStation m_serverAttachStation;

	// Token: 0x040014AC RID: 5292
	private ServerTabletopConveyenceReceiver m_serverReceiver;

	// Token: 0x02000565 RID: 1381
	private class ItemData
	{
		// Token: 0x040014AE RID: 5294
		public IAttachment m_object;

		// Token: 0x040014AF RID: 5295
		public float m_deathProp;
	}
}
