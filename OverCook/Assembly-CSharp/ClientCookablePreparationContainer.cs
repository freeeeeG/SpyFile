using System;
using UnityEngine;

// Token: 0x02000478 RID: 1144
public class ClientCookablePreparationContainer : ClientPreparationContainer
{
	// Token: 0x06001537 RID: 5431 RVA: 0x00073C13 File Offset: 0x00072013
	public override void StartSynchronising(Component synchronisedObject)
	{
		this.m_cookingHandler = base.gameObject.RequireComponent<ClientCookingHandler>();
		this.m_cookingHandler.CookingStateChangedCallback += delegate(CookingUIController.State _state)
		{
			this.m_orderCompositionChangedCallbacks(base.GetOrderComposition());
		};
		base.StartSynchronising(synchronisedObject);
	}

	// Token: 0x06001538 RID: 5432 RVA: 0x00073C44 File Offset: 0x00072044
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

	// Token: 0x0400104C RID: 4172
	private ClientCookingHandler m_cookingHandler;
}
