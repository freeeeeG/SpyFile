using System;
using UnityEngine;

// Token: 0x020003BA RID: 954
public class FryingPanCosmeticDecisions : OverlapModelsMealDecisions
{
	// Token: 0x060011C3 RID: 4547 RVA: 0x00065561 File Offset: 0x00063961
	protected override IClientOrderDefinition FindOrderDefinition()
	{
		return base.gameObject.RequestInterfaceUpwardsRecursive<IClientOrderDefinition>();
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x00065570 File Offset: 0x00063970
	protected override void CreateRenderChildren(AssembledDefinitionNode _contents)
	{
		base.CreateRenderChildren(_contents);
		if (_contents.Simpilfy() != AssembledDefinitionNode.NullNode && this.m_container.transform.childCount == 0 && this.m_burntPrefab != null)
		{
			GameObject gameObject = this.m_burntPrefab.InstantiateOnParent(this.m_container.transform, true);
			gameObject.transform.localPosition = this.m_burntPrefabOffset;
		}
	}

	// Token: 0x04000DD6 RID: 3542
	[SerializeField]
	public GameObject m_burntPrefab;

	// Token: 0x04000DD7 RID: 3543
	[SerializeField]
	public Vector3 m_burntPrefabOffset;
}
