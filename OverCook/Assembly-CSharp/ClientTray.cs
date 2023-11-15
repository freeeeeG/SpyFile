using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005D9 RID: 1497
public class ClientTray : ClientPlate
{
	// Token: 0x06001C97 RID: 7319 RVA: 0x0008BAC9 File Offset: 0x00089EC9
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_tray = (Tray)synchronisedObject;
		this.m_slots = new ClientTray.TrayIngredientContainerAdapter[this.m_tray.m_slots.Length];
		this.CalculateSlots(this.m_ingredientContainer.GetContents());
	}

	// Token: 0x06001C98 RID: 7320 RVA: 0x0008BB08 File Offset: 0x00089F08
	public IIngredientContents GetIngredientContents(GameObject _gameObject, PlacementContext _context, bool modifyReal)
	{
		this.CalculateSlots(this.m_ingredientContainer.GetContents());
		IOrderDefinition orderDefinition = _gameObject.RequestInterface<IOrderDefinition>();
		if (orderDefinition == null)
		{
			return null;
		}
		AssembledDefinitionNode orderComposition = orderDefinition.GetOrderComposition();
		this.m_tray.GetIngredientContents(orderComposition, _context, this.m_availableSlots);
		for (int i = 0; i < this.m_availableSlots.Count; i++)
		{
			ClientTray.TrayIngredientContainerAdapter ingredientContents = this.GetIngredientContents(this.m_availableSlots[i], modifyReal);
			AssembledDefinitionNode[] array = new AssembledDefinitionNode[ingredientContents.GetContentsCount() + 1];
			for (int j = 0; j < ingredientContents.GetContentsCount(); j++)
			{
				array[j] = ingredientContents.GetContentsElement(j);
			}
			array[array.Length - 1] = orderComposition;
			if (GameUtils.GetOrderPlatingPrefab(this.m_tray.GetOrderComposition(array), this.m_tray.m_platingStep) != null)
			{
				return new ClientTray.TrayIngredientContainerAdapter(ingredientContents, modifyReal);
			}
		}
		return null;
	}

	// Token: 0x06001C99 RID: 7321 RVA: 0x0008BBF1 File Offset: 0x00089FF1
	public ClientTray.TrayIngredientContainerAdapter GetIngredientContents(int slot, bool modifyReal)
	{
		this.CalculateSlots(this.m_ingredientContainer.GetContents());
		return new ClientTray.TrayIngredientContainerAdapter(this.m_slots[slot], modifyReal);
	}

	// Token: 0x06001C9A RID: 7322 RVA: 0x0008BC14 File Offset: 0x0008A014
	protected override bool CanPlaceOnPlate(GameObject _gameObject, PlacementContext _context)
	{
		IIngredientContents ingredientContents = this.GetIngredientContents(_gameObject, _context, false);
		return ingredientContents != null && this.m_tray.CanPlaceOnPlate(_gameObject, ingredientContents);
	}

	// Token: 0x06001C9B RID: 7323 RVA: 0x0008BC40 File Offset: 0x0008A040
	private void CalculateSlots(AssembledDefinitionNode[] _contents)
	{
		if (this.m_tray.AreContentsDifferent(this.m_previousContents, _contents))
		{
			ClientPlate.IngredientContainerAdapter containerToFilter = new ClientPlate.IngredientContainerAdapter(this.m_ingredientContainer);
			for (int i = 0; i < this.m_slots.Length; i++)
			{
				this.m_slots[i] = new ClientTray.TrayIngredientContainerAdapter(this.m_ingredientContainer, containerToFilter, this.m_tray.m_slots[i], false, this.m_tray);
			}
		}
		this.m_previousContents = _contents;
	}

	// Token: 0x04001655 RID: 5717
	private Tray m_tray;

	// Token: 0x04001656 RID: 5718
	private ClientTray.TrayIngredientContainerAdapter[] m_slots;

	// Token: 0x04001657 RID: 5719
	private AssembledDefinitionNode[] m_previousContents;

	// Token: 0x04001658 RID: 5720
	private List<int> m_availableSlots = new List<int>();

	// Token: 0x020005DA RID: 1498
	public class TrayIngredientContainerAdapter : IIngredientContents
	{
		// Token: 0x06001C9C RID: 7324 RVA: 0x0008BCB8 File Offset: 0x0008A0B8
		public TrayIngredientContainerAdapter(ClientTray.TrayIngredientContainerAdapter _adapter, bool _addToReal)
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

		// Token: 0x06001C9D RID: 7325 RVA: 0x0008BD44 File Offset: 0x0008A144
		public TrayIngredientContainerAdapter(ClientIngredientContainer _container, IIngredientContents _containerToFilter, OrderToPrefabLookup _lookup, bool _addToReal, Tray _tray)
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

		// Token: 0x06001C9E RID: 7326 RVA: 0x0008BE48 File Offset: 0x0008A248
		public bool Contains(AssembledDefinitionNode _node)
		{
			for (int i = 0; i < this.m_contents.Count; i++)
			{
				if (AssembledDefinitionNode.MatchingAlreadySimple(this.m_contents[i], _node.Simpilfy()))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x0008BE90 File Offset: 0x0008A290
		public bool CanAddIngredient(AssembledDefinitionNode _orderData)
		{
			return true;
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0008BE93 File Offset: 0x0008A293
		public bool CanTakeContents(AssembledDefinitionNode[] _contents)
		{
			return true;
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x0008BE96 File Offset: 0x0008A296
		public void AddIngredient(AssembledDefinitionNode _orderData)
		{
			if (this.m_addToReal)
			{
				this.m_actualContainer.AddIngredient(_orderData);
			}
			this.m_contents.Add(_orderData);
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x0008BEBC File Offset: 0x0008A2BC
		public AssembledDefinitionNode RemoveIngredient(int i)
		{
			AssembledDefinitionNode result = this.m_contents[i];
			this.m_contents.RemoveAt(i);
			return result;
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0008BEE3 File Offset: 0x0008A2E3
		public AssembledDefinitionNode GetContentsElement(int i)
		{
			return this.m_contents[i];
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x0008BEF1 File Offset: 0x0008A2F1
		public int GetContentsCount()
		{
			return this.m_contents.Count;
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x0008BEFE File Offset: 0x0008A2FE
		public AssembledDefinitionNode[] GetContents()
		{
			return this.m_contents.ToArray();
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x0008BF0B File Offset: 0x0008A30B
		public void Empty()
		{
			this.m_contents.Clear();
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0008BF18 File Offset: 0x0008A318
		public bool HasContents()
		{
			return this.m_contents.Count != 0;
		}

		// Token: 0x04001659 RID: 5721
		private List<AssembledDefinitionNode> m_contents = new List<AssembledDefinitionNode>();

		// Token: 0x0400165A RID: 5722
		private ClientIngredientContainer m_actualContainer;

		// Token: 0x0400165B RID: 5723
		private bool m_addToReal;

		// Token: 0x0400165C RID: 5724
		private Tray m_tray;
	}
}
