using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000549 RID: 1353
public class ServerPlateReturnStation : ServerSynchroniserBase
{
	// Token: 0x06001974 RID: 6516 RVA: 0x00080154 File Offset: 0x0007E554
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_attachStation = base.gameObject.GetComponent<ServerAttachStation>();
		this.m_attachStation.RegisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
		this.m_attachStation.RegisterAllowItemPickup(new Generic<bool>(this.CanRemoveItem));
		this.m_attachStation.RegisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
		this.m_returnStation = (PlateReturnStation)synchronisedObject;
		NetworkUtils.RegisterSpawnablePrefab(base.gameObject, this.m_returnStation.m_stackPrefab);
		this.m_pendingInitialise = true;
	}

	// Token: 0x06001975 RID: 6517 RVA: 0x000801E8 File Offset: 0x0007E5E8
	public override void OnDestroy()
	{
		base.OnDestroy();
		if (this.m_attachStation != null)
		{
			this.m_attachStation.UnregisterAllowItemPlacement(new Generic<bool, GameObject, PlacementContext>(this.CanAddItem));
			this.m_attachStation.UnregisterAllowItemPickup(new Generic<bool>(this.CanRemoveItem));
			this.m_attachStation.UnregisterOnItemRemoved(new ServerAttachStation.OnItemRemoved(this.OnItemRemoved));
		}
	}

	// Token: 0x06001976 RID: 6518 RVA: 0x00080254 File Offset: 0x0007E654
	public override void UpdateSynchronising()
	{
		if (this.m_pendingInitialise)
		{
			this.m_pendingInitialise = false;
			if (this.m_returnStation.m_startingPlateNumber > 0)
			{
				this.CreateStack();
				for (int i = 0; i < this.m_returnStation.m_startingPlateNumber; i++)
				{
					this.m_stack.AddToStack();
				}
			}
		}
	}

	// Token: 0x06001977 RID: 6519 RVA: 0x000802B1 File Offset: 0x0007E6B1
	private bool CanAddItem(GameObject _object, PlacementContext _context)
	{
		return _context.m_source != PlacementContext.Source.Player && !(this.m_stack != null) && !this.m_attachStation.HasItem();
	}

	// Token: 0x06001978 RID: 6520 RVA: 0x000802E8 File Offset: 0x0007E6E8
	private bool CanRemoveItem()
	{
		return this.m_stack == null || this.m_stack.GetSize() > 0;
	}

	// Token: 0x06001979 RID: 6521 RVA: 0x0008030C File Offset: 0x0007E70C
	private void OnItemRemoved(IAttachment _iHoldable)
	{
		if (this.m_stack != null && _iHoldable.AccessGameObject() == this.m_stack.gameObject)
		{
			this.m_stack = null;
		}
	}

	// Token: 0x0600197A RID: 6522 RVA: 0x00080344 File Offset: 0x0007E744
	public bool CanReturnPlate()
	{
		GameObject x = this.m_attachStation.InspectItem();
		return x == null || (this.m_stack != null && x == this.m_stack.gameObject);
	}

	// Token: 0x0600197B RID: 6523 RVA: 0x00080391 File Offset: 0x0007E791
	public void ReturnPlate()
	{
		if (this.m_stack == null)
		{
			this.CreateStack();
		}
		this.m_stack.AddToStack();
	}

	// Token: 0x0600197C RID: 6524 RVA: 0x000803B8 File Offset: 0x0007E7B8
	private void CreateStack()
	{
		GameObject gameObject = NetworkUtils.ServerSpawnPrefab(base.gameObject, this.m_returnStation.m_stackPrefab);
		this.m_attachStation.AddItem(gameObject, base.transform.rotation * Vector2.up, default(PlacementContext));
		this.m_stack = gameObject.RequireInterface<ServerPlateStackBase>();
	}

	// Token: 0x0600197D RID: 6525 RVA: 0x0008041C File Offset: 0x0007E81C
	public PlatingStepData GetPlatingStep()
	{
		if (this.m_returnStation.m_stackPrefab != null)
		{
			PlateStackBase plateStackBase = this.m_returnStation.m_stackPrefab.RequestComponent<PlateStackBase>();
			if (plateStackBase != null)
			{
				return plateStackBase.GetPlatingStep();
			}
		}
		return null;
	}

	// Token: 0x0400143F RID: 5183
	private PlateReturnStation m_returnStation;

	// Token: 0x04001440 RID: 5184
	private bool m_isDirtyReturn;

	// Token: 0x04001441 RID: 5185
	private bool m_pendingInitialise;

	// Token: 0x04001442 RID: 5186
	private ServerAttachStation m_attachStation;

	// Token: 0x04001443 RID: 5187
	private ServerPlateStackBase m_stack;
}
