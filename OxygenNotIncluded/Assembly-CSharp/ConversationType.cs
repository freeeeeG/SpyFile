using System;
using UnityEngine;

// Token: 0x020006FE RID: 1790
public class ConversationType
{
	// Token: 0x06003139 RID: 12601 RVA: 0x00105A69 File Offset: 0x00103C69
	public virtual void NewTarget(MinionIdentity speaker)
	{
	}

	// Token: 0x0600313A RID: 12602 RVA: 0x00105A6B File Offset: 0x00103C6B
	public virtual Conversation.Topic GetNextTopic(MinionIdentity speaker, Conversation.Topic lastTopic)
	{
		return null;
	}

	// Token: 0x0600313B RID: 12603 RVA: 0x00105A6E File Offset: 0x00103C6E
	public virtual Sprite GetSprite(string topic)
	{
		return null;
	}

	// Token: 0x04001D97 RID: 7575
	public string id;

	// Token: 0x04001D98 RID: 7576
	public string target;
}
