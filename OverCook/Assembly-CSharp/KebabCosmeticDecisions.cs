using System;
using UnityEngine;

// Token: 0x020003CA RID: 970
public class KebabCosmeticDecisions : MealCosmeticDecisions
{
	// Token: 0x060011FC RID: 4604 RVA: 0x00066285 File Offset: 0x00064685
	protected override void Start()
	{
		if (this.m_prefabLookup != null)
		{
			this.m_prefabLookup.CacheAssembledOrderNodes();
		}
		base.Start();
	}

	// Token: 0x060011FD RID: 4605 RVA: 0x000662A9 File Offset: 0x000646A9
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterface<IClientOrderDefinition>();
	}

	// Token: 0x060011FE RID: 4606 RVA: 0x000662B6 File Offset: 0x000646B6
	protected override void UpdateAppearance(AssembledDefinitionNode _contents)
	{
		this.DestroyContents();
		if (_contents != null)
		{
			this.CreateContents(_contents);
		}
	}

	// Token: 0x060011FF RID: 4607 RVA: 0x000662CC File Offset: 0x000646CC
	private void CreateContents(AssembledDefinitionNode _contents)
	{
		GameObject prefabForNode = this.m_prefabLookup.GetPrefabForNode(_contents);
		if (prefabForNode != null)
		{
			prefabForNode.InstantiateOnParent(this.m_container.transform, true);
		}
	}

	// Token: 0x06001200 RID: 4608 RVA: 0x00066308 File Offset: 0x00064708
	private void DestroyContents()
	{
		int childCount = this.m_container.transform.childCount;
		for (int i = childCount - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.m_container.transform.GetChild(i).gameObject);
		}
	}

	// Token: 0x04000E04 RID: 3588
	[SerializeField]
	private OrderToPrefabLookup m_prefabLookup;
}
