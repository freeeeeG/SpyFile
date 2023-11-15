using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000427 RID: 1063
public class ServerAttachItemSpawner : ServerSynchroniserBase
{
	// Token: 0x06001349 RID: 4937 RVA: 0x0006BF9C File Offset: 0x0006A39C
	public override EntityType GetEntityType()
	{
		return EntityType.PlacementItemSpawner;
	}

	// Token: 0x0600134A RID: 4938 RVA: 0x0006BFA0 File Offset: 0x0006A3A0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_attachItemSpawner = (AttachItemSpawner)synchronisedObject;
		this.m_orderDefinition = base.gameObject.RequireInterface<IOrderDefinition>();
		this.m_attachStation = base.gameObject.RequestComponent<ServerAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ServerAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_holderAdapter = new ServerAttachItemSpawner.HolderAdapter(this);
	}

	// Token: 0x0600134B RID: 4939 RVA: 0x0006C008 File Offset: 0x0006A408
	public void OnItemAdded(IAttachment _iHoldable)
	{
		ServerPlacementContainer serverPlacementContainer = _iHoldable.AccessGameObject().RequestComponent<ServerPlacementContainer>();
		if (serverPlacementContainer != null)
		{
			Vector2 left = Vector2.left;
			PlacementContext context = new PlacementContext(PlacementContext.Source.Player);
			if (serverPlacementContainer.CanHandlePlacement(this.m_holderAdapter, left, context))
			{
				serverPlacementContainer.HandlePlacement(this.m_holderAdapter, left, context);
				this.SendServerEvent(this.m_data);
			}
		}
	}

	// Token: 0x04000F33 RID: 3891
	private AttachItemSpawner m_attachItemSpawner;

	// Token: 0x04000F34 RID: 3892
	private PlacementItemSpawnerMessage m_data = new PlacementItemSpawnerMessage();

	// Token: 0x04000F35 RID: 3893
	private IOrderDefinition m_orderDefinition;

	// Token: 0x04000F36 RID: 3894
	private ServerAttachStation m_attachStation;

	// Token: 0x04000F37 RID: 3895
	private ServerAttachItemSpawner.HolderAdapter m_holderAdapter;

	// Token: 0x02000428 RID: 1064
	private class HolderAdapter : ICarrier, ICarrierPlacement
	{
		// Token: 0x0600134C RID: 4940 RVA: 0x0006C068 File Offset: 0x0006A468
		public HolderAdapter(ServerAttachItemSpawner _spawner)
		{
			this.m_itemSpawner = _spawner;
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0006C077 File Offset: 0x0006A477
		public void RegisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0006C079 File Offset: 0x0006A479
		public void UnregisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
		{
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0006C07B File Offset: 0x0006A47B
		public void CarryItem(GameObject _object)
		{
		}

		// Token: 0x06001350 RID: 4944 RVA: 0x0006C07D File Offset: 0x0006A47D
		public GameObject TakeItem()
		{
			return null;
		}

		// Token: 0x06001351 RID: 4945 RVA: 0x0006C080 File Offset: 0x0006A480
		public void DestroyCarriedItem()
		{
		}

		// Token: 0x06001352 RID: 4946 RVA: 0x0006C082 File Offset: 0x0006A482
		public GameObject InspectCarriedItem()
		{
			return this.m_itemSpawner.gameObject;
		}

		// Token: 0x06001353 RID: 4947 RVA: 0x0006C08F File Offset: 0x0006A48F
		public GameObject AccessGameObject()
		{
			return this.m_itemSpawner.gameObject;
		}

		// Token: 0x04000F38 RID: 3896
		private ServerAttachItemSpawner m_itemSpawner;
	}
}
