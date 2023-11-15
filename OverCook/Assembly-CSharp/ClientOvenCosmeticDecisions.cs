using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003DD RID: 989
public class ClientOvenCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x0600123F RID: 4671 RVA: 0x0006719C File Offset: 0x0006559C
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_cookingStation = base.gameObject.RequireComponent<CookingStation>();
		this.m_ovenCosmeticDecisions = base.gameObject.RequireComponent<OvenCosmeticDecisions>();
		this.m_attachStation = base.gameObject.GetComponent<ClientAttachStation>();
		this.m_attachStation.RegisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
		this.m_attachStation.RegisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x0006720C File Offset: 0x0006560C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_item != null)
		{
			this.OnItemRemoved(this.m_item);
		}
		if (this.m_attachStation != null)
		{
			this.m_attachStation.UnregisterOnItemAdded(new ClientAttachStation.OnItemAdded(this.OnItemAdded));
			this.m_attachStation.UnregisterOnItemRemoved(new ClientAttachStation.OnItemRemoved(this.OnItemRemoved));
		}
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x00067278 File Offset: 0x00065678
	private void OnItemAdded(IClientAttachment _iHoldable)
	{
		this.m_item = _iHoldable;
		IOrderDefinition orderDefinition = _iHoldable.AccessGameObject().RequestInterface<IOrderDefinition>();
		if (orderDefinition != null)
		{
			orderDefinition.RegisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
		}
		this.SetOccupiedState(this.IsOccupantValid(_iHoldable));
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x000672C0 File Offset: 0x000656C0
	private void OnItemRemoved(IClientAttachment _iHoldable)
	{
		if (this.m_item != null)
		{
			IOrderDefinition orderDefinition = _iHoldable.AccessGameObject().RequestInterface<IOrderDefinition>();
			if (orderDefinition != null)
			{
				orderDefinition.UnregisterOrderCompositionChangedCallback(new OrderCompositionChangedCallback(this.OnOrderCompositionChanged));
			}
			this.m_item = null;
		}
		this.SetOccupiedState(false);
	}

	// Token: 0x06001243 RID: 4675 RVA: 0x0006730A File Offset: 0x0006570A
	private void OnOrderCompositionChanged(AssembledDefinitionNode _contents)
	{
		this.SetOccupiedState(this.IsOrderOccupantValid(_contents));
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x0006731C File Offset: 0x0006571C
	private bool IsOccupantValid(IClientAttachment _iHoldable)
	{
		IClientCookable clientCookable = _iHoldable.AccessGameObject().RequestInterface<IClientCookable>();
		if (clientCookable == null || clientCookable.GetRequiredStationType() != this.m_cookingStation.m_stationType)
		{
			return false;
		}
		IClientOrderDefinition clientOrderDefinition = _iHoldable.AccessGameObject().RequireInterface<IClientOrderDefinition>();
		AssembledDefinitionNode orderComposition = clientOrderDefinition.GetOrderComposition();
		return this.IsOrderOccupantValid(orderComposition);
	}

	// Token: 0x06001245 RID: 4677 RVA: 0x00067378 File Offset: 0x00065778
	private bool IsOrderOccupantValid(AssembledDefinitionNode _contents)
	{
		AssembledDefinitionNode assembledDefinitionNode = _contents.Simpilfy();
		if (assembledDefinitionNode == AssembledDefinitionNode.NullNode)
		{
			return false;
		}
		if (assembledDefinitionNode is CompositeAssembledNode)
		{
			CompositeAssembledNode compositeAssembledNode = _contents as CompositeAssembledNode;
			for (int i = 0; i < compositeAssembledNode.m_composition.Length; i++)
			{
				MixedCompositeAssembledNode mixedCompositeAssembledNode = compositeAssembledNode.m_composition[i] as MixedCompositeAssembledNode;
				if (mixedCompositeAssembledNode != null && mixedCompositeAssembledNode.m_progress != MixedCompositeOrderNode.MixingProgress.Mixed)
				{
					return false;
				}
			}
		}
		else
		{
			MixedCompositeAssembledNode mixedCompositeAssembledNode2 = _contents as MixedCompositeAssembledNode;
			if (mixedCompositeAssembledNode2 != null && mixedCompositeAssembledNode2.m_progress != MixedCompositeOrderNode.MixingProgress.Mixed)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x06001246 RID: 4678 RVA: 0x0006740A File Offset: 0x0006580A
	private void SetOccupiedState(bool _isOccupied)
	{
		this.m_ovenCosmeticDecisions.m_animator.SetBool(ClientOvenCosmeticDecisions.m_Open, !_isOccupied);
	}

	// Token: 0x04000E46 RID: 3654
	private CookingStation m_cookingStation;

	// Token: 0x04000E47 RID: 3655
	private OvenCosmeticDecisions m_ovenCosmeticDecisions;

	// Token: 0x04000E48 RID: 3656
	private IClientAttachment m_item;

	// Token: 0x04000E49 RID: 3657
	private IOrderDefinition m_orderDefinition;

	// Token: 0x04000E4A RID: 3658
	private ClientAttachStation m_attachStation;

	// Token: 0x04000E4B RID: 3659
	private static int m_Open = Animator.StringToHash("Open");
}
