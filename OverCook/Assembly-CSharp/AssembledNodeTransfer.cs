using System;
using UnityEngine;

// Token: 0x020005E7 RID: 1511
public static class AssembledNodeTransfer
{
	// Token: 0x06001CCC RID: 7372 RVA: 0x0008CE0C File Offset: 0x0008B20C
	public static bool CanCombineWithContents(AssembledDefinitionNode _ingredientToAdd, IIngredientContents _container)
	{
		if (_container as MonoBehaviour != null)
		{
			if ((_container as MonoBehaviour).gameObject.RequestComponent<ServerMixableContainer>() != null)
			{
				ServerMixableContainer serverMixableContainer = (_container as MonoBehaviour).gameObject.RequestComponent<ServerMixableContainer>();
				if (_ingredientToAdd is IngredientAssembledNode)
				{
					return serverMixableContainer.CanTransferPremixedContents(new AssembledDefinitionNode[]
					{
						_ingredientToAdd
					}, 0f);
				}
				if (_ingredientToAdd is ItemAssembledNode)
				{
					return false;
				}
				AssembledDefinitionNode assembledDefinitionNode = _ingredientToAdd.Simpilfy();
				CompositeAssembledNode compositeAssembledNode = assembledDefinitionNode as CompositeAssembledNode;
				if (compositeAssembledNode != null)
				{
					AssembledDefinitionNode[] composition = compositeAssembledNode.m_composition;
					return serverMixableContainer.CanTransferPremixedContents(composition, 0f);
				}
				return serverMixableContainer.CanTransferPremixedContents(new AssembledDefinitionNode[]
				{
					assembledDefinitionNode
				}, 0f);
			}
			else
			{
				if ((_container as MonoBehaviour).gameObject.RequestComponent<ServerCookableContainer>() != null)
				{
					ServerCookableContainer serverCookableContainer = (_container as MonoBehaviour).gameObject.RequestComponent<ServerCookableContainer>();
					if (_ingredientToAdd is CookedCompositeAssembledNode)
					{
						float normalisedCookedProgress;
						AssembledDefinitionNode[] recookFromNode = AssembledNodeTransfer.GetRecookFromNode(_ingredientToAdd as CookedCompositeAssembledNode, out normalisedCookedProgress);
						return serverCookableContainer.CanTransferPrecookedContents(recookFromNode, normalisedCookedProgress);
					}
					if (_ingredientToAdd is MixedCompositeAssembledNode)
					{
						return serverCookableContainer.CanTransferPrecookedContents(new AssembledDefinitionNode[]
						{
							_ingredientToAdd
						}, 0f);
					}
					if (_ingredientToAdd is IngredientAssembledNode)
					{
						return serverCookableContainer.CanTransferPrecookedContents(new AssembledDefinitionNode[]
						{
							_ingredientToAdd
						}, 0f);
					}
					if (_ingredientToAdd is ItemAssembledNode)
					{
						return false;
					}
				}
				if ((_container as MonoBehaviour).gameObject.RequestComponent<ServerItemContainer>() != null)
				{
					ServerItemContainer serverItemContainer = (_container as MonoBehaviour).gameObject.RequestComponent<ServerItemContainer>();
					return _ingredientToAdd is ItemAssembledNode && serverItemContainer.CanTransferItemContents(new AssembledDefinitionNode[]
					{
						_ingredientToAdd
					});
				}
			}
		}
		for (int i = 0; i < _container.GetContentsCount(); i++)
		{
			if (_container.GetContentsElement(i).GetType() == typeof(CompositeAssembledNode))
			{
				CompositeAssembledNode compositeAssembledNode2 = _container.GetContentsElement(i) as CompositeAssembledNode;
				if (!compositeAssembledNode2.CanAddOrderNode(_ingredientToAdd, false))
				{
					return false;
				}
			}
		}
		if (_ingredientToAdd.GetType() == typeof(CompositeAssembledNode))
		{
			CompositeAssembledNode compositeAssembledNode3 = _ingredientToAdd as CompositeAssembledNode;
			for (int j = _container.GetContentsCount() - 1; j >= 0; j--)
			{
				if (!compositeAssembledNode3.CanAddOrderNode(_container.GetContentsElement(j), false))
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x0008D05C File Offset: 0x0008B45C
	public static void CombineWithContents(AssembledDefinitionNode _ingredientToAdd, IIngredientContents _container, bool _dontUpdate)
	{
		if (_container as MonoBehaviour != null && (_container as MonoBehaviour).gameObject.RequestComponent<ServerMixableContainer>() != null)
		{
			ServerMixableContainer serverMixableContainer = (_container as MonoBehaviour).gameObject.RequestComponent<ServerMixableContainer>();
			if (_ingredientToAdd is IngredientAssembledNode)
			{
				serverMixableContainer.TransferPremixedContents(new AssembledDefinitionNode[]
				{
					_ingredientToAdd
				}, 0f);
			}
			else
			{
				AssembledDefinitionNode assembledDefinitionNode = _ingredientToAdd.Simpilfy();
				CompositeAssembledNode compositeAssembledNode = assembledDefinitionNode as CompositeAssembledNode;
				MixedCompositeAssembledNode mixedCompositeAssembledNode = (!(assembledDefinitionNode is MixedCompositeAssembledNode)) ? (_ingredientToAdd as MixedCompositeAssembledNode) : (assembledDefinitionNode as MixedCompositeAssembledNode);
				float normalisedMixingProgress = 0f;
				if (mixedCompositeAssembledNode != null && mixedCompositeAssembledNode.m_recordedProgress != null)
				{
					normalisedMixingProgress = mixedCompositeAssembledNode.m_recordedProgress.Value;
				}
				if (compositeAssembledNode != null)
				{
					AssembledDefinitionNode[] composition = compositeAssembledNode.m_composition;
					serverMixableContainer.TransferPremixedContents(composition, normalisedMixingProgress);
				}
				else
				{
					serverMixableContainer.TransferPremixedContents(new AssembledDefinitionNode[]
					{
						assembledDefinitionNode
					}, normalisedMixingProgress);
				}
			}
			return;
		}
		if (_container as MonoBehaviour != null && (_container as MonoBehaviour).gameObject.RequestComponent<ServerCookableContainer>() != null)
		{
			ServerCookableContainer serverCookableContainer = (_container as MonoBehaviour).gameObject.RequestComponent<ServerCookableContainer>();
			if (_ingredientToAdd is CookedCompositeAssembledNode)
			{
				float normalisedCookedProgress;
				AssembledDefinitionNode[] recookFromNode = AssembledNodeTransfer.GetRecookFromNode(_ingredientToAdd as CookedCompositeAssembledNode, out normalisedCookedProgress);
				serverCookableContainer.TransferPrecookedContents(recookFromNode, normalisedCookedProgress);
				return;
			}
			if (_ingredientToAdd is MixedCompositeAssembledNode)
			{
				serverCookableContainer.TransferPrecookedContents(new AssembledDefinitionNode[]
				{
					_ingredientToAdd
				}, 0f);
				return;
			}
			if (_ingredientToAdd is IngredientAssembledNode)
			{
				serverCookableContainer.TransferPrecookedContents(new AssembledDefinitionNode[]
				{
					_ingredientToAdd
				}, 0f);
				return;
			}
		}
		for (int i = 0; i < _container.GetContentsCount(); i++)
		{
			if (_container.GetContentsElement(i) is CompositeAssembledNode)
			{
				CompositeAssembledNode compositeAssembledNode2 = _container.GetContentsElement(i) as CompositeAssembledNode;
				if (compositeAssembledNode2.CanAddOrderNode(_ingredientToAdd, false))
				{
					compositeAssembledNode2.AddOrderNode(_ingredientToAdd, _dontUpdate);
					return;
				}
			}
		}
		if (_ingredientToAdd is CompositeAssembledNode)
		{
			CompositeAssembledNode compositeAssembledNode3 = _ingredientToAdd as CompositeAssembledNode;
			for (int j = _container.GetContentsCount() - 1; j >= 0; j--)
			{
				if (compositeAssembledNode3.CanAddOrderNode(_container.GetContentsElement(j), false))
				{
					compositeAssembledNode3.AddOrderNode(_container.GetContentsElement(j), _dontUpdate);
					_container.RemoveIngredient(j);
				}
			}
		}
		_container.AddIngredient(_ingredientToAdd);
	}

	// Token: 0x06001CCE RID: 7374 RVA: 0x0008D2B4 File Offset: 0x0008B6B4
	private static AssembledDefinitionNode[] GetRecookFromNode(CookedCompositeAssembledNode _cookednode, out float _progress)
	{
		_progress = 0f;
		if (_cookednode.m_recordedProgress != null)
		{
			_progress = _cookednode.m_recordedProgress.Value;
		}
		else
		{
			CookedCompositeOrderNode.CookingProgress progress = _cookednode.m_progress;
			if (progress != CookedCompositeOrderNode.CookingProgress.Raw)
			{
				if (progress != CookedCompositeOrderNode.CookingProgress.Cooked)
				{
					if (progress == CookedCompositeOrderNode.CookingProgress.Burnt)
					{
						_progress = 2f;
					}
				}
				else
				{
					_progress = 1f;
				}
			}
			else
			{
				_progress = 0f;
			}
		}
		return _cookednode.m_composition;
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x0008D334 File Offset: 0x0008B734
	public static bool CanTransferFromContainer<T>(T _originObject, IIngredientContents _container) where T : MonoBehaviour, IOrderDefinition, IContainerTransferBehaviour
	{
		IngredientContainer x = _originObject.gameObject.RequestComponent<IngredientContainer>();
		if (!(x != null) || _originObject.GetOrderComposition().Simpilfy() == AssembledDefinitionNode.NullNode || !AssembledNodeTransfer.CanCombineWithContents(_originObject.GetOrderComposition(), _container))
		{
			return false;
		}
		if (_container as MonoBehaviour != null && (_container as MonoBehaviour).gameObject.RequestComponent<ServerLadleContainer>() != null && _originObject.gameObject.RequestComponent<ServerCookingHandler>() != null)
		{
			return true;
		}
		if (_container as MonoBehaviour != null && (_container as MonoBehaviour).gameObject.RequestComponent<ServerCookingHandler>() == null)
		{
			ServerCookingHandler serverCookingHandler = _originObject.gameObject.RequestComponent<ServerCookingHandler>();
			if (serverCookingHandler != null)
			{
				float cookingProgress = serverCookingHandler.GetCookingProgress();
				return cookingProgress == 0f || cookingProgress >= serverCookingHandler.AccessCookingTime;
			}
		}
		if (_container as MonoBehaviour != null && (_container as MonoBehaviour).gameObject.RequestComponent<ServerMixingHandler>() == null)
		{
			ServerMixingHandler serverMixingHandler = _originObject.gameObject.RequestComponent<ServerMixingHandler>();
			if (serverMixingHandler != null)
			{
				float mixingProgress = serverMixingHandler.GetMixingProgress();
				return mixingProgress == 0f || mixingProgress >= serverMixingHandler.AccessMixingTime;
			}
		}
		return true;
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x0008D4C0 File Offset: 0x0008B8C0
	public static void TransferFromContainer<T>(T _originObject, IIngredientContents _targetContainer, bool _dontRemove) where T : MonoBehaviour, IOrderDefinition, IContainerTransferBehaviour
	{
		ServerIngredientContainer serverIngredientContainer = _originObject.gameObject.RequireComponent<ServerIngredientContainer>();
		if (serverIngredientContainer != null && serverIngredientContainer.GetContentsCount() > 0)
		{
			AssembledDefinitionNode assembledDefinitionNode = _originObject.GetOrderComposition();
			if (_dontRemove)
			{
				assembledDefinitionNode = assembledDefinitionNode.Simpilfy();
			}
			AssembledNodeTransfer.CombineWithContents(assembledDefinitionNode, _targetContainer, _dontRemove);
			if (!_dontRemove)
			{
				ServerMessenger.TriggerAudioMessage(GameOneShotAudioTag.PutDown, _originObject.gameObject.layer);
				serverIngredientContainer.Empty();
			}
		}
	}
}
