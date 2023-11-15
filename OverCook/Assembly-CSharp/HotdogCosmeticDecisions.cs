using System;
using UnityEngine;

// Token: 0x020003C6 RID: 966
public class HotdogCosmeticDecisions : MealCosmeticDecisions
{
	// Token: 0x060011F1 RID: 4593 RVA: 0x00065FB2 File Offset: 0x000643B2
	protected override void Start()
	{
		this.m_prefabLookup.CacheAssembledOrderNodes();
		base.Start();
	}

	// Token: 0x060011F2 RID: 4594 RVA: 0x00065FC5 File Offset: 0x000643C5
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x060011F3 RID: 4595 RVA: 0x00065FD4 File Offset: 0x000643D4
	protected override void UpdateAppearance(AssembledDefinitionNode _contents)
	{
		this.DestroyCurrentModels();
		if (AssembledDefinitionNode.Matching(this.m_emptyHotdogDefinition, _contents))
		{
			this.m_emptyBun.SetActive(true);
		}
		else
		{
			this.m_emptyBun.SetActive(false);
		}
		GameObject prefabForNode = this.m_prefabLookup.GetPrefabForNode(_contents);
		if (prefabForNode != null)
		{
			prefabForNode.InstantiateOnParent(this.m_container.transform, true);
		}
	}

	// Token: 0x060011F4 RID: 4596 RVA: 0x00066044 File Offset: 0x00064444
	private void DestroyCurrentModels()
	{
		for (int i = this.m_container.transform.childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.m_container.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x04000DFA RID: 3578
	[SerializeField]
	private GameObject m_emptyBun;

	// Token: 0x04000DFB RID: 3579
	[SerializeField]
	private OrderDefinitionNode m_emptyHotdogDefinition;

	// Token: 0x04000DFC RID: 3580
	[SerializeField]
	private OrderToPrefabLookup m_prefabLookup;
}
