using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004D1 RID: 1233
public class ServerInteractable : ServerSynchroniserBase
{
	// Token: 0x060016E1 RID: 5857 RVA: 0x0007731A File Offset: 0x0007571A
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_interactable = (Interactable)synchronisedObject;
	}

	// Token: 0x1700024A RID: 586
	// (get) Token: 0x060016E2 RID: 5858 RVA: 0x0007732F File Offset: 0x0007572F
	public bool UsePlacementButton
	{
		get
		{
			return this.m_interactable.m_usePlacementButton;
		}
	}

	// Token: 0x060016E3 RID: 5859 RVA: 0x0007733C File Offset: 0x0007573C
	public virtual bool CanInteract(GameObject _interacter)
	{
		return base.enabled && this.m_interactable.enabled && !this.m_interactionSuppressed && !this.m_canInteractCallbacks.CallForResult(false, _interacter) && (this.m_interactable.m_allowMultipleInteracters || this.m_interacters.Count == 0 || this.m_interacters.Contains(_interacter));
	}

	// Token: 0x060016E4 RID: 5860 RVA: 0x000773B3 File Offset: 0x000757B3
	public void SetInteractionSuppressed(bool _suppressed)
	{
		this.m_interactionSuppressed = _suppressed;
	}

	// Token: 0x060016E5 RID: 5861 RVA: 0x000773BC File Offset: 0x000757BC
	public void RegisterCallbacks(ServerInteractable.BeginInteractCallback _addedInteractor, ServerInteractable.EndInteractCallback _removedInteractor)
	{
		this.m_addedInteracter = (ServerInteractable.BeginInteractCallback)Delegate.Combine(this.m_addedInteracter, _addedInteractor);
		this.m_removedInteracter = (ServerInteractable.EndInteractCallback)Delegate.Combine(this.m_removedInteracter, _removedInteractor);
	}

	// Token: 0x060016E6 RID: 5862 RVA: 0x000773EC File Offset: 0x000757EC
	public void UnregisterCallbacks(ServerInteractable.BeginInteractCallback _addedInteractor, ServerInteractable.EndInteractCallback _removedInteractor)
	{
		this.m_addedInteracter = (ServerInteractable.BeginInteractCallback)Delegate.Remove(this.m_addedInteracter, _addedInteractor);
		this.m_removedInteracter = (ServerInteractable.EndInteractCallback)Delegate.Remove(this.m_removedInteracter, _removedInteractor);
	}

	// Token: 0x060016E7 RID: 5863 RVA: 0x0007741C File Offset: 0x0007581C
	public void RegisterCanInteractCallbacks(Generic<bool, GameObject> _trigger)
	{
		this.m_canInteractCallbacks.Add(_trigger);
	}

	// Token: 0x060016E8 RID: 5864 RVA: 0x0007742A File Offset: 0x0007582A
	public void UnregisterCanInteractCallbacks(Generic<bool, GameObject> _trigger)
	{
		this.m_canInteractCallbacks.Remove(_trigger);
	}

	// Token: 0x060016E9 RID: 5865 RVA: 0x00077439 File Offset: 0x00075839
	public void RegisterTriggerCallbacks(ServerInteractable.BeginInteractCallback _trigger)
	{
		this.m_triggerCallbacks = (ServerInteractable.BeginInteractCallback)Delegate.Combine(this.m_triggerCallbacks, _trigger);
	}

	// Token: 0x060016EA RID: 5866 RVA: 0x00077452 File Offset: 0x00075852
	public void UnregisterTriggerCallbacks(ServerInteractable.BeginInteractCallback _trigger)
	{
		this.m_triggerCallbacks = (ServerInteractable.BeginInteractCallback)Delegate.Remove(this.m_triggerCallbacks, _trigger);
	}

	// Token: 0x060016EB RID: 5867 RVA: 0x0007746B File Offset: 0x0007586B
	public void TriggerInteract(GameObject _interacter, Vector2 _directionXZ)
	{
		this.m_triggerCallbacks(_interacter, _directionXZ);
		if (this.m_interactable.m_onInteractImpulseTrigger != string.Empty)
		{
			base.gameObject.SendTrigger(this.m_interactable.m_onInteractImpulseTrigger);
		}
	}

	// Token: 0x060016EC RID: 5868 RVA: 0x000774AC File Offset: 0x000758AC
	public void BeginInteract(GameObject _interacter, Vector2 _directionXZ)
	{
		if (this.m_interacters.Count == 0 && this.m_interactable.m_onInteractStartedTrigger != string.Empty)
		{
			base.gameObject.SendTrigger(this.m_interactable.m_onInteractStartedTrigger);
		}
		this.m_interacters.Add(_interacter);
		this.m_addedInteracter(_interacter, _directionXZ);
	}

	// Token: 0x060016ED RID: 5869 RVA: 0x00077514 File Offset: 0x00075914
	public void EndInteract(GameObject _interacter)
	{
		if (this.m_interacters.Count <= 1 && this.m_interactable.m_onInteractEndedTrigger != string.Empty)
		{
			base.gameObject.SendTrigger(this.m_interactable.m_onInteractEndedTrigger);
		}
		this.m_interacters.Remove(_interacter);
		this.m_removedInteracter(_interacter);
	}

	// Token: 0x060016EE RID: 5870 RVA: 0x0007757B File Offset: 0x0007597B
	public virtual bool InteractionIsSticky()
	{
		return this.m_stickyInteractionCallback != null && this.m_stickyInteractionCallback();
	}

	// Token: 0x060016EF RID: 5871 RVA: 0x00077596 File Offset: 0x00075996
	public void SetStickyInteractionCallback(Generic<bool> _callback)
	{
		this.m_stickyInteractionCallback = _callback;
	}

	// Token: 0x060016F0 RID: 5872 RVA: 0x0007759F File Offset: 0x0007599F
	public bool IsBeingInteractedWith()
	{
		return this.m_interacters.Count > 0;
	}

	// Token: 0x060016F1 RID: 5873 RVA: 0x000775AF File Offset: 0x000759AF
	public bool AllowMultipleInteractors()
	{
		return this.m_interactable.m_allowMultipleInteracters;
	}

	// Token: 0x060016F2 RID: 5874 RVA: 0x000775BC File Offset: 0x000759BC
	public int InteractorCount()
	{
		return this.m_interacters.Count;
	}

	// Token: 0x04001107 RID: 4359
	private Interactable m_interactable;

	// Token: 0x04001108 RID: 4360
	private List<GameObject> m_interacters = new List<GameObject>();

	// Token: 0x04001109 RID: 4361
	private ServerInteractable.BeginInteractCallback m_addedInteracter = delegate(GameObject _interacter, Vector2 _directionXZ)
	{
	};

	// Token: 0x0400110A RID: 4362
	private ServerInteractable.BeginInteractCallback m_triggerCallbacks = delegate(GameObject _interacter, Vector2 _directionXZ)
	{
	};

	// Token: 0x0400110B RID: 4363
	private List<Generic<bool, GameObject>> m_canInteractCallbacks = new List<Generic<bool, GameObject>>();

	// Token: 0x0400110C RID: 4364
	private ServerInteractable.EndInteractCallback m_removedInteracter = delegate(GameObject _interacter)
	{
	};

	// Token: 0x0400110D RID: 4365
	private bool m_interactionSuppressed;

	// Token: 0x0400110E RID: 4366
	private Generic<bool> m_stickyInteractionCallback;

	// Token: 0x020004D2 RID: 1234
	// (Invoke) Token: 0x060016F7 RID: 5879
	public delegate void BeginInteractCallback(GameObject _interacter, Vector2 _directionXZ);

	// Token: 0x020004D3 RID: 1235
	// (Invoke) Token: 0x060016FB RID: 5883
	public delegate void EndInteractCallback(GameObject _interacter);
}
