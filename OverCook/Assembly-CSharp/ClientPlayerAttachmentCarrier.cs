using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000A05 RID: 2565
public class ClientPlayerAttachmentCarrier : ClientSynchroniserBase, IPlayerCarrier, ICarrier, ICarrierPlacement
{
	// Token: 0x0600324B RID: 12875 RVA: 0x000EBD93 File Offset: 0x000EA193
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_attachmentCarrier = (PlayerAttachmentCarrier)synchronisedObject;
	}

	// Token: 0x0600324C RID: 12876 RVA: 0x000EBDA8 File Offset: 0x000EA1A8
	public override EntityType GetEntityType()
	{
		return EntityType.ChefCarry;
	}

	// Token: 0x0600324D RID: 12877 RVA: 0x000EBDAC File Offset: 0x000EA1AC
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		ChefCarryMessage chefCarryMessage = (ChefCarryMessage)serialisable;
		IClientAttachment clientAttachment = this.m_carriedObjects[(int)chefCarryMessage.m_playerAttachTarget];
		IClientAttachment clientAttachment2 = null;
		uint carriableItem = chefCarryMessage.m_carriableItem;
		if (carriableItem != 0U)
		{
			EntitySerialisationEntry entry = EntitySerialisationRegistry.GetEntry(carriableItem);
			IClientAttachment component = entry.m_GameObject.GetComponent<IClientAttachment>();
			clientAttachment2 = component;
		}
		if (clientAttachment2 != clientAttachment)
		{
			if (clientAttachment2 != null)
			{
				this.CarryItem(clientAttachment2.AccessGameObject());
			}
			else
			{
				this.TakeItem(chefCarryMessage.m_playerAttachTarget);
			}
		}
	}

	// Token: 0x0600324E RID: 12878 RVA: 0x000EBE21 File Offset: 0x000EA221
	public GameObject AccessGameObject()
	{
		return base.gameObject;
	}

	// Token: 0x0600324F RID: 12879 RVA: 0x000EBE29 File Offset: 0x000EA229
	public GameObject InspectCarriedItem()
	{
		return this.InspectCarriedItem(PlayerAttachTarget.Default);
	}

	// Token: 0x06003250 RID: 12880 RVA: 0x000EBE32 File Offset: 0x000EA232
	public GameObject InspectCarriedItem(PlayerAttachTarget playerAttachTarget)
	{
		return (!(this.m_carriedObjects[(int)playerAttachTarget] as MonoBehaviour != null)) ? null : this.m_carriedObjects[(int)playerAttachTarget].AccessGameObject();
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x000EBE5F File Offset: 0x000EA25F
	public bool HasAttachment(PlayerAttachTarget playerAttachTarget)
	{
		return this.m_attachmentCarrier.GetAttachPoint(playerAttachTarget).childCount > 0;
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x000EBE78 File Offset: 0x000EA278
	public void CarryItem(GameObject _object)
	{
		PlayerAttachTarget playerAttachTarget = PlayerAttachTarget.Default;
		IHandleAttachTarget handleAttachTarget = _object.RequestInterface<IHandleAttachTarget>();
		if (handleAttachTarget as MonoBehaviour != null)
		{
			playerAttachTarget = handleAttachTarget.PlayerAttachTarget;
		}
		MonoBehaviour monoBehaviour = this.m_carriedObjects[(int)playerAttachTarget] as MonoBehaviour;
		IClientAttachment component = _object.GetComponent<IClientAttachment>();
		if (component != null)
		{
			this.m_carriedObjects[(int)playerAttachTarget] = component;
		}
		foreach (ICarryNotified carryNotified in _object.RequestInterfaces<ICarryNotified>())
		{
			carryNotified.OnCarryBegun(this);
		}
		ClientHandlePickupReferral component2 = _object.GetComponent<ClientHandlePickupReferral>();
		if (component2 && component2.CanBeBlocked(this))
		{
			component2.SetHandlePickupReferree(new ClientPlayerAttachmentCarrier.BlockPickup());
		}
		this.m_carryItemChangedCallback(null, _object);
		WindAccumulator windAccumulator = _object.RequestComponent<WindAccumulator>();
		if (windAccumulator != null)
		{
			windAccumulator.Reset();
		}
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x000EBF50 File Offset: 0x000EA350
	public GameObject TakeItem(PlayerAttachTarget playerAttachTarget)
	{
		if (this.m_carriedObjects[(int)playerAttachTarget] != null)
		{
			IClientAttachment clientAttachment = this.m_carriedObjects[(int)playerAttachTarget];
			ClientHandlePickupReferral component = clientAttachment.AccessGameObject().GetComponent<ClientHandlePickupReferral>();
			if (component)
			{
				component.SetHandlePickupReferree(null);
			}
			ICarryNotified[] array = clientAttachment.AccessGameObject().RequestInterfaces<ICarryNotified>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnCarryEnded(this);
			}
			MonoBehaviour x = clientAttachment as MonoBehaviour;
			if (x != null)
			{
				GameObject gameObject = clientAttachment.AccessGameObject();
				this.m_carryItemChangedCallback(gameObject, null);
				this.m_carriedObjects[(int)playerAttachTarget] = null;
				return gameObject;
			}
		}
		return null;
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x000EBFF2 File Offset: 0x000EA3F2
	public GameObject TakeItem()
	{
		return this.TakeItem(PlayerAttachTarget.Default);
	}

	// Token: 0x06003255 RID: 12885 RVA: 0x000EBFFC File Offset: 0x000EA3FC
	public void DestroyCarriedItem()
	{
		IClientAttachment clientAttachment = this.m_carriedObjects[0];
		MonoBehaviour x = clientAttachment as MonoBehaviour;
		if (x != null)
		{
			GameObject param = clientAttachment.AccessGameObject();
			UnityEngine.Object.DestroyObject(clientAttachment.AccessGameObject());
			this.m_carriedObjects[0] = null;
			this.m_carryItemChangedCallback(param, null);
		}
	}

	// Token: 0x06003256 RID: 12886 RVA: 0x000EC04D File Offset: 0x000EA44D
	public void RegisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
	{
		this.m_carryItemChangedCallback = (VoidGeneric<GameObject, GameObject>)Delegate.Combine(this.m_carryItemChangedCallback, _callback);
	}

	// Token: 0x06003257 RID: 12887 RVA: 0x000EC066 File Offset: 0x000EA466
	public void UnregisterCarriedItemChangeCallback(VoidGeneric<GameObject, GameObject> _callback)
	{
		this.m_carryItemChangedCallback = (VoidGeneric<GameObject, GameObject>)Delegate.Remove(this.m_carryItemChangedCallback, _callback);
	}

	// Token: 0x04002871 RID: 10353
	private PlayerAttachmentCarrier m_attachmentCarrier;

	// Token: 0x04002872 RID: 10354
	private IClientAttachment[] m_carriedObjects = new IClientAttachment[2];

	// Token: 0x04002873 RID: 10355
	private VoidGeneric<GameObject, GameObject> m_carryItemChangedCallback = delegate(GameObject _before, GameObject _after)
	{
	};

	// Token: 0x02000A06 RID: 2566
	private class BlockPickup : IClientHandlePickup, IBaseHandlePickup
	{
		// Token: 0x0600325A RID: 12890 RVA: 0x000EC089 File Offset: 0x000EA489
		public bool CanHandlePickup(ICarrier _carrier)
		{
			return false;
		}

		// Token: 0x0600325B RID: 12891 RVA: 0x000EC08C File Offset: 0x000EA48C
		public int GetPickupPriority()
		{
			return int.MaxValue;
		}
	}
}
