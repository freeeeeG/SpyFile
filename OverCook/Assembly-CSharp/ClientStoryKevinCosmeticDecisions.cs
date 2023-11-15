using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003F1 RID: 1009
public class ClientStoryKevinCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x0600127F RID: 4735 RVA: 0x000683EE File Offset: 0x000667EE
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cosmeticDecisions = (StoryKevinCosmeticDecisions)synchronisedObject;
		this.m_interactable = base.gameObject.RequireComponent<ClientInteractable>();
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x00068414 File Offset: 0x00066814
	private void Update()
	{
		if (this.m_cosmeticDecisions != null && this.m_cosmeticDecisions.m_animator != null)
		{
			bool value = this.m_interactable != null && this.m_interactable.InteractorCount() > 0;
			this.m_cosmeticDecisions.m_animator.SetBool(ClientStoryKevinCosmeticDecisions.m_PettingAnimHash, value);
		}
	}

	// Token: 0x04000E7C RID: 3708
	private StoryKevinCosmeticDecisions m_cosmeticDecisions;

	// Token: 0x04000E7D RID: 3709
	private ClientInteractable m_interactable;

	// Token: 0x04000E7E RID: 3710
	private static readonly int m_PettingAnimHash = Animator.StringToHash("IsPetted");
}
