using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020004D4 RID: 1236
public class ClientInteractable : ClientSynchroniserBase
{
	// Token: 0x060016FF RID: 5887 RVA: 0x000775E2 File Offset: 0x000759E2
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_interactable = (Interactable)synchronisedObject;
	}

	// Token: 0x06001700 RID: 5888 RVA: 0x000775F7 File Offset: 0x000759F7
	public virtual bool InteractionIsSticky()
	{
		return this.m_stickyInteractionCallback == null || this.m_stickyInteractionCallback();
	}

	// Token: 0x06001701 RID: 5889 RVA: 0x00077612 File Offset: 0x00075A12
	public void SetStickyInteractionCallback(Generic<bool> _callback)
	{
		this.m_stickyInteractionCallback = _callback;
	}

	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06001702 RID: 5890 RVA: 0x0007761B File Offset: 0x00075A1B
	public bool UsePlacementButton
	{
		get
		{
			return this.m_interactable.m_usePlacementButton;
		}
	}

	// Token: 0x06001703 RID: 5891 RVA: 0x00077628 File Offset: 0x00075A28
	public virtual bool CanInteract(GameObject _interacter)
	{
		return (this.m_interacters.Count == 0 || this.m_interactable.m_allowMultipleInteracters || this.m_interacters.Contains(_interacter)) && !this.m_interactionSuppressed && base.enabled && this.m_interactable.enabled;
	}

	// Token: 0x06001704 RID: 5892 RVA: 0x0007768A File Offset: 0x00075A8A
	public void SetInteractionSuppressed(bool _suppressed)
	{
		this.m_interactionSuppressed = _suppressed;
	}

	// Token: 0x06001705 RID: 5893 RVA: 0x00077693 File Offset: 0x00075A93
	public void AddInteractor(GameObject _interactor)
	{
		this.m_interacters.Add(_interactor);
	}

	// Token: 0x06001706 RID: 5894 RVA: 0x000776A1 File Offset: 0x00075AA1
	public void RemoveInteractor(GameObject _interactor)
	{
		this.m_interacters.Remove(_interactor);
	}

	// Token: 0x06001707 RID: 5895 RVA: 0x000776B0 File Offset: 0x00075AB0
	public bool AllowMultipleInteractors()
	{
		return this.m_interactable.m_allowMultipleInteracters;
	}

	// Token: 0x06001708 RID: 5896 RVA: 0x000776BD File Offset: 0x00075ABD
	public int InteractorCount()
	{
		return this.m_interacters.Count;
	}

	// Token: 0x04001112 RID: 4370
	private Generic<bool> m_stickyInteractionCallback;

	// Token: 0x04001113 RID: 4371
	private Interactable m_interactable;

	// Token: 0x04001114 RID: 4372
	private bool m_interactionSuppressed;

	// Token: 0x04001115 RID: 4373
	private List<GameObject> m_interacters = new List<GameObject>();
}
