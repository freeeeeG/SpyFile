using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000540 RID: 1344
public class ClientPlacementItemSwitcher : ClientSynchroniserBase
{
	// Token: 0x06001932 RID: 6450 RVA: 0x0007F7E0 File Offset: 0x0007DBE0
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_placementItemSwitcher = (PlacementItemSwitcher)synchronisedObject;
		this.m_ingredientPropertiesComponent = this.m_placementItemSwitcher.GetComponent<IngredientPropertiesComponent>();
	}

	// Token: 0x06001933 RID: 6451 RVA: 0x0007F808 File Offset: 0x0007DC08
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		PickupItemSwitcherMessage pickupItemSwitcherMessage = (PickupItemSwitcherMessage)serialisable;
		this.m_ingredientPropertiesComponent.SetIngredientOrderNode(this.m_placementItemSwitcher.m_ingredients[pickupItemSwitcherMessage.m_itemIndex]);
		base.gameObject.SendMessage("OnItemSwitched", SendMessageOptions.DontRequireReceiver);
	}

	// Token: 0x06001934 RID: 6452 RVA: 0x0007F84A File Offset: 0x0007DC4A
	public override EntityType GetEntityType()
	{
		return EntityType.PickupItemSwitcher;
	}

	// Token: 0x04001426 RID: 5158
	private PlacementItemSwitcher m_placementItemSwitcher;

	// Token: 0x04001427 RID: 5159
	private IngredientPropertiesComponent m_ingredientPropertiesComponent;
}
