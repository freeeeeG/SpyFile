using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x020009CC RID: 2508
public class ClientAttachmentCatcher : ClientSynchroniserBase, IClientHandleCatch
{
	// Token: 0x06003128 RID: 12584 RVA: 0x000E69E0 File Offset: 0x000E4DE0
	public override EntityType GetEntityType()
	{
		return EntityType.AttachCatcher;
	}

	// Token: 0x06003129 RID: 12585 RVA: 0x000E69E4 File Offset: 0x000E4DE4
	public override void ApplyServerEvent(Serialisable serialisable)
	{
		AttachmentCatcherMessage attachmentCatcherMessage = (AttachmentCatcherMessage)serialisable;
		this.m_trackedThrowable = attachmentCatcherMessage.m_object;
	}

	// Token: 0x0600312A RID: 12586 RVA: 0x000E6A04 File Offset: 0x000E4E04
	public GameObject GetTrackedThrowable()
	{
		return this.m_trackedThrowable;
	}

	// Token: 0x04002764 RID: 10084
	private GameObject m_trackedThrowable;
}
