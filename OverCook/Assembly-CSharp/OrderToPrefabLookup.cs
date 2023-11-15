using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200080C RID: 2060
[Serializable]
public class OrderToPrefabLookup : ScriptableObject
{
	// Token: 0x06002764 RID: 10084 RVA: 0x000B9580 File Offset: 0x000B7980
	public void CacheAssembledOrderNodes()
	{
		if (this.m_cachedAssembledOrderNodes)
		{
			return;
		}
		this.m_cachedAssembledOrderNodes = true;
		for (int i = 0; i < this.m_lookupArray.Length; i++)
		{
			this.m_lookupArray[i].m_simplifiedAssembledContent = this.m_lookupArray[i].m_content.Simpilfy();
		}
	}

	// Token: 0x06002765 RID: 10085 RVA: 0x000B95D8 File Offset: 0x000B79D8
	public GameObject GetPrefabForNode(AssembledDefinitionNode _node)
	{
		if (!this.m_cachedAssembledOrderNodes)
		{
			this.CacheAssembledOrderNodes();
		}
		AssembledDefinitionNode simpleNode = _node.Simpilfy();
		for (int i = 0; i < this.m_lookupArray.Length; i++)
		{
			if (AssembledDefinitionNode.MatchingAlreadySimple(simpleNode, this.m_lookupArray[i].m_simplifiedAssembledContent))
			{
				return this.m_lookupArray[i].m_prefab;
			}
		}
		return null;
	}

	// Token: 0x06002766 RID: 10086 RVA: 0x000B9640 File Offset: 0x000B7A40
	public List<OrderContentRestriction> GetContentRestrictions()
	{
		List<OrderContentRestriction> list = new List<OrderContentRestriction>();
		for (int i = 0; i < this.m_lookupArray.Length; i++)
		{
			list.Add(new OrderContentRestriction
			{
				m_content = this.m_lookupArray[i].m_content,
				m_amountAllowed = this.m_lookupArray[i].m_amountAllowed,
				m_restrictedContent = this.m_lookupArray[i].m_restrictedContent
			});
		}
		return list;
	}

	// Token: 0x04001EEE RID: 7918
	[SerializeField]
	private OrderToPrefabLookup.ContentPrefabLookup[] m_lookupArray = new OrderToPrefabLookup.ContentPrefabLookup[0];

	// Token: 0x04001EEF RID: 7919
	[NonSerialized]
	private bool m_cachedAssembledOrderNodes;

	// Token: 0x0200080D RID: 2061
	[Serializable]
	public class ContentPrefabLookup
	{
		// Token: 0x04001EF0 RID: 7920
		public OrderDefinitionNode m_content;

		// Token: 0x04001EF1 RID: 7921
		public GameObject m_prefab;

		// Token: 0x04001EF2 RID: 7922
		public int m_amountAllowed = 1;

		// Token: 0x04001EF3 RID: 7923
		public OrderDefinitionNode[] m_restrictedContent = new OrderDefinitionNode[0];

		// Token: 0x04001EF4 RID: 7924
		[NonSerialized]
		public AssembledDefinitionNode m_simplifiedAssembledContent;
	}
}
