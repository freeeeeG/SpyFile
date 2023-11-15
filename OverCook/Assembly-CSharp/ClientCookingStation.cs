using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000462 RID: 1122
public class ClientCookingStation : ClientSynchroniserBase
{
	// Token: 0x060014D8 RID: 5336 RVA: 0x00071E9A File Offset: 0x0007029A
	public override EntityType GetEntityType()
	{
		return EntityType.CookingStation;
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x00071EA0 File Offset: 0x000702A0
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		CookingStationMessage cookingStationMessage = (CookingStationMessage)serialisable;
		this.SetCookerOn(cookingStationMessage.m_isTurnedOn);
		this.SetCooking(cookingStationMessage.m_isCooking);
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x00071ECC File Offset: 0x000702CC
	protected virtual void Awake()
	{
		this.m_cookingStation = base.gameObject.RequireComponent<CookingStation>();
		this.m_attachStation = base.gameObject.RequireComponent<ClientAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_attachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
	}

	// Token: 0x060014DB RID: 5339 RVA: 0x00071F40 File Offset: 0x00070340
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.m_attachStation.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.UnregisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_attachStation.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
	}

	// Token: 0x060014DC RID: 5340 RVA: 0x00071F98 File Offset: 0x00070398
	public override void UpdateSynchronising()
	{
		if (this.m_isTurnedOn)
		{
			if (this.m_isCooking && this.m_activeLoopingAudio == null && this.m_itemPot != null)
			{
				this.m_activeLoopingAudio = new GameLoopingAudioTag?(this.m_itemPot.GetSizzleSoundTag());
				GameUtils.StartAudio(this.m_activeLoopingAudio.Value, this, base.gameObject.layer);
			}
		}
		else if (this.m_activeLoopingAudio != null)
		{
			GameUtils.StopAudio(this.m_activeLoopingAudio.Value, this);
			this.m_activeLoopingAudio = null;
		}
	}

	// Token: 0x060014DD RID: 5341 RVA: 0x00072040 File Offset: 0x00070440
	protected override void OnDisable()
	{
		base.OnDisable();
		if (this.m_activeLoopingAudio != null)
		{
			GameUtils.StopAudio(this.m_activeLoopingAudio.Value, this);
			this.m_activeLoopingAudio = null;
		}
	}

	// Token: 0x060014DE RID: 5342 RVA: 0x00072083 File Offset: 0x00070483
	private void SetCookerOn(bool _isOn)
	{
		if (this.m_cookingStation != null)
		{
			this.m_isTurnedOn = _isOn;
			this.m_cookingStation.SetCookerOn(_isOn);
		}
	}

	// Token: 0x060014DF RID: 5343 RVA: 0x000720A9 File Offset: 0x000704A9
	private void SetCooking(bool _isCooking)
	{
		this.m_isCooking = _isCooking;
	}

	// Token: 0x060014E0 RID: 5344 RVA: 0x000720B4 File Offset: 0x000704B4
	private void OnItemAdded(IClientAttachment _iHoldable)
	{
		this.m_itemPot = _iHoldable.AccessGameObject().RequestInterface<IClientCookable>();
		IClientCookingRegionNotified clientCookingRegionNotified = _iHoldable.AccessGameObject().RequestInterfaceRecursive<IClientCookingRegionNotified>();
		if (clientCookingRegionNotified as MonoBehaviour != null)
		{
			clientCookingRegionNotified.EnterCookingRegion();
		}
	}

	// Token: 0x060014E1 RID: 5345 RVA: 0x000720F8 File Offset: 0x000704F8
	private void OnItemRemoved(IClientAttachment _iHoldable)
	{
		this.SetCookerOn(false);
		this.SetCooking(false);
		if (this.m_itemPot != null)
		{
			this.m_itemPot = null;
		}
		IClientCookingRegionNotified clientCookingRegionNotified = _iHoldable.AccessGameObject().RequestInterfaceRecursive<IClientCookingRegionNotified>();
		if (clientCookingRegionNotified as MonoBehaviour != null)
		{
			clientCookingRegionNotified.ExitCookingRegion();
		}
	}

	// Token: 0x060014E2 RID: 5346 RVA: 0x00072148 File Offset: 0x00070548
	private bool CanAddItem(GameObject _object, PlacementContext _context)
	{
		PlacementContext.Source source = _context.m_source;
		if (source == PlacementContext.Source.Game)
		{
			return true;
		}
		if (source != PlacementContext.Source.Player)
		{
			return false;
		}
		MixedCompositeOrderNode.MixingProgress? mixingProgress = null;
		IClientMixable clientMixable = _object.RequestInterface<IClientMixable>();
		if (clientMixable != null)
		{
			mixingProgress = new MixedCompositeOrderNode.MixingProgress?(clientMixable.GetMixedOrderState());
		}
		return this.m_cookingStation.CanAddItem(_object, mixingProgress);
	}

	// Token: 0x0400100E RID: 4110
	private CookingStation m_cookingStation;

	// Token: 0x0400100F RID: 4111
	private ClientAttachStation m_attachStation;

	// Token: 0x04001010 RID: 4112
	private GameLoopingAudioTag? m_activeLoopingAudio;

	// Token: 0x04001011 RID: 4113
	private IClientCookable m_itemPot;

	// Token: 0x04001012 RID: 4114
	private bool m_isTurnedOn;

	// Token: 0x04001013 RID: 4115
	private bool m_isCooking;
}
