using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200053A RID: 1338
public class ServerPlacementItemSpawner : ServerSynchroniserBase, IHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x0600190C RID: 6412 RVA: 0x0007F46F File Offset: 0x0007D86F
	public override EntityType GetEntityType()
	{
		return EntityType.PlacementItemSpawner;
	}

	// Token: 0x0600190D RID: 6413 RVA: 0x0007F474 File Offset: 0x0007D874
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_placementItemSpawner = (PlacementItemSpawner)synchronisedObject;
		this.m_orderDefinition = base.gameObject.RequireInterface<IOrderDefinition>();
		this.m_attachStation = base.gameObject.RequestComponent<ServerAttachStation>();
		this.m_holderAdapter = new ServerPlacementItemSpawner.HolderAdapter(this);
	}

	// Token: 0x0600190E RID: 6414 RVA: 0x0007F4C4 File Offset: 0x0007D8C4
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		ServerPlacementContainer serverPlacementContainer = _carrier.InspectCarriedItem().RequestComponent<ServerPlacementContainer>();
		return serverPlacementContainer != null && serverPlacementContainer.CanHandlePlacement(this.m_holderAdapter, -_directionXZ, _context);
	}

	// Token: 0x0600190F RID: 6415 RVA: 0x0007F500 File Offset: 0x0007D900
	public void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		ServerPlacementContainer serverPlacementContainer = _carrier.InspectCarriedItem().RequestComponent<ServerPlacementContainer>();
		serverPlacementContainer.HandlePlacement(this.m_holderAdapter, -_directionXZ, _context);
		IngredientAssembledNode ingredientAssembledNode = this.m_orderDefinition.GetOrderComposition() as IngredientAssembledNode;
		if (ingredientAssembledNode != null && this.m_placementItemSpawner.m_condimentAchievementFilter != null && this.m_placementItemSpawner.m_condimentAchievementFilter.m_ids.Contains(ingredientAssembledNode.m_ingriedientOrderNode.m_uID))
		{
			ServerMessenger.Achievement(_carrier.AccessGameObject(), 803, 1);
		}
		this.SendServerEvent(this.m_data);
	}

	// Token: 0x06001910 RID: 6416 RVA: 0x0007F59C File Offset: 0x0007D99C
	public int GetPlacementPriority()
	{
		return this.m_placementItemSpawner.m_pickupPriority;
	}

	// Token: 0x06001911 RID: 6417 RVA: 0x0007F5A9 File Offset: 0x0007D9A9
	public void OnFailedToPlace(GameObject _item)
	{
	}

	// Token: 0x04001414 RID: 5140
	private PlacementItemSpawner m_placementItemSpawner;

	// Token: 0x04001415 RID: 5141
	private PlacementItemSpawnerMessage m_data = new PlacementItemSpawnerMessage();

	// Token: 0x04001416 RID: 5142
	private IOrderDefinition m_orderDefinition;

	// Token: 0x04001417 RID: 5143
	private ServerAttachStation m_attachStation;

	// Token: 0x04001418 RID: 5144
	private ServerPlacementItemSpawner.HolderAdapter m_holderAdapter;

	// Token: 0x0200053B RID: 1339
	private class HolderAdapter : ICarrier, ICarrierPlacement
	{
		// Token: 0x06001912 RID: 6418 RVA: 0x0007F5AB File Offset: 0x0007D9AB
		public HolderAdapter(ServerPlacementItemSpawner _spawner)
		{
			this.m_itemSpawner = _spawner;
		}

		// Token: 0x06001913 RID: 6419 RVA: 0x0007F5BA File Offset: 0x0007D9BA
		public void RegisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x06001914 RID: 6420 RVA: 0x0007F5BC File Offset: 0x0007D9BC
		public void UnregisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x06001915 RID: 6421 RVA: 0x0007F5BE File Offset: 0x0007D9BE
		public void CarryItem(GameObject _object)
		{
		}

		// Token: 0x06001916 RID: 6422 RVA: 0x0007F5C0 File Offset: 0x0007D9C0
		public GameObject TakeItem()
		{
			return null;
		}

		// Token: 0x06001917 RID: 6423 RVA: 0x0007F5C3 File Offset: 0x0007D9C3
		public void DestroyCarriedItem()
		{
		}

		// Token: 0x06001918 RID: 6424 RVA: 0x0007F5C5 File Offset: 0x0007D9C5
		public GameObject InspectCarriedItem()
		{
			return this.m_itemSpawner.gameObject;
		}

		// Token: 0x06001919 RID: 6425 RVA: 0x0007F5D2 File Offset: 0x0007D9D2
		public GameObject AccessGameObject()
		{
			return this.m_itemSpawner.gameObject;
		}

		// Token: 0x04001419 RID: 5145
		private ServerPlacementItemSpawner m_itemSpawner;
	}
}
