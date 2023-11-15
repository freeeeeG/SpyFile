using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004E4 RID: 1252
public class ClientItemContainer : ClientSynchroniserBase, IClientOrderDefinition
{
	// Token: 0x06001755 RID: 5973 RVA: 0x00077A2C File Offset: 0x00075E2C
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_itemContainer = (ItemContainer)synchronisedObject;
		this.m_ingredientContainer = base.gameObject.GetComponent<ClientIngredientContainer>();
		this.m_ingredientContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		this.m_placementContainer = base.gameObject.RequireComponent<ClientPlacementContainer>();
		this.m_placementContainer.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		if (this.m_itemContainer.m_cosmeticsPrefab != null)
		{
			Transform parent = NetworkUtils.FindVisualRoot(base.gameObject);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_itemContainer.m_cosmeticsPrefab, parent);
			if (gameObject != null)
			{
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
			}
		}
	}

	// Token: 0x06001756 RID: 5974 RVA: 0x00077AF5 File Offset: 0x00075EF5
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x06001757 RID: 5975 RVA: 0x00077B0E File Offset: 0x00075F0E
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x06001758 RID: 5976 RVA: 0x00077B27 File Offset: 0x00075F27
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.m_itemContainer.GetOrderComposition(this.m_ingredientContainer);
	}

	// Token: 0x06001759 RID: 5977 RVA: 0x00077B3C File Offset: 0x00075F3C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.m_placementContainer)
		{
			this.m_placementContainer.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		}
		if (this.m_ingredientContainer != null)
		{
			this.m_ingredientContainer.UnregisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		}
	}

	// Token: 0x0600175A RID: 5978 RVA: 0x00077B9F File Offset: 0x00075F9F
	private void OnContentsChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
	}

	// Token: 0x0600175B RID: 5979 RVA: 0x00077BB2 File Offset: 0x00075FB2
	private bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		return this.m_itemContainer.AllowItemPlacement(_object, _context);
	}

	// Token: 0x04001122 RID: 4386
	private ItemContainer m_itemContainer;

	// Token: 0x04001123 RID: 4387
	private ClientIngredientContainer m_ingredientContainer;

	// Token: 0x04001124 RID: 4388
	private ClientPlacementContainer m_placementContainer;

	// Token: 0x04001125 RID: 4389
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};
}
