using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004EC RID: 1260
public class ClientLadleContainer : ClientSynchroniserBase, IClientOrderDefinition
{
	// Token: 0x06001783 RID: 6019 RVA: 0x00077FF4 File Offset: 0x000763F4
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_ladleContainer = (LadleContainer)synchronisedObject;
		this.m_ingredientContainer = base.gameObject.GetComponent<ClientIngredientContainer>();
		this.m_ingredientContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		this.m_PlacementContainer = base.gameObject.RequireComponent<ClientPlacementContainer>();
		this.m_PlacementContainer.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
	}

	// Token: 0x06001784 RID: 6020 RVA: 0x0007805D File Offset: 0x0007645D
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.m_ladleContainer.GetOrderComposition(this.m_ingredientContainer.GetContents());
	}

	// Token: 0x06001785 RID: 6021 RVA: 0x00078075 File Offset: 0x00076475
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x06001786 RID: 6022 RVA: 0x0007808E File Offset: 0x0007648E
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x06001787 RID: 6023 RVA: 0x000780A7 File Offset: 0x000764A7
	private bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		return this.m_ladleContainer.AllowItemPlacement(_object, _context, this.m_ingredientContainer);
	}

	// Token: 0x06001788 RID: 6024 RVA: 0x000780BC File Offset: 0x000764BC
	private void OnContentsChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
	}

	// Token: 0x06001789 RID: 6025 RVA: 0x000780CF File Offset: 0x000764CF
	protected override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_PlacementContainer != null)
		{
			this.m_PlacementContainer.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		}
	}

	// Token: 0x04001131 RID: 4401
	private LadleContainer m_ladleContainer;

	// Token: 0x04001132 RID: 4402
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};

	// Token: 0x04001133 RID: 4403
	private ClientIngredientContainer m_ingredientContainer;

	// Token: 0x04001134 RID: 4404
	private ClientPlacementContainer m_PlacementContainer;
}
