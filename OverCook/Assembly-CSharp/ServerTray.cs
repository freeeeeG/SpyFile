using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D7 RID: 1495
public class ServerTray : ServerPlate
{
	// Token: 0x06001C86 RID: 7302 RVA: 0x0008B5AA File Offset: 0x000899AA
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_tray = (Tray)synchronisedObject;
		this.m_slots = new ServerTray.TrayIngredientContainerAdapter[this.m_tray.m_slots.Length];
		this.CalculateSlots(this.m_ingredientContainer.GetContents());
	}

	// Token: 0x06001C87 RID: 7303 RVA: 0x0008B5E8 File Offset: 0x000899E8
	protected override bool CanPlaceOnPlate(GameObject _gameObject, PlacementContext _context)
	{
		IIngredientContents ingredientContents = this.GetIngredientContents(_gameObject, _context, false);
		return ingredientContents != null && !base.IsReserved() && this.m_tray.CanPlaceOnPlate(_gameObject, ingredientContents);
	}

	// Token: 0x06001C88 RID: 7304 RVA: 0x0008B624 File Offset: 0x00089A24
	public IIngredientContents GetIngredientContents(GameObject _gameObject, PlacementContext _context, bool modifyReal)
	{
		IOrderDefinition orderDefinition = _gameObject.RequestInterface<IOrderDefinition>();
		if (orderDefinition == null)
		{
			return null;
		}
		this.CalculateSlots(this.m_ingredientContainer.GetContents());
		AssembledDefinitionNode orderComposition = orderDefinition.GetOrderComposition();
		this.m_tray.GetIngredientContents(orderComposition, _context, this.m_availableSlots);
		for (int i = 0; i < this.m_availableSlots.Count; i++)
		{
			ServerTray.TrayIngredientContainerAdapter trayIngredientContainerAdapter = this.m_slots[this.m_availableSlots[i]];
			AssembledDefinitionNode[] array = new AssembledDefinitionNode[trayIngredientContainerAdapter.GetContentsCount() + 1];
			for (int j = 0; j < trayIngredientContainerAdapter.GetContentsCount(); j++)
			{
				array[j] = trayIngredientContainerAdapter.GetContentsElement(j);
			}
			array[array.Length - 1] = orderComposition;
			if (GameUtils.GetOrderPlatingPrefab(this.m_tray.GetOrderComposition(array), this.m_tray.m_platingStep) != null)
			{
				return new ServerTray.TrayIngredientContainerAdapter(trayIngredientContainerAdapter, modifyReal);
			}
		}
		return null;
	}

	// Token: 0x06001C89 RID: 7305 RVA: 0x0008B70D File Offset: 0x00089B0D
	public override bool CanTransferToContainer(IIngredientContents _container)
	{
		return base.CanTransferToContainer(_container);
	}

	// Token: 0x06001C8A RID: 7306 RVA: 0x0008B718 File Offset: 0x00089B18
	private void CalculateSlots(AssembledDefinitionNode[] _contents)
	{
		if (this.m_tray.AreContentsDifferent(this.m_previousContents, _contents))
		{
			ServerPlate.IngredientContainerAdapter containerToFilter = new ServerPlate.IngredientContainerAdapter(this.m_ingredientContainer);
			for (int i = 0; i < this.m_slots.Length; i++)
			{
				this.m_slots[i] = new ServerTray.TrayIngredientContainerAdapter(this.m_ingredientContainer, containerToFilter, this.m_tray.m_slots[i], false, this.m_tray);
			}
		}
		this.m_previousContents = _contents;
	}

	// Token: 0x0400164C RID: 5708
	private Tray m_tray;

	// Token: 0x0400164D RID: 5709
	private int m_currentSlot = -1;

	// Token: 0x0400164E RID: 5710
	private ServerTray.TrayIngredientContainerAdapter[] m_slots;

	// Token: 0x0400164F RID: 5711
	private AssembledDefinitionNode[] m_previousContents;

	// Token: 0x04001650 RID: 5712
	private List<int> m_availableSlots = new List<int>();

	// Token: 0x020005D8 RID: 1496
	public class TrayIngredientContainerAdapter : IIngredientContents
	{
		// Token: 0x06001C8B RID: 7307 RVA: 0x0008B790 File Offset: 0x00089B90
		public TrayIngredientContainerAdapter(ServerTray.TrayIngredientContainerAdapter _adapter, bool _addToReal)
		{
			this.m_actualContainer = _adapter.m_actualContainer;
			this.m_addToReal = _addToReal;
			this.m_tray = _adapter.m_tray;
			this.m_contents = new List<AssembledDefinitionNode>(_adapter.m_contents.Count);
			for (int i = 0; i < _adapter.m_contents.Count; i++)
			{
				this.m_contents.Add(_adapter.m_contents[i].Simpilfy());
			}
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x0008B81C File Offset: 0x00089C1C
		public TrayIngredientContainerAdapter(ServerIngredientContainer _container, IIngredientContents _containerToFilter, OrderToPrefabLookup _lookup, bool _addToReal, Tray _tray)
		{
			this.m_actualContainer = _container;
			this.m_addToReal = _addToReal;
			this.m_contents = new List<AssembledDefinitionNode>(_containerToFilter.GetContentsCount());
			this.m_tray = _tray;
			List<OrderContentRestriction> contentRestrictions = _lookup.GetContentRestrictions();
			for (int i = 0; i < contentRestrictions.Count; i++)
			{
				for (int j = _containerToFilter.GetContentsCount() - 1; j >= 0; j--)
				{
					AssembledDefinitionNode contentsElement = _containerToFilter.GetContentsElement(j);
					AssembledDefinitionNode assembledDefinitionNode = contentsElement.Simpilfy();
					if (AssembledDefinitionNode.MatchingAlreadySimple(assembledDefinitionNode, contentRestrictions[i].m_content.Simpilfy()))
					{
						this.m_contents.Add(assembledDefinitionNode);
						if (GameUtils.GetOrderPlatingPrefab(this.m_tray.GetOrderComposition(this.GetContents()), this.m_tray.m_platingStep) == null)
						{
							this.m_contents.Remove(assembledDefinitionNode);
						}
						else
						{
							_containerToFilter.RemoveIngredient(j);
						}
						break;
					}
				}
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x0008B91F File Offset: 0x00089D1F
		public bool CanAddIngredient(AssembledDefinitionNode _orderData)
		{
			return true;
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x0008B922 File Offset: 0x00089D22
		public bool CanTakeContents(AssembledDefinitionNode[] _contents)
		{
			return true;
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x0008B928 File Offset: 0x00089D28
		public void AddIngredient(AssembledDefinitionNode _orderData)
		{
			CompositeAssembledNode compositeAssembledNode = _orderData as CompositeAssembledNode;
			if (compositeAssembledNode != null && compositeAssembledNode.m_permittedEntries.Count > 0)
			{
				for (int i = 0; i < compositeAssembledNode.m_composition.Length; i++)
				{
					if (this.m_addToReal)
					{
						this.m_actualContainer.AddIngredient(compositeAssembledNode.m_composition[i]);
					}
					this.m_contents.Add(compositeAssembledNode.m_composition[i]);
				}
				return;
			}
			if (this.m_addToReal)
			{
				this.m_actualContainer.AddIngredient(_orderData);
			}
			this.m_contents.Add(_orderData);
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x0008B9C4 File Offset: 0x00089DC4
		public AssembledDefinitionNode RemoveIngredient(int i)
		{
			AssembledDefinitionNode result = this.m_contents[i];
			this.m_contents.RemoveAt(i);
			return result;
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x0008B9EB File Offset: 0x00089DEB
		public AssembledDefinitionNode GetContentsElement(int i)
		{
			return this.m_contents[i];
		}

		// Token: 0x06001C92 RID: 7314 RVA: 0x0008B9F9 File Offset: 0x00089DF9
		public int GetContentsCount()
		{
			return this.m_contents.Count;
		}

		// Token: 0x06001C93 RID: 7315 RVA: 0x0008BA06 File Offset: 0x00089E06
		public AssembledDefinitionNode[] GetContents()
		{
			return this.m_contents.ToArray();
		}

		// Token: 0x06001C94 RID: 7316 RVA: 0x0008BA14 File Offset: 0x00089E14
		public void Empty()
		{
			if (this.m_addToReal)
			{
				for (int i = 0; i < this.m_contents.Count; i++)
				{
					for (int j = 0; j < this.m_actualContainer.GetContentsCount(); j++)
					{
						if (AssembledDefinitionNode.MatchingAlreadySimple(this.m_contents[i], this.m_actualContainer.GetContentsElement(j)))
						{
							this.m_actualContainer.RemoveIngredient(j);
							break;
						}
					}
				}
			}
			this.m_contents.Clear();
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x0008BAA3 File Offset: 0x00089EA3
		public bool HasContents()
		{
			return this.m_contents.Count != 0;
		}

		// Token: 0x04001651 RID: 5713
		private List<AssembledDefinitionNode> m_contents = new List<AssembledDefinitionNode>();

		// Token: 0x04001652 RID: 5714
		private ServerIngredientContainer m_actualContainer;

		// Token: 0x04001653 RID: 5715
		private bool m_addToReal;

		// Token: 0x04001654 RID: 5716
		private Tray m_tray;
	}
}
