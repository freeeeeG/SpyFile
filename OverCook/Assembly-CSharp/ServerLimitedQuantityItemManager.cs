using System;
using System.Collections.Generic;
using Team17.Online.Multiplayer.Messaging;

// Token: 0x0200050D RID: 1293
public class ServerLimitedQuantityItemManager : ServerSynchroniserBase
{
	// Token: 0x06001824 RID: 6180 RVA: 0x0007ABE5 File Offset: 0x00078FE5
	public void Awake()
	{
		this.m_BaseObject = base.GetComponent<LimitedQuantityItemManager>();
		this.m_AllObjects = new FastList<ServerLimitedQuantityItem>(this.m_BaseObject.m_MaxObjects);
	}

	// Token: 0x06001825 RID: 6181 RVA: 0x0007AC0C File Offset: 0x0007900C
	public void AddItemToList(ServerLimitedQuantityItem item)
	{
		int num = this.m_AllObjects.Count - this.m_BaseObject.m_MaxObjects + 1;
		for (int i = 0; i < num; i++)
		{
			ServerLimitedQuantityItem serverLimitedQuantityItem = null;
			float num2 = float.MinValue;
			for (int j = 0; j < this.m_AllObjects.Count; j++)
			{
				ServerLimitedQuantityItem serverLimitedQuantityItem2 = this.m_AllObjects._items[j];
				if (!serverLimitedQuantityItem2.IsInvincible())
				{
					float destructionScore = serverLimitedQuantityItem2.GetDestructionScore();
					if (destructionScore > num2)
					{
						serverLimitedQuantityItem = serverLimitedQuantityItem2;
						num2 = destructionScore;
					}
				}
			}
			if (!(null != serverLimitedQuantityItem))
			{
				break;
			}
			NetworkUtils.DestroyObject(serverLimitedQuantityItem.gameObject);
			this.m_AllObjects.Remove(serverLimitedQuantityItem);
		}
		this.m_AllObjects.Add(item);
	}

	// Token: 0x06001826 RID: 6182 RVA: 0x0007ACDA File Offset: 0x000790DA
	public void RemoveItemFromList(ServerLimitedQuantityItem item)
	{
		this.m_AllObjects.Remove(item);
	}

	// Token: 0x06001827 RID: 6183 RVA: 0x0007ACE9 File Offset: 0x000790E9
	public override void OnDestroy()
	{
		this.m_AllObjects.Clear();
	}

	// Token: 0x04001372 RID: 4978
	private LimitedQuantityItemManager m_BaseObject;

	// Token: 0x04001373 RID: 4979
	private FastList<ServerLimitedQuantityItem> m_AllObjects;
}
