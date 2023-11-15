using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004B0 RID: 1200
public class ClientHeatedStation : ClientSynchroniserBase, IClientHandlePlacement, IHeatContainer, IBaseHandlePlacement
{
	// Token: 0x0600165E RID: 5726 RVA: 0x00076920 File Offset: 0x00074D20
	public override EntityType GetEntityType()
	{
		return EntityType.HeatedStation;
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x00076924 File Offset: 0x00074D24
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_heatedStation = (HeatedStation)synchronisedObject;
		this.m_attachStation = base.gameObject.RequestComponent<ClientAttachStation>();
	}

	// Token: 0x06001660 RID: 5728 RVA: 0x0007694C File Offset: 0x00074D4C
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		HeatedStationMessage heatedStationMessage = (HeatedStationMessage)serialisable;
		if (heatedStationMessage.m_msgType == HeatedStationMessage.MsgType.Heat)
		{
			this.m_heatValue = heatedStationMessage.m_heat;
		}
		else
		{
			this.m_onItemAdded();
		}
	}

	// Token: 0x17000247 RID: 583
	// (get) Token: 0x06001661 RID: 5729 RVA: 0x00076987 File Offset: 0x00074D87
	public float HeatValue
	{
		get
		{
			return this.m_heatValue;
		}
	}

	// Token: 0x06001662 RID: 5730 RVA: 0x0007698F File Offset: 0x00074D8F
	public void RegisterHeatRangeChangedCallback(GenericVoid<HeatRange> _callback)
	{
		this.m_heatRangeChanged = (GenericVoid<HeatRange>)Delegate.Combine(this.m_heatRangeChanged, _callback);
	}

	// Token: 0x06001663 RID: 5731 RVA: 0x000769A8 File Offset: 0x00074DA8
	public void UnregisterHeatRangeChangedCallback(GenericVoid<HeatRange> _callback)
	{
		this.m_heatRangeChanged = (GenericVoid<HeatRange>)Delegate.Remove(this.m_heatRangeChanged, _callback);
	}

	// Token: 0x06001664 RID: 5732 RVA: 0x000769C1 File Offset: 0x00074DC1
	public void RegisterOnItemAddedCallback(CallbackVoid _callback)
	{
		this.m_onItemAdded = (CallbackVoid)Delegate.Combine(this.m_onItemAdded, _callback);
	}

	// Token: 0x06001665 RID: 5733 RVA: 0x000769DA File Offset: 0x00074DDA
	public void UnregisterOnItemAddedCallback(CallbackVoid _callback)
	{
		this.m_onItemAdded = (CallbackVoid)Delegate.Remove(this.m_onItemAdded, _callback);
	}

	// Token: 0x06001666 RID: 5734 RVA: 0x000769F4 File Offset: 0x00074DF4
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_heatValue > 0f && this.m_heatedStation.m_dissipationRate > 0f)
		{
			float heatValue = this.m_heatValue;
			this.m_heatValue -= TimeManager.GetDeltaTime(base.gameObject) / this.m_heatedStation.m_dissipationRate;
			this.m_heatValue = Mathf.Max(this.m_heatValue, 0f);
		}
		HeatRange heat = this.m_heatedStation.GetHeat(this.m_heatValue);
		if (heat != this.m_heatRange)
		{
			this.m_heatRangeChanged(heat);
		}
		this.m_heatRange = heat;
	}

	// Token: 0x06001667 RID: 5735 RVA: 0x00076A9E File Offset: 0x00074E9E
	public void IncreaseHeat(float _value)
	{
		throw new NotImplementedException();
	}

	// Token: 0x06001668 RID: 5736 RVA: 0x00076AA8 File Offset: 0x00074EA8
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		IHeatTransferBehaviour heatTransferBehaviour = _carrier.InspectCarriedItem().RequestInterface<IHeatTransferBehaviour>();
		return (heatTransferBehaviour != null && heatTransferBehaviour.CanTransferToContainer(this)) || (this.m_attachStation != null && this.m_attachStation.CanHandlePlacement(_carrier, _directionXZ, _context));
	}

	// Token: 0x06001669 RID: 5737 RVA: 0x00076AF7 File Offset: 0x00074EF7
	public int GetPlacementPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x040010E3 RID: 4323
	private HeatedStation m_heatedStation;

	// Token: 0x040010E4 RID: 4324
	private ClientAttachStation m_attachStation;

	// Token: 0x040010E5 RID: 4325
	private float m_heatValue;

	// Token: 0x040010E6 RID: 4326
	private HeatRange m_heatRange = HeatRange.Low;

	// Token: 0x040010E7 RID: 4327
	private GenericVoid<HeatRange> m_heatRangeChanged = delegate(HeatRange _newValue)
	{
	};

	// Token: 0x040010E8 RID: 4328
	private CallbackVoid m_onItemAdded = delegate()
	{
	};
}
