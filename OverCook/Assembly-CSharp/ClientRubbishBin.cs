using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000566 RID: 1382
public class ClientRubbishBin : ClientSynchroniserBase, IClientHandlePickup, IClientHandlePlacement, IBaseHandlePickup, IBaseHandlePlacement
{
	// Token: 0x06001A04 RID: 6660 RVA: 0x000826F8 File Offset: 0x00080AF8
	public override EntityType GetEntityType()
	{
		return EntityType.RubbishBin;
	}

	// Token: 0x06001A05 RID: 6661 RVA: 0x000826FC File Offset: 0x00080AFC
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_rubbishBin = (RubbishBin)synchronisedObject;
		this.m_attachStation = this.m_rubbishBin.gameObject.RequireComponent<AttachStation>();
		this.m_clientAttachStation = this.m_rubbishBin.gameObject.RequireComponent<ClientAttachStation>();
		this.m_clientReceiver = base.gameObject.RequireComponent<ClientTabletopConveyenceReceiver>();
	}

	// Token: 0x06001A06 RID: 6662 RVA: 0x00082754 File Offset: 0x00080B54
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		RubbishBinMessage rubbishBinMessage = (RubbishBinMessage)serialisable;
		EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(rubbishBinMessage.BinnedItemEntityID);
		if (entry != null)
		{
			entry.m_GameObject.GetComponent<Collider>().enabled = false;
			Transform transform = entry.m_GameObject.transform;
			if (!rubbishBinMessage.m_alive)
			{
				for (int i = 0; i < this.m_itemData.Count; i++)
				{
					if (this.m_itemData[i].m_transform == transform)
					{
						this.m_itemData[i].m_alive = rubbishBinMessage.m_alive;
						break;
					}
				}
				ClientHandlePickupReferral clientHandlePickupReferral = entry.m_GameObject.RequestComponent<ClientHandlePickupReferral>();
				if (clientHandlePickupReferral != null && clientHandlePickupReferral.GetHandlePickupReferree() == this)
				{
					clientHandlePickupReferral.SetHandlePickupReferree(null);
				}
			}
			else
			{
				ClientRubbishBin.ItemData itemData = new ClientRubbishBin.ItemData();
				itemData.m_transform = transform;
				itemData.m_deathProp = 0f;
				itemData.m_alive = true;
				this.m_itemData.Add(itemData);
				ClientHandlePickupReferral clientHandlePickupReferral2 = entry.m_GameObject.RequestComponent<ClientHandlePickupReferral>();
				if (clientHandlePickupReferral2 != null)
				{
					clientHandlePickupReferral2.SetHandlePickupReferree(this);
				}
			}
		}
	}

	// Token: 0x06001A07 RID: 6663 RVA: 0x0008287C File Offset: 0x00080C7C
	public override void UpdateSynchronising()
	{
		for (int i = this.m_itemData.Count - 1; i >= 0; i--)
		{
			ClientRubbishBin.ItemData itemData = this.m_itemData[i];
			if (itemData.m_transform == null || itemData.m_deathProp >= 1f || !itemData.m_alive)
			{
				this.m_itemData.RemoveAt(i);
			}
		}
		for (int j = 0; j < this.m_itemData.Count; j++)
		{
			ClientRubbishBin.ItemData itemData2 = this.m_itemData[j];
			if (itemData2.m_transform.parent == this.m_attachStation.GetAttachPoint(itemData2.m_transform.gameObject))
			{
				float deltaTime = TimeManager.GetDeltaTime(base.gameObject);
				itemData2.m_deathProp = Mathf.Clamp01(itemData2.m_deathProp + deltaTime / this.m_rubbishBin.m_fallTime);
				Transform transform = itemData2.m_transform;
				transform.localPosition = -Vector3.up * this.m_rubbishBin.m_fallDistance;
				transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.AddY(this.m_rubbishBin.m_angularVelocity * deltaTime));
				transform.localScale = VectorUtils.Splat3(1f - itemData2.m_deathProp);
			}
		}
	}

	// Token: 0x06001A08 RID: 6664 RVA: 0x000829DD File Offset: 0x00080DDD
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return _carrier != null && this.m_clientAttachStation.CanHandlePickup(_carrier) && this.m_clientReceiver.IsReceiving();
	}

	// Token: 0x06001A09 RID: 6665 RVA: 0x00082A04 File Offset: 0x00080E04
	public int GetPickupPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x06001A0A RID: 6666 RVA: 0x00082A0C File Offset: 0x00080E0C
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = _carrier.InspectCarriedItem();
		return !(gameObject == null) && gameObject.RequestInterface<IDisposalBehaviour>() != null && this.m_clientAttachStation.CanHandlePlacement(_carrier, _directionXZ, _context);
	}

	// Token: 0x06001A0B RID: 6667 RVA: 0x00082A47 File Offset: 0x00080E47
	public int GetPlacementPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x040014B0 RID: 5296
	private RubbishBin m_rubbishBin;

	// Token: 0x040014B1 RID: 5297
	private List<ClientRubbishBin.ItemData> m_itemData = new List<ClientRubbishBin.ItemData>();

	// Token: 0x040014B2 RID: 5298
	private ClientAttachStation m_clientAttachStation;

	// Token: 0x040014B3 RID: 5299
	private AttachStation m_attachStation;

	// Token: 0x040014B4 RID: 5300
	private ClientTabletopConveyenceReceiver m_clientReceiver;

	// Token: 0x02000567 RID: 1383
	private class ItemData
	{
		// Token: 0x040014B5 RID: 5301
		public Transform m_transform;

		// Token: 0x040014B6 RID: 5302
		public float m_deathProp;

		// Token: 0x040014B7 RID: 5303
		public bool m_alive = true;
	}
}
