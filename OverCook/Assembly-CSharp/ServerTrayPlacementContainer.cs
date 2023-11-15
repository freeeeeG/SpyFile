using System;
using UnityEngine;

// Token: 0x0200061A RID: 1562
public class ServerTrayPlacementContainer : ServerPlacementContainer
{
	// Token: 0x06001D98 RID: 7576 RVA: 0x0009035E File Offset: 0x0008E75E
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_trayPlacementContainer = (TrayPlacementContainer)synchronisedObject;
		this.m_tray = base.gameObject.RequireComponent<ServerTray>();
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x00090384 File Offset: 0x0008E784
	protected override bool CanCombine(GameObject _placingObject, PlacementContext _context)
	{
		if (!this.m_ingredientContainer.HasContents() && _placingObject.RequestComponent<ServerTray>() != null)
		{
			return true;
		}
		IIngredientContents ingredientContents = this.m_tray.GetIngredientContents(_placingObject, _context, true);
		return ingredientContents != null && this.m_trayPlacementContainer.CanCombine(_placingObject, this.m_allowPlacementCallbacks, ingredientContents, _context);
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x000903DF File Offset: 0x0008E7DF
	public override int GetPlacementPriority()
	{
		return 1;
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x000903E4 File Offset: 0x0008E7E4
	public override void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = _carrier.InspectCarriedItem();
		if (!this.m_ingredientContainer.HasContents() && gameObject.RequestComponent<ServerTray>() != null)
		{
			ServerIngredientContainer serverIngredientContainer = gameObject.RequireComponent<ServerIngredientContainer>();
			for (int i = 0; i < serverIngredientContainer.GetContentsCount(); i++)
			{
				this.m_ingredientContainer.AddIngredient(serverIngredientContainer.GetContentsElement(i));
			}
			serverIngredientContainer.Empty();
			return;
		}
		if (this.CanCombine(gameObject, _context))
		{
			IContainerTransferBehaviour containerTransferBehaviour = gameObject.RequireInterface<IContainerTransferBehaviour>();
			IIngredientContents ingredientContents = this.m_tray.GetIngredientContents(gameObject, _context, true);
			if (ingredientContents != null)
			{
				containerTransferBehaviour.TransferToContainer(_carrier, ingredientContents, false);
			}
			return;
		}
		ServerIngredientContainer serverIngredientContainer2 = gameObject.RequireComponent<ServerIngredientContainer>();
		IContainerTransferBehaviour containerTransferBehaviour2 = base.gameObject.RequireInterface<IContainerTransferBehaviour>();
		if (containerTransferBehaviour2.CanTransferToContainer(serverIngredientContainer2))
		{
			containerTransferBehaviour2.TransferToContainer(null, serverIngredientContainer2, false);
			serverIngredientContainer2.InformOfInternalChange();
		}
	}

	// Token: 0x040016E5 RID: 5861
	private ServerTray m_tray;

	// Token: 0x040016E6 RID: 5862
	private TrayPlacementContainer m_trayPlacementContainer;
}
