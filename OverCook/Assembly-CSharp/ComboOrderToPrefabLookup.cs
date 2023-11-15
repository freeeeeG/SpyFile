using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007EF RID: 2031
[Serializable]
public class ComboOrderToPrefabLookup : ScriptableObject
{
	// Token: 0x06002704 RID: 9988 RVA: 0x000B8CB8 File Offset: 0x000B70B8
	public void CacheAssembledOrderNodes()
	{
		if (this.m_cachedAssembledOrderNodes)
		{
			return;
		}
		this.m_cachedAssembledOrderNodes = true;
		for (int i = 0; i < this.m_lookupArray.Length; i++)
		{
			this.m_lookupArray[i].CacheAssembledOrderNodes();
		}
		for (int j = 0; j < this.m_fallbackArray.Length; j++)
		{
			this.m_fallbackArray[j].CacheAssembledOrderNodes();
		}
	}

	// Token: 0x06002705 RID: 9989 RVA: 0x000B8D24 File Offset: 0x000B7124
	public List<OrderContentRestriction> GetContentRestrictions()
	{
		if (this.m_orderContentRestriction == null)
		{
			this.m_orderContentRestriction = new List<OrderContentRestriction>();
			for (int i = 0; i < this.m_lookupArray.Length; i++)
			{
				OrderDefinitionNode[] content = this.m_lookupArray[i].m_content;
				Dictionary<OrderDefinitionNode, int> dictionary = new Dictionary<OrderDefinitionNode, int>();
				for (int j = 0; j < content.Length; j++)
				{
					if (!dictionary.ContainsKey(content[j]))
					{
						dictionary.Add(content[j], 1);
					}
					else
					{
						Dictionary<OrderDefinitionNode, int> dictionary2;
						OrderDefinitionNode key;
						(dictionary2 = dictionary)[key = content[j]] = dictionary2[key] + 1;
					}
				}
				using (Dictionary<OrderDefinitionNode, int>.Enumerator enumerator = dictionary.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<OrderDefinitionNode, int> nodeCountPair = enumerator.Current;
						OrderContentRestriction orderContentRestriction = this.m_orderContentRestriction.Find((OrderContentRestriction x) => x.m_content == nodeCountPair.Key);
						if (orderContentRestriction != null)
						{
							orderContentRestriction.m_amountAllowed = Mathf.Max(orderContentRestriction.m_amountAllowed, nodeCountPair.Value);
						}
						else
						{
							orderContentRestriction = new OrderContentRestriction();
							orderContentRestriction.m_content = nodeCountPair.Key;
							orderContentRestriction.m_amountAllowed = nodeCountPair.Value;
							this.m_orderContentRestriction.Add(orderContentRestriction);
						}
					}
				}
			}
		}
		return this.m_orderContentRestriction;
	}

	// Token: 0x06002706 RID: 9990 RVA: 0x000B8E98 File Offset: 0x000B7298
	public GameObject GetPrefabForNode(AssembledDefinitionNode _node)
	{
		if (!this.m_cachedAssembledOrderNodes)
		{
			this.CacheAssembledOrderNodes();
		}
		if (_node != null)
		{
			CookedCompositeAssembledNode cookedCompositeAssembledNode = _node as CookedCompositeAssembledNode;
			if (cookedCompositeAssembledNode != null)
			{
				AssembledDefinitionNode[] array = new AssembledDefinitionNode[cookedCompositeAssembledNode.m_composition.Length];
				for (int i = 0; i < cookedCompositeAssembledNode.m_composition.Length; i++)
				{
					CookedCompositeAssembledNode cookedCompositeAssembledNode2 = new CookedCompositeAssembledNode();
					cookedCompositeAssembledNode2.ReplaceData(cookedCompositeAssembledNode);
					cookedCompositeAssembledNode2.m_composition = new AssembledDefinitionNode[]
					{
						cookedCompositeAssembledNode.m_composition[i]
					};
					array[i] = cookedCompositeAssembledNode2;
				}
				return this.FindPrefab(array);
			}
			CompositeAssembledNode compositeAssembledNode = _node as CompositeAssembledNode;
			if (compositeAssembledNode != null)
			{
				AssembledDefinitionNode[] array = compositeAssembledNode.m_composition;
				return this.FindPrefab(array);
			}
			IngredientAssembledNode ingredientAssembledNode = _node as IngredientAssembledNode;
			if (ingredientAssembledNode != null)
			{
				AssembledDefinitionNode[] array = new AssembledDefinitionNode[]
				{
					ingredientAssembledNode
				};
				return this.FindPrefab(array);
			}
		}
		return null;
	}

	// Token: 0x06002707 RID: 9991 RVA: 0x000B8F64 File Offset: 0x000B7364
	private GameObject FindPrefab(AssembledDefinitionNode[] ingredients)
	{
		int num = 0;
		for (int i = this.m_lookupArray.Length - 1; i >= 0; i--)
		{
			for (int j = ingredients.Length - 1; j >= 0; j--)
			{
				for (int k = this.m_lookupArray[i].m_content.Length - 1; k >= 0; k--)
				{
					AssembledDefinitionNode assembledContent = this.m_lookupArray[i].GetAssembledContent(k);
					if (AssembledDefinitionNode.Matching(ingredients[j], assembledContent))
					{
						num++;
					}
				}
			}
			if (num == ingredients.Length && num == this.m_lookupArray[i].m_content.Length && this.m_lookupArray[i].m_prefab != null)
			{
				return this.m_lookupArray[i].m_prefab;
			}
			num = 0;
		}
		int num2 = -1;
		for (int l = 0; l < this.m_fallbackArray.Length; l++)
		{
			if (this.m_fallbackArray[l].m_content != null && this.m_fallbackArray[l].m_prefab != null)
			{
				for (int m = 0; m < this.m_fallbackArray[l].m_content.Length; m++)
				{
					bool flag = false;
					for (int n = 0; n < ingredients.Length; n++)
					{
						AssembledDefinitionNode assembledContent2 = this.m_fallbackArray[l].GetAssembledContent(m);
						if (AssembledDefinitionNode.Matching(ingredients[n], assembledContent2))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					if (m == this.m_fallbackArray[l].m_content.Length - 1 && (num2 == -1 || this.m_fallbackArray[num2].m_content.Length < this.m_fallbackArray[l].m_content.Length))
					{
						num2 = l;
					}
				}
			}
		}
		return (num2 == -1) ? this.m_defaultFallback : this.m_fallbackArray[num2].m_prefab;
	}

	// Token: 0x04001ED0 RID: 7888
	[SerializeField]
	private ComboOrderToPrefabLookup.ContentPrefabLookup[] m_lookupArray = new ComboOrderToPrefabLookup.ContentPrefabLookup[0];

	// Token: 0x04001ED1 RID: 7889
	[SerializeField]
	private ComboOrderToPrefabLookup.ContentPrefabLookup[] m_fallbackArray = new ComboOrderToPrefabLookup.ContentPrefabLookup[0];

	// Token: 0x04001ED2 RID: 7890
	[SerializeField]
	private GameObject m_defaultFallback;

	// Token: 0x04001ED3 RID: 7891
	private List<OrderContentRestriction> m_orderContentRestriction;

	// Token: 0x04001ED4 RID: 7892
	[NonSerialized]
	private bool m_cachedAssembledOrderNodes;

	// Token: 0x020007F0 RID: 2032
	[Serializable]
	public class ContentPrefabLookup
	{
		// Token: 0x06002709 RID: 9993 RVA: 0x000B9164 File Offset: 0x000B7564
		public AssembledDefinitionNode GetAssembledContent(int index)
		{
			return this.m_assembledContent[index];
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000B9170 File Offset: 0x000B7570
		public void CacheAssembledOrderNodes()
		{
			this.m_assembledContent = new AssembledDefinitionNode[this.m_content.Length];
			for (int i = 0; i < this.m_content.Length; i++)
			{
				this.m_assembledContent[i] = this.m_content[i].Simpilfy();
			}
		}

		// Token: 0x04001ED5 RID: 7893
		public OrderDefinitionNode[] m_content;

		// Token: 0x04001ED6 RID: 7894
		public GameObject m_prefab;

		// Token: 0x04001ED7 RID: 7895
		[NonSerialized]
		private AssembledDefinitionNode[] m_assembledContent;
	}
}
