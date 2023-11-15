using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000615 RID: 1557
public class ClientPlacementContainer : ClientSynchroniserBase, IClientHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x06001D74 RID: 7540 RVA: 0x000902A2 File Offset: 0x0008E6A2
	public void Awake()
	{
		this.m_placementContainer = base.gameObject.RequireComponent<PlacementContainer>();
		this.m_ingredientContainer = base.gameObject.RequireComponent<ClientIngredientContainer>();
	}

	// Token: 0x06001D75 RID: 7541 RVA: 0x000902C8 File Offset: 0x0008E6C8
	public virtual bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = _carrier.InspectCarriedItem();
		if (this.CanCombine(gameObject, _context))
		{
			return true;
		}
		ClientPlacementContainer clientPlacementContainer = gameObject.RequestComponent<ClientPlacementContainer>();
		return clientPlacementContainer != null && clientPlacementContainer.CanCombine(base.gameObject, _context);
	}

	// Token: 0x06001D76 RID: 7542 RVA: 0x00090313 File Offset: 0x0008E713
	public virtual int GetPlacementPriority()
	{
		return 0;
	}

	// Token: 0x06001D77 RID: 7543 RVA: 0x00090316 File Offset: 0x0008E716
	protected virtual bool CanCombine(GameObject _placingObject, PlacementContext _context)
	{
		return this.m_placementContainer.CanCombine(_placingObject, this.m_allowPlacementCallback, this.m_ingredientContainer, _context);
	}

	// Token: 0x06001D78 RID: 7544 RVA: 0x00090331 File Offset: 0x0008E731
	public void RegisterAllowItemPlacement(Generic<bool, GameObject, PlacementContext> _allowPlacementCallback)
	{
		this.m_allowPlacementCallback.Add(_allowPlacementCallback);
	}

	// Token: 0x06001D79 RID: 7545 RVA: 0x0009033F File Offset: 0x0008E73F
	public void UnregisterAllowItemPlacement(Generic<bool, GameObject, PlacementContext> _allowPlacementCallback)
	{
		this.m_allowPlacementCallback.Remove(_allowPlacementCallback);
	}

	// Token: 0x040016CC RID: 5836
	private ClientIngredientContainer m_ingredientContainer;

	// Token: 0x040016CD RID: 5837
	protected List<Generic<bool, GameObject, PlacementContext>> m_allowPlacementCallback = new List<Generic<bool, GameObject, PlacementContext>>();

	// Token: 0x040016CE RID: 5838
	private PlacementContainer m_placementContainer;
}
