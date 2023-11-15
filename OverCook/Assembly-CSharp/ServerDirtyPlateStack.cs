using System;
using UnityEngine;

// Token: 0x02000465 RID: 1125
public class ServerDirtyPlateStack : ServerPlateStackBase, IHandlePlacement, ICarryNotified, ISurfacePlacementNotified, IBaseHandlePlacement
{
	// Token: 0x060014E7 RID: 5351 RVA: 0x00072250 File Offset: 0x00070650
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		DirtyPlateStack dirtyPlateStack = (DirtyPlateStack)synchronisedObject;
		if (dirtyPlateStack.m_washedPrefab != null)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, dirtyPlateStack.m_washedPrefab);
			this.m_washable = base.gameObject.RequireComponent<ServerWashable>();
			this.m_washable.RegisterAllowWashCallback(new Generic<bool>(this.AllowWash));
			this.m_washable.RegisterFinishedCallback(new CallbackVoid(this.OnWashingFinished));
		}
		if (dirtyPlateStack.m_cleanPlatePrefab != null)
		{
			NetworkUtils.RegisterSpawnablePrefab(base.gameObject, dirtyPlateStack.m_cleanPlatePrefab);
		}
	}

	// Token: 0x060014E8 RID: 5352 RVA: 0x000722F0 File Offset: 0x000706F0
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject obj = _carrier.InspectCarriedItem();
		DirtyPlateStack dirtyPlateStack = obj.RequestComponent<DirtyPlateStack>();
		return dirtyPlateStack != null && dirtyPlateStack.GetPlatingStep() == this.m_plateStack.GetPlatingStep();
	}

	// Token: 0x060014E9 RID: 5353 RVA: 0x00072330 File Offset: 0x00070730
	public void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		GameObject obj = _carrier.InspectCarriedItem();
		ServerStack serverStack = obj.RequireComponent<ServerStack>();
		for (int i = 0; i < serverStack.GetSize(); i++)
		{
			this.AddToStack();
		}
		_carrier.DestroyCarriedItem();
	}

	// Token: 0x060014EA RID: 5354 RVA: 0x0007236E File Offset: 0x0007076E
	public void OnFailedToPlace(GameObject _item)
	{
	}

	// Token: 0x060014EB RID: 5355 RVA: 0x00072370 File Offset: 0x00070770
	public int GetPlacementPriority()
	{
		return 0;
	}

	// Token: 0x060014EC RID: 5356 RVA: 0x00072373 File Offset: 0x00070773
	public void OnCarryBegun(ICarrier _carrier)
	{
		if (_carrier as ServerPlayerAttachmentCarrier != null)
		{
			this.m_carrier = _carrier;
		}
	}

	// Token: 0x060014ED RID: 5357 RVA: 0x0007238D File Offset: 0x0007078D
	public void OnCarryEnded(ICarrier _carrier)
	{
		this.m_carrier = null;
	}

	// Token: 0x060014EE RID: 5358 RVA: 0x00072396 File Offset: 0x00070796
	public void OnSurfacePlacement(ServerAttachStation _station)
	{
		this.m_attachStation = _station;
	}

	// Token: 0x060014EF RID: 5359 RVA: 0x0007239F File Offset: 0x0007079F
	public void OnSurfaceDeplacement(ServerAttachStation _station)
	{
		this.m_attachStation = null;
	}

	// Token: 0x060014F0 RID: 5360 RVA: 0x000723A8 File Offset: 0x000707A8
	public override void AddToStack()
	{
		base.AddToStack();
		if (this.m_washable != null)
		{
			Washable washable = base.gameObject.RequireComponent<Washable>();
			this.m_washable.SetDuration((float)(washable.m_duration * this.m_stack.GetSize()));
		}
	}

	// Token: 0x060014F1 RID: 5361 RVA: 0x000723F6 File Offset: 0x000707F6
	private bool AllowWash()
	{
		return this.m_stack.GetSize() > 0;
	}

	// Token: 0x060014F2 RID: 5362 RVA: 0x00072408 File Offset: 0x00070808
	private void OnWashingFinished()
	{
		ServerUtensilRespawnBehaviour serverUtensilRespawnBehaviour = base.gameObject.RequestComponent<ServerUtensilRespawnBehaviour>();
		DirtyPlateStack dirtyPlateStack = this.m_plateStack as DirtyPlateStack;
		IAttachment attachment;
		if (this.m_carrier != null && this.m_stack.GetSize() == 1)
		{
			GameObject obj = NetworkUtils.ServerSpawnPrefab(base.gameObject, dirtyPlateStack.m_cleanPlatePrefab);
			ServerUtensilRespawnBehaviour serverUtensilRespawnBehaviour2 = obj.RequestComponent<ServerUtensilRespawnBehaviour>();
			if (serverUtensilRespawnBehaviour != null && serverUtensilRespawnBehaviour2 != null)
			{
				serverUtensilRespawnBehaviour2.SetIdealRespawnLocation(serverUtensilRespawnBehaviour.GetIdealRespawnLocation());
			}
			attachment = obj.RequireInterface<IAttachment>();
		}
		else
		{
			GameObject obj2 = NetworkUtils.ServerSpawnPrefab(base.gameObject, dirtyPlateStack.m_washedPrefab);
			ServerUtensilRespawnBehaviour serverUtensilRespawnBehaviour3 = obj2.RequestComponent<ServerUtensilRespawnBehaviour>();
			if (serverUtensilRespawnBehaviour != null && serverUtensilRespawnBehaviour3 != null)
			{
				serverUtensilRespawnBehaviour3.SetIdealRespawnLocation(serverUtensilRespawnBehaviour.GetIdealRespawnLocation());
			}
			attachment = obj2.RequireInterface<IAttachment>();
			ServerPlateStackBase serverPlateStackBase = attachment.AccessGameObject().RequireComponent<ServerPlateStackBase>();
			for (int i = 0; i < this.m_stack.GetSize(); i++)
			{
				serverPlateStackBase.AddToStack();
			}
		}
		if (this.m_carrier != null || this.m_attachStation != null)
		{
			Vector3 localPosition = base.transform.localPosition;
			Quaternion localRotation = base.transform.localRotation;
			if (this.m_carrier != null)
			{
				ICarrier carrier = this.m_carrier;
				carrier.TakeItem();
				carrier.CarryItem(attachment.AccessGameObject());
			}
			else if (this.m_attachStation != null)
			{
				ServerAttachStation attachStation = this.m_attachStation;
				attachStation.TakeItem();
				attachStation.AddItem(attachment.AccessGameObject(), Vector2.up, default(PlacementContext));
			}
			attachment.AccessGameObject().transform.localPosition = localPosition;
			attachment.AccessGameObject().transform.localRotation = localRotation;
		}
		else
		{
			attachment.AccessRigidbody().transform.SetPositionAndRotation(base.transform.position, base.transform.rotation);
		}
		NetworkUtils.DestroyObjectsRecursive(base.gameObject);
	}

	// Token: 0x04001021 RID: 4129
	private ServerWashable m_washable;

	// Token: 0x04001022 RID: 4130
	private ICarrier m_carrier;

	// Token: 0x04001023 RID: 4131
	private ServerAttachStation m_attachStation;
}
