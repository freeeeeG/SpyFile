using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A02 RID: 2562
public class ServerPlayerAttachmentCarrier : ServerSynchroniserBase, IPlayerCarrier, ICarrier, ICarrierPlacement
{
	// Token: 0x06003231 RID: 12849 RVA: 0x000EB924 File Offset: 0x000E9D24
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_attachmentCarrier = (PlayerAttachmentCarrier)synchronisedObject;
	}

	// Token: 0x06003232 RID: 12850 RVA: 0x000EB939 File Offset: 0x000E9D39
	public override EntityType GetEntityType()
	{
		return EntityType.ChefCarry;
	}

	// Token: 0x06003233 RID: 12851 RVA: 0x000EB93D File Offset: 0x000E9D3D
	public void RegisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
	{
		this.m_carryItemChangedCallback = (VoidGeneric<GameObject, GameObject>)Delegate.Combine(this.m_carryItemChangedCallback, _callback);
	}

	// Token: 0x06003234 RID: 12852 RVA: 0x000EB956 File Offset: 0x000E9D56
	public void UnregisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
	{
		this.m_carryItemChangedCallback = (VoidGeneric<GameObject, GameObject>)Delegate.Remove(this.m_carryItemChangedCallback, _callback);
	}

	// Token: 0x06003235 RID: 12853 RVA: 0x000EB96F File Offset: 0x000E9D6F
	public GameObject InspectCarriedItem()
	{
		return this.InspectCarriedItem(PlayerAttachTarget.Default);
	}

	// Token: 0x06003236 RID: 12854 RVA: 0x000EB978 File Offset: 0x000E9D78
	public GameObject InspectCarriedItem(PlayerAttachTarget playerAttachTarget)
	{
		return (!(this.m_carriedObjects[(int)playerAttachTarget] as MonoBehaviour != null)) ? null : this.m_carriedObjects[(int)playerAttachTarget].AccessGameObject();
	}

	// Token: 0x06003237 RID: 12855 RVA: 0x000EB9A5 File Offset: 0x000E9DA5
	public bool HasAttachment(PlayerAttachTarget playerAttachTarget)
	{
		return this.m_attachmentCarrier.GetAttachPoint(playerAttachTarget).childCount > 0;
	}

	// Token: 0x06003238 RID: 12856 RVA: 0x000EB9BC File Offset: 0x000E9DBC
	public void DestroyCarriedItem()
	{
		IAttachment attachment = this.m_carriedObjects[0];
		GameObject param = attachment.AccessGameObject();
		NetworkUtils.DestroyObjectsRecursive(attachment.AccessGameObject());
		this.m_carriedObjects[0] = null;
		this.m_carryItemChangedCallback(param, null);
		this.m_ServerData.m_carriableItem = 0U;
		this.m_ServerData.m_playerAttachTarget = PlayerAttachTarget.Default;
		this.SendServerEvent(this.m_ServerData);
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x000EBA20 File Offset: 0x000E9E20
	public void CarryItem(GameObject _object)
	{
		PlayerAttachTarget playerAttachTarget = PlayerAttachTarget.Default;
		IHandleAttachTarget handleAttachTarget = _object.RequestInterface<IHandleAttachTarget>();
		if (handleAttachTarget as MonoBehaviour != null)
		{
			playerAttachTarget = handleAttachTarget.PlayerAttachTarget;
		}
		ServerHandlePickupReferral component = _object.GetComponent<ServerHandlePickupReferral>();
		if (component && component.CanBeBlocked(this))
		{
			component.SetHandlePickupReferree(new ServerPlayerAttachmentCarrier.BlockPickup(this));
		}
		ServerAttachmentCatchingProxy component2 = _object.GetComponent<ServerAttachmentCatchingProxy>();
		if (component2)
		{
			component2.SetHandleCatchingReferree(new ServerPlayerAttachmentCarrier.BlockCatching(this));
		}
		IAttachment attachment = this.m_carriedObjects[(int)playerAttachTarget];
		IAttachment component3 = _object.GetComponent<IAttachment>();
		if (component3 != null)
		{
			this.m_carryItemChangedCallback((!(attachment as MonoBehaviour != null)) ? null : attachment.AccessGameObject(), _object);
			ServerLimitedQuantityItem serverLimitedQuantityItem = _object.RequestComponent<ServerLimitedQuantityItem>();
			if (null != serverLimitedQuantityItem)
			{
				serverLimitedQuantityItem.AddInvincibilityCondition(this.m_true);
			}
			this.m_carriedObjects[(int)playerAttachTarget] = component3;
			if (null != serverLimitedQuantityItem)
			{
				serverLimitedQuantityItem.RegisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.OnAttachmentDestroyed));
			}
			component3.Attach(this.m_attachmentCarrier);
			foreach (ICarryNotified carryNotified in _object.RequestInterfaces<ICarryNotified>())
			{
				carryNotified.OnCarryBegun(this);
			}
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(component3.AccessGameObject());
			uint uEntityID = entry.m_Header.m_uEntityID;
			this.m_ServerData.m_carriableItem = uEntityID;
			this.m_ServerData.m_playerAttachTarget = playerAttachTarget;
			this.SendServerEvent(this.m_ServerData);
		}
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x000EBBA4 File Offset: 0x000E9FA4
	public GameObject TakeItem(PlayerAttachTarget playerAttachTarget)
	{
		IAttachment attachment = this.m_carriedObjects[(int)playerAttachTarget];
		ServerHandlePickupReferral component = attachment.AccessGameObject().GetComponent<ServerHandlePickupReferral>();
		if (component)
		{
			component.SetHandlePickupReferree(null);
		}
		attachment.Detach();
		ServerLimitedQuantityItem serverLimitedQuantityItem = attachment.AccessGameObject().RequestComponent<ServerLimitedQuantityItem>();
		if (null != serverLimitedQuantityItem)
		{
			serverLimitedQuantityItem.RemoveInvincibilityCondition(this.m_true);
			serverLimitedQuantityItem.Touch();
		}
		this.m_ServerData.m_carriableItem = 0U;
		this.m_ServerData.m_playerAttachTarget = playerAttachTarget;
		GameObject gameObject = attachment.AccessGameObject();
		this.m_carryItemChangedCallback(gameObject, null);
		if (null != serverLimitedQuantityItem)
		{
			serverLimitedQuantityItem.UnregisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.OnAttachmentDestroyed));
		}
		this.m_carriedObjects[(int)playerAttachTarget] = null;
		this.SendServerEvent(this.m_ServerData);
		return gameObject;
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x000EBC68 File Offset: 0x000EA068
	public GameObject TakeItem()
	{
		return this.TakeItem(PlayerAttachTarget.Default);
	}

	// Token: 0x0600323C RID: 12860 RVA: 0x000EBC74 File Offset: 0x000EA074
	private void OnAttachmentDestroyed(GameObject toBeDestroyed)
	{
		PlayerAttachTarget playerAttachTarget = PlayerAttachTarget.Default;
		IHandleAttachTarget handleAttachTarget = toBeDestroyed.RequestInterface<IHandleAttachTarget>();
		if (handleAttachTarget as MonoBehaviour != null)
		{
			playerAttachTarget = handleAttachTarget.PlayerAttachTarget;
		}
		IAttachment attachment = this.m_carriedObjects[(int)playerAttachTarget];
		IAttachment attachment2 = toBeDestroyed.RequestInterface<IAttachment>();
		if (attachment != null && attachment2 == attachment)
		{
			LimitedQuantityItem component = attachment.AccessGameObject().GetComponent<LimitedQuantityItem>();
			if (null != component)
			{
				component.UnregisterImpendingDestructionNotification(new ImpendingDestructionCallback(this.OnAttachmentDestroyed));
			}
			this.TakeItem();
		}
	}

	// Token: 0x0600323D RID: 12861 RVA: 0x000EBCF3 File Offset: 0x000EA0F3
	public GameObject AccessGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x04002868 RID: 10344
	private PlayerAttachmentCarrier m_attachmentCarrier;

	// Token: 0x04002869 RID: 10345
	private IAttachment[] m_carriedObjects = new IAttachment[2];

	// Token: 0x0400286A RID: 10346
	private VoidGeneric<GameObject, GameObject> m_carryItemChangedCallback = delegate(GameObject _before, GameObject _after)
	{
	};

	// Token: 0x0400286B RID: 10347
	private Generic<bool> m_true = () => true;

	// Token: 0x0400286C RID: 10348
	private ChefCarryMessage m_ServerData = new ChefCarryMessage();

	// Token: 0x02000A03 RID: 2563
	public class BlockPickup : IHandlePickup, IBaseHandlePickup
	{
		// Token: 0x06003240 RID: 12864 RVA: 0x000EBD00 File Offset: 0x000EA100
		public BlockPickup(ServerPlayerAttachmentCarrier _carrier)
		{
			this.m_carrier = _carrier;
		}

		// Token: 0x06003241 RID: 12865 RVA: 0x000EBD0F File Offset: 0x000EA10F
		public bool CanHandlePickup(ICarrier _carrier)
		{
			return false;
		}

		// Token: 0x06003242 RID: 12866 RVA: 0x000EBD12 File Offset: 0x000EA112
		public void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
		{
		}

		// Token: 0x06003243 RID: 12867 RVA: 0x000EBD14 File Offset: 0x000EA114
		public int GetPickupPriority()
		{
			return int.MaxValue;
		}

		// Token: 0x06003244 RID: 12868 RVA: 0x000EBD1B File Offset: 0x000EA11B
		public void ForceDetach()
		{
			if (this.m_carrier.InspectCarriedItem() != null)
			{
				this.m_carrier.TakeItem();
			}
		}

		// Token: 0x0400286F RID: 10351
		private ServerPlayerAttachmentCarrier m_carrier;
	}

	// Token: 0x02000A04 RID: 2564
	public class BlockCatching : IHandleCatch
	{
		// Token: 0x06003245 RID: 12869 RVA: 0x000EBD3F File Offset: 0x000EA13F
		public BlockCatching(ServerPlayerAttachmentCarrier _carrier)
		{
			this.m_carrier = _carrier;
		}

		// Token: 0x06003246 RID: 12870 RVA: 0x000EBD4E File Offset: 0x000EA14E
		public bool CanHandleCatch(ICatchable _object, Vector2 _directionXZ)
		{
			return false;
		}

		// Token: 0x06003247 RID: 12871 RVA: 0x000EBD51 File Offset: 0x000EA151
		public void HandleCatch(ICatchable _object, Vector2 _directionXZ)
		{
		}

		// Token: 0x06003248 RID: 12872 RVA: 0x000EBD53 File Offset: 0x000EA153
		public void AlertToThrownItem(ICatchable _thrown, IThrower _thrower, Vector2 _directionXZ)
		{
		}

		// Token: 0x06003249 RID: 12873 RVA: 0x000EBD55 File Offset: 0x000EA155
		public int GetCatchingPriority()
		{
			return int.MaxValue;
		}

		// Token: 0x04002870 RID: 10352
		private ServerPlayerAttachmentCarrier m_carrier;
	}
}
