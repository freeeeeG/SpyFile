using System;
using UnityEngine;

// Token: 0x0200061B RID: 1563
public class ClientTrayPlacementContainer : ClientPlacementContainer
{
	// Token: 0x06001D9D RID: 7581 RVA: 0x000904C3 File Offset: 0x0008E8C3
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_placementContainer = (TrayPlacementContainer)synchronisedObject;
		this.m_tray = base.gameObject.RequireComponent<ClientTray>();
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x000904EC File Offset: 0x0008E8EC
	protected override bool CanCombine(GameObject _placingObject, PlacementContext _context)
	{
		IIngredientContents ingredientContents = this.m_tray.GetIngredientContents(_placingObject, _context, false);
		return ingredientContents != null && this.m_placementContainer.CanCombine(_placingObject, this.m_allowPlacementCallback, ingredientContents, _context);
	}

	// Token: 0x06001D9F RID: 7583 RVA: 0x00090524 File Offset: 0x0008E924
	public override int GetPlacementPriority()
	{
		return 1;
	}

	// Token: 0x040016E7 RID: 5863
	private ClientTray m_tray;

	// Token: 0x040016E8 RID: 5864
	private TrayPlacementContainer m_placementContainer;
}
