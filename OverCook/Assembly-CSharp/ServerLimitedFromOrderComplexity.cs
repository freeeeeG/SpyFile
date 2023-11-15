using System;
using Team17.Online.Multiplayer.Messaging;
using UnityEngine;

// Token: 0x02000805 RID: 2053
public class ServerLimitedFromOrderComplexity : ServerSynchroniserBase
{
	// Token: 0x06002743 RID: 10051 RVA: 0x000B9260 File Offset: 0x000B7660
	public override void StartSynchronising(Component synchronisedObject)
	{
		base.StartSynchronising(synchronisedObject);
		this.m_limitedQuantityItemManager = GameUtils.RequireManager<LimitedQuantityItemManager>();
		this.m_iOrderDefinition = base.gameObject.RequestInterface<IOrderDefinition>();
		if (this.m_iOrderDefinition != null)
		{
			ServerLimitedQuantityItem serverLimitedQuantityItem = base.gameObject.RequireComponent<ServerLimitedQuantityItem>();
			serverLimitedQuantityItem.AddDestructionScoreModifier(new Generic<float>(this.GetOrderComplexity));
		}
	}

	// Token: 0x06002744 RID: 10052 RVA: 0x000B92BC File Offset: 0x000B76BC
	private float GetOrderComplexity()
	{
		AssembledDefinitionNode orderComposition = this.m_iOrderDefinition.GetOrderComposition();
		if (orderComposition == null)
		{
			return 0f;
		}
		AssembledDefinitionNode assembledDefinitionNode = orderComposition.Simpilfy();
		if (orderComposition == AssembledDefinitionNode.NullNode)
		{
			return 0f;
		}
		return this.m_limitedQuantityItemManager.m_OrderComplexityMultiplier * (float)assembledDefinitionNode.GetNodeCount();
	}

	// Token: 0x04001EE0 RID: 7904
	private IOrderDefinition m_iOrderDefinition;

	// Token: 0x04001EE1 RID: 7905
	private LimitedQuantityItemManager m_limitedQuantityItemManager;
}
