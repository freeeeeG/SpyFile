using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000543 RID: 1347
public class ServerPlate : ServerSynchroniserBase, IOrderDefinition, IContainerTransferBehaviour
{
	// Token: 0x0600193C RID: 6460 RVA: 0x0007F910 File Offset: 0x0007DD10
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_plate = (Plate)synchronisedObject;
		this.m_reservation = null;
		this.m_ingredientContainer = base.gameObject.GetComponent<ServerIngredientContainer>();
		this.m_ingredientContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		this.m_placementContainer = base.gameObject.GetComponent<ServerPlacementContainer>();
		this.m_placementContainer.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanPlaceOnPlate));
	}

	// Token: 0x0600193D RID: 6461 RVA: 0x0007F984 File Offset: 0x0007DD84
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.m_placementContainer)
		{
			this.m_placementContainer.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanPlaceOnPlate));
		}
		if (null != this.m_ingredientContainer)
		{
			this.m_ingredientContainer.UnregisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		}
	}

	// Token: 0x0600193E RID: 6462 RVA: 0x0007F9E8 File Offset: 0x0007DDE8
	public virtual bool CanTransferToContainer(IIngredientContents _container)
	{
		if (!this.IsReserved() && this.m_ingredientContainer.GetContentsCount() > 0 && _container.CanTakeContents(this.m_ingredientContainer.GetContents()))
		{
			for (int i = 0; i < this.m_ingredientContainer.GetContentsCount(); i++)
			{
				AssembledDefinitionNode contentsElement = this.m_ingredientContainer.GetContentsElement(i);
				if (contentsElement.m_freeObject != null)
				{
					IHandleOrderModification handleOrderModification = contentsElement.m_freeObject.RequestInterface<IHandleOrderModification>();
					if (handleOrderModification != null && !handleOrderModification.CanAddOrderContents(_container.GetContents()))
					{
						return false;
					}
				}
				if (!AssembledNodeTransfer.CanCombineWithContents(contentsElement, _container))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x0600193F RID: 6463 RVA: 0x0007FA98 File Offset: 0x0007DE98
	public void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _otherContainer, bool _dontRemove)
	{
		if (this.m_ingredientContainer.GetContentsCount() > 0 && _otherContainer.CanTakeContents(this.m_ingredientContainer.GetContents()))
		{
			for (int i = 0; i < this.m_ingredientContainer.GetContentsCount(); i++)
			{
				AssembledDefinitionNode assembledDefinitionNode = this.m_ingredientContainer.GetContentsElement(i);
				if (_dontRemove)
				{
					assembledDefinitionNode = assembledDefinitionNode.Simpilfy();
				}
				AssembledNodeTransfer.CombineWithContents(assembledDefinitionNode, _otherContainer, _dontRemove);
			}
			if (!_dontRemove)
			{
				this.m_ingredientContainer.Empty();
			}
		}
	}

	// Token: 0x06001940 RID: 6464 RVA: 0x0007FB1B File Offset: 0x0007DF1B
	private void OnContentsChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
	}

	// Token: 0x06001941 RID: 6465 RVA: 0x0007FB2E File Offset: 0x0007DF2E
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.GetOrderComposition(this.m_ingredientContainer.GetContents());
	}

	// Token: 0x06001942 RID: 6466 RVA: 0x0007FB41 File Offset: 0x0007DF41
	private AssembledDefinitionNode GetOrderComposition(AssembledDefinitionNode[] _contents)
	{
		return this.m_plate.GetOrderComposition(_contents);
	}

	// Token: 0x06001943 RID: 6467 RVA: 0x0007FB4F File Offset: 0x0007DF4F
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x06001944 RID: 6468 RVA: 0x0007FB68 File Offset: 0x0007DF68
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x06001945 RID: 6469 RVA: 0x0007FB81 File Offset: 0x0007DF81
	public PlatingStepData GetPlatingStep()
	{
		return this.m_plate.m_platingStep;
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x0007FB90 File Offset: 0x0007DF90
	private void Awake()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("GameController");
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x0007FBA8 File Offset: 0x0007DFA8
	protected virtual bool CanPlaceOnPlate(GameObject _gameObject, PlacementContext _context)
	{
		return !this.IsReserved() && this.m_plate.CanPlaceOnPlate(_gameObject, new ServerPlate.IngredientContainerAdapter(this.m_ingredientContainer));
	}

	// Token: 0x06001948 RID: 6472 RVA: 0x0007FBD0 File Offset: 0x0007DFD0
	public void StartDeliverySequence(ServerPlateStation _returnStation)
	{
		ServerAttachStation serverAttachStation = _returnStation.gameObject.RequireComponent<ServerAttachStation>();
		GameObject gameObject = serverAttachStation.TakeItem();
		foreach (Collider collider in base.gameObject.RequestComponentsRecursive<Collider>())
		{
			collider.enabled = false;
		}
		Rigidbody rigidbody = base.gameObject.GetComponent<IAttachment>().AccessRigidbody();
		rigidbody.isKinematic = true;
	}

	// Token: 0x06001949 RID: 6473 RVA: 0x0007FC37 File Offset: 0x0007E037
	public void Reserve(UnityEngine.Object reserveOwner)
	{
		this.m_reservation = reserveOwner;
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x0007FC40 File Offset: 0x0007E040
	public void ReleaseReservation(UnityEngine.Object reserveOwner)
	{
		this.m_reservation = null;
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x0007FC49 File Offset: 0x0007E049
	public bool IsReserved()
	{
		return this.m_reservation != null;
	}

	// Token: 0x0400142F RID: 5167
	private Plate m_plate;

	// Token: 0x04001430 RID: 5168
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};

	// Token: 0x04001431 RID: 5169
	protected ServerIngredientContainer m_ingredientContainer;

	// Token: 0x04001432 RID: 5170
	private ServerPlacementContainer m_placementContainer;

	// Token: 0x04001433 RID: 5171
	private UnityEngine.Object m_reservation;

	// Token: 0x02000544 RID: 1348
	public class IngredientContainerAdapter : IIngredientContents
	{
		// Token: 0x0600194D RID: 6477 RVA: 0x0007FC5C File Offset: 0x0007E05C
		public IngredientContainerAdapter(ServerIngredientContainer _container)
		{
			this.m_contents = new List<AssembledDefinitionNode>(_container.GetContentsCount());
			for (int i = 0; i < _container.GetContentsCount(); i++)
			{
				AssembledDefinitionNode contentsElement = _container.GetContentsElement(i);
				AssembledDefinitionNode item = contentsElement.Simpilfy();
				this.m_contents.Add(item);
			}
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x0007FCBD File Offset: 0x0007E0BD
		public bool CanAddIngredient(AssembledDefinitionNode _orderData)
		{
			return true;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0007FCC0 File Offset: 0x0007E0C0
		public bool CanTakeContents(AssembledDefinitionNode[] _contents)
		{
			return true;
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0007FCC3 File Offset: 0x0007E0C3
		public void AddIngredient(AssembledDefinitionNode _orderData)
		{
			this.m_contents.Add(_orderData);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0007FCD4 File Offset: 0x0007E0D4
		public AssembledDefinitionNode RemoveIngredient(int i)
		{
			AssembledDefinitionNode result = this.m_contents[i];
			this.m_contents.RemoveAt(i);
			return result;
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0007FCFB File Offset: 0x0007E0FB
		public AssembledDefinitionNode GetContentsElement(int i)
		{
			return this.m_contents[i];
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x0007FD09 File Offset: 0x0007E109
		public int GetContentsCount()
		{
			return this.m_contents.Count;
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0007FD16 File Offset: 0x0007E116
		public AssembledDefinitionNode[] GetContents()
		{
			return this.m_contents.ToArray();
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0007FD23 File Offset: 0x0007E123
		public void Empty()
		{
			this.m_contents.Clear();
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0007FD30 File Offset: 0x0007E130
		public bool HasContents()
		{
			return this.m_contents.Count != 0;
		}

		// Token: 0x04001435 RID: 5173
		private List<AssembledDefinitionNode> m_contents = new List<AssembledDefinitionNode>();
	}
}
