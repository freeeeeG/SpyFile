using System;
using UnityEngine;

// Token: 0x02000456 RID: 1110
public class ServerCleanPlateStack : ServerPlateStackBase, IHandlePickup, IHandlePlacement, IBaseHandlePickup, IBaseHandlePlacement
{
	// Token: 0x06001478 RID: 5240 RVA: 0x0006FEBE File Offset: 0x0006E2BE
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cleanPlateStack = (CleanPlateStack)this.m_plateStack;
	}

	// Token: 0x06001479 RID: 5241 RVA: 0x0006FED8 File Offset: 0x0006E2D8
	public override void AddToStack()
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
			this.AddToBaseStack();
			this.m_stack.AddToStack(item);
		}
		else
		{
			this.AddToBaseStack();
		}
		GameObject obj = this.m_stack.InspectTopOfStack();
		ServerHandlePickupReferral serverHandlePickupReferral = obj.RequestComponent<ServerHandlePickupReferral>();
		if (serverHandlePickupReferral != null)
		{
			serverHandlePickupReferral.SetHandlePickupReferree(this);
		}
		ServerHandlePlacementReferral serverHandlePlacementReferral = obj.RequestComponent<ServerHandlePlacementReferral>();
		if (serverHandlePlacementReferral != null)
		{
			serverHandlePlacementReferral.SetHandlePlacementReferree(this);
		}
	}

	// Token: 0x0600147A RID: 5242 RVA: 0x0006FF94 File Offset: 0x0006E394
	private void AddToBaseStack()
	{
		base.AddToStack();
		GameObject obj = this.m_stack.InspectTopOfStack();
		ServerUtensilRespawnBehaviour serverUtensilRespawnBehaviour = base.gameObject.RequestComponent<ServerUtensilRespawnBehaviour>();
		ServerUtensilRespawnBehaviour serverUtensilRespawnBehaviour2 = obj.RequestComponent<ServerUtensilRespawnBehaviour>();
		if (serverUtensilRespawnBehaviour != null && serverUtensilRespawnBehaviour2 != null)
		{
			serverUtensilRespawnBehaviour2.SetIdealRespawnLocation(serverUtensilRespawnBehaviour.GetIdealRespawnLocation());
		}
	}

	// Token: 0x0600147B RID: 5243 RVA: 0x0006FFEC File Offset: 0x0006E3EC
	protected override GameObject RemoveFromStack()
	{
		GameObject gameObject = base.RemoveFromStack();
		ServerHandlePickupReferral serverHandlePickupReferral = gameObject.RequestComponent<ServerHandlePickupReferral>();
		if (serverHandlePickupReferral != null && serverHandlePickupReferral.GetHandlePickupReferree() == this)
		{
			serverHandlePickupReferral.SetHandlePickupReferree(null);
		}
		ServerHandlePlacementReferral serverHandlePlacementReferral = gameObject.RequestComponent<ServerHandlePlacementReferral>();
		if (serverHandlePlacementReferral != null && serverHandlePlacementReferral.GetHandlePlacementReferree() == this)
		{
			serverHandlePlacementReferral.SetHandlePlacementReferree(null);
		}
		return gameObject;
	}

	// Token: 0x0600147C RID: 5244 RVA: 0x0007004D File Offset: 0x0006E44D
	public bool CanHandlePickup(ICarrier _carrier)
	{
		return this.m_stack.GetSize() > 0;
	}

	// Token: 0x0600147D RID: 5245 RVA: 0x00070060 File Offset: 0x0006E460
	public void HandlePickup(ICarrier _carrier, Vector2 _directionXZ)
	{
		GameObject @object = this.RemoveFromStack();
		_carrier.CarryItem(@object);
	}

	// Token: 0x0600147E RID: 5246 RVA: 0x0007007B File Offset: 0x0006E47B
	public int GetPickupPriority()
	{
		return 0;
	}

	// Token: 0x0600147F RID: 5247 RVA: 0x00070080 File Offset: 0x0006E480
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject gameObject = this.m_stack.InspectTopOfStack();
		if (gameObject)
		{
			IHandlePlacement handlePlacement = gameObject.RequireInterface<IHandlePlacement>();
			return handlePlacement.CanHandlePlacement(_carrier, _directionXZ, _context);
		}
		return false;
	}

	// Token: 0x06001480 RID: 5248 RVA: 0x000700B8 File Offset: 0x0006E4B8
	public void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject obj = this.m_stack.InspectTopOfStack();
		IHandlePlacement handlePlacement = obj.RequireInterface<IHandlePlacement>();
		handlePlacement.HandlePlacement(_carrier, _directionXZ, _context);
	}

	// Token: 0x06001481 RID: 5249 RVA: 0x000700E1 File Offset: 0x0006E4E1
	public void OnFailedToPlace(GameObject _item)
	{
	}

	// Token: 0x06001482 RID: 5250 RVA: 0x000700E3 File Offset: 0x0006E4E3
	public int GetPlacementPriority()
	{
		return 0;
	}

	// Token: 0x04000FD0 RID: 4048
	private CleanPlateStack m_cleanPlateStack;
}
