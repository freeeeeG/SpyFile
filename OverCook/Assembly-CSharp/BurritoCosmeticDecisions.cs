using System;
using UnityEngine;

// Token: 0x02000389 RID: 905
public class BurritoCosmeticDecisions : OverlapModelsMealDecisions
{
	// Token: 0x06001121 RID: 4385 RVA: 0x000626AD File Offset: 0x00060AAD
	protected override void Start()
	{
		base.Start();
		this.m_container.transform.localRotation = Quaternion.AngleAxis(90f, Vector3.up);
	}

	// Token: 0x06001122 RID: 4386 RVA: 0x000626D4 File Offset: 0x00060AD4
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x06001123 RID: 4387 RVA: 0x000626E4 File Offset: 0x00060AE4
	private bool HasContents(CompositeAssembledNode _composite)
	{
		if (_composite == null)
		{
			return false;
		}
		foreach (AssembledDefinitionNode node in _composite.m_composition)
		{
			if (!AssembledDefinitionNode.Matching(node, this.m_tortillaOrderDefinition))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001124 RID: 4388 RVA: 0x0006272C File Offset: 0x00060B2C
	protected override void DestroyAllRenderingInstances()
	{
		base.DestroyAllRenderingInstances();
		this.m_fullTortilla.SetActive(false);
		this.m_emptyTortilla.SetActive(false);
	}

	// Token: 0x06001125 RID: 4389 RVA: 0x0006274C File Offset: 0x00060B4C
	protected override GameObject GetRenderPefabForNode(AssembledDefinitionNode _order)
	{
		if (!AssembledDefinitionNode.Matching(_order, this.m_tortillaOrderDefinition))
		{
			return base.GetRenderPefabForNode(_order);
		}
		AssembledDefinitionNode orderComposition = this.m_iOrderDefinition.GetOrderComposition();
		bool flag = orderComposition != null && this.HasContents(orderComposition as CompositeAssembledNode);
		this.m_fullTortilla.SetActive(flag);
		this.m_emptyTortilla.SetActive(!flag);
		return null;
	}

	// Token: 0x04000D43 RID: 3395
	[SerializeField]
	private GameObject m_fullTortilla;

	// Token: 0x04000D44 RID: 3396
	[SerializeField]
	private GameObject m_emptyTortilla;

	// Token: 0x04000D45 RID: 3397
	[SerializeField]
	private OrderDefinitionNode m_tortillaOrderDefinition;
}
