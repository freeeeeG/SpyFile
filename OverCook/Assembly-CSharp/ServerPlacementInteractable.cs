using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000537 RID: 1335
public class ServerPlacementInteractable : ServerSynchroniserBase, IHandlePlacement, IBaseHandlePlacement
{
	// Token: 0x060018FB RID: 6395 RVA: 0x0007F364 File Offset: 0x0007D764
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x0007F370 File Offset: 0x0007D770
	public bool CanHandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		if (this.m_interactionSuppressed)
		{
			return false;
		}
		for (int i = 0; i < this.m_canInteractCallbacks.Count; i++)
		{
			if (!this.m_canInteractCallbacks[i](_carrier.AccessGameObject()))
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x060018FD RID: 6397 RVA: 0x0007F3C5 File Offset: 0x0007D7C5
	public int GetPlacementPriority()
	{
		return 1;
	}

	// Token: 0x060018FE RID: 6398 RVA: 0x0007F3C8 File Offset: 0x0007D7C8
	public void HandlePlacement(ICarrier _carrier, Vector2 _directionXZ, PlacementContext _context)
	{
		if (this.m_triggerCallbacks != null)
		{
			this.m_triggerCallbacks(_carrier.AccessGameObject(), _directionXZ);
		}
	}

	// Token: 0x060018FF RID: 6399 RVA: 0x0007F3E7 File Offset: 0x0007D7E7
	public void OnFailedToPlace(GameObject _item)
	{
	}

	// Token: 0x06001900 RID: 6400 RVA: 0x0007F3E9 File Offset: 0x0007D7E9
	public void RegisterTriggerCallback(VoidGeneric<GameObject, Vector2> _triggerCallback)
	{
		this.m_triggerCallbacks = (VoidGeneric<GameObject, Vector2>)Delegate.Combine(this.m_triggerCallbacks, _triggerCallback);
	}

	// Token: 0x06001901 RID: 6401 RVA: 0x0007F402 File Offset: 0x0007D802
	public void UnregisterTriggerCallback(VoidGeneric<GameObject, Vector2> _triggerCallback)
	{
		this.m_triggerCallbacks = (VoidGeneric<GameObject, Vector2>)Delegate.Remove(this.m_triggerCallbacks, _triggerCallback);
	}

	// Token: 0x06001902 RID: 6402 RVA: 0x0007F41B File Offset: 0x0007D81B
	public void RegisterCanInteractCallback(Generic<bool, GameObject> _canInteractCallback)
	{
		this.m_canInteractCallbacks.Add(_canInteractCallback);
	}

	// Token: 0x06001903 RID: 6403 RVA: 0x0007F429 File Offset: 0x0007D829
	public void UnregisterCanInteractCallback(Generic<bool, GameObject> _canInteractCallback)
	{
		this.m_canInteractCallbacks.Remove(_canInteractCallback);
	}

	// Token: 0x06001904 RID: 6404 RVA: 0x0007F438 File Offset: 0x0007D838
	public void SetInteractionSurpressed(bool _surpressed)
	{
		this.m_interactionSuppressed = _surpressed;
	}

	// Token: 0x04001411 RID: 5137
	private VoidGeneric<GameObject, Vector2> m_triggerCallbacks;

	// Token: 0x04001412 RID: 5138
	private List<Generic<bool, GameObject>> m_canInteractCallbacks = new List<Generic<bool, GameObject>>();

	// Token: 0x04001413 RID: 5139
	private bool m_interactionSuppressed;
}
