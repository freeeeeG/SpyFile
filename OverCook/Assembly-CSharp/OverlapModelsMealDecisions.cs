using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003DF RID: 991
public class OverlapModelsMealDecisions : MealCosmeticDecisions
{
	// Token: 0x0600124A RID: 4682 RVA: 0x000623F4 File Offset: 0x000607F4
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequireInterface<IClientOrderDefinition>();
	}

	// Token: 0x0600124B RID: 4683 RVA: 0x00062401 File Offset: 0x00060801
	protected override void Start()
	{
		if (this.m_prefabLookup != null)
		{
			this.m_prefabLookup.CacheAssembledOrderNodes();
		}
		base.Start();
	}

	// Token: 0x0600124C RID: 4684 RVA: 0x00062425 File Offset: 0x00060825
	protected override void UpdateAppearance(AssembledDefinitionNode _contents)
	{
		this.DestroyAllRenderingInstances();
		if (_contents != null)
		{
			this.CreateRenderChildren(_contents);
		}
	}

	// Token: 0x0600124D RID: 4685 RVA: 0x0006243C File Offset: 0x0006083C
	protected virtual void CreateRenderChildren(AssembledDefinitionNode _contents)
	{
		IEnumerator<AssembledDefinitionNode> enumerator = this.ShallowIteration(_contents);
		while (enumerator.MoveNext())
		{
			AssembledDefinitionNode order = enumerator.Current;
			GameObject renderPefabForNode = this.GetRenderPefabForNode(order);
			if (renderPefabForNode != null)
			{
				GameObject gameObject = renderPefabForNode.InstantiateOnParent(this.m_container.transform, true);
			}
		}
	}

	// Token: 0x0600124E RID: 4686 RVA: 0x00062490 File Offset: 0x00060890
	protected IEnumerator<AssembledDefinitionNode> ShallowIteration(AssembledDefinitionNode _contents)
	{
		if (_contents is CompositeAssembledNode)
		{
			AssembledDefinitionNode[] contents = (_contents as CompositeAssembledNode).m_composition;
			for (int i = 0; i < contents.Length; i++)
			{
				if (_contents is CookedCompositeAssembledNode)
				{
					CookedCompositeAssembledNode cookedNode = _contents as CookedCompositeAssembledNode;
					CookedCompositeAssembledNode cookedWrapper = new CookedCompositeAssembledNode();
					cookedWrapper.ReplaceData(cookedNode);
					cookedWrapper.m_composition = new AssembledDefinitionNode[]
					{
						contents[i]
					};
					yield return cookedWrapper;
				}
				else
				{
					yield return contents[i];
				}
			}
		}
		else
		{
			yield return _contents;
		}
		yield break;
	}

	// Token: 0x0600124F RID: 4687 RVA: 0x000624AC File Offset: 0x000608AC
	protected virtual void DestroyAllRenderingInstances()
	{
		for (int i = this.m_container.transform.childCount - 1; i >= 0; i--)
		{
			Transform child = this.m_container.transform.GetChild(i);
			child.transform.SetParent(null);
			UnityEngine.Object.Destroy(child.gameObject);
		}
	}

	// Token: 0x06001250 RID: 4688 RVA: 0x00062505 File Offset: 0x00060905
	protected virtual GameObject GetRenderPefabForNode(AssembledDefinitionNode _order)
	{
		return this.m_prefabLookup.GetPrefabForNode(_order);
	}

	// Token: 0x04000E4D RID: 3661
	[SerializeField]
	private OrderToPrefabLookup m_prefabLookup;
}
