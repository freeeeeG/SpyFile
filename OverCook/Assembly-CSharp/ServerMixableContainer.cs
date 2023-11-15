using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000520 RID: 1312
public class ServerMixableContainer : ServerSynchroniserBase, IOrderDefinition, IContainerTransferBehaviour
{
	// Token: 0x0600186F RID: 6255 RVA: 0x0007C548 File Offset: 0x0007A948
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_mixableContainer = (MixableContainer)synchronisedObject;
		this.m_MixingHandler = base.gameObject.RequireComponent<ServerMixingHandler>();
		ServerMixingHandler mixingHandler = this.m_MixingHandler;
		mixingHandler.m_stateChangedCallback = (StateChanged)Delegate.Combine(mixingHandler.m_stateChangedCallback, new StateChanged(delegate(CookingUIController.State _state)
		{
			this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
		}));
		this.m_MixingHandler.enabled = false;
		this.m_PlacementContainer = base.gameObject.RequireComponent<ServerPlacementContainer>();
		this.m_PlacementContainer.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		this.m_IngredientContainer = base.gameObject.GetComponent<ServerIngredientContainer>();
		this.m_IngredientContainer.RegisterContentsChangedCallback(new ContentsChangedCallback(this.OnContentsChanged));
		this.m_IngredientCatcher = base.gameObject.RequestComponent<ServerIngredientCatcher>();
		if (this.m_IngredientCatcher != null)
		{
			this.m_IngredientCatcher.RegisterAllowItemCatching(new QueryForCatching(this.AllowItemCatching));
		}
	}

	// Token: 0x06001870 RID: 6256 RVA: 0x0007C62E File Offset: 0x0007AA2E
	public bool CanTransferPremixedContents(AssembledDefinitionNode[] _contents, float _normalisedMixingProgress)
	{
		return this.m_IngredientContainer.CanTakeContents(_contents);
	}

	// Token: 0x06001871 RID: 6257 RVA: 0x0007C63C File Offset: 0x0007AA3C
	public void TransferPremixedContents(AssembledDefinitionNode[] _contents, float _normalisedMixingProgress)
	{
		for (int i = 0; i < _contents.Length; i++)
		{
		}
		float receivedProgress = _normalisedMixingProgress * this.m_MixingHandler.AccessMixingTime;
		float num = this.m_MixingHandler.GetMixingProgress();
		num = this.CalculateCombinedMixingProgress(num, this.m_IngredientContainer.GetContentsCount(), receivedProgress, _contents.Length);
		this.m_MixingHandler.SetMixingProgress(num);
		this.m_IngredientContainer.CopyContents(_contents);
		GameUtils.TriggerAudio(GameOneShotAudioTag.AddToPot, base.gameObject.layer);
	}

	// Token: 0x06001872 RID: 6258 RVA: 0x0007C6BC File Offset: 0x0007AABC
	private float CalculateCombinedMixingProgress(float recipientProgress, int recipientContents, float receivedProgress, int receivedContents)
	{
		if (Mathf.Max(recipientProgress, receivedProgress) > 2f * this.m_MixingHandler.AccessMixingTime)
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
		return (Mathf.Min(recipientProgress, this.m_MixingHandler.AccessMixingTime) + Mathf.Min(receivedProgress, this.m_MixingHandler.AccessMixingTime)) * 0.5f;
	}

	// Token: 0x06001873 RID: 6259 RVA: 0x0007C728 File Offset: 0x0007AB28
	private bool AllowItemPlacement(GameObject _object, PlacementContext _context)
	{
		float cookingProgress = 0f;
		ServerCookableContainer serverCookableContainer = base.gameObject.RequestComponent<ServerCookableContainer>();
		if (serverCookableContainer != null)
		{
			ServerCookingHandler cookingHandler = serverCookableContainer.GetCookingHandler();
			if (cookingHandler != null)
			{
				cookingProgress = cookingHandler.GetCookingProgress();
			}
		}
		return this.m_mixableContainer.AllowItemPlacement(_object, _context, this.m_mixableContainer.m_ApprovedIngredients, this.m_MixingHandler.IsOverMixed(), cookingProgress);
	}

	// Token: 0x06001874 RID: 6260 RVA: 0x0007C794 File Offset: 0x0007AB94
	private bool AllowItemCatching(GameObject _object)
	{
		return this.AllowItemPlacement(_object, default(PlacementContext));
	}

	// Token: 0x06001875 RID: 6261 RVA: 0x0007C7B4 File Offset: 0x0007ABB4
	public void OnContentsChanged(AssembledDefinitionNode[] _contents)
	{
		this.m_orderCompositionChangedCallbacks(this.GetOrderComposition());
		this.m_MixingHandler.enabled = (_contents.Length > 0);
		this.m_MixingHandler.SetMixingProgress((_contents.Length <= 0) ? 0f : this.m_MixingHandler.GetMixingProgress());
	}

	// Token: 0x06001876 RID: 6262 RVA: 0x0007C80C File Offset: 0x0007AC0C
	public AssembledDefinitionNode GetOrderComposition()
	{
		return this.m_mixableContainer.GetOrderComposition(this.m_IngredientContainer, this.m_MixingHandler.GetMixingProgress() / this.m_MixingHandler.AccessMixingTime, this.m_MixingHandler.GetMixedOrderState());
	}

	// Token: 0x06001877 RID: 6263 RVA: 0x0007C841 File Offset: 0x0007AC41
	public void RegisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Combine(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x06001878 RID: 6264 RVA: 0x0007C85A File Offset: 0x0007AC5A
	public void UnregisterOrderCompositionChangedCallback(OrderCompositionChangedCallback _callback)
	{
		this.m_orderCompositionChangedCallbacks = (OrderCompositionChangedCallback)Delegate.Remove(this.m_orderCompositionChangedCallbacks, _callback);
	}

	// Token: 0x06001879 RID: 6265 RVA: 0x0007C873 File Offset: 0x0007AC73
	public void TransferToContainer(ICarrierPlacement _carrier, IIngredientContents _plateContainer, bool _dontRemove)
	{
		AssembledNodeTransfer.TransferFromContainer<ServerMixableContainer>(this, _plateContainer, _dontRemove);
	}

	// Token: 0x0600187A RID: 6266 RVA: 0x0007C87D File Offset: 0x0007AC7D
	public bool CanTransferToContainer(IIngredientContents _container)
	{
		return AssembledNodeTransfer.CanTransferFromContainer<ServerMixableContainer>(this, _container);
	}

	// Token: 0x0600187B RID: 6267 RVA: 0x0007C886 File Offset: 0x0007AC86
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_PlacementContainer != null)
		{
			this.m_PlacementContainer.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.AllowItemPlacement));
		}
	}

	// Token: 0x0600187C RID: 6268 RVA: 0x0007C8B6 File Offset: 0x0007ACB6
	public ServerMixingHandler GetMixingHandler()
	{
		return this.m_MixingHandler;
	}

	// Token: 0x040013AC RID: 5036
	private MixableContainer m_mixableContainer;

	// Token: 0x040013AD RID: 5037
	private ServerMixingHandler m_MixingHandler;

	// Token: 0x040013AE RID: 5038
	private ServerIngredientContainer m_IngredientContainer;

	// Token: 0x040013AF RID: 5039
	private ServerPlacementContainer m_PlacementContainer;

	// Token: 0x040013B0 RID: 5040
	private ServerIngredientCatcher m_IngredientCatcher;

	// Token: 0x040013B1 RID: 5041
	private OrderCompositionChangedCallback m_orderCompositionChangedCallbacks = delegate(AssembledDefinitionNode _definition)
	{
	};
}
