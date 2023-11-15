using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000457 RID: 1111
public class ClientCleanPlateStack : ClientPlateStackBase, IClientHandlePlacement, IClientHandlePickup, IBaseHandlePlacement, IBaseHandlePickup
{
	// Token: 0x06001484 RID: 5252 RVA: 0x00070277 File Offset: 0x0006E677
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cleanPlateStack = (CleanPlateStack)this.m_plateStack;
	}

	// Token: 0x06001485 RID: 5253 RVA: 0x00070294 File Offset: 0x0006E694
	public override void UpdateSynchronising()
	{
		for (int i = this.m_delayedSetupPlates.Count - 1; i >= 0; i--)
		{
			this.DelayedPlateSetup(this.m_delayedSetupPlates[i]);
			this.m_delayedSetupPlates.RemoveAt(i);
		}
	}

	// Token: 0x06001486 RID: 5254 RVA: 0x000702E0 File Offset: 0x0006E6E0
	protected override void PlateSpawned(GameObject _object)
	{
		bool flag = false;
		if (this.m_stack.InspectTopOfStack() != null)
		{
			IIngredientContents ingredientContents = this.m_stack.InspectTopOfStack().RequestInterface<IIngredientContents>();
			flag = (ingredientContents != null && ingredientContents.HasContents());
		}
		if (flag)
		{
			GameObject item = this.m_stack.RemoveFromStack();
			this.m_stack.AddToStack(_object);
			this.m_stack.AddToStack(item);
		}
		else
		{
			this.m_stack.AddToStack(_object);
		}
		this.m_delayedSetupPlates.Add(_object);
		base.NotifyPlateAdded(_object);
	}

	// Token: 0x06001487 RID: 5255 RVA: 0x00070374 File Offset: 0x0006E774
	private void DelayedPlateSetup(GameObject _plate)
	{
		if (_plate != null && _plate.gameObject != null)
		{
			ClientHandlePickupReferral clientHandlePickupReferral = _plate.RequestComponent<ClientHandlePickupReferral>();
			if (clientHandlePickupReferral != null)
			{
				clientHandlePickupReferral.SetHandlePickupReferree(this);
			}
			ClientHandlePlacementReferral clientHandlePlacementReferral = _plate.RequestComponent<ClientHandlePlacementReferral>();
			if (clientHandlePlacementReferral != null)
			{
				clientHandlePlacementReferral.SetHandlePlacementReferree(this);
			}
		}
	}

	// Token: 0x06001488 RID: 5256 RVA: 0x000703D4 File Offset: 0x0006E7D4
	protected override void PlateRemoved()
	{
		GameObject gameObject = this.m_stack.RemoveFromStack();
		ClientHandlePickupReferral clientHandlePickupReferral = gameObject.RequestComponent<ClientHandlePickupReferral>();
		if (clientHandlePickupReferral != null && clientHandlePickupReferral.GetHandlePickupReferree() == this)
		{
			clientHandlePickupReferral.SetHandlePickupReferree(null);
		}
		ClientHandlePlacementReferral clientHandlePlacementReferral = gameObject.RequestComponent<ClientHandlePlacementReferral>();
		if (clientHandlePlacementReferral != null && clientHandlePlacementReferral.GetHandlePlacementReferree() == this)
		{
			clientHandlePlacementReferral.SetHandlePlacementReferree(null);
		}
		base.NotifyPlateRemoved(gameObject);
	}

	// Token: 0x06001489 RID: 5257 RVA: 0x00070440 File Offset: 0x0006E840
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return this.m_stack.GetSize() > 0;
	}

	// Token: 0x0600148A RID: 5258 RVA: 0x00070450 File Offset: 0x0006E850
	public int GetPickupPriority()
	{
		return 0;
	}

	// Token: 0x0600148B RID: 5259 RVA: 0x00070454 File Offset: 0x0006E854
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = this.m_stack.InspectTopOfStack();
		if (gameObject)
		{
			IClientHandlePlacement clientHandlePlacement = gameObject.RequireInterface<IClientHandlePlacement>();
			return clientHandlePlacement.CanHandlePlacement(_carrier, _directionXZ, _context);
		}
		return false;
	}

	// Token: 0x0600148C RID: 5260 RVA: 0x0007048A File Offset: 0x0006E88A
	public int GetPlacementPriority()
	{
		return 0;
	}

	// Token: 0x04000FD1 RID: 4049
	private CleanPlateStack m_cleanPlateStack;

	// Token: 0x04000FD2 RID: 4050
	private List<GameObject> m_delayedSetupPlates = new List<GameObject>();
}
