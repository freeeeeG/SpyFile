using System;
using UnityEngine;

// Token: 0x02000477 RID: 1143
public class ServerCookablePreparationContainer : ServerPreparationContainer
{
	// Token: 0x06001530 RID: 5424 RVA: 0x0007380B File Offset: 0x00071C0B
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cookablePreparationContainer = (CookablePreparationContainer)synchronisedObject;
		this.m_cookingHandler = base.gameObject.RequireComponent<ServerCookingHandler>();
		this.m_cookingHandler.CookingStateChangedCallback += delegate(CookingUIController.State _state)
		{
			this.m_orderCompositionChangedCallbacks(base.GetOrderComposition());
		};
	}

	// Token: 0x06001531 RID: 5425 RVA: 0x00073848 File Offset: 0x00071C48
	protected override bool CanAddIngredient(AssembledDefinitionNode _toAdd)
	{
		CookedCompositeAssembledNode cookedCompositeAssembledNode = this.GetAsOrderComposite() as CookedCompositeAssembledNode;
		return (cookedCompositeAssembledNode == null || cookedCompositeAssembledNode.m_progress == CookedCompositeOrderNode.CookingProgress.Raw) && base.CanAddIngredient(_toAdd);
	}

	// Token: 0x06001532 RID: 5426 RVA: 0x0007387B File Offset: 0x00071C7B
	public override void AddOrderContents(AssembledDefinitionNode[] _contents)
	{
		base.AddOrderContents(_contents);
		if (_contents.Length > 0)
		{
			this.m_cookingHandler.SetCookingProgress(0f);
		}
	}

	// Token: 0x06001533 RID: 5427 RVA: 0x0007389D File Offset: 0x00071C9D
	public override bool CanTransferToContainer(IIngredientContents _container)
	{
		return this.m_cookingHandler.GetCookedOrderState() != CookedCompositeOrderNode.CookingProgress.Burnt && AssembledNodeTransfer.CanTransferFromContainer<ServerCookablePreparationContainer>(this, _container) && base.CanTransferToContainer(_container);
	}

	// Token: 0x06001534 RID: 5428 RVA: 0x000738C8 File Offset: 0x00071CC8
	protected override CompositeAssembledNode GetAsOrderComposite()
	{
		CompositeAssembledNode asOrderComposite = base.GetAsOrderComposite();
		return new CookedCompositeAssembledNode
		{
			m_freeObject = asOrderComposite.m_freeObject,
			m_composition = asOrderComposite.m_composition,
			m_optional = asOrderComposite.m_optional,
			m_permittedEntries = asOrderComposite.m_permittedEntries,
			m_cookingStep = this.m_cookingHandler.AccessCookingType,
			m_recordedProgress = new float?(this.m_cookingHandler.GetCookingProgress() / this.m_cookingHandler.AccessCookingTime),
			m_progress = this.m_cookingHandler.GetCookedOrderState()
		};
	}

	// Token: 0x0400104A RID: 4170
	private CookablePreparationContainer m_cookablePreparationContainer;

	// Token: 0x0400104B RID: 4171
	private ServerCookingHandler m_cookingHandler;
}
