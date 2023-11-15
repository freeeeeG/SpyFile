using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000545 RID: 1349
public class ClientPlate : ClientSynchroniserBase, IClientOrderDefinition
{
	// Token: 0x06001958 RID: 6488 RVA: 0x0007FD70 File Offset: 0x0007E170
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_plate = (Plate)synchronisedObject;
		ClientAnticipateInteractionHighlight clientAnticipateInteractionHighlight = base.gameObject.RequestComponent<ClientAnticipateInteractionHighlight>();
		if (clientAnticipateInteractionHighlight != null)
		{
			Transform parent = base.transform.parent;
			if (parent != null)
			{
				ClientAnticipateInteractionHighlight clientAnticipateInteractionHighlight2 = parent.gameObject.RequestComponentUpwardsRecursive<ClientAnticipateInteractionHighlight>();
				if (clientAnticipateInteractionHighlight2 != null)
				{
					clientAnticipateInteractionHighlight2.AddChild(clientAnticipateInteractionHighlight);
				}
			}
		}
		this.m_ingredientContainer = base.gameObject.GetComponent<ClientIngredientContainer>();
		this.m_ingredientContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		this.m_placementContainer = base.gameObject.RequireComponent<ClientPlacementContainer>();
		this.m_placementContainer.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanPlaceOnPlate));
	}

	// Token: 0x06001959 RID: 6489 RVA: 0x0007FE29 File Offset: 0x0007E229
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.GetOrderComposition(this.m_ingredientContainer.GetContents());
	}

	// Token: 0x0600195A RID: 6490 RVA: 0x0007FE3C File Offset: 0x0007E23C
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x0600195B RID: 6491 RVA: 0x0007FE55 File Offset: 0x0007E255
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x0600195C RID: 6492 RVA: 0x0007FE6E File Offset: 0x0007E26E
	private void OnContentsChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x0007FE81 File Offset: 0x0007E281
	private AssembledDefinitionNode GetOrderComposition(AssembledDefinitionNode[] _contents)
	{
		return this.m_plate.GetOrderComposition(_contents);
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x0007FE8F File Offset: 0x0007E28F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.m_placementContainer)
		{
			this.m_placementContainer.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanPlaceOnPlate));
		}
	}

	// Token: 0x0600195F RID: 6495 RVA: 0x0007FEC0 File Offset: 0x0007E2C0
	protected virtual bool CanPlaceOnPlate(GameObject _gameObject, PlacementContext _context)
	{
		return this.m_plate.CanPlaceOnPlate(_gameObject, new ClientPlate.IngredientContainerAdapter(this.m_ingredientContainer));
	}

	// Token: 0x04001436 RID: 5174
	private Plate m_plate;

	// Token: 0x04001437 RID: 5175
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};

	// Token: 0x04001438 RID: 5176
	protected ClientIngredientContainer m_ingredientContainer;

	// Token: 0x04001439 RID: 5177
	private ClientPlacementContainer m_placementContainer;

	// Token: 0x0400143A RID: 5178
	private WaitForSeconds m_waitForPfxDelay;

	// Token: 0x02000546 RID: 1350
	public class IngredientContainerAdapter : IIngredientContents
	{
		// Token: 0x06001961 RID: 6497 RVA: 0x0007FEDC File Offset: 0x0007E2DC
		public IngredientContainerAdapter(ClientIngredientContainer _container)
		{
			this.m_contents = new List<AssembledDefinitionNode>(_container.GetContentsCount());
			for (int i = 0; i < _container.GetContentsCount(); i++)
			{
				AssembledDefinitionNode contentsElement = _container.GetContentsElement(i);
				AssembledDefinitionNode item = contentsElement.Simpilfy();
				this.m_contents.Add(item);
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0007FF3D File Offset: 0x0007E33D
		public bool CanAddIngredient(AssembledDefinitionNode _orderData)
		{
			return true;
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0007FF40 File Offset: 0x0007E340
		public bool CanTakeContents(AssembledDefinitionNode[] _contents)
		{
			return true;
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0007FF43 File Offset: 0x0007E343
		public void AddIngredient(AssembledDefinitionNode _orderData)
		{
			this.m_contents.Add(_orderData);
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0007FF54 File Offset: 0x0007E354
		public AssembledDefinitionNode RemoveIngredient(int i)
		{
			AssembledDefinitionNode result = this.m_contents[i];
			this.m_contents.RemoveAt(i);
			return result;
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0007FF7B File Offset: 0x0007E37B
		public AssembledDefinitionNode GetContentsElement(int i)
		{
			return this.m_contents[i];
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0007FF89 File Offset: 0x0007E389
		public int GetContentsCount()
		{
			return this.m_contents.Count;
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0007FF96 File Offset: 0x0007E396
		public AssembledDefinitionNode[] GetContents()
		{
			return this.m_contents.ToArray();
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0007FFA3 File Offset: 0x0007E3A3
		public void Empty()
		{
			this.m_contents.Clear();
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0007FFB0 File Offset: 0x0007E3B0
		public bool HasContents()
		{
			return this.m_contents.Count != 0;
		}

		// Token: 0x0400143C RID: 5180
		private List<AssembledDefinitionNode> m_contents = new List<AssembledDefinitionNode>();
	}
}
