using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004AE RID: 1198
public class ServerHeatedStation : ServerSynchroniserBase, IHeatContainer, IHandlePlacement, IHandleCatch, IBaseHandlePlacement
{
	// Token: 0x0600163E RID: 5694 RVA: 0x000763CB File Offset: 0x000747CB
	public override EntityType GetEntityType()
	{
		return EntityType.HeatedStation;
	}

	// Token: 0x0600163F RID: 5695 RVA: 0x000763CF File Offset: 0x000747CF
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_heatedStation = (HeatedStation)synchronisedObject;
		this.m_attachStation = base.gameObject.RequestComponent<ServerAttachStation>();
	}

	// Token: 0x06001640 RID: 5696 RVA: 0x000763F5 File Offset: 0x000747F5
	private void SynchroniseHeat()
	{
		this.m_data.m_msgType = HeatedStationMessage.MsgType.Heat;
		this.m_data.m_heat = this.m_heatValue;
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001641 RID: 5697 RVA: 0x00076420 File Offset: 0x00074820
	private void SynchroniseItemAdded()
	{
		this.m_data.m_msgType = HeatedStationMessage.MsgType.ItemAdded;
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001642 RID: 5698 RVA: 0x0007643A File Offset: 0x0007483A
	public static List<ServerHeatedStation> GetAllHeatedStations()
	{
		return ServerHeatedStation.s_allHeatedStations;
	}

	// Token: 0x06001643 RID: 5699 RVA: 0x00076441 File Offset: 0x00074841
	public void RegisterHeatRangeChangedCallback(GenericVoid<HeatRange> _callback)
	{
		this.m_heatRangeChanged = (GenericVoid<HeatRange>)Delegate.Combine(this.m_heatRangeChanged, _callback);
	}

	// Token: 0x06001644 RID: 5700 RVA: 0x0007645A File Offset: 0x0007485A
	public void UnregisterHeatRangeChangedCallback(GenericVoid<HeatRange> _callback)
	{
		this.m_heatRangeChanged = (GenericVoid<HeatRange>)Delegate.Remove(this.m_heatRangeChanged, _callback);
	}

	// Token: 0x06001645 RID: 5701 RVA: 0x00076473 File Offset: 0x00074873
	protected override void OnEnable()
	{
		base.OnEnable();
		ServerHeatedStation.s_allHeatedStations.Add(this);
	}

	// Token: 0x06001646 RID: 5702 RVA: 0x00076486 File Offset: 0x00074886
	protected override void OnDisable()
	{
		base.OnDisable();
		ServerHeatedStation.s_allHeatedStations.Remove(this);
	}

	// Token: 0x06001647 RID: 5703 RVA: 0x0007649C File Offset: 0x0007489C
	public override void UpdateSynchronising()
	{
		base.UpdateSynchronising();
		if (this.m_heatValue > 0f && this.m_heatedStation.m_dissipationRate > 0f)
		{
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

	// Token: 0x06001648 RID: 5704 RVA: 0x0007653F File Offset: 0x0007493F
	public void IncreaseHeat(float _value)
	{
		this.m_heatValue += _value;
		this.m_heatValue = Mathf.Clamp01(this.m_heatValue);
		this.SynchroniseHeat();
	}

	// Token: 0x06001649 RID: 5705 RVA: 0x00076568 File Offset: 0x00074968
	public void ExternalHeatTransfer(IHeatTransferBehaviour _transferBehaviour)
	{
		if (_transferBehaviour != null && _transferBehaviour.CanTransferToContainer(this))
		{
			ICarrier carrier = new ServerHeatedStation.HolderAdapter((_transferBehaviour as MonoBehaviour).gameObject);
			_transferBehaviour.TransferToContainer(carrier, this);
		}
	}

	// Token: 0x0600164A RID: 5706 RVA: 0x000765A0 File Offset: 0x000749A0
	private void BurnAchievement(GameObject _player, GameObject _object)
	{
		IOrderDefinition orderDefinition = _object.RequestInterface<IOrderDefinition>();
		if (orderDefinition != null && orderDefinition.GetOrderComposition() != AssembledDefinitionNode.NullNode)
		{
			ItemAssembledNode itemAssembledNode = orderDefinition.GetOrderComposition() as ItemAssembledNode;
			if (itemAssembledNode != null && this.m_heatedStation.m_burnAchievementFilter != null && this.m_heatedStation.m_burnAchievementFilter.m_ids.Contains(itemAssembledNode.m_itemOrderNode.m_uID))
			{
				ServerMessenger.Achievement(_player, 501, 1);
			}
			CompositeAssembledNode compositeAssembledNode = orderDefinition.GetOrderComposition() as CompositeAssembledNode;
			if (compositeAssembledNode != null)
			{
				for (int i = 0; i < compositeAssembledNode.m_composition.Length; i++)
				{
					ItemAssembledNode itemAssembledNode2 = compositeAssembledNode.m_composition[i] as ItemAssembledNode;
					if (itemAssembledNode2 != null && itemAssembledNode2.m_itemOrderNode.m_uID == this.m_heatedStation.m_coalOrderNode.m_uID)
					{
						ServerMessenger.Achievement(_player, 701, 1);
						break;
					}
				}
			}
		}
	}

	// Token: 0x0600164B RID: 5707 RVA: 0x00076698 File Offset: 0x00074A98
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		IHeatTransferBehaviour heatTransferBehaviour = _carrier.InspectCarriedItem().RequestInterface<IHeatTransferBehaviour>();
		return (heatTransferBehaviour != null && heatTransferBehaviour.CanTransferToContainer(this)) || (this.m_attachStation != null && this.m_attachStation.CanHandlePlacement(_carrier, _directionXZ, _context));
	}

	// Token: 0x0600164C RID: 5708 RVA: 0x000766E8 File Offset: 0x00074AE8
	public void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		IHeatTransferBehaviour heatTransferBehaviour = _carrier.InspectCarriedItem().RequestInterface<IHeatTransferBehaviour>();
		if (heatTransferBehaviour != null)
		{
			this.BurnAchievement(_carrier.AccessGameObject(), _carrier.InspectCarriedItem());
			heatTransferBehaviour.TransferToContainer(_carrier, this);
			this.SynchroniseItemAdded();
			return;
		}
		if (this.m_attachStation != null && this.m_attachStation.CanHandlePlacement(_carrier, _directionXZ, _context))
		{
			this.m_attachStation.HandlePlacement(_carrier, _directionXZ, _context);
		}
	}

	// Token: 0x0600164D RID: 5709 RVA: 0x0007675A File Offset: 0x00074B5A
	public void OnFailedToPlace(GameObject _item)
	{
	}

	// Token: 0x0600164E RID: 5710 RVA: 0x0007675C File Offset: 0x00074B5C
	public int GetPlacementPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x0600164F RID: 5711 RVA: 0x00076764 File Offset: 0x00074B64
	public bool CanHandleCatch(ICatchable _object, Vector2 _directionXZ)
	{
		if (!_object.AllowCatch(this, _directionXZ))
		{
			return false;
		}
		IHeatTransferBehaviour heatTransferBehaviour = _object.AccessGameObject().RequestInterface<IHeatTransferBehaviour>();
		return (heatTransferBehaviour != null && heatTransferBehaviour.CanTransferToContainer(this)) || (this.m_attachStation != null && this.m_attachStation.CanHandleCatch(_object, _directionXZ));
	}

	// Token: 0x06001650 RID: 5712 RVA: 0x000767C4 File Offset: 0x00074BC4
	public void HandleCatch(ICatchable _object, Vector2 _directionXZ)
	{
		GameObject gameObject = _object.AccessGameObject();
		IHeatTransferBehaviour heatTransferBehaviour = gameObject.RequestInterface<IHeatTransferBehaviour>();
		if (heatTransferBehaviour != null)
		{
			IThrowable throwable = gameObject.RequireInterface<IThrowable>();
			IThrower thrower = throwable.GetThrower();
			if (thrower != null)
			{
				this.BurnAchievement((thrower as MonoBehaviour).gameObject, gameObject);
			}
			ICarrier carrier = new ServerHeatedStation.HolderAdapter(gameObject);
			heatTransferBehaviour.TransferToContainer(carrier, this);
			this.SynchroniseItemAdded();
			return;
		}
		if (this.m_attachStation != null && this.m_attachStation.CanHandleCatch(_object, _directionXZ))
		{
			this.m_attachStation.HandleCatch(_object, _directionXZ);
		}
	}

	// Token: 0x06001651 RID: 5713 RVA: 0x00076853 File Offset: 0x00074C53
	public void AlertToThrownItem(ICatchable _thrown, IThrower _thrower, Vector2 _directionXZ)
	{
	}

	// Token: 0x06001652 RID: 5714 RVA: 0x00076855 File Offset: 0x00074C55
	public int GetCatchingPriority()
	{
		return int.MaxValue;
	}

	// Token: 0x040010DA RID: 4314
	private HeatedStation m_heatedStation;

	// Token: 0x040010DB RID: 4315
	private HeatedStationMessage m_data = new HeatedStationMessage();

	// Token: 0x040010DC RID: 4316
	private static List<ServerHeatedStation> s_allHeatedStations = new List<ServerHeatedStation>();

	// Token: 0x040010DD RID: 4317
	private ServerAttachStation m_attachStation;

	// Token: 0x040010DE RID: 4318
	private float m_heatValue;

	// Token: 0x040010DF RID: 4319
	private HeatRange m_heatRange = HeatRange.Low;

	// Token: 0x040010E0 RID: 4320
	private GenericVoid<HeatRange> m_heatRangeChanged = delegate(HeatRange _newValue)
	{
	};

	// Token: 0x020004AF RID: 1199
	public class HolderAdapter : ICarrier, ICarrierPlacement
	{
		// Token: 0x06001655 RID: 5717 RVA: 0x0007686A File Offset: 0x00074C6A
		public HolderAdapter(GameObject _object)
		{
			this.m_carriedItem = _object;
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x00076879 File Offset: 0x00074C79
		public void RegisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x06001657 RID: 5719 RVA: 0x0007687B File Offset: 0x00074C7B
		public void UnregisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x0007687D File Offset: 0x00074C7D
		public GameObject InspectCarriedItem()
		{
			return this.m_carriedItem;
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x00076885 File Offset: 0x00074C85
		public GameObject AccessGameObject()
		{
			return null;
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00076888 File Offset: 0x00074C88
		public void CarryItem(GameObject _object)
		{
		}

		// Token: 0x0600165B RID: 5723 RVA: 0x0007688C File Offset: 0x00074C8C
		public GameObject TakeItem()
		{
			GameObject carriedItem = this.m_carriedItem;
			this.m_carriedItem = null;
			return carriedItem;
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x000768A8 File Offset: 0x00074CA8
		public void DestroyCarriedItem()
		{
			ServerPlayerRespawnManager.KillOrRespawn(this.m_carriedItem, null);
			this.m_carriedItem = null;
		}

		// Token: 0x040010E2 RID: 4322
		private GameObject m_carriedItem;
	}
}
