using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000551 RID: 1361
public class ServerPlateStation : ServerSynchroniserBase
{
	// Token: 0x060019A9 RID: 6569 RVA: 0x000808A1 File Offset: 0x0007ECA1
	public override EntityType GetEntityType()
	{
		return EntityType.PlateStation;
	}

	// Token: 0x060019AA RID: 6570 RVA: 0x000808A4 File Offset: 0x0007ECA4
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_orderHandler = GameUtils.RequireManagerInterface<IKitchenOrderHandler>();
		this.m_attachStation = base.gameObject.GetComponent<ServerAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_attachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
		this.m_attachStation.RegisterFailedToPlace(new VoidGeneric<GameObject>(this.OnFailedToPlace));
		this.m_plateStation = (PlateStation)synchronisedObject;
		if (this.m_plateStation.m_returnStations != null && this.m_plateStation.m_returnStations.Length > 0)
		{
			this.m_serverReturnStations = this.m_plateStation.m_returnStations.ConvertAll((PlateReturnStation x) => x.gameObject.RequireComponent<ServerPlateReturnStation>());
		}
	}

	// Token: 0x060019AB RID: 6571 RVA: 0x00080992 File Offset: 0x0007ED92
	private void SendDeliverEvent(GameObject _object, bool _success)
	{
		this.m_data.m_delivered = _object;
		this.m_data.m_success = _success;
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x060019AC RID: 6572 RVA: 0x000809B8 File Offset: 0x0007EDB8
	public Transform GetAttachPoint(GameObject gameObject)
	{
		return this.m_attachStation.GetAttachPoint(gameObject);
	}

	// Token: 0x060019AD RID: 6573 RVA: 0x000809C8 File Offset: 0x0007EDC8
	public ServerPlateReturnStation GetReturnStation(PlatingStepData _plateType)
	{
		int num = this.m_serverReturnStations.FindIndex_Predicate((ServerPlateReturnStation x) => x.GetPlatingStep() == _plateType);
		if (num >= 0)
		{
			return this.m_serverReturnStations[num];
		}
		return null;
	}

	// Token: 0x060019AE RID: 6574 RVA: 0x00080A0B File Offset: 0x0007EE0B
	public TeamID GetTeamID()
	{
		return this.m_plateStation.m_teamId;
	}

	// Token: 0x060019AF RID: 6575 RVA: 0x00080A18 File Offset: 0x0007EE18
	private void DeliverCurrentPlate()
	{
		ServerPlate plate = this.m_plate;
		plate.Reserve(this);
		this.m_orderHandler.FoodDelivered(plate.GetOrderComposition(), plate.GetPlatingStep(), this);
		plate.StartDeliverySequence(this);
		this.SendDeliverEvent(plate.gameObject, true);
	}

	// Token: 0x060019B0 RID: 6576 RVA: 0x00080A5F File Offset: 0x0007EE5F
	private bool CanAddItem(GameObject _object, PlacementContext _context)
	{
		return !(this.m_plate == null) || _object.GetComponent<ServerPlate>() != null;
	}

	// Token: 0x060019B1 RID: 6577 RVA: 0x00080A80 File Offset: 0x0007EE80
	private void OnItemAdded(IAttachment _iHoldable)
	{
		this.m_plate = _iHoldable.AccessGameObject().GetComponent<ServerPlate>();
		ServerIngredientContainer component = this.m_plate.GetComponent<ServerIngredientContainer>();
		if (component != null)
		{
			IIngredientContents component2 = component.GetComponent<IIngredientContents>();
			if (component2 != null && component2.GetContentsCount() >= 1)
			{
				AssembledDefinitionNode[] contents = component2.GetContents();
				for (int i = 0; i < contents.Length; i++)
				{
					if (contents[i] != null && contents[i].m_freeObject != null)
					{
						NetworkUtils.DestroyObject(contents[i].m_freeObject);
					}
				}
			}
			this.DeliverCurrentPlate();
		}
	}

	// Token: 0x060019B2 RID: 6578 RVA: 0x00080B18 File Offset: 0x0007EF18
	private void OnItemRemoved(IAttachment _iHoldable)
	{
		this.m_plate = null;
	}

	// Token: 0x060019B3 RID: 6579 RVA: 0x00080B21 File Offset: 0x0007EF21
	private void OnFailedToPlace(GameObject _object)
	{
		this.SendDeliverEvent(_object, false);
	}

	// Token: 0x060019B4 RID: 6580 RVA: 0x00080B2B File Offset: 0x0007EF2B
	private void Awake()
	{
	}

	// Token: 0x04001458 RID: 5208
	private PlateStation m_plateStation;

	// Token: 0x04001459 RID: 5209
	private PlateStationMessage m_data = new PlateStationMessage();

	// Token: 0x0400145A RID: 5210
	private ServerPlateReturnStation[] m_serverReturnStations = new ServerPlateReturnStation[0];

	// Token: 0x0400145B RID: 5211
	private IKitchenOrderHandler m_orderHandler;

	// Token: 0x0400145C RID: 5212
	private ServerAttachStation m_attachStation;

	// Token: 0x0400145D RID: 5213
	private ServerPlate m_plate;
}
