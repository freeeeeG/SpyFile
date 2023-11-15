using System;
using Team17.Online;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x0200045A RID: 1114
public class ServerCookableContainer : ServerSynchroniserBase, IOrderDefinition, IContainerTransferBehaviour
{
	// Token: 0x06001494 RID: 5268 RVA: 0x00070614 File Offset: 0x0006EA14
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_cookableContainer = (CookableContainer)synchronisedObject;
		this.m_cookingHandler = base.gameObject.RequireComponent<ServerCookingHandler>();
		this.m_cookingHandler.CookingStateChangedCallback += delegate(CookingUIController.State _state)
		{
			this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
		};
		this.m_cookingHandler.enabled = false;
		this.m_itemContainer = base.gameObject.GetComponent<ServerIngredientContainer>();
		this.m_itemContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		this.m_placementContainer = base.gameObject.RequireComponent<ServerPlacementContainer>();
		this.m_placementContainer.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		this.m_ingredientCatcher = base.gameObject.RequestComponent<ServerIngredientCatcher>();
		if (this.m_ingredientCatcher != null)
		{
			this.m_ingredientCatcher.RegisterAllowItemCatching(new QueryForCatching(this.AllowItemCatching));
		}
		Mailbox.Server.RegisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x06001495 RID: 5269 RVA: 0x00070704 File Offset: 0x0006EB04
	private void OnGameStateChanged(IOnlineMultiplayerSessionUserId sessionUserId, Serialisable message)
	{
		GameStateMessage gameStateMessage = (GameStateMessage)message;
		if (gameStateMessage.m_State == GameState.StartEntities)
		{
			this.m_cookingHandler.SetCookingProgress(0f);
		}
	}

	// Token: 0x06001496 RID: 5270 RVA: 0x00070735 File Offset: 0x0006EB35
	public bool CanTransferToContainer(IIngredientContents _container)
	{
		return AssembledNodeTransfer.CanTransferFromContainer<ServerCookableContainer>(this, _container);
	}

	// Token: 0x06001497 RID: 5271 RVA: 0x0007073E File Offset: 0x0006EB3E
	public void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _plateContainer, bool _dontRemove)
	{
		AssembledNodeTransfer.TransferFromContainer<ServerCookableContainer>(this, _plateContainer, _dontRemove);
	}

	// Token: 0x06001498 RID: 5272 RVA: 0x00070748 File Offset: 0x0006EB48
	public AssembledDefinitionNode GetOrderComposition()
	{
		ServerMixableContainer serverMixableContainer = base.gameObject.RequestComponent<ServerMixableContainer>();
		AssembledDefinitionNode cookableMixableContents = null;
		bool isMixed = false;
		if (serverMixableContainer != null)
		{
			cookableMixableContents = serverMixableContainer.GetOrderComposition();
			isMixed = serverMixableContainer.GetMixingHandler().IsMixed();
		}
		return this.m_cookableContainer.GetOrderComposition(this.m_itemContainer, this.m_cookingHandler, cookableMixableContents, isMixed);
	}

	// Token: 0x06001499 RID: 5273 RVA: 0x0007079D File Offset: 0x0006EB9D
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x0600149A RID: 5274 RVA: 0x000707B6 File Offset: 0x0006EBB6
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x0600149B RID: 5275 RVA: 0x000707CF File Offset: 0x0006EBCF
	private bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		return this.m_cookableContainer.AllowItemPlacement(_object, _context, this.m_cookingHandler);
	}

	// Token: 0x0600149C RID: 5276 RVA: 0x000707E4 File Offset: 0x0006EBE4
	private bool AllowItemCatching(GameObject _object)
	{
		return this.AllowItemPlacement(_object, default(PlacementContext));
	}

	// Token: 0x0600149D RID: 5277 RVA: 0x00070801 File Offset: 0x0006EC01
	public bool CanTransferPrecookedContents(AssembledDefinitionNode[] _contents, float _normalisedCookedProgress)
	{
		return this.m_itemContainer.CanTakeContents(_contents);
	}

	// Token: 0x0600149E RID: 5278 RVA: 0x00070818 File Offset: 0x0006EC18
	public void TransferPrecookedContents(AssembledDefinitionNode[] _contents, float _normalisedCookedProgress)
	{
		for (int i = 0; i < _contents.Length; i++)
		{
		}
		float receivedProgress = _normalisedCookedProgress * this.m_cookingHandler.AccessCookingTime;
		float num = this.m_cookingHandler.GetCookingProgress();
		num = this.CalculateCombinedCookingProgress(num, this.m_itemContainer.GetContentsCount(), receivedProgress, _contents.Length);
		this.m_cookingHandler.SetCookingProgress(num);
		this.m_itemContainer.CopyContents(_contents);
		GameUtils.TriggerAudio(GameOneShotAudioTag.AddToPot, base.gameObject.layer);
	}

	// Token: 0x0600149F RID: 5279 RVA: 0x00070898 File Offset: 0x0006EC98
	private float CalculateCombinedCookingProgress(float recipientProgress, int recipientContents, float receivedProgress, int receivedContents)
	{
		if (Mathf.Max(recipientProgress, receivedProgress) > 2f * this.m_cookingHandler.AccessCookingTime)
		{
			return Mathf.Max(recipientProgress, receivedProgress);
		}
		if (recipientContents == 0)
		{
			return receivedProgress;
		}
		if (receivedContents == 0)
		{
			return recipientProgress;
		}
		return (Mathf.Min(recipientProgress, this.m_cookingHandler.AccessCookingTime) + Mathf.Min(receivedProgress, this.m_cookingHandler.AccessCookingTime)) * 0.5f;
	}

	// Token: 0x060014A0 RID: 5280 RVA: 0x00070904 File Offset: 0x0006ED04
	public int GetPlacementPriority()
	{
		return 0;
	}

	// Token: 0x060014A1 RID: 5281 RVA: 0x00070908 File Offset: 0x0006ED08
	public void OnContentsChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
		this.m_cookingHandler.enabled = (_contents.Length > 0);
		this.m_cookingHandler.SetCookingProgress((_contents.Length <= 0) ? 0f : this.m_cookingHandler.GetCookingProgress());
	}

	// Token: 0x060014A2 RID: 5282 RVA: 0x00070960 File Offset: 0x0006ED60
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (null != this.m_placementContainer)
		{
			this.m_placementContainer.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		}
		Mailbox.Server.UnregisterForMessageType(MessageType.GameState, new OrderedMessageReceivedCallback(this.OnGameStateChanged));
	}

	// Token: 0x060014A3 RID: 5283 RVA: 0x000709B3 File Offset: 0x0006EDB3
	public ServerCookingHandler GetCookingHandler()
	{
		return this.m_cookingHandler;
	}

	// Token: 0x04000FD5 RID: 4053
	private CookableContainer m_cookableContainer;

	// Token: 0x04000FD6 RID: 4054
	private ServerPlacementContainer m_placementContainer;

	// Token: 0x04000FD7 RID: 4055
	private ServerIngredientCatcher m_ingredientCatcher;

	// Token: 0x04000FD8 RID: 4056
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};

	// Token: 0x04000FD9 RID: 4057
	private float m_hideAfterTime = float.MinValue;

	// Token: 0x04000FDA RID: 4058
	private ServerIngredientContainer m_itemContainer;

	// Token: 0x04000FDB RID: 4059
	private ServerCookingHandler m_cookingHandler;
}
