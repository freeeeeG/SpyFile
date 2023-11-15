using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200045B RID: 1115
public class ClientCookableContainer : ClientSynchroniserBase, IClientOrderDefinition
{
	// Token: 0x060014A7 RID: 5287 RVA: 0x000709FC File Offset: 0x0006EDFC
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_cookableContainer = (CookableContainer)synchronisedObject;
		this.m_cookingHandler = base.gameObject.RequireComponent<ClientCookingHandler>();
		this.m_cookingHandler.CookingStateChangedCallback += delegate(CookingUIController.State _state)
		{
			this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
			if (_state == CookingUIController.State.Ruined)
			{
				OvercookedAchievementManager overcookedAchievementManager = GameUtils.RequestManager<OvercookedAchievementManager>();
				if (overcookedAchievementManager != null)
				{
					overcookedAchievementManager.IncStat(15, 1f, ControlPadInput.PadNum.One);
				}
			}
		};
		this.m_itemContainer = base.gameObject.GetComponent<ClientIngredientContainer>();
		this.m_itemContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		if (this.m_cookableContainer.m_cosmeticsPrefab != null)
		{
			Transform parent = NetworkUtils.FindVisualRoot(base.gameObject);
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_cookableContainer.m_cosmeticsPrefab, parent);
			if (gameObject != null)
			{
				gameObject.transform.localPosition = Vector3.zero;
				gameObject.transform.localRotation = Quaternion.identity;
			}
		}
		this.m_placementContainer = base.gameObject.RequireComponent<ClientPlacementContainer>();
		this.m_placementContainer.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
	}

	// Token: 0x060014A8 RID: 5288 RVA: 0x00070AF0 File Offset: 0x0006EEF0
	public AssembledDefinitionNode GetOrderComposition()
	{
		ClientMixableContainer clientMixableContainer = base.gameObject.RequestComponent<ClientMixableContainer>();
		AssembledDefinitionNode cookableMixableContents = null;
		bool isMixed = false;
		if (clientMixableContainer != null)
		{
			cookableMixableContents = clientMixableContainer.GetOrderComposition();
			isMixed = clientMixableContainer.GetMixingHandler().IsMixed();
		}
		return this.m_cookableContainer.GetOrderComposition(this.m_itemContainer, this.m_cookingHandler, cookableMixableContents, isMixed);
	}

	// Token: 0x060014A9 RID: 5289 RVA: 0x00070B45 File Offset: 0x0006EF45
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x060014AA RID: 5290 RVA: 0x00070B5E File Offset: 0x0006EF5E
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x060014AB RID: 5291 RVA: 0x00070B77 File Offset: 0x0006EF77
	private void OnContentsChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
	}

	// Token: 0x060014AC RID: 5292 RVA: 0x00070B8A File Offset: 0x0006EF8A
	private bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		return this.m_cookableContainer.AllowItemPlacement(_object, _context, this.m_cookingHandler);
	}

	// Token: 0x060014AD RID: 5293 RVA: 0x00070B9F File Offset: 0x0006EF9F
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.m_placementContainer)
		{
			this.m_placementContainer.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		}
	}

	// Token: 0x060014AE RID: 5294 RVA: 0x00070BCF File Offset: 0x0006EFCF
	public ClientCookingHandler GetCookingHandler()
	{
		return this.m_cookingHandler;
	}

	// Token: 0x04000FDD RID: 4061
	private CookableContainer m_cookableContainer;

	// Token: 0x04000FDE RID: 4062
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};

	// Token: 0x04000FDF RID: 4063
	private ClientIngredientContainer m_itemContainer;

	// Token: 0x04000FE0 RID: 4064
	private ClientCookingHandler m_cookingHandler;

	// Token: 0x04000FE1 RID: 4065
	private ClientPlacementContainer m_placementContainer;
}
