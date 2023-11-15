using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020003F4 RID: 1012
public class ClientStoryOnionKingCosmeticDecisions : ClientSynchroniserBase
{
	// Token: 0x06001285 RID: 4741 RVA: 0x000684AA File Offset: 0x000668AA
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_cosmeticDecisions = (StoryOnionKingCosmeticDecisions)synchronisedObject;
		this.m_triggerDialogue = base.gameObject.RequireComponent<ClientTriggerDialogue>();
	}

	// Token: 0x06001286 RID: 4742 RVA: 0x000684D0 File Offset: 0x000668D0
	private void Update()
	{
		if (this.m_cosmeticDecisions != null && this.m_cosmeticDecisions.m_animator != null)
		{
			bool value = this.m_triggerDialogue != null && this.m_triggerDialogue.IsSpeaking();
			this.m_cosmeticDecisions.m_animator.SetBool(ClientStoryOnionKingCosmeticDecisions.m_Talking, value);
		}
	}

	// Token: 0x04000E80 RID: 3712
	private StoryOnionKingCosmeticDecisions m_cosmeticDecisions;

	// Token: 0x04000E81 RID: 3713
	private ClientTriggerDialogue m_triggerDialogue;

	// Token: 0x04000E82 RID: 3714
	private static readonly int m_Talking = Animator.StringToHash("IsTalking");
}
