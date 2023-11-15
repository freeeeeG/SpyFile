using System;
using System.Collections.Generic;

// Token: 0x020006CB RID: 1739
public class Chatty : KMonoBehaviour, ISimEveryTick
{
	// Token: 0x06002F4A RID: 12106 RVA: 0x000F98DA File Offset: 0x000F7ADA
	protected override void OnPrefabInit()
	{
		base.GetComponent<KPrefabID>().AddTag(GameTags.AlwaysConverse, false);
		base.Subscribe(-594200555, new Action<object>(this.OnStartedTalking));
		this.identity = base.GetComponent<MinionIdentity>();
	}

	// Token: 0x06002F4B RID: 12107 RVA: 0x000F9914 File Offset: 0x000F7B14
	private void OnStartedTalking(object data)
	{
		MinionIdentity minionIdentity = data as MinionIdentity;
		if (minionIdentity == null)
		{
			return;
		}
		this.conversationPartners.Add(minionIdentity);
	}

	// Token: 0x06002F4C RID: 12108 RVA: 0x000F9940 File Offset: 0x000F7B40
	public void SimEveryTick(float dt)
	{
		if (this.conversationPartners.Count == 0)
		{
			return;
		}
		for (int i = this.conversationPartners.Count - 1; i >= 0; i--)
		{
			MinionIdentity minionIdentity = this.conversationPartners[i];
			this.conversationPartners.RemoveAt(i);
			if (!(minionIdentity == this.identity))
			{
				minionIdentity.AddTag(GameTags.PleasantConversation);
			}
		}
		base.gameObject.AddTag(GameTags.PleasantConversation);
	}

	// Token: 0x04001C1F RID: 7199
	private MinionIdentity identity;

	// Token: 0x04001C20 RID: 7200
	private List<MinionIdentity> conversationPartners = new List<MinionIdentity>();
}
